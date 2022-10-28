using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;

namespace terraguardians.Companions
{
    public class BlueBase : CompanionBase
    {
        public override string Name => "Blue";
        public override string Description => "";
        public override int Age => 17;
        public override Genders Gender => Genders.Female;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override bool CanCrouch => true;
        public override int Width => 26;
        public override int Height => 82;
        public override float Scale => 99f / 82;
        public override int InitialMaxHealth => 175; //1150
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 15;
        public override float MaxRunSpeed => 4.75f;
        public override float RunAcceleration => 0.13f;
        public override float RunDeceleration => 0.5f;
        public override int JumpHeight => 19;
        public override float JumpSpeed => 7.52f;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override SoundStyle HurtSound => Terraria.ID.SoundID.NPCHit6;
        //public override SoundStyle DeathSound => Terraria.ID.SoundID.DD2_KoboldDeath;
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
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 12; i++) anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(20);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(21, 1);
                anim.AddFrame(22, 1);
                anim.AddFrame(23, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(24);
        protected override Animation SetPlayerMountedArmFrame => new Animation(25);
        protected override Animation SetThroneSittingFrames => new Animation(27);
        protected override Animation SetBedSleepingFrames => new Animation(28);
        protected override Animation SetRevivingFrames => new Animation(33);
        protected override Animation SetDownedFrames => new Animation(32);
        protected override Animation SetPetrifiedFrames => new Animation(34);
        protected override Animation SetBackwardStandingFrames => new Animation(35);
        protected override Animation SetBackwardReviveFrames => new Animation(37);
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    new AnimationPositionCollection(new Vector2(18, 31), true), 
                    new AnimationPositionCollection(new Vector2(30, 31), true)
                };
                //Left Arm
                Hands[0].AddFramePoint2X(10, 6, 14);
                Hands[0].AddFramePoint2X(11, 40, 9);
                Hands[0].AddFramePoint2X(12, 43, 41);
                
                Hands[0].AddFramePoint2X(16, 12, 5);
                Hands[0].AddFramePoint2X(17, 30, 7);
                Hands[0].AddFramePoint2X(18, 37, 19);
                Hands[0].AddFramePoint2X(19, 31, 32);
                
                Hands[0].AddFramePoint2X(21, 43, 22);
                Hands[0].AddFramePoint2X(22, 43, 31);
                Hands[0].AddFramePoint2X(23, 40, 42);
                
                Hands[0].AddFramePoint2X(33, 43, 43);
                
                //Right Arm
                Hands[1].AddFramePoint2X(10, 9, 14);
                Hands[1].AddFramePoint2X(11, 42, 9);
                Hands[1].AddFramePoint2X(12, 45, 41);
                
                Hands[1].AddFramePoint2X(16, 15, 5);
                Hands[1].AddFramePoint2X(17, 34, 7);
                Hands[1].AddFramePoint2X(18, 39, 19);
                Hands[1].AddFramePoint2X(19, 33, 32);
                
                Hands[1].AddFramePoint2X(21, 42, 22);
                Hands[1].AddFramePoint2X(22, 45, 31);
                Hands[1].AddFramePoint2X(23, 43, 42);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16);
        #endregion
    }
}