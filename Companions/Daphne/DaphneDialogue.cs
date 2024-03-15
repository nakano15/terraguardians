using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class DaphneDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            return "(As she fixes her eyes at you, she approaches and snifs you. After doing so, she started to wag her tail and bark.)";
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*She's not staring at you*");
            }
            else
            {
                Mes.Add("Bark! Bark! *Wagging tail*");
                Mes.Add("*Chasing her own tail.*");
                Mes.Add("Bark! *She seems happy to see you.*");
                if (!Main.dayTime)
                {
                    Mes.Add("Grrr.... *She's growling at something outside.*");
                    Mes.Add("Yaaaawn~");
                }
                if (Main.moonPhase == 0)
                    Mes.Add("Awoooooooooo~!!");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*You petted her, and then she lied down on the floor, wanting her belly rubbed.*");
            Mes.Add("*She starts licking your face out of happiness.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch (context)
            {
                case RequestContext.NoRequest:
                    return "Bark! Bark! (She looks okay right now.)";
                case RequestContext.HasRequest:
                    return "(She's staring at you, attentiously. She seems to be wanting... [objective]..?)";
                case RequestContext.Completed:
                    return "(She started licking you, she seems really happy.)";
                case RequestContext.Rejected:
                case RequestContext.Failed:
                    return GetWhining;
                case RequestContext.TooManyRequests:
                    return "(You just reminded that you have a lot on your hands right now.)";
                case RequestContext.PostponeRequest:
                    return "(She leaned her head slightly sideways)";
                case RequestContext.AskIfRequestIsCompleted:
                    return GetBark;
                case RequestContext.CancelRequestAskIfSure:
                    return "Bark? (She wonders if you really want to cancel her request.)";
                case RequestContext.CancelRequestYes:
                    return GetWhining;
                case RequestContext.CancelRequestNo:
                    return GetBark;
            }
            return base.RequestMessages(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.RevivingMessage:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "Whine... Whine... Whine...";
                        case 1:
                            return "(Licking the injured person)";
                        case 2:
                            return "... Bark! Bark! Whine... Whine....";
                    }
                case ReviveContext.HelpCallReceived:
                case ReviveContext.ReviveWithOthersHelp:
                    return GetBark;
                case ReviveContext.ReachedFallenAllyMessage:
                case ReviveContext.RevivedByItself:
                    return GetWhining;
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            return GetBark;
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            return GetWhining;
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "Wuf? (Are you sure you want to pick her as your buddy?)";
                case BuddiesModeContext.PlayerSaysYes:
                    return "Bark! Bark! /*Pant pant/* She seems very happy.";
                case BuddiesModeContext.PlayerSaysNo:
                    return "Whine, whine..";
                case BuddiesModeContext.NotFriendsEnough:
                    return "Ruh? (Maybe you two aren't that much friends enough for that.)";
                case BuddiesModeContext.Failed:
                    return GetWhining;
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "(Don't I have a buddy already? She's even looking at your buddy wondering that too.)";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return GetAuf;
                case ChangeLeaderContext.Failed:
                    return GetWhining;
            }
            return base.ChangeLeaderMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch (context)
            {
                case ControlContext.SuccessTakeControl:
                case ControlContext.SuccessReleaseControl:
                case ControlContext.GiveCompanionControl:
                case ControlContext.TakeCompanionControl:
                    return GetAuf;
                case ControlContext.FailTakeControl:
                case ControlContext.FailReleaseControl:
                    return GetWhine;
                case ControlContext.NotFriendsEnough:
                    return "Whine.. Whine... (Seems like you're not friends enough for that.)";
                case ControlContext.ControlChatter:
                    return GetBark;
            }
            return base.ControlMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return GetBark;
                case DismountCompanionContext.Fail:
                    return GetWhine;
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return GetRuh;
                case InteractionMessageContext.Accepts:
                    return GetAuf;
                case InteractionMessageContext.Rejects:
                    return GetWhine;
                case InteractionMessageContext.Nevermind:
                    return GetAuf;
            }
            return base.InteractionMessages(companion, context);
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch (context)
            {
                case InviteContext.Success:
                case InviteContext.SuccessNotInTime:
                case InviteContext.ArrivalMessage:
                    return GetBark;
                case InviteContext.Failed:
                case InviteContext.CancelInvite:
                    return GetWhine;
            }
            return base.InviteMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return GetBark;
                case JoinMessageContext.FullParty:
                    return GetWhine + " (Too many people following you.)";
                case JoinMessageContext.Fail:
                    return GetWhine;
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return GetBark;
                case LeaveMessageContext.Fail:
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return GetWhine;
                case LeaveMessageContext.AskIfSure:
                    return GetRuh;
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch (context)
            {
                case MountCompanionContext.Success:
                case MountCompanionContext.SuccessMountedOnPlayer:
                case MountCompanionContext.SuccessCompanionMount:
                    return GetAuf;
                case MountCompanionContext.Fail:
                    return GetWhine;
                case MountCompanionContext.NotFriendsEnough:
                    return GetWhine + " (Not friends enough for that.)";
                case MountCompanionContext.AskWhoToCarryMount:
                    return GetRuh + " (Who she should carry?)";
                }
            return base.MountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            return GetBark;
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            return GetBark;
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    return "(Yep, she's asleep.)";
                case SleepingMessageContext.OnWokeUp:
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    if (Main.rand.Next(2) == 0)
                        return "Yaawn~ Bark? ";
                    return "(As she wakes up, she stares at you and shakes her tail.)";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch (context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return GetRuh;
                default:
                    return GetBark;
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            return GetBark;
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            return GetAuf;
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch (Context)
            {
                
            }
            return base.GetOtherMessage(companion, Context);
        }

        string GetBark => "Bark! Bark!";
        string GetAuf => "Auf!";
        string GetRuh => "Ruh?";
        string GetGrowling => "Grrr!";
        string GetHowl => "Awooo!!";
        string GetWhine => GetWhining;
        string GetWhining => "Whine.. Whine.. Whine..";
    }
}