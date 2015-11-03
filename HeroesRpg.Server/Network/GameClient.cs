using Akka.Event;
using HeroesRpg.Network;
using HeroesRpg.Protocol;
using HeroesRpg.Server.Game.Entity;
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
        public GameObject ControlledObject
        {
            get; set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(NetMessage message)
        {
            Send(message.Serialize());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        public void SendUnreliable(NetMessage message)
        {
            SendUnreliable(1, message.Serialize());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        public void SendUnreliable(byte channelId, NetMessage message)
        {
            SendUnreliable(channelId, message.Serialize());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="message"></param>
        public void SendReliable(byte channelId, NetMessage message)
        {
            SendReliable(channelId, message.Serialize());
        }
    }
}
