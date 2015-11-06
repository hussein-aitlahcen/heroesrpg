using Box2D.Common;
using CocosSharp;
using HeroesRpg.Client.Game.World.Entity.Impl.Animated;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        public AnimatedEntity()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="animation"></param>
        public virtual bool StartAnimation(Animation animation, bool repeat = true)
        {
            if (SpriteSheet == null)
                return false;

            if (CurrentAnimation == animation)
                return false;

            CurrentAnimation = animation;
            
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

            return true;
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
            ComputeDecorationPositions();
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
        /// <param name="reader"></param>
        public void UpdateAnimatedEntityData(BinaryReader reader)
        {

        }
    }
}
