using Box2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;

namespace HeroesRpg.Server.Game.Entity
{
    public abstract class MovableEntity : GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        public bool MovablePartDirty
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MovementSpeedX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MovementSpeedY
        {
            get;
            private set;
        }

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
        protected override void InitializeNetworkParts()
        {
            base.InitializeNetworkParts();
            AddNetworkPart(() => MovablePartDirty, () => MovablePartDirty = true, CreateMovableNetworkPart);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            base.Update();
            if(MovementSpeedX != 0 || MovementSpeedY != 0)
            {
                SetVelocity(MovementSpeedX, MovementSpeedY);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetVelocity(float x, float y)
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.LinearVelocity = new b2Vec2(x, y);
                OnMovablePartModified();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetMovementSpeed(int x, int y)
        {
            MovementSpeedX = x;
            MovementSpeedY = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForceToCencer(b2Vec2 force)
        {
            if (PhysicsBody != null)
            {
                PhysicsBody.ApplyForceToCenter(force);
                OnMovablePartModified();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="force"></param>
        public void ApplyForceToCenter(b2Vec2 force)
        {
            if (PhysicsBody != null)
            {
                PhysicsBody.ApplyForceToCenter(force);
                OnMovablePartModified();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyLinearImpulseToCenter(float x, float y)
        {
            if (PhysicsBody != null)
            {
                PhysicsBody.ApplyLinearImpulse(new b2Vec2(x, y), PhysicsBody.WorldCenter);
                OnMovablePartModified();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnMovablePartModified()
        {
            MovablePartDirty = true;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            base.ToNetwork(writer);
            ToMovableEntityPart(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ToMovableEntityPart(BinaryWriter writer)
        {
            if (PhysicsBody != null)
            {
                writer.Write(PhysicsBody.LinearVelocity.x);
                writer.Write(PhysicsBody.LinearVelocity.y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MovableEntityPart CreateMovableNetworkPart() =>
            new MovableEntityPart(
                PhysicsBody.LinearVelocity.x,
                PhysicsBody.LinearVelocity.y,
                PhysicsBody.Position.x,
                PhysicsBody.Position.y);
    }
}
