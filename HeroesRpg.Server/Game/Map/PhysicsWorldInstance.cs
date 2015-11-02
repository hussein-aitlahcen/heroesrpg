using Akka.Actor;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Server.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    partial class PhysicsWorldInstance
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="gravityX"></param>
        /// <param name="gravityY"></param>
        /// <param name="ptm"></param>
        /// <returns></returns>
        public static Props Create(float gravityX, float gravityY, int ptm) => Props.Create(() => new PhysicsWorldInstance(gravityX, gravityY, ptm));

        /// <summary>
        /// 
        /// </summary>
        public sealed class CreateEntityBody : PhysicsWorldMessage
        {
            public CreateEntityBody(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class DestroyEntityBody : PhysicsWorldMessage
        {
            public DestroyEntityBody(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class EntityBodyCreated : PhysicsWorldMessage
        {
            public EntityBodyCreated(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class EntityBodyDestroyed : PhysicsWorldMessage
        {
            public EntityBodyDestroyed(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract class PhysicsWorldMessage
        {
            public GameObject GameObj
            {
                get;
                private set;
            }

            public PhysicsWorldMessage(GameObject obj)
            {
                GameObj = obj;
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class PhysicsWorldInstance : TypedActor,
        IHandle<PhysicsWorldInstance.CreateEntityBody>,
        IHandle<PhysicsWorldInstance.DestroyEntityBody>
    {
        /// <summary>
        /// 
        /// </summary>
        private b2World m_world;

        /// <summary>
        /// 
        /// </summary>
        private float m_gravityX, m_gravityY;

        /// <summary>
        /// 
        /// </summary>
        private int m_ptmRatio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptm"></param>
        public PhysicsWorldInstance(float gravityX, float gravityY, int ptm)
        {
            m_gravityX = gravityX;
            m_gravityY = gravityY;
            m_ptmRatio = ptm;

            InitPhysics();
            InitGround();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPhysics()
        {
            m_world = new b2World(new b2Vec2(m_gravityX, m_gravityY));
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGround()
        {
            var def = new b2BodyDef();
            def.position = new b2Vec2(PointToMeter(-2500), 0);
            def.type = b2BodyType.b2_staticBody;

            var groundBody = m_world.CreateBody(def);

            var groundBox = new b2PolygonShape();
            groundBox.SetAsBox(PointToMeter(5000), PointToMeter(60));

            var fd = new b2FixtureDef();
            fd.shape = groundBox;
            fd.friction = 1f;

            groundBody.CreateFixture(fd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private float PointToMeter(float value) => value / m_ptmRatio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(CreateEntityBody message)
        {
            message.GameObj.CreatePhysicsBody(m_world, m_ptmRatio);

            Sender.Tell(new EntityBodyCreated(message.GameObj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(DestroyEntityBody message)
        {
            m_world.DestroyBody(message.GameObj.PhysicsBody);

            Sender.Tell(new EntityBodyDestroyed(message.GameObj));
        }
    }
}
