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
    public sealed class GameObjectPart : StatePart
    {
        /// <summary>
        /// 
        /// </summary>
        public override StatePartTypeEnum Type
        {
            get
            {
                return StatePartTypeEnum.GAME_OBJECT;
            }
        }
        
        public int Id { get; private set; }
        public float PositionX { get; private set; }
        public float PositionY { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public GameObjectPart()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="mass"></param>
        /// <param name="density"></param>
        /// <param name="friction"></param>
        /// <param name="fixedRotation"></param>
        public GameObjectPart(int id, float positionX, float positionY, int width, int height)
        {
            Id = id;
            PositionX = positionX;
            PositionY = positionY;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            PositionX = reader.ReadSingle();
            PositionY = reader.ReadSingle();
            Width = reader.ReadInt32();
            Height = reader.ReadInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            writer.Write(Id);
            writer.Write(PositionX);
            writer.Write(PositionY);
            writer.Write(Width);
            writer.Write(Height);
        }
    }
}
