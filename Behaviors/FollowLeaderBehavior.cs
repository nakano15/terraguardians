using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class FollowLeaderBehavior : BehaviorBase
    {
        private bool TriedTakingFurnitureToSit = false, GotFurnitureToSit = false;
        private byte StuckCounter = 0;
        private bool StuckCounterIncreased = false;
        private int IdleTime = 0;
        public bool AllowIdle = true;
        byte PathingCooldown = 0;

        public override void Update(Companion companion)
        {
            UpdateFollow(companion);
        }

        private void IncreaseStuckCounter(Companion c)
        {
            if((c.IsMountedOnSomething && PlayerMod.IsPlayerCharacter(c.GetCharacterMountedOnMe)) || c.gross) return;
            StuckCounter++;
            StuckCounterIncreased = true;
            if (StuckCounter >= 60)
            {
                StuckCounter = 0;
                if (!c.CreatePathingTo(c.Owner.Bottom - Vector2.UnitY * 2, false))
                {
                    c.BePulledByPlayer();
                    //c.Teleport(c.Owner.Bottom);
                    //c.Path.CancelPathing();
                    c.reviveBehavior.ClearReviveTarget();
                }
                c.Target = null;
            }
        }

        public virtual void GetMaxFollowDistance(Companion companion, MainMod.CompanionMaxDistanceFromPlayer DistanceInSettings, out float MaxDistX, out float MaxDistY)
        {
            switch(DistanceInSettings)
            {
                default:
                    MaxDistX = 500;
                    MaxDistY = 400;
                    return;
                case MainMod.CompanionMaxDistanceFromPlayer.Nearer:
                    MaxDistX = 400;
                    MaxDistY = 300;
                    return;
                case MainMod.CompanionMaxDistanceFromPlayer.Far:
                    MaxDistX = 700;
                    MaxDistY = 500;
                    return;
                case MainMod.CompanionMaxDistanceFromPlayer.Farther:
                    MaxDistX = 1000;
                    MaxDistY = 700;
                    return;
            }
        }

        public void UpdateFollow(Companion companion)
        {
            if (companion.IsBeingControlledBySomeone)
            {
                return;
            }
            Player Owner = companion.Owner;
            Vector2 Center = companion.Center;
            Vector2 OwnerPosition = Owner.Center, OwnerBottom = Owner.Bottom;
            Vector2 OwnerVelocity = Owner.velocity;
            Companion Mount = null;
            bool OwnerUsingFurniture = false;
            bool OwnerIsIdle = false;
            {
                Companion Controlled = PlayerMod.PlayerGetControlledCompanion(Owner as Player);
                if (Controlled != null) Owner = Controlled;
                OwnerPosition = Owner.Center;
                OwnerBottom = Owner.Bottom;
                Mount = PlayerMod.PlayerGetMountedOnCompanion(Owner as Player);
                if (Mount != null) //If the player is mounted on another companion, that companion will be what they will take position based off
                {
                    OwnerPosition = Mount.Center;
                    OwnerBottom = Mount.Bottom;
                    OwnerVelocity = Mount.velocity;
                    OwnerUsingFurniture = Mount.UsingFurniture;
                    OwnerIsIdle = Mount.velocity.X == 0 && Mount.velocity.Y == 0;
                }
                else
                {
                    OwnerUsingFurniture = (Owner as Player).sitting.isSitting || (Owner as Player).sleeping.isSleeping;
                    OwnerIsIdle = Owner.velocity.X == 0 && Owner.velocity.Y == 0;
                }
                if (companion.Data.FollowAhead && !OwnerUsingFurniture)
                {
                    OwnerPosition.X += Owner.direction * (Owner.GetModPlayer<PlayerMod>().FollowAheadDistancing + companion.SpriteWidth * 0.5f * companion.Scale);
                }
            }
            {
                float MaxDistX, MaxDistY;
                GetMaxFollowDistance(companion, MainMod.MaxDistanceFromPlayer, out MaxDistX, out MaxDistY);
                if(MaxDistX > 0 && MaxDistY > 0 && (Math.Abs(OwnerPosition.X - Center.X) >= MaxDistX || 
                    Math.Abs(OwnerPosition.Y - Center.Y) >= MaxDistY))
                {
                    IncreaseStuckCounter(companion);
                    companion.reviveBehavior.ClearReviveTarget();
                    //companion.Teleport(Owner.Bottom);
                }
            }
            if (companion.KnockoutStates > 0 || Companion.Is2PCompanion) return;
            if(Companion.Behaviour_InDialogue || Companion.Behaviour_AttackingSomething || Companion.Behavior_FollowingPath || Companion.Behavior_RevivingSomeone)
            {
                TriedTakingFurnitureToSit = GotFurnitureToSit = false;
                return;
            }
            if(companion.GetCharacterMountedOnMe == Owner)
            {
                return;
            }
            if (companion.CompanionHasControl && AllowIdle)
            {
                bool FastIdle = OwnerUsingFurniture && !GotFurnitureToSit && TriedTakingFurnitureToSit;
                if (OwnerIsIdle || FastIdle)
                {
                    if (FastIdle || !(OwnerUsingFurniture && companion.UsingFurniture))
                    {
                        IdleTime++;
                        if (IdleTime >= 30 * 60 || FastIdle)
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
                TriedTakingFurnitureToSit = GotFurnitureToSit = false;
            }
            bool GoAhead = companion.Data.FollowAhead;
            float Distancing = 0;
            {
                Player p = Owner;
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
                            if (companion.UseFurniture(chair.X, chair.Y))
                            {
                                GotFurnitureToSit = true;
                            }
                            return;
                        }
                        GotFurnitureToSit = false;
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
                            if (companion.UseFurniture(furniture.X, furniture.Y))
                                GotFurnitureToSit = true;
                            return;
                        }
                        furniture = WorldMod.GetClosestChair(p.Bottom);
                        if(furniture.X > 0 && furniture.Y > 0)
                        {
                            if (companion.UseFurniture(furniture.X, furniture.Y))
                                GotFurnitureToSit = true;
                            return;
                        }
                        GotFurnitureToSit = false;
                    }
                }
                PlayerMod pm = Owner.GetModPlayer<PlayerMod>();
                float MyFollowDistance = companion.FollorOrder.Distance;
                bool TakeBehindPosition = !GoAhead || ((Owner.direction > 0 && OwnerPosition.X < Center.X) || (Owner.direction < 0 && OwnerPosition.X > Center.X));
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
            float DistanceFromPlayer = Math.Abs(OwnerPosition.X - Center.X);
            if(Owner.velocity.Y == 0 && Owner.TouchedTiles.Count > 0 && Math.Abs(OwnerBottom.Y - companion.Bottom.Y) >= 3 * 16)
            {
                if (companion.CreatePathingTo(OwnerBottom - Vector2.UnitY * 2, false, false, true))
                    return;
            }
            if((!GoAhead && DistanceFromPlayer > 40 + Distancing) || 
                (GoAhead && DistanceFromPlayer > 20 + Math.Abs(companion.velocity.X)) || 
            (companion.wet && companion.breath < companion.breathMax && !companion.HasWaterbreathingAbility && DistanceFromPlayer > 8 && !Owner.wet))
            {
                if (IsDangerousAhead(companion, (int)MathF.Min(MathF.Abs(companion.velocity.X * 1.6f) * (1f / 16), 3)) || CheckForHoles(companion, ExtraCheckRangeX: MathF.Abs(companion.velocity.X * 1.2f)))
                {
                    if (PathingCooldown == 0)
                    {
                        companion.CreatePathingTo(OwnerBottom - Vector2.UnitY * 2, false, false, true);
                        PathingCooldown = 12;
                    }
                    else
                    {
                        companion.BePulledByPlayer();
                    }
                    /*if ((companion.velocity.X < 0 && companion.direction < 0) || (companion.velocity.X > 0 && companion.direction > 0))
                    {
                        if(OwnerPosition.X > Center.X)
                            companion.MoveLeft = true;
                        else
                            companion.MoveRight = true;
                    }*/
                }
                else
                {
                    if(OwnerPosition.X < Center.X)
                        companion.MoveLeft = true;
                    else
                        companion.MoveRight = true;
                }
            }
            if (
                ((companion.wet && (companion.breath < companion.breathMax * .5f || (!Owner.wet || OwnerPosition.Y < Center.Y))) && companion.HasSwimmingAbility && companion.GetJumpState<FlipperJump>().Available) || 
                (companion.HasFlightAbility && OwnerPosition.Y < companion.position.Y - companion.velocity.Y && companion.wingTime > 0))
            {
                companion.controlJump = true;
            }
            if(companion.CompanionHasControl && companion.velocity.X == 0 && companion.velocity.Y == 0 && companion.itemAnimation == 0)
            {
                if(OwnerPosition.X + OwnerVelocity.X < Center.X)
                    companion.direction = -1;
                else
                    companion.direction = 1;
            }
            if((companion.MoveLeft || companion.MoveRight) && companion.velocity.X == 0 && companion.velocity.Y == 0)
            {
                IncreaseStuckCounter(companion);
            }
            companion.WalkMode = false;
            if (!StuckCounterIncreased)
                StuckCounter = 0;
            StuckCounterIncreased = false;
            if (PathingCooldown > 0) PathingCooldown--;
        }
    }
}