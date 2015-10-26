using System.Reflection;
using Microsoft.Xna.Framework;
using CocosSharp;
using CocosDenshion;
using HeroesRpg.Client.Data.Texture;
using HeroesRpg.Common.Manager;
using HeroesRpg.Client.Game.Graphic;
using EmptyKeys.UserInterface;
using Microsoft.Xna.Framework.Graphics;
using EmptyKeys.UserInterface.Themes;

namespace HeroesRpg.Client
{
    public class AppDelegate : CCApplicationDelegate
    {
        public static CCApplication SharedApplication;
        public static CCWindow SharedWindow;

        public override void ApplicationDidFinishLaunching(CCApplication application, CCWindow mainWindow)
        {
            SharedApplication = application;
            SharedWindow = mainWindow;

            application.ContentRootDirectory = "Content";
            var windowSize = mainWindow.WindowSizeInPixels;

            var desiredWidth = 1024.0f;
            var desiredHeight = 768.0f;

            CocosSharpEngine engine = new CocosSharpEngine(application.GraphicsDevice, (int)desiredWidth, (int)desiredHeight);
            
            // This will set the world bounds to be (0,0, w, h)
            // CCSceneResolutionPolicy.ShowAll will ensure that the aspect ratio is preserved
            CCScene.SetDefaultDesignResolution(desiredWidth, desiredHeight, CCSceneResolutionPolicy.ShowAll);

            mainWindow.AllowUserResizing = false;
            //mainWindow.AllowUserResizing = false;

            //// Determine whether to use the high or low def versions of our images
            //// Make sure the default texel to content size ratio is set correctly
            //// Of course you're free to have a finer set of image resolutions e.g (ld, hd, super-hd)
            //if (desiredWidth < windowSize.Width)
            //{
            //    application.ContentSearchPaths.Add("hd");
            //    CCSprite.DefaultTexelToContentSizeRatio = 2.0f;
            //}
            //else
            //{
            //    application.ContentSearchPaths.Add("ld");
            //    CCSprite.DefaultTexelToContentSizeRatio = 1.0f;
            //}

            ResourceDictionary.DefaultDictionary = Light.GetThemeDictionary();
            SpriteFont font = CCContentManager.SharedContentManager.Load<SpriteFont>("fonts/Segoe_UI_10_Regular");
            FontManager.DefaultFont = Engine.Instance.Renderer.CreateFont(font);
            FontManager.Instance.LoadFonts(CCContentManager.SharedContentManager, "fonts");

            SuperManager.Instance.InitializeManagers(typeof(Manager.ActorManager).Namespace);
            SceneLoader.Instance.LoadLoginScene();
        }

        public override void ApplicationDidEnterBackground(CCApplication application)
        {
        }

        public override void ApplicationWillEnterForeground(CCApplication application)
        {
        }
    }
}