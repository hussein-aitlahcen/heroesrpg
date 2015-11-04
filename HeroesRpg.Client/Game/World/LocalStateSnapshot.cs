using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World
{
    public sealed class LocalStateSnapshot
    {
        public long GameTime { get; }
        public float PositionX { get; }
        public float PositionY { get; }
        public float VelocityX { get; }
        public float VelocityY { get; }
        public LocalStateSnapshot(long gameTime, float x, float y, float vX, float vY)
        {
            GameTime = gameTime;
            PositionX = x;
            PositionY = y;
            VelocityX = vX;
            VelocityY = vY;
        }
    }
}
