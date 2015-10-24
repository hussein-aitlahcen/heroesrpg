using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Data.Actor.Exception
{
    public sealed class UnknowActorTypeException : System.Exception
    {
        public UnknowActorTypeException(string message) : base(message) { }
    }
}
