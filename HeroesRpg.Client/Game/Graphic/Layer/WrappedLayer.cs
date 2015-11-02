using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Layer.HUD;
using log4net;
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
        protected static ILog Logger = LogManager.GetLogger(typeof(WrappedLayer));

        /// <summary>
        /// 
        /// </summary>
        public WrappedLayer() : base(CCColor4B.White) { }
    }
}
