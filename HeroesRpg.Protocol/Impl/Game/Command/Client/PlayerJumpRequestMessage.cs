using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Game.Command.Client
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class PlayerJumpRequestMessage : NetMessage
    {
    }
}
