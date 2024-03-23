using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class LiebreBase : TerraGuardianBase
    {
        public const string SkeletonBodyID = "skeletonbody", SkeletonLeftArmID = "skeletonlarm", SkeletonRightArmID = "skeletonrarm", 
            MouthID = "mouth", MouthLitID = "mouthlit", ScytheID = "scythe", HeadPlasmaID = "head_plasma";
        public const int MaxSoulsContainedValue = 10000;

        public override string Name => "Liebre";
        public override string Description => "Tasked with collecting souls from the\nTerra Realm and deliver to their destination.\nFeared by many, but he only want to have friends.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 24;
        public override int Height => 66;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int SpriteWidth => 64;
        public override int SpriteHeight => 80;
        public override float Scale => 60f / 66;
        public override int FramesInRow => 20;
        public override int Age => 88;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 28);
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 125; //1100
        public override int HealthPerLifeCrystal => 25;
        public override int HealthPerLifeFruit => 30;
        public override float AccuracyPercent => .72f;
        public override float MaxFallSpeed => .55f;
        public override float MaxRunSpeed => 4.9f;
        public override float RunAcceleration => .14f;
        public override float RunDeceleration => .42f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.16f;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override bool SleepsWhenOnBed => false;

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[] { new InitialItemDefinition(ItemID.SilverBroadsword), new InitialItemDefinition(ItemID.HealingPotion, 5) };
        }

        #region Animation
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 2; i <= 9; i++)
                    anim.AddFrame(i, 24);
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
                anim.AddFrame(11);
                anim.AddFrame(12);
                anim.AddFrame(14);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(15);
        protected override Animation SetChairSittingFrames => new Animation(15);
        protected override Animation SetThroneSittingFrames => new Animation(16);
        protected override Animation SetBedSleepingFrames => new Animation(17);
        protected override Animation SetDownedFrames => new Animation(19);
        protected override Animation SetRevivingFrames => new Animation(18);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(15, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(15, 13,32);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition => new AnimationPositionCollection(14, 16, true);
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),
                    right = new AnimationPositionCollection(27, 18, true);
                left.AddFramePoint2X(11, 17, 6);
                left.AddFramePoint2X(12, 23, 11);
                left.AddFramePoint2X(13, 25, 19);
                left.AddFramePoint2X(14, 23, 24);

                left.AddFramePoint2X(15, 20, 26);

                left.AddFramePoint2X(18, 21, 29);

                right.AddFramePoint2X(3, 26, 18);
                right.AddFramePoint2X(4, 26, 17);
                right.AddFramePoint2X(5, 26, 17);
                right.AddFramePoint2X(6, 26, 18);
                right.AddFramePoint2X(8, 27, 17);
                right.AddFramePoint2X(9, 27, 17);
                right.AddFramePoint2X(10, 27, 17);
                
                right.AddFramePoint2X(11, 21, 6);
                right.AddFramePoint2X(12, 25, 11);
                right.AddFramePoint2X(13, 27, 19);
                right.AddFramePoint2X(14, 25, 24);
                
                right.AddFramePoint2X(15, 27, 18);
                right.AddFramePoint2X(16, 20, 25);
                right.AddFramePoint2X(17, 15, 25);
                right.AddFramePoint2X(18, 28, 22);
                return new AnimationPositionCollection[] { left, right };
            }
        }

        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(17, 14, true);
                anim.AddFramePoint2X(18, 18, 17);
                anim.AddFramePoint2X(21, 18, 17);
                return anim;
            }
        }
        
        
        #endregion
    }
}