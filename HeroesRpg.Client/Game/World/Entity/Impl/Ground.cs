using Box2D.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Ground : StaticEntity
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public Ground()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public Ground(int width, int height)
        {
            Width = width;
            Height = height;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(GetPointToMeter(Width / 2), GetPointToMeter(Height / 2));
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
