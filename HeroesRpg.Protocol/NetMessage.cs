using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;
using HeroesRpg.Protocol.Impl.Selection.Client;
using HeroesRpg.Protocol.Impl.Selection.Server;
using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol
{
    [ProtoInclude(1011, typeof(CharactersListMessage))]
    [ProtoInclude(1010, typeof(CharacterSelectionResultMessage))]
    [ProtoInclude(1009, typeof(CharacterDeletionResultMessage))]
    [ProtoInclude(1008, typeof(CharacterCreationResultMessage))]
    [ProtoInclude(1007, typeof(CharactersListRequestMessage))]
    [ProtoInclude(1006, typeof(CharacterDeletionMessage))]
    [ProtoInclude(1005, typeof(CharacterSelectionMessage))]
    [ProtoInclude(1004, typeof(CharacterCreationMessage))]
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
