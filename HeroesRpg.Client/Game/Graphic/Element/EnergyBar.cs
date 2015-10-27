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
        public float Ratio { get { return CurrentEnergy / Math.Max(1, MaxEnergy); } }
        public float Width { get; set; }
        public float Height { get; set; }   
        public float MidWidth { get { return Width / 2; } }
        public float MidHeight { get { return Height / 2; } }
        public CCColor4B Foreground { get; set; }
        public CCColor4B Background { get; set; }

        private CCDrawNode ForegroundNode;
        
        public EnergyBar(int currentEnergy, int maxEnergy, float width, float height, CCColor4B bg, CCColor4B fg)
        {
            CurrentEnergy = currentEnergy;
            MaxEnergy = maxEnergy;
            ForegroundNode = new CCDrawNode();
            Background = bg;
            Foreground = fg;
            Width = width;
            Height = height;

            AddChild(ForegroundNode);

            Draw();       
        }

        protected void Draw()
        {
            Clear();
            DrawRect(new CCRect(PositionX - 2, PositionY - 2, Width + 4, Height + 4), Background);
            DrawRect(new CCRect(0, 0, Width * Ratio, Height), Foreground);
        }
    }
}
