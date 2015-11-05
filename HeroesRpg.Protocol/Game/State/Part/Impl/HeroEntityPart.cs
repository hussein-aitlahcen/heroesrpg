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
    public sealed class HeroEntityPart : StatePart
    {
        /// <summary>
        /// 
        /// </summary>
        public override StatePartTypeEnum Type
        {
            get
            {
                return StatePartTypeEnum.HERO_ENTITY;
            }
        }

        public int HeroId { get; private set; }
        public int HeroType { get; private set; }
        public string PlayerName { get; private set; }

        public HeroEntityPart() { }

        public HeroEntityPart(int heroId, int heroType, string playerName)
        {
            HeroId = heroId;
            HeroType = heroType;
            PlayerName = playerName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            HeroId = reader.ReadInt32();
            HeroType = reader.ReadInt32();
            PlayerName = reader.ReadString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            writer.Write(HeroId);
            writer.Write(HeroType);
            writer.Write(PlayerName);
        }
    }
}
