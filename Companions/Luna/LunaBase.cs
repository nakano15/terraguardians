using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class LunaBase : TerraGuardianBase
    {
        public override string Name => "Luna";
        public override string FullName => "Luna Crescent";
        public override string Description => "She can tell you about almost everything related to TerraGuardians.\nGo ahead, ask it.";
        public override int Age => 19;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Autumn, 17);
        public override Sizes Size => Sizes.Large;
        public override Genders Gender => Genders.Female;
        public override bool HelpAlliesOverFighting => true;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override int Width => 24;
        public override int Height => 84;
        public override int CrouchingHeight => 70;
        public override float Scale => 101f / 84;
        public override int InitialMaxHealth => 200; //950
        public override int HealthPerLifeCrystal => 30;
        public override int HealthPerLifeFruit => 15;
        public override float AccuracyPercent => 0.63f;
        public override float MaxRunSpeed => 5f;
        public override float RunAcceleration => 0.2f;
        public override float RunDeceleration => 0.53f;
        public override float JumpSpeed => 5.83f;
        public override bool CanCrouch => true;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ VisitUnlock = 0, MoveInUnlock = 0, FollowerUnlock = 0 };
        protected override CompanionDialogueContainer GetDialogueContainer => new LunaDialogues();
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.CopperBroadsword),
                new InitialItemDefinition(ItemID.RichMahoganyBow),
                new InitialItemDefinition(ItemID.Mushroom, 5),
                new InitialItemDefinition(ItemID.WoodenArrow, 250)
            };
        }
        public override bool DrawBehindWhenSharingBed => true;
        public override bool CanSpawnNpc()
        {
            return WorldMod.GetTerraGuardiansCount > 0;
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(2);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 3; i <= 10; i++)
                {
                    anim.AddFrame(i, 24);
                }
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(11);
        protected override Animation SetPlayerMountedArmFrame => new Animation(11);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 16; i <= 18; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 12; i <= 15; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(19);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 20; i <= 22; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(24);
        protected override Animation SetChairSittingFrames => new Animation(23);
        protected override Animation SetThroneSittingFrames => new Animation(25);
        protected override Animation SetBedSleepingFrames => new Animation(26);
        protected override Animation SetRevivingFrames => new Animation(27);
        protected override Animation SetDownedFrames => new Animation(28);
        protected override Animation SetBackwardStandingFrames => new Animation(29);
        protected override Animation SetBackwardReviveFrames => new Animation(30);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(23, 0);
                anim.AddFrameToReplace(24, 0);
                anim.AddFrameToReplace(25, 1);
                //anim.AddFrameToReplace(31, 0);
                return anim;
            }
        }
        
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                left.AddFrameToReplace(25, 0);

                right.AddFrameToReplace(0, 0);
                right.AddFrameToReplace(1, 1);
                right.AddFrameToReplace(23, 1);
                right.AddFrameToReplace(24, 1);
                right.AddFrameToReplace(25, 2);
                right.AddFrameToReplace(31, 3);
                return new AnimationFrameReplacer[]{ left, right };
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(20 + 2, 34, true);
                anim.AddFramePoint(25, 0, 0);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection LeftArm = new AnimationPositionCollection(),
                    RightArm = new AnimationPositionCollection();
                LeftArm.AddFramePoint2X(12, 13, 3);
                LeftArm.AddFramePoint2X(13, 33, 12);
                LeftArm.AddFramePoint2X(14, 36, 22);
                LeftArm.AddFramePoint2X(15, 30, 31);
                
                LeftArm.AddFramePoint2X(16, 30, 4);
                LeftArm.AddFramePoint2X(17, 40, 18);
                LeftArm.AddFramePoint2X(18, 36, 42);
                
                LeftArm.AddFramePoint2X(20, 11, 17);
                LeftArm.AddFramePoint2X(21, 34, 18);
                LeftArm.AddFramePoint2X(22, 30, 33);
                
                LeftArm.AddFramePoint2X(24, 29, 28);
                
                LeftArm.AddFramePoint2X(27, 33, 43);
                LeftArm.AddFramePoint2X(30, 33, 43);

                RightArm.AddFramePoint2X(12, 16, 3);
                RightArm.AddFramePoint2X(13, 35, 12);
                RightArm.AddFramePoint2X(14, 38, 22);
                RightArm.AddFramePoint2X(15, 32, 31);
                
                RightArm.AddFramePoint2X(16, 32, 4);
                RightArm.AddFramePoint2X(17, 42, 18);
                RightArm.AddFramePoint2X(18, 38, 42);
                
                RightArm.AddFramePoint2X(20, 13, 17);
                RightArm.AddFramePoint2X(21, 36, 18);
                RightArm.AddFramePoint2X(22, 32, 33);
                
                RightArm.AddFramePoint2X(27, 41, 43);
                RightArm.AddFramePoint2X(30, 41, 43);
                return new AnimationPositionCollection[]{ LeftArm, RightArm };
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(16, 16, true);
                anim.AddFramePoint2X(17, 22, 20);
                anim.AddFramePoint2X(18, 24, 25);
                
                anim.AddFramePoint2X(19, 16, 23);
                anim.AddFramePoint2X(20, 16, 23);
                anim.AddFramePoint2X(21, 16, 23);
                anim.AddFramePoint2X(22, 16, 23);
                
                anim.AddFramePoint2X(25, 17, 19);
                
                anim.AddFramePoint2X(27, 25, 27);
                anim.AddFramePoint2X(30, 25, 27);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(22, 12, true);
                anim.AddFramePoint2X(17, 28, 18);
                anim.AddFramePoint2X(18, 30, 24);
                
                anim.AddFramePoint2X(19, 22, 19);
                anim.AddFramePoint2X(20, 22, 19);
                anim.AddFramePoint2X(21, 22, 19);
                anim.AddFramePoint2X(22, 22, 19);
                
                anim.AddFramePoint2X(27, 32, 27);
                anim.AddFramePoint2X(30, 32, 27);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset 
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(8 - 2, -4, true);
                anim.AddFramePoint2X(25, -11, -16);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0, false);
        #endregion
        public override void ModifyAnimation(Companion companion)
        {
            if (companion.sitting.isSitting && companion.BodyFrameID != 25 && companion.Owner != null)
            {
                Player p = companion.Owner;
                if (p.sitting.isSitting && p.Bottom == companion.Bottom)
                {
                    companion.BodyFrameID = 31;
                    if (companion.itemAnimation == 0)
                        companion.ArmFramesID[0] = 31;
                    companion.ArmFramesID[1] = 31;
                    return;
                }
            }
            if (companion.IsMountedOnSomething || companion.itemAnimation > 0) return;
            for(byte i = 0; i < companion.ArmFramesID.Length; i++)
            {
                short Frame = companion.ArmFramesID[i];
                if (Frame < 11)
                {
                    if (Frame == 4 || Frame == 8)
                        companion.ArmFramesID[i] = 1;
                    else
                        companion.ArmFramesID[i] = 0;
                }
            }
        }
    }
}