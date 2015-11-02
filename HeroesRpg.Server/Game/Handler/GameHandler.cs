using Akka.Actor;
using Akka.Routing;
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
    public abstract class GameHandler<T> : TypedActor 
        where T : GameHandler<T>, new()
    {
        private const int HANDLER_INSTANCES = 10;

        private static IActorRef m_actorRef = GameSystem.Instance.System.ActorOf(Props.Create(() => new T()).WithRouter(new RoundRobinPool(HANDLER_INSTANCES)), typeof(T).Name);

        public static IActorRef ActorInstance => m_actorRef;
    }
}
