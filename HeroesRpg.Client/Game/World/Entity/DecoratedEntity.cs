using Box2D.Dynamics;
using CocosSharp;
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
    public abstract class DecoratedEntity : GameObject
    {
        /// <summary>
        /// 
        /// </summary>
        public const float MARGIN = 8f, BASE_MARGIN = 10f;

        /// <summary>
        /// 
        /// </summary>
        private HashSet<IEntityDecoration> m_decoration;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bodyType"></param>
        /// <param name="fixedRotation"></param>
        public DecoratedEntity(int id, b2BodyType bodyType, bool fixedRotation = true) : base(id, bodyType, fixedRotation)
        {
            m_decoration = new HashSet<IEntityDecoration>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoration"></param>
        public void AddDecoration(IEntityDecoration decoration)
        {
            if (m_decoration.Add(decoration))
            {
                AddChild(decoration.Node);
                ComputeDecorationPositions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoration"></param>
        public void RemoveDecoration(IEntityDecoration decoration)
        {
            if (m_decoration.Remove(decoration))
            {
                RemoveChild(decoration.Node);
                ComputeDecorationPositions();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void ComputeDecorationPositions()
        {
            var size = ScaledContentSize;
            var currentHeight = size.Height + BASE_MARGIN;
            foreach (var decoration in m_decoration.OrderByDescending(deco => deco.DecorationType))
            {
                decoration.Node.Position = new CCPoint(size.Width / 2, currentHeight);
                currentHeight += decoration.GetContentSize().Height + MARGIN;
            }
        }
    }
}
