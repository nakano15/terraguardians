using System.Collections.Generic;
using Terraria;
using Terraria.UI;
using terraguardians.Interfaces;
using terraguardians.Behaviors.Orders;

namespace terraguardians.Interfaces.Orders
{
    public class MountOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("MountDismount");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(false);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                Companion c = Companions[0];
                if (!c.IsRunningBehavior)
                {
                    if ((c.IsMountedOnSomething || c.PlayerCanMountCompanion(MainMod.GetLocalPlayer)) && c.ToggleMount(MainMod.GetLocalPlayer))
                    {
                        if (c.IsMountedOnSomething)
                            c.SaySomething(c.GetDialogues.MountCompanionMessage(c, c.MountStyle == MountStyles.CompanionRidesPlayer ? MountCompanionContext.SuccessMountedOnPlayer : MountCompanionContext.Success));
                        else
                            c.SaySomething(c.GetDialogues.DismountCompanionMessage(c, c.MountStyle == MountStyles.CompanionRidesPlayer ? DismountCompanionContext.SuccessMountOnPlayer : DismountCompanionContext.SuccessMount));
                    }
                    else
                    {
                        if (!c.IsMountedOnSomething)
                        {
                            if (c.FriendshipLevel < c.Base.GetFriendshipUnlocks.MountUnlock)
                                c.SaySomething(c.GetDialogues.MountCompanionMessage(c, MountCompanionContext.NotFriendsEnough));
                            else
                                c.SaySomething(c.GetDialogues.MountCompanionMessage(c, MountCompanionContext.Fail));
                        }
                        else
                            c.SaySomething(c.GetDialogues.DismountCompanionMessage(c, DismountCompanionContext.Fail));
                    }
                }
            }
        }
    }
    
    public class PlayerControlOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("ControlCompanion");

        public override void OnActivate()
        {
            CompanionOrderInterface.SelectedCompanion = 0;
            CompanionOrderInterface.ExecuteOrders();
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                Companion c = Companions[0];
                if ((c.IsBeingControlledBy(MainMod.GetLocalPlayer) || c.PlayerCanControlCompanion(MainMod.GetLocalPlayer)) && c.TogglePlayerControl(MainMod.GetLocalPlayer))
                {
                    if (!c.IsBeingControlledBySomeone)
                    {
                        c.SaySomething(c.GetDialogues.ControlMessage(c, ControlContext.SuccessTakeControl));
                    }
                    else
                    {
                        c.SaySomething(c.GetDialogues.ControlMessage(c, ControlContext.SuccessReleaseControl));
                    }
                }
                else
                {
                    if (!c.IsBeingControlledBySomeone)
                    {
                        if (c.FriendshipLevel < c.Base.GetFriendshipUnlocks.ControlUnlock)
                            c.SaySomething(c.GetDialogues.ControlMessage(c, ControlContext.NotFriendsEnough));
                        else
                            c.SaySomething(c.GetDialogues.ControlMessage(c, ControlContext.FailTakeControl));
                    }
                    else
                    {
                        c.SaySomething(c.GetDialogues.ControlMessage(c, ControlContext.FailReleaseControl));
                    }
                }
            }
        }
    }
    public class SetLeaderOrders : CompanionOrderInterface.CompanionOrderStep
    {
        public override string Text => GetTranslation("SetAsLeader");

        public override void OnActivate()
        {
            CompanionOrderInterface.OpenCompanionSelectionStep(false);
        }

        public override void FinallyDo(List<Companion> Companions)
        {
            if (Companions.Count > 0)
            {
                Companion c = Companions[0];
                bool CanBeLeader = true;
                if (PlayerMod.PlayerGetControlledCompanion(MainMod.GetLocalPlayer) != null)
                    CanBeLeader = false;
                else
                {
                    CanBeLeader = PlayerMod.PlayerChangeLeaderCompanion(MainMod.GetLocalPlayer, Companions[0]);
                }
                if (CanBeLeader)
                {
                    c.SaySomething(c.GetDialogues.ChangeLeaderMessage(c, ChangeLeaderContext.Success));
                }
                else
                {
                    c.SaySomething(c.GetDialogues.ChangeLeaderMessage(c, ChangeLeaderContext.Failed));
                }
            }
        }
    }
}