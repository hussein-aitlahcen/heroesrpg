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
    public class Label : CCLabel
    {
        /// <summary>
        /// 
        /// </summary>
        public const string FONT = "fonts/Segoe_UI_15_Bold";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="size"></param>
        /// <param name="color"></param>
        public Label(string message, CCColor3B color) : base(message, FONT, 15, CCLabelFormat.SpriteFont)
        {
            Color = color;
            AnchorPoint = CCPoint.AnchorUpperLeft;
        }
    }
}
