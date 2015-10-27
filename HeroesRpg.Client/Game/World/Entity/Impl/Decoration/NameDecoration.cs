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
    public sealed class NameDecoration : LabeledDecoration
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="color"></param>
        /// <param name="scale"></param>
        public NameDecoration(string name, CCColor3B color) 
            : base(DecorationTypeEnum.NAME,  name, color)
        {
        }
    }
}
