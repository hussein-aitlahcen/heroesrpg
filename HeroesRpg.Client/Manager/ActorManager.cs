using HeroesRpg.Client.Data.Actor;
using HeroesRpg.Client.Data.Actor.Exception;
using HeroesRpg.Client.Data.Texture;
using HeroesRpg.Common.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Manager
{
    /// <summary>
    /// Data manager that will load all resources
    /// </summary>
    [ManagerDefinition(ManagerEnum.DATA_MANAGER, ManagerEnum.SPRITE_MANAGER)]
    public sealed class ActorManager : Manager<ActorManager>
    {
        /// <summary>
        /// 
        /// </summary>
        private Dictionary<string, ActorData> m_actors;

        /// <summary>
        /// 
        /// </summary>
        public ActorManager()
        {
            m_actors = new Dictionary<string, ActorData>();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            base.Initialize();

            LoadCharacters();
        }

        /// <summary>
        /// 
        /// </summary>
        private void LoadCharacters()
        {
            foreach(var key in SpriteSheetCache.Instance.Keys)
            {
                try
                {
                    m_actors[key] = ActorFactory.Instance.CreateActor(key);
                }
                catch(UnknowActorTypeException e)
                {
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        public T GetActorByKey<T>(string key) where T : ActorData =>
            (T)m_actors[key];

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T GetActorByName<T>(string name) where T : ActorData => 
            (T)m_actors
                .Values
                .First(character => character.Name.Equals(name, StringComparison.OrdinalIgnoreCase));        
    }
}
