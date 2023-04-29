using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class MabelBase : TerraGuardianBase
    {
        /// <summary>
        /// -Objective centered.
        /// -Naive.
        /// -Tries to be like a mother to the Angler.
        /// -Exceeds on caffeine when stressed.
        /// -Loves company.
        /// -Insecure.
        /// -She saw Rococo before, somewhere.
        /// </summary>
        public override string Name => "Mabel";
        public override string Description => "She dreams of being a model. And she still pursues it.";
        public override int Age => 17;
        public override Sizes Size => Sizes.Large;
        public override Genders Gender => Genders.Female;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override float Scale => 92f / 84;
        public override bool CanCrouch => true;
        public override int Width => 30;
        public override int Height => 84;
        public override int CrouchingHeight => 72;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 150; //950
        public override int HealthPerLifeCrystal => 35;
        public override int HealthPerLifeFruit => 15;
        public override float AccuracyPercent => 0.81f;
        public override float MaxRunSpeed => 5.78f;
        public override float RunAcceleration => 0.17f;
        public override float RunDeceleration => 0.39f;
        public override int JumpHeight => 17;
        public override float JumpSpeed => 7.12f;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 3 };
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.BeeKeeper),
                new InitialItemDefinition(ItemID.HealingPotion, 5)
            };
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new MabelDialogues();
        public override void UpdateAttributes(Companion companion)
        {
            companion.DodgeRate += 10;
        }
        //Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation a = new Animation();
                for (short i = 1; i < 9; i++)
                    a.AddFrame(i, 24);
                return a;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation a = new Animation();
                for (short i = 10; i < 14; i++)
                    a.AddFrame(i, 1);
                return a;
            }
        }
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation a = new Animation();
                a.AddFrame(10, 1);
                a.AddFrame(14, 1);
                a.AddFrame(15, 1);
                return a;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(17);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation a = new Animation();
                for(short i = 18; i < 21; i++)
                    a.AddFrame(i, 1);
                return a;
            }
        }
        protected override Animation SetSittingFrames => new Animation(16);
        protected override Animation SetThroneSittingFrames => new Animation(21);
        protected override Animation SetBedSleepingFrames => new Animation(22);
        protected override Animation SetRevivingFrames => new Animation(23);
        protected override Animation SetDownedFrames => new Animation(24);
        protected override Animation SetBackwardStandingFrames => new Animation(25);
        protected override Animation SetBackwardReviveFrames => new Animation(26);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer a = new AnimationFrameReplacer();
                a.AddFrameToReplace(16, 0);
                return a;
            }
        }
        //Animation Frames
        protected override AnimationPositionCollection SetSleepingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(8, 0), true);

                return a;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(19, 14), true);
                a.AddFramePoint2X(17, 19, 20);
                return a;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(24, 37), true);
                a.AddFramePoint(21, 0, 0);
                return a;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 14, 5);
                left.AddFramePoint2X(11, 37, 8);
                left.AddFramePoint2X(12, 39, 20);
                left.AddFramePoint2X(13, 35, 33);
                
                left.AddFramePoint2X(14, 42, 20);
                left.AddFramePoint2X(15, 42, 39);
                
                left.AddFramePoint2X(18, 14, 12);
                left.AddFramePoint2X(19, 37, 15);
                left.AddFramePoint2X(20, 39, 28);
                
                left.AddFramePoint2X(23, 36, 40);

                AnimationPositionCollection right = new AnimationPositionCollection();
                right.AddFramePoint2X(10, 17, 5);
                right.AddFramePoint2X(11, 40, 8);
                right.AddFramePoint2X(12, 42, 20);
                right.AddFramePoint2X(13, 38, 33);
                
                right.AddFramePoint2X(14, 45, 20);
                right.AddFramePoint2X(15, 44, 39);
                
                right.AddFramePoint2X(18, 17, 12);
                right.AddFramePoint2X(19, 40, 15);
                right.AddFramePoint2X(20, 42, 27);

                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(23, 12), true);
                a.AddFramePoint2X(14, 36, 21);
                a.AddFramePoint2X(15, 38, 31);
                
                a.AddFramePoint2X(17, 23, 19);
                
                a.AddFramePoint2X(23, 34, 28);
                return a;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(4, -5), true);
                a.AddFramePoint2X(21, -8, -16);
                return a;
            }
        }
    }
}