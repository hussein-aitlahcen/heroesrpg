using System;
using System.Collections.Generic;
using System.IO;
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
    public sealed class Ground : StaticEntity
    {
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public override EntityTypeEnum Type => EntityTypeEnum.GROUND;

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
        protected override void InitializeNetworkParts()
        {
            base.InitializeNetworkParts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(GetPointToMeter(Width / 2f), GetPointToMeter(Height / 2f));
            return shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            base.ToNetwork(writer);
            writer.Write(Width);
            writer.Write(Height);
        }
    }
}
