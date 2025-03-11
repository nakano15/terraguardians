using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Behaviors.Orders
{
    public class WaitHereBehavior : IdleBehavior
    {
        public Vector2 WaitingLocation = Vector2.Zero;

        public override void Update(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake || (companion.reviveBehavior is ReviveBehavior && (companion.reviveBehavior as ReviveBehavior).TryingToReviveSomeone)) return;
            if (WaitingLocation.X == 0)
            {
                if (companion.Owner != null)
                    WaitingLocation = companion.Owner.Bottom;
                else
                    WaitingLocation = companion.Bottom;
            }
            if (companion.TargettingSomething)
                return;
            float Distance = WaitingLocation.X - companion.Bottom.X;
            companion.MoveRight = false;
            companion.MoveLeft = false;
            if (Math.Abs(Distance) > 160)
            {
                if (Distance > 0)
                {
                    companion.MoveRight = true;
                }
                else
                {
                    companion.MoveLeft = true;
                }
            }
            else if (Math.Abs(Distance) > 86)
            {
                if (Distance > 0)
                {
                    if (companion.direction < 0)
                        companion.direction *= -1;
                }
                else
                {
                    if (companion.direction > 0)
                        companion.direction *= -1;
                }
            }
            UpdateIdle(companion, true);
        }
    }
}