using CocosDenshion;
using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Sound
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class SoundPlayer : Singleton<SoundPlayer>
    {
        /// <summary>
        /// 
        /// </summary>
        public const string SOUNDS_PATH = "sounds/";

        /// <summary>
        /// 
        /// </summary>
        public const string BUTTON_CLICK = "common/button_click";
        public const string MENU_OPENED = "common/menu_open";
        public const string MENU_CLOSED = "common/menu_close";

        /// <summary>
        /// 
        /// </summary>
        public SoundPlayer()
        {
            CCSimpleAudioEngine.SharedEngine.EffectsVolume = 50;
            CCSimpleAudioEngine.SharedEngine.BackgroundMusicVolume = 50;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sound"></param>
        public void PlaySound(string sound)
        {
            CCSimpleAudioEngine.SharedEngine.PlayEffect(SOUNDS_PATH + sound);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayButtonClick()
        {
            PlaySound(BUTTON_CLICK);
        }

        /// <summary>
        /// 
        /// </summary>
        public void PlayMenuOpened()
        {
            PlaySound(MENU_OPENED);
        }
        
        /// <summary>
        /// 
        /// </summary>
        public void PlayMenuClosed()
        {
            PlaySound(MENU_CLOSED);
        }
    }
}
