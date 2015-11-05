using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common
{
    public static class Constant
    {
        public const float TICK_RATE_MS = TICK_RATE * 1000;
        public const float TICK_RATE = 1 / 60f;
        public const float UPDATE_RATE_SECOND = 1 / 40f;
        public const float UPDATE_RATE_MS = UPDATE_RATE_SECOND * 1000;
    }
}
