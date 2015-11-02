using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Server.Game.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class StaticEntity : GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        public StaticEntity() : base(Box2D.Dynamics.b2BodyType.b2_staticBody)
        {
        }
    }
}
