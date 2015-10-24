using CocosSharp;
using HeroesRpg.Client.Data.Texture;
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
    public abstract class ActorData
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string FullName
        {
            get;
            private set;
        }

        /// <summary>
        /// Fetch the character sprite sheet from the cache
        /// </summary>
        public CCSpriteSheet SpriteSheet => SpriteSheetCache.Instance[FullName];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public ActorData(string name, string fullName)
        {
            Name = name;
            FullName = fullName;
        }
    }
}
