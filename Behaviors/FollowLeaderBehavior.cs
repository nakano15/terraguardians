using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class FollowLeaderBehavior : BehaviorBase
    {
        private bool TriedTakingFurnitureToSit = false;
        private byte StuckCounter = 0;
        private bool StuckCounterIncreased = false;
        private int IdleTime = 0;
        public bool AllowIdle = true;

        public override void Update(Companion companion)
        {
            UpdateFollow(companion);
        }

        private void IncreaseStuckCounter(Companion c)
        {
            if(c.IsMountedOnSomething || c.gross) return;
            StuckCounter++;
            StuckCounterIncreased = true;
            if (StuckCounter >= 60)
            {
                StuckCounter = 0;
                if (!c.CreatePathingTo(c.Owner.Bottom - Vector2.UnitY * 2, false))
                {
                    c.Teleport(c.Owner.Bottom);
                    c.Path.CancelPathing();
                }
                c.Target = null;
            }
        }

        public void UpdateFollow(Companion companion)
        {
            Entity Owner = companion.Owner;
            Vector2 Center = companion.Center;
            Vector2 OwnerPosition = Owner.Center, OwnerBottom = Owner.Bottom;
            Companion Mount = null;
            bool OwnerUsingFurniture = false;
            bool OwnerIsIdle = false;
            if (Owner is Player)
            {
                Mount = PlayerMod.PlayerGetMountedOnCompanion(Owner as Player);
                if (Mount != null) //If the player is mounted on another companion, that companion will be what they will take position based off
                {
                    OwnerPosition = Mount.Center;
                    OwnerBottom = Mount.Bottom;
                    OwnerUsingFurniture = Mount.UsingFurniture;
                    OwnerIsIdle = Mount.velocity.X == 0 && Mount.velocity.Y == 0;
                }
                else
                {
                    OwnerUsingFurniture = (Owner as Player).sitting.isSitting || (Owner as Player).sleeping.isSleeping;
                    OwnerIsIdle = Owner.velocity.X == 0 && Owner.velocity.Y == 0;
                }
            }
            if(Math.Abs(OwnerPosition.X - Center.X) >= 500 || 
                Math.Abs(OwnerPosition.Y - Center.Y) >= 400)
            {
                IncreaseStuckCounter(companion);
                //companion.Teleport(Owner.Bottom);
            }
            if(Companion.Behaviour_InDialogue || Companion.Behaviour_AttackingSomething || Companion.Behavior_FollowingPath)
            {
                TriedTakingFurnitureToSit = false;
                return;
            }
            if(companion.GetCharacterMountedOnMe == Owner)
            {
                return;
            }
            if (!companion.IsMountedOnSomething && AllowIdle)
            {
                if (OwnerIsIdle)
                {
                    if (!(OwnerUsingFurniture && companion.UsingFurniture))
                    {
                        IdleTime++;
                        if (IdleTime >= 30 * 60)
                        {
                            if (companion.idleBehavior is IdleBehavior)
                            {
                                (companion.idleBehavior as IdleBehavior).UpdateIdle(companion, true);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    IdleTime = 0;
                }
            }
            if (companion.GoingToOrUsingFurniture || TriedTakingFurnitureToSit)
            {
                if(OwnerUsingFurniture) // && MathF.Abs(companion.Center.X - Owner.Center.X) < 8 * 16 && MathF.Abs(companion.Center.Y - Owner.Center.Y) < 6 * 16)
                {
                    return;
                }
                companion.LeaveFurniture();
                TriedTakingFurnitureToSit = false;
            }
            float Distancing = 0;
            if (Owner is Player)
            {
                Player p = (Player)Owner;
                if (!TriedTakingFurnitureToSit)
                {
                    //Companion Mount = PlayerMod.PlayerGetMountedOnCompanion(p);
                    if(Mount != null)
                        p = Mount;
                    if(p.sitting.isSitting)
                    {
                        TriedTakingFurnitureToSit = true;
                        if(PlayerMod.IsCompanionLeader(p, companion))
                        {
                            int tx = (int)(p.Center.X * (1f / 16)), ty = (int)((p.Bottom.Y - 2) * (1f / 16));
                            Tile tile = Main.tile[tx, ty];
                            bool IsChair = tile.TileType == Terraria.ID.TileID.Chairs;
                            if ((companion.ShareChairWithPlayer || !IsChair) && companion.UseFurniture((int)(p.Center.X * (1f / 16)), (int)((p.Bottom.Y - 2) * (1f / 16))))
                            {
                                return;
                            }
                        }
                        Point chair = WorldMod.GetClosestChair(p.Bottom);
                        if(chair.X > 0 && chair.Y > 0)
                        {
                            companion.UseFurniture(chair.X, chair.Y);
                            return;
                        }
                    }
                    if(p.sleeping.isSleeping)
                    {
                        TriedTakingFurnitureToSit = true;
                        if(PlayerMod.IsCompanionLeader(p, companion) && companion.ShareBedWithPlayer && companion.UseFurniture((int)(p.Center.X * (1f / 16)), (int)((p.Bottom.Y - 2) * (1f / 16))))
                        {
                            return;
                        }
                        Point furniture = WorldMod.GetClosestBed(p.Bottom);
                        if(furniture.X > 0 && furniture.Y > 0)
                        {
                            companion.UseFurniture(furniture.X, furniture.Y);
                            return;
                        }
                        furniture = WorldMod.GetClosestChair(p.Bottom);
                        if(furniture.X > 0 && furniture.Y > 0)
                        {
                            companion.UseFurniture(furniture.X, furniture.Y);
                            return;
                        }
                    }
                }
                PlayerMod pm = ((Player)Owner).GetModPlayer<PlayerMod>();
                float MyFollowDistance = companion.width * 0.8f + 12;
                bool TakeBehindPosition = (Owner.direction > 0 && OwnerPosition.X < Center.X) || (Owner.direction < 0 && OwnerPosition.X > Center.X);
                if(TakeBehindPosition)
                {
                    Distancing = pm.FollowBehindDistancing + MyFollowDistance * 0.5f;
                    pm.FollowBehindDistancing += MyFollowDistance;
                }
                else
                {
                    Distancing = pm.FollowAheadDistancing + MyFollowDistance * 0.5f;
                    pm.FollowAheadDistancing += MyFollowDistance;
                }
            }
            float DistanceFromPlayer = Math.Abs((OwnerPosition.X - Center.X) - Owner.velocity.X);
            if(Owner.velocity.Y == 0 && Math.Abs(OwnerBottom.Y - companion.Bottom.Y) >= 3 * 16)
            {
                if (companion.CreatePathingTo(OwnerBottom - Vector2.UnitY * 2, false))
                    return;
            }
            if(DistanceFromPlayer > 40 + Distancing || 
            (companion.breath < companion.breathMax && DistanceFromPlayer < 8))
            {
                if(OwnerPosition.X < Center.X)
                    companion.MoveLeft = true;
                else
                    companion.MoveRight = true;
            }
            else
            {
                if(!companion.IsMountedOnSomething && companion.velocity.X == 0 && companion.velocity.Y == 0 && companion.itemAnimation == 0)
                {
                    if(OwnerPosition.X < Center.X)
                        companion.direction = -1;
                    else
                        companion.direction = 1;
                }
            }
            if((companion.MoveLeft || companion.MoveRight) && companion.velocity.X == 0 && companion.velocity.Y == 0)
            {
                IncreaseStuckCounter(companion);
            }
            companion.WalkMode = false;
            if (!StuckCounterIncreased)
                StuckCounter = 0;
            StuckCounterIncreased = false;
        }
    }
}