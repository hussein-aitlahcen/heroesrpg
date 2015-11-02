using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State
{
    /// <summary>
    /// 
    /// </summary>
    public enum StateTypeEnum
    {
        GAME_OBJECT,
    }
    
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializableState
    {
        StateTypeEnum Type { get; }
        void FromNetwork(BinaryReader reader);
        void ToNetwork(BinaryWriter writer);
    }
}
