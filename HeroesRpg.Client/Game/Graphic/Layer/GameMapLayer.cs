using Box2D.Collision.Shapes;
using Box2D.Common;
using Box2D.Dynamics;
using CocosSharp;
using HeroesRpg.Client.Game.World.Entity;
using HeroesRpg.Client.Game.World.Entity.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Layer
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameMapLayer : WrappedLayer
    {
        /// <summary>
        /// 
        /// </summary>
        public const int PTM_RATIO = 32;

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
        public GameMapLayer()
        {
            Color = CCColor3B.Gray;

            Floor = new CCDrawNode();
            
            Schedule(Update);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void AddedToScene()
        {
            base.AddedToScene();
            
            InitPhysics();

            Floor.AnchorPoint = CCPoint.AnchorLowerLeft;
            Floor.DrawRect(new CCRect(0, 0, LayerSizeInPixels.Width, 60), CCColor4B.LightGray);

            AddChild(Floor);
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitPhysics()
        {
            CCSize s = LayerSizeInPixels;

            var gravity = new b2Vec2(0.0f, -22.0f);

            World = new b2World(gravity);

            var def = new b2BodyDef();
            def.position = new b2Vec2(0, -1);
            def.type = b2BodyType.b2_staticBody;

            var groundBody = World.CreateBody(def);

            var groundBox = new b2PolygonShape();
            groundBox.SetAsBox(s.Width, 0 / PTM_RATIO);
            
            var fd = new b2FixtureDef();
            fd.shape = groundBox;
            fd.friction = 0f;

            groundBody.CreateFixture(fd);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddGameObject(GameObject obj)
        {
            obj.CreatePhysicsBody(World, PTM_RATIO);
            AddChild(obj);
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
    }
}
