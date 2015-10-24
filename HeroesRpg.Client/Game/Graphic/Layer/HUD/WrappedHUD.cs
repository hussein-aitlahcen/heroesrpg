using CocosSharp;
using EmptyKeys.UserInterface.Controls;
using HeroesRpg.Common.Generic;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Layer.HUD
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WrappedHUD<TWrapper, TRootUI> : WrappedLayer
        where TWrapper : WrappedHUD<TWrapper, TRootUI>, new()
        where TRootUI : UIRoot, new()
    {
        /// <summary>
        /// 
        /// </summary>
        protected static readonly ILog m_log = LogManager.GetLogger(typeof(WrappedHUD<TWrapper, TRootUI>));

        /// <summary>
        /// 
        /// </summary>
        public static TWrapper Instance => Singleton<TWrapper>.Instance;

        /// <summary>
        /// 
        /// </summary>
        private TRootUI m_rootUI;
        private float m_gameTime;
        private CCCustomCommand m_renderCommand;

        /// <summary>
        /// 
        /// </summary>
        public TRootUI UI => m_rootUI;

        /// <summary>
        /// 
        /// </summary>
        public WrappedHUD()
        {
            Opacity = 0;
            m_rootUI = new TRootUI();
            m_renderCommand = new CCCustomCommand(RenderUI);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            Schedule(UpdateUI);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frameTimeInSeconds"></param>
        private void UpdateUI(float frameTimeInSeconds)
        {
            m_gameTime = frameTimeInSeconds * 1000;
            m_rootUI.UpdateInput(m_gameTime);
            m_rootUI.UpdateLayout(m_gameTime);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="worldTransform"></param>
        protected override void VisitRenderer(ref CCAffineTransform worldTransform)
        {
            base.VisitRenderer(ref worldTransform);
            Renderer.AddCommand(m_renderCommand);
        }

        /// <summary>
        /// 
        /// </summary>
        private void RenderUI()
        {
            m_rootUI.Draw(m_gameTime);
        }
    }
}
