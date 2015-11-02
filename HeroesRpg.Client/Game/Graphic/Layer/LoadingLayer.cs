using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Game.World.Entity.Impl;
using HeroesRpg.Client.Game.World.Entity.Impl.Animated;
using HeroesRpg.Protocol.Enum;
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
    public abstract class LoadingLayer : WrappedLayer
    {
        /// <summary>
        /// 
        /// </summary>
        public int Percentage
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDoneLoading
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        private Hero m_loadingHero;

        /// <summary>
        /// 
        /// </summary>
        public LoadingLayer()
        {
            m_loadingHero = new DragonBallHero() { Position = new CCPoint(0, 15) };
            m_loadingHero.Position = new CCPoint(0, 15);
            m_loadingHero.SetHeroId((int)DragonBallHeroEnum.BROLY);
            m_loadingHero.SetPlayerName("Broly");
            m_loadingHero.Scale = 1.3f;
            m_loadingHero.StartAnimation(Animation.WALK);
            ZOrder = int.MaxValue;
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            var bounds = VisibleBoundsWorldspace;
            
            AddChild(BackgroungLayer.LOADING_BG);
            AddChild(new LoadingLabel("Chargement", CCColor3B.Black) { Position = new CCPoint(15, bounds.MaxY - 15) });
            AddChild(m_loadingHero);

            MoveHero();            
        }

        /// <summary>
        /// 
        /// </summary>
        private void MoveHero()
        {
            var goalX = m_loadingHero.PositionX == 0 ? VisibleBoundsWorldspace.MidX - 50 : 0;
            m_loadingHero.FlipX = goalX == 0;
            m_loadingHero.RunAction(new CCSequence(
                new CCMoveTo(5f, new CCPoint(goalX, 15)),
                new CCCallFunc(MoveHero)));
        }
    }
}
