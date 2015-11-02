using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Game.World.Entity.Impl.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        public float CurrentLife
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float MaxLife
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CurrentLifeDecoration LifeDecoration
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public CombatEntity()
        {
            MaxLife = 100;
            CurrentLife = 150;
            AddDecoration(LifeDecoration = new CurrentLifeDecoration(() => CurrentLife, () => MaxLife));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);

            SetLife(CurrentLife * 0.99f);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="life"></param>
        public void SetLife(float life)
        {
            CurrentLife = life;
            LifeDecoration.Update();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);
            UpdateCombatEntityPart(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void UpdateCombatEntityPart(BinaryReader reader)
        {

        }
    }
}
