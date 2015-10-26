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
        public static Animation STAND = new Animation("stand", 0.35f, new b2Vec2(0, 0));
        public static Animation WALK = new Animation("walk", 0.20f, new b2Vec2(5, 0));

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
        public b2Vec2 LinearVelocity
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rawSprite"></param>
        public Animation(string rawSprite, float delay, b2Vec2 linearVelocity)
        {
            RawSprite = rawSprite;
            SpriteDelay = delay;
            LinearVelocity = linearVelocity;
        }
    }
}
