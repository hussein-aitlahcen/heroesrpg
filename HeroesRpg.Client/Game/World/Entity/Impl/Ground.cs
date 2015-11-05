using Box2D.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CocosSharp;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Ground : StaticEntity
    {
        public CCDrawNode GroundSprite { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Ground()
        {
            AddChild(GroundSprite = new CCDrawNode());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(PhysicWidth / 2f, PhysicHeight / 2f);
            GroundSprite.DrawRect(new CCRect(0, 0,  Width, Height), CCColor4B.Black);
            return shape;
        }
    }
}
