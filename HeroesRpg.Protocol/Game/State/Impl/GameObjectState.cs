using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class GameObjectState : EntityState
    {
        /// <summary>
        /// 
        /// </summary>
        public override StateTypeEnum Type
        {
            get
            {
                return StateTypeEnum.GAME_OBJECT;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            base.ToNetwork(writer);
        }
    }
}
