using terraguardians;
using Terraria;
using System.Collections.Generic;

namespace terraguardians.Personalities
{
    public class ToughPersonality : PersonalityBase
    {
        public override string Name => "Tough";

        public ToughPersonality() : base(MainMod.GetMod)
        {
            
        }

        public class ToughPersonalityDialogues : CompanionDialogueContainer
        {
            public override string GreetMessages(Companion companion)
            {
                return "I wasn't expecting to meet someone new, even more non aggressive one. I'm [name], what's your name?";
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
                            return "No. I don't need anything from you now.";
                        return "I need nothing right now.";
                    case RequestContext.HasRequest: //[objective] tag useable for listing objective
                        return "Hm. You might have a use for me. I need you to [objective] for me. Would you do that?";
                    case RequestContext.Completed:
                        return "Nice job. You're a tough one too, huh?";
                    case RequestContext.Accepted:
                        return "Great. Return to me with the news.";
                    case RequestContext.TooManyRequests:
                        return "You have quite a lot on your shoulders, don't you?";
                    case RequestContext.Rejected:
                        return "Seems like it's too much for you.";
                    case RequestContext.PostponeRequest:
                        return "Another time? Okay, if you say so.";
                    case RequestContext.Failed:
                        return "Grr. Why did I even bothered asking you to do that?";
                    case RequestContext.AskIfRequestIsCompleted:
                        return "What is it, [nickname]? Did what I asked of you?";
                    case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                        return "Memory failing you? I asked you to [objective]. Can you remember that now?"; 
                    case RequestContext.CancelRequestAskIfSure:
                        return "I gave you the request thinking you would do it. Will you really drop it?";
                    case RequestContext.CancelRequestYes:
                        return "Next time, don't accept what you're unable to complete.";
                    case RequestContext.CancelRequestNo:
                        return "Great. Return to me with news of my request.";
                }
                return base.RequestMessages(companion, context);
            }

            public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
            {
                switch(context)
                {
                    case MoveInContext.Success:
                        return "You want to have me around? Sure, I can move in here. I hope you have a house for me.";
                    case MoveInContext.Fail:
                        return "Sorry. Can't do that now.";
                    case MoveInContext.NotFriendsEnough:
                        return "Maybe if I knew better who invites me to live here, I would.";
                }
                return base.AskCompanionToMoveInMessage(companion, context);
            }

            public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
            {
                switch(context)
                {
                    case MoveOutContext.Success:
                        return "Fine. You can keep your stinky house anyways.";
                    case MoveOutContext.Fail:
                        return "You can't kick me out now.";
                    case MoveOutContext.NoAuthorityTo:
                        return "What? I can't hear you. Maybe I don't respect you.";
                }
                return base.AskCompanionToMoveOutMessage(companion, context);
            }

            public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
            {
                switch(context)
                {
                    case JoinMessageContext.Success:
                        return "It's about time. Lets go on an adventure.";
                    case JoinMessageContext.Fail:
                        return "Now is not the moment.";
                    case JoinMessageContext.FullParty:
                        return "You've got too many people with you. I can't fit in that group.";
                }
                return base.JoinGroupMessages(companion, context);
            }

            public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
            {
                switch(context)
                {
                    case LeaveMessageContext.Success:
                        return "Sure, that was interesting. Call me again anytime.";
                    case LeaveMessageContext.Fail:
                        return "You wont get rid of me that easily.";
                    case LeaveMessageContext.AskIfSure:
                        return "Are you sure that here is a good place to drop me out of your group? Would be better some town or house.";
                    case LeaveMessageContext.DangerousPlaceYesAnswer:
                        return "If you say so. I will fight my way home then.";
                    case LeaveMessageContext.DangerousPlaceNoAnswer:
                        return "Then better we find a safe place for me to leave party at. From there I can take care of myself.";
                }
                return base.LeaveGroupMessages(companion, context);
            }

