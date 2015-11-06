using CocosSharp;
using HeroesRpg.Client.Data.Actor;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using HeroesRpg.Client.Game.World.Entity.Impl.Decoration;
using System.IO;
using HeroesRpg.Protocol.Game.State.Part.Impl;
using HeroesRpg.Protocol.Game.State.Part;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public enum HeroTypeEnum
    {
        DRAGON_BALL = 0,
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract partial class Hero : CombatEntity
    {
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
        public CCColor3B PlayerNameColor
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract ActorData BreedData
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public NameDecoration PlayerNameDecoration
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public override CCSpriteSheet SpriteSheet =>
            BreedData.SpriteSheet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroType"></param>
        /// <param name="id"></param>
        public Hero(HeroTypeEnum type)
        {
            PlayerName = "PlaceHolder";
            PlayerNameColor = CCColor3B.Black;
            HeroType = type;

            AddDecoration(PlayerNameDecoration = new NameDecoration(() => PlayerName, () => PlayerNameColor));
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
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);

            var heroPart = new HeroEntityPart();
            heroPart.FromNetwork(reader);
            UpdateHeroPart(heroPart);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void UpdateHeroPart(HeroEntityPart part)
        {
            SetHeroId(part.HeroId);
            SetHeroType(part.HeroType);
            SetPlayerName(part.PlayerName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        public override void UpdatePart(IEnumerable<StatePart> parts)
        {
            base.UpdatePart(parts);
            var heroPart = parts.FirstOrDefault(part => part.Type == StatePartTypeEnum.HERO_ENTITY);
            if (heroPart != null)
                UpdateHeroPart(heroPart as HeroEntityPart);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroType"></param>
        public virtual void SetHeroType(int heroType)
        {
            HeroType = (HeroTypeEnum)heroType;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroId"></param>
        public virtual void SetHeroId(int heroId)
        {
            HeroId = heroId;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public virtual void SetPlayerName(string name)
        {
            PlayerName = name;
            PlayerNameDecoration.Update();
            ComputeDecorationPositions();
        }
    }
}
