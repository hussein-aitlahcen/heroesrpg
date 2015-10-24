using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity
{
    public abstract class StaticEntity : GameObject
    {
        public StaticEntity(int id) : base(id) { }
    }
}
