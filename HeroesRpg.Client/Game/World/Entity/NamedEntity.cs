using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NamedEntity : AnimatedEntity
    {
        public const int NAME_LABEL_HEIGHT = 5;

        /// <summary>
        /// 
        /// </summary>
        public string CustomName
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Label NameLabel
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public NamedEntity(int id, string name) : base(id)
        {
            CustomName = name;
            AddChild(NameLabel = new Label(name, CCColor3B.Black)
            {
                AnchorPoint = CCPoint.AnchorMiddle,
                Scale = 0.40f,
            });
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override void OnFrameChanged()
        {
            NameLabel.Position = new CCPoint(PositionX + (ContentSize.Width / 2), PositionY + ContentSize.Height + NAME_LABEL_HEIGHT);
        }
    }
}
