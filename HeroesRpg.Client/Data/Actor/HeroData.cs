using HeroesRpg.Client.Data.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Data.Actor
{
    public abstract class HeroData : ActorData
    {
        public HeroData(string name, string fullName) : base(name, fullName)
        {
        }
    }
}
