using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Fluffles
{
    public class FlufflesDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            if (Main.rand.Next(2) == 0)
                return "(It looks like she's greeting you.)";
            return "(She smiled as she met you.)";
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            if (companion.IsSleeping)
            {
            }
            else if (Main.bloodMoon)
            {
                Mes.Add("(She looks really annoyed, growling at you.)");
                Mes.Add("(She seems to be wanting this night to end as soon as possible.)");
                Mes.Add("(Her face is full of anger, maybe I shouldn't try speaking to her.)");
            }
            else
            {
                Mes.Add("(She's asking if you're okay.)");
                Mes.Add("(She softly hits her paw on her chest a few times after she saw you.)");
                Mes.Add("(She looks relieved for seeing you.)");
                if (Main.raining)
                {
                    Mes.Add("(She looks at the sky, as the rain drops pass through her body.)");
                    Mes.Add("(Everytime It's raining, she has a sorrow look. Maybe she liked that?)");
                }
                if (Main.eclipse)
                {
                    Mes.Add("(She looks to be on alert.)");
                    Mes.Add("(She seems to be scared for the people around, more than herself.)");
                }
                if (Main.moonPhase == 0)
                {
                    Mes.Add("(She shows you a portrait she carries with her. In It you see a male and a female Terrarian, a TerraGuardian and her.)");
                }
                if (companion.IsUsingToilet)
                {
                    Mes.Add("(She seems embarrassed. I think she wants you to leave the room.)");
                    Mes.Add("(As she notices you, she puts one of her paws on her hip, while with the other she signals for you to leave the room.)");
                }
                if (HasCompanionSummoned(CompanionDB.Rococo))
                {
                    Mes.Add("(She's looking at [gn:" + CompanionDB.Rococo + "] with a puzzled face. It's like as if she has seen him before.)");
                    Mes.Add("(She seems to be trying to recall something, after looking at [gn:" + CompanionDB.Rococo + "].)");
                }
                if (HasCompanionSummoned(CompanionDB.Blue))
                {
                    if (HasCompanionSummoned(CompanionDB.Sardine))
                    {
                        Mes.Add("(She seems to be asking [gn:" + CompanionDB.Blue + "] if they're going to play again.)");
                        Mes.Add("([gn:" + CompanionDB.Sardine + "] begun panicking as [gn:" + CompanionDB.Blue + "] and [gn:" + CompanionDB.Fluffles + "] stares at him, with a grim smile.)");
                    }
                    else if (CanTalkAboutCompanion(CompanionDB.Sardine))
                    {
                        Mes.Add("([gn:" + CompanionDB.Blue + "] and her must be scheming something. I wonder if that's related to [gn:"+CompanionDB.Sardine+"].)");
                    }
                }
                if (HasCompanionSummoned(CompanionDB.Sardine))
                {
                    Mes.Add("(She's looking at [gn:" + CompanionDB.Sardine + "] while pretending to be biting something. He starting backing off after noticing that.)");
                    Mes.Add("([gn:" + CompanionDB.Sardine + "] seems to be wanting to run away.)");
                }
                if (HasCompanionSummoned(CompanionDB.Zacks))
                {
                    Mes.Add("(She's staring at [gn:" + CompanionDB.Zacks + "]. She seems to be full of thoughts after looking at him. What could she be thinking about?)");
                }
                if (HasCompanionSummoned(CompanionDB.Nemesis))
                {
                    Mes.Add("([gn:" + CompanionDB.Nemesis + "] stares at her, while she stares at [gn:"+CompanionDB.Nemesis+"]. I don't think anything will come out of this.)");
                }
                if (HasCompanionSummoned(CompanionDB.Alex))
                {
                    Mes.Add("(She's petting [gn:" + CompanionDB.Alex + "] in the head. He seems to be enjoying It.)");
                    Mes.Add("(She threw a bone for [gn:" + CompanionDB.Alex + "] to pick. After brought It back, she petted him.)");
                }
                if (HasCompanionSummoned(CompanionDB.Leopold))
                {
                    Mes.Add("(She gave a scare on [gn:" + CompanionDB.Leopold + "]. Now I should be trying to find some leaves.)");
                    Mes.Add("([gn:" + CompanionDB.Leopold + "] seems to be trying to avoid being surprised by her.)");
                    Mes.Add("(She seems to be blushing, and avoiding to stare directly at [gn:"+CompanionDB.Leopold+"].)");
                }
                if (HasCompanionSummoned(CompanionDB.Mabel))
                {
                    Mes.Add("(She looks at [gn:" + CompanionDB.Mabel + "] up and down, then she looked at her own body. Is she trying to compare them?)");
                    Mes.Add("(She tries posing like [gn:" + CompanionDB.Mabel + "], but by the way she shook her head, It seems like she didn't liked the idea.)");
                }
                if (HasCompanionSummoned(CompanionDB.Vladimir))
                {
                    Mes.Add("(As she looke at [gn:" + CompanionDB.Vladimir + "], he opened his arms, which made her go hug him. She seems to be crying.)");
                }
                if (HasCompanionSummoned(CompanionDB.Michelle))
                {
                    Mes.Add("([gn:" + CompanionDB.Michelle + "] is playing with her. They seems to be liking It.)");
                    Mes.Add("([gn:" + CompanionDB.Michelle + "] tried petting her, but the hand passed through her body.)");
                }
                if (HasCompanionSummoned(CompanionDB.Glenn))
                {
                    Mes.Add("(She smiles and waves at [gn:" + CompanionDB.Glenn + "]. He also waved back at her while smiling.)");
                    Mes.Add("(It seems like both her and [gn:" + CompanionDB.Glenn + "] are great friends.)");
                }
                if (HasCompanionSummoned(CompanionDB.Cinnamon))
                {
                    Mes.Add("(She stares at [gn:" + CompanionDB.Cinnamon + "], then shows a soft smile.)");
                }
                if (IsPlayerRoomMate())
                {
                    Mes.Add("(She seems really happy for having you as her room mate.)");
                    Mes.Add("(She seems to be trying to apologize for the scare she gave you on the last time you slept.)");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(She seems to be telling that likes having your company.)");
            Mes.Add("(She shows you a portrait she carries with her, having a male and a female terrarian, a terraguardian, and her. She seems to be pointing at the terraguardian in the image. Maybe that was her boyfriend, or husband?)");
            Mes.Add("(She looks thoughtful.)");
            Mes.Add("(She touched your heart, then smiled at you, and then pulled the paw.)");
            Mes.Add("(She gave you a hug, which your felt, as she smiled while having her eyes closed.)");
            Mes.Add("(She kneels on the floor and stares at you for a while.)");
            Mes.Add("(She leaned on your shoulder for a while.)");
            Mes.Add("(She seems to be thanking you for breaking her haunt.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.Next(2) == 0)
                        return "(After you asked, she shook her head.)";
                    return "(She waves the pointing finger left and right.)";
                case RequestContext.HasRequest:
                    {
                        //if (guardian.request.Base is TravelRequestBase)
                        //    return "(She seems to be feeling lonelly, maybe she wants to [objective]?)";
                        //else
                        {
                            if (Main.rand.Next(2) == 0) return "(After you asked if she had a request, she pulled a list of things she wants you to do. The list contains this: [objective])";
                            return "(She seems to be happy after you asked that, then gave you a list of things she needs. She asks you to: [objective])";
                        }
                    }
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "(After you reported the progress, she looked very joyful.)";
                    return "(Upon receiving what she asked, she gave you a thankful hug.)";
                case RequestContext.Accepted:
                    return "(She nods at you. She seems to be counting on you.)";
                case RequestContext.TooManyRequests:
                    return "(She seems to have noticed that you have many things to do, and decided not to give you her request.)";
                case RequestContext.Rejected:
                    return "(She looked a bit sad after you rejected.)";
                case RequestContext.PostponeRequest:
                    return "(She's looking at you with a question mark face.)";
                case RequestContext.Failed:
                    return "(Her face shows the disappointment your failure brought, but she seems to recognize that you did your best.)";
                case RequestContext.AskIfRequestIsCompleted:
                    return "(She looks a bit excited. I think she knows you completed her request.)";
                case RequestContext.CancelRequestAskIfSure:
                    return "(She looks at you like as if didn't believed what you said. You're really wanting to cancel her request?)";
                case RequestContext.CancelRequestYes:
                    return "(She lowers her head, then shook It slowly side ways.)";
                case RequestContext.CancelRequestNo:
                    return "(She got a smile on her face after hearing that.)";
                case RequestContext.RemindObjective:
                    return "(After you told her that you forgot what she asked, she drew in the ground what she asked of you: [objective].)";
            }
            return base.RequestMessages(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "(She's snoring gently.)";
                        case 1:
                            return "(She seems to be having a peaceful rest.)";
                        case 2:
                            return "(You wonder what she may be dreaming of, to make her sleep so calmly.)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    return "(She woke up, then yawned, and is looking at you with a sleepy face.)";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "(She jumped out of the bed, and is now looking at you excited, wanting to know if you did what she asked for.)";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.RevivingMessage:
                    List<string> Mes = new List<string>();
                    if (target == companion.Owner)
                    {
                        Mes.Add("(Tear drops are falling on your body.)");
                        Mes.Add("(She's trying to ease your pain.)");
                        Mes.Add("(She's trying to make you comfortable on the floor.)");
                    }
                    else
                    {
                        if (target is Companion && (target as Companion).GetCompanionID.IsSameID(CompanionDB.Leopold))
                        {
                            Mes.Add("(She seems to be trying her best to reanimate him.)");
                            Mes.Add("(She seems to be crying while attempting to revive him.)");
                            Mes.Add("(She seems to be trying to make him comfortable, while healing his wounds.)");
                        }
                        else
                        {
                            Mes.Add("(She's trying to soothe the one she's tending.)");
                            Mes.Add("(She's trying to reduce the pain.)");
                            Mes.Add("(She seems to be tending the wounds.)");
                        }
                    }
                    return Mes[Main.rand.Next(Mes.Count)];
                case ReviveContext.HelpCallReceived:
                    return "(You feel a friendly presence tending you.)";
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "(She's rushing at the fallen ally.)";
                case ReviveContext.RevivedByItself:
                    return "";
                case ReviveContext.ReviveWithOthersHelp:
                    return "";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "(She's trying to comfort the fallen person, as she helps tending their wounds.)";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "(She seems to be waiting for your questions.)";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "(She's waiting to see if you want to ask anything else.)";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "(She nods to you.)";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "(She jumps out of excitement.)";
                case JoinMessageContext.FullParty:
                    return "(She looks at the size of your group, and then denies.)";
                case JoinMessageContext.Fail:
                    return "(She doesn't seems very interessed in joining your group right now.)";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "(She seems to be worried about the place you want her to leave the group. Are you sure you want to do that?)";
                case LeaveMessageContext.Success:
                    return "(She nods and then gives you a farewell.)";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "(She leaves your team with a sad look in her face.)";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "(She smiled after you said no.)";
                case LeaveMessageContext.Fail:
                    return "(She doesn't seems inclined into leaving your group right now.)";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "(She doesn't seems to mind carrying you.)";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "(She got on your shoulders.)";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "(She seems to be waiting for you to tell who to carry.)";
                case MountCompanionContext.Fail:
                    return "(She refuses.)";
                case MountCompanionContext.NotFriendsEnough:
                    return "(She doesn't feel okay with that.)";
                case MountCompanionContext.SuccessCompanionMount:
                    return "(She got on their shoulders.)";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "(She placed you on the ground, gently.)";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "(She got off your shoulders.)";
                case DismountCompanionContext.Fail:
                    return "(She doesn't want to let you go right now.)";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "(She allowed, and initiated the bond-merge with you.)";
                case ControlContext.SuccessReleaseControl:
                    return "(She released the bond-merge with you, as you asked.)";
                case ControlContext.FailTakeControl:
                    return "(She doesn't seems to want to do that now.)";
                case ControlContext.FailReleaseControl:
                    return "(She refused to break the bond-merge.)";
                case ControlContext.NotFriendsEnough:
                    return "(She seems to be pretending to not have heard you say that.)";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "(She reminds you that she's there, should you need her.)";
                        case 1:
                            return "(She wonders if you're fine.)";
                        case 2:
                            return "(She's looking directly at you.)";
                    }
                case ControlContext.GiveCompanionControl:
                    return "(She seems okay with that.)";
                case ControlContext.TakeCompanionControl:
                    return "(She gave control back to you.)";
            }
            return base.ControlMessage(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "(After you asked that, her eyes widened, and she seems to be waiting to assure you want to be her buddy. I must remind that once picking a buddy, I cannot change or remove my buddy.)";
                case BuddiesModeContext.PlayerSaysYes:
                    return "(She doesn't knows how to react for you to choosing her as buddy, she looks happy for the choice.)";
                case BuddiesModeContext.PlayerSaysNo:
                    return "(She seems to be trying to hide her disappointment.)";
                case BuddiesModeContext.NotFriendsEnough:
                    return "(She seems awkward with that question. Maybe we're not friends enough for that.)";
                case BuddiesModeContext.Failed:
                    return "(She denied.)";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "(She's finding it weird that you asked her to be your buddy, when you already got one.)";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldMessage1:
                    return "(Her face turned red while looking at the Rabbit Guardian. Her facial expression also changed, she seems a bit shy.)";
                case MessageIDs.LeopoldMessage2:
                    return "*Is that... AAAAAHHH!! A ghost!!! Help!*";
                case MessageIDs.LeopoldMessage3:
                    return "(After he screamed, she looked at you with a sad face, and your character tried comforting her, while you looked at him with an angry face.)";
                case MessageIDs.LeopoldEscapedMessage:
                    return "(She looks a bit disappointed that he ran away...)";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "(She seems to not be able to believe what is happening.)";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}