using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol
{
    [ProtoInclude(1003, typeof(IdentificationResultMessage))]
    [ProtoInclude(1002, typeof(IdentificationMessage))]
    [ProtoInclude(1001, typeof(ClientVersionRequired))]
    [ProtoInclude(1000, typeof(WelcomeConnectMessage))]
    [ProtoContract]
    public abstract class NetMessage
    {
        public byte[] Serialize()
        {
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, this);
                return stream.ToArray();
            }
        }

        public static NetMessage Deserialize(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<NetMessage>(stream);
            }
        }
    }
}
