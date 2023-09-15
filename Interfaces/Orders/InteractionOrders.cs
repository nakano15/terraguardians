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
            CompanionOrderInterface.OpenCompanionSelectionStep(false);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                Companions[0].ToggleMount(MainMod.GetLocalPlayer);
            }
        }
    }
    
    public class PlayerControlOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Control Companion";

        public override void OnActivate()
        {
            CompanionOrderInterface.SelectedCompanion = 0;
            CompanionOrderInterface.ExecuteOrders();
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                Companions[0].TogglePlayerControl(MainMod.GetLocalPlayer);
            }
        }
    }
    public class SetLeaderOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => "Set as Leader";

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(false);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                PlayerMod.PlayerChangeLeaderCompanion(MainMod.GetLocalPlayer, Companions[0]);
            }
        }
    }
}