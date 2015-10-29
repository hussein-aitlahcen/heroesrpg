using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity.Impl.Decoration
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class EnergyBarDecoration : EntityDecoration<EnergyBar>
    {
        /// <summary>
        /// 
        /// </summary>
        public CCColor4B BackgroundColor { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public CCColor4B ForegroundColor { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        public CCColor4B TextColor { get; protected set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        public EnergyBarDecoration(DecorationTypeEnum type, Func<float> currentEnergy, Func<float> maxEnergy, float width, float height, CCColor4B background, CCColor4B foreground, CCColor4B textColor)
            : base(type, new EnergyBar(width, height))
        {
            BackgroundColor = background;
            ForegroundColor = foreground;
            TextColor = textColor;

            Node.Initialize(currentEnergy, maxEnergy, () => BackgroundColor, () => ForegroundColor, () => TextColor);
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            Node.Redraw();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void SetBackgroundColor(CCColor4B color)
        {
            BackgroundColor = color;
            Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void SetForegroundColor(CCColor4B color)
        {
            ForegroundColor = color;
            Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="color"></param>
        public void SetTextColor(CCColor4B color)
        {
            TextColor = color;
            Update();
        }
    }
}
