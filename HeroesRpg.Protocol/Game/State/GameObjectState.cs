using HeroesRpg.Protocol.Game.State.Part;
using HeroesRpg.Protocol.Game.State.Part.Impl;
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
    public sealed class GameObjectState : ISerializableState
    {
        /// <summary>
        /// 
        /// </summary>
        public StateTypeEnum Type
        {
            get
            {
                return StateTypeEnum.GAME_OBJECT;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Id
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public List<StatePart> Parts
        { 
            get
            {
                return m_parts;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private List<StatePart> m_parts;

        /// <summary>
        /// 
        /// </summary>
        public GameObjectState()
        {
            m_parts = new List<StatePart>();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parts"></param>
        public GameObjectState(int id, IEnumerable<StatePart> parts)
        {
            Id = id;
            m_parts = new List<StatePart>(parts);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="part"></param>
        public void AddPart(StatePart part) => m_parts.Add(part);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public void ToNetwork(BinaryWriter writer)
        {
            writer.Write(Id);

            writer.Write(Parts.Count);
            foreach (var part in m_parts)
            {
                writer.Write((byte)part.Type);
                part.ToNetwork(writer);
            }        
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public void FromNetwork(BinaryReader reader)
        {
            Id = reader.ReadInt32();
            var size = reader.ReadInt32();
            for(var i = 0; i < size; i++)
            {
                var type = (StatePartTypeEnum)reader.ReadByte();
                var part = StatePartFactory.Instance.Create(type, reader);
                m_parts.Add(part);
            }
        }
    }
}
