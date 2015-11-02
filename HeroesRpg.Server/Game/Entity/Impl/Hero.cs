using System;
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
            shape.SetAsBox(GetPointToMeter(55 / 2), GetPointToMeter(90 / 2));
            return shape;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            base.ToNetwork(writer);
            ToNetworkHeroPart(writer);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void ToNetworkHeroPart(BinaryWriter writer)
        {
            writer.Write(HeroId);
            writer.Write(PlayerName);
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
                PlayerName);
    }
}
