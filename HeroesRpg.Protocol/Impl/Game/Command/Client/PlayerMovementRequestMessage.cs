using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Game.Command.Client
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class PlayerMovementRequestMessage : NetMessage
    {
        public sbyte MovementX { get; set; }
        public sbyte MovementY { get; set; }
    }
}
