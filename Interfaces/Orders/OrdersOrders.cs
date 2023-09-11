using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class FollowOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Follow me";

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
        public override string Text => "Wait here";

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
        public override string Text => "Guard here";

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
        public override string Text => "Go Ahead or Behind";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                
            }
        }
    }
    
    public class AvoidCombatOrder : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Avoid Combat";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                
            }
        }
    }
}