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
    public abstract class MovableEntity : DecoratedEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public const float MOVEMENT_FORCE_X = 18f;

        /// <summary>
        /// 
        /// </summary>
        public int MovementX
        {
            get
            {
                return m_movementX;
            }
            set
            {
                m_movementX = value;
                Move();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int MovementY
        {
            get
            {
                return m_movementY;
            }
            set
            {
                m_movementY = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private int m_movementX, m_movementY;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MovableEntity(int id) : base(id, Box2D.Dynamics.b2BodyType.b2_dynamicBody) { }

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
        public virtual void Stand()
        {
            ResetVelocityX();
            OnStand();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        public virtual void Move()
        {
            if (MovementX != 0)
            {
                FlipX = MovementX < 0;
                ResetVelocityX();
                ApplyLinearImpulseToCenter(GetMovementVelocity());
                OnMove();
            }
            else
            {
                Stand();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual void Fly()
        {
            if(MovementY != 0)
            {
                if (MovementY > 0)
                    OnFlyDown();
                else
                    OnFlyUp();
            }
            else
            {

            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnStand()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnMove()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFlyUp()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFlyDown()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual b2Vec2 GetMovementVelocity()
        {
            return new b2Vec2(MOVEMENT_FORCE_X * MovementX, 0);
        }

        public override void Update(float dt)
        {
            base.Update(dt);

            if (MovementX != 0)
            {
                if (PhysicsBody != null)
                {
                    // Reset friction decay for the movement velocity
                    ResetVelocityX();
                    ApplyLinearImpulseToCenter(GetMovementVelocity());

                    //    b2Vec2 forceA = new b2Vec2(0, -PhysicsBody.Mass * PhysicsBody.World.Gravity.y);
                    //    PhysicsBody.ApplyForce(forceA, PhysicsBody.WorldCenter);
                }
            }
        }
    }
}
