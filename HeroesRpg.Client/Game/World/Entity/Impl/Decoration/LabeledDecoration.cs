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
        public Func<string> TextHolder
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Func<CCColor3B> ColorHolder
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="text"></param>
        /// <param name="color"></param>
        public LabeledDecoration(DecorationTypeEnum type, Func<string> text, Func<CCColor3B> color) 
            : base(type, new Label(text(), color()))
        {
            TextHolder = text;
            ColorHolder = color;
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Update()
        {
            base.Update();
            Node.Text = TextHolder();
            Node.Color = ColorHolder();
        }
    }
}
