using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldStateSnapshot
    {
        /// <summary>
        /// 
        /// </summary>
        public long GameTime
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<ISerializableState> States
        {
            get
            {
                return m_states;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<ISerializableState> m_states;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public WorldStateSnapshot(long gameTime) : this()
        {
            GameTime = gameTime;
        }

        /// <summary>
        /// 
        /// </summary>
        public WorldStateSnapshot()
        {
            m_states = new List<ISerializableState>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public WorldStateSnapshot AddState(ISerializableState state)
        {
            m_states.Add(state);
            return this;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IEnumerable<T> GetStates<T>() where T : ISerializableState => m_states.OfType<T>();

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetSingleState<T>() where T : ISerializableState => GetStates<T>().FirstOrDefault();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void ToNetwork(BinaryWriter writer)
        {
            writer.Write(GameTime);

            // header including size
            writer.Write(m_states.Count);
            foreach(var state in m_states)
            {
                // type for deserialization
                writer.Write((byte)state.Type);
                state.ToNetwork(writer);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public void FromNetwork(BinaryReader reader)
        {
            GameTime = reader.ReadInt64();

            var size = reader.ReadInt32();
            for(var i = 0; i < size; i++)
            {
                var stateType = (StateTypeEnum)reader.ReadByte();
                ISerializableState state = null;
                switch (stateType)
                {
                    case StateTypeEnum.GAME_OBJECT:
                        state = new GameObjectState();
                        break;
                }
                if (state == null)
                    throw new NullReferenceException("unknow serializable state type : " + stateType);
                state.FromNetwork(reader);
                m_states.Add(state);
            }
        }
    }
}
