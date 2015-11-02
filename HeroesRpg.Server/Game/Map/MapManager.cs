using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    partial class MapManager
    {
        public sealed class GetMap { public int ID { get; } public GetMap(int id) { ID = id; } }
        public sealed class MapFound { public IActorRef Map { get; } public MapFound(IActorRef map) { Map = map; } }
        public sealed class MapNotFound { public int ID { get; } public MapNotFound(int id) { ID = id; } }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class MapManager : TypedActor,
        IHandle<MapManager.GetMap>
    {
        public const string NAME_PREFIX = "map-";
        public const int DEFAULT_MAP = 0;
        
        /// <summary>
        /// 
        /// </summary>
        public MapManager()
        {
            Context.ActorOf(MapInstance.Create(0), NAME_PREFIX + "0");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(GetMap message)
        {
            var map = GetMapActor(message.ID);
            if (map.IsNobody())
                map = GetMapActor(DEFAULT_MAP);
            Sender.Tell(new MapFound(map));         
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private IActorRef GetMapActor(int id)
        {
            return Context.Child(NAME_PREFIX + id);
        }
    }
}
