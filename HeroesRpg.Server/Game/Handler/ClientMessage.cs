using HeroesRpg.Protocol;
using HeroesRpg.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Handler
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ClientMessage<T> where T : NetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public GameClient Client
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        public T Message
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="message"></param>
        public ClientMessage(GameClient client, T message)
        {
            Client = client;
            Message = message;
        }
    }
}
