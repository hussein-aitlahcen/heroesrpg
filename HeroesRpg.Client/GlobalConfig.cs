using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GlobalConfig : Singleton<GlobalConfig>
    {
        public const int PTM_RATIO = 32;
        public const string GAME_HOST = "localhost";
        public const int GAME_PORT = 111;
    }
}
