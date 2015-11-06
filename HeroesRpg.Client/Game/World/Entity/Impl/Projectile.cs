using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using CocosSharp;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Projectile : AnimatedEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public override CCSpriteSheet SpriteSheet => null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);
            Ghost.PositionX = GetMeterToPoint(PhysicsBody.Position.x);
            Ghost.PositionY = GetMeterToPoint(PhysicsBody.Position.y);
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
