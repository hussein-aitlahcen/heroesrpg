using Akka.Actor;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Protocol.Game.State;
using HeroesRpg.Protocol.Impl.Game.World.Server;
using HeroesRpg.Server.Game.Entity;
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
        public const int PTM_RATIO = 32;

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
        private Queue<WorldStateSnapshot> m_stateSnapshots;

        /// <summary>
        ///
        /// </summary>
        public MapInstance(int id)
        {
            m_stateSnapshots = new Queue<WorldStateSnapshot>();
            m_gameObjects = new Dictionary<int, GameObject>();
            m_physicsWorld = Context.ActorOf(PhysicsWorldInstance.Create(0, -22, PTM_RATIO), "physics-world");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.TickDone message)
        {
            // TODO: process client commands

            var snapShot = new WorldStateSnapshot(message.GameTime);
            foreach(var gameObject in m_gameObjects.Values)
            {
                var parts = gameObject.GetDirtyParts().ToList();
                if(parts.Count > 0)
                    snapShot.AddState(new GameObjectState(parts));
            }


            var netmsg = new WorldStateSnapshotMessage();
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    snapShot.ToNetwork(writer);
                }
                netmsg.WorldStateData = stream.ToArray();
            }

            foreach(var gameObject in m_gameObjects.Values)
            {
                if(gameObject.ControllerId != 0)
                {
                    var combat = (CombatEntity)gameObject;
                    combat.SetCurrentLife((int)Math.Floor(combat.CurrentLife + 1 * 1.1));
                    GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendUnreliable(gameObject.ControllerId, netmsg));
                }
            }


                        
            // tick the world            
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromMilliseconds(message.Delta), Sender, PhysicsWorldInstance.Tick.Instance, Self);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(AddEntity message)
        {
            if (m_gameObjects.ContainsKey(message.GameObj.Id))
            {
                Sender.Tell(new AddEntityResult(message.GameObj, AddEntityResultEnum.DUPLICATE_ID));
            }
            else
            {
                m_gameObjects.Add(message.GameObj.Id, message.GameObj);
                m_physicsWorld.Forward(new PhysicsWorldInstance.CreateEntityBody(message.GameObj));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(PhysicsWorldInstance.EntityBodyCreated message)
        {
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
