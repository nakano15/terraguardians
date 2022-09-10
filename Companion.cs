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
        public CompanionData Data{
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
        
        new public void Update(int i)
        {
            if(IsPlayerID(Main.myPlayer) && Main.netMode < 2)
                Terraria.GameInput.LockOnHelper.Update();
            if((IsPlayerID(Main.myPlayer) || IsLocalCompanion) && Main.dontStarveWorld)
            {
                Terraria.GameContent.DontStarveDarknessDamageDealer.Update(this);
            }
            ResetStatus();
            UpdateTimers();
            ResizeHitbox();
            if(IsLocalCompanion)
            {
                if(Terraria.GameContent.Events.DD2Event.DownedInvasionAnyDifficulty)
                {
                    downedDD2EventAnyDifficulty = true;
                }
            }
            if(mount.Active && mount.Type == 0)
            {
                int lightX = (int)((position.X + width * 0.5f) * DivisionBy16);
                int lightY = (int)((position.Y + height * 0.5f - 14) * DivisionBy16);
                Lighting.AddLight(lightX, lightY, 0.5f, 0.2f, 0.05f);
                Lighting.AddLight(lightX + direction, lightY, 0.5f, 0.2f, 0.05f);
                Lighting.AddLight(lightX + direction * 2, lightY, 0.5f, 0.2f, 0.05f);
            }
            outOfRange = false;
            if((!IsPlayerID(Main.myPlayer) || IsLocalCompanion) && Main.netMode == 1)
            {
                int TileX = (int)((position.X + width * 0.5f) * DivisionBy16);
                int TileY = (int)((position.Y + height * 0.5f) * DivisionBy16);
                if(!Main.sectionManager.TilesLoaded(TileX - 3, TileY - 3, TileX + 3, TileY + 3))
                {
                    outOfRange = true;
                }
                if(outOfRange)
                {
                    numMinions = 0;
                    slotsMinions = 0;
                    itemAnimation = 0;
                    UpdateBuffs(whoAmI);
                    UpdateAnimations();
                }
            }
            if(tankPet >= 0)
            {
                if(tankPetReset) tankPet = -1;
                else tankPetReset = true;
            }
            IsVoidVaultEnabled = (IsPlayerID(Main.myPlayer) || IsLocalCompanion) && HasItem(4131);
            if(chatOverhead.timeLeft > 0) chatOverhead.timeLeft--;
            if(snowBallLauncherInteractionCooldown > 0) snowBallLauncherInteractionCooldown--;
            if(environmentBuffImmunityTimer > 0)environmentBuffImmunityTimer --;
            if(outOfRange) return;
            UpdateHairDyeDust();
            UpdateMiscCounter();
            PlayerLoader.PreUpdate(this);
            UpdateGravity();
            UpdateManaRegenDelay();
            UpdateSocialShadow();
            UpdateTeleportVisuals();
            if(IsPlayerID(Main.myPlayer) || IsLocalCompanion)
            {
                /*if(!DD2Event.Ongoing)
                {
                   //Remove dd2 crystals. 
                }*/
                TryPortalJumping();
                doorHelper.Update(this);
                UpdateMinionTarget();
            }
            if (ghost)
            {
                Ghost();
                return;
            }
            if(dead)
            {
                UpdateDead();
                return;
            }
            if(velocity.Y == 0) mount.FatigueRecovery();
            ResetControls();

            PostControlsHandler();
        }

        private void PostControlsHandler()
        {
            if (controlLeft && controlRight)
			{
				controlLeft = false;
				controlRight = false;
            }
            if (controlQuickHeal)
			{
				if (releaseQuickHeal)
				{
					QuickHeal();
				}
				releaseQuickHeal = false;
			}
			else
			{
				releaseQuickHeal = true;
			}
			if (controlQuickMana)
			{
				if (releaseQuickMana)
				{
					QuickMana();
				}
				releaseQuickMana = false;
			}
			else
			{
				releaseQuickMana = true;
			}
			if (controlCreativeMenu)
			{
				if (releaseCreativeMenu)
				{
					ToggleCreativeMenu();
				}
				releaseCreativeMenu = false;
			}
			else
			{
				releaseCreativeMenu = true;
			}
            if (controlSmart)
			{
				releaseSmart = false;
			}
			else
			{
				releaseSmart = true;
			}
			if (controlMount)
			{
				if (releaseMount)
				{
					QuickMount();
				}
				releaseMount = false;
			}
			else
			{
				releaseMount = true;
			}
            if(confused)
            {
                bool lastleft = controlLeft, lastup = controlUp;
                controlLeft = controlRight;
                controlRight = lastleft;
                controlUp = controlRight;
                controlDown = lastup;
            }
            else if(cartFlip)
            {
                if(controlRight || controlLeft)
                {
                    bool LastLeft = controlLeft;
                    controlLeft = controlRight;
                    controlRight = LastLeft;
                }
                else
                {
                    cartFlip = false;
                }
            }
            for(int i = 0; i < doubleTapCardinalTimer.Length; i++)
            {
                if(doubleTapCardinalTimer[i] > 0)doubleTapCardinalTimer[i]--;
            }
            for(int m = 0; m < 4; m++)
            {
                bool PressedKey = false, HeldKey = false;
                switch(m)
                {
                    case 0:
                        HeldKey = controlDown;
                        PressedKey = HeldKey && releaseDown;
                        break;
                    case 1:
                        HeldKey = controlUp;
                        PressedKey = HeldKey && releaseUp;
                        break;
                    case 2:
                        HeldKey = controlRight;
                        PressedKey = HeldKey && releaseRight;
                        break;
                    case 3:
                        HeldKey = controlLeft;
                        PressedKey = HeldKey && releaseLeft;
                        break;
                }
                if(PressedKey)
                {
                    if (doubleTapCardinalTimer[m] > 0) KeyDoubleTap(m);
                    else doubleTapCardinalTimer[m] = 15;
                }
                if(HeldKey)
                {
                    holdDownCardinalTimer[m]++;
                    KeyHoldDown(m, holdDownCardinalTimer[m]);
                }
                else
                {
                    holdDownCardinalTimer[m] = 0;
                }
            }
            
        }

        private void ResetControls()
        {
			controlUp = false;
			controlLeft = false;
			controlDown = false;
			controlRight = false;
			controlJump = false;
			controlUseItem = false;
			controlUseTile = false;
			controlThrow = false;
			controlHook = false;
			controlTorch = false;
			controlSmart = false;
			controlMount = false;
        }

        private void UpdateManaRegenDelay()
        {
            maxRegenDelay = ((1f - (float)statMana / statManaMax2) * 240f + 45) * 0.7f;
        }

        private void UpdateGravity()
        {
            float gravitydensity = Main.maxTilesX * (1f / 4200);
            gravitydensity *= gravitydensity;
            float GravityPower = (float)((double)(position.Y * DivisionBy16 - (60 + 10 * gravitydensity) / (Main.worldSurface * (1d / 6))));
            if(GravityPower < 0.25f)
                GravityPower = 0.25f;
            else if(GravityPower > 1) GravityPower = 1;
            gravity *= GravityPower;
        }

        private void UpdateTimers()
        {
            if(emoteTime > 0) emoteTime--;
            if(ghostDmg > 0)
            {
                ghostDmg -= 6.66666651f;
                if(ghostDmg < 0) ghostDmg = 0;
            }
            if(Main.expertMode)
            {
                const int MaxlifeSteal = 70;
                if(lifeSteal < MaxlifeSteal)
                {
                    lifeSteal += 0.5f;
                    if(lifeSteal > MaxlifeSteal) lifeSteal = MaxlifeSteal;
                }
            }
            else
            {
                const int MaxlifeSteal = 80;
                if(lifeSteal < MaxlifeSteal)
                {
                    lifeSteal += 0.6f;
                    if(lifeSteal > MaxlifeSteal) lifeSteal = MaxlifeSteal;
                }
            }
            infernoCounter++;
            if(infernoCounter >= 180) infernoCounter = 0;
            if(timeSinceLastDashStarted < 300)
                timeSinceLastDashStarted++;
            if(_framesLeftEligibleForDeadmansChestDeathAchievement > 0)
                _framesLeftEligibleForDeadmansChestDeathAchievement--;
            if(starCloakCooldown > 0)
            {
                starCloakCooldown--;
                if(Main.rand.Next(5) == 0)
                {
                    for(byte k = 0; k < 2; k++)
                    {
                        Dust dust = Dust.NewDustDirect(position, width, height, 45, 0, 0,, 255, default(Color), (float)Main.rand.Next(20, 26) * 0.1f);
                        dust.noLight = true;
                        dust.noGravity = true;
                        dust.velocity *= 0.5f;
                        dust.velocity.X = 0;
                        dust.velocity.Y -= 0.5f;
                    }
                }
                //if(starCloakCooldown == 0)
                //    SoundEngine.PlaySound(25); //In the source it's int, here it's something else.
            }
            if(runSoundDelay > 0) runSoundDelay--;
            if(itemAnimation > 0) attackCD = 0;
            else if(attackCD > 0) attackCD--;
            if(potionDelay > 0) potionDelay --;
        }

        public void ResizeHitbox(bool Collision = false)
        {
            position.Y += height;
            height = (Collision ? 42 :  Base.Height) + HeightOffsetBoost;
            position.Y -= height;
        }

        private void ResetStatus()
        {
            ResetEffects();
            maxFallSpeed = 10f;
            gravity = defaultGravity;
            jumpHeight = 15;
            jumpSpeed = 5.01f;
            maxRunSpeed = 3f;
            runAcceleration = 0.08f;
            runSlowdown = 0.2f;
            accRunSpeed = maxRunSpeed;
            if(!mount.Active || !mount.Cart) onWrongGround = false;
            heldProj = -1;
            instantMovementAccumulatedThisFrame = Vector2.Zero;
            if (PortalPhysicsEnabled) maxFallSpeed = 35f;
            if(wet)
            {
                if(honeyWet)
                {
                    gravity = 0.1f;
                    maxFallSpeed = 3f;
                }
                else if (merman)
                {
                    gravity = 0.3f;
                    maxFallSpeed = 7f;
                }
                else if(trident && !lavaWet)
                {
                    gravity = 0.25f;
                    maxFallSpeed = 6f;
                    jumpHeight = 25;
                    jumpSpeed = 5.51f;
                    if(controlUp)
                    {
                        gravity = 0.1f;
                        maxFallSpeed = 2;
                    }
                }
                else
                {
                    gravity = 0.2f;
                    maxFallSpeed = 5;
                    jumpHeight = 30;
                    jumpSpeed = 6.01f;
                }
            }
            if(vortexDebuff)
            {
                gravity = 0;
            }
            maxFallSpeed += 0.01f;
        }
    
        private void UpdateAnimations()
        {

        }
    }
}