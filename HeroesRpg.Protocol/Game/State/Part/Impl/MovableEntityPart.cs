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

        public float ImpulseX { get; private set; }
        public float ImpulseY { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public MovableEntityPart() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="impulseX"></param>
        /// <param name="impulseY"></param>
        public MovableEntityPart(float impulseX, float impulseY)
        {
            ImpulseX = impulseX;
            ImpulseY = impulseY;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            ImpulseX = reader.ReadSingle();
            ImpulseY = reader.ReadSingle();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            writer.Write(ImpulseX);
            writer.Write(ImpulseY);
        }
    }
}