            public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
            {
                switch(context)
                {
                    case MountCompanionContext.Success:
                        return "Sure. Hop on.";
                    case MountCompanionContext.SuccessMountedOnPlayer:
                        return "I accept the ride. Don't let me fall.";
                    case MountCompanionContext.Fail:
                        return "Not right now.";
                    case MountCompanionContext.NotFriendsEnough:
                        return "We'd better rely on our feet for now.";
                    case MountCompanionContext.SuccessCompanionMount:
                        return "I'll carry them.";
                    case MountCompanionContext.AskWhoToCarryMount:
                        return "I can do that. Who should I carry?";
                }
                return base.MountCompanionMessage(companion, context);
            }

            public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
            {
                switch(context)
                {
                    case DismountCompanionContext.SuccessMount:
                        return "Back to using your feet.";
                    case DismountCompanionContext.SuccessMountOnPlayer:
                        return "I'll be back to walking then.";
                    case DismountCompanionContext.Fail:
                        return "Not at this moment.";
                }
                return base.DismountCompanionMessage(companion, context);
            }

            public override string OnToggleShareBedsMessage(Companion companion, bool Share)
            {
                if (Share)
                    return "Sure thing. Try not to make me fall off it.";
                return "Then I'll try using another bed instead, when going sleep.";
            }

            public override string OnToggleShareChairMessage(Companion companion, bool Share)
            {
                if (Share)
                    return "Sure thing, I don't mind.";
                return "That's okay by me.";
            }

            public override string SleepingMessage(Companion companion, SleepingMessageContext context)
            {
                switch(context)
                {
                    case SleepingMessageContext.WhenSleeping:
                        return "(You hear the sound of a log being sawed.)";
                    case SleepingMessageContext.OnWokeUp:
                        return "I hope you have a important reason for waking me up.";
                    case SleepingMessageContext.OnWokeUpWithRequestActive:
                        return "I hope you woke me up because you completed my request.";
                }
                return base.SleepingMessage(companion, context);
            }

            public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
            {
                switch(context)
                {
                    case TacticsChangeContext.OnAskToChangeTactic:
                        return "Need me to change how I fight? How do you think I should approach combat?";
                    case TacticsChangeContext.ChangeToCloseRange:
                        return "I'll be taking on foes in close range, then.";
                    case TacticsChangeContext.ChangeToMidRanged:
                        return "I'll keep an acceptable distance from my foes, then.";
                    case TacticsChangeContext.ChangeToLongRanged:
                        return "I'll avoid getting too close to my target while attacking them from far away, then.";
                    case TacticsChangeContext.ChangeToStickClose:
                        return "Fine! Let them come then.";
                    case TacticsChangeContext.Nevermind:
                        return "Yes, my combat style is perfect, isn't it?";
                    case TacticsChangeContext.FollowAhead:
                        return "I'll lead the way, then.";
                    case TacticsChangeContext.FollowBehind:
                        return "Fine, you're the boss.";
                    case TacticsChangeContext.AvoidCombat:
                        return "Fine, I'll avoid hostility.";
                    case TacticsChangeContext.PartakeInCombat:
                        return "Back to blood spilling.";
                    case TacticsChangeContext.AllowSubattackUsage:
                        return "Alright. I will use my special attacks whenever I see fit.";
                    case TacticsChangeContext.UnallowSubattackUsage:
                        return "Alright. Do let me know when I should use my special attacks.";
                    case TacticsChangeContext.GenericWillDo:
                        return "Yes, I'll do that.";
                    case TacticsChangeContext.GenericWillNotDo:
                        return "I will stop doing that, then.";
                }
                return base.TacticChangeMessage(companion, context);
            }

            public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
            {
                switch(context)
                {
                    case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                        return "You want to speak with me? Alright, do let me know what about.";
                    case TalkAboutOtherTopicsContext.AfterFirstTime:
                        return "Anything else you want to know about?";
                    case TalkAboutOtherTopicsContext.Nevermind:
                        return "Okay.";
                }
                return base.TalkAboutOtherTopicsMessage(companion, context);
            }

