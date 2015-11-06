using Box2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using Box2D.Collision.Shapes;
using Box2D.Dynamics;

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
        public float InitialVelocityX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float InitialVelocityY
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float VelocityX => PhysicsBody == null ? InitialVelocityX : PhysicsBody.LinearVelocity.x;

        /// <summary>
        /// 
        /// </summary>
        public float VelocityY => PhysicsBody == null ? InitialVelocityY : PhysicsBody.LinearVelocity.y;

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
        /// <param name="world"></param>
        /// <param name="ptm"></param>
        public override void CreatePhysicsBody(b2World world, int ptm)
        {
            base.CreatePhysicsBody(world, ptm);
            SetVelocity(InitialVelocityX, InitialVelocityY);
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
        public override void UpdateBeforePhysics()
        {
            base.UpdateBeforePhysics();
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
            InitialVelocityX = x;
            InitialVelocityY = y;
            if (PhysicsBody != null)
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
            CreateMovableNetworkPart().ToNetwork(writer);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MovableEntityPart CreateMovableNetworkPart() =>
            new MovableEntityPart(
                PhysicsBody != null ? PhysicsBody.LinearVelocity.x : InitialVelocityX,
                PhysicsBody != null ? PhysicsBody.LinearVelocity.y : InitialVelocityY,
                PhysicsBody != null ? PhysicsBody.Position.x : GetPointToMeter(InitialPositionX),
                PhysicsBody != null ? PhysicsBody.Position.y : GetPointToMeter(InitialPositionY));
    }
}
