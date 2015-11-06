using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using HeroesRpg.Protocol.Enum;
using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using log4net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity
{
    /// <summary>
    /// Base class of all game objects
    /// </summary>
    public abstract class GameObject : CCSprite, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        protected static ILog Logger = LogManager.GetLogger(typeof(GameObject));

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
        public bool IsLocal => WorldManager.Instance.ControlledObjectId == Id;

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
        public float PtmRatio
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
        public float WorldPositionX => GetMeterToPoint(PhysicPositionX);

        /// <summary>
        /// 
        /// </summary>
        public float WorldPositionY => GetMeterToPoint(PhysicPositionY);

        /// <summary>
        /// 
        /// </summary>
        public float PhysicPositionX => PhysicsBody == null ? InitialPosition.x : PhysicsBody.Position.x;

        /// <summary>
        /// 
        /// </summary>
        public float PhysicPositionY => PhysicsBody == null ? InitialPosition.y : PhysicsBody.Position.y;

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
        public bool FixedRotation
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public b2Vec2 InitialPosition
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float UpdateTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public GameObject(b2BodyType physicsBodyType)
        {
            BodyType = physicsBodyType;
            AnchorPoint = CCPoint.AnchorLowerLeft;
            InitialPosition = new b2Vec2(0, 0);

            Mass = 1f;
            Density = 1f;
            Friction = 0.4f;

            Schedule(Update);
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
            PhysicsBodyDef.position = InitialPosition;
            PhysicsBodyDef.type = BodyType;
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
            
            PositionX = GetMeterToPoint(InitialPosition.x);
            PositionY = GetMeterToPoint(InitialPosition.y);
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
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            UpdateTime += dt;
            base.Update(dt);
            if (PhysicsBody != null)
            {
                PositionX = GetMeterToPoint(PhysicsBody.Position.x);
                PositionY = GetMeterToPoint(PhysicsBody.Position.y);                
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void UpdateBeforePhysics(float dt)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mass"></param>
        public void SetMass(float mass)
        {
            Mass = mass;
            if (PhysicsBody != null)
            {
                PhysicsBody.Mass = mass;
            }
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
                PhysicsBody.SetFixedRotation(fixedRotation);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="linearDamping"></param>
        public void SetLinearDamping(float linearDamping)
        {
            LinearDamping = linearDamping;
            if(PhysicsBody != null)
            {
                PhysicsBody.LinearDamping = linearDamping;
            }
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gravityScale"></param>
        public void SetGravityScale(float gravityScale)
        {
            GravityScale = gravityScale;
            if(PhysicsBody != null)
            {
                PhysicsBody.GravityScale = gravityScale;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public virtual void FromNetwork(BinaryReader reader)
        {
            var gameObjPart = new GameObjectPart();
            gameObjPart.FromNetwork(reader);
            UpdateGameObjectPart(gameObjPart);

            var physicPart = new PhysicObjectPart();
            physicPart.FromNetwork(reader);
            UpdatePhysicPart(physicPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateGameObjectPart(GameObjectPart part)
        {
            Id = part.Id;
            SetPhysicsPosition(part.PositionX, part.PositionY);
            SetWidth(part.Width);
            SetHeight(part.Height);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdatePhysicPart(PhysicObjectPart part)
        {
            SetMass(part.Mass);
            SetFriction(part.Friction);
            SetDensity(part.Density);
            SetFixedRotation(part.FixedRotation);
            SetLinearDamping(part.LinearDamping);
            SetGravityScale(part.GravityScale);
            SetBullet(part.Bullet);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        public virtual void UpdatePart(IEnumerable<StatePart> parts)
        {
            var gameObjectPart = parts.FirstOrDefault(part => part.Type == StatePartTypeEnum.GAME_OBJECT);
            if (gameObjectPart != null)
                UpdateGameObjectPart(gameObjectPart as GameObjectPart);
            var physicPart = parts.OfType<PhysicObjectPart>().FirstOrDefault();
            if (physicPart != null)
                UpdatePhysicPart(physicPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        public void SetWidth(int width)
        {
            Width = width;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="height"></param>
        public void SetHeight(int height)
        {
            Height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPhysicsPosition(float x, float y)
        {
            InitialPosition = new b2Vec2(x, y);
            if(PhysicsBody != null)
            {
                if (x != PhysicsBody.Position.x || y != PhysicsBody.Position.y)
                {
                    PhysicsBody.SetTransform(new b2Vec2(x, y), PhysicsBody.Angle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual new void Dispose()
        {
            base.Dispose();
        }
    }
}
