using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions
{
    public class MinervaBase : TerraGuardianBase
    {
        public override string Name => "Minerva";
        public override string Description => "She's not very sociable, but is\na good cook. If you're feeling\nhungry, go see her to get food.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 29;
        public override int Height => 90;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 112;
        public override float Scale => 108f / 90;
        public override int Age => 19;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Winter, 9);
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 300; //1000
        public override int HealthPerLifeCrystal => 40;
        public override int HealthPerLifeFruit => 20;
        public override float AccuracyPercent => 0.47f;
        public override float MaxFallSpeed => 0.62f;
        public override float MaxRunSpeed => 4.8f;
        public override float RunAcceleration => 0.16f;
        public override float RunDeceleration => 0.52f;
        public override int JumpHeight => 16;
        public override float JumpSpeed => 7.6f;
        public override bool CanCrouch => true;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override CompanionDialogueContainer GetDialogueContainer => new MinervaDialogues();

        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 2, MoveInUnlock = 3 };
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.PearlwoodSword),
                new InitialItemDefinition(ItemID.AmethystStaff),
                new InitialItemDefinition(ItemID.BottledHoney, 5),
                new InitialItemDefinition(ItemID.BowlofSoup, 5)
            };
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short f = 2; f <= 9; f++)
                {
                    anim.AddFrame(f, 24);
                }
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(10);
        protected override Animation SetPlayerMountedArmFrame => new Animation(10);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 21; i <= 23; i++)
                {
                    anim.AddFrame(i);
                }
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 11; i <= 14; i++)
                {
                    anim.AddFrame(i);
                }
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(24);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 25; i <= 27; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(16);
        protected override Animation SetChairSittingFrames => new Animation(15);
        protected override Animation SetThroneSittingFrames => new Animation(18);
        protected override Animation SetBedSleepingFrames => new Animation(17);
        protected override Animation SetRevivingFrames => new Animation(20);
        protected override Animation SetDownedFrames => new Animation(19);
        protected override Animation SetBackwardStandingFrames => new Animation(28);
        protected override Animation SetBackwardReviveFrames => new Animation(29);
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(0, 0);
                right.AddFrameToReplace(15, 1);
                right.AddFrameToReplace(16, 1);
                right.AddFrameToReplace(20, 2);
                right.AddFrameToReplace(21, 3);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(15, 0);
                anim.AddFrameToReplace(16, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Position
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                for (short i = 15; i <= 16; i++)
                {
                    anim.AddFramePoint2X(i, 22, 43);
                }
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16, false);
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(11, 14, 7);
                left.AddFramePoint2X(12, 36, 14);
                left.AddFramePoint2X(13, 41, 26);
                left.AddFramePoint2X(14, 33, 36);
                
                left.AddFramePoint2X(20, 30, 47);
                
                left.AddFramePoint2X(21, 21, 15);
                left.AddFramePoint2X(22, 38, 22);
                left.AddFramePoint2X(23, 34, 43);
                
                left.AddFramePoint2X(25, 24, 19);
                left.AddFramePoint2X(26, 34, 30);
                left.AddFramePoint2X(27, 35, 45);

                right.AddFramePoint2X(11, 17, 7);
                right.AddFramePoint2X(12, 38, 14);
                right.AddFramePoint2X(13, 43, 26);
                right.AddFramePoint2X(14, 35, 36);
                
                right.AddFramePoint2X(20, 34, 47);
                
                right.AddFramePoint2X(21, 24, 15);
                right.AddFramePoint2X(22, 41, 722);
                right.AddFramePoint2X(23, 36, 43);
                
                right.AddFramePoint2X(25, 26, 19);
                right.AddFramePoint2X(26, 37, 30);
                right.AddFramePoint2X(27, 37, 45);

                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(18, 21, true);
                anim.AddFramePoint2X(20, 23, 33);
                anim.AddFramePoint2X(24, 18, 35);
                anim.AddFramePoint2X(25, 18, 35);
                anim.AddFramePoint2X(26, 18, 35);
                anim.AddFramePoint2X(27, 18, 35);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(24, 17, true);
                anim.AddFramePoint2X(20, 28, 28);
                
                anim.AddFramePoint2X(23, 28, 22);
                
                for (short i = 24; i <= 27; i++)
                    anim.AddFramePoint2X(i, 24, 31);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                for (short i = 15; i <= 16; i++)
                {
                    anim.AddFramePoint2X(i, 5, -5);
                }
                anim.AddFramePoint2X(18, -12, -16);
                return anim;
            }
        }
        #endregion
        public override void ModifyAnimation(Companion companion)
        {
            if (companion.GetCharacterMountedOnMe != null)
            {
                if (companion.ArmFramesID[1] == 0)
                    companion.ArmFramesID[1] = 1;
            }
        }
    }
}