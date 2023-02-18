using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Audio;
using ReLogic.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public partial class Companion : Player
    {
        internal static bool ScanBiomes = false;
        public virtual bool DropFromPlatform { get {return controlDown; } }
        public int GetFallTolerance { get { return Base.FallHeightTolerance + extraFall; }}
        public float Accuracy = 50, Trigger = 50;
        private SceneMetrics BiomeCheck = new SceneMetrics();

        private void LogCompanionStatusToData()
        {
            Data.MaxHealth = statLifeMax;
            Data.MaxMana = statManaMax;
            Data.ExtraAccessorySlot = extraAccessory;
        }

        public void UpdateCompanion()
        {
            //int PlayerBackup = Main.myPlayer; 
            Main.myPlayer = whoAmI; //= 255 ; Always restore Main.myPlayer if ANY script here ends before the end of the script.
            ReferedCompanion = this;
            NewAimDirectionBackup = AimDirection;
            try
            {
                InnerUpdate();
            }
            catch{ }
            WorldMod.UpdateCompanionCount(this);
            UpdateAimMovement();
            Main.myPlayer = MainMod.MyPlayerBackup; //PlayerBackup;
            ReferedCompanion = null;
        }

        private void InnerUpdate()
        {
            try
            {
                ScaleUpdate();
                FlipWeaponUsageHand = false;
                //Scale *= 1f + MathF.Sin(SystemMod.HandyCounter * 0.01f) * 0.5f; //Handy for testing positioning
                ResetMobilityStatus();
                ResetControls();
                LiquidMovementHindering();
                float SpaceGravity = UpdateSpaceGravity();
                if(vortexDebuff)
                {
                    gravity = 0;
                }
                UpdateTimers();
                ResizeHitbox();
                UpdateHairDyeDust();
                UpdateMiscCounter();
                PlayerLoader.PreUpdate(this);
                UpdateBehaviour();
                heldProj = -1;
                if(Base.CanCrouch && Crouching)
                {
                    if(itemAnimation == 0)
                    {
                        if (MoveLeft)
                            direction = -1;
                        else if (MoveRight)
                            direction = 1;
                    }
                    MoveLeft = MoveRight = false;
                }
                UpdateProjCaches();
                if(UpdateDeadState())
                {
                    return;
                }
                if(IsLocalCompanion)
                {
                    TryPortalJumping();
                    UpdateDoorHelper();
                    //doorHelper.Update(this);
                }
                UpdateFallDamage(SpaceGravity);
                UpdateTileTargetPosition();
                UpdateImmunity();
                DoResetEffects();
                UpdateProjCaches();
                UpdateDyes();
                _accessoryMemory = 0;
                _accessoryMemory2 = 0;
                bool UnderwaterFlag;
                UpdateBuffs(out UnderwaterFlag);
                UpdateEquipments(UnderwaterFlag);
                UpdateWalkMode();
                UpdateCapabilitiesMemory();
                UpdateBiomes();
                UpdateInteractions();
                BlockMovementWhenUsingHeavyWeapon();
                //UpdatePulley(); //Needs to be finished
                UpdateRunSpeeds();
                sandStorm = false;
                UpdateJump();
                UpdateOtherMobility();
                LateControlUpdate();
                GrappleMovement();
                UpdateCollisions();
                UpdateItem();
                UpdateAnimations();
                FinishingScripts();
                UpdateChatMessage();
                UpdateExtra();
            }
            catch
            {

            }
        }

        private void ResetProjCaches()
        {
            highestStormTigerGemOriginalDamage = 0;
            highestAbigailCounterOriginalDamage = 0;
            for (int i = 0; i < ownedProjectileCounts.Length; i++) ownedProjectileCounts[i] = 0;
        }

        private void UpdateProjCaches()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (!Main.projectile[i].active || !ProjMod.IsThisCompanionProjectile(i, this))
                    continue;
                ownedProjectileCounts[Main.projectile[i].type]++;
                switch(Main.projectile[i].type)
                {
                    case 831:
                        {
                            highestStormTigerGemOriginalDamage = Math.Max(Main.projectile[i].originalDamage, highestStormTigerGemOriginalDamage);
                        }
                        break;
                    case 832:
                        {
                            highestAbigailCounterOriginalDamage = Math.Max(Main.projectile[i].originalDamage, highestAbigailCounterOriginalDamage);
                        }
                        break;
                }
            }
        }

        new public void UpdateBiomes()
        {
            if (ScanBiomes) ScanAround();
            Tile CenterTile = Framing.GetTileSafely(Center);
            ZoneDungeon = false;
            if (BiomeCheck.DungeonTileCount >= 250 && this.Center.Y > Main.worldSurface * 16)
            {
                if (CenterTile != null && Main.wallDungeon[CenterTile.WallType])
                {
                    ZoneDungeon = true;
                }
            }
            if (CenterTile != null)
                behindBackWall = CenterTile.WallType > 0;
            if (behindBackWall)
            {
                if (ZoneDesert && Center.Y > Main.worldSurface)
                {
                    if (WallID.Sets.Conversion.Sandstone[CenterTile.WallType] || WallID.Sets.Conversion.HardenedSand[CenterTile.WallType])
                        ZoneUndergroundDesert = true;
                }
                if (CenterTile.WallType == 184 || CenterTile.WallType == 180)
                    ZoneGranite = true;
                if (CenterTile.WallType == 183 || CenterTile.WallType == 178)
                    ZoneMarble = true;
                if (CenterTile.WallType == 108 || CenterTile.WallType == 86)
                    ZoneHive = true;
                if (CenterTile.WallType >= 48 && CenterTile.WallType <= 53)
                    ZoneGemCave = true;
            }
            else
            {
                ZoneUndergroundDesert = false;
                ZoneGranite = false;
                ZoneMarble = false;
                ZoneHive = false;
                ZoneGemCave = false;
            }
            ZoneCorrupt = BiomeCheck.EnoughTilesForCorruption;
            ZoneCrimson = BiomeCheck.EnoughTilesForCrimson;
            ZoneHallow = BiomeCheck.EnoughTilesForHallow;
            ZoneJungle = BiomeCheck.EnoughTilesForJungle;
            ZoneSnow = BiomeCheck.EnoughTilesForSnow;
            ZoneDesert = BiomeCheck.EnoughTilesForDesert;
            ZoneGlowshroom = BiomeCheck.EnoughTilesForGlowingMushroom;
            ZoneMeteor = BiomeCheck.EnoughTilesForMeteor;
            ZoneWaterCandle = BiomeCheck.WaterCandleCount > 0;
            ZonePeaceCandle = BiomeCheck.PeaceCandleCount > 0;
            ZoneGraveyard = BiomeCheck.EnoughTilesForGraveyard;
            ZoneCorrupt = BiomeCheck.EnoughTilesForCorruption;
            if (HasGardenGnomeNearby != BiomeCheck.HasGardenGnome)
            {
                luckNeedsSync = true;
                HasGardenGnomeNearby = BiomeCheck.HasGardenGnome;
            }
            Point CompanionTile = Center.ToTileCoordinates();
            ZoneUnderworldHeight = CompanionTile.Y > Main.UnderworldLayer;
            ZoneRockLayerHeight = CompanionTile.Y <= Main.UnderworldLayer && CompanionTile.Y > Main.rockLayer;
            ZoneDirtLayerHeight = CompanionTile.Y <= Main.rockLayer && CompanionTile.Y > Main.worldSurface * 0.35f;
            ZoneSkyHeight = CompanionTile.Y <= Main.worldSurface * 0.35f;
            ZoneBeach = WorldGen.oceanDepths(CompanionTile.X, CompanionTile.Y);
            ZoneRain = Main.raining && CompanionTile.Y <= Main.worldSurface;
            ZoneSandstorm = ZoneDesert && !ZoneBeach && Sandstorm.Happening && CompanionTile.Y <= Main.worldSurface;
            ZonePurity = InZonePurity();
        }

        private void ScanAround()
        {
            Rectangle Region = new Rectangle((int)(Center.X * DivisionBy16), (int)(Center.Y * DivisionBy16), 600, 400);
            Region.X -= (int)(Region.Width * 0.5f);
            Region.Y -= (int)(Region.Height * 0.5f);
            BiomeCheck.ScanAndExportToMain(new SceneMetricsScanSettings(){ VisualScanArea = Region, BiomeScanCenterPositionInWorld = Center, ScanOreFinderData = false });
            ZoneTowerNebula = ZoneTowerSolar = ZoneTowerStardust = ZoneTowerVortex = false;
            for (int i = 0; i < 200; i++)
            {
                if (!Main.npc[i].active) continue;
                const float MaxDistance = 4000;
                switch(Main.npc[i].type)
                {
                    case 493:
                        if (Distance(Main.npc[i].Center) <= MaxDistance)
                        {
                            ZoneTowerStardust = true;
                        }
                        break;
                    case 507:
                        if (Distance(Main.npc[i].Center) <= MaxDistance)
                        {
                            ZoneTowerNebula = true;
                        }
                        break;
                    case 422:
                        if (Distance(Main.npc[i].Center) <= MaxDistance)
                        {
                            ZoneTowerVortex = true;
                        }
                        break;
                    case 517:
                        if (Distance(Main.npc[i].Center) <= MaxDistance)
                        {
                            ZoneTowerSolar = true;
                        }
                        break;
                    case 549:
                        {
                            if (Distance(Main.npc[i].Center) <= MaxDistance)
                            {
                                ZoneOldOneArmy = true;
                            }
                        }
                        break;
                }
            }
        }

        public void UpdateWalkMode()
        {
            if(!WalkMode) return;
            maxRunSpeed = accRunSpeed = 1.5f;
            runAcceleration = 0.1f;
            runSlowdown = 0.1f;
            dashType = 0;
            dashDelay = 30;
        }

        public void ScaleUpdate(bool ForceUpdate = false)
        {
            if(this is TerraGuardian)
            {
                if(PlayerSizeMode)
                    FinalScale = Base.GetPlayerSizeScale;
                else
                    FinalScale = Base.Scale;
            }
            else
                FinalScale = 1;
            if (ForceUpdate)
                Scale = FinalScale;
            else
            {
                if(Math.Abs(Scale - FinalScale) < 0.01f)
                    Scale = FinalScale;
                else
                    Scale = MathHelper.Lerp(Scale, FinalScale, 1f / 60);
            }
        }

        private void UpdateCapabilitiesMemory()
        {
            HasRunningAbility = maxRunSpeed != accRunSpeed;
            HasIceSkatesAbility = iceSkate;
            HasDashingdodgeAbility = dashType > 0;
            HasSwimmingAbility = accFlipper || accMerman;
            HasWaterbreathingAbility = gills || accMerman;
            HasLavaImmunityAbility = lavaImmune;
            HasFeatherfallAbility = slowFall;
            HasDoubleJumpBottleAbility = hasJumpOption_Basilisk || hasJumpOption_Blizzard || hasJumpOption_Cloud || hasJumpOption_Fart || hasJumpOption_Sail || hasJumpOption_Sandstorm || hasJumpOption_Santank || hasJumpOption_Unicorn || hasJumpOption_WallOfFleshGoat;
            HasExtraJumpAbility = jumpBoost || frogLegJumpBoost;
            HasFallDamageImmunityAbility = noFallDmg || slowFall;
            HasGravityFlippingAbility = gravControl || gravControl2;
            HasWallClimbingAbility = spikedBoots > 0;
            HasWaterWalkingAbility = waterWalk || waterWalk2;
            HasFlightAbility = wingsLogic > 0;
        }

        protected void UpdateDoorHelper()
        {
            bool IgnoreDoors = IsMountedOnSomething && Base.MountStyle == MountStyles.CompanionRidesPlayer;
            float VelocityXBackup = velocity.X;
            if (!IgnoreDoors)
            {
                if(velocity.X == 0)
                {
                    if(MoveRight)
                    {
                        velocity.X += runAcceleration;
                    }
                    if(MoveLeft)
                    {
                        velocity.X -= runAcceleration;
                    }
                }
            }
            ResizeHitbox(true);
            doorHelper.LookForDoorsToClose(this);
            if (!IgnoreDoors) doorHelper.LookForDoorsToOpen(this);
            velocity.X = VelocityXBackup;
        }

        private void BlockMovementWhenUsingHeavyWeapon()
        {
            if (itemAnimation > 0 && Items.GuardianItemPrefab.GetItemType(HeldItem) == Items.GuardianItemPrefab.ItemType.Heavy)
            {
                MoveLeft = MoveRight = ControlJump = MoveUp = false;
                if(!Base.CanCrouch || itemAnimation == 0)
                    MoveDown = false;
            }
        }

        private void UpdateChatMessage()
        {
            if(chatOverhead.timeLeft > 0) chatOverhead.timeLeft--;
        }

        private void UpdateCollisions()
        {
            ResizeHitbox(true);
            StickyMovement();
            if(gravDir == -1f)
            {
                waterWalk = waterWalk2 = false;
            }
            LiquidCollisionScript();
            if (Main.expertMode && ZoneSnow && wet && !lavaWet && !honeyWet && !arcticDivingGear && environmentBuffImmunityTimer == 0)
            {
                AddBuff(46, 150);
            }
            UpdateGraphicsOffset();
            OtherCollisionScripts();
            UpdateFallingAndMovement();
            ResizeHitbox(false);
            CheckDrowning();
        }

        new public virtual void CheckDrowning()
        {
            base.CheckDrowning();
        }

        public void DoResetEffects()
        {
            int tileRangeXBackup = tileRangeX, tileRangeYBackup = tileRangeY;
            //Main.myPlayer = MainMod.MyPlayerBackup; //Workaround for interface issues
            ResetEffects(); //Is causing issues with interfaces, because some mods updates their interface on reset effects.
            //Main.myPlayer = whoAmI;
            luckPotion = 0;
            if (!IsPlayerCharacter)
            {
                tileRangeX = tileRangeXBackup;
                tileRangeY = tileRangeYBackup;
            }
            ResizeHitbox();
            LogCompanionStatusToData();
            int LCs = (int)(Math.Min((statLifeMax - 100) * 0.05f, 15)), LFs = 0;
            if(statLifeMax > 400)
            {
                LFs = (int)(Math.Min((statLifeMax - 400) * 0.2f, 20));
            }
            statLifeMax2 = Base.InitialMaxHealth + Base.HealthPerLifeCrystal * LCs + Base.HealthPerLifeFruit * LFs;
            int MCs = (int)((Math.Min((statManaMax - 20) * 0.05f, 9)));
            statManaMax2 = Base.InitialMaxMana + Base.ManaPerManaCrystal * MCs;
            Accuracy = Base.AccuracyPercent;
            Trigger = Base.TriggerPercent;
            DodgeRate = 0;
            BlockRate = 0;
            if(this is TerraGuardian)
                DefenseRate = (statDefense * 0.002f);
            else
                DefenseRate = 0;
            bool MaxHealth = statLife >= statLifeMax2;
            GetCommonData.UpdateSkills(this);
            if (statLife < statLifeMax2 && MaxHealth) statLife = statLifeMax2;
            Base.UpdateAttributes(this);
            GetGoverningBehavior().UpdateStatus(this);
        }

        private bool UpdateDeadState()
        {
            if(ghost)
            {
                Ghost();
                return true;
            }
            if(dead)
            {
                UpdateDead();
                if(this is TerraGuardian)
                    ((TerraGuardian)this).UpdateDeadAnimation();
                return true;
            }
            return false;
        }

        new public void UpdateDead()
        {
            _portalPhysicsTime = 0;
            MountFishronSpecialCounter = 0f;
            gem = -1;
            ownedLargeGems = 0;
            brainOfConfusionDodgeAnimationCounter = 0;
            ResetFloorFlags();
            wings = 0;
            wingsLogic = 0;
            equippedWings = null;
            ResetVisibleAccessories();
            poisoned = false;
            venom = false;
            onFire = false;
            dripping = false;
            drippingSlime = false;
            drippingSparkleSlime = false;
            hungry = false;
            heartyMeal = false;
            starving = false;
            burned = false;
            suffocating = false;
            onFire2 = false;
            onFire3 = false;
            onFrostBurn = false;
            onFrostBurn2 = false;
            blind = false;
            blackout = false;
            loveStruck = false;
            dryadWard = false;
            stinky = false;
            resistCold = false;
            electrified = false;
            moonLeech = false;
            headcovered = false;
            vortexDebuff = false;
            windPushed = false;
            setForbidden = false;
            setMonkT3 = false;
            setHuntressT3 = false;
            setApprenticeT3 = false;
            setSquireT3 = false;
            setForbiddenCooldownLocked = false;
            setSolar = setVortex = setNebula = setStardust = false;
            nebulaLevelDamage = nebulaLevelLife = nebulaLevelMana = 0;
            trapDebuffSource = false;
            yoraiz0rEye = 0;
            yoraiz0rDarkness = false;
            hasFloatingTube = false;
            hasUnicornHorn = false;
            hasAngelHalo = false;
            hasRainbowCursor = false;
            leinforsHair = false;
            PlayerLoader.UpdateDead(this);
            gravDir = 1f;
            for (int i = 0; i < MaxBuffs; i++)
            {
                if (buffType[i] <= 0 || !Main.persistentBuff[buffType[i]])
                {
                    buffTime[i] = 0;
                    buffType[i] = 0;
                }
            }
            if (IsPlayerCharacter)
            {
                Main.npcChatText = "";
                Main.editSign = false;
                Main.npcChatCornerItem = 0;
            }
            numMinions = 0;
            grappling[0] = -1;
            grappling[1] = -1;
            grappling[2] = -1;
            sign = -1;
            SetTalkNPC(-1);
            statLife = 0;
            channel = false;
            potionDelay = 0;
            chest = -1;
            tileEntityAnchor.Clear();
            changeItem = -1;
            itemAnimation = 0;
            immuneAlpha += 2;
            if (immuneAlpha > 255)
            {
                immuneAlpha = 255;
            }
            headPosition += headVelocity;
            bodyPosition += bodyVelocity;
            legPosition += legVelocity;
            headRotation += headVelocity.X * 0.1f;
            bodyRotation += bodyVelocity.X * 0.1f;
            legRotation += legVelocity.X * 0.1f;
            headVelocity.Y += 0.1f;
            bodyVelocity.Y += 0.1f;
            legVelocity.Y += 0.1f;
            headVelocity.X *= 0.99f;
            bodyVelocity.X *= 0.99f;
            legVelocity.X *= 0.99f;
            for (int j = 0; j < npcTypeNoAggro.Length; j++)
            {
                npcTypeNoAggro[j] = false;
            }
            if (difficulty == 2)
            {
                if (respawnTimer > 0)
                {
                    respawnTimer = Utils.Clamp(respawnTimer - 1, 0, 1800);
                }
                else if (IsLocalCompanion || IsPlayerCharacter || Main.netMode == 2)
                {
                    ghost = true;
                }
            }
            else
            {
                respawnTimer = Utils.Clamp(respawnTimer - 1, 0, 1800);
                if (respawnTimer <= 0 && (IsLocalCompanion || IsPlayerCharacter))
                {
                    Spawn(PlayerSpawnContext.ReviveFromDeath);
                }
            }
        }

        new public void Spawn(PlayerSpawnContext context)
        {
            StopVanityActions();
            bool WasDead = dead;
            if (IsLocalCompanion || IsPlayerCharacter)
            {
                FindSpawn();
                if (!CheckSpawn(SpawnX, SpawnY))
                {
                    SpawnX = -1;
                    SpawnY = -1;
                }
            }
            headPosition = Vector2.Zero;
            bodyPosition = Vector2.Zero;
            legPosition = Vector2.Zero;
            headRotation = 0;
            bodyRotation = 0;
            legRotation = 0;
            rabbitOrderFrame.Reset();
            lavaTime = lavaMax;
            //
            if(statLife <= 0)
            {
                if (spawnMax)
                {
                    statLife = statLifeMax2;
                    statMana = statManaMax2;
                }
                else
                {
                    int NewHealthValue = statLifeMax2 / 2;
                    if(NewHealthValue > statLife) statLife = NewHealthValue;
                    else 
                    statLife = Base.InitialMaxHealth;
                }
                breath = breathMax;
            }
            immune = true;
            if (dead)
            {
                PlayerLoader.OnRespawn(this);
            }
            dead = false;
            immuneTime = 0;
            //
            active = true;
            if(IsTownNpc && !GetTownNpcState.Homeless)
            {
                Spawn_SetPosition(GetTownNpcState.HomeX, GetTownNpcState.HomeY);
            }
            else if(SpawnX >= 0 && SpawnY >= 0)
            {
                Spawn_SetPosition(SpawnX, SpawnY);
            }
            else
            {
                Spawn_SetPosition(Main.spawnTileX, Main.spawnTileY);
            }
            wet = false;
            wetCount = 0;
            lavaWet = false;
            fallStart = (int)(position.Y * DivisionBy16);
            fallStart2 = fallStart;
            velocity.X = 0;
            velocity.Y = 0;
            ResetAdvancedShadows();
            for (int i = 0; i < 3; i++) UpdateSocialShadow();
            oldPosition = position + BlehOldPositionFixer;
            SetTalkNPC(-1);
            //
            if(pvpDeath)
            {
                pvpDeath = false;
                immuneTime = 300;
                statLife = statLifeMax;
            }
            else
            {
                immuneTime = 60;
            }
            if (immuneTime > 0 && !hostile)
                immuneNoBlink = true;
            //
            if (WasDead) immuneAlpha = 255;
            //Well... I guess I wont be updating graveyard
            //They can spawn Baby Finch.
        }

        private void Spawn_SetPosition(int FloorX, int FloorY)
        {
            position.X = FloorX * 16 + 8 - width * 0.5f;
            position.Y = FloorY * 16 - height;
        }

        private void FinishingScripts()
        {
            if (mount.Type == 8)
			{
				mount.UseDrill(this);
			}
			if (statLife > statLifeMax2)
			{
				statLife = statLifeMax2;
			}
			if (statMana > statManaMax2)
			{
				statMana = statManaMax2;
			}
			grappling[0] = -1;
			grapCount = 0;
            releaseUseItem = !controlUseTile;
			UpdateAdvancedShadows();
			PlayerLoader.PostUpdate(this);
        }

        private void UpdateItem()
        {
            numMinions = 0;
            slotsMinions = 0f;
            UpdateItemScript();
        }

        protected virtual void UpdateItemScript()
        {
            bool MaskedMouse = false;
            Vector2 BackedUpAimPosition = Vector2.Zero;
            if(Base.CompanionType == CompanionTypes.Terrarian && HeldItem.shoot > -1)
            {
                SystemMod.BackupMousePosition();
                MaskedMouse = true;
                BackedUpAimPosition = AimDirection;
                AimDirection = GetAimDestinationPosition(GetAimedPosition) - Center;
                Main.mouseX = (int)(GetAimedPosition.X - Main.screenPosition.X);
                Main.mouseY = (int)(GetAimedPosition.Y - Main.screenPosition.Y);
            }
            if(mount.Type != 8) ItemCheck_ManageRightClickFeatures();
            ItemCheck(whoAmI);
            if(MaskedMouse)
            {
                SystemMod.RevertMousePosition();
                AimDirection = BackedUpAimPosition;
            }
        }

        private void UpdateFallingAndMovement()
        {
            bool falling = false;
            if ((base.velocity.Y > gravity) || (base.velocity.Y < -gravity))
            {
                falling = true;
            }
            Vector2 velocity = base.velocity;
            slideDir = 0;
            bool ignorePlats = false, fallThrough = DropFromPlatform;
            if ((gravDir == -1) | (mount.Active && (mount.Cart || mount.Type == 12 || mount.Type == 7 || mount.Type == 8 || mount.Type == 23 || mount.Type == 44 || mount.Type == 48)) | GoingDownWithGrapple)
            {
                ignorePlats = fallThrough = true;
            }
            onTrack = false;
            bool TrackFlag = false;
            if (mount.Active && mount.Cart)
            {
                float SpeedMult = ((ignoreWater || merman) ? 1 : (honeyWet ? 0.25f : (!wet ? 1f : 0.5f)));
                velocity *= SpeedMult;
                DelegateMethods.Minecart.rotation = fullRotation;
                DelegateMethods.Minecart.rotationOrigin = fullRotationOrigin;
                BitsByte CollisionInfo = Minecart.TrackCollision(ref position, ref base.velocity, ref lastBoost, width, height, controlDown, controlUp, fallStart2, false, mount.Delegations);
                if(CollisionInfo[0])
                {
                    onTrack = true;
                    gfxOffY = Minecart.TrackRotation(ref fullRotation, position + base.velocity, width, height, controlDown, controlUp, mount.Delegations);
                    fullRotationOrigin = new Vector2(width * 0.5f, height);
                }
                if(CollisionInfo[1])
                {
                    if(controlLeft || controlRight)
                        cartFlip = !cartFlip;
                    if(base.velocity.X > 0)
                        direction = 1;
                    else if (velocity.X < 0)
                        direction = -1;
                    mount.Delegations.MinecartBumperSound(position, width, height);
                }
                base.velocity /= SpeedMult;
                if (CollisionInfo[3] && IsLocalCompanion)
                {
                    TrackFlag = true;
                }
                if (CollisionInfo[2])
                {
                    cartRampTime = (int)(Math.Abs(base.velocity.X) / mount.RunSpeed * 20);
                }
                if(CollisionInfo[4])
                {
                    trackBoost -= 4f;
                }
                if(CollisionInfo[5])
                    trackBoost += 4;
            }
            Vector2 SavedPosition = position;
            if (vortexDebuff)
                base.velocity.Y = base.velocity.Y * 0.8f + (float)Math.Cos(Center.X % 120f / 120f * ((float)Math.PI * 2)) * (5f * 0.2f);
            PlayerLoader.PreUpdateMovement(this);
            if (tongued)
            {
                base.position += base.velocity;
            }
            else if (honeyWet && !ignoreWater)
            {
                HoneyCollision(fallThrough, ignorePlats);
            }
            else if (wet && !merman && !ignoreWater && !trident)
            {
                WaterCollision(fallThrough, ignorePlats);
            }
            else
            {
                DryCollision(fallThrough, ignorePlats);
                if (mount.Active && mount.IsConsideredASlimeMount && base.velocity.Y != 0 && !SlimeDontHyperJump)
                {
                    float SpeedXBackup = base.velocity.X;
                    base.velocity.X = 0;
                    DryCollision(fallThrough, ignorePlats);
                    base.velocity.X = SpeedXBackup;
                }
                if (mount.Active && mount.Type == 43 && base.velocity.Y != 0)
                {
                    float SpeedXBackup = base.velocity.X;
                    base.velocity.X = 0;
                    DryCollision(fallThrough, ignorePlats);
                    base.velocity.X = SpeedXBackup;
                }
            }
            UpdateTouchingTiles();
            //TryBouncingBlocks(falling);
            //TryLandingOnDetonator();
            if (!tongued)
            {
                SlopingCollision(fallThrough, ignorePlats);
                if (!isLockedToATile)
                {
                    Collision.StepConveyorBelt(this, gravDir);
                }
            }
            if (TrackFlag)
            {
				NetMessage.SendData(13, -1, -1, null, whoAmI);
				Minecart.HitTrackSwitch(new Vector2(base.position.X, base.position.Y), width, height);
            }
            if (velocity.X != base.velocity.X)
            {
                if (velocity.X < 0) slideDir = -1;
                else if (velocity.X > 0) slideDir = 1;
            }
            if (gravDir == 1 && Collision.up)
            {
                base.velocity.Y = 0.01f;
                if (!merman) jump = 0;
            }
            else if (gravDir == -1 && Collision.down)
            {
                base.velocity.Y = -0.01f;
                if (!merman) jump = 0;
            }
            if (base.velocity.Y == 0 && grappling[0] == -1) FloorVisuals(falling);
            if (IsLocalCompanion)
            {
                Collision.SwitchTiles(base.position, width, height, oldPosition, 1);
            }
            BordersMovement();
            //PressurePlateHelper.UpdatePlayerPosition(this); //Disabled temporarily for trouble making
        }

        private void OtherCollisionScripts()
        {
            if(IsLocalCompanion)
            {
                if(!iceSkate) CheckIceBreak();
                CheckCrackedBrickBreak();
            }
            SlopeDownMovement();
            bool AllowStepdownWater = mount.Type == 7 || mount.Type == 8 || mount.Type == 12 || mount.Type == 44 || mount.Type == 49;
            if (velocity.Y == gravity && (!mount.Active || (!mount.Cart && mount.Type != 48 && !AllowStepdownWater)))
            {
                Collision.StepDown(ref position, ref velocity, width, height, ref stepSpeed, ref gfxOffY, (int)gravDir, waterWalk || waterWalk2);
            }
            if(gravDir == -1f)
            {
                if ((carpetFrame != -1 || velocity.Y <= gravity) && !controlUp)
                {
					Collision.StepUp(ref base.position, ref base.velocity, width, height, ref stepSpeed, ref gfxOffY, (int)gravDir, controlUp);
                }
            }
            else if ((carpetFrame != -1 || velocity.Y >= gravity) && !controlDown && !mount.Cart && !AllowStepdownWater && grappling[0] == -1)
            {
				Collision.StepUp(ref base.position, ref base.velocity, width, height, ref stepSpeed, ref gfxOffY, (int)gravDir, controlUp);
            }
            oldPosition = position;
            oldDirection = direction;
        }

        private void UpdateGraphicsOffset()
        {
            float gfxoffset = 1f + Math.Abs(velocity.Y) * 0.333f;
            if (gfxOffY > 0)
            {
                gfxOffY -= gfxoffset * stepSpeed;
                if(gfxOffY < 0) gfxOffY = 0;
            }
            else if (gfxOffY < 0)
            {
                gfxOffY += gfxoffset * stepSpeed;
                if(gfxOffY > 0) gfxOffY = 0;
            }
            if(gfxOffY > 32) gfxOffY = 32;
            if (gfxOffY < -32) gfxOffY = -32;
        }

        private void LiquidCollisionScript()
        {
            int LavaHurtHeight = height;
            if (waterWalk)
            {
                LavaHurtHeight -= 6;
            }
            bool LavaCollision = Collision.LavaCollision(position, width, LavaHurtHeight);
            if(LavaCollision)
            {
                if (!lavaImmune && IsLocalCompanion && hurtCooldowns[4] <= 0)
                {
                    if (lavaTime > 0)
                    {
                        lavaTime --;
                    }
                    else
                    {
                        int Damage = 80, DebuffTime = 420;
                        if (lavaRose)
                        {
                            Damage = 35;
                            DebuffTime = 210;
                        }
                        Hurt(PlayerDeathReason.ByOther(2), Damage, 0, cooldownCounter: 4);
                        AddBuff(24, DebuffTime);
                    }
                }
                lavaWet = true;
            }
            else
            {
                lavaWet = false;
                if (lavaTime < lavaMax)
                {
                    lavaTime ++;
                }
            }
            if(lavaTime > lavaMax) lavaTime = lavaMax;
            if(waterWalk2 && !waterWalk)
            {
                LavaHurtHeight -= 6;
            }
            bool WetCollision = Collision.WetCollision(position, width, height);
            bool IsHoney = Collision.honey;
            if(IsHoney)
            {
                AddBuff(48, 1800);
                honeyWet = true;
            }
            if(WetCollision)
            {
                if((onFire || onFire3) && !lavaWet)
                {
                    for(int i = 0; i < MaxBuffs; i++)
                    {
                        if(buffType[i] == 24 || buffType[i] == 323) DelBuff(i);
                    }
                }
                if (!wet)
                {
                    if (wetCount == 0)
                    {
                        wetCount = 10;
                        if (!LavaCollision)
                        {
                            if (honeyWet)
                            {
                                for (int i = 0; i < 20; i++)
                                {
                                    int d = Dust.NewDust(new Vector2(base.position.X - 6f, base.position.Y + height * 0.5f - 8f), width + 12, 24, 152);
                                    Main.dust[d].velocity.Y -= 1f;
                                    Main.dust[d].velocity.X *= 2.5f;
                                    Main.dust[d].scale = 1.3f;
                                    Main.dust[d].alpha = 100;
                                    Main.dust[d].noGravity = true;
                                }
                                SoundEngine.PlaySound(SoundID.Splash, position);
                            }
                            else
                            {
                                for (int i = 0; i < 50; i++)
                                {
                                    int d = Dust.NewDust(new Vector2(base.position.X - 6f, base.position.Y + height * 0.5f - 8f), width + 12, 24, Dust.dustWater());
                                    Main.dust[d].velocity.Y -= 3f;
                                    Main.dust[d].velocity.X *= 2.5f;
                                    Main.dust[d].scale = 0.8f;
                                    Main.dust[d].alpha = 100;
                                    Main.dust[d].noGravity = true;
                                }
                                SoundEngine.PlaySound(SoundID.Splash, position); //ends with 0?
                            }
                        }
                        else
                        {
                            for (int i = 0; i < 20; i++)
                            {
                                int d = Dust.NewDust(new Vector2(base.position.X - 6f, base.position.Y + height * 0.5f - 8f), width + 12, 24, 35);
                                Main.dust[d].velocity.Y -= 1.5f;
                                Main.dust[d].velocity.X *= 2.5f;
                                Main.dust[d].scale = 1.3f;
                                Main.dust[d].alpha = 100;
                                Main.dust[d].noGravity = true;
                            }
                            SoundEngine.PlaySound(SoundID.Splash, position);
                        }
                    }
                    wet = true;
                    if (ShouldFloatInWater)
                    {
                        velocity.Y *= 0.5f;
                        if(velocity.Y > 3) velocity.Y = 3;
                    }
                }
            }
            else if (wet)
            {
                wet = false;
                if (jump > jumpHeight * 0.2f && wetSlime == 0)
                {
                    jump = (int)(jumpHeight * 0.2f);
                }
                if (wetCount == 0)
                {
                    wetCount = 10;
                    if (!lavaWet)
					{
						if (honeyWet)
						{
							for (int i = 0; i < 20; i++)
							{
								int d = Dust.NewDust(new Vector2(base.position.X - 6f, base.position.Y + height * 0.5f - 8f), width + 12, 24, 152);
								Main.dust[d].velocity.Y -= 1f;
								Main.dust[d].velocity.X *= 2.5f;
								Main.dust[d].scale = 1.3f;
								Main.dust[d].alpha = 100;
								Main.dust[d].noGravity = true;
							}
                            SoundEngine.PlaySound(SoundID.Splash, position);
						}
						else
						{
							for (int i = 0; i < 50; i++)
							{
								int d = Dust.NewDust(new Vector2(base.position.X - 6f, base.position.Y + height * 0.5f), width + 12, 24, Dust.dustWater());
								Main.dust[d].velocity.Y -= 4f;
								Main.dust[d].velocity.X *= 2.5f;
								Main.dust[d].scale = 0.8f;
								Main.dust[d].alpha = 100;
								Main.dust[d].noGravity = true;
							}
                            SoundEngine.PlaySound(SoundID.Splash, position); //Ends with 0
						}
					}
					else
					{
						for (int i = 0; i < 20; i++)
						{
							int d = Dust.NewDust(new Vector2(base.position.X - 6f, base.position.Y + height * 0.5f - 8f), width + 12, 24, 35);
							Main.dust[d].velocity.Y -= 1.5f;
							Main.dust[d].velocity.X *= 2.5f;
							Main.dust[d].scale = 1.3f;
							Main.dust[d].alpha = 100;
							Main.dust[d].noGravity = true;
						}
                        SoundEngine.PlaySound(SoundID.Splash, position);
					}
                }
            }
            if (!wet)
            {
                lavaWet = honeyWet = false;
            }
            else if (!IsHoney) honeyWet = false;
            if (wetCount > 0) wetCount--;
            if (wetSlime > 0) wetSlime--;
            if (wet && mount.Active)
            {
                switch (mount.Type)
                {
                    case 5:
                    case 7:
                        if (IsLocalCompanion) mount.Dismount(this);
                        break;
                    case 3:
                    case 50:
                        wetSlime = 30;
                        if (velocity.Y > 2)
                            velocity.Y *= 0.9f;
                        velocity.Y -= 0.5f;
                        if(velocity.Y < -4f)
                            velocity.Y = -4f;
                        break;
                }
            }
        }

        private void LateControlUpdate()
        {
            if ((releaseRight = !controlRight))
            {
                rightTimer = 7;
            }
            if((releaseLeft = !controlLeft))
            {
                leftTimer = 7;
            }
            releaseDown = !controlDown;
            if(rightTimer > 0) rightTimer--;
            else if (controlRight)
                rightTimer = 7;
            if(leftTimer > 0) leftTimer --;
            else if (controlLeft) leftTimer = 7;
        }

        private void UpdateOtherMobility()
        {
            DashMovement();
            WallslideMovement();
            CarpetMovement();
            DoubleJumpVisuals();
            if(wingsLogic > 0 || mount.Active)
                sandStorm = false;
            if(velocity.Y != 0)
            {
                canRocket = (gravDir == 1 && velocity.Y > - jumpSpeed) || (gravDir == -1 && velocity.Y < jumpSpeed);
            }
            UpdateWings();
            UpdateTongued();
            if(IsLocalCompanion)
            {
                if(controlHook && releaseHook)
                {
                    QuickGrapple();
                }
                releaseHook = !controlHook;
            }
            UpdateCartDamage();
            Update_NPCCollision();
            UpdateDamageTilesCollision();
        }

        private void UpdateDamageTilesCollision()
        {
            Vector2 TakenInfo = ((mount.Active && mount.Cart) ? Collision.HurtTiles(GetCollisionPosition, velocity, defaultWidth, defaultHeight - 16, fireWalk) : Collision.HurtTiles(GetCollisionPosition, velocity, defaultWidth, defaultHeight, fireWalk));
            if(TakenInfo.Y == 0 && !fireWalk)
            {
                foreach (Point touchedTile in TouchedTiles)
                {
                    Tile tile = Main.tile[touchedTile.X, touchedTile.Y];
                    if (tile.HasTile && TileID.Sets.TouchDamageHot[tile.TileType] != 0)
                    {
                        TakenInfo.Y = TileID.Sets.TouchDamageHot[tile.TileType];
                        TakenInfo.X = ((!(Center.X * DivisionBy16 < touchedTile.X + 0.5f)) ? 1 : -1);
                    }
                }
            }
            if(TakenInfo.Y == 20)
            {
                AddBuff(67, 20);
            }
            else if (TakenInfo.Y == 15)
            {
                if(suffocateDelay < 5)
                    suffocateDelay++;
                else
                    AddBuff(68, 1);
            }
            else if (TakenInfo.Y != 0)
            {
                int Damage = Main.DamageVar(TakenInfo.Y, -luck);
                Hurt(PlayerDeathReason.ByOther(3), Damage, 0, cooldownCounter: 0);
                if(TakenInfo.Y == 60 || TakenInfo.Y == 80)
                {
                    AddBuff(30, Main.rand.Next(240, 600));
                }
            }
            else
            {
                suffocateDelay = 0;
            }
        }

        private void UpdateCartDamage() //Need work
        {
            if(!mount.Active || !mount.Cart || Math.Abs(velocity.X) <= 4)
                return;
        }

        private void UpdateTongued()
        {
            if(tongued)
            {
                StopVanityActions();
                bool RemoveTongue = false;
                if(Main.wofNPCIndex >= 0)
                {
                    Vector2 EndPosition = new Vector2(Main.npc[Main.wofNPCIndex].position.X + Main.npc[Main.wofNPCIndex].width * 0.5f + Main.npc[Main.wofNPCIndex].direction * 200, Main.npc[Main.wofNPCIndex].position.Y + Main.npc[Main.wofNPCIndex].height * 0.5f);
                    Vector2 Diference = EndPosition - Center;
                    float Length = Diference.Length();
                    const float MinDistance = 11f;
                    float MovementPercentage = Length;
                    if(Length > MinDistance)
                    {
                        MovementPercentage = MinDistance / Length;
                    }
                    else
                    {
                        MovementPercentage = 1;
                        RemoveTongue = true;
                    }
                    Diference *= MovementPercentage;
                    velocity = Diference;
                }
                else
                    RemoveTongue = true;
                if(RemoveTongue && IsLocalCompanion)
                {
                    for (int i = 0; i < MaxBuffs; i++)
                    {
                        if(buffType[i] == 38)
                            DelBuff(i);
                    }
                }
            }
            if (IsLocalCompanion)
                WOFTongue();
        }

        private void UpdateWings()
        {
            bool IsFlapping = false;
            if (((velocity.Y == 0 || sliding) && releaseJump) || (autoJump && justJumped))
            {
                mount.ResetFlightTime(velocity.X);
                wingTime = wingTimeMax;
            }
            if(wingsLogic > 0 && controlJump && wingTime > 0 && jump == 0 && velocity.Y != 0)
                IsFlapping = true;
            if((wingsLogic == 22 || wingsLogic == 28 || wingsLogic == 30 || wingsLogic == 32 || wingsLogic == 29 || wingsLogic == 33 || wingsLogic == 35 || wingsLogic == 37 || wingsLogic == 45) && controlJump && TryingToHoverDown && wingTime > 0)
                IsFlapping = true;
            if(frozen || webbed || stoned)
            {
                if(mount.Active)
                    mount.Dismount(this);
                velocity.Y += gravity;
                if(velocity.Y > maxFallSpeed)
                    velocity.Y = maxFallSpeed;
                sandStorm = false;
                CancelAllJumpVisualEffects();
            }
            else
            {
                bool IsCustomWings = ItemLoader.WingUpdate(this, IsFlapping);
                if(IsFlapping)
                {
                    //WingAirVisuals();
                    WingMovement();
                }
                WingFrame(IsFlapping, IsCustomWings);
                if(wingsLogic > 0 && rocketBoots != 0 && velocity.Y != 0 && rocketTime != 0)
                {
                    const int WingTimeBoost = 6;
                    int TimeIncrease = rocketTime * WingTimeBoost;
                    wingTime += TimeIncrease;
                    if(wingTime > wingTimeMax + TimeIncrease)
                        wingTime = wingTimeMax + TimeIncrease;
                    rocketTime = 0;
                }
                if(IsFlapping && wings != 0 && wings != 4 && wings != 22 && wings != 24 && wings != 28 && wings != 30 && wings != 33 && wings != 45 && !IsCustomWings)
                {
                    bool FlappyFrame = wingFrame == 3;
                    if (wings == 43 || wings == 44)
                    {
                        FlappyFrame = wingFrame == 4;
                    }
                    if (FlappyFrame)
                    {
                        if(!flapSound)
                        {
                            SoundEngine.PlaySound(SoundID.Item32, position);
                            flapSound = true;
                        }
                    }
                    else
                    {
                        flapSound = false;
                    }
                }
                if (velocity.Y == 0 || sliding || (autoJump && justJumped))
                {
                    rocketTime = rocketTimeMax;
                }
                if(empressBrooch)
                {
                    rocketTime = rocketTimeMax;
                }
                if((wingTime == 0 || wingsLogic == 0) && rocketBoots != 0 && controlJump && rocketDelay == 0 && canRocket && rocketRelease && !canJumpAgain_Cloud)
                {
                    if (rocketTime > 0)
                    {
                        rocketTime--;
                        rocketDelay = 10;
                        if(rocketDelay2 <= 0)
                        {
                            if(rocketBoots == 1)
                            {
                                rocketDelay2 = 30;
                            }
                            else if (rocketBoots == 2 || rocketBoots == 3 || rocketBoots == 4)
                            {
                                rocketDelay2 = 15;
                            }
                        }
                        if (rocketSoundDelay <= 0)
                        {
                            if(vanityRocketBoots == 1)
                            {
                                rocketSoundDelay = 30;
                                SoundEngine.PlaySound(SoundID.Item13, position);
                            }
                            else if(vanityRocketBoots >= 2 && vanityRocketBoots <= 4)
                            {
                                rocketSoundDelay = 15;
                                SoundEngine.PlaySound(SoundID.Item24, position);
                            }
                        }
                    }
                    else
                    {
                        canRocket = false;
                    }
                }
                if(rocketSoundDelay > 0)
                    rocketSoundDelay--;
                if(rocketDelay2 > 0) rocketDelay2 --;
                if(rocketDelay == 0)
                {
                    rocketFrame = false;
                }
                if(rocketDelay > 0)
                {
                    rocketFrame = true;
                    //RocketBootVisuals()
                    if (rocketDelay == 0)
                        releaseJump = true;
                    else
                        rocketDelay--;
                    velocity.Y -= 0.1f * gravDir;
                    if(velocity.Y * gravDir > 0)
                        velocity.Y -= 0.5f * gravDir;
                    else if(velocity.Y * gravDir > -jumpSpeed * 0.5f)
                        velocity.Y -= 0.1f * gravDir;
                    if(velocity.Y * gravDir < -jumpSpeed * 1.5f)
                        velocity.Y = -jumpSpeed * 1.5f * gravDir;
                }
                else if(!IsFlapping)
                {
                    if (mount.CanHover())
                    {
                        mount.Hover(this);
                    }
                    else if (mount.CanFly() && controlJump && jump == 0)
                    {
                        if(mount.Flight())
                        {
                            if(TryingToHoverDown)
                            {
                                velocity.Y *= 0.9f;
                                if(velocity.Y > -1 && velocity.Y < 0.5f)
                                    velocity.Y = 1E-05f;
                            }
                            else
                            {
                                float JumpSpeedVal = jumpSpeed;
                                if(mount.Type == 50)
                                    JumpSpeedVal *= 0.5f;
                                if(velocity.Y > 0)
                                    velocity.Y -= 0.5f;
                                else if(velocity.Y > -JumpSpeedVal * 1.5f)
                                    velocity.Y -= 0.1f;
                                if(velocity.Y < -JumpSpeedVal * 1.5f)
                                    velocity.Y = -JumpSpeedVal * 1.5f;
                            }
                        }
                        else
                        {
                            velocity.Y += gravity * 0.333f * gravDir;
                            if (velocity.Y * gravDir > maxFallSpeed * 0.333f && !TryingToHoverDown)
                            {
                                velocity.Y = maxFallSpeed * 0.333f * gravDir;
                            }
                        }
                    }
                    else if (slowFall && !TryingToHoverDown)
                    {
                        if(TryingToHoverUp)
                        {
                            gravity *= 0.1f;
                        }
                        else
                        {
                            gravity *= 0.333f;
                        }
                        velocity.Y += gravity;
                    }
                    else if (wingsLogic > 0 && controlJump && velocity.Y > 0)
                    {
                        bool noLightEmittence = wingsLogic != wings;
                        fallStart = (int)(position.Y * DivisionBy16);
                        if(velocity.Y > 0)
                        {
                            //Do wings logic scripts
                        }
                    }
                    else if (cartRampTime <= 0)
                    {
                        velocity.Y += gravity * gravDir;
                    }
                    else
                    {
                        cartRampTime --;
                    }
                }
                //if (!mount.Active || mount.Type != 5)
            }
        }

        private void UpdateJump()
        {
            if(MoveDown && controlJump)
                releaseJump = autoJump = false;
            JumpMovement();
            if(wingsLogic == 0) wingTime = 0;
            if(rocketBoots == 0) rocketTime = 0;
            if(jump == 0) CancelAllJumpVisualEffects();
            releaseUp = !controlUp;
        }

        private void UpdateRunSpeeds()
        {
            if(grappling[0] != -1 || tongued)
                return;
            //if(wingsLogic > 0 && velocity.Y != 0 && !merman && !mount.Active)
                //WingAirLogicTweaks();
            if(empressBlade) runAcceleration *= 2;
            if (hasMagiluminescence && base.velocity.Y == 0f)
				{
					runAcceleration *= 2f;
					maxRunSpeed *= 1.2f;
					accRunSpeed *= 1.2f;
					runSlowdown *= 2f;
				}
				if (mount.Active && mount.Type == 43 && base.velocity.Y != 0f)
				{
					runSlowdown = 0f;
				}
				if (sticky)
				{
					maxRunSpeed *= 0.25f;
					runAcceleration *= 0.25f;
					runSlowdown *= 2f;
					if (velocity.X > maxRunSpeed)
					{
						velocity.X = maxRunSpeed;
					}
					if (velocity.X < 0f - maxRunSpeed)
					{
						velocity.X = 0f - maxRunSpeed;
					}
				}
				else if (powerrun)
				{
					maxRunSpeed *= 3.5f;
					runAcceleration *= 1f;
					runSlowdown *= 2f;
				}
				else if (runningOnSand && desertBoots)
				{
					const float SpeedBonus = 1.75f;
					maxRunSpeed *= SpeedBonus;
					accRunSpeed *= SpeedBonus;
					runAcceleration *= SpeedBonus;
					runSlowdown *= SpeedBonus;
				}
				else if (slippy2)
				{
					runAcceleration *= 0.6f;
					runSlowdown = 0f;
					if (iceSkate)
					{
						runAcceleration *= 3.5f;
						maxRunSpeed *= 1.25f;
					}
				}
				else if (slippy)
				{
					runAcceleration *= 0.7f;
					if (iceSkate)
					{
						runAcceleration *= 3.5f;
						maxRunSpeed *= 1.25f;
					}
					else
					{
						runSlowdown *= 0.1f;
					}
				}
            if(sandStorm)
            {
                runAcceleration *= 1.5f;
                maxRunSpeed *= 2;
            }
            if (isPerformingJump_Blizzard && hasJumpOption_Blizzard)
				{
					runAcceleration *= 3f;
					maxRunSpeed *= 1.5f;
				}
				if (isPerformingJump_Fart && hasJumpOption_Fart)
				{
					runAcceleration *= 3f;
					maxRunSpeed *= 1.75f;
				}
				if (isPerformingJump_Unicorn && hasJumpOption_Unicorn)
				{
					runAcceleration *= 3f;
					maxRunSpeed *= 1.5f;
				}
				if (isPerformingJump_Santank && hasJumpOption_Santank)
				{
					runAcceleration *= 3f;
					maxRunSpeed *= 1.5f;
				}
				if (isPerformingJump_WallOfFleshGoat && hasJumpOption_WallOfFleshGoat)
				{
					runAcceleration *= 3f;
					maxRunSpeed *= 1.5f;
				}
				if (isPerformingJump_Basilisk && hasJumpOption_Basilisk)
				{
					runAcceleration *= 3f;
					maxRunSpeed *= 1.5f;
				}
				if (isPerformingJump_Sail && hasJumpOption_Sail)
				{
					runAcceleration *= 1.5f;
					maxRunSpeed *= 1.25f;
				}
				if (carpetFrame != -1)
				{
					runAcceleration *= 1.25f;
					maxRunSpeed *= 1.5f;
				}
            //if (inventory[selectedItem].type == 3106 && stealth < 1f)
            PlayerLoader.PostUpdateRunSpeeds(this);
            int BackedUpDirection = direction;
            HorizontalMovement();
            if(itemAnimation > 0)
                direction = BackedUpDirection;
            if (velocity.Y == 0)
            {
                AddSkillProgress(Math.Abs(velocity.X), CompanionSkillContainer.AthleticsID);
            }
            else
            {
                AddSkillProgress(Math.Abs(velocity.Y), CompanionSkillContainer.AcrobaticsID);
            }
        }

        private void UpdatePulley()
        {
            if(grapCount > 0)
                pulley = false;
            if(!pulley)
                return;
            if(mount.Active)
                pulley = false;
            sandStorm = false;
            CancelAllJumpVisualEffects();
            int TileX = (int)((position.X + width * 0.5f) * DivisionBy16),
                TileY = (int)((position.Y + height * 0.5f) * DivisionBy16);
            bool Moved = false;
            if(pulleyDir == 0)
                pulleyDir = 1;
            if(pulleyDir == 1)
            {
                if(direction == -1 && controlLeft && (releaseLeft || leftTimer == 0))
                {
                    pulleyDir = 2;
                    Moved = true;
                }
                else if((direction == 1 && controlRight && releaseRight) || rightTimer == 0)
                {
                    pulleyDir = 2;
                    Moved = true;
                }
                else
                {
                    if(direction == 1 && controlLeft)
                    {
                        direction = -1;
                        Moved = true;
                    }
                    if(direction == -1 && controlRight)
                    {
                        direction = 1;
                        Moved = true;
                    }
                }
            }
            else if(pulleyDir == 2)
            {
                if(direction == 1 && controlLeft)
                {
                    Moved = true;
                    if(!Collision.SolidCollision(new Vector2(TileX * 16 + 8 - width * 0.5f, position.Y), width, height))
                    {
                        pulleyDir = 1;
                        direction = -1;
                    }
                }
                if(direction == -1 && controlRight)
                {
                    Moved = true;
                    if(!Collision.SolidCollision(new Vector2(TileX * 16 + 8 - width * 0.5f, position.Y), width, height))
                    {
                        pulleyDir = 1;
                        direction = 1;
                    }
                }
            }
            int FaceDirection = controlLeft ? -1 : 1;
            bool CanMoveForward = CanMoveForwardOnRope(FaceDirection, TileX, TileY);
            if(CanMoveForward)
            {
                if(controlLeft && direction == -1)
                {
                    instantMovementAccumulatedThisFrame.X -= 1f;
                }
                if(controlRight && direction == 1)
                {
                    instantMovementAccumulatedThisFrame.X += 1;
                }
            }
            //continue another time.
        }

        private void GetPettingInfo(int animalNpcIndex, out int targetDirection, out Vector2 playerPositionWhenPetting, out bool isPetSmall)
        {
            //IL_000a: Unknown result type (might be due to invalid IL or missing references)
            //IL_0015: Unknown result type (might be due to invalid IL or missing references)
            //IL_006c: Unknown result type (might be due to invalid IL or missing references)
            //IL_007c: Unknown result type (might be due to invalid IL or missing references)
            //IL_0081: Unknown result type (might be due to invalid IL or missing references)
            //IL_0086: Unknown result type (might be due to invalid IL or missing references)
            NPC nPC = Main.npc[animalNpcIndex];
            targetDirection = ((nPC.Center.X > base.Center.X) ? 1 : (-1));
            isPetSmall = nPC.type == 637 || nPC.type == 656;
            int num = 36;
            switch (nPC.type)
            {
            case 637:
                num = 28;
                break;
            case 656:
                num = 24;
                break;
            }
            playerPositionWhenPetting = nPC.Bottom + new Vector2((float)(-targetDirection * num), 0f);
        }

        private bool CanMoveForwardOnRope(int dir, int x, int y)
        {
            //IL_00f1: Unknown result type (might be due to invalid IL or missing references)
            int num = x + dir;
            if (Main.tile[num, y] != null && Main.tile[num, y].HasTile && Main.tileRope[Main.tile[num, y].TileType])
            {
                int num2 = num * 16 + 8 - width / 2;
                float y2 = position.Y;
                y2 = y * 16 + 22;
                if ((!Main.tile[num, y - 1].HasTile || !Main.tileRope[Main.tile[num, y - 1].TileType]) && (!Main.tile[num, y + 1].HasTile || !Main.tileRope[Main.tile[num, y + 1].TileType]))
                {
                    y2 = y * 16 + 22;
                }
                if (Collision.SolidCollision(new Vector2((float)num2, y2), width, height))
                {
                    return false;
                }
                return true;
            }
            return false;
        }

        private void CancelAllJumpVisualEffects()
        {
            isPerformingJump_Cloud = false;
            isPerformingJump_Sandstorm = false;
            isPerformingJump_Blizzard = false;
            isPerformingJump_Fart = false;
            isPerformingJump_Sail = false;
            isPerformingJump_Unicorn = false;
            isPerformingJump_Santank = false;
        }

        private void UpdateInteractions()
        {
            UpdatePettingAnimal();
            InternalUpdateSitting();
			sleeping.UpdateState(this);
            if(furniturex > -1 && furniturey > -1 && reachedfurniture)
            {
                if (!sitting.isSitting && !sleeping.isSleeping)
                {
                    LeaveFurniture();
                }
                else
                {
                    UpdateFurniturePositioning();
                }
            }
			eyeHelper.Update(this);
        }

        private void InternalUpdateSitting()
        {
			//sitting.UpdateSitting(this);
            if(!sitting.isSitting) return;
            Point coords = (Bottom - Vector2.UnitY * 2).ToTileCoordinates();
            if(!PlayerSittingHelper.GetSittingTargetInfo(this, coords.X, coords.Y, out var TargetDirection, out var _, out var seatDownOffset))
            {
                sitting.SitUp(this, false);
                return;
            }
            if (controlLeft || controlRight || controlUp || controlDown || controlJump || pulley || mount.Active || TargetDirection != direction)
            {
                sitting.SitUp(this);
                return;
            }
            int MaxPlayers = this is TerraGuardian ? 3 : 2;
            if (Main.sittingManager.GetNextPlayerStackIndexInCoords(coords) >= MaxPlayers)
            {
                sitting.SitUp(this);
                return;
            }
            if (sitting.isSitting)
            {
                sitting.offsetForSeat = seatDownOffset;
                Main.sittingManager.AddPlayerAndGetItsStackedIndexInCoords(this.whoAmI, coords, out sitting.sittingIndex);
            }
        }

        protected virtual void UpdateFurniturePositioning()
        {

        }

        private void UpdateEquipments(bool Underwater)
        {
			head = armor[0].headSlot;
			body = armor[1].bodySlot;
			legs = armor[2].legSlot;
			ResetVisibleAccessories();
            if(MountFishronSpecialCounter > 0)
            {
                MountFishronSpecialCounter -= 1;
            }
            if(_portalPhysicsTime > 0) _portalPhysicsTime --;
            UpdateEquips(whoAmI);
            if (portableStoolInfo.HasAStool && controlUp && !gravControl && !mount.Active && velocity.X == 0 && velocity.Y == 0 && !pulley && grappling[0] == -1 && CanFitSpace(portableStoolInfo.HeightBoost))
            {
                portableStoolInfo.IsInUse = true;
                ResizeHitbox();
            }
            if(velocity.Y == 0 || controlJump) portalPhysicsFlag = false;
            if(inventory[selectedItem].type == 3384 || portalPhysicsFlag)
                _portalPhysicsTime = 30;
            if(mount.Active)
                mount.UpdateEffects(this);
            gemCount++;
            if(gemCount >= 10)
            {
                gem = -1;
                ownedLargeGems = 0;
                gemCount = 0;
                for(int i = 0; i <= 58; i++)
                {
                    if(inventory[i].type == 0 || inventory[i].stack == 0)
                    {
                        inventory[i].TurnToAir();
                    }
                    if(inventory[i].type >= 1522 && inventory[i].type <= 1527)
                    {
                        gem = inventory[i].type - 1522;
                        ownedLargeGems[gem] = true;
                    }
                    if(inventory[i].type == 3643)
                    {
                        gem = 6;
                        ownedLargeGems[gem] = true;
                    }
                }
            }
            UpdateArmorLights();
            UpdateArmorSets(whoAmI);
            PlayerLoader.PostUpdateEquips(this);
            if(maxTurretsOld != maxTurrets)
            {
                UpdateMaxTurrets();
                maxTurretsOld = maxTurrets;
            }
            if(shieldRaised)
            {
                statDefense += 20;
            }
            if((merman || forceMerman) && Underwater)
                wings = 0;
            if(invis)
            {
                if(itemAnimation == 0 && aggro > -750)
                    aggro = -750;
                else if(aggro > -250)
                    aggro = -250;
            }
            if(inventory[selectedItem].type == 3106)
            {
                if(itemAnimation == 0)
                {
                    stealthTimer = 15;
                    if(stealth > 0)
                    {
                        stealth += 0.1f;
                    }
                }
                else if(Math.Abs(velocity.X) < 0.1f && Math.Abs(velocity.Y) < 0.1f && !mount.Active)
                {
                    if(stealthTimer == 0 && stealth > 0f)
                    {
                        stealth -= 0.02f;
                        if(stealth <= 0)
                        {
                            stealth = 0;
                        }
                    }
                }
                else
                {
                    if(stealth > 0)
                    {
                        stealth += 0.1f;
                    }
                    if(mount.Active)
                    {
                        stealth = 1;
                    }
                }
                if(stealth > 1)
                    stealth = 1;
                GetDamage(DamageClass.Melee) += (1f - stealth) * 3;
                GetCritChance(DamageClass.Melee) += (int)((1f - stealth) * 30);
                GetKnockback(DamageClass.Melee) *= 2 - stealth;
                aggro -= (int)((1f - stealth) * 750);
                if(stealthTimer > 0)
                    stealthTimer --;
            }
            else if (shroomiteStealth)
            {
                if(itemAnimation > 0)
                {
                    stealthTimer = 5;
                }
                else if(Math.Abs(velocity.X) < 0.1f && Math.Abs(velocity.Y) < 0.1f && !mount.Active)
                {
                    if (stealthTimer == 0 && stealth > 0)
                    {
                        stealth -= 0.015f;
                        if(stealth <= 0)
                        {
                            stealth = 0;
                        }
                    }
                }
                else
                {
                    if(mount.Active)
                    {
                        stealth = 1;
                    }
                    else if(stealth < 1)
                    {
                        float MovementSum = Math.Abs(velocity.X) + Math.Abs(velocity.Y);
                        stealth += MovementSum * 0.0075f;
                        if(stealth > 1)
                        {
                            stealth = 1;
                        }
                    }
                }
                GetDamage(DamageClass.Ranged) += (1f - stealth) * 0.6f;
                GetCritChance(DamageClass.Ranged) += (int)((1f - stealth) * 10);
                GetKnockback(DamageClass.Ranged) *= 1f + (1f - stealth) * 0.5f;
                aggro -= (int)((1f - stealth) * 750f);
                if(stealthTimer > 0)
                {
                    stealthTimer--;
                }
            }
            else if(setVortex)
            {
                bool PlayVortexEffect = false;
                if(vortexStealthActive)
                {
                    stealth -= 0.04f;
                    if(stealth < 0)
                    {
                        stealth = 0;
                    }
                    else
                    {
                        PlayVortexEffect = true;
                    }
                    GetDamage(DamageClass.Ranged) += (1f - stealth) * 0.8f;
                    GetCritChance(DamageClass.Ranged) += (int)((1f - stealth) * 20);
                    GetKnockback(DamageClass.Ranged) *= 1f + (1f - stealth) * 0.5f;
                    aggro -= (int)((1f - stealth) * 1200);
                    accRunSpeed *= 0.3f;
                    maxRunSpeed *= 0.3f;
                    if(mount.Active)
                    {
                        vortexStealthActive = false;
                    }
                }
                else
                {
                    stealth += 0.04f;
                    if(stealth > 1)
                        stealth = 1;
                    else
                        PlayVortexEffect = true;
                }
                if(PlayVortexEffect)
                {
                    if(Main.rand.Next(2) == 0)
                    {
                        Vector2 DustMovement = Vector2.UnitY.RotatedByRandom(6.283185f);
                        Dust dust = Main.dust[Dust.NewDust(Center - DustMovement * 30, 0, 0, 229)];
                        dust.noGravity = true;
                        dust.position = Center - DustMovement * (float)Main.rand.Next(5, 11);
                        dust.velocity = DustMovement.RotatedBy(1.570796f) * 4f;
                        dust.scale = 0.5f + Main.rand.NextFloat();
                        dust.fadeIn = 0.5f;
                    }
                    if(Main.rand.Next(2) == 0)
                    {
                        Vector2 DustMovement = Vector2.UnitY.RotatedByRandom(6.283185f);
                        Dust dust = Main.dust[Dust.NewDust(Center - DustMovement * 30, 0, 0, 240)];
                        dust.noGravity = true;
                        dust.position = Center - DustMovement * 12f;
                        dust.velocity = DustMovement.RotatedBy(-1.570796f) * 42f;
                        dust.scale = 0.5f + Main.rand.NextFloat();
                        dust.fadeIn = 0.5f;
                    }
                }
            }
            else
            {
                stealth = 1;
            }
            if(manaSick)
            {
                GetDamage(DamageClass.Magic) *= 1f - manaSickReduction;
            }
            if(pickSpeed < 0.3f)
                pickSpeed = 0.3f;
            if(tileSpeed > 3)
                tileSpeed = 3;
            tileSpeed = 1f / tileSpeed;
            if(wallSpeed > 3)
                wallSpeed = 3;
            wallSpeed = 1f / wallSpeed;
            //Press F to pay respects to the max mana cap.
            if(statDefense < 0)
                statDefense = 0;
            if(slowOgreSpit)
            {
                moveSpeed *= 0.333f;
                if(velocity.Y == 0 && Math.Abs(velocity.X) > 1f)
                {
                    velocity.X *= 0.5f;
                }
            }
            else if (dazed)
            {
                moveSpeed *= 0.333f;
            }
            else if(slow)
            {
                moveSpeed *= 0.5f;
            }
            else if(chilled)
            {
                moveSpeed *= 0.75f;
            }
            if(shieldRaised)
            {
                moveSpeed *= 0.333f;
                if(velocity.Y == 0 && Math.Abs(velocity.X) > 3)
                    velocity.X *= 0.5f;
            }
            if(DD2Event.Ongoing)
            {
                DD2Event.FindArenaHitbox();
                if(DD2Event.ShouldBlockBuilding(Center))
                {
                    noBuilding = true;
                    AddBuff(199, 3);
                }
            }
            PlayerLoader.PostUpdateMiscEffects(this);
            UpdateLifeRegen();
            soulDrain = 0;
            UpdateManaRegen();
            if(manaRegenCount < 0) manaRegenCount = 0;
            if(statMana > statManaMax2)
            {
                statMana = statManaMax2;
            }
            runAcceleration *= moveSpeed;
            maxRunSpeed *= moveSpeed;
            UpdateJumpHeight();
            for(int i = 0; i < MaxBuffs; i++)
            {
                if(buffType[i] > 0 && buffTime[i] > 0 && buffImmune[i])
                    DelBuff(i);
            }
            if(brokenArmor) statDefense = (int)(statDefense * 0.5f);
            if(witheredArmor) statDefense = (int)(statDefense * 0.5f);
            if(witheredWeapon) GetDamage(DamageClass.Generic) *= 0.5f;
            lastTileRangeX = tileRangeX;
            lastTileRangeY = tileRangeY;
            if(mount.Active)
            {
                movementAbilitiesCache.CopyFrom(this);
            }
            else
            {
                movementAbilitiesCache.PasteInto(this);
            }
            if(mount.Active && mount.BlockExtraJumps)
            {
				canJumpAgain_Cloud = false;
				canJumpAgain_Sandstorm = false;
				canJumpAgain_Blizzard = false;
				canJumpAgain_Fart = false;
				canJumpAgain_Sail = false;
				canJumpAgain_Unicorn = false;
				canJumpAgain_Santank = false;
				canJumpAgain_WallOfFleshGoat = false;
				canJumpAgain_Basilisk = false;
            }
            else if (velocity.Y == 0 || sliding)
            {
                if (hasJumpOption_Cloud)
                {
                    canJumpAgain_Cloud = true;
                }
                if (hasJumpOption_Sandstorm)
                {
                    canJumpAgain_Sandstorm = true;
                }
                if (hasJumpOption_Blizzard)
                {
                    canJumpAgain_Blizzard = true;
                }
                if (hasJumpOption_Fart)
                {
                    canJumpAgain_Fart = true;
                }
                if (hasJumpOption_Sail)
                {
                    canJumpAgain_Sail = true;
                }
                if (hasJumpOption_Unicorn)
                {
                    canJumpAgain_Unicorn = true;
                }
                if (hasJumpOption_Santank)
                {
                    canJumpAgain_Santank = true;
                }
                if (hasJumpOption_WallOfFleshGoat)
                {
                    canJumpAgain_WallOfFleshGoat = true;
                }
                if (hasJumpOption_Basilisk)
                {
                    canJumpAgain_Basilisk = true;
                }
            }
            else
            {
				if (!hasJumpOption_Cloud)
				{
					canJumpAgain_Cloud = false;
				}
				if (!hasJumpOption_Sandstorm)
				{
					canJumpAgain_Sandstorm = false;
				}
				if (!hasJumpOption_Blizzard)
				{
					canJumpAgain_Blizzard = false;
				}
				if (!hasJumpOption_Fart)
				{
					canJumpAgain_Fart = false;
				}
				if (!hasJumpOption_Sail)
				{
					canJumpAgain_Sail = false;
				}
				if (!hasJumpOption_Unicorn)
				{
					canJumpAgain_Unicorn = false;
				}
				if (!hasJumpOption_Santank)
				{
					canJumpAgain_Santank = false;
				}
				if (!hasJumpOption_WallOfFleshGoat)
				{
					canJumpAgain_WallOfFleshGoat = false;
				}
				if (!hasJumpOption_Basilisk)
				{
					canJumpAgain_Basilisk = false;
				}
            }
            if(!carpet)
            {
                canCarpet = false;
                carpetFrame = -1;
            }
            else if(velocity.Y == 0 || sliding)
            {
                canCarpet = true;
                carpetTime = 0;
                carpetFrame = -1;
                carpetFrameCounter = 0;
            }
            if(gravDir == -1) canCarpet = false;
            if(ropeCount > 0) ropeCount--;
			if (!pulley && !frozen && !webbed && !stoned && !controlJump && gravDir == 1f && ropeCount == 0 && grappling[0] == -1 && !tongued && !mount.Active)
			{
				FindPulley();
			}
        }

    private void StopPettingAnimal()
	{
		isPettingAnimal = false;
		isTheAnimalBeingPetSmall = false;
	}

	private void UpdatePettingAnimal()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		if (!isPettingAnimal)
		{
			return;
		}
		if (talkNPC == -1)
		{
			StopPettingAnimal();
			return;
		}
		int num = Math.Sign(Main.npc[talkNPC].Center.X - base.Center.X);
		if (controlLeft || controlRight || controlUp || controlDown || controlJump || pulley || mount.Active || num != direction)
		{
			StopPettingAnimal();
			return;
		}
		GetPettingInfo(talkNPC, out var _, out var playerPositionWhenPetting, out var _);
		if (base.Bottom.Distance(playerPositionWhenPetting) > 2f)
		{
			StopPettingAnimal();
		}
	}

    private void UpdateBuffs(out bool Underwater)
    {
        AppliedFoodLevel = 0;
        PlayerLoader.PreUpdateBuffs(this);
        for (int num25 = 0; num25 < BuffLoader.BuffCount; num25++)
        {
            buffImmune[num25] = false;
        }
        UpdateBuffs(whoAmI);
        PlayerLoader.PostUpdateBuffs(this);
        if(IsLocalCompanion)
        {
            UpdatePet(whoAmI);
            UpdatePetLight(whoAmI);
        }
        if(kbBuff) GetKnockback(DamageClass.Generic) *= 1.5f;
        UpdateLuckFactors();
        RecalculateLuck();
        Underwater = wet && !lavaWet && (!mount.Active && !mount.IsConsideredASlimeMount);
        if(accMerman && Underwater)
        {
            releaseJump = true;
            wings = 0;
            merman = true;
            accFlipper = true;
            AddBuff(34, 2);
        }
        else
        {
            merman = false;
        }
        if(!Underwater && forceWerewolf)
            forceMerman = false;
        if(forceMerman && Underwater)
            wings = 0;
        accMerman = hideMerman = forceMerman = false;
        if(wolfAcc && !merman && !Main.dayTime && !wereWolf)
        {
            AddBuff(28, 60);
        }
        wolfAcc = false;
        hideWolf = false;
        forceWerewolf = false;
        IsSober = true;
        if(IsLocalCompanion)
        {
            for( int i = 0; i < MaxBuffs; i++)
            {
                if (buffType[i] > 0)
                {
                    if(buffTime[i] <= 0)
                    {
                        DelBuff(i);
                    }
                    else
                    {
                        switch(buffType[i])
                        {
                            case BuffID.WeaponImbueConfetti:
                            case BuffID.WeaponImbueCursedFlames:
                            case BuffID.WeaponImbueFire:
                            case BuffID.WeaponImbueGold:
                            case BuffID.WeaponImbueIchor:
                            case BuffID.WeaponImbueNanites:
                            case BuffID.WeaponImbuePoison:
                            case BuffID.WeaponImbueVenom:
                                HasWeaponEnchanted = true;
                                break;
                            case BuffID.WellFed:
                                AppliedFoodLevel = 1;
                                break;
                            case BuffID.WellFed2:
                                AppliedFoodLevel = 2;
                                break;
                            case BuffID.WellFed3:
                                AppliedFoodLevel = 3;
                                break;
                            case BuffID.Tipsy:
                                IsSober = false;
                                break;
                        }
                    }
                }
            }
            if(Owner == null && (hungry || starving))
            {
                if(hungry)
                {
                    DelBuff(FindBuffIndex(BuffID.Hunger));
                }
                if(starving)
                {
                    DelBuff(FindBuffIndex(BuffID.Starving));
                }
                //AddBuff(BuffID.WellFed, 5 * 60);
            }
        }
        IsHungry = AppliedFoodLevel == 0;
        beetleDefense = false;
        beetleOffense = false;
        setSolar = false;
    }

        private void UpdateTileTargetPosition()
        {
            tileTargetX = Math.Clamp((int)((Center.X + GetAimedPosition.X) * DivisionBy16), 5, Main.maxTilesX - 5);
            tileTargetY = Math.Clamp((int)((Center.Y + GetAimedPosition.Y) * DivisionBy16), 5, Main.maxTilesY - 5);
            /*for(sbyte i = -1; i < 2; i++)
            {
                if(Main.tile[tileTargetX + i, tileTargetY] == null)
                    Main.tile[tileTargetX + i, tileTargetY] = default(Tile); //Is readonly, for some reason.
            }*/
            
        }

        private void UpdateTimers()
        {
            if(emoteTime > 0) emoteTime--;
            if(ghostDmg > 0) 
            {
                ghostDmg -= 6.66666651f;
                if(ghostDmg < 0) ghostDmg = 0;
            }
            if (Main.expertMode)
			{
				if (lifeSteal < 70f)
				{
					lifeSteal += 0.5f;
				}
				if (lifeSteal > 70f)
				{
					lifeSteal = 70f;
				}
			}
			else
			{
				if (lifeSteal < 80f)
				{
					lifeSteal += 0.6f;
				}
				if (lifeSteal > 80f)
				{
					lifeSteal = 80f;
				}
			}
            if(runSoundDelay > 0) runSoundDelay--;
            if(itemAnimation == 0) attackCD = 0;
            else if(attackCD > 0) attackCD--;
            if(potionDelay > 0) potionDelay--;
            if(petalTimer > 0) petalTimer--;
            if(shadowDodgeTimer > 0) shadowDodgeTimer--;
            if(yoraiz0rEye > 0) Yoraiz0rEye();
        }

        private float UpdateSpaceGravity()
        {
            float WorldSizeX = Main.maxTilesX * (1f / 4200);
            WorldSizeX *= WorldSizeX;
            float SpaceGravity = (float)((position.Y * DivisionBy16 - (60 + 10 * WorldSizeX)) / (Main.worldSurface * (1f / 6)));
            if(SpaceGravity < 0.25f)
                SpaceGravity = 0.25f;
            if(SpaceGravity > 1)
                SpaceGravity = 1;
            gravity *= SpaceGravity;
            return SpaceGravity;
        }

        private void UpdateFallDamage(float SpaceGravity)
        {
            if(!IsLocalCompanion)
                return;
            if(velocity.Y <= 0) fallStart2 = (int)(position.Y * DivisionBy16);
            bool ResetFallDistance = jump > 0 || rocketDelay > 0 || wet || slowFall || SpaceGravity < 0.8f || tongued; //Need to add space gravity here.
            if(velocity.Y == 0)
            {
                int FallDamageDistance = 0;
                int Tolerance = GetFallTolerance;
                if(!(mount.CanFly() || (mount.Cart && Minecart.OnTrack(position, width, height)) || mount.Type == 1))
                {
                    FallDamageDistance = (int)(position.Y * DivisionBy16) - fallStart;
                }
                if((gravDir == 1 && FallDamageDistance > 0) || (gravDir == -1 && FallDamageDistance < 0))
                {
                    int xstart = (int)(position.X * DivisionBy16), xend = (int)((position.X + width) * DivisionBy16),
                        ypos = (int)(gravDir == 1 ? (position.Y + height + 1f) * DivisionBy16 : (position.Y - 1f) * DivisionBy16);
                    for(int x = xstart; x <= xend; x++)
                    {
                        Tile tile = Main.tile[x, ypos];
                        if(tile != null && tile.HasTile && 
                            (tile.TileType == 189 || tile.TileType == 196 || tile.TileType == 460))
                            {
                                FallDamageDistance = 0;
                                break;
                            }
                    }
                }
                if(stoned)
                {
                    int DamageValue = (int)((FallDamageDistance * gravDir - 2) * 20);
                    if(DamageValue > 0)
                    {
                        Hurt(PlayerDeathReason.ByOther(5), DamageValue, 0);
                        immune = false;
                    }
                }
                else if(!noFallDmg && equippedWings == null && FallDamageDistance * gravDir > Tolerance)
                {
                    immune = false;
                    int DamageValue = (int)((float)FallDamageDistance * gravDir - Tolerance) * 10;
                    if(mount.Active)
                    {
                        DamageValue = (int)(DamageValue * mount.FallDamage);
                    }
                    Hurt(PlayerDeathReason.ByOther(0), DamageValue, 0);
                }
                ResetFallDistance = true;
            }
            if(ResetFallDistance) 
            {
                fallStart = (int)(position.Y * DivisionBy16);
            }
        }

        public virtual void UpdateAnimations()
        {
            PlayerFrame();
            BodyFrameID = (short)(legFrame.Y * (1f / 56));
            short ArmFrame = (short)(bodyFrame.Y * (1f / 56));
            for(int i = 0; i < ArmFramesID.Length; i++) ArmFramesID[i] = ArmFrame;
            Base.ModifyAnimation(this);
        }

        private void ResizeHitbox(bool Collision = false)
        {
            position.X += (int)(width * 0.5f);
            width = (Collision ? 20 : (int)(Base.Width * Scale));
            position.X -= (int)(width * 0.5f);
            position.Y += height;
            height = (Collision ? 42 : Crouching ? (int)(Base.CrouchingHeight * Scale) : (int)(Base.Height * Scale)) + HeightOffsetBoost;
            position.Y -= height;
        }

        private void ResetMobilityStatus()
        {
            if(PortalPhysicsEnabled)
                maxFallSpeed = 35f;
            else
                maxFallSpeed = Base.MaxFallSpeed;
            gravity = Base.Gravity;
            jumpHeight = Base.JumpHeight;
            jumpSpeed = Base.JumpSpeed;
            maxRunSpeed = accRunSpeed = Base.MaxRunSpeed;
            runAcceleration = Base.RunAcceleration;
            runSlowdown = Base.RunDeceleration;
        }

        private void LiquidMovementHindering()
        {
            if(wet) //Default Gravity is 0.4f;
            {
                if(honeyWet)
                {
                    gravity *= 0.25f; //0.1f...
                    maxFallSpeed *= 3f / 10; //3f...
                }
                else if (merman)
                {
                    gravity *= 0.75f; //0.3f...
                    maxFallSpeed *= 0.7f; //7f...
                }
                else if(trident && !lavaWet)
                {
                    if(MoveUp)
                    {
                        gravity *= 0.25f; //0.1f
                        maxFallSpeed *= 0.2f; //2f
                    }
                    else
                    {
                        gravity *= 0.625f; //0.25f
                        maxFallSpeed *= 0.6f; //6f
                    }
                    jumpHeight += 10; //25
                    jumpSpeed += 0.5f; //5.51f
                }
                else
                {
                    gravity *= 0.5f; //0.2f
                    maxFallSpeed *= 0.5f; //5
                    jumpHeight *= 2; //30
                    jumpSpeed += 1; //6.01f
                }
            }
        }

        private void ResetControls()
        {
            MoveLeft = MoveRight = MoveUp = MoveDown = ControlJump = ControlAction = false;
            if(Base.CanCrouch && !releaseDown && itemAnimation > 0)
                MoveDown = true;
        }
    }
}