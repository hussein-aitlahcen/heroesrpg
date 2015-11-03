using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State.Part.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MovableEntityPart : StatePart
    {
        /// <summary>
        /// 
        /// </summary>
        public override StatePartTypeEnum Type
        {
            get
            {
                return StatePartTypeEnum.MOVABLE_ENTITY;
            }
        }

        public float VelocityX { get; private set; }
        public float VelocityY { get; private set; }
        public float X { get; private set; }
        public float Y { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public MovableEntityPart() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impulseX"></param>
        /// <param name="impulseY"></param>
        public MovableEntityPart(float vX, float vY, float x, float y)
        {
            VelocityX = vX;
            VelocityY = vY;
            X = x;
            Y = y;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            VelocityX = reader.ReadSingle();
            VelocityY = reader.ReadSingle();
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            writer.Write(VelocityX);
            writer.Write(VelocityY);
            writer.Write(X);
            writer.Write(Y);
        }
    }
}
