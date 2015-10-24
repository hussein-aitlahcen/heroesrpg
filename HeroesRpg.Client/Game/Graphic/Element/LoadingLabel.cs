using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Element
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class LoadingLabel : Label
    {
        /// <summary>
        /// 
        /// </summary>
        public const float INTERVAL = 0.5f;

        /// <summary>
        /// 
        /// </summary>
        private int m_points;

        /// <summary>
        /// 
        /// </summary>
        private string m_message;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public LoadingLabel(string message, CCColor3B color) : base(message, color)
        {
            Color = color;
            m_message = message;
            AnchorPoint = CCPoint.AnchorUpperLeft;
            Schedule(UpdateLoading, INTERVAL);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delta"></param>
        private void UpdateLoading(float delta)
        {
            Text = m_message + string.Concat(Enumerable.Repeat(".", m_points = (m_points + 1) % 4));
        }
    }
}
