using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Network
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TClient"></typeparam>
    public abstract class NetServer<TClient, TMessage>
        where TClient : NetServerClient, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public delegate TMessage MessageFactory(byte[] data);

        /// <summary>
        /// 
        /// </summary>
        private Host m_host;

        /// <summary>
        /// 
        /// </summary>
        private MessageFactory m_messageFactory;

        /// <summary>
        /// 
        /// </summary>
        public NetServer(MessageFactory messageFactory)
        {
            m_messageFactory = messageFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="port"></param>
        /// <param name="peerLimit"></param>
        public void Initialize(int port, int peerLimit)
        {
            m_host = new Host();
            m_host.InitializeServer(port, peerLimit);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Poll()
        {
            if (m_host == null || !m_host.IsInitialized)
                return;

            Event e;
            m_host.Service(0, out e);
            switch (e.Type)
            {
                case EventType.Connect:
                    var peer = e.Peer;
                    var client = new TClient();
                    client.Initialize(peer);
                    var handle = GCHandle.Alloc(client);
                    peer.UserData = (IntPtr)handle;
                    OnConnected(client);
                    break;
                                        
                case EventType.Receive:
                    client = GCHandle.FromIntPtr((peer = e.Peer).UserData).Target as TClient;
                    OnMessageReceived(client, m_messageFactory(e.Packet.ToArray()));
                    break;

                case EventType.Disconnect:
                    client = GCHandle.FromIntPtr((peer = e.Peer).UserData).Target as TClient;
                    OnDisconnected(client);
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void BroadcastNoAllocate(byte channelId, byte[] data) => Broadcast(channelId, data, PacketFlags.NoAllocate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void BroadcastUnsequenced(byte channelId, byte[] data) => Broadcast(channelId, data, PacketFlags.Unsequenced);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void BroadcastUnreliable(byte channelId, byte[] data) => Broadcast(channelId, data, PacketFlags.UnreliableFragment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void BroadcastReliable(byte channelId, byte[] data) => Broadcast(channelId, data, PacketFlags.Reliable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Broadcast(byte[] data) => Broadcast(0, data, PacketFlags.Reliable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        /// <param name="flags"></param>
        private void Broadcast(byte channelId, byte[] data, PacketFlags flags)
        {
            var packet = new Packet();
            packet.Initialize(data, flags);
            m_host.Broadcast(channelId, ref packet);
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnConnected(TClient client);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public abstract void OnMessageReceived(TClient client, TMessage message);

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnDisconnected(TClient client);
    }
}
