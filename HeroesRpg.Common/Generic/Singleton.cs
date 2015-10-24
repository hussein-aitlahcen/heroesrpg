using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Generic
{
    public class Singleton<T>
        where T : class, new()
    {
        protected static ILog m_log = LogManager.GetLogger(typeof(T));
        private static T m_instance = new T();
        public static T Instance => m_instance;
    }
}
