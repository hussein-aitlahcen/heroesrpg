using Box2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MovableEntity : GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MovableEntity(int id) : base(id, Box2D.Dynamics.b2BodyType.b2_dynamicBody) { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="velocity"></param>
        public void ApplyLinearVelocity(b2Vec2 velocity)
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.LinearVelocity = velocity;
            }
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (PhysicsBody != null)
            {
            //    b2Vec2 forceA = new b2Vec2(0, -PhysicsBody.Mass * PhysicsBody.World.Gravity.y);
            //    PhysicsBody.ApplyForce(forceA, PhysicsBody.WorldCenter);
            }
        }
    }
}
