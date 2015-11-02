using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Game.Map.Server
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class PhysicsWorldDataMessage : NetMessage
    {
        public float GravityX { get; set; }
        public float GravityY { get; set; }
        public int PtmRatio { get; set; }
    }
}
