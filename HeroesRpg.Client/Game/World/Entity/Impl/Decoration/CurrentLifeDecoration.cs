using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity.Impl.Decoration
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class CurrentLifeDecoration : EnergyBarDecoration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentLife"></param>
        /// <param name="maxLife"></param>
        public CurrentLifeDecoration(int currentLife, int maxLife)
            : base(DecorationTypeEnum.LIFE, currentLife, maxLife, 50, 8, CCColor4B.Blue, CCColor4B.Green)
        {
        }
    }
}
