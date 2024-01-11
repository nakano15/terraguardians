using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;
using terraguardians.Companions.Fluffles;

namespace terraguardians.Companions
{
    public class FlufflesBase : TerraGuardianBase
    {
        const string FoxSoulTextureID = "foxsoul";

        static readonly string[] Names = new string[]{ "Fluffles", "Krümel" }; //Still, Thank BentoFox for the names.
        public override string Name => "Fluffles";
        public override string[] PossibleNames => Names;
        public override string Description => "She was an experienced adventurer, part of a\ngroup of Terrarians and TerraGuardians.\nA traumatic experience unallows her to speak.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 26;
        public override int Height => 92;
        public override int FramesInRow => 23;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 104;
        public override float Scale => 101f / 92;
        public override int Age => 31;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 11);
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 235; //955
        public override int HealthPerLifeCrystal => 35;
        public override int HealthPerLifeFruit => 10;
        public override float AccuracyPercent => .77f;
        public override float MaxFallSpeed => .25f;
        public override float MaxRunSpeed => 4.85f;
        public override float RunAcceleration => .15f;
        public override float RunDeceleration => .26f;
        public override int JumpHeight => 17;
        public override float JumpSpeed => 6.18f;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.CompanionRidesPlayer;
        public override bool IsNocturnal => true;
        public override bool SitOnPlayerLapOnChair => false;
        public override Companion GetCompanionObject => new FlufflesCompanion();
        public override BehaviorBase PreRecruitmentBehavior => new FlufflesPreRecruitBehavior();
        protected override CompanionDialogueContainer GetDialogueContainer => new Fluffles.FlufflesDialogues();
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0, MountUnlock = 0 };
        public override bool CanSpawnNpc()
        {
            return NPC.downedHalloweenTree || (MainMod.GetLocalPlayer.statDefense >= 10 && (Main.halloween || NPC.downedBoss2));
        }
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.BoneSword),
                new InitialItemDefinition(ItemID.GoldBow),
                new InitialItemDefinition(ItemID.BoneArrow, 250),
                new InitialItemDefinition(ItemID.LesserHealingPotion, 5)
            };
        }
        public override void UpdateAttributes(Companion companion)
        {
            PlayerMod pm = companion.GetPlayerMod;
            pm.CanEnterKnockOutColdState = false;
            pm.CanBeKilled = false;
            pm.CanBeAttackedWhenKOd = false;
            pm.HasEmptyReviveBarOnKO = true;
            pm.CanBeHelpedToRevive = false;
            companion.gills = true;
            companion.fireWalk = true;
            companion.noFallDmg = true;
            companion.suffocateDelay = 0;
            companion.buffImmune[BuffID.Suffocation] = true;
            companion.buffImmune[BuffID.Horrified] = true;
            companion.buffImmune[BuffID.TheTongue] = true;
            companion.buffImmune[BuffID.MoonLeech] = true;
        }

        #region Animation
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(3, 48);
                anim.AddFrame(4, 48);
                anim.AddFrame(5, 48);
                anim.AddFrame(4, 48);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(2);
        protected override Animation SetPlayerMountedArmFrame => new Animation(10);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 6; i <= 9; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(10);
        protected override Animation SetChairSittingFrames => new Animation(11);
        protected override Animation SetThroneSittingFrames => new Animation(16);
        protected override Animation SetBedSleepingFrames => new Animation(17);
        protected override Animation SetRevivingFrames => new Animation(15);
        protected override Animation SetDownedFrames => new Animation(18);

        protected override Animation SetBackwardStandingFrames => new Animation(19);
        protected override Animation SetBackwardReviveFrames => new Animation(22);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(11, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0, true);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(11, 21, 36);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(11, 4, -4);
                anim.AddFramePoint2X(16, -14, -14);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(6, 15, 3);
                left.AddFramePoint2X(7, 31, 11);
                left.AddFramePoint2X(8, 35, 20);
                left.AddFramePoint2X(9, 31, 29);

                left.AddFramePoint2X(10, 25 + 2, 31);

                left.AddFramePoint2X(15, 39, 44);

                right.AddFramePoint2X(6, 27, 3);
                right.AddFramePoint2X(7, 34, 11);
                right.AddFramePoint2X(8, 38, 20);
                right.AddFramePoint2X(9, 34, 29);

                right.AddFramePoint2X(15, 42, 44);
                return new AnimationPositionCollection[]{ left, right };
            }
        }

        protected override AnimationPositionCollection SetMountShoulderPosition => new AnimationPositionCollection(25, 31, true);
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(22, 13, true);
                anim.AddFramePoint2X(15, 38, 39);
                anim.AddFramePoint2X(17, 10, 38);
                anim.AddFramePoint2X(18, 33, 33);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingCompanionOffset => new AnimationPositionCollection(0, -2f, true);
        #endregion

        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(FoxSoulTextureID, "FoxSoul");
        }

        public class FlufflesCompanion : TerraGuardian
        {
            float KnockoutAlpha = 1, SoulOpacity = 0f;
            bool SoulAttached = false, FirstFrame = true;
            Vector2 SoulPosition = Vector2.Zero, SoulVelocity = Vector2.Zero;
            byte SoulFrame = 0;
            public static float GetColorMod {
                get
                {
                    return MainMod.GetGhostColorMod;
                }
            }

            public override void UpdateAttributes()
            {
                
            }

            public static Color GhostfyColor(Color Original, float KoAlpha, float ColorMod)
            {
                Color color = Original;
                color.R -= (byte)(color.R * ColorMod);
                color.G += (byte)((255 - color.G) * ColorMod);
                color.B += (byte)((255 - color.B) * ColorMod);
                color *= KoAlpha;
                color *= 0.75f;
                //color.A -= (byte)(color.A * ColorMod);
                return color;
            }

            public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
            {
                float ColorMod = GetColorMod;
                Holder.DrawColor = GhostfyColor(Holder.DrawColor, KnockoutAlpha, ColorMod);
                if (KnockoutStates > 0)
                    Holder.HatColor = Color.Transparent;
            }

            public bool DrawSoulFire { get { return SoulOpacity > 0; } }

            public DrawData GetSoulFireDrawData()
            {
                return new DrawData(Base.GetSpriteContainer.GetExtraTexture(FoxSoulTextureID), SoulPosition - Main.screenPosition, new Rectangle(16 * (int)(SoulFrame * (1f / 8)), 0, 16, 20), Color.White * SoulOpacity * .75f, 0, new Vector2(8, 10), 1f, SpriteEffects.None);;
            }

            public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
            {
                if (IsDrawingFrontLayer && SoulOpacity > 0)
                {
                    if (Owner == null)
                    {
                        DrawDatas.Add(GetSoulFireDrawData());
                    }
                }
            }

            public override void UpdateCompanionHook()
            {
                if (KnockoutStates == KnockoutStates.Awake && !IsMountedOnSomething && !UsingFurniture && velocity.Y == 0 && GetGoverningBehavior() is not Fluffles.FriendlyHauntBehavior)
                {
                    gfxOffY = MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 2) * 6;
                }
                //return;
                UpdateKOBehavior();
                UpdateHauntAttemptBehavior();
                if (SoulOpacity > 0)
                {
                    Lighting.AddLight(SoulPosition, new Vector3(0.82f * .33f, 2.17f * .33f, 1.61f * .33f) * SoulOpacity);
                    SoulFrame++;
                    if (SoulFrame >= 8 * 6)
                        SoulFrame -= 8 * 6;
                    if (Main.rand.Next(3) == 0)
                        Dust.NewDust(SoulPosition - new Vector2(8, 10), 16, 20, 75, 0f, -.5f);
                }
            }

            void UpdateHauntAttemptBehavior()
            {
                if (KnockoutStates > KnockoutStates.Awake) return;
                if (Owner == null && !Dialogue.IsParticipatingDialogue(this) && !UsingFurniture && !IsRunningBehavior && 
                    !TargettingSomething && velocity.X == 0 && velocity.Y == 0 && Main.rand.Next(300) == 0)
                {
                    List<Player> PossibleTargets = new List<Player>();
                    const float RangeX = 100, RangeY = 60;
                    for (int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active && PlayerMod.IsPlayerCharacter(Main.player[p]) && !Main.player[p].dead && 
                            PlayerMod.GetPlayerKnockoutState(Main.player[p]) == KnockoutStates.Awake && !IsHostileTo(Main.player[p]) &&
                            MathF.Abs(Main.player[p].Center.X - Center.X) < RangeX + 20 + width * 0.5f && 
                            MathF.Abs(Main.player[p].Center.Y - Center.Y) < RangeY + 27 + height * 0.5f)
                        {
                            PossibleTargets.Add(Main.player[p]);
                        }
                    }
                    foreach (Companion c in MainMod.ActiveCompanions.Values)
                    {
                        if (!c.dead && c.KnockoutStates == KnockoutStates.Awake && !IsHostileTo(c) &&
                            MathF.Abs(c.Center.X - Center.X) < RangeX + 20 + width * 0.5f && 
                            MathF.Abs(c.Center.Y - Center.Y) < RangeY + 27 + height * 0.5f)
                        {
                            PossibleTargets.Add(c);
                        }
                    }
                    if (PossibleTargets.Count > 0)
                    {
                        RunBehavior(new FriendlyHauntBehavior(PossibleTargets[Main.rand.Next(PossibleTargets.Count)]));
                    }
                    PossibleTargets.Clear();
                }
            }

            void UpdateKOBehavior()
            {
                if (KnockoutStates > KnockoutStates.Awake)
                {
                    GetPlayerMod.ChangeReviveStack(1);
                    if (KnockoutAlpha > 0)
                    {
                        KnockoutAlpha -= 0.005f;
                        if (KnockoutAlpha < 0)
                        {
                            KnockoutAlpha = 0;
                        }
                    }
                    else
                    {
                        if (Owner != null)
                        {
                            Bottom = Owner.Bottom;
                            direction = Owner.direction;
                        }
                    }
                    //Soul script
                    if (SoulOpacity < 1f)
                    {
                        SoulOpacity += 1f / 60;
                        if (SoulOpacity > 1)
                            SoulOpacity = 1;
                    }
                    else if (Owner != null)
                    {
                        if (SoulAttached)
                        {
                            Vector2 EndPosition = Owner.Center;
                            EndPosition.X += 4 * Owner.direction;
                            EndPosition.Y += 2 * Owner.gravDir;
                            SoulPosition.X += (EndPosition.X - SoulPosition.X) * .8f;
                            SoulPosition.Y += (EndPosition.Y - SoulPosition.Y) * .8f;
                        }
                        else if (KnockoutAlpha <= 0)
                        {
                            float Distance = Owner.Center.X - SoulPosition.X;
                            const float MaxSoulMoveSpeed = 6f;
                            bool AtPointHorizontally = false, AtPointVertically = false;
                            float SoulAcceleration = 0.15f;
                            if (MathF.Abs(Distance) < 20)
                            {
                                SoulVelocity.X *= 0.3f;
                                AtPointHorizontally = true;
                            }
                            else
                            {
                                if (Distance < 0)
                                {
                                    SoulVelocity.X -= SoulAcceleration;
                                    if (SoulVelocity.X < -MaxSoulMoveSpeed)
                                        SoulVelocity.X = -MaxSoulMoveSpeed;
                                }
                                else
                                {
                                    SoulVelocity.X += SoulAcceleration;
                                    if (SoulVelocity.X > MaxSoulMoveSpeed)
                                        SoulVelocity.X = MaxSoulMoveSpeed;
                                }
                            }
                            Distance = Owner.Center.Y - SoulPosition.Y;
                            if (MathF.Abs(Distance) < 20)
                            {
                                SoulVelocity.Y *= 0.3f;
                                AtPointVertically = true;
                            }
                            else
                            {
                                if (Distance < 0)
                                {
                                    SoulVelocity.Y -= SoulAcceleration;
                                    if (SoulVelocity.Y < -MaxSoulMoveSpeed)
                                        SoulVelocity.Y = -MaxSoulMoveSpeed;
                                }
                                else
                                {
                                    SoulVelocity.Y += SoulAcceleration;
                                    if (SoulVelocity.Y > MaxSoulMoveSpeed)
                                        SoulVelocity.Y = MaxSoulMoveSpeed;
                                }
                            }
                            SoulPosition += SoulVelocity;
                            if (AtPointHorizontally && AtPointVertically)
                            {
                                SoulAttached = true;
                            }
                        }
                        else
                        {
                            SoulPosition = Center;
                            SoulPosition.Y += height * .25f;
                            SoulPosition.X += 8 * Scale;
                            SoulVelocity.X = 0;
                            SoulVelocity.Y = 0;
                        }
                    }
                }
                else
                {
                    const float OpacityChangePerFrame = 0.005f;
                    const float MaxOpacity = .8f;
                    Tile tile = Main.tile[(int)(Center.X * DivisionBy16), (int)(Center.Y * DivisionBy16)];
                    bool ReduceOpacity = Main.dayTime && !Main.eclipse && ZoneOverworldHeight && (tile.WallType == 0 || tile.HasTile && !Main.tileSolid[tile.TileType]);
                    if (ReduceOpacity)
                    {
                        float MinOpacity = 0.2f;
                        if (Math.Abs(Center.X - MainMod.GetLocalPlayer.Center.X) > 120 || 
                            Math.Abs(Center.Y - MainMod.GetLocalPlayer.Center.Y) > 120)
                        {
                            if (Owner == MainMod.GetLocalPlayer)
                            {
                                MinOpacity = .1f;
                            }
                            else
                            {
                                MinOpacity = 0;
                            }
                        }
                        if (FirstFrame)
                        {
                            KnockoutAlpha = MinOpacity;
                            FirstFrame = false;
                        }
                        else if (KnockoutAlpha > MinOpacity)
                        {
                            KnockoutAlpha -= OpacityChangePerFrame;
                            if (KnockoutAlpha < MinOpacity)
                            {
                                KnockoutAlpha = MinOpacity;
                            }
                        }
                        else if (KnockoutAlpha < MinOpacity)
                        {
                            KnockoutAlpha += OpacityChangePerFrame;
                            if (KnockoutAlpha > MinOpacity)
                            {
                                KnockoutAlpha = MinOpacity;
                            }
                        }
                    }
                    else if (KnockoutAlpha < MaxOpacity)
                    {
                        if (FirstFrame)
                        {
                            KnockoutAlpha = MaxOpacity;
                            FirstFrame = false;
                        }
                        else
                        {
                            KnockoutAlpha += OpacityChangePerFrame;
                            if (KnockoutAlpha > MaxOpacity)
                                KnockoutAlpha = MaxOpacity;
                        }
                    }
                    //Soul Script
                    if (SoulOpacity > 0f)
                    {
                        SoulAttached = false;
                        SoulOpacity -= 1f / 60;
                        if (SoulOpacity < 0)
                            SoulOpacity = 0;
                    }
                    SoulPosition = Center;
                    //SoulPosition.Y += height * .25f;
                    //SoulPosition.X += 8 * direction * Scale;
                    SoulVelocity.X = 0;
                    SoulVelocity.Y = 0;
                }
            }

            public override void ModifyAnimation()
            {
                if (BodyFrameID == 15 || BodyFrameID == 10 || BodyFrameID == 22)
                    return;
                short NewFrame = -1;
                bool WasJumpingFrame = false;
                switch (BodyFrameID)
                {
                    case 0: //Idle Forward
                        if (gfxOffY >= 2)
                            NewFrame = 2;
                        if (gfxOffY <= -2)
                        {
                            NewFrame = 1;
                        }
                        break;
                    case 19: //Idle Backward
                        if (gfxOffY >= 2)
                            NewFrame = 21;
                        if (gfxOffY <= -2)
                        {
                            NewFrame = 20;
                        }
                        break;
                    case 2: //Jumping
                        {
                            WasJumpingFrame = true;
                            if (velocity.Y != 0)
                            {
                                if (MathF.Abs(velocity.Y) < 1)
                                {
                                    NewFrame = 1;
                                }
                                else if (velocity.Y >= 1)
                                {
                                    NewFrame = 13;
                                }
                            }
                        }
                        break;
                }
                if (!WasJumpingFrame && ((velocity.X > 0 && direction < 0) || (velocity.X < 0 && direction > 0)))
                {
                    if (BodyFrameID >= 3 && BodyFrameID <= 5)
                    {
                        NewFrame = (short)(BodyFrameID + 9);
                    }
                }
                if (NewFrame > -1)
                {
                    if (ArmFramesID[0] == BodyFrameID)
                    {
                        ArmFramesID[0] = NewFrame;
                    }
                    if (ArmFramesID[1] == BodyFrameID)
                    {
                        ArmFramesID[1] = NewFrame;
                    }
                    BodyFrameID = NewFrame;
                }
            }
        }
    }
}