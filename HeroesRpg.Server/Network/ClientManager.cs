using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Akka.Event;
using HeroesRpg.Protocol.Impl.Connection.Server;

namespace HeroesRpg.Server.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ClientManager : TypedActor,
        IHandle<ClientManager.AddClient>,
        IHandle<ClientManager.RemoveClient>,
        IHandle<ClientManager.GetClient>      
    {
        public sealed class AddClient { public GameClient Client { get; } public AddClient(GameClient client) { Client = client; } }
        public sealed class RemoveClient { public GameClient Client { get; } public RemoveClient(GameClient client) { Client = client; } }
        public sealed class GetClient { public uint UniqueID { get; } public GetClient(uint uniqueId) { UniqueID = uniqueId; } }
        public sealed class ClientFound { public GameClient Client { get; } public ClientFound(GameClient client) { Client = client; } }
        public sealed class ClientNotFound { public uint UniqueID { get; } public ClientNotFound(uint uniqueId) { UniqueID = uniqueId; } }

        /// <summary>
        /// 
        /// </summary>
        private readonly ILoggingAdapter m_log = Logging.GetLogger(Context);

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<uint, GameClient> m_clientById;
        
        /// <summary>
        /// 
        /// </summary>
        public ClientManager()
        {
            m_clientById = new Dictionary<uint, GameClient>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(AddClient message)
        {
            m_clientById.Add(message.Client.UniqueID, message.Client);
            m_log.Info("add client : {0}", message.Client.UniqueID);

            message.Client.Send(new WelcomeConnectMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(RemoveClient message)
        {
            m_clientById.Remove(message.Client.UniqueID);
            m_log.Info("remove client : {0}", message.Client.UniqueID);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(GetClient message)
        {
            if (m_clientById.ContainsKey(message.UniqueID))
                Sender.Tell(new ClientFound(m_clientById[message.UniqueID]));
            else
                Sender.Tell(new ClientNotFound(message.UniqueID));
        }
    }
}
