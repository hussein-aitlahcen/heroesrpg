using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Scene
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoginScene : WrappedScene<LoginScene>
    {
        /// <summary>
        /// 
        /// </summary>
        public const float LOADING_CHECK_INTERVAL = 0.3f;

        /// <summary>
        /// 
        /// </summary>
        public LoadingLayer LoadingLayer
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public LoginLayer LoginLayer
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public LoginScene()
        {
            AddChild(LoginLayer = new LoginLayer());
            AddChild(LoadingLayer = new FakeLoadingLayer());
            Schedule(Update, LOADING_CHECK_INTERVAL);
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);
            if(LoadingLayer.IsDoneLoading)
            {
               RemoveChild(LoadingLayer);
            }
        }
    }
}
