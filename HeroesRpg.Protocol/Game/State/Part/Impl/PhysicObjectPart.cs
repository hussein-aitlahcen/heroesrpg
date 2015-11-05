using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeroesRpg.Protocol.Game.State.Part.Impl
{
    public sealed class PhysicObjectPart : StatePart
    {      
        /// <summary>
         /// 
        /// </summary>
        public override StatePartTypeEnum Type
        {
            get
            {
                return StatePartTypeEnum.PHYSIC_OBJECT;
            }
        }
        
        public bool Bullet { get; private set; }
        public float GravityScale { get; private set; }
        public float LinearDamping { get; private set; }
        public float Mass { get; private set; }
        public float Density { get; private set; }
        public float Friction { get; private set; }
        public bool FixedRotation { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public PhysicObjectPart()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="mass"></param>
        /// <param name="density"></param>
        /// <param name="friction"></param>
        /// <param name="fixedRotation"></param>
        public PhysicObjectPart(bool bullet, float gravityScale, float linearDamping, float mass, float density, float friction, bool fixedRotation)
        {
            Bullet = bullet;
            GravityScale = gravityScale;
            LinearDamping = linearDamping;
            Mass = mass;
            Density = density;
            Friction = friction;
            FixedRotation = fixedRotation;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="reader"></param>
        public override void FromNetwork(BinaryReader reader)
        {
            Bullet = reader.ReadBoolean();
            GravityScale = reader.ReadSingle();
            LinearDamping = reader.ReadSingle();
            Mass = reader.ReadSingle();
            Density = reader.ReadSingle();
            Friction = reader.ReadSingle();
            FixedRotation = reader.ReadBoolean();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="writer"></param>
        public override void ToNetwork(BinaryWriter writer)
        {
            writer.Write(Bullet);
            writer.Write(GravityScale);
            writer.Write(LinearDamping);
            writer.Write(Mass);
            writer.Write(Density);
            writer.Write(Friction);
            writer.Write(FixedRotation);
        }
    }
}
