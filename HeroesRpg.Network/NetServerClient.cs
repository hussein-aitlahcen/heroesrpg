using ENet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesRpg.Network
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class NetServerClient
    {
        /// <summary>
        /// 
        /// </summary>
        private Peer m_peer;


        /// <summary>
        /// 
        /// </summary>
        private static ulong NextClientId;
        private static object NextClientIdLock = new object();

        /// <summary>
        /// 
        /// </summary>
        public ulong ClientId
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peer"></param>
        internal void Initialize(Peer peer)
        {
            lock(NextClientIdLock)
                ClientId = ++NextClientId;
            m_peer = peer;
            m_peer.SetTimeouts(5, 2000, 4000);
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
        public void Send(byte[] data) => SendReliable(0, data);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="channelId"></param>
        /// <param name="data"></param>
        /// <param name="flags"></param>
        private void Send(byte channelId, byte[] data, PacketFlags flags)
        {
            try
            {
                m_peer.Send(channelId, data, flags);
            }
            catch(Exception e)
            {
            }
        }
    }
}
