using Akka.Actor;
using Akka.Event;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Common.Generic;
using HeroesRpg.Protocol.Game.State;
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
        public static Props Create(float gravityX = GRAVITY_X, float gravityY = GRAVITY_Y, int ptm = PTM_RATIO) => Props.Create(() => new PhysicsWorldInstance(gravityX, gravityY, ptm));

        /// <summary>
        /// 
        /// </summary>
        public sealed class Tick : Singleton<Tick>
        { 
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class TickDone : PhysicsWorldMessage
        {
            /// <summary>
            /// 
            /// </summary>
            public double Delta
            {
                get;
                private set;
            }

            /// <summary>
            /// 
            /// </summary>
            public double GameTime
            {
                get;
                private set;
            }
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="snap"></param>
            public TickDone(double delta, double time)
            {
                Delta = delta;
                GameTime = time;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class CreateEntityBody : PhysicsWorldGameObjectMessage
        {
            public CreateEntityBody(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class DestroyEntityBody : PhysicsWorldGameObjectMessage
        {
            public DestroyEntityBody(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class EntityBodyCreated : PhysicsWorldGameObjectMessage
        {
            public EntityBodyCreated(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public sealed class EntityBodyDestroyed : PhysicsWorldGameObjectMessage
        {
            public EntityBodyDestroyed(GameObject obj) : base(obj) { }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract class PhysicsWorldGameObjectMessage : PhysicsWorldMessage
        {
            public GameObject GameObj
            {
                get;
                private set;
            }

            public PhysicsWorldGameObjectMessage(GameObject obj)
            {
                GameObj = obj;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract class PhysicsWorldMessage
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class PhysicsWorldInstance : TypedActor,
        IHandle<PhysicsWorldInstance.CreateEntityBody>,
        IHandle<PhysicsWorldInstance.DestroyEntityBody>,
        IHandle<PhysicsWorldInstance.Tick>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILoggingAdapter m_log = Logging.GetLogger(Context);

        /// <summary>
        /// 
        /// </summary>
        public const float TICK_RATE = 1.0f / 60.0f;

        /// <summary>
        /// 
        /// </summary>
        public const float TICK_MS = TICK_RATE * 1000;

        /// <summary>
        /// 
        /// </summary>
        public const int WORLD_VELOCITY_ITE = 8;

        /// <summary>
        /// 
        /// </summary>
        public const int WORLD_POSITION_ITE = 3;

        /// <summary>
        /// 
        /// </summary>
        public const int PTM_RATIO = 32;

        /// <summary>
        /// 
        /// </summary>
        public const float GRAVITY_X = 0;

        /// <summary>
        /// 
        /// </summary>
        public const float GRAVITY_Y = -22f;

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
        private double m_gameTime;

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

            m_gameTime = 0;

            InitPhysics();
            InitGround();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AroundPreStart()
        {
            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromMilliseconds(TICK_RATE), Self, new Tick(), Self);
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

            Context.Parent.Forward(new EntityBodyCreated(message.GameObj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(DestroyEntityBody message)
        {
            m_world.DestroyBody(message.GameObj.PhysicsBody);

            Context.Parent.Forward(new EntityBodyDestroyed(message.GameObj));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(Tick message)
        {
            var begin = DateTime.Now;

            UpdateWorld();

            var end = DateTime.Now;
            var updateTime = (end - begin).TotalMilliseconds;
            var delta = TICK_MS - updateTime;
            if(updateTime > TICK_MS)
            {
                m_log.Info("physics world update lagged : " + updateTime);
                m_gameTime += Math.Abs(delta);
                delta = 1;
            }

            m_gameTime += TICK_MS;

            Context.Parent.Tell(new TickDone(delta, m_gameTime));
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateWorld()
        {
            m_world.Step(TICK_RATE, WORLD_VELOCITY_ITE, WORLD_POSITION_ITE);
        }
    }
}
