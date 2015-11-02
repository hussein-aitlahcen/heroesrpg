using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Data.Actor
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class DragonBallHeroData : HeroData
    {
        /// <summary>
        /// 
        /// </summary>
        public const string BROLY = "broly";
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fullName"></param>
        public DragonBallHeroData(string name, string fullName) : base(name, fullName)
        {
        }
    }
}
