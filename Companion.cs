using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class Companion : Player
    {
        public const float DivisionBy16 = 1f / 16;

        public CompanionBase Base
        {
            get
            {
                return Data.Base;
            }
        }
        private CompanionData _data = new CompanionData();
        public CompanionData Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        public uint ID { get { return Data.ID; } }
        public string ModID { get { return Data.ModID; } }
        public bool IsPlayerCharacter = false;
        public int Owner = -1;
        #region Useful getter setters
        public bool MoveLeft{get{ return controlLeft;} set{ controlLeft = value; }}
        public bool LastMoveLeft{get{ return releaseLeft;} set{ releaseRight = value; }}
        public bool MoveRight{get{ return controlRight;} set{ controlRight = value; }}
        public bool LastMoveRight{get{ return releaseRight;} set{ releaseRight = value; }}
        public bool MoveUp{get{ return controlUp;} set{ controlUp = value; }}
        public bool LastMoveUp{get{ return releaseUp;} set{ releaseUp = value; }}
        public bool MoveDown{get{ return controlDown;} set{ controlDown = value; }}
        public bool LastMoveDown{get{ return releaseDown;} set{ releaseDown = value; }}
        public bool ControlJump{get{ return controlJump;} set{ controlJump = value; }}
        public bool LastControlJump{get{ return releaseJump;} set{ releaseJump = value; }}
        public bool ControlAction { get{ return controlUseItem; } set { controlUseItem = value; }}
        public bool LastControlAction { get{ return releaseUseItem; } set { releaseUseItem = value; }}
        #endregion
        public Vector2 AimPosition = Vector2.Zero;

        public bool IsLocalCompanion
        {
            get
            {
                return Main.netMode == 0 || (Main.netMode == 1 && Owner == Main.myPlayer) || (Main.netMode == 2 && Owner == -1);
            }
        }

        public bool IsPlayerID(int ID)
        {
            return IsPlayerCharacter && ID == whoAmI;
        }
        
        public void UpdateCompanion()
        {
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
            if(IsLocalCompanion)
            {
                TryPortalJumping();
                doorHelper.Update(this);
            }
            UpdateFallDamage(SpaceGravity);
            UpdateTileTargetPosition();
            UpdateImmunity();
            ResetEffects();
            UpdateDyes();
            bool UnderwaterFlag;
            UpdateBuffs(out UnderwaterFlag);
            UpdateEquipments(UnderwaterFlag);
            UpdateInteractions();
            UpdatePulley();
        }

        private void UpdatePulley()
        {
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
            //bool CanMoveForward = //CanMoveForwardOnRole is also a private method...

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
			sitting.UpdateSitting(this);
			sleeping.UpdateState(this);
			eyeHelper.Update(this);
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
            if(IsLocalCompanion)
            {
                for( int i = 0; i < MaxBuffs; i++)
                {
                    if(buffType[i] > 0 && buffTime[i] <= 0)
                    {
                        DelBuff(i);
                    }
                }
            }
			beetleDefense = false;
			beetleOffense = false;
			setSolar = false;
        }

        private void UpdateTileTargetPosition()
        {
            tileTargetX = Math.Clamp((int)((Center.X + AimPosition.X) * DivisionBy16), 5, Main.maxTilesX - 5);
            tileTargetY = Math.Clamp((int)((Center.Y + AimPosition.Y) * DivisionBy16), 5, Main.maxTilesY - 5);
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
            if(velocity.Y == 0 && oldVelocity.Y != 0)
            {
                int FallDamageDistance = 0;
                int Tolerance = Base.FallHeightTolerance + extraFall;
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
                fallStart = (int)(position.Y * DivisionBy16);
        }

        protected virtual void UpdateAnimations()
        {
            PlayerFrame();
        }

        private void ResizeHitbox(bool Collision = false)
        {
            position.Y += height;
            height = (Collision ? 42 : Base.Height) + HeightOffsetBoost;
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
            maxRunSpeed = accRunSpeed = Base.RunSpeed;
            runAcceleration = Base.RunAcceleration;
            runSlowdown = base.runSlowdown;
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
            LastMoveLeft = MoveLeft;
            LastMoveRight = MoveRight;
            LastMoveUp = MoveUp;
            LastMoveDown = MoveDown;
            LastControlJump = ControlJump;
            LastControlAction = ControlAction;
            MoveLeft = MoveRight = MoveUp = MoveDown = ControlJump = ControlAction = false;
        }
    }
}