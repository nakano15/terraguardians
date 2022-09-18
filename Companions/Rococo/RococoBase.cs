using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions
{
    public class RococoBase : CompanionBase
    {
        public override string Name => "Rococo";
        public override string Description => "";
        public override int Age => 15;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override int Width => 28;
        public override int Height => 86;
        public override float Scale => 94f / 86;
        public override int InitialMaxHealth => 200;
        public override int HealthPerLifeCrystal => 40;
        public override int HealthPerLifeFruit => 10;
        //public override float Gravity => 0.5f;
        public override float MaxRunSpeed => 5.2f;
        public override float RunAcceleration => 0.18f;
        public override float RunDeceleration => 0.47f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.08f;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        #region  Animations
        protected override Animation SetWalkingFrames {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24); //8
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames 
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 16; i <= 19; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        #endregion
    }
}