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
{/* You need to add behavior base,and change animation code if you need to. i don't know how to handle them properly */
    public class CastellaBase : TerraGuardianBase
    {
        public const string HairBackTextureID = "hairback",HeadWerewolfTextureID = "headwere";
        public const byte MetamorphosisActionID = 0, HuntingActionID = 1;

        public override string Name => "Castella";
        public override string Description => "A mysterious woman, owner of a castle,\nafflicted by a curse.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 24;
        public override int Height => 88;
        public override int CrouchingHeight => 56;
        public override int SpriteWidth => 128;
        public override int SpriteHeight => 96;
        public override float Scale => 102f / 96f;
        public override int FramesInRow => 16;
        public override int Age => 36;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Summer, 28);
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 250; //1125
        public override int HealthPerLifeCrystal => 40;
        public override int HealthPerLifeFruit => 15;
        public override float AccuracyPercent => 0.72f;
        public override float MaxFallSpeed => 0.5f;
        public override float MaxRunSpeed => 5.3f;
        public override float RunAcceleration => 0.21f;
        public override float RunDeceleration => 0.47f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 6.81f;
        public override bool CanCrouch => true;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override PartDrawOrdering MountedDrawOrdering => PartDrawOrdering.InBetween;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MoveInUnlock = 3 };
        protected override CompanionDialogueContainer GetDialogueContainer => new Castella.CastellaDialogues();
        public override Companion GetCompanionObject => new Castella.CastellaCompanion();
        public override bool DrawBehindWhenSharingBed => true;

        /*public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.WoodenSword),
                new InitialItemDefinition(ItemID.Mushroom, 3)
            };
        }*/

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        /*protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(13, 1);
                anim.AddFrame(14, 1);
                anim.AddFrame(19, 1);
                return anim;
            }
        }*/
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i <= 13; i++)
                {
                    anim.AddFrame(i, 1);
                }
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(23);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 24; i <= 26; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(18);
        protected override Animation SetChairSittingFrames => new Animation(17);
        protected override Animation SetThroneSittingFrames => new Animation(20);
        protected override Animation SetBedSleepingFrames => new Animation(19);
        protected override Animation SetDownedFrames => new Animation(22);
        protected override Animation SetRevivingFrames => new Animation(21);
        //protected override Animation SetBackwardStandingFrames => new Animation(29);
        //protected override Animation SetBackwardReviveFrames => new Animation(31);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(17, 0);
                anim.AddFrameToReplace(18, 0);
                
                anim.AddFrameToReplace(43, 1);
                anim.AddFrameToReplace(44, 1);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(17, 0);
                right.AddFrameToReplace(43, 1);
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
                anim.AddFramePoint2X(17, 32, 36);
                anim.AddFramePoint2X(18, 32, 36);
                
                anim.AddFramePoint2X(43, 32, 36);
                anim.AddFramePoint2X(44, 32, 36);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions //Will need review on some positions.
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 21, 4);
                left.AddFramePoint2X(11, 41, 11);
                left.AddFramePoint2X(12, 46, 23);
                left.AddFramePoint2X(13, 40, 31);

                left.AddFramePoint2X(14, 14, 9);
                left.AddFramePoint2X(15, 46, 7);
                left.AddFramePoint2X(16, 58, 31);
                
                left.AddFramePoint2X(18, 42, 26);
                
                left.AddFramePoint2X(21, 40, 37);
                
                left.AddFramePoint2X(24, 32, 21);
                left.AddFramePoint2X(25, 50, 27);
                left.AddFramePoint2X(26, 45, 41);
                
                left.AddFramePoint2X(39, 32, 4);
                left.AddFramePoint2X(40, 39, 12);
                left.AddFramePoint2X(41, 43, 20);
                left.AddFramePoint2X(42, 38, 28);
                
                left.AddFramePoint2X(43, 38, 27);
                left.AddFramePoint2X(44, 28, 27);
                
                left.AddFramePoint2X(48, 35, 26);
                
                left.AddFramePoint2X(50, 45, 17);
                left.AddFramePoint2X(51, 52, 35);
                left.AddFramePoint2X(52, 42, 41);
                
                left.AddFramePoint2X(62, 39, 12);

                right.AddFramePoint2X(10, 16, 9);
                right.AddFramePoint2X(11, 44, 11);
                right.AddFramePoint2X(12, 48, 23);
                right.AddFramePoint2X(13, 42, 31);
                
                right.AddFramePoint2X(14, 16, 9);
                right.AddFramePoint2X(15, 48, 7);
                right.AddFramePoint2X(16, 59, 32);
                
                right.AddFramePoint2X(18, 45, 27);

                right.AddFramePoint2X(21, 44, 37);
                
                right.AddFramePoint2X(24, 34, 21);
                right.AddFramePoint2X(25, 52, 27);
                right.AddFramePoint2X(26, 47, 42);
                
                right.AddFramePoint2X(39, 37, 4);
                right.AddFramePoint2X(40, 42, 12);
                right.AddFramePoint2X(41, 47, 20);
                right.AddFramePoint2X(42, 42, 28);

                right.AddFramePoint2X(44, 44, 27);

                right.AddFramePoint2X(48, 49, 36);
                
                right.AddFramePoint2X(50, 49, 17);
                right.AddFramePoint2X(51, 55, 36);
                right.AddFramePoint2X(52, 48, 41);
                
                right.AddFramePoint2X(62, 42, 12);

                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(26, 14), true);
                anim.AddFramePoint2X(23, 32, 25);
                anim.AddFramePoint2X(24, 32, 25);
                anim.AddFramePoint2X(25, 32, 25);
                anim.AddFramePoint2X(26, 32, 25);
                
                anim.AddFramePoint2X(49, 32, 25);
                anim.AddFramePoint2X(50, 32, 25);
                anim.AddFramePoint2X(51, 32, 25);
                anim.AddFramePoint2X(52, 32, 25);
                
                anim.AddFramePoint2X(55, 31, 26);
                anim.AddFramePoint2X(56, 38, 31);
                anim.AddFramePoint2X(57, 38, 31);
                anim.AddFramePoint2X(58, 38, 31);
                anim.AddFramePoint2X(59, 30, 26);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(32, 11), true);
                anim.AddFramePoint2X(14, 28, 13);
                anim.AddFramePoint2X(15, 35, 11);
                anim.AddFramePoint2X(16, 50, 19);

                anim.AddFramePoint2X(21, 38, 24);

                anim.AddFramePoint2X(23, 41, 28);
                anim.AddFramePoint2X(24, 41, 28);
                anim.AddFramePoint2X(25, 41, 28);
                anim.AddFramePoint2X(26, 41, 28);
                
                anim.AddFramePoint2X(48, 35, 20);
                
                anim.AddFramePoint2X(49, 41, 27);
                anim.AddFramePoint2X(50, 41, 27);
                anim.AddFramePoint2X(51, 41, 27);
                anim.AddFramePoint2X(52, 41, 27);
                
                anim.AddFramePoint2X(55, 37, 22);
                anim.AddFramePoint2X(56, 44, 40);
                anim.AddFramePoint2X(57, 44, 40);
                anim.AddFramePoint2X(58, 45, 40);
                anim.AddFramePoint2X(59, 37, 22);
                
                anim.AddFramePoint2X(63, 42, 33);
                anim.AddFramePoint2X(64, 42, 33);
                anim.AddFramePoint2X(65, 43, 34);
                anim.AddFramePoint2X(66, 43, 35);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                const float XBonus = 6, YBonus = -5;
                anim.AddFramePoint2X(17, XBonus, YBonus);
                anim.AddFramePoint2X(18, XBonus, YBonus);
                anim.AddFramePoint2X(20, -10, -18);
                
                anim.AddFramePoint2X(43, XBonus, YBonus);
                anim.AddFramePoint2X(44, XBonus, YBonus);
                anim.AddFramePoint2X(45, -12, -18);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset 
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16, false);
        #endregion
        #region Sprite Related
        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(HairBackTextureID, "hair_back");
            container.AddExtraTexture(HeadWerewolfTextureID, "head_were");
        }
        #endregion
    }
}