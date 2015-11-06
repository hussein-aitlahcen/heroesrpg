using Akka.Actor;
using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Protocol;
using HeroesRpg.Protocol.Enum;
using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using HeroesRpg.Server.Network;
using System;
using System.Collections.Generic;
using System.IO;

namespace HeroesRpg.Server.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public enum GameObjectNetworkType
    {
        HIDDEN,
        SHARE_CREATION_ONLY,
        SHARE_CREATION_DELETION,
        SHARE_FULL,
    }

    /// <summary>
    /// 
    /// </summary>
    public enum GameObjectNetworkOperation
    {
        SHARE_CREATION,
        SHARE_UPDATE,
        SHARE_DELETION,
    }

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
        public GameObjectNetworkType NetType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract int SubType
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
        public bool PhysicPartDirty
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
        public bool Bullet
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float GravityScale
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float LinearDamping
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
        public int Width
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsRemovable
        {
            get;
            protected set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float WorldPositionX => GetMeterToPoint(PhysicPositionX);

        /// <summary>
        /// 
        /// </summary>
        public float WorldPositionY => GetMeterToPoint(PhysicPositionY);

        /// <summary>
        /// 
        /// </summary>
        public float PhysicPositionX => PhysicsBody == null ? InitialPositionX : PhysicsBody.Position.x;

        /// <summary>
        /// 
        /// </summary>
        public float PhysicPositionY => PhysicsBody == null ? InitialPositionY : PhysicsBody.Position.y;

        /// <summary>
        /// 
        /// </summary>
        public float PhysicWidth => GetPointToMeter(Width);

        /// <summary>
        /// 
        /// </summary>
        public float PhysicHeight => GetPointToMeter(Height);

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

            Bullet = false;
            GravityScale = 1f;
            LinearDamping = 0f;
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
            AddNetworkPart(() => PhysicPartDirty, () => PhysicPartDirty = false, CreatePhysicNetworkPart);
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
        public virtual void CreatePhysicsBody(b2World world, int ptm)
        {
            PtmRatio = ptm;

            PhysicsBodyDef = new b2BodyDef();
            PhysicsBodyDef.type = BodyType;
            PhysicsBodyDef.position = new b2Vec2(GetPointToMeter(InitialPositionX), GetPointToMeter(InitialPositionY));
            PhysicsBodyDef.fixedRotation = FixedRotation;
            PhysicsBodyDef.gravityScale = GravityScale;
            PhysicsBodyDef.linearDamping = LinearDamping;
            PhysicsBodyDef.bullet = Bullet;

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
        public void SetWorldPosition(float x, float y)
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
        /// <param name="sharable"></param>
        public void SetNetworkType(GameObjectNetworkType netType)
        {
            NetType = netType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operation"></param>
        /// <returns></returns>
        public bool CanNetworkOperation(GameObjectNetworkOperation operation)
        {
            switch (operation)
            {
                case GameObjectNetworkOperation.SHARE_CREATION:
                    return NetType == GameObjectNetworkType.SHARE_CREATION_DELETION ||
                        NetType == GameObjectNetworkType.SHARE_FULL ||
                        NetType == GameObjectNetworkType.SHARE_CREATION_ONLY;

                case GameObjectNetworkOperation.SHARE_DELETION:
                    return NetType == GameObjectNetworkType.SHARE_CREATION_DELETION ||
                        NetType == GameObjectNetworkType.SHARE_FULL;

                case GameObjectNetworkOperation.SHARE_UPDATE:
                    return NetType == GameObjectNetworkType.SHARE_FULL;

                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendUnreliable(NetMessage message)
        {
            if (ControllerId > 0)
                GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendUnreliable(ControllerId, message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void SendReliable(NetMessage message)
        {
            if (ControllerId > 0)
                GameSystem.Instance.ClientMgr.Tell(new ClientManager.SendReliable(ControllerId, message));
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
        /// <param name="width"></param>
        public void SetWidth(int width)
        {
            Width = width;
            OnGameObjectPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        public void SetHeight(int height)
        {
            Height = height;
            OnGameObjectPartDirty();
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
            OnPhysicPartDirty();
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
            OnPhysicPartDirty();
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
            OnPhysicPartDirty();
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
            OnPhysicPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linearDamping"></param>
        public void SetLinearDamping(float linearDamping)
        {
            LinearDamping = linearDamping;
            if (PhysicsBody != null)
            {
                PhysicsBody.LinearDamping = linearDamping;
            }
            OnPhysicPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bullet"></param>
        public void SetBullet(bool bullet)
        {
            Bullet = bullet;
            if (PhysicsBody != null)
            {
                PhysicsBody.SetBullet(bullet);
            }
            OnPhysicPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gravityScale"></param>
        public void SetGravityScale(float gravityScale)
        {
            GravityScale = gravityScale;
            if (PhysicsBody != null)
            {
                PhysicsBody.GravityScale = gravityScale;
            }
            OnPhysicPartDirty();
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
        private void OnPhysicPartDirty()
        {
            PhysicPartDirty = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public virtual void ToNetwork(BinaryWriter writer)
        {
            CreateGameObjectNetworkPart().ToNetwork(writer);
            CreatePhysicNetworkPart().ToNetwork(writer);
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
                    Width, 
                    Height);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatePart CreatePhysicNetworkPart() =>
            new PhysicObjectPart(
                Bullet,
                GravityScale,
                LinearDamping,
                Mass, 
                Density, 
                Friction,
                FixedRotation);

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateBeforePhysics()
        {
        }        
    }
}
