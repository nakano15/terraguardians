using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class AlexBase : TerraGuardianBase
    {
        public AlexBase() : base()
        {
            VladimirBase.CarryBlacklist.Add(new CompanionID(CompanionDB.Alex));
        }

        public override string Name => "Alex";
        public override string Description => "Your new best friend - a very playful one.";
        public override int Age => 42;
        public override Sizes Size => Sizes.Large;
        public override Genders Gender => Genders.Male;
        public override CompanionGroup GetCompanionGroup => MainMod.GetGiantDogGroup;
        public override bool HelpAlliesOverFighting => true;
        public override int Width => 68;
        public override int Height => 62;
        public override int CrouchingHeight => 52;
        public override bool CanCrouch => false;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override int InitialMaxHealth => 175; //1125
        public override int HealthPerLifeCrystal => 30;
        public override int HealthPerLifeFruit => 25;
        public override float AccuracyPercent => 0.36f;
        public override float MaxFallSpeed => 0.7f;
        public override float MaxRunSpeed => 5.65f;
        public override float RunAcceleration => 0.36f;
        public override float RunDeceleration => 0.83f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 6.71f;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MoveInUnlock = 0, VisitUnlock = 0 };
        public override bool DrawBehindWhenSharingBed => true;
        public override bool DrawBehindWhenSharingChair => true;
        protected override CompanionDialogueContainer GetDialogueContainer => new AlexDialogues();
        public override BehaviorBase PreRecruitmentBehavior => new Companions.Alex.AlexPreRecruitBehavior();
        #region  Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i <= 13; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetPlayerMountedArmFrame => new Animation(-1);
        protected override Animation SetSittingFrames => new Animation(14);
        protected override Animation SetChairSittingFrames => new Animation(31);
        protected override Animation SetSittingItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 15; i <= 17; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetThroneSittingFrames => new Animation(22);
        protected override Animation SetBedSleepingFrames => new Animation(23);
        protected override Animation SetRevivingFrames => new Animation(20);
        protected override Animation SetDownedFrames => new Animation(29);
        protected override Animation SetPetrifiedFrames => new Animation(30);
        protected override Animation SetBackwardStandingFrames => new Animation(32);
        protected override Animation SetBackwardReviveFrames => new Animation(33);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim  = new AnimationFrameReplacer();
                anim.AddFrameToReplace(14, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(new Vector2(8, 0), true);
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(30, 25), true);
                anim.AddFramePoint2X(14, 13, 20);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(20, 41), true);
                anim.AddFramePoint(22, 0, 0);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection main = new AnimationPositionCollection();
                main.AddFramePoint2X(10, 37, 28);
                main.AddFramePoint2X(11, 41, 31);
                main.AddFramePoint2X(12, 42, 37);
                main.AddFramePoint2X(13, 39, 43);
                
                main.AddFramePoint2X(15, 30, 15);
                main.AddFramePoint2X(16, 35, 28);
                main.AddFramePoint2X(17, 28, 37);
                
                main.AddFramePoint2X(19, 38, 47);
                main.AddFramePoint2X(20, 38, 47);
                main.AddFramePoint2X(21, 38, 47);
                main.AddFramePoint2X(24, 38, 47);
                main.AddFramePoint2X(25, 38, 47);
                main.AddFramePoint2X(26, 38, 47);
                main.AddFramePoint2X(27, 38, 47);
                main.AddFramePoint2X(28, 38, 47);
                return new AnimationPositionCollection[]{ main };
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(38, 23), true);
                anim.AddFramePoint2X(14, 27, 18);
                anim.AddFramePoint2X(15, 27, 18);
                anim.AddFramePoint2X(16, 27, 18);
                anim.AddFramePoint2X(17, 27, 18);
                anim.AddFramePoint2X(18, 27, 18);
                anim.AddFramePoint2X(22, 23, 17);
                
                anim.AddFramePoint2X(19, -1000, -1000);
                anim.AddFramePoint2X(20, -1000, -1000);
                anim.AddFramePoint2X(21, -1000, -1000);
                anim.AddFramePoint2X(24, -1000, -1000);
                anim.AddFramePoint2X(25, -1000, -1000);
                anim.AddFramePoint2X(26, -1000, -1000);
                anim.AddFramePoint2X(27, -1000, -1000);
                anim.AddFramePoint2X(28, -1000, -1000);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(4, -8), true);
                anim.AddFramePoint2X(22, -12, -14);
                return anim;
            }
        }
        #endregion

        protected override void SetupSkinsOutfitsContainer(ref Dictionary<byte, CompanionSkinInfo> Skins, ref Dictionary<byte, CompanionSkinInfo> Outfits)
        {
            Skins.Add(1, new Alex.AndroidSkin());
        }
    }
}