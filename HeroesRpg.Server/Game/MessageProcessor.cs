using Akka.Actor;
using Akka.Event;
using HeroesRpg.Protocol;
using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;
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
        public class ProcessMessage { public GameClient Client { get; } public NetMessage Message { get; } public ProcessMessage(GameClient c, NetMessage m) { Client = c; Message = m; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ProcessMessage message)
        {
            var netmsg = message.Message;
            var client = message.Client;

            var identificationMessage = netmsg as IdentificationMessage;
            var charactersListRequestMessage = netmsg as CharactersListRequestMessage;
            
            var physicsWorldDataRequestMessage = netmsg as PhysicsWorldDataRequestMessage;

            if (identificationMessage != null)
            {
                Handle<ConnectionHandler, IdentificationMessage>(client, identificationMessage); 
            }
            else if(charactersListRequestMessage != null)
            {
            }
            else if(physicsWorldDataRequestMessage != null)
            {
                Handle<MapHandler, PhysicsWorldDataRequestMessage>(client, physicsWorldDataRequestMessage);
            }

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
        private static void Handle<THandler, TMessage>(GameClient client, TMessage message)
            where THandler : GameHandler<THandler>, new()
            where TMessage : NetMessage
        {
            GameHandler<THandler>.ActorInstance.Tell(new ClientMessage<TMessage>(client, message));
        }
    }
}
