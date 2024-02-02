using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class GoSellLootOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("GoSellLoot");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            foreach (Companion c in Companions)
            {
                if (!c.IsBeingControlledBySomeone)
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
        public override string Text => GetTranslation("LiftMeUp");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(true);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                foreach(Companion c in Companions)
                {
                    if (c.IsBeingControlledBySomeone || c.IsMountedOnSomething) continue;
                    c.RunBehavior(new LiftPlayerBehavior(MainMod.GetLocalPlayer, c));
                    break;
                }
            }
        }
    }
    public class FreeControlOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("FreeControl");

        public override void OnActivate()
        {
            CompanionOrderInterface.ExecuteOrders();
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            PlayerMod pm = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>();
            pm.CompanionFreeControl = !pm.CompanionFreeControl;
            if (pm.GetCompanionControlledByMe != null)
            {
                if (pm.CompanionFreeControl)
                    pm.GetCompanionControlledByMe.SaySomething(pm.GetCompanionControlledByMe.GetDialogues.ControlMessage(pm.GetCompanionControlledByMe, ControlContext.GiveCompanionControl));
                else
                    pm.GetCompanionControlledByMe.SaySomething(pm.GetCompanionControlledByMe.GetDialogues.ControlMessage(pm.GetCompanionControlledByMe, ControlContext.TakeCompanionControl));
            }
            if (pm.CompanionFreeControl)
                Main.NewText(GetTranslation("FreeControlEnableMes"));
            else
                Main.NewText(GetTranslation("FreeControlDisableMes"));
        }
    }
}