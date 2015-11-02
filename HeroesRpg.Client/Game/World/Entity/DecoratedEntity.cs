using Box2D.Dynamics;
using CocosSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

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
        public const float BASE_MARGIN = 20f;

        /// <summary>
        /// 
        /// </summary>
        private HashSet<IEntityDecoration> m_decorations;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="bodyType"></param>
        /// <param name="fixedRotation"></param>
        public DecoratedEntity(b2BodyType bodyType) : base(bodyType)
        {
            m_decorations = new HashSet<IEntityDecoration>();
        }

        /// <summary>
        /// 
        /// </summary>
        public void ClearDecoration()
        {
            foreach(var decoration in m_decorations)
            {
                RemoveChild(decoration.Node);
                decoration.Node.Dispose();
            }
            m_decorations.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="decoration"></param>
        public void AddDecoration(IEntityDecoration decoration)
        {
            if (m_decorations.Add(decoration))
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
            if (m_decorations.Remove(decoration))
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
            foreach (var decoration in m_decorations.OrderByDescending(deco => deco.DecorationType))
            {
                currentHeight += decoration.BottomMargin;
                decoration.Node.Position = new CCPoint(size.Width / 2, currentHeight);
                currentHeight += decoration.GetContentSize().Height;
                currentHeight += decoration.TopMargin;
            }
        }        
    }
}
