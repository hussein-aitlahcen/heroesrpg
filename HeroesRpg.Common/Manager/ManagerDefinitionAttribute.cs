using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Common.Manager
{
    public sealed class ManagerDefinitionAttribute : Attribute
    {
        public int Id { get; private set; }
        public IEnumerable<int> Dependencies { get; private set; }
        public ManagerDefinitionAttribute(int id, params int[] dependencies)
        {
            Id = id;
            Dependencies = dependencies;
        }
    }
}
