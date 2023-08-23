using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class LeonaBase : TerraGuardianBase
    {
        public const string giantswordtextureid = "giantsword";

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
        public override float RunAcceleration => .22f;
        public override float RunDeceleration => .33f;
        public override int JumpHeight => 13;
        public override float JumpSpeed => 8.2f;
        public override bool CanCrouch => true; //Add crouching animation later
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        //public override bool DrawBehindWhenSharingBed => true;
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
        public override CompanionSpritesContainer SetSpritesContainer(CompanionBase cb, Mod mod)
        {
            CompanionSpritesContainer container = base.SetSpritesContainer(cb, mod);
            container.AddExtraTexture(giantswordtextureid, "giantsword");
            return container;
        }
        public override void UpdateAttributes(Companion companion)
        {
            companion.GetArmorPenetration<GenericDamageClass>() += 10;
        }
        public override Companion GetCompanionObject => new Leona.LeonaCompanion();
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
        protected override Animation SetCrouchingFrames => new Animation(27);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 28; i <= 30; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(19, 0);
                anim.AddFrameToReplace(22, 1);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer right = new AnimationFrameReplacer(), left = new AnimationFrameReplacer();
                left.AddFrameToReplace(20, 0);

                right.AddFrameToReplace(20, 0);
                right.AddFrameToReplace(31, 1);
                return new AnimationFrameReplacer[]{ left, right };
            }
        }
        #endregion
        #region Animation Position
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(19, 20, 39);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0, true);
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
                
                left.AddFramePoint2X(28, 19, 13);
                left.AddFramePoint2X(29, 35, 21);
                left.AddFramePoint2X(30, 29, 39);
                
                right.AddFramePoint2X(2, 34, 12);
                right.AddFramePoint2X(3, 37, 18);
                right.AddFramePoint2X(4, 42, 40);
                right.AddFramePoint2X(5, 35, 49);

                right.AddFramePoint2X(15, 23, 5);
                right.AddFramePoint2X(16, 33, 12);
                right.AddFramePoint2X(17, 37, 22);
                right.AddFramePoint2X(18, 34, 30);
                
                right.AddFramePoint2X(20, 36, 16);
                
                right.AddFramePoint2X(24, 36, 20);
                
                right.AddFramePoint2X(27, 36, 20);
                right.AddFramePoint2X(28, 22, 13);
                right.AddFramePoint2X(29, 38, 21);
                right.AddFramePoint2X(30, 32, 39);

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
                
                anim.AddFramePoint2X(27, 19, 25);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(19, 8, -4);
                anim.AddFramePoint2X(20, -10, -16);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(0, 3, true);
        #endregion
        /*#region Animation Scripts
        public override void ModifyAnimation(Companion companion)
        {
            if (companion.sleeping.isSleeping && companion.Owner != null)
            {
                Player p = companion.Owner;
                if (p.sleeping.isSleeping && p.Bottom == companion.Bottom)
                {
                    companion.BodyFrameID = 
                    companion.ArmFramesID[0] = 
                    companion.ArmFramesID[1] = 22;
                }
            }
            else if (companion.ArmFramesID[1] < 20 && (companion.ArmFramesID[1] < 15 || companion.ArmFramesID[1] >= 19))
            {
                companion.ArmFramesID[1] = 2;
            }
        }
        #endregion
        #region Drawing
        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if(!IsDrawingFrontLayer)
            {
                Vector2? SwordPosition = null;
                TerraGuardian tg = Holder.tg;
                switch(tg.ArmFramesID[1])
                {
                    case 1:
                    case 2:
                    case 3:
                    case 4:
                    case 5:
                    case 20:
                    case 27:
                        SwordPosition = tg.GetAnimationPosition(AnimationPositions.HandPosition, tg.ArmFramesID[1], 1) - Main.screenPosition + Vector2.UnitY * tg.gfxOffY;
                        break;
                }
                if (SwordPosition.HasValue)
                {
                    if (tg.IsUsingAnyChair)
                    {
                        SwordPosition = SwordPosition.Value + tg.sitting.offsetForSeat;
                    }
                    Vector2 Origin = new Vector2(drawSet.playerEffect == Microsoft.Xna.Framework.Graphics.SpriteEffects.None ? 69 : 11, 10);
                    DrawData dd = new DrawData(tg.Base.GetSpriteContainer.GetExtraTexture(giantswordtextureid), SwordPosition.Value, null, Holder.DrawColor, 0, Origin, tg.Scale, drawSet.playerEffect, 0);
                    DrawDatas.Insert(0, dd);
                }
            }
        }
        #endregion*/
    }
}