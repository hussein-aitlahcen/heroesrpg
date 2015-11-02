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
    public sealed class CombatEntityPart : StatePart
    {
        /// <summary>
        /// 
        /// </summary>
        public override StatePartTypeEnum Type
        {
            get
            {
                return StatePartTypeEnum.COMBAT_ENTITY;
            }
        }

        public int MaxLife { get; private set; }
        public int CurrentLife { get; private set; }

        public CombatEntityPart() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLife"></param>
        /// <param name="currentLife"></param>
        public CombatEntityPart(int maxLife, int currentLife)
        {
            MaxLife = maxLife;
            CurrentLife = currentLife;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            MaxLife = reader.ReadInt32();
            CurrentLife = reader.ReadInt32();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            writer.Write(MaxLife);
            writer.Write(CurrentLife);
        }
    }
}
