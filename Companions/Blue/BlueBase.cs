using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class BlueBase : TerraGuardianBase
    {
        public override string Name => "Blue";
        public override string FullName => "Freya Lightpelt";
        public override string Description => "It may not look like it, but she really cares about her look.\nShe constantly does her hair and paints her nails.";
        public override int Age => 17;
        public override Sizes Size => Sizes.Large;
        public override Genders Gender => Genders.Female;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 27);
        public override CompanionID? IsPartnerOf => new CompanionID(CompanionDB.Zack);
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override bool CanCrouch => true;
        public override int Width => 26;
        public override int Height => 82;
        public override float Scale => 99f / 82;
        public override int FavoriteFood => ItemID.Hotdog;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 175; //1150
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 15;
        public override float MaxRunSpeed => 4.75f;
        public override float RunAcceleration => 0.13f;
        public override float RunDeceleration => 0.5f;
        public override int JumpHeight => 19;
        public override float JumpSpeed => 7.52f;
        public override float AccuracyPercent => 0.46f;
        public override SoundStyle HurtSound => SoundID.NPCHit6;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MoveInUnlock = 0, VisitUnlock = 0 };
        public override BehaviorBase PreRecruitmentBehavior => new Blue.BlueRecruitmentBehavior();
        protected override CompanionDialogueContainer GetDialogueContainer => new BlueDialogues();
        public override CompanionData CreateCompanionData => new BlueData();
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.IronBroadsword),
                new InitialItemDefinition(ItemID.IronBow),
                new InitialItemDefinition(ItemID.WoodenArrow, 250),
                new InitialItemDefinition(ItemID.HealingPotion, 10)
            };
        }
        #region  Animations
        protected override Animation SetIdleFrames => new Animation(38);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24); //8
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames 
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 16; i <= 19; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 10; i <= 12; i++) anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(20);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(21, 1);
                anim.AddFrame(22, 1);
                anim.AddFrame(23, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(24);
        protected override Animation SetChairSittingFrames => new Animation(26);
        protected override Animation SetPlayerMountedArmFrame => new Animation(25);
        protected override Animation SetThroneSittingFrames => new Animation(27);
        protected override Animation SetBedSleepingFrames => new Animation(28);
        protected override Animation SetRevivingFrames => new Animation(33);
        protected override Animation SetDownedFrames => new Animation(32);
        protected override Animation SetPetrifiedFrames => new Animation(34);
        protected override Animation SetBackwardStandingFrames => new Animation(35);
        protected override Animation SetBackwardReviveFrames => new Animation(37);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer f = new AnimationFrameReplacer();
                f.AddFrameToReplace(24, 0);
                f.AddFrameToReplace(26, 1);
                f.AddFrameToReplace(31, 2);
                return f;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                left.AddFrameToReplace(27, 0);

                right.AddFrameToReplace(27, 0);
                right.AddFrameToReplace(29, 1);
                //right.AddFrameToReplace(39, 1);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    new AnimationPositionCollection(new Vector2(18, 31), true), 
                    new AnimationPositionCollection(new Vector2(30, 31), true)
                };
                //Left Arm
                Hands[0].AddFramePoint2X(10, 6, 14);
                Hands[0].AddFramePoint2X(11, 40, 9);
                Hands[0].AddFramePoint2X(12, 43, 41);
                
                Hands[0].AddFramePoint2X(16, 12, 5);
                Hands[0].AddFramePoint2X(17, 30, 7);
                Hands[0].AddFramePoint2X(18, 37, 19);
                Hands[0].AddFramePoint2X(19, 31, 32);
                
                Hands[0].AddFramePoint2X(21, 43, 22);
                Hands[0].AddFramePoint2X(22, 43, 31);
                Hands[0].AddFramePoint2X(23, 40, 42);
                
                Hands[0].AddFramePoint2X(33, 43, 43);
                
                //Right Arm
                Hands[1].AddFramePoint2X(10, 9, 14);
                Hands[1].AddFramePoint2X(11, 42, 9);
                Hands[1].AddFramePoint2X(12, 45, 41);
                
                Hands[1].AddFramePoint2X(16, 15, 5);
                Hands[1].AddFramePoint2X(17, 34, 7);
                Hands[1].AddFramePoint2X(18, 39, 19);
                Hands[1].AddFramePoint2X(19, 33, 32);
                
                Hands[1].AddFramePoint2X(21, 42, 22);
                Hands[1].AddFramePoint2X(22, 45, 31);
                Hands[1].AddFramePoint2X(23, 43, 42);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition 
        {
            get
            {
                AnimationPositionCollection pos = new AnimationPositionCollection(new Vector2(16, 16), true);
                pos.AddFramePoint2X(11, 29, 22);
                pos.AddFramePoint2X(12, 33, 29);

                for (short i = 20; i <= 22; i++) pos.AddFramePoint2X(i, 30, 31);
                return pos;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(21, 37), true);
                anim.AddFramePoint(27, 0, 0);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(new Vector2(16, 0));
        protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(new Vector2(-3, -2));
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(6, -4), true);
                a.AddFramePoint2X(27, -8, -15);
                return a;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(21+1, 12- 2, true);
                anim.AddFramePoint2X(11, 33 - 1, 17);
                anim.AddFramePoint2X(12, 38 - 1, 24);
                anim.AddFramePoint2X(20, 38 - 1, 24);
                anim.AddFramePoint2X(21, 38 - 1, 24);
                anim.AddFramePoint2X(22, 38 - 1, 24);
                anim.AddFramePoint2X(23, 38 - 1, 24);
                anim.AddFramePoint2X(33, 36 + 1, 27 - 2);

                anim.AddFramePoint2X(30, -1000, -1000);
                anim.AddFramePoint2X(31, -1000, -1000);
                return anim;
            }
        }
        #endregion

        const string bunnytexturekey = "bunnytex";

        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(bunnytexturekey, "bunny");
        }

        public override void ModifyAnimation(Companion companion)
        {
            //How to save that she has bunny?
            bool HasBunny = !companion.Crouching && companion.BodyFrameID != 33 && companion.BodyFrameID != 34 && companion.BodyFrameID != 37 && (companion.Data as BlueData).HasBunny;
            if(HasBunny)
            {
                ApplyHeldBunnyAnimation((TerraGuardian)companion);
            }
        }

        public static void ApplyHeldBunnyAnimation(TerraGuardian Blue, bool HoldingLeopold = false)
        {
            short FrameID;
            bool UpdateBodyAnimation = true;
            switch(Blue.BodyFrameID)
            {
                default:
                    FrameID = (short)(HoldingLeopold ? 39 : 29);
                    UpdateBodyAnimation = Blue.BodyFrameID == 0 || Blue.BodyFrameID == 38;
                    break;
                case 20:
                    FrameID = 23;
                    break;
                case 27:
                    FrameID = 31;
                    break;
                case 28:
                    FrameID = 30;
                    break;
                case 32:
                    return;
                case 35:
                    FrameID = 37;
                    break;
            }
            if (UpdateBodyAnimation)
            {
                Blue.BodyFrameID = FrameID;
            }
            for(int i = 0; i < Blue.ArmFramesID.Length; i++)
            {
                if (Blue.HeldItems[i].ItemAnimation <= 0 && (i != 0 || Blue.GetCharacterMountedOnMe == null))
                    Blue.ArmFramesID[i] = FrameID;
            }
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            TerraGuardian tg = Holder.GetCompanion as TerraGuardian;
            if (IsDrawingFrontLayer)
            {
                bool HasBunny = !tg.Crouching && tg.BodyFrameID != 33 && tg.BodyFrameID != 34 && tg.BodyFrameID != 37 && tg.BodyFrameID != 32 && (tg.Data as BlueData).HasBunny;
                if (HasBunny)
                {
                    CompanionSpritesContainer container = GetSpriteContainer;
                    Texture2D texture = container.GetExtraTexture(bunnytexturekey);
                    int DrawBackIndex = -1, DrawFrontIndex = -1;
                    int Frame = 0;
                    if (tg.ArmFramesID[0] == 31)
                        Frame = 2;
                    else if (tg.ArmFramesID[0] == 30)
                        Frame = 1;
                    else if (tg.ArmFramesID[0] == 36)
                        Frame = 3;
                    for(int i = 0; i < DrawDatas.Count; i++)
                    {
                        if (DrawDatas[i].texture == container.ArmFrontSpritesTexture[1])
                        {
                            DrawBackIndex = i;
                        }
                        if (DrawDatas[i].texture == container.ArmSpritesTexture[0])
                        {
                            if (DrawBackIndex == -1)
                                DrawBackIndex = i;
                            DrawFrontIndex = i;
                        }
                    }
                    Rectangle AnimFrame = tg.GetAnimationFrame(Frame);
                    if (DrawBackIndex > -1)
                    {
                        DrawData dd = new DrawData(texture, Holder.DrawPosition, AnimFrame, Holder.DrawColor, tg.fullRotation, Holder.Origin, tg.Scale, drawSet.playerEffect, 0);
                        DrawDatas.Insert(DrawBackIndex, dd);
                    }
                    if (DrawFrontIndex > -1)
                    {
                        AnimFrame.Y += AnimFrame.Height;
                        DrawData dd = new DrawData(texture, Holder.DrawPosition, AnimFrame, Holder.DrawColor, tg.fullRotation, Holder.Origin, tg.Scale, drawSet.playerEffect, 0);
                        DrawDatas.Insert(DrawFrontIndex + 1, dd);
                    }
                }
            }
        }

        public static bool HasBunnyInInventory(Companion companion)
        {
            for(int i = 0; i < 50; i++)
            {
                if(companion.inventory[i].type == 0) continue;
                switch(companion.inventory[i].type)
                {
                    case ItemID.Bunny:
                    case ItemID.GemBunnyAmber:
                    case ItemID.GemBunnyAmethyst:
                    case ItemID.GemBunnyDiamond:
                    case ItemID.GemBunnyEmerald:
                    case ItemID.GemBunnyRuby:
                    case ItemID.GemBunnySapphire:
                    case ItemID.GemBunnyTopaz:
                    case ItemID.GoldBunny:
                        return true;
                }
            }
            return false;
        }

        public override void UpdateBehavior(Companion companion)
        {
            if (companion.dead || companion.KnockoutStates > 0) return;
            (companion.Data as BlueData).UpdateBlueData(companion);
            if (companion.Owner == null && !companion.TargettingSomething && !companion.IsRunningBehavior && Main.rand.Next(15) == 0 && !(companion.Data as BlueData).HasBunny && (companion.Data as BlueData).CanPickupLeopold)
            {
                foreach(Companion c in MainMod.GetActiveCompanions)
                {
                    if (c.IsSameID(CompanionDB.Leopold))
                    {
                        if (c.Owner == null && !c.IsRunningBehavior && !c.dead && (c.Center - companion.Center).Length() < 200)
                        {
                            companion.RunBehavior(new Blue.CatchLeopoldBehavior((TerraGuardian)c));
                            (companion.Data as BlueData).SetLeopoldPickupCooldown();
                        }
                        return;
                    }
                }
            }
        }

        protected override void SetupSkinsOutfitsContainer(ref Dictionary<byte, CompanionSkinInfo> Skins, ref Dictionary<byte, CompanionSkinInfo> Outfits)
        {
            Outfits.Add(1, new Blue.RedHoodOutfit(true));
            Outfits.Add(2, new Blue.RedHoodOutfit(false));
        }
    }
}