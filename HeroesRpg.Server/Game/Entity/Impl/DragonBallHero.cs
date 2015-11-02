using Box2D.Collision.Shapes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using HeroesRpg.Protocol.Enum;

namespace HeroesRpg.Server.Game.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DragonBallHero : Hero
    {
        /// <summary>
        /// 
        /// </summary>
        public DragonBallHero() : base(HeroTypeEnum.DRAGON_BALL)
        {
        }
    }
}
