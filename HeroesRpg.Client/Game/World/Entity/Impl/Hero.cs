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

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
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
    public sealed partial class Hero : NamedEntity
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
        public Hero(int breedId, int id, string name) : base(id, name)
        {
            Breed = (HeroEnum)breedId;
            BreedData = GetHeroDataById(Breed);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override b2Shape CreatePhysicsShape(int ptm)
        {
            var shape = new b2PolygonShape();
            shape.SetAsBox(ContentSize.Width / ptm, ContentSize.Height / ptm);
            return shape;
        }
    }
}
