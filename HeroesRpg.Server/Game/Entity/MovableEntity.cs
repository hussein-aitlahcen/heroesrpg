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
        protected override void InitializeNetworkParts()
        {
            base.InitializeNetworkParts();
            AddNetworkPart(() => MovablePartDirty, () => MovablePartDirty = false, CreateMovableNetworkPart);
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
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void SetLinearImpulse(float x, float y)
        {
            if (PhysicsBody != null)
            {
                ImpulseX = x;
                ImpulseY = y;

                ApplyLinearImpulseToCenter(new b2Vec2(x, y));
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
        public void ApplyLinearImpulseToCenter(b2Vec2 impulse)
        {
            if (PhysicsBody != null)
            {
                PhysicsBody.ApplyLinearImpulse(impulse, PhysicsBody.WorldCenter);
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
            writer.Write(ImpulseX);
            writer.Write(ImpulseY);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public MovableEntityPart CreateMovableNetworkPart() =>
            new MovableEntityPart(
                ImpulseX,
                ImpulseY);
    }
}
