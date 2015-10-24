using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class MovableEntity : GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        public MovableEntity(int id) : base(id) { }
    }
}
