using Akka.Actor;
using HeroesRpg.Protocol.Enum;
using HeroesRpg.Protocol.Impl.Game.Map.Client;
using HeroesRpg.Protocol.Impl.Game.Map.Server;
using HeroesRpg.Server.Game.Entity.Impl;
using HeroesRpg.Server.Game.Map;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Handler.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class MapHandler : GameHandler<MapHandler>,
        IHandle<ClientMessage<PhysicsWorldDataRequestMessage>>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Handle(ClientMessage<PhysicsWorldDataRequestMessage> message)
        {
            var testObj = new DragonBallHero();
            testObj.SetPlayerName(message.Client.ClientId.ToString());
            testObj.SetId((int)message.Client.ClientId);
            testObj.SetFixedRotation(true);
            testObj.SetControllerId(message.Client.ClientId);
            testObj.SetHeroId((int)DragonBallHeroEnum.BROLY);
            testObj.SetWorldPosition(200, 200);
            
            GameSystem.Instance.MapMgr.Ask<MapManager.MapFound>(new MapManager.GetMap(0)).ContinueWith((task) =>
            {
                var mapFound = task.Result;
                mapFound.Map.Tell(new MapInstance.AddEntity(testObj));
            });
        }
    }
}
