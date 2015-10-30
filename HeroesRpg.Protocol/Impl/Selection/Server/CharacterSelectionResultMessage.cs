using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Selection.Server
{
    [ProtoContract]
    public sealed class CharacterSelectionResultMessage : NetMessage
    {
    }
}
