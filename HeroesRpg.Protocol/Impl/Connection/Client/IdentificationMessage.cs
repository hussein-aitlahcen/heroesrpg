using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Connection.Client
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class IdentificationMessage : NetMessage
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
