using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
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
    public abstract class GameObject
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
        public float PositionX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float PositionY
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

            Mass = 1f;
            Density = 1f;
            Friction = 0.4f;
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
            PhysicsBodyDef.position = new b2Vec2(GetPointToMeter(PositionX), GetPointToMeter(PositionY));
            PhysicsBodyDef.fixedRotation = FixedRotation;

            PhysicsBody = world.CreateBody(PhysicsBodyDef);
            PhysicsBody.Mass = Mass;

            PhysicsBodyFixture = new b2FixtureDef();
            PhysicsBodyFixture.shape = CreatePhysicsShape();
            PhysicsBodyFixture.density = Density;
            PhysicsBodyFixture.friction = Friction;

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
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetWorldPosition(int x, int y)
        {
            PositionX = x;
            PositionY = y;
        }
        
        public void SetMass(float mass) => Mass = mass;
        public void SetDensity(float density) => Density = density;
        public void SetFriction(float friction) => Friction = friction;
        public void SetFixedRotation(bool fixedRotation) => PhysicsBodyDef.fixedRotation = fixedRotation;

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
    }
}
