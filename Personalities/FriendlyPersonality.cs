using terraguardians;
using Terraria;
using System.Collections.Generic;

namespace terraguardians.Personalities
{
    public class FriendlyPersonality : PersonalityBase
    {
        public override string Name => "Friendly";
        protected override CompanionDialogueContainer SetDialogueContainer => new FriendlyDialogues();

        public class FriendlyDialogues : CompanionDialogueContainer
        {
            public override string GreetMessages(Companion companion)
            {
                return "Hi! It's nice to see someone new.";
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
                        return "I don't need anything right now. Maybe another time I have something to ask of you.";
                    case RequestContext.HasRequest: //[objective] tag useable for listing objective
                        return "I'm so glad you asked, I need your help to [objective] for me. Would you do that?";
                    case RequestContext.Completed:
                        return "Amazing, [nickname]. Take this as my token of appreciation.";
                    case RequestContext.Accepted:
                        return "Please return to me once you've done my request, okay?";
                    case RequestContext.TooManyRequests:
                        return "Oh... You've got too much on your hands right now. Take care of your other requests first.";
                    case RequestContext.Rejected:
                        return "Okay... I'll drop this request then...";
                    case RequestContext.PostponeRequest:
                        return "Another time? Okay. I can ask this another time.";
                    case RequestContext.Failed:
                        return "You failed...? At least you tried...";
                    case RequestContext.AskIfRequestIsCompleted:
                        return "Oh, you're back. Have you completed my request?";
                    case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                        return "Don't worry. I can remind you of my request. I asked you to [objective]."; 
                    case RequestContext.CancelRequestAskIfSure:
                        return "You want to cancel my request? Are you sure?";
                    case RequestContext.CancelRequestYes:
                        return "Aww... It's fine...";
                    case RequestContext.CancelRequestNo:
                        return "Oh, okay. I'll await for your return then.";
                }
                return base.RequestMessages(companion, context);
            }

            public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
            {
                switch(context)
                {
                    case MoveInContext.Success:
                        return "Yes, I can live here. The people here seems nice.";
                    case MoveInContext.Fail:
                        return "Sorry, but not at this moment.";
                    case MoveInContext.NotFriendsEnough:
                        return "You look like a nice person, but I'm not sure if this place is good for me to stay at...";
                }
                return base.AskCompanionToMoveInMessage(companion, context);
            }

            public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
            {
                switch(context)
                {
                    case MoveOutContext.Success:
                        return "Oh... That's sad... I was starting to like living here. I'll pack my things then...";
                    case MoveOutContext.Fail:
                        return "Sorry, but I'm not leaving right now.";
                    case MoveOutContext.NoAuthorityTo:
                        return "A friend let me stay here, and you're trying to boot me out? Why?";
                }
                return base.AskCompanionToMoveOutMessage(companion, context);
            }

            public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
            {
                switch(context)
                {
                    case JoinMessageContext.Success:
                        return "I don't mind joining your adventure. Lets go.";
                    case JoinMessageContext.Fail:
                        return "Sorry, but not right now...";
                    case JoinMessageContext.FullParty:
                        return "There's too many people following you.";
                }
                return base.JoinGroupMessages(companion, context);
            }

            public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
            {
                switch(context)
                {
                    case LeaveMessageContext.Success:
                        return "Okay, I'll go back home then. Safe travels, [nickname].";
                    case LeaveMessageContext.Fail:
                        return "I'm not leaving this group right now.";
                    case LeaveMessageContext.AskIfSure:
                        return "This place look unsafe for leaving the group. You sure you want me to leave the group here?";
                    case LeaveMessageContext.DangerousPlaceYesAnswer:
                        return "Fine... I'll try making my way home then..";
                    case LeaveMessageContext.DangerousPlaceNoAnswer:
                        return "Alright. Lets find a town before I leave the group.";
                }
                return base.LeaveGroupMessages(companion, context);
            }

            public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
            {
                switch(context)
                {
                    case MountCompanionContext.Success:
                        return "Sure, I can carry you.";
                    case MountCompanionContext.SuccessMountedOnPlayer:
                        return "That would be handy.";
                    case MountCompanionContext.Fail:
                        return "Better not at this time.";
                    case MountCompanionContext.NotFriendsEnough:
                        return "Sorry, but no.";
                    case MountCompanionContext.SuccessCompanionMount:
                        return "I'll be carrying them, then.";
                    case MountCompanionContext.AskWhoToCarryMount:
                        return "I don't mind. Who should I carry?";
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
                        return "Alright. Thanks.";
                    case DismountCompanionContext.Fail:
                        return "Better not at this moment.";
                }
                return base.DismountCompanionMessage(companion, context);
            }

