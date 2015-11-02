using Akka.Actor;
using Akka.Routing;
using HeroesRpg.Common.Generic;
using HeroesRpg.Server.Game.Map;
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
    public sealed class GameSystem : Singleton<GameSystem>
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MSG_PROC_INSTANCES = 10;

        /// <summary>
        /// 
        /// </summary>
        public ActorSystem System
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IActorRef ClientMgr
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IActorRef MessageProc
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public IActorRef MapMgr
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Initialize()
        {
            System = ActorSystem.Create("game-system");

            InitializeActors();
            InitializeNetwork();          
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeActors()
        {
            ClientMgr = System.ActorOf<ClientManager>("client-manager");
            MessageProc = System.ActorOf(Props.Create(() => new MessageProcessor()).WithRouter(new RoundRobinPool(MSG_PROC_INSTANCES)), "message-proc");
            MapMgr = System.ActorOf<MapManager>("map-manager");
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeNetwork()
        {
            NetworkAcceptor.Instance.Initialize();
        }
    }
}
