using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Element
{
    public enum lineTypes
    {
        LINE_TEMP = 1,
        LINE_DASHED = 2,
        LINE_NONE = 3
    }

    /// <summary>
    /// 
    /// </summary>
    public class EnergyBar : CCDrawNode
    {
        public const float UPDATE_INTERVAL = 0.05f;

        public int CurrentEnergy { get; private set; }  
        public int MaxEnergy { get; private set; }      
        public float Ratio { get { return CurrentEnergy / (float)Math.Max(1, MaxEnergy); } }
        public float Width { get; set; }
        public float Height { get; set; }   
        public float MidWidth { get { return Width / 2; } }
        public float MidHeight { get { return Height / 2; } }
        public CCColor4B Foreground { get; set; }
        public CCColor4B Background { get; set; }
        public CCColor4B TextColor { get; set; }

        private CCDrawNode ForegroundNode;
        private Label Text { get; set; }

        public EnergyBar(int currentEnergy, int maxEnergy, float width, float height, CCColor4B bg, CCColor4B fg, CCColor4B textColor)
        {

            CurrentEnergy = currentEnergy;
            MaxEnergy = maxEnergy;
            ForegroundNode = new CCDrawNode();
            Background = bg;
            Foreground = fg;
            TextColor = textColor;
            Width = width;
            Height = height;

            AddChild(ForegroundNode);

            Text = new Label($"{CurrentEnergy}/{MaxEnergy}", new CCColor3B(TextColor));
            Text.PositionX = PositionX + MidWidth;
            Text.PositionY = PositionY + MidHeight + 1;
            Text.AnchorPoint = CCPoint.AnchorMiddle; 
            Text.Scale = 0.55f;

            AddChild(Text);

            Draw();       
        }

        protected void Draw()
        {
            Clear();

            DrawRect(new CCRect(PositionX - 2, PositionY - 2, Width + 4, Height + 4), CCColor4B.LightGray, 0.5f, Background);
            //DrawLine(new CCPoint(PositionX - 2, PositionY - 2), new CCPoint(PositionX + Width + 2, PositionY - 2), Background);
            //DrawLine(new CCPoint(PositionX - 2, PositionY + Height + 2), new CCPoint(PositionX + Width + 2, PositionY + Height + 2), Background);

            //DrawLine(new CCPoint(PositionX - 2, PositionY - 2), new CCPoint(PositionX - 2, PositionY + Height + 2), Background);
            //DrawLine(new CCPoint(PositionX + Width + 2, PositionY - 2), new CCPoint(PositionX + Width + 2, PositionY + Height + 2), Background);


            //DrawString((int)(PositionX + MidWidth), (int)(PositionY + MidHeight), $"{CurrentEnergy}/{MaxEnergy}");
            DrawRect(new CCRect(0, 0, Width * Ratio, Height), Foreground);
        }
    }
}
