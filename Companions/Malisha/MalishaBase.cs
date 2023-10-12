using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using terraguardians.Companions.Malisha;

namespace terraguardians.Companions
{
    public class MalishaBase : TerraGuardianBase
    {
        public const string TailTextureID = "tail";

        public static List<CompanionID> CarryBlacklist = new List<CompanionID>();
        public override string Name => "Malisha";
        public override string Description => "Two things are important for her: Practicing Magic and Experimenting.\nDon't volunteer.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 28;
        public override int Height => 84;
        public override int CrouchingHeight => 54;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override float Scale => 97f / 84f;
        public override int Age => 21;
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 135; //960
        public override int HealthPerLifeCrystal => 15;
        public override int HealthPerLifeFruit => 30;
        public override float AccuracyPercent => 0.91f;
        public override float Gravity => 0.45f;
        public override float MaxRunSpeed => 5.8f;
        public override float RunAcceleration => 0.15f;
        public override float RunDeceleration => 0.41f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 6.36f;
        public override bool CanCrouch => true;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override PartDrawOrdering MountedDrawOrdering => PartDrawOrdering.InBetween;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 5, MountUnlock = 7 };
        protected override CompanionDialogueContainer GetDialogueContainer => new MalishaDialogues();
        public override Companion GetCompanionObject => new MalishaCompanion();
        public override bool DrawBehindWhenSharingBed => true;

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.AmberStaff, 1),
                new InitialItemDefinition(ItemID.HealingPotion, 5),
                new InitialItemDefinition(ItemID.ManaPotion, 15),
                new InitialItemDefinition(ItemID.FlintlockPistol),
                new InitialItemDefinition(ItemID.MusketBall, 250),
            };
        }

        public override void UpdateAttributes(Companion companion)
        {
            companion.maxMinions++;
        }

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 1; i < 9; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(-1); //Malisha doesn't use her arm to carry someone.
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(14, 1);
                anim.AddFrame(15, 1);
                anim.AddFrame(16, 1);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i < 14; i++)
                {
                    anim.AddFrame(i, 1);
                }
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(22);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 23; i < 26; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(18);
        protected override Animation SetChairSittingFrames => new Animation(17);
        protected override Animation SetThroneSittingFrames => new Animation(26);
        protected override Animation SetBedSleepingFrames => new Animation(20);
        protected override Animation SetDownedFrames => new Animation(21);
        protected override Animation SetRevivingFrames => new Animation(19);
        protected override Animation SetBackwardStandingFrames => new Animation(27);
        protected override Animation SetBackwardReviveFrames => new Animation(28);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(17, 0);
                anim.AddFrameToReplace(18, 1);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                left.AddFrameToReplace(26, 0);
                right.AddFrameToReplace(26, 0);
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
                anim.AddFramePoint2X(17, 21, 36);
                anim.AddFramePoint2X(18, 21, 36);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0, true);
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 12, 3);
                left.AddFramePoint2X(11, 31, 12);
                left.AddFramePoint2X(12, 34, 19);
                left.AddFramePoint2X(13, 30, 28);
                
                left.AddFramePoint2X(14, 5, 7);
                left.AddFramePoint2X(15, 31, 6);
                left.AddFramePoint2X(16, 41, 40);
                
                left.AddFramePoint2X(19, 37, 43);
                
                left.AddFramePoint2X(23, 32, 14);
                left.AddFramePoint2X(24, 42, 24);
                left.AddFramePoint2X(25, 36, 38);

                right.AddFramePoint2X(10, 16, 3);
                right.AddFramePoint2X(11, 34, 12);
                right.AddFramePoint2X(12, 37, 19);
                right.AddFramePoint2X(13, 33, 28);
                
                right.AddFramePoint2X(14, 7, 7);
                right.AddFramePoint2X(15, 33, 6);
                right.AddFramePoint2X(16, 43, 40);
                
                right.AddFramePoint2X(23, 35, 14);
                right.AddFramePoint2X(24, 45, 24);
                right.AddFramePoint2X(25, 39, 38);

                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(16, 31, true);
                anim.AddFramePoint2X(1, 17, 31);
                anim.AddFramePoint2X(2, 18, 30);
                anim.AddFramePoint2X(3, 17, 31);
                anim.AddFramePoint2X(5, 15, 31);
                anim.AddFramePoint2X(6, 14, 30);
                anim.AddFramePoint2X(7, 15, 31);
                
                anim.AddFramePoint2X(14, 20, 31);
                anim.AddFramePoint2X(15, 22, 31);
                anim.AddFramePoint2X(16, 25, 30);
                
                anim.AddFramePoint2X(19, 11, 26);
                
                anim.AddFramePoint2X(22, 14, 38);
                anim.AddFramePoint2X(23, 14, 38);
                anim.AddFramePoint2X(24, 14, 38);
                anim.AddFramePoint2X(25, 14, 38);
                
                anim.AddFramePoint2X(26, 14, 32);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(25, 12), true);
                anim.AddFramePoint2X(16, 37, 23);
                anim.AddFramePoint2X(19, 34, 26);
                anim.AddFramePoint2X(22, 35, 24);
                anim.AddFramePoint2X(23, 34, 27);
                anim.AddFramePoint2X(24, 34, 27);
                anim.AddFramePoint2X(25, 34, 27);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetWingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(14, -1000, -1000);
                anim.AddFramePoint(15, -1000, -1000);
                anim.AddFramePoint(16, -1000, -1000);
                
                anim.AddFramePoint(19, 28, 33);
                anim.AddFramePoint(23, 28, 33);
                anim.AddFramePoint(24, 28, 33);
                anim.AddFramePoint(25, 28, 33);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(17, 7, -4);
                anim.AddFramePoint2X(18, 7, -4);
                
                anim.AddFramePoint2X(26, -8, -16);
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
        #endregion

        #region Spriting Layer
        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(TailTextureID, "tails");
        }
        #endregion
    }
}