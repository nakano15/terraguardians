using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class FollowOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("FollowMe");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                c.CancelBehavior();
            }
        }
    }
    
    public class WaitOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("WaitHere");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                c.RunBehavior(new WaitHereBehavior());
            }
        }
    }

    public class GuardOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("GuardHere");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                c.RunBehavior(new GuardHereBehavior());
            }
        }
    }

    public class GoAheadOrBehindOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("FollowAheadOrBehind");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                c.Data.FollowAhead = !c.Data.FollowAhead;
            }
        }
    }
    
    public class AvoidCombatOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("AvoidCombat");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                c.Data.AvoidCombat = !c.Data.AvoidCombat;
            }
        }
    }
}