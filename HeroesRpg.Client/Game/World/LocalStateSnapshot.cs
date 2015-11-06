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
    public sealed class LocalStateSnapshot
    {
        /// <summary>
        /// 
        /// </summary>
        public long PhysicUpdateSequence
        {
            get;
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, LocalEntityState> m_gameObjectState;

        /// <summary>
        /// 
        /// </summary>
        public LocalStateSnapshot(long physicUpdateSequence)
        {
            PhysicUpdateSequence = physicUpdateSequence;
            m_gameObjectState = new Dictionary<int, LocalEntityState>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool ContainsGameObjectState(int id) => m_gameObjectState.ContainsKey(id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LocalEntityState GetGameObjectState(int id) => m_gameObjectState[id];

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="vx"></param>
        /// <param name="vy"></param>
        public void AddEntityState(int id, float x, float y) => m_gameObjectState.Add(id, new LocalEntityState(x, y));
    }
}
