using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Connection.Server
{
    [ProtoContract]
    public sealed class WelcomeConnectMessage : NetMessage
    {
    }
}
