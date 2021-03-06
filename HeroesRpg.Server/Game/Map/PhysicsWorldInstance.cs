﻿using Akka.Actor;
using Akka.Event;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Common;
using HeroesRpg.Common.Generic;
using HeroesRpg.Protocol.Game.State;
using HeroesRpg.Server.Game.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public sealed class TakeSnap : PhysicsWorldMessage
        {
            public long PhysicUpdateSequence { get; }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="snap"></param>
            public TakeSnap(long physicUpdateSequence)
            {
                PhysicUpdateSequence = physicUpdateSequence;
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
        private Stopwatch m_updateWatch;

        /// <summary>
        /// 
        /// </summary>
        private long m_updateAcumulator;

        /// <summary>
        /// 
        /// </summary>
        private long m_physicUpdateSequence;
        
        /// <summary>
        /// 
        /// </summary>
        private int m_ptmRatio;

        /// <summary>
        /// 
        /// </summary>
        private List<GameObject> m_objectToCreate, m_objectToDestroy;

        /// <summary>
        /// 
        /// </summary>
        private List<GameObject> m_objects;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptm"></param>
        public PhysicsWorldInstance(float gravityX, float gravityY, int ptm)
        {
            m_objects = new List<GameObject>();
            m_objectToCreate = new List<GameObject>();
            m_objectToDestroy = new List<GameObject>();
            m_gravityX = gravityX;
            m_gravityY = gravityY;
            m_ptmRatio = ptm;

            InitPhysics();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void AroundPreStart()
        {
            m_updateWatch = Stopwatch.StartNew();

            Context.System.Scheduler.ScheduleTellOnce(TimeSpan.FromMilliseconds(0), Self, new Tick(), Self);
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
        /// <param name="value"></param>
        /// <returns></returns>
        private float PointToMeter(float value) => value / m_ptmRatio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(CreateEntityBody message)
        {
            m_objectToCreate.Add(message.GameObj);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(DestroyEntityBody message)
        {
            m_objectToDestroy.Add(message.GameObj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(Tick message)
        {
            CreateRemoveObjects();

            m_updateAcumulator += m_updateWatch.ElapsedMilliseconds;
            m_updateWatch.Restart();

            while(m_updateAcumulator > Constant.TICK_RATE_MS)
            {
                UpdateBeforePhysics();
                UpdateWorld(Constant.TICK_RATE + 0.00002f);
                ++m_physicUpdateSequence;
                m_updateAcumulator -= Constant.TICK_RATE_MS_LONG;
            }

            Context.Parent.Tell(new TakeSnap(m_physicUpdateSequence));
        }
        
        /// <summary>
        /// 
        /// </summary>
        private void UpdateBeforePhysics()
        {
            foreach (var obj in m_objects)
                obj.UpdateBeforePhysics();
        }

        /// <summary>
        /// 
        /// </summary>
        private void CreateRemoveObjects()
        {
            var c = m_objectToCreate.Count;
            var d = m_objectToDestroy.Count;
            var max = Math.Max(c, d);
            for (int i = 0; i < max; i++)
            {
                if (i < c)
                {
                    var createObj = m_objectToCreate[i];
                    createObj.CreatePhysicsBody(m_world, m_ptmRatio);
                    m_objects.Add(createObj);
                    Context.Parent.Tell(new EntityBodyCreated(createObj), ActorRefs.Nobody);
                }
                if (i < d)
                {
                    var destroyObj = m_objectToDestroy[i];
                    m_world.DestroyBody(destroyObj.PhysicsBody);
                    m_objects.Remove(destroyObj);
                    Context.Parent.Tell(new EntityBodyDestroyed(destroyObj), ActorRefs.Nobody);
                }
            }

            m_objectToCreate.Clear();
            m_objectToDestroy.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        private void UpdateWorld(float delta)
        {
            m_world.Step(delta, WORLD_VELOCITY_ITE, WORLD_POSITION_ITE);
        }
    }
}
