using Akka.Actor;
using Akka.Event;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Common;
using HeroesRpg.Common.Util;
using HeroesRpg.Protocol;
using HeroesRpg.Protocol.Enum;
using HeroesRpg.Protocol.Game.State;
using HeroesRpg.Protocol.Impl.Game.Map.Server;
using HeroesRpg.Protocol.Impl.Game.World.Server;
using HeroesRpg.Server.Game.Entity;
using HeroesRpg.Server.Game.Entity.Impl;
using HeroesRpg.Server.Network;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public enum AddEntityResultEnum
    {
        SUCCESS,
        DUPLICATE_ID,
        FAILURE,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum RemovEntityResultEnum
    {
        SUCCESS,
        FAILURE
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class MapInstance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="mapId"></param>
        /// <returns></returns>
        public static Props Create(int mapId) => Props.Create(typeof(MapInstance), mapId);

        /// <summary>
        /// 
        /// </summary>
        public sealed class MovementCommand : MapInstanceMessage
        {
            public sbyte X { get; }
            public sbyte Y { get; }
            public MovementCommand(GameObject obj, sbyte movementX, sbyte movementY) : base(obj)
            {
                X = movementX;
                Y = movementY;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class SpellUseCommand : MapInstanceMessage
        {
            public int SpellId { get; }
            public float Angle { get; }
            public SpellUseCommand(GameObject obj, int spellId, float angle) : base(obj)
            {
                SpellId = spellId;
                Angle = angle;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class JumpCommand : MapInstanceMessage
        {
            public JumpCommand(GameObject obj) : base(obj)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AddEntity : MapInstanceMessage
        {
            public AddEntity(GameObject obj) : base(obj)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class RemoveEntity : MapInstanceMessage
        {
            public RemoveEntity(GameObject obj) : base(obj)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AddEntityResult : MapInstanceMessage
        {
            public AddEntityResultEnum Code { get; private set; }
            public AddEntityResult(GameObject obj, AddEntityResultEnum resultCode) : base(obj)
            {
                Code = resultCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class RemoveEntityResult : MapInstanceMessage
        {
            public RemovEntityResultEnum Code { get; private set; }
            public RemoveEntityResult(GameObject obj, RemovEntityResultEnum resultCode) : base(obj)
            {
                Code = resultCode;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract class MapInstanceMessage
        {
            public GameObject GameObj { get; private set; }
            public MapInstanceMessage(GameObject obj)
            {
                GameObj = obj;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MapInstance : TypedActor,
        IHandle<MapInstance.AddEntity>,
        IHandle<MapInstance.RemoveEntity>,
        IHandle<PhysicsWorldInstance.EntityBodyCreated>,
        IHandle<PhysicsWorldInstance.EntityBodyDestroyed>,
        IHandle<PhysicsWorldInstance.TakeSnap>,
        IHandle<MapInstance.MovementCommand>,
        IHandle<MapInstance.SpellUseCommand>,
        IHandle<MapInstance.JumpCommand>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILoggingAdapter Logger = Logging.GetLogger(Context);

        /// <summary>
        /// 
        /// </summary>
        public const int SNAPSHOT_BUFFER = 10;
        
        /// <summary>
        /// 
        /// </summary>
        private IActorRef m_physicsWorld;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, GameObject> m_gameObjects;

        /// <summary>
        /// 
        /// </summary>
        private Queue<PrivateWorldStateSnapshot> m_stateSnapshots;

        /// <summary>
        /// 
        /// </summary>
        private Queue<Action> m_playerCommands;

        /// <summary>
        /// 
        /// </summary>
        private long m_snapAcumulator;

        /// <summary>
        /// 
        /// </summary>
        private Stopwatch m_snapshotTime;


        /// <summary>
        ///
        /// </summary>
        public MapInstance(int id)
        {
            m_playerCommands = new Queue<Action>();
            m_stateSnapshots = new Queue<PrivateWorldStateSnapshot>();
            m_gameObjects = new Dictionary<int, GameObject>();
            m_physicsWorld = Context.ActorOf(PhysicsWorldInstance.Create(), "physics-world");
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AroundPreStart()
        {
            m_snapshotTime = Stopwatch.StartNew();

            var ground = new Ground();
            ground.SetWidth(2000);
            ground.SetHeight(50);
            Self.Tell(new AddEntity(ground));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.TakeSnap message)
        {
            while (m_playerCommands.Count > 0)
                m_playerCommands.Dequeue()();

            TakeSnapshotIfRequired(message.PhysicUpdateSequence);
            
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromMilliseconds(0), Sender, PhysicsWorldInstance.Tick.Instance, Self);
        }

        /// <summary>
        /// 
        /// </summary>
        private void TakeSnapshotIfRequired(long physicUpdateSequence)
        {
            m_snapAcumulator += m_snapshotTime.ElapsedMilliseconds;
            m_snapshotTime.Restart();

            while (m_snapAcumulator > Constant.UPDATE_RATE_MS)
            {
                var privateSnap = new PrivateWorldStateSnapshot(physicUpdateSequence);
                var snapShot = new WorldStateSnapshot(physicUpdateSequence);
                foreach (var gameObject in m_gameObjects.Values)
                {
                    if (gameObject.IsRemovable)
                    {
                        Self.Tell(new RemoveEntity(gameObject));
                    }
                    else if (gameObject.CanNetworkOperation(GameObjectNetworkOperation.SHARE_UPDATE))
                    {
                        // delta only
                        var parts = gameObject.GetDirtyParts().ToList();
                        if (parts.Count > 0)
                            snapShot.AddState(new GameObjectState(gameObject.Id, parts));

                        // full position
                        privateSnap.AddObjectSnapshot(new GameObjectSnapshot(
                            gameObject.Id,
                            gameObject.PhysicsBody.Position.x,
                            gameObject.PhysicsBody.Position.y));
                    }
                }

                CleanOutdatedSnapshot(privateSnap);

                var netmsg = new WorldStateSnapshotMessage();
                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                        snapShot.ToNetwork(writer);
                    netmsg.WorldStateData = stream.ToArray();
                }
                BroadcastUnreliable(netmsg);

                m_snapAcumulator -= Constant.UPDATE_RATE_MS_LONG;
            }            
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void CleanOutdatedSnapshot(PrivateWorldStateSnapshot snapShot)
        {
            m_stateSnapshots.Enqueue(snapShot);
            if (m_stateSnapshots.Count > SNAPSHOT_BUFFER)
                m_stateSnapshots.Dequeue().Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(AddEntity message)
        {
            if (m_gameObjects.ContainsKey(message.GameObj.Id))            
                Sender.Tell(new AddEntityResult(message.GameObj, AddEntityResultEnum.DUPLICATE_ID));            
            else            
                m_physicsWorld.Forward(new PhysicsWorldInstance.CreateEntityBody(message.GameObj));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void BroadcastReliable(NetMessage message)
        {
            foreach(var obj in m_gameObjects.Values)            
                obj.SendReliable(message);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void BroadcastUnreliable(NetMessage message)
        {
            foreach (var obj in m_gameObjects.Values)            
                obj.SendUnreliable(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.EntityBodyDestroyed message)
        {
            message.GameObj.Map = null;

            m_gameObjects.Remove(message.GameObj.Id);

            BroadcastEntityDestroy(message.GameObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.EntityBodyCreated message)
        {
            message.GameObj.Map = Self;

            SendPhysicWorldData(message.GameObj.ControllerId);
            SendSpawnedEntities(message.GameObj.ControllerId);

            m_gameObjects.Add(message.GameObj.Id, message.GameObj);

            BroadcastEntitySpawn(message.GameObj);

            Sender.Tell(new AddEntityResult(message.GameObj, AddEntityResultEnum.SUCCESS));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void BroadcastEntitySpawn(GameObject obj)
        {
            if (obj.CanNetworkOperation(GameObjectNetworkOperation.SHARE_CREATION))
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                        obj.ToNetwork(writer);
                    BroadcastReliable(new EntitySpawMessage()
                    {
                        Type = obj.Type,
                        SubType = obj.SubType,
                        EntityData = stream.ToArray()
                    });
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        private void BroadcastEntityDestroy(GameObject obj)
        {
            if (obj.CanNetworkOperation(GameObjectNetworkOperation.SHARE_DELETION))
            {
                BroadcastReliable(new EntityDestroyMessage() { ObjectId = obj.Id });
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerId"></param>
        private void SendPhysicWorldData(ulong controllerId)
        {
            if (controllerId != 0)
            {
                GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendReliable(controllerId, new PhysicsWorldDataMessage()
                {
                    GravityX = PhysicsWorldInstance.GRAVITY_X,
                    GravityY = PhysicsWorldInstance.GRAVITY_Y,
                    PtmRatio = PhysicsWorldInstance.PTM_RATIO,
                    VelocityIte = PhysicsWorldInstance.WORLD_VELOCITY_ITE,
                    PositionIte = PhysicsWorldInstance.WORLD_POSITION_ITE
                }));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="controllerId"></param>
        private void SendSpawnedEntities(ulong controllerId)
        {
            if (controllerId > 0)
            {
                foreach (var gameObject in m_gameObjects.Values.Where(o => o.CanNetworkOperation(GameObjectNetworkOperation.SHARE_CREATION)))
                {
                    using (var stream = new MemoryStream())
                    {
                        using (var writer = new BinaryWriter(stream))
                            gameObject.ToNetwork(writer);
                        GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendReliable(controllerId, new EntitySpawMessage()
                        {
                            Type = gameObject.Type,
                            EntityData = stream.ToArray()
                        }));
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(RemoveEntity message)
        {
            if (!m_gameObjects.ContainsKey(message.GameObj.Id))
                Sender.Tell(new RemoveEntityResult(message.GameObj, RemovEntityResultEnum.FAILURE));
            else
                m_physicsWorld.Forward(new PhysicsWorldInstance.DestroyEntityBody(message.GameObj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(MovementCommand message)
        {
            m_playerCommands.Enqueue(() =>
            {
                var mv = message.GameObj as MovableEntity;
                mv.SetMovementSpeed(message.X * 10, message.Y * 10);
            });
        }

        int i;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(SpellUseCommand message)
        {
            var projectile = new Projectile();
            projectile.SetId(--i);
            projectile.SetWidth(50);
            projectile.SetHeight(50);
            projectile.SetBullet(true);
            projectile.SetGravityScale(0f);
            projectile.SetVelocity(4, 0);
            projectile.SetFixedRotation(true);
            projectile.SetWorldPosition(message.GameObj.WorldPositionX + 50f, message.GameObj.WorldPositionY + 50f);
            Self.Tell(new AddEntity(projectile));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(JumpCommand message)
        {
            //float impulse = body->GetMass() * 10;
            //body->ApplyLinearImpulse(b2Vec2(0, impulse), body->GetWorldCenter());
            m_playerCommands.Enqueue(() =>
            {
                var mv = message.GameObj as MovableEntity;
                var impulse = message.GameObj.Mass * 60;
                mv.ApplyLinearImpulseToCenter(0, impulse);
            });
        }
    }
}
