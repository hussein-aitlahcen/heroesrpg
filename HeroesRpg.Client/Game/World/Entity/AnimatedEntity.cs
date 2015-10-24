using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AnimatedEntity : MovableEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public const int TAG_CURRENT_ANIMATION = 1000;

        /// <summary>
        /// 
        /// </summary>
        public abstract CCSpriteSheet SpriteSheet
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public AnimatedEntity(int id) : base(id)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        public virtual void StartAnimation(string animation, bool repeat = true, float delay = 0.20f)
        {
            var animationFrames = SpriteSheet.Frames.FindAll((x) => x.TextureFilename.StartsWith(animation));
            CCFiniteTimeAction action = new CCAnimate(new CCAnimation(animationFrames, delay));
            if (repeat)
                action = new CCRepeatForever(action);
            RunAction(action);
            OnFrameChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contains"></param>
        public virtual void SetInitialFrame(string contains)
        {
            SpriteFrame = SpriteSheet.Frames.First(frame => frame.TextureFilename.StartsWith(contains));
            OnFrameChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void OnFrameChanged()
        {
        }
    }
}
