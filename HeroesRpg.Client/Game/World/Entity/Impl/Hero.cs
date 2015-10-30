using CocosSharp;
using HeroesRpg.Client.Data.Actor;
using HeroesRpg.Client.Game.Graphic.Element;
using HeroesRpg.Client.Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Box2D.Collision.Shapes;
using HeroesRpg.Client.Game.World.Entity.Impl.Decoration;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public enum HeroEnum
    {
        BROLY = 0,
    }

    /// <summary>
    /// 
    /// </summary>
    public partial class Hero
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="breed"></param>
        /// <returns></returns>
        public static HeroData GetHeroDataById(HeroEnum breed)
        {
            switch(breed)
            {
                case HeroEnum.BROLY:
                    return ActorManager.Instance.GetActorByName<HeroData>(HeroData.BROLY);
                default:
                    throw new UnknowHeroBreedException("unknow hero id : " + (int)breed);
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed partial class Hero : CombatEntity
    {
        /// <summary>
        /// 
        /// </summary>
        public HeroEnum Breed
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public HeroData BreedData
        {
            get;
            private set;
        }

        /// <summary>
        /// 
        /// </summary>
        public override CCSpriteSheet SpriteSheet => BreedData.SpriteSheet;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="heroType"></param>
        /// <param name="id"></param>
        public Hero(int breedId, int id, string name) : base(id)
        {
            Breed = (HeroEnum)breedId;
            BreedData = GetHeroDataById(Breed);

            AddDecoration(new NameDecoration(name, CCColor3B.Orange));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape()
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(ScaledContentSize.Width / 2 / PtmRatio, ScaledContentSize.Height / 2 / PtmRatio);
            return shape;
        }
    }
}
