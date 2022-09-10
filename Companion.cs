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
            UpdateBuffs();
        }

        private void UpdateBuffs()
        {
            PlayerLoader.PreUpdateBuffs(this);
			for (int num25 = 0; num25 < BuffLoader.BuffCount; num25++)
			{
				buffImmune[num25] = false;
			}
            //UpdateBuffs(whoAmI);
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