using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Game.World.Entity.Impl.Decoration;
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
    public abstract class CombatEntity : AnimatedEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public const int LIFE_LABEL_HEIGHT = 30;

        /// <summary>
        /// 
        /// </summary>
        public int CurrentLife
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int MaxLife
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public NameDecoration LifeDecoration
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CombatEntity(int id) : base(id)
        {
            AddDecoration(new CurrentLifeDecoration(CurrentLife, MaxLife));
        }
    }
}
