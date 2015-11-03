using HeroesRpg.Protocol.Impl.Connection.Client;
using HeroesRpg.Protocol.Impl.Connection.Server;
using HeroesRpg.Protocol.Impl.Game.Command.Client;
using HeroesRpg.Protocol.Impl.Game.Map.Client;
using HeroesRpg.Protocol.Impl.Game.Map.Server;
using HeroesRpg.Protocol.Impl.Game.World.Server;
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
    [ProtoInclude(1017, typeof(ClientControlledObjectMessage))]
    [ProtoInclude(1016, typeof(PlayerMovementRequestMessage))]
    [ProtoInclude(1015, typeof(WorldStateSnapshotMessage))]
    [ProtoInclude(1014, typeof(EntitySpawMessage))]
    [ProtoInclude(1013, typeof(PhysicsWorldDataRequestMessage))]
    [ProtoInclude(1012, typeof(PhysicsWorldDataMessage))]
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
        private byte[] m_serializedBuffer;

        public byte[] Serialize()
        {
            if (m_serializedBuffer == null)
            {
                using (var stream = new MemoryStream())
                {
                    using (var lzStream = new LZ4.LZ4Stream(stream, LZ4.LZ4StreamMode.Compress))
                        Serializer.Serialize(lzStream, this);
                    m_serializedBuffer = stream.ToArray();
                }
            }
            return m_serializedBuffer;
        }

        public static NetMessage Deserialize(byte[] data)
        {
            using (var stream = new LZ4.LZ4Stream(new MemoryStream(data), LZ4.LZ4StreamMode.Decompress))
            {
                return Serializer.Deserialize<NetMessage>(stream);
            }
        }     
    }
}
