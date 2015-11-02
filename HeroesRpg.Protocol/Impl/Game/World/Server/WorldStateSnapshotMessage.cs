using HeroesRpg.Protocol.Game.State;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Impl.Game.World.Server
{
    [ProtoContract(ImplicitFields = ImplicitFields.AllPublic)]
    public sealed class WorldStateSnapshotMessage : NetMessage
    {
        /// <summary>
        /// 
        /// </summary>
        public byte[] WorldStateData { get; set; }        
    }
}
