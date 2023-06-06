using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions
{
    public class DominoBase : TerraGuardianBase
    {
        public override string Name => "Domino";
        public override string Description => "A sly smuggler from the Ether Realm.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 28;
        public override int Height => 84;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override float Scale => 96f / 84;
        public override int Age => 26;
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 200; //1000
        public override int HealthPerLifeCrystal => 20;
        public override int HealthPerLifeFruit => 25;
        public override float AccuracyPercent => .97f;
        public override float MaxRunSpeed => 5.6f;
        public override float RunAcceleration => 0.22f;
        public override float RunDeceleration => 0.37f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 6.45f;
        public override bool CanCrouch => true;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override bool IsNocturnal => true;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 2, MountUnlock = 6 };
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.Handgun),
                new InitialItemDefinition(ItemID.HealingPotion, 5),
                new InitialItemDefinition(ItemID.MusketBall, 999)
            };
        }
        public override void UpdateAttributes(Companion companion)
        {
            TerraGuardian tg = companion as TerraGuardian;
            if (tg.HeldItems[1].ItemAnimation == 0 && MainMod.IsDualWieldableWeapon(companion.HeldItem.type))
            {
                tg.HeldItems[1].IsActionHand = true;
                tg.HeldItems[1].SelectedItem = tg.selectedItem;
            }
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new DominoDialogues();
        public override BehaviorBase PreRecruitmentBehavior => new Companions.Domino.DominoRecruitmentBehavior();
        #region Animations
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
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 13; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(26);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 14; i <= 16; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(17);
        protected override Animation SetChairSittingFrames => new Animation(18);
        protected override Animation SetThroneSittingFrames => new Animation(19);
        protected override Animation SetBedSleepingFrames => new Animation(20);
        protected override Animation SetRevivingFrames => new Animation(21);
        protected override Animation SetDownedFrames => new Animation(22);
        protected override Animation SetBackwardStandingFrames => new Animation(24);
        protected override Animation SetBackwardReviveFrames => new Animation(25);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(17, 0);
                anim.AddFrameToReplace(18, 1);
                anim.AddFrameToReplace(23, 0);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(26, 0);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(18, 15), true);
                anim.AddFramePoint2X(26, 23, 22);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(17, 25, 34);
                anim.AddFramePoint2X(18, 25, 34);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 16, 5);
                left.AddFramePoint2X(11, 32, 11);
                left.AddFramePoint2X(12, 35, 20);
                left.AddFramePoint2X(13, 31, 28);
                
                left.AddFramePoint2X(14, 25, 13);
                left.AddFramePoint2X(15, 38, 19);
                left.AddFramePoint2X(16, 36, 32);
                
                left.AddFramePoint2X(21, 44, 38);

                right.AddFramePoint2X(10, 19, 5);
                right.AddFramePoint2X(11, 36, 11);
                right.AddFramePoint2X(12, 40, 20);
                right.AddFramePoint2X(13, 36, 28);
                
                right.AddFramePoint2X(14, 30, 13);
                right.AddFramePoint2X(15, 42, 19);
                right.AddFramePoint2X(16, 39, 32);

                return new AnimationPositionCollection[] {left, right};
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(23, 10), true);
                anim.AddFramePoint2X(27, 29, 20);
                anim.AddFramePoint2X(21, 29, 20);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset =>new AnimationPositionCollection(new Vector2(16, 0), false);
        #endregion
    }
}