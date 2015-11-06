using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Map
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameObjectSnapshot
    {
        public int Id { get; }
        public float PositionX { get; }
        public float PositionY { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public GameObjectSnapshot(int id, float x, float y)
        {
            Id = id;
            PositionX = x;
            PositionY = y;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PrivateWorldStateSnapshot : IDisposable
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
        public Dictionary<int, GameObjectSnapshot> GameObjects => m_objectSnapshot;

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, GameObjectSnapshot> m_objectSnapshot;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameTime"></param>
        public PrivateWorldStateSnapshot(long physicUpdateSequence)
        {
            PhysicUpdateSequence = physicUpdateSequence;
            m_objectSnapshot = new Dictionary<int, GameObjectSnapshot>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snap"></param>
        public void AddObjectSnapshot(GameObjectSnapshot snap) => m_objectSnapshot.Add(snap.Id, snap);
                
        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            m_objectSnapshot.Clear();
            m_objectSnapshot = null;
        }
    }
}
