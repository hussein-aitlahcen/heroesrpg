using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Network
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NetClient<TMessage>
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
        public NetClient(MessageFactory messageFactory)
        {
            m_messageFactory = messageFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address"></param>
        /// <param name="port"></param>
        public void Connect(string address, int port)
        {
            Disconnect();
            m_host = new Host();
            m_host.InitializeClient(1);
            m_host.Connect(address, port, 0);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Disconnect()
        {
            if (m_host.IsInitialized)
                m_host.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="host"></param>
        public void SetHost(Host host) => m_host = host;
        
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
                    OnConnected();
                    break;

                case EventType.Receive:
                    OnMessageReceived(m_messageFactory(e.Packet.ToArray()));
                    break;

                case EventType.Disconnect:
                    OnDisconnected();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void SendNoAllocate(byte channelId, byte[] data) => Send(channelId, data, PacketFlags.NoAllocate);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void SendUnsequenced(byte channelId, byte[] data) => Send(channelId, data, PacketFlags.Unsequenced);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void SendUnreliable(byte channelId, byte[] data) => Send(channelId, data, PacketFlags.UnreliableFragment);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        public void SendReliable(byte channelId, byte[] data) => Send(channelId, data, PacketFlags.Reliable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public void Send(byte[] data) => Send(0, data, PacketFlags.Reliable);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        /// <param name="flags"></param>
        private void Send(byte channelId, byte[] data, PacketFlags flags)
        {
            var packet = new Packet();
            packet.Initialize(data, flags);
            m_host.Broadcast(channelId, ref packet);
        }

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnConnected();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public abstract void OnMessageReceived(TMessage message);

        /// <summary>
        /// 
        /// </summary>
        public abstract void OnDisconnected();
    }
}
