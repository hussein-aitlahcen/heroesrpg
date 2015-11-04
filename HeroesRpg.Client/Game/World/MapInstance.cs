using Box2D.Common;
using Box2D.Dynamics;
using HeroesRpg.Client.Game.World.Entity;
using HeroesRpg.Client.Game.World.Entity.Impl;
using HeroesRpg.Common.Generic;
using HeroesRpg.Protocol.Game.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World
{
    public sealed class MapInstance : Singleton<MapInstance>
    {
        /// <summary>
        /// 
        /// </summary>
        public b2World World
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PtmRatio
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float GravityX
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public float GravityY
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int VelocityIterations
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int PositionIterations
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private Dictionary<int, GameObject> m_gameObjects;

        /// <summary>
        /// 
        /// </summary>
        public MapInstance()
        {
            m_gameObjects = new Dictionary<int, GameObject>();
            World = new b2World(new b2Vec2(0, 0));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptmRatio"></param>
        /// <param name="gravX"></param>
        /// <param name="gravY"></param>
        /// <param name="velocityIte"></param>
        /// <param name="positionIte"></param>
        public void Initialize(int ptmRatio, float gravX, float gravY, int velocityIte, int positionIte)
        {
            PtmRatio = ptmRatio;
            GravityX = gravX;
            GravityY = gravY;
            VelocityIterations = velocityIte;
            PositionIterations = positionIte;

            InitPhysics();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPhysics()
        {
            World.Gravity = new b2Vec2(GravityX, GravityY);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdatePhysics(float dt)
        {
            WorldManager.Instance.UpdateEntities();
            World.Step(dt, VelocityIterations, PositionIterations);
        }

        /// <summary>
        /// 
        /// </summary>
        public void UpdateEntities(WorldStateSnapshot snapShot)
        {
            foreach(var state in snapShot.States)
            {
                switch (state.Type)
                {
                    case StateTypeEnum.GAME_OBJECT:
                        var objState = (GameObjectState)state;
                        var obj = GetGameObject(objState.Id);
                        if(obj != null)
                        {
                            obj.UpdatePart(objState.Parts);
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObj"></param>
        public bool AddGameObject(GameObject gameObj)
        {
            if(gameObj.IsLocal)
                WorldManager.Instance.LocalPlayer = gameObj as Hero;

            if (!m_gameObjects.ContainsKey(gameObj.Id))
            {
                gameObj.CreatePhysicsBody(World, PtmRatio);
                m_gameObjects.Add(gameObj.Id, gameObj);
                return true;
            }
            else
            {
                Log.Debug("duplicated game object id : " + gameObj.Id + " type=" + gameObj.GetType().Name);
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gameObj"></param>
        public bool RemoveGameObject(int id)
        {
            if (m_gameObjects.ContainsKey(id))
            {
                var obj = m_gameObjects[id];
                World.DestroyBody(obj.PhysicsBody);
                return true;
            }
            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public GameObject GetGameObject(int id)
        {
            if (m_gameObjects.ContainsKey(id))
                return m_gameObjects[id];
            return null;
        }
    }
}
