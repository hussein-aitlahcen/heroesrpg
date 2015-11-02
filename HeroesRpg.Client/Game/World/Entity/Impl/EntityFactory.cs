using HeroesRpg.Common.Generic;
using HeroesRpg.Protocol.Enum;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Client.Game.World.Entity.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class EntityFactory : Singleton<EntityFactory>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="entityData"></param>
        /// <returns></returns>
        public GameObject CreateFromNetwork(EntityTypeEnum type, byte[] entityData)
        {
            var reader = new BinaryReader(new MemoryStream(entityData));
            GameObject obj = null;
            switch (type)
            {
                case EntityTypeEnum.HERO:
                    var heroType = (HeroTypeEnum)reader.ReadInt32();
                    switch (heroType)
                    {
                        case HeroTypeEnum.DRAGON_BALL:
                            obj = new DragonBallHero();
                            break;
                    }
                    break;
            }
            if(obj != null)
                obj.FromNetwork(reader);
            return obj;
        }
    }
}
