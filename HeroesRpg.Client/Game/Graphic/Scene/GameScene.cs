using HeroesRpg.Client.Game.Graphic.Layer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CocosSharp;
using HeroesRpg.Client.Game.World.Entity.Impl;
using HeroesRpg.Client.Game.Util;
using Box2D.Common;
using HeroesRpg.Client.Game.World.Entity.Impl.Animated;
using HeroesRpg.Client.Network;
using HeroesRpg.Protocol.Impl.Game.Command.Client;
using HeroesRpg.Client.Game.World;

namespace HeroesRpg.Client.Game.Graphic.Scene
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class GameScene : WrappedScene<GameScene>
    {        
        /// <summary>
        /// 
        /// </summary>
        public GameMapLayer MapLayer
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        private sbyte MovementX;

        /// <summary>
        /// 
        /// </summary>
        private sbyte MovementY;

        /// <summary>
        /// 
        /// </summary>
        public GameScene()
        {
            AddChild(MapLayer = new GameMapLayer());
                                  
            Schedule(Update);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected override void OnKeyPressed(CCEventKeyboard ev)
        {
            base.OnKeyPressed(ev);
            switch (ev.Keys)
            {
                case CCKeys.Space:
                    break;

                case CCKeys.Left:
                    MovementX--;
                    SendMovementRequest();
                    break;

                case CCKeys.Right:
                    MovementX++;
                    SendMovementRequest();
                    break;

                case CCKeys.A:
                    SendSpellUseRequest();
                    break;
            }            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ev"></param>
        protected override void OnKeyReleased(CCEventKeyboard ev)
        {
            base.OnKeyReleased(ev);
            switch (ev.Keys)
            {
                case CCKeys.Left:
                    MovementX++;
                    SendMovementRequest();
                    break;

                case CCKeys.Right:
                    MovementX--;
                    SendMovementRequest();
                    break;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendSpellUseRequest()
        {
            GameClient.Instance.Send(new PlayerUseSpellRequestMessage() { SpellId = 0, Angle = 0 });
        }

        /// <summary>
        /// 
        /// </summary>
        private void SendMovementRequest()
        {
            GameClient.Instance.Send(new PlayerMovementRequestMessage() { MovementX = MovementX, MovementY = MovementY });
            if (WorldManager.Instance.LocalPlayer != null)
            {
                WorldManager.Instance.LocalPlayer.SetMovementSpeed(MovementX, MovementY);
            }
        }
    }
}
