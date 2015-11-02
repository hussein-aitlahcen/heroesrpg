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
    public abstract class EntityState : ISerializableState
    {
        /// <summary>
        /// 
        /// </summary>
        public abstract StateTypeEnum Type
        {
            get;
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
        public EntityState()
        {
            m_parts = new List<StatePart>();
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
        public virtual void ToNetwork(BinaryWriter writer)
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
        public virtual void FromNetwork(BinaryReader reader)
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
