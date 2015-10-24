using CocosSharp;
using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Data.Texture
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SpriteSheetCache : Singleton<SpriteSheetCache>
    {
        private Dictionary<string, CCSpriteSheet> m_spriteSheets;

        /// <summary>
        /// 
        /// </summary>
        public SpriteSheetCache()
        {
            m_spriteSheets = new Dictionary<string, CCSpriteSheet>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="plistFile"></param>
        /// <returns></returns>
        public CCSpriteSheet AddSpriteSheet(string key, string plistFile)
        {
            return m_spriteSheets[key] = new CCSpriteSheet(plistFile);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public CCSpriteSheet this[string key] => m_spriteSheets[key];

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<string> Keys => m_spriteSheets.Keys;

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<CCSpriteSheet> Values => m_spriteSheets.Values;
    }
}
