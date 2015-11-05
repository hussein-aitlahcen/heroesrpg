using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Game.Command.Client
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class PlayerUseSpellRequestMessage : NetMessage
    {
        public int SpellId { get; set; }
        public float Angle { get; set; }
    }
}
