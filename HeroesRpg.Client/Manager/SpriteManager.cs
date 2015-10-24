using CocosSharp;
using HeroesRpg.Client.Data.Texture;
using HeroesRpg.Common.Manager;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Manager
{
    /// <summary>
    /// 
    /// </summary>
    [ManagerDefinition(ManagerEnum.SPRITE_MANAGER)]
    public sealed class SpriteManager : Manager<SpriteManager>
    {
        public const string ROOT = ".\\Content\\";
        public const string SPRITES_PATH = ROOT + "sprites\\";
        public static string[] SUFFIXES = { "\\sprites.plist" };
        
        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            LoadSprites();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadSprites()
        {
            foreach (var file in Directory.GetFiles(SPRITES_PATH, "*.plist", SearchOption.AllDirectories))
            {
                var key = NormalizeKey(file);
                var content = NormalizeContentPath(file);
                SpriteSheetCache.Instance.AddSpriteSheet(key, content);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string NormalizeKey(string file)
        {
            string cleanedName = file;
            foreach (var suffix in SUFFIXES)
                cleanedName = cleanedName.Replace(suffix, string.Empty);
            return cleanedName.Replace(SPRITES_PATH, string.Empty).Replace("\\", ".");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        private string NormalizeContentPath(string file) => file.Replace(ROOT, string.Empty);
    }
}
