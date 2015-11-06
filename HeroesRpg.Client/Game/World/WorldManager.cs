using HeroesRpg.Client.Game.World.Entity;
using HeroesRpg.Client.Game.World.Entity.Impl;
using HeroesRpg.Common;
using HeroesRpg.Common.Generic;
using HeroesRpg.Common.Util;
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
        public static int MIN_SNAP_BUFFER = (int)Math.Floor(CL_INTERPOLATION / Constant.UPDATE_RATE_SECOND);

        /// <summary>
        /// 
        /// </summary>
        public const float CL_INTERPOLATION = 0.10f;

        /// <summary>
        /// 
        /// </summary>
        public const double CL_INTERPOLATION_ERROR = 0.5;

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
        private long m_serverPhysicUpdateSequence;

        /// <summary>
        /// 
        /// </summary>
        public long ServerPhysicUpdateSequence => m_serverPhysicUpdateSequence;

        /// <summary>
        /// 
        /// </summary>
        private long m_initialServerPhysicUpdateSequence;

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
            get
            {
                if (ControlledObjectId == -1)
                    return null;
                return MapInstance.Instance.GetGameObject(ControlledObjectId) as Hero;
            }
        }
        
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
            m_serverPhysicUpdateSequence = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsSnapshotBuffered() => m_stateSnapshots.Count >= MIN_SNAP_BUFFER;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public bool IsLocalSnapshotBuffered() => InterpolationState != null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool HasInterpolatedLocalEntityState(int id) => IsLocalSnapshotBuffered() && InterpolationState.ContainsGameObjectState(id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public LocalEntityState GetInterpolatedEntityState(int id) => InterpolationState.GetGameObjectState(id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapShot"></param>
        public void AddLocalStateSnapshot(long physicUpdateSequence)
        {            
            var snapShot = new LocalStateSnapshot(physicUpdateSequence + m_initialServerPhysicUpdateSequence);
            foreach (var obj in MapInstance.Instance.GameObjects)
                snapShot.AddEntityState(obj.Id, obj.PhysicPositionX, obj.PhysicPositionY);
            m_localSnapshots.Enqueue(snapShot);            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="snapShot"></param>
        public void AddWorldStateSnapshot(WorldStateSnapshot snapShot)
        {
            if (snapShot.PhysicUpdateSequence >= m_serverPhysicUpdateSequence)
            {
                m_serverPhysicUpdateSequence = snapShot.PhysicUpdateSequence;
                m_stateSnapshots.Enqueue(snapShot);
            }
            if (m_stateSnapshots.Count == 1)
                m_initialServerPhysicUpdateSequence = snapShot.PhysicUpdateSequence;
            AddLocalStateSnapshot(MapInstance.Instance.ClientPhysicUpdateSequence);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateEntities()
        {
            if (IsSnapshotBuffered())
            {
                var worldState = m_stateSnapshots.Dequeue();

                if(m_localSnapshots.Count > MIN_SNAP_BUFFER)
                    InterpolationState = m_localSnapshots.Dequeue();

                MapInstance.Instance.UpdateEntitiesFromNet(worldState);
            }           
        }
    }
}
