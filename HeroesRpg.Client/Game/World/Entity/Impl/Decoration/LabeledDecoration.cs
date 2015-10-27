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
    public abstract class LabeledDecoration : EntityDecoration<Label>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public LabeledDecoration(DecorationTypeEnum type, string text, CCColor3B color) 
            : base(type, new Label(text, color) { Scale = 1f })
        {
        }        
    }
}
