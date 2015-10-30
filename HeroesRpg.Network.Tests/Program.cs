using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesRpg.Network.Tests
{
    class Program
    {
        static void Main(string[] args)
        {
            var host = new ENet.Host();
            host.InitializeServer(111, 500);            
            Task.Factory.StartNew(server, host);

            var peers = new ENet.Host[500];
            for (int i = 0; i < peers.Length; i++)
            {
                var peer = (peers[i] = new ENet.Host());
                peer.InitializeClient(1);
                peer.Connect("localhost", 111, 0);
                Task.Factory.StartNew(client, peer);
            }

            Console.Read();
        }

        private static void server(object state)
        {
            var host = (ENet.Host)state;
            ENet.Event e;
            host.Service(100, out e);
            switch (e.Type)
            {
                case ENet.EventType.Connect:
                    var packet = new ENet.Packet();
                    packet.Initialize(Encoding.Default.GetBytes("Bonsoir"), ENet.PacketFlags.UnreliableFragment);
                    e.Peer.Send(0, packet);
                    break;

                case ENet.EventType.Receive:
                    e.Peer.Send(0, e.Packet);
                    break;
            }
            Task.Factory.StartNew(server, host);
        }

        static int k = 1;
        private static void client(object state)
        {
            var peer = (ENet.Host)state;
            ENet.Event e;
            if (peer.Service(5, out e))
            {
                switch (e.Type)
                {
                    case ENet.EventType.Connect:
                        break;
                    case ENet.EventType.Receive:
                        Console.WriteLine(Interlocked.Increment(ref k) + " : " + Encoding.Default.GetString(e.Packet.ToArray()));
                        break;
                    case ENet.EventType.Disconnect:
                        return;
                }
                Task.Factory.StartNew(client, peer);
            }
        }
    }
}
