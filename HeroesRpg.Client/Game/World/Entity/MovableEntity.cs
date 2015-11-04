using Box2D.Common;
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
        public const int INTERPOLATION_MIN_DELTA = 20;

        /// <summary>
        /// 
        /// </summary>
        public const float INTERPOLATION_CONSTANT = 1 / 40f;

        /// <summary>
        /// 
        /// </summary>
        public const float STEP_INTERPOLATION = INTERPOLATION_CONSTANT / 5;

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
            if (MovementSpeedX != 0 || MovementSpeedY != 0)
            {
                SetVelocity(MovementSpeedX, MovementSpeedY);
            }
            if (PhysicsBody != null)
            {
                if (IsPositionCorrection)
                {
                    var diffTime = CorrectionTime - UpdateTime;
                    var ratio = 1 - (diffTime / INTERPOLATION_CONSTANT);
                    var interpolation = CCPoint.Lerp(new CCPoint(PhysicsBody.Position.x, PhysicsBody.Position.y), new CCPoint(CorrectionX, CorrectionY), ratio);
                    PhysicsBody.SetTransform(new b2Vec2(interpolation.X, interpolation.Y), PhysicsBody.Angle);
                }

                if (IsLocal)
                    WorldManager.Instance.AddLocalStateSnapshot(PositionX, PositionY, PhysicsBody.LinearVelocity.x, PhysicsBody.LinearVelocity.y);
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
            Ghost.DrawRect(new CCRect(0, 0,  55, 85), new CCColor4B(0, 0, 0, 50));
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

                if (IsLocal && WorldManager.Instance.IsVelocityInterpolationValid(x, y))
                {
                    Logger.Debug("velocity prediction valid, SKIPP");
                    return;
                }
                PhysicsBody.LinearVelocity = new b2Vec2(x, y);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impulse"></param>
        public void ApplyLinearImpulse(b2Vec2 impulse)
        {
            if(PhysicsBody != null)
            {
                PhysicsBody.ApplyLinearImpulse(impulse, PhysicsBody.WorldCenter);
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
                var worldX = GetMeterToPoint(nextX);
                var worldY = GetMeterToPoint(nextY);
                var realDiffX = worldX - PositionX;
                var realDiffY = worldY - PositionY;
                var diffX = Math.Abs(worldX - PositionX);
                var diffY = Math.Abs(worldY - PositionY);

                if ((diffX > INTERPOLATION_MIN_DELTA || diffY > INTERPOLATION_MIN_DELTA)
                    ||
                    (PhysicsBody.LinearVelocity.x == 0f && PhysicsBody.LinearVelocity.y == 0f &&
                    (diffX > INTERPOLATION_MIN_DELTA / 4 || diffY > INTERPOLATION_MIN_DELTA / 4)))
                {
                    if (IsLocal && WorldManager.Instance.IsPositionInterpolationValid(worldX, worldY))
                    {
                        return;
                    }
                    if (realDiffX > 0 && MovementSpeedX <= 0
                        || realDiffX < 0 && MovementSpeedX >= 0
                        || realDiffY > 0 && MovementSpeedY <= 0
                        || realDiffY < 0 && MovementSpeedY >= 0)
                    {
                        if (diffX > INTERPOLATION_MIN_DELTA)
                            nextX += GetPointToMeter(realDiffX > 0 ? -INTERPOLATION_MIN_DELTA : +INTERPOLATION_MIN_DELTA);
                        if (diffY > INTERPOLATION_MIN_DELTA)
                            nextY += GetPointToMeter(realDiffX > 0 ? -INTERPOLATION_MIN_DELTA : +INTERPOLATION_MIN_DELTA);

                        CorrectionX = nextX;
                        CorrectionY = nextY;
                        CorrectionTime = UpdateTime + INTERPOLATION_CONSTANT;
                    }
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
            UpdateMovableEntityData(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void UpdateMovableEntityData(BinaryReader reader)
        {
            InitialVelocityX = reader.ReadSingle();
            InitialVelocityY = reader.ReadSingle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateMovableEntityPart(MovableEntityPart part)
        {
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
    }
}
