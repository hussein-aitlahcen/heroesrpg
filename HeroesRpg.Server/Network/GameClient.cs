using Akka.Event;
using HeroesRpg.Network;
using HeroesRpg.Protocol;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameClient : NetServerClient
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILog m_log = LogManager.GetLogger(typeof(GameClient));

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(NetMessage message)
        {
            m_log.Info("sent > " + message.GetType().Name);
            Send(message.Serialize());
        }
    }
}
