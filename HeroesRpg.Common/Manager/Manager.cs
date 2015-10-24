using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Manager
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class Manager<T> : Singleton<T>, IManager
        where T : Manager<T>, new()
    { 
        /// <summary>
        /// 
        /// </summary>
        public bool IsInitialized
        {
            get;
            private set;
        }

        /// <summary>
        /// Sould be overriden
        /// </summary>
        public virtual void Initialize()
        {
            IsInitialized = true;
        }
    }
}
