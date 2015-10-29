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
    public enum DecorationTypeEnum: int
    {
        NAME = 0,
        LIFE = 1,
    }

    /// <summary>
    /// 
    /// </summary>
    public interface IEntityDecoration
    {
        float TopMargin { get; }
        float BottomMargin { get; }
        int DecorationType { get; }
        CCNode Node { get; }
        CCSize GetContentSize();
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class EntityDecoration<T> : IEntityDecoration
        where T : CCNode
    {
        /// <summary>
        /// 
        /// </summary>
        public T Node
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public int DecorationType
        {
            get;
            private set;
        }
        
        /// <summary>
        /// 
        /// </summary>
        public abstract float TopMargin { get; }

        /// <summary>
        /// 
        /// </summary>
        public abstract float BottomMargin { get; }

        /// <summary>
        /// 
        /// </summary>
        CCNode IEntityDecoration.Node => Node;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        public EntityDecoration(DecorationTypeEnum type, T node)
        {
            DecorationType = (int)type;
            Node = node;
            Node.AnchorPoint = CCPoint.AnchorMiddle;
        }

        /// <summary>
        /// 
        /// </summary>
        public virtual void Update()
        {
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public CCSize GetContentSize()
        {
            var size = Node.ScaledContentSize;
            return new CCSize(size.Width, size.Height);
        }
    }
}
