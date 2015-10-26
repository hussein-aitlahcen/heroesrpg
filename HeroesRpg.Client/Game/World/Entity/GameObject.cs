using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using System;
using System.Collections.Generic;
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
        const int PTM_RATIO = 32;

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
        public b2BodyType BodyType
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
        /// <param name="id"></param>
        public GameObject(int id, b2BodyType physicsBodyType, bool fixedRotation = true)
        {
            Id = id;
            BodyType = physicsBodyType;
            FixedRotation = fixedRotation;
            
            Schedule(Update);     
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract b2Shape CreatePhysicsShape(int ptm);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="world"></param>
        public void CreatePhysicsBody(b2World world, int ptm)
        {
            var def = new b2BodyDef();
            def.position = new b2Vec2(PositionX / ptm, PositionY / ptm);
            def.type = BodyType;
            def.fixedRotation = FixedRotation;

            var body = world.CreateBody(def);
            
            var fd = new b2FixtureDef();
            fd.shape = CreatePhysicsShape(ptm);
            fd.density = 1f;

            body.CreateFixture(fd);

            PhysicsBody = body;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);

            if(PhysicsBody != null)
            {
                PositionX = PhysicsBody.Position.x * PTM_RATIO;
                PositionY = PhysicsBody.Position.y * PTM_RATIO;
            }
        }
    }
}
