using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;

namespace terraguardians.Interfaces.Orders
{
    public class OrderOption : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Orders";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenNewOrderList(CompanionOrderInterface.OrderOrders);
        }
    }

    public class ActionOption : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Actions";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenNewOrderList(CompanionOrderInterface.ActionOrders);
        }
    }
    public class ItemOption : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Items";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenNewOrderList(CompanionOrderInterface.ItemOrders);
        }
    }
    public class InteractionOption : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Interactions";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenNewOrderList(CompanionOrderInterface.InteractionOrders);
        }
    }
    public class TacticsOption : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Tactics";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenNewOrderList(CompanionOrderInterface.TacticsOrders);
        }
    }
    public class PullCompanionsOption : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Pull Companion";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
                c.BePulledByPlayer();
        }
    }
}