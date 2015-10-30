using HeroesRpg.Protocol.Impl.Connection.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var msg = new IdentificationMessage()
            {
                Username = "Salut",
                Password = "ok",
            };

            var bytes = msg.Serialize();

            var msg1 = NetMessage.Deserialize(bytes);
        }
    }
}