            public override string ControlMessage(Companion companion, ControlContext context)
            {
                switch(context)
                {
                    case ControlContext.SuccessTakeControl:
                        return "We shall face our foes as one, [nickname].";
                    case ControlContext.SuccessReleaseControl:
                        return "I release you from the bond-merge. I hope you achieved your goal, [nickname].";
                    case ControlContext.FailTakeControl:
                        return "Now is not the moment.";
                    case ControlContext.FailReleaseControl:
                        return "I can't release you right now, [nickname].";
                    case ControlContext.NotFriendsEnough:
                        return "I don't trust you enough for that.";
                    case ControlContext.ControlChatter:
                        switch(Main.rand.Next(3))
                        {
                            default:
                                return "Need my suggestion? Maybe we should bash something.";
                            case 1:
                                return "I'm still right here.";
                            case 2:
                                return "Taking a break from beating up monsters?";
                        }
                    case ControlContext.GiveCompanionControl:
                        return "I'll take over from now then. Let me know if you change your mind.";
                    case ControlContext.TakeCompanionControl:
                        return "Control returns to you.";
                }
                return base.ControlMessage(companion, context);
            }

            public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
            {
                switch(context)
                {
                    case UnlockAlertMessageContext.MoveInUnlock:
                        return "Hey [nickname], this place actually look nice. Do you mind if I moved in here? Let me know if you don't.";
                    case UnlockAlertMessageContext.ControlUnlock:
                        return "[nickname], I wanted to let you know that I wont mind if we bond-merge together, so feel free to ask me that anytime you need.";
                    case UnlockAlertMessageContext.FollowUnlock:
                        return "Hey [nickname], seems like you've been exploring interesting places in your adventure. Let me know if you're going on another one, I'd like to have some things to kill.";
                    case UnlockAlertMessageContext.MountUnlock:
                        if (companion.Base.MountStyle == MountStyles.CompanionRidesPlayer)
                            return "Hey, how strong is your back? Would you mind carrying me? No, my feet are not sore, if that's what you think.";
                        return "Hey [nickname], are your feet sore? I wont mind carrying you, if you ask me.";
                    case UnlockAlertMessageContext.RequestsUnlock:
                        return "Hey, if you don't mind, I might need some help with some requests. Do check me out some times to see if I have any.";
                    case UnlockAlertMessageContext.BuddiesModeUnlock:
                        return "I think I know you enough to tell you this: You can appoint me as your Buddy if you want, and I would accept such a honor heartfelt. Just remember that's a for life thing, by the way.";
                    case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                        return "Now that we're newly buddies, know that I wont question most of what you ask of me, so feel free to ask. If you ask something dumb, on the other hand, I will bonk your head.";
                }
                return base.UnlockAlertMessages(companion, context);
            }

            public override string InteractionMessages(Companion companion, InteractionMessageContext context)
            {
                switch(context)
                {
                    case InteractionMessageContext.OnAskForFavor:
                        return "Need my help on something? What is it?";
                    case InteractionMessageContext.Accepts:
                        return "Sure. I'll do it.";
                    case InteractionMessageContext.Rejects:
                        return "Uh... No.";
                    case InteractionMessageContext.Nevermind:
                        return "Fine.";
                }
                return base.InteractionMessages(companion, context);
            }

            public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
            {
                switch(context)
                {
                    case ChangeLeaderContext.Success:
                        return "I'll lead your followers then.";
                    case ChangeLeaderContext.Failed:
                        return "I'm not leaving my place.";
                }
                return "";
            }

