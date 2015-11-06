using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg
{
    public static class ObjectExtensions
    {
        public static Matcher<T> Match<T>(this T obj) where T : class => new Matcher<T>(obj);
    }
}
