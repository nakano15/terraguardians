using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class GoSellLootOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Go Sell Loot";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                c.RunBehavior(new Behaviors.Actions.SellLootAction());
            }
        }
    }
    
    /*public class UseFurnitureOrders : CompanionOrderInterface.CompanionOrderStep //Seems kinda dumb
    {
        public override string Text => "Use Nearby Furniture";

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
    }*/

    public class LiftMeOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Lift me up.";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                Companions[0].RunBehavior(new LiftPlayerBehavior(MainMod.GetLocalPlayer));
            }
        }
    }
    public class FreeControlOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Free Control";

        public override void OnActivate()
        {
            CompanionOrderInterface.ExecuteOrders();
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
            pm.CompanionFreeControl = !pm.CompanionFreeControl;
            Main.NewText("Companion Free Control is " + (pm.CompanionFreeControl ? "enabled. Press movement keys to disable." : "disabled."));
        }
    }
}