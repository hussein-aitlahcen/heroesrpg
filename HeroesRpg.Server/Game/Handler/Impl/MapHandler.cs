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
            message.Client.Send(new PhysicsWorldDataMessage()
            {
                GravityX = 0f,
                GravityY = -22.0f,
                PtmRatio = 32
            });

            // TESTING PURPOSE
                        
            for (int i = 0; i < 15; i++)
            {
                var testObj = new DragonBallHero();
                testObj.SetPlayerName("p" + i);
                testObj.SetHeroId((int)DragonBallHeroEnum.BROLY);
                testObj.SetWorldPosition(100 + 50 * i, i * 20);

                GameSystem.Instance.MapMgr.Ask<MapManager.MapFound>(new MapManager.GetMap(0)).ContinueWith((task) =>
                {
                    var mapFound = task.Result;
                    mapFound.Map.Ask<MapInstance.AddEntityResult>(new MapInstance.AddEntity(testObj)).ContinueWith((task1) =>
                    {
                        using (var stream = new MemoryStream())
                        {
                            using (var writer = new BinaryWriter(stream))
                            {
                                writer.Write((int)testObj.HeroType);
                                testObj.ToNetwork(writer);
                            }
                            message.Client.Send(new EntitySpawMessage()
                            {
                                Type = EntityTypeEnum.HERO,
                                EntityData = stream.ToArray()
                            });
                        }
                    });
                });
            }
        }
    }
}
