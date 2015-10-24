using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class UnknowHeroBreedException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public UnknowHeroBreedException(string message) : base(message) { }
    }
}
