using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class FollowLeaderBehavior : BehaviorBase
    {
        private bool TriedTakingFurnitureToSit = false;

        public override void Update(Companion companion)
        {
            UpdateFollow(companion);
        }

        public void UpdateFollow(Companion companion)
        {
            Entity Target = companion.Target;
            Entity Owner = companion.Owner;
            if(Target != null) return;
            Vector2 Center = companion.Center;
            Vector2 OwnerPosition = Owner.Center;
            if(Math.Abs(OwnerPosition.X - Center.X) >= 500 || 
                Math.Abs(OwnerPosition.Y - Center.Y) >= 400)
            {
                companion.Teleport(Owner.Bottom);
            }
            if(Companion.Behaviour_InDialogue || Companion.Behaviour_AttackingSomething)
            {
                TriedTakingFurnitureToSit = false;
                return;
            }
            if (companion.GoingToOrUsingFurniture)
            {
                bool OwnerUsingFurniture = false;
                if(Owner is Player)
                {
                    Player p = (Player)Owner;
                    if(p.sitting.isSitting || p.sleeping.isSleeping)
                        OwnerUsingFurniture = true;
                }
                if(OwnerUsingFurniture) // && MathF.Abs(companion.Center.X - Owner.Center.X) < 8 * 16 && MathF.Abs(companion.Center.Y - Owner.Center.Y) < 6 * 16)
                {
                    return;
                }
                companion.LeaveFurniture();
            }
            TriedTakingFurnitureToSit = false;
            float Distancing = 0;
            if (Owner is Player)
            {
                Player p = (Player)Owner;
                if (!TriedTakingFurnitureToSit)
                {
                    if(p.sitting.isSitting)
                    {
                        TriedTakingFurnitureToSit = true;
                        Point chair = GetClosestChair(p.Bottom);
                        if(chair.X > 0 && chair.Y > 0)
                        {
                            companion.UseFurniture(chair.X, chair.Y);
                            return;
                        }
                    }
                    if(p.sleeping.isSleeping)
                    {
                        TriedTakingFurnitureToSit = true;
                        Point furniture = GetClosestBed(p.Bottom);
                        if(furniture.X > 0 && furniture.Y > 0)
                        {
                            companion.UseFurniture(furniture.X, furniture.Y);
                            return;
                        }
                        furniture = GetClosestChair(p.Bottom);
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
            if(Math.Abs((OwnerPosition.X - Center.X) - Owner.velocity.X) > 40 + Distancing)
            {
                if(OwnerPosition.X < Center.X)
                    companion.MoveLeft = true;
                else
                    companion.MoveRight = true;
            }
            companion.WalkMode = false;
        }
    }
}