using Akka.Actor;
using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Handler.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class ConnectionHandler : GameHandler<ConnectionHandler>,
        IHandle<ClientMessage<IdentificationMessage>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ClientMessage<IdentificationMessage> message)
        {
            message.Client.Send(new IdentificationResultMessage() { Code = IdentificationResultEnum.SUCCESS });
        }
    }
}
