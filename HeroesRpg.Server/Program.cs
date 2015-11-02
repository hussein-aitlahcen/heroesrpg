using HeroesRpg.Server.Game;
using HeroesRpg.Server.Network;
using log4net.Config;
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
            XmlConfigurator.Configure();

            GameSystem.Instance.Initialize();

            Console.ReadLine();
        }
    }
}
