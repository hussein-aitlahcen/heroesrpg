﻿using Box2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using Box2D.Dynamics;
using CocosSharp;
using HeroesRpg.Common;

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
        public bool IsPositionCorrection => UpdateTime < CorrectionTime;

        /// <summary>
        /// 
        /// </summary>
        public float CorrectionX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float CorrectionY
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public float CorrectionTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CCDrawNode Ghost
        {
            get;
            private set;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MovableEntity()
            : base(b2BodyType.b2_dynamicBody)
        {
            Ghost = new CCDrawNode();
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();            
            Parent.AddChild(Ghost);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);
           
            if (PhysicsBody != null)
            {
                if (IsPositionCorrection)
                {
                    var diffTime = CorrectionTime - UpdateTime;
                    var ratio = 1 - (diffTime / Constant.UPDATE_RATE_SECOND);
                    var interpolation = CCPoint.Lerp(new CCPoint(PhysicsBody.Position.x, PhysicsBody.Position.y), new CCPoint(CorrectionX, CorrectionY), ratio);

                    PhysicsBody.SetTransform(new b2Vec2(interpolation.X, interpolation.Y), PhysicsBody.Angle);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void UpdateBeforePhysics(float dt)
        {
            base.UpdateBeforePhysics(dt);
            if (MovementSpeedX != 0 || MovementSpeedY != 0)
            {
                SetVelocity(MovementSpeedX, MovementSpeedY);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetMovementSpeed(int x, int y)
        {
            MovementSpeedX = x * 10;
            MovementSpeedY = y * 10;
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

            Ghost.Cleanup();
            Ghost.PositionX = PositionX;
            Ghost.PositionY = PositionY;
            Ghost.DrawRect(new CCRect(0, 0,  Width, Height), new CCColor4B(0, 0, 0, 50));
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
        public void SetVelocity(float x, float y)
        {
            if (PhysicsBody != null)
            {
                PhysicsBody.LinearVelocity = new b2Vec2(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetVelocityInterpolation(float x, float y)
        {
            if (PhysicsBody != null)
            {
                if (PhysicsBody.LinearVelocity.x == x && PhysicsBody.LinearVelocity.y == y)
                    return;
                
                PhysicsBody.LinearVelocity = new b2Vec2(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyLinearImpulse(float x, float y)
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.ApplyLinearImpulse(new b2Vec2(x, y), PhysicsBody.WorldCenter);
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
            }
        }

        // <summary>
        /// 
        /// </summary>
        /// <param name="nextX"></param>
        /// <param name="nextY"></param>
        public void SetPositionInterpolation(float nextX, float nextY)
        {
            if (PhysicsBody != null)
            {
                if (WorldManager.Instance.HasInterpolatedLocalEntityState(Id))
                {
                    var state = WorldManager.Instance.GetInterpolatedEntityState(Id);

                    var diffX = state.PositionX - nextX;
                    var diffY = state.PositionY - nextY;
                    var absX = Math.Abs(diffX);
                    var absY = Math.Abs(diffY);

                    Logger.Debug($"diffX={absX} time={WorldManager.Instance.InterpolationState.PhysicUpdateSequence - WorldManager.Instance.ServerPhysicUpdateSequence}");
                    
                    if (absX > WorldManager.CL_INTERPOLATION_ERROR || absY > WorldManager.CL_INTERPOLATION_ERROR)
                    {
                        CorrectionX = nextX;
                        CorrectionY = nextY;
                        CorrectionTime = UpdateTime + Constant.UPDATE_RATE_SECOND;
                    }
                }
                else
                {
                    CorrectionX = nextX;
                    CorrectionY = nextY;
                    CorrectionTime = UpdateTime + Constant.UPDATE_RATE_SECOND;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);

            var movablePart = new MovableEntityPart();
            movablePart.FromNetwork(reader);
            UpdateMovableEntityPart(movablePart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateMovableEntityPart(MovableEntityPart part)
        {
            InitialVelocityX = part.VelocityX;
            InitialVelocityY = part.VelocityY;
            if(!IsLocal)
                SetVelocityInterpolation(part.VelocityX, part.VelocityY);
            SetPositionInterpolation(part.X, part.Y);
            Ghost.PositionX = GetMeterToPoint(part.X);
            Ghost.PositionY = GetMeterToPoint(part.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        public override void UpdatePart(IEnumerable<StatePart> parts)
        {
            base.UpdatePart(parts);
            var movablePart = parts.FirstOrDefault(part => part.Type == StatePartTypeEnum.MOVABLE_ENTITY);
            if (movablePart != null)
                UpdateMovableEntityPart(movablePart as MovableEntityPart);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();

            Ghost.RemoveFromParent(true);
            Ghost.Dispose();
        }
    }
}
