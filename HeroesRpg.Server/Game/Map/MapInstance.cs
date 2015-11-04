using Akka.Actor;
using Akka.Event;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
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
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Map
{
    public enum AddEntityResultEnum
    {
        SUCCESS,
        DUPLICATE_ID,
        FAILURE,
    }

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
        IHandle<PhysicsWorldInstance.TickDone>,
        IHandle<MapInstance.MovementCommand>
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
        public const long SNAPSHOT_TICK = 1000 / 40;

        /// <summary>
        /// 
        /// </summary>
        public const long TICK = 10;

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
        private long m_nextSnapshot;
        
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
            var ground = new Ground(5000, 1);
            Self.Tell(new AddEntity(ground));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.TickDone message)
        {
            while (m_playerCommands.Count > 0)
                m_playerCommands.Dequeue()();

            // TODO: process client commands            
            TakeSnapshotIfRequired(message.GameTime);

            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromMilliseconds(10), Sender, PhysicsWorldInstance.Tick.Instance, Self);
            //Sender.Tell(PhysicsWorldInstance.Tick.Instance);   
        }

        /// <summary>
        /// 
        /// </summary>
        private void TakeSnapshotIfRequired(long gameTime)
        {
            if (m_nextSnapshot < gameTime)
            {
                m_nextSnapshot = gameTime + SNAPSHOT_TICK;

                var privateSnap = new PrivateWorldStateSnapshot(gameTime);
                var snapShot = new WorldStateSnapshot(gameTime);
                foreach (var gameObject in m_gameObjects.Values)
                {
                    gameObject.Update();

                    if (gameObject is MovableEntity)
                    {
                        var m = gameObject as MovableEntity;
                    }
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

                CleanOutdatedSnapshot(privateSnap);
                
                var netmsg = new WorldStateSnapshotMessage();
                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                        snapShot.ToNetwork(writer);
                    netmsg.WorldStateData = stream.ToArray();
                }
                BroadcastUnreliable(netmsg);
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
            foreach(var controller in m_gameObjects.Values.Select(obj => obj.ControllerId))            
                GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendReliable(controller, message));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        private void BroadcastUnreliable(NetMessage message)
        {
            foreach (var controller in m_gameObjects.Values.Select(obj => obj.ControllerId))            
                GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendUnreliable(controller, message));            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.EntityBodyCreated message)
        {
            message.GameObj.Map = Self;

            if (message.GameObj.ControllerId != 0)
            {
                GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendReliable(message.GameObj.ControllerId, new PhysicsWorldDataMessage()
                {
                    GravityX = PhysicsWorldInstance.GRAVITY_X,
                    GravityY = PhysicsWorldInstance.GRAVITY_Y,
                    PtmRatio = PhysicsWorldInstance.PTM_RATIO,
                    VelocityIte = PhysicsWorldInstance.WORLD_VELOCITY_ITE,
                    PositionIte = PhysicsWorldInstance.WORLD_POSITION_ITE
                }));
            }

            foreach(var gameObject in m_gameObjects.Values)
            {
                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                        gameObject.ToNetwork(writer);

                    GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendReliable(message.GameObj.ControllerId, new EntitySpawMessage()
                    {
                        Type = gameObject.Type,
                        EntityData = stream.ToArray()
                    }));
                }
            }

            m_gameObjects.Add(message.GameObj.Id, message.GameObj);
            
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                    message.GameObj.ToNetwork(writer);
                BroadcastReliable(new EntitySpawMessage()
                {
                    Type = message.GameObj.Type,
                    EntityData = stream.ToArray()
                });
            }
            Sender.Tell(new AddEntityResult(message.GameObj, AddEntityResultEnum.SUCCESS));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(RemoveEntity message)
        {

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
                Logger.Info("movement command : " + message.X);
            });
        }
    }
}
