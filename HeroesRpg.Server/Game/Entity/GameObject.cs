using Akka.Actor;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Protocol.Enum;
using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public class NetworkStatePartCreator
    {
        /// <summary>
        /// 
        /// </summary>
        public Func<bool> DirtyGetter
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Action DirtySetter
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<StatePart> Creator
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirtyGetter"></param>
        /// <param name="creator"></param>
        public NetworkStatePartCreator(Func<bool> dirtyGetter, Action dirtySetter, Func<StatePart> creator)
        {
            DirtyGetter = dirtyGetter;
            Creator = creator;
            DirtySetter = dirtySetter;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class NetworkStatePartsHolder
    {
        /// <summary>
        /// 
        /// </summary>
        public List<NetworkStatePartCreator> PartsCreator
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public NetworkStatePartsHolder()
        {
            PartsCreator = new List<NetworkStatePartCreator>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fetcher"></param>
        public void AddPartCreator(Func<bool> dirtyGetter, Action dirtySetter, Func<StatePart> creator) => PartsCreator.Add(new NetworkStatePartCreator(dirtyGetter, dirtySetter, creator));

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatePart> CreateDirtyParts()
        {
            foreach (var statePartCreator in PartsCreator)
                if (statePartCreator.DirtyGetter())
                {
                    yield return statePartCreator.Creator();
                    statePartCreator.DirtySetter();
                }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract EntityTypeEnum Type
        {
            get;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public IActorRef Map
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool GameObjectPartDirty
        {
            get;
            set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public ulong ControllerId
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public b2Body PhysicsBody
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public b2BodyDef PhysicsBodyDef
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public b2BodyType BodyType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public b2Fixture PhysicsBodyFixture
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PtmRatio
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Mass
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Density
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float Friction
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool FixedRotation
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float InitialPositionX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float InitialPositionY
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        private NetworkStatePartsHolder m_networkParts;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public GameObject(b2BodyType physicsBodyType)
        {
            m_networkParts = new NetworkStatePartsHolder();

            BodyType = physicsBodyType;

            Mass = 1f;
            Density = 1f;
            Friction = 0.4f;

            InitializeNetworkParts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerable<StatePart> GetDirtyParts()
        {
            return m_networkParts.CreateDirtyParts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dirtyGetter"></param>
        /// <param name="creator"></param>
        protected void AddNetworkPart(Func<bool> dirtyGetter, Action dirtySetter, Func<StatePart> creator) =>  m_networkParts.AddPartCreator(dirtyGetter, dirtySetter, creator);

        /// <summary>
        /// 
        /// </summary>
        protected virtual void InitializeNetworkParts()
        {
            AddNetworkPart(() => GameObjectPartDirty, () => GameObjectPartDirty = false, CreateGameObjectNetworkPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract b2Shape CreatePhysicsShape();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="world"></param>
        public void CreatePhysicsBody(b2World world, int ptm)
        {
            PtmRatio = ptm;

            PhysicsBodyDef = new b2BodyDef();
            PhysicsBodyDef.type = BodyType;
            PhysicsBodyDef.position = new b2Vec2(GetPointToMeter(InitialPositionX), GetPointToMeter(InitialPositionY));
            PhysicsBodyDef.fixedRotation = FixedRotation;

            PhysicsBody = world.CreateBody(PhysicsBodyDef);
            PhysicsBody.Mass = Mass;
            PhysicsBody.ResetMassData();

            var fixtureDef = new b2FixtureDef();
            fixtureDef.shape = CreatePhysicsShape();
            fixtureDef.density = Density;
            fixtureDef.friction = Friction;

            PhysicsBodyFixture = PhysicsBody.CreateFixture(fixtureDef);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float GetPointToMeter(float value) => value / PtmRatio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public float GetMeterToPoint(float value) => value * PtmRatio;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetWorldPosition(int x, int y)
        {
            InitialPositionX = x;
            InitialPositionY = y;
            SetPhysicsPosition(GetPointToMeter(x), GetPointToMeter(y));            
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPhysicsPosition(float x, float y)
        {
            if(PhysicsBodyDef != null)
            {
                PhysicsBodyDef.position.x = x;
                PhysicsBodyDef.position.y = y;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void SetId(int id)
        {
            Id = id;
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public void SetControllerId(ulong id)
        {
            ControllerId = id;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mass"></param>
        public void SetMass(float mass)
        {
            Mass = mass;
            if(PhysicsBody != null)
            {
                PhysicsBody.Mass = mass;
            }
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="density"></param>
        public void SetDensity(float density)
        {
            Density = density;
            if (PhysicsBody != null)
            {
                PhysicsBodyFixture.Density = density;
            }
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="friction"></param>
        public void SetFriction(float friction)
        {
            Friction = friction;
            if (PhysicsBody != null)
            {
                PhysicsBodyFixture.Friction = friction;
            }
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fixedRotation"></param>
        public void SetFixedRotation(bool fixedRotation)
        {
            FixedRotation = fixedRotation;
            if (PhysicsBody != null)
            {
                PhysicsBodyDef.fixedRotation = fixedRotation;
            }
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnGameObjectPartDirty()
        {
            GameObjectPartDirty = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public virtual void ToNetwork(BinaryWriter writer)
        {
            ToNetworkGameObjectPart(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void ToNetworkGameObjectPart(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(PhysicsBody.Position.x);
            writer.Write(PhysicsBody.Position.y);
            writer.Write(Mass);
            writer.Write(Density);
            writer.Write(Friction);
            writer.Write(FixedRotation);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatePart CreateGameObjectNetworkPart() =>    
                new GameObjectPart(
                    Id,
                    PhysicsBody.Position.x,
                    PhysicsBody.Position.y,
                    Mass,
                    Density,
                    Friction,
                    FixedRotation);

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {

        }        
    }
}
