using Akka.Actor;
using HeroesRpg.Protocol.Impl.Game.Command.Client;
using HeroesRpg.Server.Game.Map;
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
    public sealed class CommandHandler : GameHandler<CommandHandler>,
        IHandle<ClientMessage<PlayerMovementRequestMessage>>,
        IHandle<ClientMessage<PlayerUseSpellRequestMessage>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ClientMessage<PlayerUseSpellRequestMessage> message)
        {
            message
                .Client
                .ControlledObject
                .Map
                .Tell(new MapInstance.SpellUseCommand(message.Client.ControlledObject, message.Message.SpellId, message.Message.Angle));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ClientMessage<PlayerMovementRequestMessage> message)
        {
            message
                .Client
                .ControlledObject
                .Map
                .Tell(new MapInstance.MovementCommand(message.Client.ControlledObject, message.Message.MovementX, message.Message.MovementY));
        }
    }
}
