using HeroesRpg.Protocol.Enum;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Game.Map.Server
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class EntitySpawMessage : NetMessage
    {
        public EntityTypeEnum Type { get; set; }
        public byte[] EntityData { get; set; }
    }
}
