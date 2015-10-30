using HeroesRpg.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Network
{
    public interface IGameFrame
    {
        bool ProcessMessage(NetMessage message);
    }
}
