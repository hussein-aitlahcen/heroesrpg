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

    [ProtoContract]
    public sealed class IdentificationResultMessage : NetMessage
    {
        [ProtoMember(1)]
        public IdentificationResultEnum Code
        {
            get;
            set;
        }
    }
}
