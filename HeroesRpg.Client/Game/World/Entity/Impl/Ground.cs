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
        public int Width { get; private set; }
        public int Height { get; private set; }
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
            shape.SetAsBox(GetPointToMeter(Width / 2f), GetPointToMeter(Height / 2f));
            ContentSize = new CCSize(Width, Height);
            GroundSprite.DrawRect(new CCRect(0, 0, Width, GetMeterToPoint(Height)), CCColor4B.Black);
            return shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
        }
    }
}
