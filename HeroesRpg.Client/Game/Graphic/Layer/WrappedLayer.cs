using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Layer
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WrappedLayer : CCLayerColor
    {
        /// <summary>
        /// 
        /// </summary>
        public WrappedLayer() : base(CCColor4B.White) { }
    }
}
