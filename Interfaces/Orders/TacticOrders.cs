using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class ChargeOnTargetOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Take on Monsters";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }
    
    public class AvoidContactOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Avoid Contact with Monsters";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }
    
    public class AttackFromFarOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Keep your distance from Monsters";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }

    public class FreeWillOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Free will";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }
}