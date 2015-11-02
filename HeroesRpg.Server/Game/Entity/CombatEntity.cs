using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HeroesRpg.Server.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CombatEntity : AnimatedEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public bool CombatPartDirty
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
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int CurrentLife
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public CombatEntity()
        {
            SetFixedRotation(true);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeNetworkParts()
        {
            base.InitializeNetworkParts();
            AddNetworkPart(() => CombatPartDirty, () => CombatPartDirty = false, CreateCombatNetworkPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxLife"></param>
        public void SetMaxLife(int maxLife)
        {
            MaxLife = maxLife;
            OnCombatPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentLife"></param>
        public void SetCurrentLife(int currentLife)
        {
            CurrentLife = currentLife;
            OnCombatPartDirty();
        }

        public override void ToNetwork(BinaryWriter writer)
        {
            base.ToNetwork(writer);
            writer.Write(MaxLife);
            writer.Write(CurrentLife);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnCombatPartDirty() => CombatPartDirty = true;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public StatePart CreateCombatNetworkPart() =>
            new CombatEntityPart(MaxLife, CurrentLife);
    }
}
