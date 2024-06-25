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
        bool HoldingLeopold = false;

        public CatchLeopoldBehavior(TerraGuardian Leopold)
        {
            if (!Leopold.IsSameID(CompanionDB.Leopold))
            {
                Deactivate();
                return;
            }
            this.Leopold = Leopold;
        }

        public override void ChangeLobbyDialogueOptions(MessageDialogue Message, out bool ShowCloseButton)
        {
            ShowCloseButton = true;
            if (HoldingLeopold)
            {
                Message.AddOption("I wanted to talk with " + Leopold.GetNameColored() + ".", OnAskToTalkToLeopold);
                Message.AddOption("Place " + Leopold.GetNameColored() + " on the ground.", OnAskToPlaceLeopoldOnTheGround);
            }
        }

        void OnAskToTalkToLeopold()
        {
            if (Leopold != null)
                Dialogue.StartDialogue(Leopold);
        }

        void OnAskToPlaceLeopoldOnTheGround()
        {
            if (Leopold != null)
            {
                if (Leopold.IsRunningBehavior && Leopold.GetGoverningBehavior() is not terraguardians.Companions.Leopold.HeldByBlueBehavior)
                {
                    Leopold.GetGoverningBehavior().Deactivate();
                }
                Deactivate();
                Dialogue.LobbyDialogue("*Fine...*");
            }
        }

        
        public override void Update(Companion companion)
        {
            if (Leopold == null || !Leopold.active || Leopold.dead || Leopold.Owner != null)
            {
                Deactivate();
                return;
            }
            if (HoldingLeopold)
            {
                if (!Leopold.IsRunningBehavior || Leopold.GetGoverningBehavior() is not terraguardians.Companions.Leopold.HeldByBlueBehavior)
                {
                    Deactivate();
                    return;
                }
                companion.GetGoverningBehavior(false).Update(companion);
            }
            else
            {
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
                    HoldingLeopold = true;
                    //Deactivate();
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
        }

        public override void WhenKOdOrKilled(Companion companion, bool Died)
        {
            Deactivate();
        }
    }
}