            public override string OnToggleShareBedsMessage(Companion companion, bool Share)
            {
                if (Share)
                    return "Sure. Maybe there's enough space for us.";
                return "Okay. I'll find another bed to use then.";
            }

            public override string OnToggleShareChairMessage(Companion companion, bool Share)
            {
                if (Share)
                    return "I don't mind sharing my chair with you.";
                return "I'll find another chair for me when necessary then.";
            }

            public override string SleepingMessage(Companion companion, SleepingMessageContext context)
            {
                switch(context)
                {
                    case SleepingMessageContext.WhenSleeping:
                        return "(They're sleeping while smiling.)";
                    case SleepingMessageContext.OnWokeUp:
                        return "Yawn~. Oh, hello [nickname]. It's late. Need something?";
                    case SleepingMessageContext.OnWokeUpWithRequestActive:
                        return "Yawn~ Oh, hello [nickname]. Did you do my request?";
                }
                return base.SleepingMessage(companion, context);
            }

            public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
            {
                switch(context)
                {
                    case TacticsChangeContext.OnAskToChangeTactic:
                        return "Need me to change how I will fight? What is your suggestion?";
                    case TacticsChangeContext.ChangeToCloseRange:
                        return "Fight things at close range? Alright.";
                    case TacticsChangeContext.ChangeToMidRanged:
                        return "Fight things in Mid Range? Okay.";
                    case TacticsChangeContext.ChangeToLongRanged:
                        return "Fight things away from them? Right.";
                    case TacticsChangeContext.Nevermind:
                        return "Changed your mind? Okay.";
                    case TacticsChangeContext.FollowAhead:
                        return "I'll be following ahead, then.";
                    case TacticsChangeContext.FollowBehind:
                        return "I'll be behind you, then.";
                    case TacticsChangeContext.AvoidCombat:
                        return "I'll avoid attacking things, then";
                    case TacticsChangeContext.PartakeInCombat:
                        return "Okay. I will help in combat again, then.";
                    case TacticsChangeContext.AllowSubattackUsage:
                        return "I'll use my subattack again, then.";
                    case TacticsChangeContext.UnallowSubattackUsage:
                        return "I'll let you pick when I should use subattack, then.";
                }
                return base.TacticChangeMessage(companion, context);
            }

            public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
            {
                switch(context)
                {
                    case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                        return "Yes, we can talk. What do you want to talk about?";
                    case TalkAboutOtherTopicsContext.AfterFirstTime:
                        return "Is there anything else you want to talk about?";
                    case TalkAboutOtherTopicsContext.Nevermind:
                        return "Alright. Anything else you need to speak of?";
                }
                return base.TalkAboutOtherTopicsMessage(companion, context);
            }

            public override string ControlMessage(Companion companion, ControlContext context)
            {
                switch(context)
                {
                    case ControlContext.SuccessTakeControl:
                        return "Let us bond-merge then.";
                    case ControlContext.SuccessReleaseControl:
                        return "I'll release you from bond-merge.";
                    case ControlContext.FailTakeControl:
                        return "No way I'm doing that now.";
                    case ControlContext.FailReleaseControl:
                        return "Sorry, but I'm not releasing you right now.";
                    case ControlContext.NotFriendsEnough:
                        return "I don't trust you enough to that.";
                    case ControlContext.ControlChatter:
                        return "Wanted to speak with me?";
                    case ControlContext.GiveCompanionControl:
                        return "I'll be taking control for now, then. Let me know if you want to take control again.";
                    case ControlContext.TakeCompanionControl:
                        return "Control returned to you. I'll be on standby, if you need something.";
                }
                return base.ControlMessage(companion, context);
            }

            public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
            {
                switch(context)
                {
                    case UnlockAlertMessageContext.MoveInUnlock:
                        return "Hi [nickname]. I just wanted to let you know, that I would like to live here. Speak to me about that if you want me to live here.";
                    case UnlockAlertMessageContext.ControlUnlock:
                        return "I think you're trustworthy, so I'll tell you this: Whenever you need to do something too dangerous for you, I will allow you to bond-merge with me. Just do be careful if you try that.";
                    case UnlockAlertMessageContext.FollowUnlock:
                        return "Hi [nickname]. You seems to be having lots of interesting adventures. I'm interested in making part of it, so if you need help on your travels, feel free to call me.";
                    case UnlockAlertMessageContext.MountUnlock:
                        if(companion.Base.MountStyle == MountStyles.PlayerMountsOnCompanion)
                            return "You look quite worn out, [nickname]. I could carry you if you need to rest your feet, it's no problem for me at all.";
                        return "Hey [nickname], would you mind carrying me on your travels? I can help you with fight while you take care of mobility.";
                    case UnlockAlertMessageContext.RequestsUnlock:
                        return "Say [nickname], would you mind helping me? I have some requests that I need help doing, and I wanted to know if you could help me with them.";
                    case UnlockAlertMessageContext.BuddiesModeUnlock:
                        return "[nickname], I have something serious to tell you: You've been a great friend to me, and I wanted to let you know that, you can pick me as your Buddy for life, if you want to.";
                    case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                        return "Ah [nickname], I forgot to tell you, but since we're Buddies, I have no reasons to question whatever you ask of me. I hope knowing this made you happy.";
                }
                return base.UnlockAlertMessages(companion, context);
            }

