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

            m_hero = new Hero(0, 10, "Smarken") { Position = new CCPoint(VisibleBoundsScreenspace.MidX, 500) };
            m_hero.Scale = 1.0f;
            m_hero.StartAnimation(Animation.STAND);

            MapLayer.AddGameObject(m_hero);

            Schedule(Update);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);

            Animation animation = null;
            if(InputHelper.Instance.IsKeyIn(CCKeys.Left))
            {
                animation = Animation.WALK;
                m_hero.FlipX = true;
            }
            else if(InputHelper.Instance.IsKeyIn(CCKeys.Right))
            {
                animation = Animation.WALK;
                m_hero.FlipX = false;
            }
            else
            {
                animation = Animation.STAND;
            }
            m_hero.StartAnimation(animation);
        }
    }
}