            public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
            {
                switch(context)
                {
                    case BuddiesModeContext.AskIfPlayerIsSure:
                        return "Ah, so you finally asked me that. Do know that once you pick a Buddy, you can't leave them, neither can change them. Do you still want to pick me as a Buddy?";
                    case BuddiesModeContext.PlayerSaysYes:
                        return "Then I accept too. Lets make our buddiship be the best of both realms.";
                    case BuddiesModeContext.PlayerSaysNo:
                        return "Got scared of the for life part? Heh.";
                    case BuddiesModeContext.NotFriendsEnough:
                        return "I'd like to hear that from someone I liked more.";
                    case BuddiesModeContext.Failed:
                        return "Not now.";
                    case BuddiesModeContext.AlreadyHasBuddy:
                        return "You've got a buddy already, [nickname].";
                }
                return "";
            }

            public override string InviteMessages(Companion companion, InviteContext context)
            {
                switch(context)
                {
                    case InviteContext.Success:
                        return "Sure, I'll be there soon. Just you wait.";
                    case InviteContext.SuccessNotInTime:
                        return "I will show up there tomorrow, if you don't mind.";
                    case InviteContext.Failed:
                        return "I don't feel like visiting now.";
                    case InviteContext.CancelInvite:
                        return "Don't need me there anymore? Fine then.";
                    case InviteContext.ArrivalMessage:
                        return "I'm here, [nickname]. What is that important thing you wanted to talk to me about?";
                }
                return "";
            }

            public override string GetOtherMessage(Companion companion, string Context)
            {
                switch(Context)
                {
                    case MessageIDs.LeopoldEscapedMessage:
                        return "Well, that was quite a show.";
                    case MessageIDs.VladimirRecruitPlayerGetsHugged:
                        return "And I can't believe I've been following that person.";
                    case MessageIDs.RPSAskToPlaySuccess:
                        return "Rock, Paper and Scissors? That kids game? Fine.. If you say so.";
                    case MessageIDs.RPSAskToPlayFail:
                        return "I have something else keeping me busy right now.";
                    case MessageIDs.RPSCompanionWinMessage:
                        return "Haha! I won!";
                    case MessageIDs.RPSCompanionLoseMessage:
                        return "What?! I lost?!";
                    case MessageIDs.RPSCompanionTieMessage:
                        return "Hm. Neither of us won this time.";
                    case MessageIDs.RPSPlayAgainMessage:
                        return "Another? Sure. I don't mind.";
                    case MessageIDs.RPSEndGameMessage:
                        return "Now that that's out of the way, anything else you want to talk about.";
                }
                return base.GetOtherMessage(companion, Context);
            }

            public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
            {
                switch(context)
                {
                    case ReviveContext.HelpCallReceived:
                        return "Don't worry! I'm coming for your rescue.";
                    case ReviveContext.RevivingMessage:
                        {
                            return "Try not to breath too hard.";
                        }
                    case ReviveContext.OnComingForFallenAllyNearbyMessage:
                        return "I'm coming for you. Hang on!";
                    case ReviveContext.ReachedFallenAllyMessage:
                        return "I'm here, now lets heal those wounds.";
                    case ReviveContext.RevivedByItself:
                        return "I'm back.. Ow...";
                    case ReviveContext.ReviveWithOthersHelp:
                        return "Thanks. I'm still a bit sore, but I'll survive.";
                }
                return base.ReviveMessages(companion, target, context);
            }

            public override string VisitingMessages(Companion companion, bool AllowedToVisit)
            {
                if (AllowedToVisit)
                    return "Ah, nice! You'll be seeing my face more frequently then.";
                return "I guess you don't want to see my face here. Very well, I will stop visiting you.";
            }
            
            public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
            {
                Weight = 1f;
                return "It's always great to meet new people.";
            }

            public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
            {
                Weight = 1f;
                return "Nice. More people to watch me kick behinds.";
            }

            public override string CompanionLeavesGroupMessage(Companion WhoReacts, Companion WhoLeft, out float Weight)
            {
                Weight = 1f;
                return "Leaving already? Alright, see ya.";
            }
        }
    }
}