            public override string InteractionMessages(Companion companion, InteractionMessageContext context)
            {
                switch(context)
                {
                    case InteractionMessageContext.OnAskForFavor:
                        return "You need my help with something? What is it?";
                    case InteractionMessageContext.Accepts:
                        return "Alright, I will do it.";
                    case InteractionMessageContext.Rejects:
                        return "No, sorry..";
                    case InteractionMessageContext.Nevermind:
                        return "Changed your mind? Okay.";
                }
                return base.InteractionMessages(companion, context);
            }

            public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
            {
                switch(context)
                {
                    case ChangeLeaderContext.Success:
                        return "I'll take the lead, then.";
                    case ChangeLeaderContext.Failed:
                        return "Sorry, I'm not moving out from here.";
                }
                return "";
            }

            public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
            {
                switch(context)
                {
                    case BuddiesModeContext.AskIfPlayerIsSure:
                        return "You're really asking me to be your Buddy? You know this is a thing that lasts for, like, forever, right? You can't undo this. Would you still pick me as your Buddy?";
                    case BuddiesModeContext.PlayerSaysYes:
                        return "Yes! Yes!! I accept [nickname]. I'm so happy that you picked me as your Buddy. Shall our Buddiship last forever, [nickname].";
                    case BuddiesModeContext.PlayerSaysNo:
                        return "Oh... I hope you find someone to be your Buddy, then...";
                    case BuddiesModeContext.NotFriendsEnough:
                        return "I don't know you well enough for that.. What if...";
                    case BuddiesModeContext.Failed:
                        return "Sorry, but no.";
                    case BuddiesModeContext.AlreadyHasBuddy:
                        return "But [nickname], you've got yourself a Buddy. They might even be heartbroken by now.";
                }
                return "";
            }

            public override string InviteMessages(Companion companion, InviteContext context)
            {
                switch(context)
                {
                    case InviteContext.Success:
                        return "Sure, I can visit you. I might show up anytime.";
                    case InviteContext.SuccessNotInTime:
                        return "It's quite late right now, but tomorrow I might be showing up there.";
                    case InviteContext.Failed:
                        return "I can't leave this place right now, sorry.";
                    case InviteContext.CancelInvite:
                        return "You don't need me there anymore? Okay.";
                    case InviteContext.ArrivalMessage:
                        return "I'm here, [nickname]. What did you wanted to speak with me about?";
                }
                return "";
            }

            public override string GetOtherMessage(Companion companion, string Context)
            {
                switch(Context)
                {
                    case MessageIDs.LeopoldEscapedMessage:
                        return "You could have tried to speak with him. This was all clearly a misunderstanding.";
                    case MessageIDs.LeopoldMessage1:
                        return "Uh... Excuse me... What are you talking about?";
                    case MessageIDs.LeopoldMessage2:
                        return "*What do you mean? Aren't you hostage of that Terrarian?*";
                    case MessageIDs.LeopoldMessage3:
                        return "Not at all. They're actually my friend, and I'm just travelling with them. But I think they may be awkward with what just happened.";
                    case MessageIDs.VladimirRecruitPlayerGetsHugged:
                        return "That's really odd looking, but I can't stop watching either.";
                }
                return base.GetOtherMessage(companion, Context);
            }

            public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
            {
                switch(context)
                {
                    case ReviveContext.HelpCallReceived:
                        return "Don't worry, I'm coming!";
                    case ReviveContext.RevivingMessage:
                        return "You'll be fine, I'm taking care of you.";
                    case ReviveContext.OnComingForFallenAllyNearbyMessage:
                        return "No!";
                    case ReviveContext.ReachedFallenAllyMessage:
                        return "I'm here. Try to stay with me.";
                    case ReviveContext.RevivedByItself:
                        return "Ow, my head... I hope that doesn't happen again.";
                    case ReviveContext.ReviveWithOthersHelp:
                        return "Thank you! I'm so happy to have you guys around.";
                }
                return base.ReviveMessages(companion, target, context);
            }
        }
    }
}