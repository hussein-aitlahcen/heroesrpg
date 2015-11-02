using HeroesRpg.Client.Data.Actor;
using HeroesRpg.Client.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesRpg.Protocol.Enum;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public partial class DragonBallHero
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static DragonBallHeroData GetHeroDataById(int id)
        {
            return GetHeroDataById((DragonBallHeroEnum)id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hero"></param>
        /// <returns></returns>
        public static DragonBallHeroData GetHeroDataById(DragonBallHeroEnum hero)
        {
            switch (hero)
            {
                case DragonBallHeroEnum.BROLY:
                    return ActorManager.Instance.GetActorByName<DragonBallHeroData>(DragonBallHeroData.BROLY);
                default:
                    throw new UnknowHeroBreedException("unknow hero id : " + (int)hero);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class DragonBallHero : Hero
    {
        /// <summary>
        /// 
        /// </summary>
        public override ActorData BreedData
        {
            get
            {
                return m_data;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private DragonBallHeroData m_data;

        /// <summary>
        /// 
        /// </summary>
        public DragonBallHero()
            : base(HeroTypeEnum.DRAGON_BALL)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            base.FromNetwork(reader);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroId"></param>
        public override void SetHeroId(int heroId)
        {
            base.SetHeroId(heroId);
            m_data = GetHeroDataById((DragonBallHeroEnum)HeroId);
        }
    }
}
