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
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {

        }
    }
    
    public class UseFurnitureOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Use Nearby Furniture";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {

        }
    }
    public class LiftMeOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Lift me up.";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {

        }
    }
    public class FreeControlOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Free Control";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {

        }
    }
}