using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Game.Sound;
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
    public sealed class MainMenuLayer : WrappedLayer
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MENU_ALIGN_PADDING = 15;

        /// <summary>
        /// 
        /// </summary>
        public CCMenu Menu
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public MainMenuLayer() 
        {
            Color = CCColor3B.White;
            Opacity = 127;            
            Menu = new CCMenu
                (
                    new CCMenuItemLabel(new Label("Retour", CCColor3B.Black), ComeBackToGame),
                    new CCMenuItemLabel(new Label("Quitter", CCColor3B.Black), LeaveGame)
                );
            Menu.AlignItemsVertically(MENU_ALIGN_PADDING);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            AddChild(BackgroungLayer.MENU_BROLY);
            AddChild(Menu);
            AddChild(new Label("Menu", CCColor3B.Black) { Position = new CCPoint(VisibleBoundsWorldspace.MaxX - 15, VisibleBoundsWorldspace.MaxY - 15), AnchorPoint = CCPoint.AnchorUpperRight });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        private void ComeBackToGame(object target)
        {
            SoundPlayer.Instance.PlayButtonClick();
            RemoveFromParent(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        private void LeaveGame(object target)
        {
            SoundPlayer.Instance.PlayButtonClick();
            Application.ExitGame();
        }
    }
}
