using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using HeroesRpg.Protocol.Enum;

namespace HeroesRpg.Server.Game.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Projectile : AnimatedEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public override int SubType => 0;

        /// <summary>
        /// 
        /// </summary>
        public override EntityTypeEnum Type => EntityTypeEnum.PROJECTILE;

        /// <summary>
        /// 
        /// </summary>
        public Projectile()
        {
            SetNetworkType(GameObjectNetworkType.SHARE_CREATION_DELETION);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void UpdateBeforePhysics()
        {
            base.UpdateBeforePhysics();
            IsRemovable = WorldPositionX > 1024 || WorldPositionX < 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(PhysicWidth / 2, PhysicHeight / 2);
            return shape;
        }
    }
}
