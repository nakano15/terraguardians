using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Blue
{
    public class CatchLeopoldBehavior : BehaviorBase
    {
        TerraGuardian Leopold;
        ushort AttemptTime = 0;

        public CatchLeopoldBehavior(TerraGuardian Leopold)
        {
            if (!Leopold.IsSameID(CompanionDB.Leopold))
            {
                Deactivate();
                return;
            }
            this.Leopold = Leopold;
        }
        
        public override void Update(Companion companion)
        {
            if (Leopold == null || !Leopold.active || Leopold.dead || Leopold.Owner != null)
            {
                Deactivate();
                return;
            }
            if(AttemptTime == 0)
            {
                companion.CreatePathingTo(Leopold.Bottom, true);
            }
            if (companion.Path.State != PathFinder.PathingState.TracingPath)
            {
                companion.WalkMode = true;
                if (Leopold.Center.X < companion.Center.X)
                {
                    companion.MoveLeft = true;
                }
                else
                {
                    companion.MoveRight = true;
                }
            }
            if (companion.Hitbox.Intersects(Leopold.Hitbox))
            {
                Leopold.RunBehavior(new Leopold.HeldByBlueBehavior((TerraGuardian)companion));
                switch(Main.rand.Next(3))
                {
                    default:
                        Leopold.SaySomething("*Ack! No! Blue!*");
                        break;
                    case 1:
                        companion.SaySomething("*I got you!*");
                        break;
                    case 2:
                        companion.SaySomething("*You can't run away from me this time.*");
                        break;
                }
                Deactivate();
            }
            else
            {
                AttemptTime ++;
                if (AttemptTime >= 10 * 60)
                {
                    Deactivate();
                }
            }
        }

        public override void WhenKOdOrKilled(Companion companion, bool Died)
        {
            Deactivate();
        }
    }
}