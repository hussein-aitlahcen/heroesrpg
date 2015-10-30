using CocosSharp;
using HeroesRpg.Common.Generic;
using HeroesRpg.Network;
using HeroesRpg.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            m_frames = new List<IGameFrame>();
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
            foreach (var frame in m_frames.ToArray())
                if (frame.ProcessMessage(message))
                    return;
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
            Poll();
        }
    }
}
