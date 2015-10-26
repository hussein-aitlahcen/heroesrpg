using CocosSharp;
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
    public class BackgroungLayer : WrappedLayer
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BACKGROUND_FOLDER = "images/backgrounds/";
        public static BackgroungLayer LOADING_BG = new BackgroungLayer("menu/main", 150);
        public static BackgroungLayer LOGIN_GOKU = new BackgroungLayer("login/goku");
        public static BackgroungLayer MENU_BROLY = new BackgroungLayer("menu/broly", 150);

        /// <summary>
        /// 
        /// </summary>
        public CCTexture2D BackgroundTexture
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CCSprite BackgroundSprite
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bgFile"></param>
        public BackgroungLayer(string bgFile, byte opacity = 255): base()
        {
            BackgroundSprite = 
                new CCSprite(BackgroundTexture = 
                    new CCTexture2D(BACKGROUND_FOLDER + bgFile));
            BackgroundSprite.AnchorPoint = CCPoint.AnchorLowerLeft;
            BackgroundSprite.Opacity = opacity;
            Opacity = opacity;
            AddChild(BackgroundSprite);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
        }
    }
}
