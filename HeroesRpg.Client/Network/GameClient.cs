using CocosSharp;
using HeroesRpg.Common.Generic;
using HeroesRpg.Network;
using HeroesRpg.Protocol;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Network
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameClient : NetClient<NetMessage>, ICCUpdatable
    {
        /// <summary>
        /// 
        /// </summary>
        public static GameClient Instance => Singleton<GameClient>.Instance;

        /// <summary>
        /// 
        /// </summary>
        private List<IGameFrame> m_frames;

        /// <summary>
        /// 
        /// </summary>
        private ConcurrentQueue<NetMessage> m_messages;

        /// <summary>
        /// 
        /// </summary>
        public bool IsConnected
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public GameClient() : base(NetMessage.Deserialize)
        {
            m_messages = new ConcurrentQueue<NetMessage>();
            m_frames = new List<IGameFrame>();
            Poll();
        }

        /// <summary>
        /// 
        /// </summary>
        private new void Poll()
        {
            base.Poll();

            Thread.Sleep(5);

            Task.Factory.StartNew(Poll);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public void Send(NetMessage message) => Send(message.Serialize());

        /// <summary>
        /// 
        /// </summary>
        public override void OnConnected()
        {
            IsConnected = true;
            OnMessageReceived(new ClientConnectedMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        public override void OnDisconnected()
        {
            IsConnected = false;
            OnMessageReceived(new ClientDisconnectedMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public override void OnMessageReceived(NetMessage message)
        {
            m_messages.Enqueue(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        public void RemoveFrame(IGameFrame frame) => m_frames.Remove(frame);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="frame"></param>
        public void AddFrame(IGameFrame frame) => m_frames.Add(frame);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public void Update(float dt)
        {
            var length = m_messages.Count;
            for (int i = 0; i < length; i++)
            {
                NetMessage message;
                m_messages.TryDequeue(out message);
                foreach (var frame in m_frames.ToArray())
                    if (frame.ProcessMessage(message))
                        return;
            }
        }
    }
}
