using HeroesRpg.Client.Game.Graphic.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using HeroesRpg.Client.Game.World.Entity.Impl;
using HeroesRpg.Client.Game.Util;
using Box2D.Common;
using HeroesRpg.Client.Game.World.Entity.Impl.Animated;

namespace HeroesRpg.Client.Game.Graphic.Scene
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameScene : WrappedScene<GameScene>
    {        
        /// <summary>
        /// 
        /// </summary>
        public GameMapLayer MapLayer
        {
            get;
            private set;
        }

        private Hero m_hero;

        /// <summary>
        /// 
        /// </summary>
        public GameScene()
        {
            AddChild(MapLayer = new GameMapLayer());
                                  
            Schedule(Update);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected override void OnKeyPressed(CCEventKeyboard ev)
        {
            base.OnKeyPressed(ev);
            switch (ev.Keys)
            {
                case CCKeys.Space:
                    //to change velocity by 10
                    float impulse = m_hero.PhysicsBody.Mass * 10;
                    m_hero.ApplyLinearImpulseToCenter(new b2Vec2(0, impulse));
                    break;

                case CCKeys.Left:
                    break;

                case CCKeys.Right:
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected override void OnKeyReleased(CCEventKeyboard ev)
        {
            base.OnKeyReleased(ev);
            switch (ev.Keys)
            {
                case CCKeys.Left:
                    break;

                case CCKeys.Right:
                    break;
            }
        }
    }
}
