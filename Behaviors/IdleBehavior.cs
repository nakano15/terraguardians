using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class IdleBehavior : BehaviorBase
    {
        public IdleStates CurrentState = IdleStates.Waiting;
        public int IdleTime = 0;

        public override void Update(Companion companion)
        {
            if (!UpdateGoingHomeBehavior(companion))
                UpdateIdle(companion);
        }

        public bool UpdateGoingHomeBehavior(Companion companion)
        {
            if(Companion.Behaviour_AttackingSomething || Companion.Behaviour_InDialogue)
                return false;
            if(Main.dayTime != companion.Base.IsNocturnal)
            {
                return false;
            }
            CompanionTownNpcState tns = companion.GetTownNpcState;
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
                        if(System.MathF.Abs(tns.HomeX * 16 + 8 - companion.Center.X) < 8)
                        {
                            ChangeIdleState(IdleStates.IdleHome, 5);
                        }
                        else if(tns.HomeX * 16 + 8 < companion.Center.X)
                        {
                            companion.MoveLeft = true;
                        }
                        else
                        {
                            companion.MoveRight = true;
                        }
                        companion.WalkMode = true;
                        break;
                    case IdleStates.WanderHome:
                        {
                            Point TileAhead = companion.direction < 0 ? companion.BottomLeft.ToTileCoordinates() : companion.BottomRight.ToTileCoordinates();
                            TileAhead.Y--;
                            for(byte i = 0; i < 2; i++)
                            {
                                Tile t = Main.tile[TileAhead.X, TileAhead.Y];
                                if(t != null && t.HasTile && Main.tileSolid[t.TileType])
                                {
                                    companion.direction *= -1;
                                    break;
                                }
                                TileAhead.Y--;
                            }
                            if (companion.direction < 0) companion.MoveLeft = true;
                            else companion.MoveRight = true;
                            companion.WalkMode = true;
                            if (IdleTime <= 0)
                            {
                                ChangeIdleState(IdleStates.IdleHome, Main.rand.Next(200, 401));
                            }
                        }
                        break;
                    case IdleStates.IdleHome:
                        {
                            if (IdleTime <= 0)
                            {
                                if (Main.rand.Next(3) == 0)
                                {
                                    ChangeIdleState(IdleStates.WanderHome, Main.rand.Next(200, 401));
                                }
                                else
                                {
                                    ChangeIdleState(IdleStates.IdleHome, Main.rand.Next(200, 401));
                                    companion.direction *= -1;
                                }
                            }
                        }
                        break;
                }
            }
            IdleTime--;
            return true;
        }

        public void UpdateIdle(Companion companion)
        {
            if(Companion.Behaviour_AttackingSomething || Companion.Behaviour_InDialogue)
                return;
            switch(CurrentState)
            {
                default:
                    {
                        ChangeIdleState(Main.rand.Next(3) == 0 ? IdleStates.Wandering : IdleStates.Waiting, Main.rand.Next(200, 401));
                        break;
                    }
                case IdleStates.Waiting:
                    {
                        IdleTime--;
                        if(IdleTime <= 0)
                        {
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
                case IdleStates.Wandering:
                    {
                        IdleTime --;
                        if(IdleTime <= 0)
                        {
                            ChangeIdleState(IdleStates.Waiting, Main.rand.Next(200, 401));
                        }
                        else
                        {
                            companion.WalkMode = true;
                            if(companion.direction > 0)
                                companion.MoveRight = true;
                            else
                                companion.MoveLeft = true;
                        }
                    }
                    break;
            }
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
            GoHome,
            IdleHome,
            WanderHome,
            GoToClosestWaitingPoint,
            IdleAroundWaitingPoint,
            WanderAroundWaitingPoint
        }
    }
}