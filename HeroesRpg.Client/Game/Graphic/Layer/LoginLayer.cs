using CocosSharp;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Debug;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Input;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Game.Graphic.Layer.HUD;
using Microsoft.Xna.Framework.Graphics;
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
    public sealed class LoginLayer : WrappedLayer
    {
        /// <summary>
        /// 
        /// </summary>
        public LoginLayer()
        {
            Color = CCColor3B.Orange;
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();
            AddChild(BackgroungLayer.LOGIN_GOKU);
            AddChild(LoginHUD.Instance);
            AddChild(new Label("Connexion", CCColor3B.Black) { Position = new CCPoint(VisibleBoundsWorldspace.MidX, VisibleBoundsWorldspace.MaxY - 15), AnchorPoint = CCPoint.AnchorMiddleTop });
        }
    }
}
