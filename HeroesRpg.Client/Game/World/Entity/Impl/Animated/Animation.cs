using Box2D.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity.Impl.Animated
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Animation
    {
        public static Animation STAND = new Animation("stand", 0.45f);
        public static Animation WALK = new Animation("walk", 0.20f);

        /// <summary>
        /// 
        /// </summary>
        public string RawSprite
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float SpriteDelay
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawSprite"></param>
        public Animation(string rawSprite, float delay)
        {
            RawSprite = rawSprite;
            SpriteDelay = delay;
        }
    }
}
