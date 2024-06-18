using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.DataStructures;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using terraguardians.Companions.CaptainStench;
using System;
using terraguardians.Companions.CaptainStench.Subattacks;


namespace terraguardians.Companions
{
    public class CaptainStenchBase : TerraGuardianBase
    {
        const string HolsteredPistolID = "hpistol", PistolID = "pistol", ScouterID = "scouter", 
            Yegg1ID = "yegg1", Yegg2ID = "yegg2";
        public override string Name => "Captain Stench";
        public override string CompanionContentFolderName => "CaptainStench";
        public override string[] PossibleNames => new string[]{"Captain Sally", "Captain Sara"};
        public override string Description => "A now rogue pirate that lost her whole crew to a fatal collision with a meteorite when scouting the world of terraria.\nShe no longer has any way off the planet so she spends the rest of her days adventuring."/*"A Space pilot once renown for pillaging through out the galaxy, now is left stranded\n" +
                "on a unknown planet after survivng a collision with a meteorite which killed her whole crew."*/;
        public override Sizes Size => Sizes.Medium;
        public override int Width => 22;
        public override int Height => 66;
        //Need Y position discount
        public override int SpriteWidth => 120;
        public override int SpriteHeight => 100;
        public override float Scale => 2f;
        public override int FramesInRow => 17;
        public override int Age => 32;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Winter, 6);
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 250; //1125
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 10;
        public override int InitialMaxMana => 100;
        public override int ManaPerManaCrystal => 50;
        public override float AccuracyPercent => .72f;
        public override float MaxRunSpeed => 4.9f;
        public override float RunAcceleration => .14f;
        public override float RunDeceleration => 0.42f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 7.16f;
        public override bool CanCrouch => true;
        public override string ContributorName => "Smokey";
        protected override SubAttackBase[] GetDefaultSubAttacks()
        {
            return [
                new StenchSaberSheathingAction(),
                new StenchSaberSubAttack(),
                new ReflectorSubAttack(),
                new StenchTripleSlashSubAttack()
            ];
        }
        public override Rectangle GetHeadDrawFrame(Texture2D HeadTexture, Companion companion)
        {
            return new Rectangle(0, companion.direction == -1 ? 14 : 0, 17, 14);
        }
        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(PistolID, "LaserPistol");
            container.AddExtraTexture(ScouterID, "scouter");
            container.AddExtraTexture(HolsteredPistolID, "holsteredpistol");
            container.AddExtraTexture(Yegg1ID, "yegg_0");
            container.AddExtraTexture(Yegg2ID, "yegg_1");
        }
        public override Companion GetCompanionObject => new StenchCompanion();
        public override CompanionData CreateCompanionData => new StenchData();

        protected override CompanionDialogueContainer GetDialogueContainer => new CaptainStenchDialogue();

        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MountUnlock = 255, ControlUnlock = 255 };

        #region Animations
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
        protected override Animation SetJumpingFrames => new Animation(18);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 24; i <= 27; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetDownedFrames => new Animation(28);
        protected override Animation SetRevivingFrames => new Animation(29);
        protected override Animation SetSittingFrames => new Animation(78);
        protected override Animation SetThroneSittingFrames => new Animation(79);
        protected override Animation SetBedSleepingFrames => new Animation(80);
        #endregion

        #region Animation Positions
        protected override AnimationPositionCollection SetBodyOffsetPosition => new AnimationPositionCollection(0, 9);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(78, 47, 74);
                anim.AddFramePoint(79, 120, 100);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint(80, -6, 32);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection hand = new AnimationPositionCollection();
                hand.AddFramePoint(24, 53, 38);
                hand.AddFramePoint(25, 70, 46);
                hand.AddFramePoint(26, 75, 58);
                hand.AddFramePoint(27, 70, 70);
                
                hand.AddFramePoint(29, 54, 74);
                return [ hand, new AnimationPositionCollection() ];
            }
        }
        #endregion

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (!IsDrawingFrontLayer)
            {
                TerraGuardian tg = Holder.GetCompanion as TerraGuardian;
                DrawData dd;
                if (tg.direction == 1)
                {
                    Texture2D ScouterTexture = GetSpriteContainer.GetExtraTexture(ScouterID);
                    dd = new DrawData(ScouterTexture, Holder.DrawPosition + tg.BodyOffset, Holder.BodyFrame, Holder.DrawColor, tg.fullRotation, tg.fullRotationOrigin, tg.Scale, drawSet.playerEffect, 0);
                    DrawDatas.Add(dd);
                }
                if (Holder.GetCompanion.itemAnimation <= 0)
                {
                    Texture2D holsteredPistol = GetSpriteContainer.GetExtraTexture(HolsteredPistolID);
                    dd = new DrawData(holsteredPistol, Holder.DrawPosition + tg.BodyOffset, Holder.BodyFrame, Holder.DrawColor, tg.fullRotation, tg.fullRotationOrigin, tg.Scale, drawSet.playerEffect, 0);
                    DrawDatas.Add(dd);
                }
            }
        }

        public class StenchCompanion : TerraGuardian
        {
            public WeaponInfusions CurrentInfusion
            {
                get
                {
                    return (Data as StenchData).CurrentInfusion;
                }
                set
                {
                    (Data as StenchData).CurrentInfusion = value;
                }
            }
            public int YeggFrame = -1;
            public Rectangle YeggRect = new Rectangle();
            public bool HoldingWeapon = false;

            public bool HasPhantomDevice
            {
                get
                {
                    return (Data as StenchData).HasPhantomDevice;
                }
                set
                {
                    (Data as StenchData).HasPhantomDevice = value;
                }
            }

            public override void ModifyAnimation()
            {
                bool UpdateArm = false;
                if (velocity.Y != 0)
                {
                    if (MathF.Abs(velocity.Y) < 2)
                    {
                        BodyFrameID = 19;
                        UpdateArm = true;
                    }
                    else if (velocity.Y >= 2f)
                    {
                        BodyFrameID = 20;
                        UpdateArm = true;
                    }
                    if (HoldingWeapon)
                    {
                        BodyFrameID += 3;
                        UpdateArm = true;
                    }
                }
                else if (velocity.X != 0)
                {
                    if (HoldingWeapon)
                    {
                        BodyFrameID += 8;
                        UpdateArm = true;
                    }
                }
                else
                {
                    if (HoldingWeapon)
                    {
                        BodyFrameID++;
                        UpdateArm = true;
                    }
                }
                if (UpdateArm && itemAnimation <= 0)
                {
                    for (int i = 0; i < 2; i++)
                        ArmFramesID[i] = BodyFrameID;
                }
            }

            public override void PostUpdateAnimation()
            {
                switch (BodyFrameID)
                {
                    default:
                        YeggFrame = -1;
                        break;
                    case 1:
                        YeggFrame = 0;
                        break;
                    case 10: //1
                    case 11:
                    case 12:
                    case 13:
                    case 14:
                    case 15:
                    case 16:
                    case 17:
                        YeggFrame = BodyFrameID - 9;
                        break;
                    case 21: //9
                    case 22:
                    case 23:
                        YeggFrame = BodyFrameID - 12;
                        break;
                    case 38: //12
                    case 39: 
                    case 40: 
                    case 41: 
                    case 42: 
                    case 43: 
                    case 44: 
                    case 45: 
                    case 46: 
                    case 47: 
                    case 48: 
                    case 49: 
                    case 50: 
                    case 51: 
                    case 52: 
                    case 53: 
                    case 54: 
                    case 55: 
                    case 56: 
                    case 57: 
                    case 58: 
                    case 59: 
                    case 60: 
                    case 61: 
                    case 62: 
                    case 63: 
                    case 64: 
                    case 65: 
                    case 66: 
                    case 67: 
                        YeggFrame = BodyFrameID - 26;
                        break;
                    case 88: 
                    case 89: 
                    case 90: 
                        YeggFrame = BodyFrameID - 46;
                        break;
                }
                if (YeggFrame > -1)
                {
                    YeggRect = GetAnimationFrame(YeggFrame);
                    if (CurrentInfusion != WeaponInfusions.Diamond)
                    {
                        YeggRect.Y += YeggRect.Height * 3 * ((int)CurrentInfusion - 1);
                    }
                }
            }

            public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
            {
                DrawData dd;
                if (IsDrawingFrontLayer)
                {
                    if (YeggFrame > -1 && CurrentInfusion > WeaponInfusions.None)
                    {
                        Texture2D YeggTexture = Base.GetSpriteContainer.GetExtraTexture(CurrentInfusion >= WeaponInfusions.Diamond ? Yegg2ID : Yegg1ID);
                        dd = new DrawData(YeggTexture, Holder.DrawPosition + BodyOffset, YeggRect, CurrentInfusion > WeaponInfusions.None ? Color.White : Holder.DrawColor, fullRotation, fullRotationOrigin, Scale, drawSet.playerEffect, 0);
                        DrawDatas.Insert(1, dd);
                    }
                }
            }
        }

        class StenchData : CompanionData
        {
            public WeaponInfusions CurrentInfusion = WeaponInfusions.None;
            public bool HasPhantomDevice = false;
        }

        public enum WeaponInfusions : byte
        {
            None = 0,
            Amethyst = 1,
            Topaz = 2,
            Sapphire = 3,
            Ruby = 4,
            Emerald = 5,
            Amber = 6,
            Diamond = 7
        }
    }
}