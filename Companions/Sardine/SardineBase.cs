using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class SardineBase : CompanionBase
    {
        public override string Name => "Sardine";
        public override string Description => "He's an adventurer that has visited many worlds,\nearns his life as a bounty hunter. But actually forgot\nwhich world his house is at.";
        public override int Age => 25;
        public override int SpriteWidth => 72;
        public override int SpriteHeight => 56;
        public override int FramesInRow => 26;
        public override bool CanCrouch => false;
        public override int Width => 14;
        public override int Height => 38;
        public override float Scale => 34f / 38;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 80; //320
        public override int HealthPerLifeCrystal => 12;
        public override int HealthPerLifeFruit => 3;
        //public override float Gravity => 0.5f;
        public override float MaxRunSpeed => 4.82f;
        public override float RunAcceleration => 0.15f;
        public override float RunDeceleration => 0.5f;
        public override int JumpHeight => 12;
        public override float JumpSpeed => 9.76f;
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override SoundStyle HurtSound => Terraria.ID.SoundID.NPCHit51;
        public override SoundStyle DeathSound => Terraria.ID.SoundID.NPCDeath54;
        public override MountStyles MountStyle => MountStyles.CompanionRidesPlayer;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MoveInUnlock = 0, VisitUnlock = 1 };
        protected override CompanionDialogueContainer GetDialogueContainer => new SardineDialogues();
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
                for(short i = 13; i <= 16; i++)
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
        protected override Animation SetSittingFrames => new Animation(17);
        protected override Animation SetPlayerMountedArmFrame => new Animation(17);
        protected override Animation SetChairSittingFrames => new Animation(18);
        protected override Animation SetThroneSittingFrames => new Animation(19);
        protected override Animation SetBedSleepingFrames => new Animation(20);
        protected override Animation SetRevivingFrames => new Animation(22);
        protected override Animation SetDownedFrames => new Animation(21);
        protected override Animation SetPetrifiedFrames => new Animation(23);
        protected override Animation SetBackwardStandingFrames => new Animation(24);
        protected override Animation SetBackwardReviveFrames => new Animation(25);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer f = new AnimationFrameReplacer();
                f.AddFrameToReplace(17, 0);
                return f;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    new AnimationPositionCollection(), 
                    new AnimationPositionCollection()
                };
                //Left Arm
                Hands[0].AddFramePoint2X(10, 10, 12);
                Hands[0].AddFramePoint2X(11, 27, 14);
                Hands[0].AddFramePoint2X(12, 31, 26);
                
                Hands[0].AddFramePoint2X(13, 12, 9);
                Hands[0].AddFramePoint2X(14, 22, 12);
                Hands[0].AddFramePoint2X(15, 25, 18);
                Hands[0].AddFramePoint2X(16, 21, 23);
                
                Hands[0].AddFramePoint2X(17, 16, 18);
                
                Hands[0].AddFramePoint2X(22, 21, 23);
                
                //Right Arm
                Hands[1].AddFramePoint2X(10, 12, 12);
                Hands[1].AddFramePoint2X(11, 29, 14);
                Hands[1].AddFramePoint2X(12, 33, 26);
                
                Hands[1].AddFramePoint2X(13, 15, 4);
                Hands[1].AddFramePoint2X(14, 24, 12);
                Hands[1].AddFramePoint2X(15, 27, 18);
                Hands[1].AddFramePoint2X(16, 23, 23);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition 
        {
            get
            {
                AnimationPositionCollection animation = new AnimationPositionCollection(new Vector2(16, 25), true);
                return animation;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition => new AnimationPositionCollection(new Vector2(17, 25), true);
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 14);
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(3, -4), true);
                a.AddFramePoint2X(19, -2, -13);
                return a;
            }
        }
        #endregion
    }
}