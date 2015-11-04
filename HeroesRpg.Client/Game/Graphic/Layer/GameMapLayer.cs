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
using HeroesRpg.Client.Game.World;

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
                        
            GameClient.Instance.Send(new PhysicsWorldDataRequestMessage());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddGameObject(GameObject obj)
        {
            if (MapInstance.Instance.AddGameObject(obj))
            {
                AddChild(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void RemoveGameObject(GameObject obj)
        {
            if (MapInstance.Instance.RemoveGameObject(obj.Id))
            {
                RemoveChild(obj);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        public override void Update(float dt)
        {
            base.Update(dt);
            MapInstance.Instance.UpdatePhysics(dt);   
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
                    MapInstance.Instance.Initialize(
                        physics.PtmRatio,
                        physics.GravityX,
                        physics.GravityY,
                        physics.VelocityIte,
                        physics.PositionIte);
                    Logger.Debug("Physics data received");
                    processed = true;
                })
                .With<ClientControlledObjectMessage>((m) =>
                {
                    WorldManager.Instance.ControlledObjectId = m.ObjectId;
                })
                .With<EntitySpawMessage>((entitySpawn) =>
                {
                    var entity = EntityFactory.Instance.CreateFromNetwork(entitySpawn.Type, entitySpawn.EntityData);
                    if (entity != null)
                    {
                        AddGameObject(entity);
                        Logger.Debug("[entity]");
                        Logger.Debug("id=" + entity.Id);
                        Logger.Debug("position=" + entity.InitialPosition);
                        var animated = entity as AnimatedEntity;
                        if (animated != null)
                        {
                            Logger.Debug("Animation stand for entity : " + entity.Id);
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
                    WorldManager.Instance.AddWorldStateSnapshot(snapshot);
                });
            return processed;
        }
    }
}
