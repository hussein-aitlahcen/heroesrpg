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
        public float ImpulseX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float ImpulseY
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
        /// <param name="world"></param>
        /// <param name="ptm"></param>
        public override void CreatePhysicsBody(b2World world, int ptm)
        {
            base.CreatePhysicsBody(world, ptm);
            SetImpule(ImpulseX, ImpulseY);
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
        public void SetImpule(float x, float y)
        {
            if(PhysicsBody != null)
            {
                ApplyLinearImpulseToCenter(new b2Vec2(x, y));
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
            ImpulseX = reader.ReadSingle();
            ImpulseY = reader.ReadSingle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateMovableEntityPart(MovableEntityPart part)
        {
            SetImpule(part.ImpulseX, part.ImpulseY);
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
