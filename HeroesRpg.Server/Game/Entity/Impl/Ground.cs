﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using HeroesRpg.Protocol.Enum;

namespace HeroesRpg.Server.Game.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class Ground : StaticEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public override EntityTypeEnum Type => EntityTypeEnum.GROUND;

        /// <summary>
        /// 
        /// </summary>
        public override int SubType => 0;

        /// <summary>
        /// 
        /// </summary>
        public Ground()
        {
            SetNetworkType(GameObjectNetworkType.SHARE_CREATION_DELETION);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void InitializeNetworkParts()
        {
            base.InitializeNetworkParts();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(PhysicWidth / 2, PhysicHeight / 2);
            return shape;
        }
    }
}
