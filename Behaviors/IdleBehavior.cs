using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class IdleBehavior : BehaviorBase
    {
        public IdleStates CurrentState = IdleStates.Waiting;
        public int IdleTime = 0;

        public IdleBehavior()
        {
            IdleTime = Main.rand.Next(100, 300);
        }

        public override void Update(Companion companion)
        {
            if (!UpdateGoingHomeBehavior(companion))
                UpdateIdle(companion);
        }

        public bool UpdateGoingHomeBehavior(Companion companion)
        {
            if(Companion.Is2PCompanion || (companion.IsBeingControlledBySomeone && !companion.CompanionHasControl) || Companion.Behaviour_AttackingSomething || Companion.Behavior_RevivingSomeone || Companion.Behaviour_InDialogue || Companion.Behavior_FollowingPath)
                return false;
            if(!companion.GoHomeTime && !Main.raining)
            {
                return false;
            }
            CompanionTownNpcState tns = companion.GetTownNpcState;
            bool TryGoingSleep = companion.IsOnSleepTime;
            if (tns == null)
            {
                Vector2 IdlePosition = new Vector2(Main.spawnTileX * 16 + 8, Main.spawnTileY * 16);
                {
                    NPC NearestNpc = null;
                    float NearestDistance = float.MaxValue;
                    bool homeless = true;
                    for(int i = 0; i < 200; i++)
                    {
                        if (Main.npc[i].active && Main.npc[i].townNPC && Main.npc[i].type != Terraria.ID.NPCID.OldMan)
                        {
                            float Distance = (Main.npc[i].Center - companion.Center).Length();
                            if(!homeless && Main.npc[i].homeless)
                                continue;
                            homeless = Main.npc[i].homeless;
                            if(Distance < NearestDistance)
                            {
                                NearestNpc = Main.npc[i];
                                NearestDistance = Distance;
                            }
                        }
                    }
                    if (NearestNpc != null)
                    {
                        if(!NearestNpc.homeless)
                        {
                            IdlePosition.X = NearestNpc.homeTileX * 16 + 8;
                            IdlePosition.Y = NearestNpc.homeTileY * 16;
                        }
                        else
                        {
                            IdlePosition = NearestNpc.Bottom;
                        }
                    }
                }
                if(MathF.Abs(companion.Center.X - IdlePosition.X) > 200)
                {
                    ChangeIdleState(IdleStates.GoToClosestWaitingPoint, 5);
                    if(IdlePosition.X < companion.Center.X)
                    {
                        companion.MoveLeft = true;
                    }
                    else
                    {
                        companion.MoveRight = true;
                    }
                    companion.WalkMode = true;
                }
                else
                {
                    if(CurrentState == IdleStates.GoToClosestWaitingPoint)
                    {
                        if(IdleTime > 0)
                        {
                            if(IdlePosition.X < companion.Center.X)
                            {
                                companion.MoveLeft = true;
                            }
                            else
                            {
                                companion.MoveRight = true;
                            }
                            companion.WalkMode = true;
                        }
                        ChangeIdleState(Main.rand.Next(3) == 0 ? IdleStates.WanderAroundWaitingPoint : IdleStates.IdleAroundWaitingPoint, Main.rand.Next(30, 151));
                    }
                    else
                    {
                        switch(CurrentState)
                        {
                            case IdleStates.IdleAroundWaitingPoint:
                                if(IdleTime <= 0)
                                {
                                    if(Main.rand.Next(3) == 0)
                                    {
                                        ChangeIdleState(IdleStates.WanderAroundWaitingPoint, Main.rand.Next(200, 401));
                                    }
                                    else
                                    {
                                        ChangeIdleState(IdleStates.IdleAroundWaitingPoint, Main.rand.Next(200, 401));
                                        companion.direction *= -1;
                                    }
                                }
                                break;
                            case IdleStates.WanderAroundWaitingPoint:
                                if (IdleTime <= 0)
                                {
                                    ChangeIdleState(IdleStates.IdleAroundWaitingPoint, Main.rand.Next(200, 401));
                                }
                                else
                                {
                                    float Distance = IdlePosition.X - companion.Center.X;
                                    if(Math.Abs(Distance) < 150)
                                    {
                                        if (Distance > 0 && companion.direction == 1)
                                        {
                                            companion.MoveLeft = true;
                                        }
                                        else if(Distance < 0 && companion.direction == -1)
                                        {
                                            companion.MoveRight = true;
                                        }
                                    }
                                    else
                                    {
                                        if(companion.direction == 1)
                                            companion.MoveRight = true;
                                        else
                                            companion.MoveLeft = true;
                                    }
                                    companion.WalkMode = true;
                                }
                                break;
                        }
                    }
                }
            }
            else
            {
                if(!companion.IsAtHome)
                {
                    if(CurrentState != IdleStates.GoHome)
                    {
                        ChangeIdleState(IdleStates.GoHome, 5);
                    }
                }
                switch(CurrentState)
                {
                    case IdleStates.GoHome:
                        if (tns.Homeless)
                        {
                            ChangeIdleState(IdleStates.GoToClosestWaitingPoint, 5);
                            return true;
                        }
                        if(System.MathF.Abs(tns.HomeX * 16 + 8 - companion.Center.X) < 8)
                        {
                            if(TryGoingSleep && TrySendingToBed(companion))
                            {
                                return true;
                            }
                            ChangeIdleState(IdleStates.IdleHome, 5);
                        }
                        else
                        {
                            bool AnyPlayerNearby = false;
                            Vector2 CheckPosition = companion.Center;
                            Vector2 HousePosition = new Vector2(tns.HomeX * 16 + 8, tns.HomeY * 16);
                            for(int i = 0; i < 255; i++)
                            {
                                if (Main.player[i].active && PlayerMod.IsPlayerCharacter(Main.player[i]))
                                {
                                    if(Main.player[i].Distance(CheckPosition) < Main.screenWidth || Main.player[i].Distance(HousePosition) < Main.screenWidth)
                                    {
                                        AnyPlayerNearby = true;
                                        break;
                                    }
                                }
                            }
                            if (!AnyPlayerNearby)
                            {
                                companion.Teleport(HousePosition);
                            }
                            else if (!companion.CreatePathingTo(tns.HomeX, tns.HomeY, true))
                            {
                                if (!IsDangerousAhead(companion))
                                {
                                    if(tns.HomeX * 16 + 8 < companion.Center.X)
                                    {
                                        companion.MoveLeft = true;
                                    }
                                    else
                                    {
                                        companion.MoveRight = true;
                                    }
                                }
                            }
                        }
                        companion.WalkMode = true;
                        break;
                    case IdleStates.WanderHome:
                        {
                            int CheckAheadX = (int)((companion.Center.X + companion.SpriteWidth * 0.6f * companion.direction) * (1f / 16));
                            int CheckAheadY = (int)((companion.position.Y + Player.defaultHeight) * (1f / 16));
                            bool Door = false;
                            for (int y = 0; y < 4; y++)
                            {
                                Tile tile = Main.tile[CheckAheadX, CheckAheadY - y];
                                if (tile != null && tile.HasTile && (tile.TileType == Terraria.ID.TileID.ClosedDoor || (tile.TileType == Terraria.ID.TileID.OpenDoor && (tile.TileFrameX >= 18 && tile.TileFrameX <= 36) || tile.TileType == Terraria.ID.TileID.TallGateClosed || tile.TileType == Terraria.ID.TileID.TallGateOpen)))
                                {
                                    Door = true;
                                    break;
                                }
                            }
                            if(Door)
                            {
                                companion.direction *= -1;
                            }
                            if (companion.direction < 0) companion.MoveLeft = true;
                            else companion.MoveRight = true;
                            companion.WalkMode = true;
                            if (IdleTime <= 0)
                            {
                                if(TryGoingSleep && TrySendingToBed(companion))
                                {
                                    return true;
                                }
                                ChangeIdleState(IdleStates.IdleHome, Main.rand.Next(200, 401));
                            }
                        }
                        break;
                    default:
                    case IdleStates.IdleHome:
                        {
                            if (IdleTime <= 0)
                            {
                                companion.LeaveFurniture();
                                if(TryGoingSleep && TrySendingToBed(companion))
                                {
                                    return true;
                                }
                                if (Main.rand.NextFloat() < 0.6f && TryUsingFurnitureNearby(companion, true))
                                {
                                    ChangeIdleState(IdleStates.UseNearbyFurnitureHome, 800 + Main.rand.Next(601));
                                    return true;
                                }
                                if (Main.rand.NextFloat() < 0.4f)
                                {
                                    ChangeIdleState(IdleStates.WanderHome, Main.rand.Next(300, 601));
                                    if (Main.rand.NextFloat() < 0.6f)
                                    {
                                        int WanderStartX = (int)(companion.Center.X * Companion.DivisionBy16) + Main.rand.Next(-8, 9);
                                        int WanderStartY = (int)(companion.Bottom.Y * Companion.DivisionBy16) + Main.rand.Next(-8, 9);
                                        if(!companion.GetTownNpcState.Homeless && companion.GetTownNpcState.HouseInfo.BelongsToThisHousing(WanderStartX, WanderStartY) && CheckIfPlaceIsSafe(companion, WanderStartX, WanderStartY))
                                            companion.CreatePathingTo(WanderStartX, WanderStartY, true);
                                    }
                                }
                                else
                                {
                                    ChangeIdleState(IdleStates.IdleHome, Main.rand.Next(300, 601));
                                    companion.direction *= -1;
                                }
                            }
                        }
                        break;
                    case IdleStates.UseNearbyFurnitureHome:
                        {
                            if (IdleTime <= 0)
                            {
                                if(TryGoingSleep && TrySendingToBed(companion))
                                {
                                    return true;
                                }
                                companion.LeaveFurniture();
                                ChangeIdleState(IdleStates.IdleHome, 400 + Main.rand.Next(201));
                            }
                            companion.WalkMode = true;
                        }
                        break;
                    case IdleStates.GoSleepHome:
                        {
                            if (!companion.GoingToOrUsingFurniture)
                            {
                                ChangeIdleState(IdleStates.IdleHome, Main.rand.Next(200, 401));
                            }
                            companion.WalkMode = true;
                            if(IdleTime <= 0)
                            {
                                if(TryGoingSleep)
                                {
                                    ChangeIdleState(IdleStates.GoSleepHome, Main.rand.Next(400, 801));
                                }
                                else
                                {
                                    ChangeIdleState(IdleStates.IdleHome, Main.rand.Next(200, 401));
                                }
                            }
                        }
                        break;
                }
            }
            IdleTime--;
            return true;
        }

        public bool CheckIfPlaceIsSafe(Companion c, int X, int Y)
        {
            Tile tile = Main.tile[X, Y];
            switch(tile.LiquidType)
            {
                case LiquidID.Lava:
                    if (!c.HasLavaImmunityAbility)
                        return false;
                    break;
                case LiquidID.Water:
                    if (!c.HasWaterbreathingAbility)
                        return false;
                    break;
            }
            if (!tile.HasTile || tile.IsActuated)
            {
                bool FoundTile = false;
                for (int i = 0; i < 8; i++)
                {
                    Y++;
                    tile = Main.tile[X, Y];
                    if (tile.HasTile && !tile.IsActuated)
                    {
                        FoundTile = true;
                    }
                }
                if (!FoundTile) return false;
            }
            if (PathFinder.IsDangerousTile(X, Y, c.fireWalk))
            {
                return false;
            }
            return true;
        }

        public bool TrySendingToBed(Companion c)
        {
            if(TryUsingBedNearby(c, true))
            {
                ChangeIdleState(IdleStates.GoSleepHome, Main.rand.Next(400, 801));
                return true;
            }
            return false;
        }

        public void UpdateIdle(Companion companion, bool FollowerMode = false)
        {
            if(Companion.Is2PCompanion || (companion.IsBeingControlledBySomeone && !companion.CompanionHasControl) || Companion.Behaviour_AttackingSomething || Companion.Behavior_RevivingSomeone || Companion.Behaviour_InDialogue || Companion.Behavior_FollowingPath)
                return;
            if(companion.wet && companion.breath < companion.breathMax)
                ChangeIdleState(IdleStates.Wandering, 5);
            Player Owner = companion.Owner;
            switch(CurrentState)
            {
                default:
                    {
                        ChangeIdleState(Main.rand.Next(3) == 0 ? IdleStates.Wandering : IdleStates.Waiting, Main.rand.Next(200, 401));
                        break;
                    }
                case IdleStates.GoSleepHome:
                    {
                        IdleTime--;
                        if (IdleTime <= 0)
                        {
                            companion.LeaveFurniture();
                            ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                        }
                    }
                    break;
                case IdleStates.UseNearbyFurniture:
                    {
                        IdleTime--;
                        companion.WalkMode = true;
                        if (IdleTime <= 0)
                        {
                            companion.LeaveFurniture();
                            if(Main.rand.Next(3) == 0)
                            {
                                ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                                if(companion.velocity.X == 0 && companion.velocity.Y == 0)
                                {
                                    companion.direction *= -1;
                                }
                            }
                            else
                            {
                                ChangeIdleState(IdleStates.Wandering, Main.rand.Next(200, 601));
                            }
                        }
                    }
                    break;
                case IdleStates.Waiting:
                    {
                        IdleTime--;
                        if(IdleTime <= 0)
                        {
                            companion.LeaveFurniture();
                            if (Main.rand.Next(3) == 0 && TryUsingFurnitureNearby(companion, false, (FollowerMode ? Owner.Bottom : default(Vector2)), (FollowerMode ? 5 : 8)))
                            {
                                ChangeIdleState(IdleStates.UseNearbyFurniture, Main.rand.Next(400, 801));
                                return;
                            }
                            if(Main.rand.Next(2) == 0)
                            {
                                ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                                if(companion.velocity.X == 0 && companion.velocity.Y == 0)
                                {
                                    companion.direction *= -1;
                                }
                            }
                            else
                            {
                                ChangeIdleState(IdleStates.Wandering, Main.rand.Next(200, 601));
                                if (!FollowerMode && Main.rand.NextFloat() < 0.4f)
                                {
                                    int WanderStartX = (int)(companion.Center.X * Companion.DivisionBy16) + Main.rand.Next(-8, 9);
                                    int WanderStartY = (int)(companion.Bottom.Y * Companion.DivisionBy16) + Main.rand.Next(-8, 9);
                                    if (CheckIfPlaceIsSafe(companion, WanderStartX, WanderStartY))
                                        companion.CreatePathingTo(WanderStartX, WanderStartY, true);
                                }
                            }
                        }
                    }
                    break;
                case IdleStates.Wandering:
                    {
                        IdleTime --;
                        if(IdleTime <= 0)
                        {
                            ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                            companion.LeaveFurniture();
                        }
                        else
                        {
                            MoveTowardsDirection(companion);
                            companion.WalkMode = true;
                            if (FollowerMode)
                            {
                                if (Owner is Player)
                                {
                                    Companion Mount = PlayerMod.PlayerGetMountedOnCompanion(Owner as Player);
                                    if (Mount != null)
                                    {
                                        Owner = Mount;
                                    }
                                }
                                if (Math.Abs(Owner.Center.X - companion.Center.X) > 6 * 16)
                                {
                                    if ((companion.direction == -1 && companion.Center.X < Owner.Center.X) || 
                                        (companion.direction == 1 && companion.Center.X > Owner.Center.X))
                                        companion.direction *= -1;
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public bool TryUsingBedNearby(Companion companion, bool AtHome = false)
        {
            AtHome = AtHome && companion.IsAtHome;
            if (AtHome && !companion.IsTownNpc)
            {
                return false;
            }
            BuildingInfo building = AtHome ? companion.GetTownNpcState.HouseInfo : null;
            Point Bed = WorldMod.GetClosestBed(companion.Bottom, HouseLimitation: building);
            if(Bed.X > 0 && Bed.Y > 0)
            {
                if (companion.UseFurniture(Bed.X, Bed.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryUsingFurnitureNearby(Companion companion, bool AtHome, Vector2 CenterPosition = default(Vector2), int TileRange = 8)
        {
            AtHome = AtHome && companion.IsAtHome;
            if (AtHome && !companion.IsTownNpc)
            {
                return false;
            }
            BuildingInfo building = AtHome ? companion.GetTownNpcState.HouseInfo : null;
            if (CenterPosition == default(Vector2))
                CenterPosition = companion.Bottom;
            Point Chair = WorldMod.GetClosestChair(CenterPosition, HouseLimitation: building, DistanceX: TileRange);
            if(Chair.X > 0 && Chair.Y > 0)
            {
                if (companion.UseFurniture(Chair.X, Chair.Y))
                {
                    return true;
                }
            }
            return false;
        }

        public void ChangeIdleState(IdleStates NewState, int NewTime)
        {
            CurrentState = NewState;
            IdleTime = NewTime;
        }

        public enum IdleStates : byte
        {
            Waiting,
            Wandering,
            UseNearbyFurniture,
            GoHome,
            IdleHome,
            WanderHome,
            UseNearbyFurnitureHome,
            GoSleepHome,
            GoToClosestWaitingPoint,
            IdleAroundWaitingPoint,
            WanderAroundWaitingPoint
        }
    }
}