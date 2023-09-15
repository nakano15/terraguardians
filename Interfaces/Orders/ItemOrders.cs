using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class UseBuffsOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Use Buff Potions";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach(Companion c in Companions)
            {
                c.RunBehavior(new Behaviors.Actions.UseBuffPotionsAction());
            }
        }
    }
}