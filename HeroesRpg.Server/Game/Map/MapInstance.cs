using Akka.Actor;
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
        public sealed class AddEntity
        {
            public GameObject GameObj { get; private set; }
            public AddEntity(GameObject obj)
            {
                GameObj = obj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class RemoveEntity
        {
            public GameObject GameObj { get; private set; }
            public RemoveEntity(GameObject obj)
            {
                GameObj = obj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class AddEntityResult
        {
            public GameObject GameObj { get; private set; }
            public AddEntityResultEnum Code { get; private set; }

            public AddEntityResult(GameObject obj, AddEntityResultEnum resultCode)
            {
                GameObj = obj;
                Code = resultCode;
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
        IHandle<PhysicsWorldInstance.TickDone>
    {
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
        public MapInstance(int id)
        {
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
            // TODO: process client commands

            var privateSnap = new PrivateWorldStateSnapshot(message.GameTime);
            var snapShot = new WorldStateSnapshot(message.GameTime);
            foreach(var gameObject in m_gameObjects.Values)
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

            m_stateSnapshots.Enqueue(privateSnap);

            CleanOutdatedSnapshot();
            
            // broadcast only if 
            if (snapShot.States.Count > 0)
            {
                var netmsg = new WorldStateSnapshotMessage();
                using (var stream = new MemoryStream())
                {
                    using (var writer = new BinaryWriter(stream))
                        snapShot.ToNetwork(writer);
                    netmsg.WorldStateData = stream.ToArray();
                }
                BroadcastUnreliable(netmsg);
            }
                                    
            // tick the world            
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromMilliseconds(message.Delta), Sender, PhysicsWorldInstance.Tick.Instance, Self);
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void CleanOutdatedSnapshot()
        {
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
    }
}
