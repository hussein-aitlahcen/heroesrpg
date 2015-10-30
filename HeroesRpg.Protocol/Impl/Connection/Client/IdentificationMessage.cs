using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Connection.Client
{
    [ProtoContract]
    public sealed class IdentificationMessage : NetMessage
    {
        [ProtoMember(1)]
        public string Username
        {
            get; set;
        }
        [ProtoMember(2)]
        public string Password { get; set; }
    }
}
