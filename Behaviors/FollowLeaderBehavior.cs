using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class FollowLeaderBehavior : BehaviorBase
    {
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
                return;
            float Distancing = 0;
            if (Owner is Player)
            {
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