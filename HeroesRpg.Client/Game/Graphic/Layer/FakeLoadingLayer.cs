using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Graphic.Layer
{
    public sealed class FakeLoadingLayer : LoadingLayer
    {
        public const int FAKE_LOADING_TIME = 3;

        private float m_loadingTime;

        protected override void AddedToScene()
        {
            base.AddedToScene();

            Schedule(Update);
        }

        public override void Update(float dt)
        {
            base.Update(dt);
            IsDoneLoading = (m_loadingTime += dt) >= FAKE_LOADING_TIME;
        }
    }
}
