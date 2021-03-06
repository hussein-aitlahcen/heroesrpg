﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using System.IO;
using HeroesRpg.Protocol.Enum;
using HeroesRpg.Protocol.Game.State.Part.Impl;

namespace HeroesRpg.Server.Game.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public abstract partial class Hero : CombatEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public bool HeroPartDirty
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override EntityTypeEnum Type => EntityTypeEnum.HERO;

        /// <summary>
        /// 
        /// </summary>
        public HeroTypeEnum HeroType
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int HeroId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string PlayerName
        {
            get;
            private set;
        }
                        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroType"></param>
        /// <param name="id"></param>
        public Hero(HeroTypeEnum type)
        {
            PlayerName = "[???]";

            SetNetworkType(GameObjectNetworkType.SHARE_FULL);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeNetworkParts()
        {
            base.InitializeNetworkParts();
            AddNetworkPart(() => HeroPartDirty, () => HeroPartDirty = false, CreateHeroNetworkPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(PhysicWidth / 2, PhysicHeight / 2);
            return shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            base.ToNetwork(writer);
            CreateHeroNetworkPart().ToNetwork(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroType"></param>
        public virtual void SetHeroType(int heroType)
        {
            HeroType = (HeroTypeEnum)heroType;
            OnHeroPartDirty();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroId"></param>
        public virtual void SetHeroId(int heroId)
        {
            HeroId = heroId;
            OnHeroPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public virtual void SetPlayerName(string name)
        {
            PlayerName = name;
            OnHeroPartDirty();
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnHeroPartDirty()
        {
            HeroPartDirty = true;
        }

        /// <summary>
        /// 
        /// </summary>
        public HeroEntityPart CreateHeroNetworkPart() =>
            new HeroEntityPart(
                HeroId,
                (int)HeroType,
                PlayerName);
    }
}
