using HeroesRpg.Client.Game.Graphic.Scene;
using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SceneLoader : Singleton<SceneLoader>
    {
        /// <summary>
        /// 
        /// </summary>
        public void LoadLoginScene()
        {
            AppDelegate.SharedWindow.RunWithScene(LoginScene.Instance);
        }
    }
}
