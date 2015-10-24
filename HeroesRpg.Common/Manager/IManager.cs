using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Manager
{
    public interface IManager
    {
        bool IsInitialized { get; }
        void Initialize();
    }
}
