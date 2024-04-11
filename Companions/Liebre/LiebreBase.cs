using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class LiebreBase : TerraGuardianBase
    {
        internal static float BlessedSoulBuffPower = 1f;
        internal static int BlessedSoulBuffDuration = 0;
        internal static byte EncounterTimes = 0;

        internal static bool SoulSaved = false;
        internal static Vector2 PlayerSoulPosition = Vector2.Zero;
        public const string SkeletonBodyID = "skeletonbody", SkeletonLeftArmID = "skeletonlarm", SkeletonRightArmID = "skeletonrarm", 
            MouthID = "mouth", MouthLitID = "mouthlit", ScytheID = "scythe", HeadPlasmaID = "head_plasma", EyesID = "eyes";

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
        public override Companion GetCompanionObject => new LiebreCompanion();
        public override CompanionData CreateCompanionData => new LiebreData();
        protected override CompanionDialogueContainer GetDialogueContainer => new LiebreDialogues();
        public override BehaviorBase PreRecruitmentBehavior => new Liebre.LiebrePreRecruitBehavior();
        public override bool CanSpawnNpc()
        {
            return NPC.downedBoss3;
        }

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[] { 
                new InitialItemDefinition(ItemID.SilverBroadsword), 
                new InitialItemDefinition(ItemID.HealingPotion, 5) 
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
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 11; i <= 14; i++)
                    anim.AddFrame(i);
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

        public override void SetupSpritesContainer(CompanionSpritesContainer container)
        {
            container.AddExtraTexture(SkeletonBodyID, "skeleton_body");
            container.AddExtraTexture(SkeletonLeftArmID, "skeleton_left_arm");
            container.AddExtraTexture(SkeletonRightArmID, "skeleton_right_arm");
            container.AddExtraTexture(MouthID, "mouth");
            container.AddExtraTexture(MouthLitID, "mouth_lit");
            container.AddExtraTexture(ScytheID, "scythe");
            container.AddExtraTexture(HeadPlasmaID, "head_p");
            container.AddExtraTexture(EyesID, "Eyes");
        }

        internal static void Initialize()
        {
            SoulSaved = false;
            PlayerSoulPosition = Vector2.Zero;
            BlessedSoulBuffDuration = 0;
        }

        internal static void UpdateBlessedSoulBuff()
        {
            if (BlessedSoulBuffDuration > 0)
                BlessedSoulBuffDuration--;
        }

        internal static void CheckCanGetBlessedSoulBuff(Player player)
        {
            if (BlessedSoulBuffDuration >= 5)
            {
                player.AddBuff(ModContent.BuffType<Buffs.BlessedSoul>(), 5);
            }
        }

        internal static void SaveBuffInfos(TagCompound tag)
        {
            tag.Add("liebrebuffpower", BlessedSoulBuffPower);
            tag.Add("liebrebuffduration", BlessedSoulBuffDuration);
            tag.Add("liebremettimes", EncounterTimes);
        }

        internal static void LoadBuffInfos(TagCompound tag, uint Version)
        {
            BlessedSoulBuffPower = tag.GetFloat("liebrebuffpower");
            BlessedSoulBuffDuration = tag.GetInt("liebrebuffduration");
            if (Version >= 44)
                EncounterTimes = tag.GetByte("liebremettimes");
        }

        public class LiebreCompanion : TerraGuardian
        {
            public List<FallenSoul> ActiveSouls = new List<FallenSoul>();
            public byte LastDefeatedAllyCount = 0;
            public Dictionary<Player, int> PlayerDeathCounter = new Dictionary<Player, int>();
            public byte MouthOpenTime = 0;
            public bool HoldingScythe = true;
            public byte ScytheType = 1;
            public const int ScytheDiagonalHoldX = 12, ScytheDiagonalHoldY = 48;
            public const int ScytheVerticalHoldX = 33, ScytheVerticalHoldY = 52;
            public Vector2 ScythePosition = Vector2.Zero, ScytheSpeed = Vector2.Zero;
            public float ScytheRotation = 0f;
            public bool ScytheFacingLeft = false;
            public byte ScythePickupDelay = 0;
            LiebreData GetData => Data as LiebreData;
            Color DrawSkeletonColor = Color.White;
            public const int MaxSoulsContainedValue = 1000;

            public override void UpdateCompanionHook()
            {
                UpdateScythe();
                UpdateSoul();
            }

            public override void OnPlayerDeathHook(Player Target)
            {
                SpawnSoul(Target.Center, 1, Target, HoverOnly: true);
            }

            public override void OnNpcDeathHook(NPC Target)
            {
                if (Target.lifeMax > 5 || Target.damage == 0)
                {
                    bool SpawnSoul = true;
                    /*if (Target.type == Terraria.ID.NPCID.Creeper)
                    {
                        SpawnSoul = false;
                    }
                    if (Target.type >= Terraria.ID.NPCID.EaterofWorldsHead && Target.type <= Terraria.ID.NPCID.EaterofWorldsTail)
                    {
                        if (!NPC.AnyNPCs(Terraria.ID.NPCID.EaterofWorldsBody))
                        {
                            SpawnSoul = false;
                        }
                    }*/
                    int SoulValue = Math.Max(1, Target.lifeMax / 120);
                    if (SpawnSoul)
                    {
                        if (Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[Target.type])
                        {
                            SoulValue += 15;
                        }
                        TrySpawningSoul(Target.Center, SoulValue, null);
                    }
                }
            }

            public override void OnCompanionDeathHook(Companion Target)
            {
                SpawnSoul(Target.Center, 1, Target, HoverOnly: true);
            }

            public void TrySpawningSoul(Vector2 Position, int SoulValue = 1, Player Owner = null, bool HoverOnly = false)
            {
                if (MathF.Abs(Position.X - Center.X) < 2000 && 
                    MathF.Abs(Position.Y - Center.Y) < 1600)
                    SpawnSoul(Position, SoulValue, Owner, HoverOnly);
            }

            void UpdateSoul()
            {
                if (MouthOpenTime > 0) MouthOpenTime--;
                Vector2 SoulEndPos = GetMouthPosition(BodyFrameID) * Scale;
                Vector2 SoulHoldPos = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[1], 1) - new Vector2(0, 8);
                SoulEndPos.X -= SpriteWidth * 0.5f;
                if (direction < 0)
                    SoulEndPos.X *= -1;
                SoulEndPos.Y = -SpriteHeight + SoulEndPos.Y;
                SoulEndPos += Bottom;
                int DefeatedAllyCount = 0;
                bool CanPullSouls = !CCed && KnockoutStates == KnockoutStates.Awake && (!IsRunningBehavior || GetGoverningBehavior() is not Liebre.SoulUnloadingAction);
                PlayerSoulPosition = Vector2.Zero;
                for (int s = 0; s < ActiveSouls.Count; s++)
                {
                    float MaxSoulSpeed = 12f;
                    FallenSoul soul = ActiveSouls[s];
                    Vector2 DirectionComparer = ((soul.HoverOnly ? SoulHoldPos : SoulEndPos) + velocity - soul.Position);
                    if (soul.HoverOnly)
                    {
                        if (!soul.IsOwnerActive)
                        {
                            soul.HoverOnly = false;
                        }
                        else if (soul.IsOwnerAlive)
                        {
                            Player player = soul.OwnerCharacter;
                            player.Bottom = Bottom;
                            player.fallStart = (int)player.position.Y / 16;
                            player.immuneTime *= 3;
                            if (Main.rand.Next(2) == 0)
                                SaySomething("*Welcome back.*");
                            else
                                SaySomething("*You returned.*");
                            ActiveSouls.RemoveAt(s);
                            continue;
                        }
                        else if (soul.OwnerCharacter != null && soul.OwnerCharacter.ghost)
                        {
                            soul.HoverOnly = false;
                            switch (Main.rand.Next(2))
                            {
                                default:
                                    SaySomething("*Your quest is over, " + soul.OwnerCharacter.name + ".*");
                                    break;
                                case 1:
                                    SaySomething("*Your time has came, " + soul.OwnerCharacter.name + ".*");
                                    break;
                            }
                            if (soul.OwnerCharacter == MainMod.GetLocalPlayer)
                            {
                                SoulSaved = true;
                            }
                            if (PlayerDeathCounter.ContainsKey(soul.OwnerCharacter))
                                PlayerDeathCounter.Remove(soul.OwnerCharacter);
                            PlayerDeathCounter.Add(soul.OwnerCharacter, 0);
                        }
                        if (LastDefeatedAllyCount > 1)
                            DirectionComparer += new Vector2(5f * (float)Math.Sin((float)DefeatedAllyCount / LastDefeatedAllyCount * 360), 5f * (float)Math.Cos((float)DefeatedAllyCount / LastDefeatedAllyCount * 360));
                        DefeatedAllyCount++;
                    }
                    else
                    {
                        if (soul.LifeTime == 255)
                        {
                            ActiveSouls.RemoveAt(s);
                            continue;
                        }
                        soul.LifeTime++;
                    }
                    if (CanPullSouls || soul.HoverOnly)
                    {
                        float Distance = DirectionComparer.Length();
                        if (Distance > 160)
                        {
                            MaxSoulSpeed *= 2;
                        }
                        {
                            if (!soul.HoverOnly)
                                MouthOpenTime = 3;
                            if (Distance < 40)
                            {
                                soul.Velocity *= 0.9f;
                            }
                            /*if (Distance < 4)
                            {
                                if (soul.HoverOnly)
                                {
                                    soul.Velocity *= 0.99f;
                                }
                            }
                            else*/
                            {
                                //DirectionComparer.Normalize();
                                soul.Velocity++;// += DirectionComparer;
                            }
                            if (soul.Velocity > MaxSoulSpeed)
                            {
                                soul.Velocity = MaxSoulSpeed;
                            }
                        }
                        if (Distance < soul.Velocity)
                        {
                            soul.Velocity = Distance;
                        }
                    }
                    else
                    {
                        soul.Velocity *= 0.99f;
                    }
                    if (DirectionComparer.Length() > 0)
                    {
                        DirectionComparer.Normalize();
                    }
                    {
                        int PixelDistanceCalc = (int)((DirectionComparer * soul.Velocity).Length());
                        for (int i = 0; i < PixelDistanceCalc; i++)
                        {
                            Vector2 EffectPos = new Vector2(soul.Position.X, soul.Position.Y) + DirectionComparer * i;
                            int dustid = Dust.NewDust(soul.Position, 8, 8, 175, 0f, 0f, 100, default(Color), 2f * (1f + 0.2f * (soul.SoulValue - 1)));
                            Main.dust[dustid].noGravity = true;
                            Dust dust = Main.dust[dustid];
                            dust.velocity *= 0f;
                        }
                    }
                    soul.Position += DirectionComparer * soul.Velocity;
                    if (!soul.HoverOnly && (soul.Position - SoulEndPos).Length() < 4)
                    {
                        soul.Position = SoulEndPos;
                        ActiveSouls.RemoveAt(s);
                        LiebreData data = GetData;
                        int LastCapacity = data.SoulsLoaded;
                        if (HasBeenMet)
                        {
                            data.SoulsLoaded += soul.SoulValue;
                        }
                        //SoundEngine.PlaySound(Terraria.ID.SoundID.Item3, Center);
                        if (Owner != null)
                        {
                            if (data.SoulsLoaded >= MaxSoulsContainedValue)
                            {
                                RunBehavior(new Liebre.SoulUnloadingAction());
                            }
                            else
                            {
                                const int NearlyFull = (int)(MaxSoulsContainedValue * 0.9f),
                                    TwoThirdsThere = (int)(MaxSoulsContainedValue * 0.6f);
                                if (LastCapacity < NearlyFull && data.SoulsLoaded >= NearlyFull)
                                {
                                    SaySomething("*I'm nearly reaching my capacity...*");
                                }
                                if (LastCapacity < TwoThirdsThere && data.SoulsLoaded >= TwoThirdsThere)
                                {
                                    SaySomething("*I can deliver the souls now.*");
                                }
                            }
                        }
                        else
                        {
                            if (data.SoulsLoaded >= (int)(MaxSoulsContainedValue * .9f))
                            {
                                RunBehavior(new Liebre.SoulUnloadingAction());
                            }
                        }
                        for (int effect = 0; effect < 5; effect++)
                        {
                            int dustid = Dust.NewDust(soul.Position, 8, 8, 175, 0f, 0f, 100, default(Color), 2f);
                            Main.dust[dustid].noGravity = true;
                            Dust dust = Main.dust[dustid];
                            dust.velocity = dust.position - soul.Position;
                            dust.velocity.Normalize();
                            dust.velocity *= 3f;
                        }
                        continue;
                    }
                    if (soul.OwnerCharacter == MainMod.GetLocalPlayer)
                    {
                        PlayerSoulPosition = soul.Position;
                    }
                    for (int effect = 0; effect < 3; effect++)
                    {
                        int dustid = Dust.NewDust(soul.Position, 8, 8, 175, 0f, 0f, 100, default(Color), 2f);
                        Main.dust[dustid].noGravity = true;
                        Dust dust = Main.dust[dustid];
                        dust.velocity *= 0f;
                    }
                }
                Player[] Keys = PlayerDeathCounter.Keys.ToArray();
                foreach (Player k in Keys)
                {
                    if (!k.active)
                        PlayerDeathCounter.Remove(k);
                    else
                    {
                        k.position = Vector2.Zero;
                        if (MainMod.GetLocalPlayer == k && SoulSaved)
                        {
                            PlayerSoulPosition = Center;
                        }
                        if (PlayerDeathCounter[k] < 151)
                        {
                            PlayerDeathCounter[k]++;
                            if (PlayerDeathCounter[k] == 150)
                            {
                                if (IsPlayerBuddy(k))
                                {
                                    SaySomething("*You took care of me while you was alive. Time for me to retribute the favor.*");
                                }
                                else
                                {
                                    switch (Main.rand.Next(3))
                                    {
                                        default:
                                            SaySomething("Time to take you to your resting place.");
                                            break;
                                        case 1:
                                            SaySomething("I'll make sure to bring you to your resting place safe and sound.");
                                            break;
                                        case 2:
                                            SaySomething("You fought well until the very end, time to get you some rest.");
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
                LastDefeatedAllyCount = (byte)DefeatedAllyCount;
            }

            void UpdateScythe()
            {
                bool CantHoldScythe = (HeldItems[1].ItemAnimation > 0) || KnockoutStates > KnockoutStates.Awake;
                if (HoldingScythe && CantHoldScythe)
                {
                    DetachScythe();
                }
                else
                {
                    if (CantHoldScythe) ScythePickupDelay = 150;
                    if (!HoldingScythe)
                    {
                        Vector2 MoveDirection = (Center - ScythePosition);
                        const float MaxMoveSpeed = 8f, MoveSpeed = 0.15f;
                        bool AtXPosition = false, AtYPosition = false;
                        if (Math.Abs(MoveDirection.X) < width * 0.5f)
                        {
                            if (Math.Abs(ScytheSpeed.X) > 1f)
                                ScytheSpeed *= 0.8f;
                            AtXPosition = true;
                        }
                        else
                        {
                            if (MoveDirection.X < 0)
                            {
                                ScytheSpeed.X -= MoveSpeed;
                                if (ScytheSpeed.X < -MaxMoveSpeed)
                                    ScytheSpeed.X = -MaxMoveSpeed;
                            }
                            else
                            {
                                ScytheSpeed.X += MoveSpeed;
                                if (ScytheSpeed.X > MaxMoveSpeed)
                                    ScytheSpeed.X = MaxMoveSpeed;
                            }
                        }
                        if (Math.Abs(MoveDirection.Y) < height * 0.5f)
                        {
                            if (Math.Abs(ScytheSpeed.Y) > 1f)
                                ScytheSpeed *= 0.8f;
                            AtYPosition = true;
                        }
                        else
                        {
                            if (MoveDirection.Y < 0)
                            {
                                ScytheSpeed.Y -= MoveSpeed;
                                if (ScytheSpeed.Y < -MaxMoveSpeed)
                                    ScytheSpeed.Y = -MaxMoveSpeed;
                            }
                            else
                            {
                                ScytheSpeed.Y += MoveSpeed;
                                if (ScytheSpeed.Y > MaxMoveSpeed)
                                    ScytheSpeed.Y = MaxMoveSpeed;
                            }
                        }
                        ScythePosition += ScytheSpeed;
                        if (ScythePickupDelay == 0 && AtXPosition && AtYPosition)
                        {
                            HoldingScythe = true;
                        }
                        ScytheRotation += ScytheSpeed.Length() * 0.025f * direction;
                    }
                }
                if (ScythePickupDelay > 0) ScythePickupDelay--;
            }

            public void DetachScythe()
            {
                ScythePickupDelay = 150;
                HoldingScythe = false;
                ScythePosition = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[1], 1);
                ScytheRotation = 0;
                ScytheSpeed = Vector2.Zero;
                ScytheFacingLeft = direction < 0;
                byte ScytheType = 0;
                switch (ArmFramesID[1])
                {
                    default:
                        ScytheType = 1;
                        break;
                    case 16:
                        ScytheRotation = -1.57079633f * direction;
                        break;
                    case 17:
                        break;
                }
                this.ScytheType = ScytheType;
            }

            public void SpawnSoul(Vector2 Position, int SoulValue = 1, Player Owner = null, bool HoverOnly = false)
            {
                ActiveSouls.Add(new FallenSoul() { Position = Position, SoulValue = SoulValue, OwnerCharacter = Owner, HoverOnly = HoverOnly });
            }

            public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
            {
                LiebreData data = GetData;
                float MinOpacity = (float)data.SoulsLoaded / (250 + data.SoulsLoaded);
                Color color = Holder.DrawColor;
                DrawSkeletonColor = Holder.DrawColor;
                float OpacityRate = 1f - (MathHelper.Max(MinOpacity, (float)(color.R + color.G + color.B) / (255 * 3)));
                Holder.DrawColor = Color.White * OpacityRate;
            }

            public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
            {
                if (!Holder.GetCompanion.GetGoverningBehavior().IsVisible) return;
                bool HasSoulNearby = MouthOpenTime > 0;
                int MouthFrame = 0;
                switch(BodyFrameID)
                {
                    case 16:
                        MouthFrame = 1;
                        break;
                    case 17:
                        MouthFrame = 2;
                        break;
                    case 18:
                        MouthFrame = 3;
                        break;
                }
                DrawData? ScytheSlot = DrawEquippedScythe(DrawSkeletonColor, Holder);
                for (int i = DrawDatas.Count - 1; i >= 0; i--)
                {
                    DrawData curdd = DrawDatas[i];
                    DrawData dd;
                    if (curdd.texture == Holder.BodyTexture)
                    {
                        dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(SkeletonBodyID), Holder.DrawPosition, Holder.BodyFrame, DrawSkeletonColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                        DrawDatas.Insert(i, dd);
                        if (HasSoulNearby)
                        {
                            Rectangle MouthRect = new Rectangle(Base.SpriteWidth * MouthFrame, Base.SpriteHeight, Base.SpriteWidth, Base.SpriteHeight);
                            dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(MouthLitID), Holder.DrawPosition, MouthRect, Holder.DrawColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                            dd.shader = Holder.BodyShader;
                            DrawDatas.Insert(i+2, dd);
                            dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(MouthID), Holder.DrawPosition, MouthRect, Holder.DrawColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                            dd.shader = Holder.BodyShader;
                            DrawDatas.Insert(i+2, dd);
                            MouthRect.Y = 0;
                            dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(MouthLitID), Holder.DrawPosition, MouthRect, Holder.DrawColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                            dd.shader = Holder.HeadShader;
                            DrawDatas.Insert(i+1, dd);
                            dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(MouthID), Holder.DrawPosition, MouthRect, Holder.DrawColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                            dd.shader = Holder.HeadShader;
                            DrawDatas.Insert(i+1, dd);
                        }
                        Rectangle EyeRect = new Rectangle(GetEyesFrame() * Base.SpriteWidth, 0, Base.SpriteWidth, Base.SpriteHeight);
                        dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(EyesID), Holder.DrawPosition, EyeRect, Color.White, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                        dd.shader = Holder.HeadShader;
                        DrawDatas.Insert(i + 1, dd);
                    }
                    else if (curdd.texture == Holder.ArmTexture[0])
                    {
                        dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(SkeletonLeftArmID), Holder.DrawPosition, Holder.ArmFrame[0], DrawSkeletonColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                        dd.shader = Holder.HeadShader;
                        DrawDatas.Insert(i, dd);
                    }
                    else if (curdd.texture == Holder.ArmTexture[1])
                    {
                        dd = new DrawData(Base.GetSpriteContainer.GetExtraTexture(SkeletonRightArmID), Holder.DrawPosition, Holder.ArmFrame[1], DrawSkeletonColor, fullRotation, Holder.Origin, Scale, drawSet.playerEffect, 0);
                        dd.shader = Holder.HeadShader;
                        DrawDatas.Insert(i, dd);
                        if (ScytheSlot.HasValue)
                            DrawDatas.Insert(i, ScytheSlot.Value);
                    }
                }
                if (!ScytheSlot.HasValue)
                {
                    DrawDatas.Insert(0, DrawFlyingScythe(DrawSkeletonColor));
                }
                Texture2D SoulTexture = TextureAssets.Projectile[ProjectileID.LostSoulHostile].Value;
                foreach (FallenSoul soul in ActiveSouls)
                {
                    Vector2 SoulPosition = soul.Position - Main.screenPosition;
                    DrawData dd = new DrawData(SoulTexture, SoulPosition, null, Color.White, 0f, new Vector2(SoulTexture.Width, SoulTexture.Height) * 0.5f, 1f + (soul.SoulValue - 1) * .1f, SpriteEffects.None);
                    DrawDatas.Add(dd);
                }
            }

            int GetEyesFrame()
            {
                switch(BodyFrameID)
                {
                    default:
                        return 0;
                    case 16:
                        return 1;
                    case 17:
                    case 19:
                    case 20:
                    case 21:
                        return 2;
                    case 18:
                        return 3;
                }
            }

            protected override void PreInitialize()
            {
                var _forceload = TextureAssets.Projectile[ProjectileID.LostSoulHostile].Value;
            }

            DrawData DrawFlyingScythe(Color color)
            {
                Vector2 ScythePosition = this.ScythePosition - Main.screenPosition;
                SpriteEffects ScytheEffect = SpriteEffects.None;
                Vector2 ScytheOrigin = (ScytheType == 0 ?
                    new Vector2(ScytheVerticalHoldX, ScytheVerticalHoldY) :
                    new Vector2(ScytheDiagonalHoldX, ScytheDiagonalHoldY));
                if (!ScytheFacingLeft)
                {
                    switch (ScytheType)
                    {
                        case 0:
                            ScytheEffect = SpriteEffects.FlipHorizontally;
                            ScytheOrigin.X = 66 - ScytheOrigin.X;
                            break;
                        case 1:
                            ScytheEffect = SpriteEffects.FlipHorizontally;
                            ScytheOrigin.X = 66 - ScytheOrigin.X;
                            break;
                    }
                }
                Texture2D ScytheTexture = Base.GetSpriteContainer.GetExtraTexture(ScytheID);
                return new DrawData(ScytheTexture, ScythePosition, new Rectangle(ScytheType * 66, 0, 66, 66), color, ScytheRotation, ScytheOrigin, Scale, ScytheEffect, 0);
            }

            DrawData? DrawEquippedScythe(Color color, TgDrawInfoHolder Holder)
            {
                if (!HoldingScythe) return null;
                Vector2 ScythePosition = GetAnimationPosition(AnimationPositions.HandPosition, ArmFramesID[1], 1, false, false, false, false, false);
                if (direction < 0)
                    ScythePosition.X = SpriteWidth - ScythePosition.X;
                ScythePosition += Holder.DrawPosition;
                SpriteEffects ScytheEffect = SpriteEffects.None;
                float ScytheRotation = 0f;
                byte ScytheType = 0;
                switch (ArmFramesID[1])
                {
                    default:
                        ScytheType = 1;
                        break;
                    case 16:
                        ScytheRotation = -1.57079633f * direction;
                        break;
                    case 17:
                        break;
                }
                Vector2 ScytheOrigin = (ScytheType == 0 ?
                    new Vector2(ScytheVerticalHoldX, ScytheVerticalHoldY) :
                    new Vector2(ScytheDiagonalHoldX, ScytheDiagonalHoldY));
                if (direction > 0)
                {
                    switch (ScytheType)
                    {
                        case 0:
                            ScytheEffect = SpriteEffects.FlipHorizontally;
                            ScytheOrigin.X = 66 - ScytheOrigin.X;
                            break;
                        case 1:
                            ScytheEffect = SpriteEffects.FlipHorizontally;
                            ScytheOrigin.X = 66 - ScytheOrigin.X;
                            break;
                    }
                }
                Texture2D ScytheTexture = Base.GetSpriteContainer.GetExtraTexture(ScytheID);
                return new DrawData(ScytheTexture, ScythePosition, new Rectangle(ScytheType * 66, 0, 66, 66), color, ScytheRotation, ScytheOrigin, Scale, ScytheEffect);
            }

            public Vector2 GetMouthPosition(int Frame)
            {
                switch (Frame)
                {
                    case 16:
                        return new Vector2(32, 32);
                    case 17:
                        return new Vector2(32, 34);
                    case 18:
                        return new Vector2(42, 40);
                    case 19:
                        return new Vector2(27, 38);
                }
                return new Vector2(40, 34);
            }

            public class FallenSoul
            {
                public Vector2 Position = Vector2.Zero;
                public float Velocity = 0;
                public Player OwnerCharacter = null;
                public bool HoverOnly = false;
                public byte LifeTime = 0;
                public int SoulValue = 1;

                public bool IsOwnerActive
                {
                    get
                    {
                        return OwnerCharacter != null && OwnerCharacter.active;
                    }
                }

                public bool IsOwnerAlive
                {
                    get
                    {
                        return OwnerCharacter != null && !OwnerCharacter.dead && !OwnerCharacter.ghost;
                    }
                }
            }
        }

        public class LiebreData : CompanionData
        {
            protected override uint CustomSaveVersion => 1;
            public int SoulsLoaded = 0;

            public override void CustomSave(TagCompound save, uint UniqueID)
            {
                save.Add("SoulCount_"+UniqueID, SoulsLoaded);
            }

            public override void CustomLoad(TagCompound tag, uint UniqueID, uint LastVersion)
            {
                if (LastVersion == 0) return;
                SoulsLoaded = tag.GetInt("SoulCount_"+UniqueID);
            }
        }
    }
}