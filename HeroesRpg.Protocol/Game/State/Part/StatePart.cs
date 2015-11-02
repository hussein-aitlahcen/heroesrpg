using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State.Part
{
    /// <summary>
    /// 
    /// </summary>
    public enum StatePartTypeEnum
    {
        GAME_OBJECT,
        MOVABLE_ENTITY,
        STATIC_ENTITY,
        COMBAT_ENTITY
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class StatePart 
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract StatePartTypeEnum Type
        {
            get;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public abstract void FromNetwork(BinaryReader reader);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public abstract void ToNetwork(BinaryWriter writer);
    }
}
