using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Layer;
using HeroesRpg.Client.Game.Sound;
using HeroesRpg.Client.Game.Util;
using HeroesRpg.Common.Generic;
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
    /// <typeparam name="TScene"></typeparam>
    public abstract class WrappedScene<TScene> : CCScene
        where TScene : WrappedScene<TScene>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        public static TScene Instance => Singleton<TScene>.Instance;

        /// <summary>
        /// 
        /// </summary>
        public CCEventListenerMouse MouseListener
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CCEventListenerKeyboard KeyboardListener
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="window"></param>
        public WrappedScene() : base(AppDelegate.SharedWindow)
        {
            AddEventListener(MouseListener = new CCEventListenerMouse()
            {
                OnMouseDown = OnMouseDown,
                OnMouseUp = OnMouseUp,
            });
            AddEventListener(KeyboardListener = new CCEventListenerKeyboard()
            {
                OnKeyPressed = OnKeyPressed,
                OnKeyReleased = OnKeyReleased,
            });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public bool IsKeyDown(CCKeys keys) => InputHelper.Instance.IsKeyIn(keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="button"></param>
        /// <returns></returns>
        public bool IsMouseDown(CCMouseButton button) => InputHelper.Instance.IsMousePressed(button);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected virtual void OnKeyPressed(CCEventKeyboard ev)
        {
            InputHelper.Instance.OnKeyPress(ev.Keys);
            switch (ev.Keys)
            {
                case CCKeys.Escape:
                    if (!Children.Any(child => child is MainMenuLayer))
                    {
                        SoundPlayer.Instance.PlayMenuOpened();
                        AddChild(new MainMenuLayer());
                    }
                    else
                    {
                        SoundPlayer.Instance.PlayMenuClosed();
                        RemoveChild(Children.First(child => child is MainMenuLayer));
                    }
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected virtual void OnKeyReleased(CCEventKeyboard ev) => InputHelper.Instance.OnKeyRelease(ev.Keys);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected virtual void OnMouseDown(CCEventMouse ev) => InputHelper.Instance.OnMousePress(ev.MouseButton);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected virtual void OnMouseUp(CCEventMouse ev) => InputHelper.Instance.OnMouseRelease(ev.MouseButton);
    }
}
