using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Behaviors.Orders
{
    public class GuardHereBehavior : BehaviorBase
    {
        public Vector2 WaitingLocation = Vector2.Zero;
        int TimeAway = 0;

        public override void Update(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake) return;
            if (WaitingLocation.X == 0)
            {
                if (companion.Owner != null)
                    WaitingLocation = companion.Owner.Bottom;
                else
                    WaitingLocation = companion.Bottom;
            }
            bool InCombat = companion.TargettingSomething;
            float Distance = WaitingLocation.X - companion.Bottom.X;
            float MaxDistance = InCombat ? 48 : 16;
            companion.MoveRight = false;
            companion.MoveLeft = false;
            if (Math.Abs(Distance) > MaxDistance)
            {
                if (Distance > 0)
                {
                    companion.MoveRight = true;
                    companion.MoveLeft = false;
                }
                else
                {
                    companion.MoveLeft = true;
                    companion.MoveRight = false;
                }
                TimeAway++;
                if (TimeAway >= 300)
                {
                    companion.Teleport(WaitingLocation);
                    TimeAway = 0;
                }
            }
            else
            {
                TimeAway = 0;
            }
        }
    }
}