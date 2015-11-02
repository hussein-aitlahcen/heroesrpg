using HeroesRpg.Common.Generic;
using HeroesRpg.Protocol.Game.State;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class WorldManager : Singleton<WorldManager>
    {
        /// <summary>
        /// 
        /// </summary>
        public const int MAX_SNAP_BUFFER = 10;

        /// <summary>
        /// 
        /// </summary>
        public const int MIN_SNAP_BUFFER = 2;

        /// <summary>
        /// 
        /// </summary>
        private Queue<WorldStateSnapshot> m_stateSnapshots;

        /// <summary>
        /// 
        /// </summary>
        private double m_gameTime;

        /// <summary>
        /// 
        /// </summary>
        public WorldManager()
        {
            m_stateSnapshots = new Queue<WorldStateSnapshot>();
            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            m_stateSnapshots.Clear();
            m_gameTime = -1;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapShot"></param>
        public void RegisterSnapshot(WorldStateSnapshot snapShot)
        {
            if (snapShot.GameTime >= m_gameTime)
            {
                m_gameTime = snapShot.GameTime;
                m_stateSnapshots.Enqueue(snapShot);
                if (m_stateSnapshots.Count > MAX_SNAP_BUFFER)
                    m_stateSnapshots.Dequeue();
            }
            else
            {
                Log.Debug($"old world snap reveived time={snapShot.GameTime}, realtime={m_gameTime}");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateEntities()
        {            
            if(m_stateSnapshots.Count > MIN_SNAP_BUFFER)
            {
                MapInstance.Instance.UpdateEntities(m_stateSnapshots.Dequeue());
            }
        }
    }
}
