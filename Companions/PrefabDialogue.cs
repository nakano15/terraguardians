using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Prefabs
{
    public class PrefabDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            return "";
        }
        
        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            return "";
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return "";
                case RequestContext.HasRequest: //[objective] tag useable for listing objective
                    return "";
                case RequestContext.Completed:
                    return "";
                case RequestContext.Accepted:
                    return "";
                case RequestContext.TooManyRequests:
                    return "";
                case RequestContext.Rejected:
                    return "";
                case RequestContext.PostponeRequest:
                    return "";
                case RequestContext.Failed:
                    return "";
                case RequestContext.AskIfRequestIsCompleted:
                    return "";
                case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                    return ""; 
                case RequestContext.CancelRequestAskIfSure:
                    return "";
                case RequestContext.CancelRequestYes:
                    return "";
                case RequestContext.CancelRequestNo:
                    return "";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "";
                case MoveInContext.Fail:
                    return "";
                case MoveInContext.NotFriendsEnough:
                    return "";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "";
                case MoveOutContext.Fail:
                    return "";
                case MoveOutContext.NoAuthorityTo:
                    return "";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "";
                case JoinMessageContext.Fail:
                    return "";
                case JoinMessageContext.FullParty:
                    return "";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "";
                case LeaveMessageContext.Fail:
                    return "";
                case LeaveMessageContext.AskIfSure:
                    return "";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "";
                case MountCompanionContext.Fail:
                    return "";
                case MountCompanionContext.NotFriendsEnough:
                    return "";
                case MountCompanionContext.SuccessCompanionMount:
                    return "";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "";
                case DismountCompanionContext.Fail:
                    return "";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "";
            return "";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "";
            return "";
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    return "";
                case SleepingMessageContext.OnWokeUp:
                    return "";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "";
                case TacticsChangeContext.Nevermind:
                    return "";
                case TacticsChangeContext.FollowAhead:
                    return "";
                case TacticsChangeContext.FollowBehind:
                    return "";
                case TacticsChangeContext.AvoidCombat:
                    return "";
                case TacticsChangeContext.PartakeInCombat:
                    return "";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "";
                case ControlContext.SuccessReleaseControl:
                    return "";
                case ControlContext.FailTakeControl:
                    return "";
                case ControlContext.FailReleaseControl:
                    return "";
                case ControlContext.NotFriendsEnough:
                    return "";
                case ControlContext.ControlChatter:
                    return "";
                case ControlContext.GiveCompanionControl:
                    return "";
                case ControlContext.TakeCompanionControl:
                    return "";
            }
            return base.ControlMessage(companion, context);
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "";
                case UnlockAlertMessageContext.MountUnlock:
                    return "";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "";
                case InteractionMessageContext.Accepts:
                    return "";
                case InteractionMessageContext.Rejects:
                    return "";
                case InteractionMessageContext.Nevermind:
                    return "";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "";
                case ChangeLeaderContext.Failed:
                    return "";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "";
                case BuddiesModeContext.PlayerSaysYes:
                    return "";
                case BuddiesModeContext.PlayerSaysNo:
                    return "";
                case BuddiesModeContext.NotFriendsEnough:
                    return "";
                case BuddiesModeContext.Failed:
                    return "";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "";
                case InviteContext.SuccessNotInTime:
                    return "";
                case InviteContext.Failed:
                    return "";
                case InviteContext.CancelInvite:
                    return "";
                case InviteContext.ArrivalMessage:
                    return "";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "";
                case ReviveContext.RevivingMessage:
                    {
                        return "";
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "";
                case ReviveContext.RevivedByItself:
                    return "";
                case ReviveContext.ReviveWithOthersHelp:
                    return "";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}