using Box2D.Common;
using CocosSharp;
using HeroesRpg.Client.Game.World.Entity.Impl.Animated;
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
        public Animation CurrentAnimation
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CCFiniteTimeAction AnimationAction
        {
            get;
            private set;
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
        public virtual void StartAnimation(Animation animation, bool repeat = true)
        {
            if (CurrentAnimation == animation)
                return;
            CurrentAnimation = animation;

            var realLinearVelocity = new b2Vec2(FlipX ? -animation.LinearVelocity.x : animation.LinearVelocity.x, -animation.LinearVelocity.y);
            ApplyLinearVelocity(realLinearVelocity);

            var animationFrames = GetAnimationSprites(animation.RawSprite);
            SpriteFrame = animationFrames.First();

            if (AnimationAction != null)
                StopAction(AnimationAction.Tag);
            AnimationAction = new CCAnimate(new CCAnimation(animationFrames, animation.SpriteDelay));
            if (repeat)
                AnimationAction = new CCRepeatForever(AnimationAction);
            AnimationAction.Tag = TAG_CURRENT_ANIMATION;
            RunAction(AnimationAction);

            OnFrameChanged();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        /// <returns></returns>
        public List<CCSpriteFrame> GetAnimationSprites(string animation) => SpriteSheet.Frames.FindAll((x) => x.TextureFilename.StartsWith(animation));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="contains"></param>
        public virtual void SetSpriteFrame(string contains)
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
