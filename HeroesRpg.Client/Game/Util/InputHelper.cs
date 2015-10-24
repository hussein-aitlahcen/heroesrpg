using CocosSharp;
using HeroesRpg.Common.Generic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.Util
{
    public sealed class InputHelper : Singleton<InputHelper>
    {
        private readonly HashSet<CCKeys> m_inputs = new HashSet<CCKeys>();
        private readonly HashSet<CCMouseButton> m_inputsMouse = new HashSet<CCMouseButton>();

        public void OnKeyPress(CCKeys key)
        {
            m_inputs.Add(key);
        }

        public void OnKeyRelease(CCKeys key)
        {
            m_inputs.Remove(key);
        }

        public void OnMousePress(CCMouseButton mouse)
        {
            m_inputsMouse.Add(mouse);
        }

        public void OnMouseRelease(CCMouseButton mouse)
        {
            m_inputsMouse.Remove(mouse);
        }

        public bool IsKeyIn(CCKeys key)
        {
            return m_inputs.Contains(key);
        }

        public bool IsMousePressed(CCMouseButton mouse)
        {
            return m_inputsMouse.Contains(mouse);
        }
    }
}
