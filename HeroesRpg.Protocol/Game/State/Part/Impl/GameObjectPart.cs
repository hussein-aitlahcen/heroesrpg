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
        public float Mass { get; private set; }
        public float Density { get; private set; }
        public float Friction { get; private set; }
        public bool FixedRotation { get; private set; }

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
        public GameObjectPart(int id, float positionX, float positionY, float mass, float density, float friction, bool fixedRotation)
        {
            Id = id;
            PositionX = positionX;
            PositionY = positionY;
            Mass = mass;
            Density = density;
            Friction = friction;
            FixedRotation = fixedRotation;
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
            Mass = reader.ReadSingle();
            Density = reader.ReadSingle();
            Friction = reader.ReadSingle();
            FixedRotation = reader.ReadBoolean();
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
            writer.Write(Mass);
            writer.Write(Density);
            writer.Write(Friction);
            writer.Write(FixedRotation);
        }
    }
}
