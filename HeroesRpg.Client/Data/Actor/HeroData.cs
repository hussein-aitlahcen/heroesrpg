using HeroesRpg.Client.Data.Texture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Data.Actor
{
    public sealed class HeroData : ActorData
    {
        public const string BROLY = "broly";
        public HeroData(string name, string fullName) : base(name, fullName)
        {
        }
    }
}
