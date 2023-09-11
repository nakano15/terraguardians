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

        public override void Update(Companion companion)
        {
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
            }
        }
    }
}