using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using HeroesRpg.Client.Game.World.Entity;
using HeroesRpg.Client.Game.World.Entity.Impl;
using HeroesRpg.Client.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HeroesRpg.Protocol;
using HeroesRpg.Protocol.Impl.Game.Map.Server;
using HeroesRpg.Protocol.Impl.Game.Map.Client;
using HeroesRpg.Client.Game.World.Entity.Impl.Animated;
using HeroesRpg.Protocol.Impl.Game.World.Server;
using HeroesRpg.Protocol.Game.State;
using System.IO;

namespace HeroesRpg.Client.Game.Graphic.Layer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameMapLayer : WrappedLayer, IDisposable, IGameFrame
    {
        /// <summary>
        /// 
        /// </summary>
        public CCDrawNode Floor
        {
            get;
            private set;
        }

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
        private Dictionary<int, GameObject> m_gameObjects;
        
        /// <summary>
        /// 
        /// </summary>
        public GameMapLayer()
        {
            m_gameObjects = new Dictionary<int, GameObject>();

            Color = CCColor3B.White;

            GameClient.Instance.AddFrame(this);
                        
            Schedule(Update);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();

            m_gameObjects.Clear();

            World = new b2World(b2Vec2.Zero);
            
            Floor = new CCDrawNode();
            Floor.AnchorPoint = CCPoint.AnchorLowerLeft;
            Floor.DrawRect(new CCRect(0, 0, LayerSizeInPixels.Width, 65), CCColor4B.LightGray);

            AddChild(Floor);

            GameClient.Instance.Send(new PhysicsWorldDataRequestMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPhysics()
        {
            CCSize s = LayerSizeInPixels;
                        
            World.Gravity = new b2Vec2(GravityX, GravityY);

            var def = new b2BodyDef();
            def.position = new b2Vec2(0, 0);
            def.type = b2BodyType.b2_staticBody;

            var groundBody = World.CreateBody(def);

            var groundBox = new b2PolygonShape();
            groundBox.SetAsBox(s.Width / PtmRatio, 60 / PtmRatio);
            
            var fd = new b2FixtureDef();
            fd.shape = groundBox;
            fd.friction = 1f;

            groundBody.CreateFixture(fd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddGameObject(GameObject obj)
        {
            obj.CreatePhysicsBody(World, PtmRatio);
            AddChild(obj);
            m_gameObjects.Add(obj.Id, obj);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveGameObject(GameObject obj)
        {
            World.DestroyBody(obj.PhysicsBody);
            RemoveChild(obj);
            m_gameObjects.Remove(obj.Id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);            
            World.Step(dt, 8, 4);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public bool ProcessMessage(NetMessage message)
        {
            var processed = false;
            message.Match()
                .With<PhysicsWorldDataMessage>((physics) =>
                {
                    GravityX = physics.GravityX;
                    GravityY = physics.GravityY;
                    PtmRatio = physics.PtmRatio;
                    InitPhysics();
                    processed = true;
                })
                .With<EntitySpawMessage>((entitySpawn) =>
                {
                    var entity = EntityFactory.Instance.CreateFromNetwork(entitySpawn.Type, entitySpawn.EntityData);
                    if (entity != null)
                    {
                        AddGameObject(entity);

                        var animated = entity as AnimatedEntity;
                        if (animated != null)
                        {
                            animated.StartAnimation(Animation.STAND);
                        }
                    }
                    processed = true;
                })
                .With<WorldStateSnapshotMessage>((m) =>
                {
                    var snapshot = new WorldStateSnapshot();
                    using (var stream = new MemoryStream(m.WorldStateData))
                    {
                        using (var reader = new BinaryReader(stream))
                        {
                            snapshot.FromNetwork(reader);
                        }
                    }

                    foreach(var state in snapshot.States)
                    {
                        switch (state.Type)
                        {
                            case StateTypeEnum.GAME_OBJECT:
                                // TODO: virtual method SelectNetworkPart(parts) from gameobject
                                break;
                        }
                    }
                });
            return processed;
        }
    }
}
