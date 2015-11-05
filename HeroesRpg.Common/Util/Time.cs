using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Util
{
    public static class Time
    {
        public static long TotalMilliseconds => (long)(DateTime.Now - new DateTime(1970, 1, 1)).TotalMilliseconds;
    }
}
