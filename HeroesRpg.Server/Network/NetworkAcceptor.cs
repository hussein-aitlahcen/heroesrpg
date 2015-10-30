using Akka.Actor;
using HeroesRpg.Common.Generic;
using HeroesRpg.Network;
using HeroesRpg.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Event;
using System.Threading;
using HeroesRpg.Protocol.Impl.Connection.Server;
using HeroesRpg.Protocol.Impl.Connection.Client;

namespace HeroesRpg.Server.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class NetworkAcceptor : NetServer<GameClient, NetMessage>
    {
        /// <summary>
        /// 
        /// </summary>
        public static NetworkAcceptor Instance => Singleton<NetworkAcceptor>.Instance;

        /// <summary>
        /// 
        /// </summary>
        public NetworkAcceptor() : base(NetMessage.Deserialize)
        { 
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            Initialize(111, 100);

            Task.Factory.StartNew(HearthBeat);
        }

        /// <summary>
        /// 
        /// </summary>
        private void HearthBeat()
        {
            Poll();

            Thread.Sleep(10);

            Task.Factory.StartNew(HearthBeat);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public override void OnConnected(GameClient client)
        {
            Console.WriteLine("client connected");

            client.Send(new WelcomeConnectMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        public override void OnDisconnected(GameClient client)
        {
            Console.WriteLine("client disconnected");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public override void OnMessageReceived(GameClient client, NetMessage message)
        {
            Console.WriteLine("client msg received : " + message.GetType().Name);

            if(message is IdentificationMessage)
            {
                client.Send(new IdentificationResultMessage() { Code = IdentificationResultEnum.SUCCESS });
            }
        }
    }
}
