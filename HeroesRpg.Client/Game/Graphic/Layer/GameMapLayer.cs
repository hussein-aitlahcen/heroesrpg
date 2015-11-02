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
        public GameMapLayer()
        {
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
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveGameObject(GameObject obj)
        {
            World.DestroyBody(obj.PhysicsBody);
            RemoveChild(obj);
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
            var physicsWorldDataMessage = message as PhysicsWorldDataMessage;
            var entitySpawnMessage = message as EntitySpawMessage;

            if(physicsWorldDataMessage != null)
            {
                GravityX = physicsWorldDataMessage.GravityX;
                GravityY = physicsWorldDataMessage.GravityY;
                PtmRatio = physicsWorldDataMessage.PtmRatio;
                InitPhysics();

                //var m_hero = new DragonBallHero();
                //m_hero.Position = new CCPoint(VisibleBoundsWorldspace.MidX, 500);
                //m_hero.SetPlayerName("Smarken");
                //m_hero.SetHeroId((int)DragonBallHeroEnum.BROLY);
                //m_hero.StartAnimation(Animation.STAND);

                //var testObj = new DragonBallHero();
                //testObj.Position = new CCPoint(VisibleBoundsWorldspace.MidX, 250);
                //testObj.SetPlayerName("Test");
                //testObj.SetHeroId((int)DragonBallHeroEnum.BROLY);
                //testObj.StartAnimation(Animation.STAND);

                //AddGameObject(testObj);
                //AddGameObject(m_hero);
                return true;
            }
            else if(entitySpawnMessage != null)
            {
                var entity = EntityFactory.Instance.CreateFromNetwork(entitySpawnMessage.Type, entitySpawnMessage.EntityData);
                if(entity != null)
                {
                    AddGameObject(entity);

                    var animated = entity as AnimatedEntity;
                    if(animated != null)
                    {
                        animated.StartAnimation(Animation.STAND);
                    }
                }
                return true;
            }

            return false;
        }
    }
}
