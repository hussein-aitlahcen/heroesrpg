using Box2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeroesRpg.Client.Game.World.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MovableEntity : DecoratedEntity
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MovableEntity()
            : base(Box2D.Dynamics.b2BodyType.b2_dynamicBody)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForceToCencer(b2Vec2 force)
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.ApplyForceToCenter(force);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetLinearVelocity(float x, float y)
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.LinearVelocity = new b2Vec2(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyLinearImpulseToCenter(b2Vec2 impulse)
        {
            if (PhysicsBody != null)
            {
                PhysicsBody.ApplyLinearImpulse(impulse, PhysicsBody.WorldCenter);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void ResetVelocityX()
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.LinearVelocity = new b2Vec2(0, PhysicsBody.LinearVelocity.y);
            }
        }
                        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);
            UpdateMovableEntityData(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void UpdateMovableEntityData(BinaryReader reader)
        {
            var velocityX = reader.ReadSingle();
            var velocityY = reader.ReadSingle();
            SetLinearVelocity(velocityX, velocityY);
        }
    }
}
