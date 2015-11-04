using HeroesRpg.Client.Game.World.Entity;
using HeroesRpg.Client.Game.World.Entity.Impl;
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
        public const int CL_INTERPOLATION = MIN_SNAP_BUFFER * 50;

        /// <summary>
        /// 
        /// </summary>
        private Queue<WorldStateSnapshot> m_stateSnapshots;

        /// <summary>
        /// 
        /// </summary>
        private Queue<LocalStateSnapshot> m_localSnapshots;

        /// <summary>
        /// 
        /// </summary>
        private long m_gameTime;

        /// <summary>
        /// 
        /// </summary>
        public long GameTime => m_gameTime;

        /// <summary>
        /// 
        /// </summary>
        public int ControlledObjectId
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        public LocalStateSnapshot InterpolationState
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public Hero LocalPlayer
        {
            get;
            set;
        }

        /// <summary>
        /// 
        /// </summary>
        private bool m_localSnapNeeded;

        /// <summary>
        /// 
        /// </summary>
        public WorldManager()
        {
            m_stateSnapshots = new Queue<WorldStateSnapshot>();
            m_localSnapshots = new Queue<LocalStateSnapshot>();
            Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        public void Reset()
        {
            m_localSnapshots.Clear();
            m_stateSnapshots.Clear();
            m_gameTime = -1;
            m_localSnapNeeded = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSnapshotBuffered() => m_stateSnapshots.Count > MIN_SNAP_BUFFER;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsLocalSnapshotBuffered() => InterpolationState != null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentX"></param>
        /// <param name="currentY"></param>
        /// <returns></returns>
        public bool IsPositionInterpolationValid(float nextX, float nextY)
        {
            // minium required
            if (!IsLocalSnapshotBuffered())
                return true;
            
            var diffX = Math.Abs(InterpolationState.PositionX - nextX);
            var diffY = Math.Abs(InterpolationState.PositionY - nextY);

            return diffX <= MovableEntity.INTERPOLATION_MIN_DELTA && diffY <= MovableEntity.INTERPOLATION_MIN_DELTA;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nextVelocityX"></param>
        /// <param name="nextVelocityY"></param>
        /// <returns></returns>
        public bool IsVelocityInterpolationValid(float nextVelocityX, float nextVelocityY)
        {
            // minium required
            if (!IsLocalSnapshotBuffered())
                return true;

            var diffX = Math.Abs(InterpolationState.VelocityX - nextVelocityX);
            var diffY = Math.Abs(InterpolationState.VelocityY - nextVelocityY);

            return diffX <= 0.05 && diffY <= 0.05;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapShot"></param>
        public void AddLocalStateSnapshot(float x, float y, float vX, float vY)
        {
            if (m_localSnapNeeded)
            {
                m_localSnapNeeded = false;
                m_localSnapshots.Enqueue(new LocalStateSnapshot(
                    GameTime,
                    x,
                    y,
                    vX,
                    vY));
                if (m_localSnapshots.Count > MIN_SNAP_BUFFER)
                    InterpolationState = m_localSnapshots.Dequeue();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapShot"></param>
        public void AddWorldStateSnapshot(WorldStateSnapshot snapShot)
        {
            if (snapShot.GameTime >= m_gameTime)
            {
                m_localSnapNeeded = true;
                m_gameTime = snapShot.GameTime;
                m_stateSnapshots.Enqueue(snapShot);
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
            if(IsSnapshotBuffered())
                MapInstance.Instance.UpdateEntities(m_stateSnapshots.Dequeue());            
        }
    }
}
