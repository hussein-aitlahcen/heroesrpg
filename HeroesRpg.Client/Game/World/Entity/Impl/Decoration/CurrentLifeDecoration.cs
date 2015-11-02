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
    public sealed class CurrentLifeDecoration : EnergyBarDecoration
    {
        /// <summary>
        /// 
        /// </summary>
        public override float BottomMargin
        {
            get
            {
                return 0f;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public override float TopMargin
        {
            get
            {
                return 5f;
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentLife"></param>
        /// <param name="maxLife"></param>
        public CurrentLifeDecoration(Func<float> currentLife, Func<float> maxLife)
            : base(DecorationTypeEnum.LIFE, currentLife, maxLife, 50, 8, CCColor4B.LightGray, CCColor4B.Green, CCColor4B.Black)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            if (Node.Ratio < 0.25f)
            {
                ForegroundColor = CCColor4B.Red;
                TextColor = CCColor4B.Orange;
            }
            else if(Node.Ratio < 0.50f)
            {
                ForegroundColor = CCColor4B.Orange;
                TextColor = CCColor4B.Magenta;
            }
            else
            {
                BackgroundColor = CCColor4B.Gray;
                ForegroundColor = CCColor4B.Green;
            }
            base.Update();
        }
    }
}
