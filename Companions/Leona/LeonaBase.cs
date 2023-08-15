using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions
{
    public class LeonaBase : TerraGuardianBase
    {
        public override string Name => "Leona";
        public override string Description => "";
        public override Sizes Size => Sizes.Large;
        public override int Width => 40;
        public override int Height => 100;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 108;
        public override float Scale => 122f / 108;
        public override int Age => 31;
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 225; //1350
        public override int HealthPerLifeCrystal => 35;
        public override int HealthPerLifeFruit => 30;
        public override float AccuracyPercent => .68f;
        public override float MaxRunSpeed => 5.15f;
        public override float RunDeceleration => .33f;
        public override int JumpHeight => 13;
        public override float JumpSpeed => 8.2f;
        public override bool CanCrouch => false; //Add crouching animation later
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.CobaltNaginata),
                new InitialItemDefinition(ItemID.HealingPotion, 5),
                new InitialItemDefinition(ItemID.CobaltRepeater),
                new InitialItemDefinition(ItemID.IchorArrow, 250)
            };
        }
        public override void UpdateAttributes(Companion companion)
        {
            companion.GetArmorPenetration<GenericDamageClass>() += 10;
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 6; i <= 13; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(14);
        protected override Animation SetPlayerMountedArmFrame => new Animation(14);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 2; i <= 5; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 15; i <= 18; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(19);
        protected override Animation SetChairSittingFrames => new Animation(19);
        protected override Animation SetThroneSittingFrames => new Animation(20);
        protected override Animation SetBedSleepingFrames => new Animation(21);
        protected override Animation SetDownedFrames => new Animation(23);
        protected override Animation SetRevivingFrames => new Animation(24);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(19, 0);
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
                anim.AddFramePoint2X(19, 18, 40);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),
                    right = new AnimationPositionCollection();
                left.AddFramePoint2X(15, 12, 5);
                left.AddFramePoint2X(16, 30, 12);
                left.AddFramePoint2X(17, 34, 22);
                left.AddFramePoint2X(18, 31, 30);
                left.AddFramePoint2X(24, 23, 42);
                
                right.AddFramePoint2X(2, 34, 9);
                right.AddFramePoint2X(3, 37, 17);
                right.AddFramePoint2X(4, 42, 40);
                right.AddFramePoint2X(5, 35, 49);

                right.AddFramePoint2X(15, 23, 5);
                right.AddFramePoint2X(16, 33, 12);
                right.AddFramePoint2X(17, 37, 22);
                right.AddFramePoint2X(18, 34, 30);

                return new AnimationPositionCollection[]{ left, right };
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(17, 17, true);
                anim.AddFramePoint2X(3, 21, 22);
                anim.AddFramePoint2X(4, 29, 31);
                anim.AddFramePoint2X(5, 23, 38);
                
                anim.AddFramePoint2X(20, 16, 22);
                anim.AddFramePoint2X(24, 19, 25);
                return anim;
            }
        }
        #endregion
    }
}