using terraguardians;
using Terraria;
using System.Collections.Generic;

namespace terraguardians.Personalities
{
    public class NeutralPersonality : PersonalityBase
    {
        public override string Name => "Neutral";

        public class NeutralDialogues : CompanionDialogueContainer
        {
            public override string GreetMessages(Companion companion)
            {
                return "Hello. Are you new here? I am.";
            }
            
            public override string NormalMessages(Companion companion)
            {
                return ".";
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
                        if (Main.rand.Next(2) == 0)
                            return "I don't need anything right now.";
                        return "No. There is nothing I need now.";
                    case RequestContext.HasRequest: //[objective] tag useable for listing objective
                        return "Yes. I need someone to [objective] for me. Would you be up to?";
                    case RequestContext.Completed:
                        return "Thank you. Please, take this reward.";
                    case RequestContext.Accepted:
                        return "Alright. Let me know once you're done.";
                    case RequestContext.TooManyRequests:
                        return "You've got too much on your hands.";
                    case RequestContext.Rejected:
                        return "Okay. I guess I'll scrap this, then.";
                    case RequestContext.PostponeRequest:
                        return "Fine. I'll hold onto this request for now.";
                    case RequestContext.Failed:
                        return "Well, I was really wanting to have that done.";
                    case RequestContext.AskIfRequestIsCompleted:
                        return "Did you do my request?";
                    case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                        return "[objective] is what I asked of you."; 
                    case RequestContext.CancelRequestAskIfSure:
                        return "Are you sure that want to cancel my request?";
                    case RequestContext.CancelRequestYes:
                        return "Okay then. I'll try doing that myself later, then.";
                    case RequestContext.CancelRequestNo:
                        return "Okay, go do it then.";
                }
                return base.RequestMessages(companion, context);
            }

            public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
            {
                switch(context)
                {
                    case MoveInContext.Success:
                        return "Yes, I could live here.";
                    case MoveInContext.Fail:
                        return "No. I'd rather not move in here right now.";
                    case MoveInContext.NotFriendsEnough:
                        return "I hardly even know if it's a good idea to stay here.";
                }
                return base.AskCompanionToMoveInMessage(companion, context);
            }

            public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
            {
                switch(context)
                {
                    case MoveOutContext.Success:
                        return "Fine. I'll pack my things, then.";
                    case MoveOutContext.Fail:
                        return "I will stay here for a bit longer.";
                    case MoveOutContext.NoAuthorityTo:
                        return "You have no authority over this.";
                }
                return base.AskCompanionToMoveOutMessage(companion, context);
            }

            public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
            {
                switch(context)
                {
                    case JoinMessageContext.Success:
                        return "Sure. Lets go.";
                    case JoinMessageContext.Fail:
                        return "No.";
                    case JoinMessageContext.FullParty:
                        return "There's too many people.";
                }
                return base.JoinGroupMessages(companion, context);
            }

            public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
            {
                switch(context)
                {
                    case LeaveMessageContext.Success:
                        return "Fine. I'll go back home then.";
                    case LeaveMessageContext.Fail:
                        return "I wont be leaving right now.";
                    case LeaveMessageContext.AskIfSure:
                        return "Are you sure that want to leave me here?";
                    case LeaveMessageContext.DangerousPlaceYesAnswer:
                        return "I guess I'll have to fight my way home now..";
                    case LeaveMessageContext.DangerousPlaceNoAnswer:
                        return "Good. Lets reach a safe place first.";
                }
                return base.LeaveGroupMessages(companion, context);
            }

            public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
            {
                switch(context)
                {
                    case MountCompanionContext.Success:
                        return "Sure. I can carry you. Lets go.";
                    case MountCompanionContext.SuccessMountedOnPlayer:
                        return "Thanks. Don't let me fall.";
                    case MountCompanionContext.Fail:
                        return "Now doesn't seems like a good moment for that.";
                    case MountCompanionContext.NotFriendsEnough:
                        return "I don't think so.";
                    case MountCompanionContext.SuccessCompanionMount:
                        return "I can carry them.";
                    case MountCompanionContext.AskWhoToCarryMount:
                        return "Sure. Who should I carry?";
                }
                return base.MountCompanionMessage(companion, context);
            }

            public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
            {
                switch(context)
                {
                    case DismountCompanionContext.SuccessMount:
                        return "There you go.";
                    case DismountCompanionContext.SuccessMountOnPlayer:
                        return "Thank you for the ride.";
                    case DismountCompanionContext.Fail:
                        return "Better not do that rightn ow.";
                }
                return base.DismountCompanionMessage(companion, context);
            }

            public override string OnToggleShareBedsMessage(Companion companion, bool Share)
            {
                if (Share)
                    return "I think the bed is big enough for us, anyways.";
                return "I'll use another bed, then.";
            }

            public override string OnToggleShareChairMessage(Companion companion, bool Share)
            {
                if (Share)
                    return "Sure, I can share my chair with you.";
                return "I'll take another chair, then.";
            }

            public override string SleepingMessage(Companion companion, SleepingMessageContext context)
            {
                switch(context)
                {
                    case SleepingMessageContext.WhenSleeping:
                        return "(It seems like they're snoring.)";
                    case SleepingMessageContext.OnWokeUp:
                        return "Uh~ Oh.. Do you need something?";
                    case SleepingMessageContext.OnWokeUpWithRequestActive:
                        return "Uh~ Hm... Is it regarding my request, that you woke me up?";
                }
                return base.SleepingMessage(companion, context);
            }

            public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
            {
                switch(context)
                {
                    case TacticsChangeContext.OnAskToChangeTactic:
                        return "Is something wrong with the way I fight? Fine, what do you suggest?";
                    case TacticsChangeContext.ChangeToCloseRange:
                        return "I'll take foes by close range, then.";
                    case TacticsChangeContext.ChangeToMidRanged:
                        return "I'll try fighting foes by mid range.";
                    case TacticsChangeContext.ChangeToLongRanged:
                        return "I'll keep distance from my foes, then.";
                    case TacticsChangeContext.Nevermind:
                        return "I guess the way I fight seems fine, now.";
                    case TacticsChangeContext.FollowAhead:
                        return "I'll be ahead of you, then.";
                    case TacticsChangeContext.FollowBehind:
                        return "Fine, I'll follow you.";
                    case TacticsChangeContext.AvoidCombat:
                        return "Avoid fighting? Oh.. Okay..";
                    case TacticsChangeContext.PartakeInCombat:
                        return "I can fight again? Good.";
                    case TacticsChangeContext.AllowSubattackUsage:
                        return "I shouldn't use subattacks? Oh well..";
                    case TacticsChangeContext.UnallowSubattackUsage:
                        return "I can use subattacks again? Nice!";
                }
                return base.TacticChangeMessage(companion, context);
            }

            public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
            {
                switch(context)
                {
                    case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                        return "Want to talk with me? What do you want to know?";
                    case TalkAboutOtherTopicsContext.AfterFirstTime:
                        return "There you go. Something else you want to talk about?";
                    case TalkAboutOtherTopicsContext.Nevermind:
                        return "Okay. Any last thing you want to speak with me about?";
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
}