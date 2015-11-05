using CocosSharp;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Game.World.Entity.Impl.Decoration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using HeroesRpg.Protocol.Game.State.Part;

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
        /// <param name="maxLife"></param>
        public void SetMaxLife(float maxLife)
        {
            MaxLife = maxLife;
            LifeDecoration.Update();
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

            var combatPart = new CombatEntityPart();
            combatPart.FromNetwork(reader);
            UpdateCombatEntityPart(combatPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateCombatEntityPart(CombatEntityPart part)
        {
            SetMaxLife(part.MaxLife);
            SetLife(part.CurrentLife);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        public override void UpdatePart(IEnumerable<StatePart> parts)
        {
            base.UpdatePart(parts);
            var combatPart = parts.FirstOrDefault(part => part.Type == StatePartTypeEnum.COMBAT_ENTITY);
            if (combatPart != null)
                UpdateCombatEntityPart(combatPart as CombatEntityPart);
        }
    }
}
