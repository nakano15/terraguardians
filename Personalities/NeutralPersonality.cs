using terraguardians;
using Terraria;
using System.Collections.Generic;

namespace terraguardians.Personalities
{
    public class NeutralPersonality : PersonalityBase
    {
        public override string Name => "Neutral";
        protected override CompanionDialogueContainer SetDialogueContainer => new NeutralDialogues();

        public NeutralPersonality() : base(MainMod.GetMod)
        {
            
        }

        public class NeutralDialogues : CompanionDialogueContainer
        {
            public override string GreetMessages(Companion companion)
            {
                return "Hello. Are you new here? I am. My name is [name].";
            }
            
            public override string NormalMessages(Companion companion)
            {
                return "";
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
                        return "We'll be bond-merged then. Be careful in combat.";
                    case ControlContext.SuccessReleaseControl:
                        return "Alright. I hope this helped you.";
                    case ControlContext.FailTakeControl:
                        return "No.";
                    case ControlContext.FailReleaseControl:
                        return "I don't think it's a good idea to do it right now.";
                    case ControlContext.NotFriendsEnough:
                        return "I don't like you that much, yet.";
                    case ControlContext.ControlChatter:
                        return "Be careful with what you do, [nickname].";
                    case ControlContext.GiveCompanionControl:
                        return "Yeah, Thanks.";
                    case ControlContext.TakeCompanionControl:
                        return "There you go";
                }
                return base.ControlMessage(companion, context);
            }

            public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
            {
                switch(context)
                {
                    case UnlockAlertMessageContext.MoveInUnlock:
                        return "[nickname], I'd like to move in here. Do you have a place I could live? If you want me around?";
                    case UnlockAlertMessageContext.ControlUnlock:
                        return "[nickname], I allow you to bond-merge with me, if you ever need to do something dangerous. Just be careful, alright?";
                    case UnlockAlertMessageContext.FollowUnlock:
                        return "If you need someone to go with you on your adventures, you can call me.";
                    case UnlockAlertMessageContext.MountUnlock:
                        if(companion.Base.MountStyle == MountStyles.PlayerMountsOnCompanion)
                            return "I have news for you [nickname], you no longer need to walk, just hop onto my shoulder.";
                        return "Hey [nickname], I wanted to let you know that you can carry me on your travels, if you need some extra combat suport.";
                    case UnlockAlertMessageContext.RequestsUnlock:
                        return "Hey [nickname], I might need some of your help. Do check if I have any request up sometimes, okay?";
                    case UnlockAlertMessageContext.BuddiesModeUnlock:
                        return "Hello [nickname], I think you are trustworthy, so if you think about appointing me as your Buddy, I will be happy to accept.";
                    case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                        return "Oh [nickname], I forgot to tell you something. Since we are now buddies, there's no reason why not to trust whatever you ask of me, so if you need me to do something, feel free to ask.";
                }
                return base.UnlockAlertMessages(companion, context);
            }

            public override string InteractionMessages(Companion companion, InteractionMessageContext context)
            {
                switch(context)
                {
                    case InteractionMessageContext.OnAskForFavor:
                        return "Do you need my help with something?";
                    case InteractionMessageContext.Accepts:
                        return "I can do that.";
                    case InteractionMessageContext.Rejects:
                        return "No.";
                    case InteractionMessageContext.Nevermind:
                        return "You don't need my help anymore?";
                }
                return base.InteractionMessages(companion, context);
            }

            public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
            {
                switch(context)
                {
                    case ChangeLeaderContext.Success:
                        return "I shall take the lead, then.";
                    case ChangeLeaderContext.Failed:
                        return "No.";
                }
                return base.ChangeLeaderMessage(companion, context);
            }

            public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
            {
                switch(context)
                {
                    case BuddiesModeContext.AskIfPlayerIsSure:
                        return "You want to pick me as your buddy?";
                    case BuddiesModeContext.PlayerSaysYes:
                        return "Then we are now Buddies, [nickname]. Shall our buddiship be long.";
                    case BuddiesModeContext.PlayerSaysNo:
                        return "That's not the kind of thing you should joke about, [nickname].";
                    case BuddiesModeContext.NotFriendsEnough:
                        return "I still don't know you enough for that.";
                    case BuddiesModeContext.Failed:
                        return "No.";
                    case BuddiesModeContext.AlreadyHasBuddy:
                        return "You have a Buddy already, remember?";
                }
                return "";
            }

            public override string InviteMessages(Companion companion, InviteContext context)
            {
                switch(context)
                {
                    case InviteContext.Success:
                        return "Sure, I'll be there soon.";
                    case InviteContext.SuccessNotInTime:
                        return "Sure, but not right now.";
                    case InviteContext.Failed:
                        return "No.";
                    case InviteContext.CancelInvite:
                        return "You don't want me to visit anymore? Fine.";
                    case InviteContext.ArrivalMessage:
                        return "I'm here. What were you want to talk to me about?";
                }
                return "";
            }

            public override string GetOtherMessage(Companion companion, string Context)
            {
                switch(Context)
                {
                    case MessageIDs.LeopoldEscapedMessage:
                        return "I think this all was a misunderstanding, but it's too late now.";
                    case MessageIDs.LeopoldMessage1:
                        return "Save from what? What are you talking about?";
                    case MessageIDs.LeopoldMessage2:
                        return "*Huh? Aren't you being held captive by that Terrarian?*";
                    case MessageIDs.LeopoldMessage3:
                        return "No! I'm just exploring the world with that Terrarian. I'm actually quite sad for them for witnessing that scene you made.";
                    case MessageIDs.VladimirRecruitPlayerGetsHugged:
                        return "Do... You two know each other?";
                }
                return base.GetOtherMessage(companion, Context);
            }

            public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
            {
                switch(context)
                {
                    case ReviveContext.HelpCallReceived:
                        return "Don't worry, I will help you";
                    case ReviveContext.RevivingMessage:
                        return "Hold on, I'm tending your wounds.";
                    case ReviveContext.OnComingForFallenAllyNearbyMessage:
                        return "Hang on, I'll help you.";
                    case ReviveContext.ReachedFallenAllyMessage:
                        return "You're safe now.";
                    case ReviveContext.RevivedByItself:
                        return "Ok, I woke up.";
                    case ReviveContext.ReviveWithOthersHelp:
                        return "Thank you, I appreciate your help.";
                }
                return base.ReviveMessages(companion, target, context);
            }
        }
    }
}