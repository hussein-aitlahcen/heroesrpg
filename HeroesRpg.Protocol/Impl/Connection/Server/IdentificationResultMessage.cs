using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Connection.Server
{
    public enum IdentificationResultEnum
    {
        SUCCESS,
        WRONG_CREDENTIALS,
        BANNED,
    }

    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class IdentificationResultMessage : NetMessage
    {
        public IdentificationResultEnum Code { get; set; }
    }
}
