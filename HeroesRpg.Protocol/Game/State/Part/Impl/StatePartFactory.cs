using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State.Part.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class StatePartDeserializationException : Exception
    {
        public StatePartDeserializationException(string message, Exception inner = null) : base(message, inner) { }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class StatePartFactory : Singleton<StatePartFactory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public StatePart Create(StatePartTypeEnum type, BinaryReader reader)
        {
            StatePart part = null;
            switch (type)
            {
                case StatePartTypeEnum.GAME_OBJECT: part = new GameObjectPart(); break;
                case StatePartTypeEnum.MOVABLE_ENTITY: part = new MovableEntityPart(); break; 
                case StatePartTypeEnum.COMBAT_ENTITY: part = new CombatEntityPart(); break;
                case StatePartTypeEnum.HERO_ENTITY: part = new HeroEntityPart(); break;
                case StatePartTypeEnum.PHYSIC_OBJECT: part = new PhysicObjectPart(); break;
                default: throw new ArgumentException("unknow state part type id : " + type);
            }
            try {
                part.FromNetwork(reader);
            }
            catch(Exception e)
            {
                throw new StatePartDeserializationException("unable to deserialize part : " + part.Type, e);
            }
            return part;
        }
    }
}
