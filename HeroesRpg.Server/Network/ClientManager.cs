using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Event;
using HeroesRpg.Protocol.Impl.Connection.Server;
using HeroesRpg.Protocol;

namespace HeroesRpg.Server.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClientManager : TypedActor,
        IHandle<ClientManager.AddClient>,
        IHandle<ClientManager.RemoveClient>,
        IHandle<ClientManager.GetClient>,
        IHandle<ClientManager.SendReliable>,
        IHandle<ClientManager.SendUnreliable> 
    {
        public sealed class AddClient
        {
            public GameClient Client { get; }
            public AddClient(GameClient client)
            { Client = client; }
        }
        public sealed class RemoveClient
        {
            public GameClient Client { get; }
            public RemoveClient(GameClient client)
            { Client = client; }
        }
        public sealed class GetClient
        {
            public ulong ClientId { get; }
            public GetClient(uint clientId)
            { ClientId = clientId; }
        }
        public sealed class ClientFound
        {
            public GameClient Client { get; }
            public ClientFound(GameClient client)
            { Client = client; }
        }
        public sealed class ClientNotFound
        {
            public ulong ClientId { get; }
            public ClientNotFound(ulong clientId)
            { ClientId = clientId; }
        }
        public sealed class SendUnreliable : SendMessage
        {
            public SendUnreliable(ulong clientId, NetMessage msg) : base(0, clientId, msg) { }
        }
        public sealed class SendReliable : SendMessage
        {
            public SendReliable(ulong clientId, NetMessage msg) : base(0, clientId, msg) { }
        }
        public abstract class SendMessage
        {
            public byte ChannelId { get; }
            public ulong ClientId { get; }
            public NetMessage NetMsg { get; }
            public SendMessage(byte channelId, ulong clientId, NetMessage msg)
            {
                NetMsg = msg;
                ClientId = clientId;
                NetMsg = msg;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly ILoggingAdapter m_log = Logging.GetLogger(Context);

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<ulong, GameClient> m_clientById;
        
        /// <summary>
        /// 
        /// </summary>
        public ClientManager()
        {
            m_clientById = new Dictionary<ulong, GameClient>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(AddClient message)
        {
            m_clientById.Add(message.Client.ClientId, message.Client);
            m_log.Info("add client : {0}", message.Client.ClientId);

            message.Client.Send(new WelcomeConnectMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(RemoveClient message)
        {
            m_clientById.Remove(message.Client.ClientId);
            m_log.Info("remove client : {0}", message.Client.ClientId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(GetClient message)
        {
            if (m_clientById.ContainsKey(message.ClientId))
                Sender.Tell(new ClientFound(m_clientById[message.ClientId]));
            else
                Sender.Tell(new ClientNotFound(message.ClientId));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(SendReliable message)
        {
            if (m_clientById.ContainsKey(message.ClientId))
                m_clientById[message.ClientId].SendReliable(message.ChannelId, message.NetMsg);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(SendUnreliable message)
        {
            if (m_clientById.ContainsKey(message.ClientId))
                m_clientById[message.ClientId].SendUnreliable(message.ChannelId, message.NetMsg);
        }
    }
}
