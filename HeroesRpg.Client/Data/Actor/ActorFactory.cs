using CocosSharp;
using HeroesRpg.Client.Data.Actor.Exception;
using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Data.Actor
{
    public sealed class ActorFactory : Singleton<ActorFactory>
    {
        public const string HERO = "hero";

        public ActorData CreateActor(string key)
        {
            var keyData = key.Split('.');
            var actorType = keyData[0];
            var actorName = keyData[1];

            switch(actorType)
            {
                case HERO:
                    return new DragonBallHeroData(actorName, key);

                default:
                    throw new UnknowActorTypeException($"actor type cannot be determined : {key}");
            }
        }
    }
}
