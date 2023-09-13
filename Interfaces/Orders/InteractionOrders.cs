using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class MountOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Mount/Dismount Companion";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }
    
    public class PlayerControlOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Control Companion";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }
    public class SetLeaderOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Set as Leader";

        public override void OnActivate()
        {
            
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            
        }
    }
}