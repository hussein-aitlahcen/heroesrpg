using HeroesRpg.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            NetworkAcceptor.Instance.Initialize();

            Console.ReadLine();
        }
    }
}
