﻿using Akka.Actor;
using Akka.Event;
using HeroesRpg.Protocol;
using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;
using HeroesRpg.Protocol.Impl.Game.Command.Client;
using HeroesRpg.Protocol.Impl.Game.Map.Client;
using HeroesRpg.Protocol.Impl.Selection.Client;
using HeroesRpg.Server.Game.Handler;
using HeroesRpg.Server.Game.Handler.Impl;
using HeroesRpg.Server.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MessageProcessor : TypedActor,
        IHandle<MessageProcessor.ProcessMessage>
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly ILoggingAdapter m_log = Logging.GetLogger(Context);

        /// <summary>
        /// 
        /// </summary>
        public class ProcessMessage
        {
            public GameClient Client { get; }
            public NetMessage Message { get; }
            public ProcessMessage(GameClient c, NetMessage m)
            {
                Client = c;
                Message = m;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ProcessMessage message)
        {
            var netmsg = message.Message;
            var client = message.Client;

            netmsg
                .Match()
                .With<IdentificationMessage>(m => Handle<ConnectionHandler, IdentificationMessage>(client, m))
                .With<PhysicsWorldDataRequestMessage>(m => Handle<MapHandler, PhysicsWorldDataRequestMessage>(client, m))
                .With<PlayerMovementRequestMessage>(m => Handle<CommandHandler, PlayerMovementRequestMessage>(client, m))
                .With<PlayerUseSpellRequestMessage>(m => Handle<CommandHandler, PlayerUseSpellRequestMessage>(client, m))
                .With<PlayerJumpRequestMessage>(m => Handle<CommandHandler, PlayerJumpRequestMessage>(client, m))
                .Default((obj) => m_log.Debug("unhandled msg {0}", obj.GetType().Name));

            m_log.Info("received < {0}", message.Message.GetType().Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="THandler"></typeparam>
        /// <typeparam name="TMessage"></typeparam>
        /// <param name="handler"></param>
        /// <param name="client"></param>
        /// <param name="message"></param>
        private void Handle<THandler, TMessage>(GameClient client, TMessage message)
            where THandler : GameHandler<THandler>, new()
            where TMessage : NetMessage
        {
            GameHandler<THandler>.ActorInstance.Tell(new ClientMessage<TMessage>(client, message));
        }
    }
}
