using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class ChargeOnTargetOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("GoMeleeOrder");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                c.TacticsOverride = CombatTactics.CloseRange;
            }
        }
    }
    
    public class AvoidContactOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("GoRangedOrder");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                c.TacticsOverride = CombatTactics.MidRange;
            }
        }
    }
    
    public class AttackFromFarOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("GoDistanceOrder");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                c.TacticsOverride = CombatTactics.LongRange;
            }
        }
    }

    public class StickCloseOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("GoStickCloseOrder");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                c.TacticsOverride = CombatTactics.StickClose;
            }
        }
    }

    public class FreeWillOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("FreeWillOrder");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                c.TacticsOverride = null;
            }
        }
    }
}