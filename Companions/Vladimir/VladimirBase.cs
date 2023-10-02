using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class VladimirBase : TerraGuardianBase
    {
        public static List<CompanionID> CarryBlacklist = new List<CompanionID>();
        public override string Name => "Vladimir";
        public override string FullName => "Vladimir Svirepyy Varvar"; //Surnames means Ferocious Barbarian
        public override string Description => "A bear TerraGuardian that likes giving hugs to people.";
        public override Sizes Size => Sizes.Large;
        public override bool HelpAlliesOverFighting => true;
        public override int Width => 44;
        public override int Height => 116;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 128;
        public override int SpriteHeight => 160;
        public override float Scale => 138f / 116f;
        public override int FramesInRow => 15;
        public override int Age => 26;
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 250; //1600
        public override int HealthPerLifeCrystal => 50;
        public override int HealthPerLifeFruit => 30;
        public override float AccuracyPercent => 0.72f;
        public override float Gravity => 0.7f;
        public override float MaxRunSpeed => 4.9f;
        public override float RunAcceleration => 0.14f;
        public override float RunDeceleration => 0.42f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.16f;
        public override bool CanCrouch => true;
        public override Companion GetCompanionObject => new Vladimir.VladimirCompanion();
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override PartDrawOrdering MountedDrawOrdering => PartDrawOrdering.InBetween;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0, MountUnlock = 3, MoveInUnlock = 0 };
        protected override CompanionDialogueContainer GetDialogueContainer => new VladimirDialogues();
        public override BehaviorBase DefaultFollowLeaderBehavior => new Vladimir.VladimirFollowerBehavior();
        public override BehaviorBase PreRecruitmentBehavior => new Vladimir.VladimirPreRecruitBehavior();

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.WoodenSword),
                new InitialItemDefinition(ItemID.Mushroom, 3)
            };
        }

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 2; i < 10; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(10);
        protected override Animation SetPlayerMountedArmFrame => new Animation(1);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(13, 1);
                anim.AddFrame(14, 1);
                anim.AddFrame(19, 1);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 13; i < 17; i++)
                {
                    anim.AddFrame(i, 1);
                }
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(11);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 17; i < 20; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(20);
        protected override Animation SetChairSittingFrames => new Animation(21);
        protected override Animation SetThroneSittingFrames => new Animation(22);
        protected override Animation SetBedSleepingFrames => new Animation(24);
        protected override Animation SetDownedFrames => new Animation(26);
        protected override Animation SetRevivingFrames => new Animation(27);
        protected override Animation SetBackwardStandingFrames => new Animation(29);
        protected override Animation SetBackwardReviveFrames => new Animation(31);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(20, 0);
                anim.AddFrameToReplace(21, 1);
                anim.AddFrameToReplace(23, 2);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(1, 0);
                right.AddFrameToReplace(12, 1);
                right.AddFrameToReplace(20, 2);
                right.AddFrameToReplace(23, 3);
                right.AddFrameToReplace(25, 4);
                right.AddFrameToReplace(28, 5);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        #endregion

        #region Animation Position
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(20, 30, 62);
                anim.AddFramePoint2X(21, 30, 62);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(13, 21, 14);
                left.AddFramePoint2X(14, 45, 26);
                left.AddFramePoint2X(15, 52, 40);
                left.AddFramePoint2X(16, 44, 56);

                left.AddFramePoint2X(17, 21, 20);
                left.AddFramePoint2X(18, 45, 32);
                left.AddFramePoint2X(19, 44, 62);
                
                left.AddFramePoint2X(23, 32, 58);
                left.AddFramePoint2X(25, 23, 72);
                
                left.AddFramePoint2X(27, 44, 71);
                
                left.AddFramePoint2X(28, 42, 65);
                
                left.AddFramePoint2X(32, 42, 65);

                right.AddFramePoint2X(13, 35, 14);
                right.AddFramePoint2X(14, 48, 26);
                right.AddFramePoint2X(15, 55, 40);
                right.AddFramePoint2X(16, 48, 56);
                
                right.AddFramePoint2X(17, 35, 20);
                right.AddFramePoint2X(18, 48, 32);
                right.AddFramePoint2X(19, 48, 62);
                
                right.AddFramePoint2X(23, 32, 58);
                right.AddFramePoint2X(25, 40, 72);
                
                right.AddFramePoint2X(27, 51, 71);
                
                right.AddFramePoint2X(28, 42, 65);
                
                right.AddFramePoint2X(32, 42, 65);
                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(39, 46), true);
                anim.AddFramePoint2X(11, 39, 52);
                anim.AddFramePoint2X(12, 39, 52);
                
                anim.AddFramePoint2X(23, 32, 58);
                anim.AddFramePoint2X(25, 23, 70);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(30, 28), true);
                anim.AddFramePoint2X(11, 30, 34);
                anim.AddFramePoint2X(12, 30, 34);
                anim.AddFramePoint2X(17, 30, 34);
                anim.AddFramePoint2X(18, 30, 34);
                anim.AddFramePoint2X(19, 30, 34);
                
                anim.AddFramePoint2X(23, -1000, -1000);
                anim.AddFramePoint2X(25, -1000, -1000);
                
                anim.AddFramePoint2X(27, 50, 47);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetWingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(23, -1000, -1000);
                anim.AddFramePoint(25, -1000, -1000);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(21, 8, -8);
                anim.AddFramePoint2X(22, -12, -18);
                anim.AddFramePoint2X(23, -12, -18);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset 
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(0, 2), true);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 48, false);
        #endregion
    }
}