using Akka.Actor;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Server.Game.Entity;
using HeroesRpg.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Map
{
    public enum AddEntityResultEnum
    {
        SUCCESS,
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
        IHandle<MapInstance.RemoveEntity>
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
        public MapInstance(int id)
        {
            m_physicsWorld = Context.ActorOf(PhysicsWorldInstance.Create(0, -22, PTM_RATIO), "physics-world");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(AddEntity message)
        {
            m_physicsWorld
                .Ask<PhysicsWorldInstance.EntityBodyCreated>(new PhysicsWorldInstance.CreateEntityBody(message.GameObj))
                .ContinueWith((task) => new AddEntityResult(message.GameObj, AddEntityResultEnum.SUCCESS))
                .PipeTo(Sender);            
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
        private void AddGameObj()
        {

        }
    }
}
