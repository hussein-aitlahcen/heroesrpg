using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using HeroesRpg.Protocol.Game.State.Part.Impl;
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
    public abstract class GameObject : CCSprite
    {
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
        public b2FixtureDef PhysicsBodyFixture
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
        public b2Vec2 PhysicsPosition
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
            PhysicsPosition = new b2Vec2(0, 0);

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
        public void CreatePhysicsBody(b2World world, int ptm)
        {
            PtmRatio = ptm;

            PhysicsBodyDef = new b2BodyDef();
            PhysicsBodyDef.position = PhysicsPosition;
            PhysicsBodyDef.type = BodyType;
            PhysicsBodyDef.fixedRotation = FixedRotation;

            PhysicsBody = world.CreateBody(PhysicsBodyDef);
            PhysicsBody.Mass = Mass;

            PhysicsBodyFixture = new b2FixtureDef();
            PhysicsBodyFixture.shape = CreatePhysicsShape();
            PhysicsBodyFixture.density = Density;
            PhysicsBodyFixture.friction = Friction;

            PositionX = GetMeterToPoint(PhysicsPosition.x);
            PositionY = GetMeterToPoint(PhysicsPosition.y);

            PhysicsBody.CreateFixture(PhysicsBodyFixture);
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
                PhysicsBodyFixture.density = density;
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
                PhysicsBodyFixture.friction = friction;
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
        /// <param name="reader"></param>
        public virtual void FromNetwork(BinaryReader reader)
        {
            UpdateGameObjectData(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void UpdateGameObjectData(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            SetPhysicsPosition(reader.ReadSingle(), reader.ReadSingle());
            Mass = reader.ReadSingle();
            Density = reader.ReadSingle();
            Friction = reader.ReadSingle();
            FixedRotation = reader.ReadBoolean();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateGameObjectPart(GameObjectPart part)
        {
            Id = part.Id;
            SetPhysicsPosition(part.PositionX, part.PositionY);
            SetMass(part.Mass);
            SetFriction(part.Friction);
            SetDensity(part.Density);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetPhysicsPosition(float x, float y)
        {
            PhysicsPosition = new b2Vec2(x, y);
            if(PhysicsBody != null)
            {
                PhysicsBodyDef.position.x = x;
                PhysicsBodyDef.position.y = y;
            }
        }
    }
}
