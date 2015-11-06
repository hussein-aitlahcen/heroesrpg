using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World
{
    public sealed class LocalEntityState
    {
        public float PositionX { get; }
        public float PositionY { get; }
        public LocalEntityState(float x, float y)
        {
            PositionX = x;
            PositionY = y;
        }
    }
}
