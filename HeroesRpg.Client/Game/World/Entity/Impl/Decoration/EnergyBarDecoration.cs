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
    public abstract class EnergyBarDecoration : EntityDecoration<EnergyBar>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EnergyBarDecoration(DecorationTypeEnum type, int currentEnergy, int maxEnergy, float width, float height, CCColor4B background, CCColor4B foreground)
            : base(type, new EnergyBar(currentEnergy, maxEnergy, width, height, CCColor4B.Black, CCColor4B.Gray))
        {
        }
    }
}
