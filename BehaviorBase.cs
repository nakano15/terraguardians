using System;
using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class BehaviorBase
    {
        #region Permissions
        private BitsByte _permissionset1 = 0;

        public virtual bool AllowDespawning { get { return true; } }
        public bool UseHealingItems { get { return !_permissionset1[0]; } set { _permissionset1[0] = !value; }}
        public bool RunCombatBehavior { get { return !_permissionset1[1]; } set { _permissionset1[1] = !value; } }
        public bool AllowSeekingTargets { get { return !_permissionset1[2]; } set { _permissionset1[2] = !value; } }
        public bool CanBeAttacked { get { return !_permissionset1[3]; } set { _permissionset1[3] = !value; } }
        public bool CanBeHurtByNpcs { get { return !_permissionset1[4]; } set { _permissionset1[4] = !value; } }
        public bool CanTargetNpcs { get { return !_permissionset1[5]; } set { _permissionset1[5] = !value; } }
        public bool IsVisible { get { return !_permissionset1[6]; } set { _permissionset1[6] = !value; } }
        #endregion
        private Companion Owner;
        public Companion GetOwner{ get { return Owner; } }
        private bool Active = true;
        public bool IsActive { get { return Active; } }

        public void Deactivate()
        {
            Active = false;
            OnEnd();
        }

        public BehaviorBase()
        {

        }

        public BehaviorBase(Companion owner)
        {
            Owner = owner;
        }

        public void SetOwner(Companion owner)
        {
            Owner = owner;
        }

        #region Hooks
        public virtual void Update(Companion companion)
        {
            
        }

        public virtual string CompanionNameChange(Companion companion)
        {
            return companion.name;
        }

        public virtual MessageBase ChangeStartDialogue(Companion companion)
        {
            return null;
        }

        public virtual bool AllowStartingDialogue(Companion companion)
        {
            return true;
        }

        public virtual void ChangeLobbyDialogueOptions(MessageBase Message, out bool ShowCloseButton)
        {
            ShowCloseButton = true;
        }

        public virtual void UpdateAnimationFrame(Companion companion)
        {

        }
        
        public virtual void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public virtual void CompanionDrawLayerSetup(Companion companion, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {

        }

        public virtual void UpdateStatus(Companion companion)
        {
            
        }

        public virtual bool IsHostileTo(Player target)
        {
            return false;
        }

        public virtual bool CanKill(Companion companion)
        {
            return true;
        }
        #endregion

        public void MoveTowardsDirection(Companion companion, int direction = 0)
        {
            if (direction == 0)
                direction = companion.direction;
            int CheckAheadX = (int)((companion.Center.X + companion.SpriteWidth * 0.6f * direction) * (1f / 16));
            int CheckAheadY = (int)((companion.position.Y + companion.height) * (1f / 16));
            byte GapHeight = 0;
            bool DangerAhead = false, GroundAhead = false;
            byte HoleHeight = 0;
            byte WaterHeight = 0;
            for(int y = 0; y < 8; y++)
            {
                if (WorldGen.InWorld(CheckAheadX, CheckAheadY - y))
                {
                    Tile tile = Main.tile[CheckAheadX, CheckAheadY - y];
                    if(tile.HasTile && !tile.IsActuated)
                    {
                        switch(tile.TileType)
                        {
                            case TileID.ClosedDoor:
                            case TileID.TallGateClosed:
                                GapHeight++;
                                break;
                            default:
                                if (Main.tileSolid[tile.TileType])
                                {
                                    if (GapHeight < 3) GapHeight = 0;
                                }
                                else
                                {
                                    GapHeight ++;
                                }
                                break;
                        }
                        if(GapHeight < 3)
                        {
                            switch (tile.TileType)
                            {
                                case TileID.Spikes:
                                case TileID.WoodenSpikes:
                                case TileID.PressurePlates:
                                    DangerAhead = true;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        GapHeight ++;
                    }
                }
                if(!GroundAhead && WorldGen.InWorld(CheckAheadX, CheckAheadY + y))
                {
                    Tile tile = Main.tile[CheckAheadX, CheckAheadY + y];
                    if(tile.HasTile && !tile.IsActuated)
                    {
                        switch (tile.TileType)
                        {
                            case TileID.Spikes:
                            case TileID.WoodenSpikes:
                            case TileID.PressurePlates:
                                DangerAhead = true;
                                break;
                        }
                        if(!Main.tileSolid[tile.TileType] && tile.LiquidAmount > 50 && ((tile.LiquidType == LiquidID.Lava) || 
                        (tile.LiquidType == LiquidID.Water && WaterHeight++ * 16 >= companion.height - 8 && !companion.HasWaterWalkingAbility && !companion.HasWaterbreathingAbility)))
                        {
                            DangerAhead = true;
                        }
                        if (Main.tileSolid[tile.TileType])
                        {
                            if (HoleHeight < 4)
                            {
                                HoleHeight = 0;
                                GroundAhead = true;
                            }
                        }
                        else
                        {
                            HoleHeight++;
                        }
                    }
                    else
                    {
                        HoleHeight++;
                    }
                }
            }
            if (DangerAhead || GapHeight < 3 || HoleHeight >= 4)
            {
                direction *= -1;
            }
            if(direction > 0)
                companion.MoveRight = true;
            else
                companion.MoveLeft = true;
        }

        public Player ViewRangeCheck(Companion companion, int Direction, int DistanceX = 300, int DistanceY = 150, bool SpotPlayers = true, bool SpotCompanions = false)
        {
            Player Nearest = null;
            float NearestDistance = float.MaxValue;
            Rectangle rect = new Rectangle((int)companion.Center.X, (int)companion.Center.Y, DistanceX, DistanceY);
            rect.Y -= (int)(rect.Height * 0.5f);
            if(Direction < 0) rect.X -= rect.Width;
            for(int p = 0; p < 255; p++)
            {
                if(Main.player[p].active && !Main.player[p].dead && Main.player[p] != companion)
                {
                    if(Main.player[p] is Companion)
                    {
                        if(!SpotCompanions || (Main.player[p] as Companion).Owner == null) continue;
                    }
                    else
                    {
                        if (!SpotPlayers) continue;
                    }
                    if(rect.Intersects(Main.player[p].getRect()))
                    {
                        float Distance = (Main.player[p].Center - companion.Center).Length();
                        if(Distance < NearestDistance)
                        {
                            Nearest = Main.player[p];
                            NearestDistance = Distance;
                        }
                    }
                }
            }
            return Nearest;
        }

        public bool ThereIsPlayerInNpcViewRange(Companion companion)
        {
            bool PlayerClose = false;
            for(int i = 0; i < 255; i++)
            {
                if (Main.player[i].active && !(Main.player[i] is Companion))
                {
                    if (MathF.Abs(Main.player[i].Center.X - companion.Center.X) < NPC.sWidth * 0.5f + NPC.safeRangeX && 
                        MathF.Abs(Main.player[i].Center.Y - companion.Center.Y) < NPC.sHeight * 0.5f + NPC.safeRangeY)
                        {
                            PlayerClose = true;
                            break;
                        }
                }
            }
            return PlayerClose;
        }

        public virtual void ChangeDrawMoment(Companion companion, ref CompanionDrawMomentTypes DrawMomentType)
        {
            
        }

        public virtual void OnBegin()
        {
            
        }

        public virtual void OnEnd()
        {

        }
    }
}