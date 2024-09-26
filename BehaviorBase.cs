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
        internal static List<AffectedByBehaviorInfo> AffectedList = new List<AffectedByBehaviorInfo>();

        #region Permissions
        private BitsByte _permissionset1 = 0, _permissionset2 = 0;
        private List<Companion> AffectedCompanions = new List<Companion>();

        public virtual bool AllowDespawning { get { return true; } }
        public bool UseHealingItems { get { return !_permissionset1[0]; } set { _permissionset1[0] = !value; }}
        public bool RunCombatBehavior { get { return !_permissionset1[1]; } set { _permissionset1[1] = !value; } }
        public bool AllowSeekingTargets { get { return !_permissionset1[2]; } set { _permissionset1[2] = !value; } }
        public bool CanBeAttacked { get { return !_permissionset1[3]; } set { _permissionset1[3] = !value; } }
        public bool CanBeHurtByNpcs { get { return !_permissionset1[4]; } set { _permissionset1[4] = !value; } }
        public bool CanTargetNpcs { get { return !_permissionset1[5]; } set { _permissionset1[5] = !value; } }
        public bool IsVisible { get { return !_permissionset1[6]; } set { _permissionset1[6] = !value; } }
        public bool AllowRevivingSomeone { get { return !_permissionset1[7]; } set { _permissionset1[7] = !value; } }
        public bool CanAggroNpcs { get { return !_permissionset2[0]; } set { _permissionset2[0] = !value; } }
        #endregion
        private Companion Owner;
        public Companion GetOwner{ get { return Owner; } }
        private bool Active = true;
        public bool IsActive { get { return Active; } }
        public virtual bool CanBeFacedOnDialogue => true;

        public void Deactivate()
        {
            Active = false;
            OnEnd();
        }

        internal static void Unload()
        {
            AffectedList.Clear();
            AffectedList = null;
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

        public virtual void ChangeLobbyDialogueOptions(MessageDialogue Message, out bool ShowCloseButton)
        {
            ShowCloseButton = true;
        }

        public virtual void UpdateAnimationFrame(Companion companion)
        {

        }

        public virtual void UpdateAffectedCompanionAnimationFrame(Companion companion)
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

        public virtual void ModifyHurt(Companion companion, ref Player.HurtModifiers modifiers)
        {
            
        }
        #endregion

        private bool IsPassableTile(Tile tile)
        {
            if (tile.HasTile && !tile.IsActuated)
            {
                switch(tile.TileType)
                {
                    case TileID.ClosedDoor:
                    case TileID.TallGateClosed:
                        return true;
                    default:
                        if (Main.tileSolid[tile.TileType])
                        {
                            return false;
                        }
                        break;
                }
            }
            return true;
        }

        public void MoveTowards(Companion companion, Vector2 Position)
        {
            MoveTowardsDirection(companion, Position.X < companion.Center.X ? -1 : 1);
        }

        public void MoveTowardsDirection(Companion companion, int direction = 0)
        {
            if (direction == 0)
                direction = companion.direction;
            int CheckAheadX = (int)((companion.Center.X + (companion.width * 0.5f + 8) * direction) * (1f / 16));
            int CheckAheadY = (int)((companion.position.Y + companion.height) * (1f / 16));
            byte GapHeight = 0;
            bool DangerAhead = false, GroundAhead = false, BlockedAhead = false;
            byte HoleHeight = 0;
            byte WaterHeight = 0;
            int GroundHeight = CheckAheadY;
            if (IsDangerousAhead(companion))
            {
                DangerAhead = true;
            }
            else
            {
                {
                    bool FoundGround = false, FoundOpenSpace = false;
                    for (int y = 0; y < 3; y++)
                    {
                        int PosY = GroundHeight;
                        if (WorldGen.InWorld(CheckAheadX, PosY))
                        {
                            Tile tile = Main.tile[CheckAheadX, PosY];
                            if (!IsPassableTile(tile))
                            {
                                FoundGround = true;
                                GroundHeight--;
                                if (FoundOpenSpace)
                                {
                                    break;
                                }
                            }
                            else
                            {
                                FoundOpenSpace = true;
                                if (FoundGround)
                                {
                                    break;
                                }
                                GroundHeight++;
                            }
                        }
                    }
                    if (!FoundGround && !FoundOpenSpace)
                    {
                        BlockedAhead = true;
                    }
                }
                if (!BlockedAhead)
                {
                    CheckAheadY = GroundHeight;
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
                                if(!Main.tileSolid[tile.TileType] && tile.LiquidAmount > 50 && ((tile.LiquidType == LiquidID.Lava || 
                                (tile.LiquidType == LiquidID.Water) && WaterHeight++ * 16 >= companion.height - 8 && !companion.HasWaterWalkingAbility && !companion.HasWaterbreathingAbility)))
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
                }
            }
            //companion.SaySomething("Danger Ahead? " + DangerAhead + "  Gap Height? " + GapHeight + "  Hole Height? " + HoleHeight);
            if (BlockedAhead || DangerAhead || GapHeight < 3 || HoleHeight >= 4)
            {
                direction *= -1;
            }
            if(direction > 0)
                companion.MoveRight = true;
            else
                companion.MoveLeft = true;
        }

        public bool CheckForHoles(Companion companion, int direction = 0, float ExtraCheckRangeX = 0)
        {
            if (direction == 0)
                direction = companion.direction;
            int CheckAheadX = (int)((companion.Center.X + (companion.SpriteWidth * 0.5f + 8 + ExtraCheckRangeX) * direction) * (1f / 16));
            int GroundHeight = (int)(companion.Bottom.Y * (1f / 16));
            bool FoundGround = false, FoundOpenSpace = false;
            for (int y = 0; y < 3; y++)
            {
                int PosY = GroundHeight;
                if (WorldGen.InWorld(CheckAheadX, PosY))
                {
                    Tile tile = Main.tile[CheckAheadX, PosY];
                    if (!IsPassableTile(tile))
                    {
                        FoundGround = true;
                        GroundHeight--;
                        if (FoundOpenSpace)
                        {
                            break;
                        }
                    }
                    else
                    {
                        FoundOpenSpace = true;
                        if (FoundGround)
                        {
                            break;
                        }
                        GroundHeight++;
                    }
                }
            }
            if (!FoundGround && !FoundOpenSpace)
            {
                return true;
            }
            return false;
        }

        public Player SeekCharacterInViewRange(Companion companion, int Direction = 0, int DistanceX = 300, int DistanceY = 150, bool SpotPlayers = true, bool SpotCompanions = false)
        {
            return ViewRangeCheck(companion, Direction, DistanceX, DistanceY, SpotPlayers, SpotCompanions);
        }

        public Player ViewRangeCheck(Companion companion, int Direction, int DistanceX = 300, int DistanceY = 150, bool SpotPlayers = true, bool SpotCompanions = false)
        {
            if (Direction == 0) Direction = companion.direction;
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

        public virtual void AffectedCompanionChangeDrawMoment(Companion companion, ref CompanionDrawMomentTypes DrawMomentType)
        {
            
        }

        public virtual void OnBegin()
        {
            
        }

        public virtual void OnEnd()
        {

        }

        public virtual void WhenKOdOrKilled(Companion companion, bool Died)
        {

        }

        public bool IsDangerousAhead(Companion companion, int ExtraCheckDistanceX = 0, int ExtraCheckHeight = 0, int Direction = 0)
        {
            if (Direction == 0) Direction = companion.direction;
            int TileAheadX = (int)(companion.Center.X * (1f / 16) + (2 + ExtraCheckDistanceX) * Direction);
            int TileAheadY = (int)(companion.Bottom.Y * (1f / 16));
            byte LavaTiles = 0;
            for (int x = 0; x < 3; x++)
            {
                bool SolidTileFound = false;
                
                for (int y = -2; y < 6 + ExtraCheckHeight; y++)
                {
                    Tile tile = Main.tile[TileAheadX + x * Direction, TileAheadY + y];
                    if (tile != null)
                    {
                        if (tile.HasTile && !tile.IsActuated)
                        {
                            if (Main.tileSolid[tile.TileType])
                            {
                                SolidTileFound = true;
                                LavaTiles = 0;
                            }
                            if ((tile.TileType != TileID.Cactus || Main.dontStarveWorld) && (TileID.Sets.TouchDamageBleeding[tile.TileType] || 
                                (!companion.fireWalk && TileID.Sets.TouchDamageHot[tile.TileType]) || 
                                TileID.Sets.TouchDamageImmediate[tile.TileType] > 0))
                                {
                                    return true;
                                }
                        }
                        if (tile.LiquidType == LiquidID.Lava && tile.LiquidAmount > 0)
                        {
                            if (LavaTiles <= x)
                            {
                                LavaTiles++;
                                if (LavaTiles >= 2)
                                {
                                    return true;
                                }
                                break;
                            }
                        }
                    }
                    if (SolidTileFound && y >= 0)
                    {
                        break;
                    }
                }
                //if (SolidTileFound) return false;
            }
            return false;
        }

        protected void AffectCompanion(Companion other)
        {
            if (Owner == null) return;
            foreach(AffectedByBehaviorInfo b in AffectedList)
            {
                if (b.BehaviorOwner == Owner && b.TheBehavior == this)
                {
                    b.AffectedOnes.Add(other);
                    return;
                }
            }
            AffectedByBehaviorInfo nb = new AffectedByBehaviorInfo(this, Owner);
            nb.AffectedOnes.Add(other);
            AffectedList.Add(nb);
        }

        internal static void UpdateAffectedCompanions()
        {
            for (int i = 0; i < AffectedList.Count; i++)
            {
                if (!AffectedList[i].TheBehavior.IsActive)
                {
                    AffectedList.RemoveAt(i);
                }
            }
        }

        public class AffectedByBehaviorInfo
        {
            public BehaviorBase TheBehavior;
            public Companion BehaviorOwner;
            public List<Companion> AffectedOnes = new List<Companion>();

            public AffectedByBehaviorInfo(BehaviorBase behavior, Companion Owner)
            {
                BehaviorOwner = Owner;
                TheBehavior = behavior;
            }

            public bool Contains(Companion c)
            {
                foreach(Companion c2 in AffectedOnes)
                {
                    if (c2 == c) return true;
                }
                return false;
            }
        }
    }
}