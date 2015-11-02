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
    public class EnergyBar : CCDrawNode
    {
        public Func<float> CurrentEnergy { get; private set; }  
        public Func<float> MaxEnergy { get; private set; }      
        public float Ratio { get { return CurrentEnergy() / Math.Max(1, MaxEnergy()); } }
        public float Width { get; set; }
        public float Height { get; set; }   
        public float MidWidth { get { return Width / 2; } }
        public float MidHeight { get { return Height / 2; } }
        public Func<CCColor4B> Foreground { get; private set; }
        public Func<CCColor4B> Background { get; private set; }
        public Func<CCColor4B> TextColor { get; private set; }

        private Label TextLabel { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentEnergy"></param>
        /// <param name="maxEnergy"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <param name="bg"></param>
        /// <param name="fg"></param>
        /// <param name="textColor"></param>
        public EnergyBar(float width, float height, Func<float> currentEnergy, Func<float> maxEnergy, Func<CCColor4B> bg, Func<CCColor4B> fg, Func<CCColor4B> textColor)
            : this(width, height)
        {
            Initialize(currentEnergy, maxEnergy, bg, fg, textColor); 
        }

        /// <summary>
        /// 
        /// </summary>
        public EnergyBar(float width, float height)
        {
            Width = width;
            Height = height;

            TextLabel = new Label("?/?", CCColor3B.White);
            TextLabel.PositionX = PositionX + MidWidth;
            TextLabel.PositionY = PositionY + MidHeight + 0.5f;
            TextLabel.AnchorPoint = CCPoint.AnchorMiddle;
            TextLabel.Scale = 0.50f;

            AddChild(TextLabel);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentEnergy"></param>
        /// <param name="maxEnergy"></param>
        /// <param name="bg"></param>
        /// <param name="fg"></param>
        /// <param name="textColor"></param>
        public void Initialize(Func<float> currentEnergy, Func<float> maxEnergy, Func<CCColor4B> bg, Func<CCColor4B> fg, Func<CCColor4B> textColor)
        {
            CurrentEnergy = currentEnergy;
            MaxEnergy = maxEnergy;
            Background = bg;
            Foreground = fg;
            TextColor = textColor;

            Redraw();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Redraw()
        {           
            const float padding = 1.5f;
            const float doblepadding = 2 * padding;

            Cleanup();

            var txtColor = TextColor();
            var bgColor = Background();
            var fgColor = Foreground();

            TextLabel.Color = new CCColor3B(txtColor);
            TextLabel.Text = $"{Math.Round(CurrentEnergy(), 1)}/{MaxEnergy()}";

            DrawRect(new CCRect(-padding, -padding, Width + doblepadding, Height + doblepadding), bgColor, 0.5f, CCColor4B.Blue);
            DrawRect(new CCRect(0, 0, Width * Math.Min(1, Ratio), Height), fgColor);
        }
    }
}
