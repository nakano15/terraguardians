using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class LeonaDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*I guess I'll never get tired of seeing Terrarians. Hello, I'm Leona.*";
                case 1:
                    return "*Look at what I found here. The name's Leona. Need something killed?*";
                case 2:
                    return "*Oh, a friendly face! Happy to meet me? I'm Leona, Swordswoman.*";
            }
        }
        
        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            bool LeonaHasSword = (companion as Leona.LeonaCompanion).HoldingSword;
            if (companion.IsUsingToilet)
            {
                if (LeonaHasSword)
                    Mes.Add("*You sure that you want to speak with me right now? Not only the smell isn't good, but I also have a long range sword.*");
                Mes.Add("*Couldn't you find a more improper moment to talk with me?*");
                Mes.Add("*I was having problems doing my business here, and you staring at me isn't helping either.*");
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*Heeey shortie, came to check on me?*");
                    Mes.Add("*What is it? Someone bullying you? Tell me who and I will take care of them.*");
                    Mes.Add("*I was expecting to see you.*");
                    Mes.Add("*I'm not fat! This is just stocking for whenever I get no food for days. Wait, that's fat...*");

                    Mes.Add("*My time as a Royal Guard is over, even more since a member I liked to mess with left for unknown reasons.*");
                    Mes.Add("*I think you would like visiting the Ether Realm. At least, as long as I'm with you, you'd be safe there.*");
                    Mes.Add("*I actually like this place. The Terrarians here are really nice to me. I just need to be careful about where I walk to.*");

                    if (Main.dayTime)
                    {
                        if (Main.raining)
                        {
                            Mes.Add("*Mr Raindrop, falling away from me now...*");
                            Mes.Add("*Do you like rainy weather too? It make me kind of drowzy, but I'll survive.*");
                            Mes.Add("*Oh, look at you: Dripping. Need a towel? I don't have one.*");
                        }
                        else
                        {
                            Mes.Add("*My beautiful fur will only get beautier with sunlight help.*");
                            Mes.Add("*Are you here to get some tanning too? Great!*");
                            if (LeonaHasSword)
                                Mes.Add("*It's being quite hard to wield the sword. Its metal is getting hotter.*");
                        }
                    }
                    else
                    {
                        if (Main.raining)
                        {
                            Mes.Add("*I hope it keeps raining while I'm asleep.*");
                            Mes.Add("*What a nice surprise. Yes, we can chat for a while.*");
                        }
                        Mes.Add("*I can't wait for the moment of hitting the bed...*");
                        Mes.Add("*Oh, you came visit me. Could you be brief? I'm going to bed soon.*");
                        Mes.Add("*A nice meal and then I'm ready for bed.*");
                    }
                    if (LeonaHasSword)
                    {
                        Mes.Add("*This sword? It doesn't weight on me that much.*");
                        Mes.Add("*Are you trying to keep distance from me? Don't worry, I don't plan on using this on you.*");
                        Mes.Add("*Sadly, as a part of my old job, I got the habit of keeping my weapon ready all the time.*");
                    }
                    else
                    {
                        Mes.Add("*I feel defenseless without my sword. Oh, you shouldn't have heard that.*");
                        Mes.Add("*It's so odd to have so much freedom of movement with my right arm. What could I do with it..?*");
                        Mes.Add("*Where I stored my sword? Why do you want to know? You think you can lift it? Someday you'll have to let me see you lift it, haha.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Blue))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Blue+"] keeps picking on me because of my belly. She always forgets that I have a huge sword.*");
                        if (player.Male)
                            Mes.Add("*If you had to choose between [gn:"+CompanionDB.Blue+"] and me, who you'd pick? I'm just asking, there's no particular reason..*");
                        if (CanTalkAboutCompanion(CompanionDB.Zack))
                            Mes.Add("*I guess [gn:"+CompanionDB.Blue+"] has something more important to mind than my belly, even more with that boyfriend of hers looking at people like they're walking beefs.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Alex))
                    {
                        Mes.Add("*If I didn't had enough responsibility already, I'd adopt [gn:"+CompanionDB.Alex+"].*");
                        Mes.Add("*Do you even know that "+AlexRecruitmentScript.AlexOldPartner+" [gn:"+CompanionDB.Alex+"] talks about? No? I guess it wasn't your fault her death, then?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Brutus))
                    {
                        Mes.Add("*I wasn't actually expecting to see [gn:"+CompanionDB.Brutus+"] here. I guess fate made us bump into each other again.*");
                        Mes.Add("*How I know [gn:"+CompanionDB.Brutus+"]? I was also a Royal Guard, just like him. Lets say that I'm more into offensive and he's into defensive.*");
                        Mes.Add("*I love teasing [gn:"+CompanionDB.Brutus+"] every now and then. I just like seeing him mad.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Domino))
                    {
                        Mes.Add("*Ah, so [gn:"+CompanionDB.Domino+"] is here too? He managed to cause serious headaches to the guards in the Ether Realm. Hopefully he wont do the same here.*");
                        Mes.Add("*Do let me know if [gn:"+CompanionDB.Domino+"]'s presence here is hazardous. I wouldn't mind having a talk with him, if you know what I mean.*");
                        if (CanTalkAboutCompanion(CompanionDB.Brutus))
                        {
                            Mes.Add("*If you knew how many times [gn:"+CompanionDB.Brutus+"] got yelled for failing at capturing [gn:"+CompanionDB.Domino+"]... Lets say that turned them into nemesis...*");
                            Mes.Add("*I once offered myself to capture [gn:"+CompanionDB.Domino+"], but [gn:"+CompanionDB.Brutus+"] intervened, saying that this matter was personal.");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Leopold))
                    {
                        Mes.Add("*It seems like this world is getting more and more popular for TerraGuardians, huh? Even [gn:"+CompanionDB.Leopold+"] joined in.*");
                        Mes.Add("*Magic, magic, magic. Only I feel satisfied with slicing things in half?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Mabel))
                    {
                        Mes.Add("*I'm glad I don't get as much attention from people like [gn:"+CompanionDB.Mabel+"] does. I really dislike crowding, but she seems to not mind.*");
                    }
                    bool GlennMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Glenn),
                        SardineMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Sardine),
                        BreeMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Bree);
                    if (CanTalkAboutCompanion(CompanionDB.Glenn))
                    {
                        if (!SardineMet && !BreeMet)
                        {
                            Mes.Add("*How irresponsible could someone be to leave their cub living alone in a house?! Once we find [gn:"+CompanionDB.Glenn+"]'s parents, I will have a SERIOUS conversation with them.*");
                        }
                        else if (BreeMet && !SardineMet)
                        {
                            Mes.Add("*I wonder where could be [gn:"+CompanionDB.Glenn+"]'s idiotic father. Even his husband is looking for him, and he's nowhere to be found.*");
                        }
                        else if (!BreeMet && SardineMet)
                        {
                            Mes.Add("*So... We found [gn:"+CompanionDB.Glenn+"]'s father, but his mother is missing. Couldn't they make my job easier?*");
                        }
                        else
                        {
                            Mes.Add("*It's really good to have [gn:"+CompanionDB.Glenn+"]'s family finally reunited, but it was really irresponsible of them to leave him alone. At least they are unharmed.*");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Sardine))
                    {
                        if (CanTalkAboutCompanion(CompanionDB.Bree))
                        {
                            Mes.Add("*[gn:"+CompanionDB.Sardine+"]'s house is really noisy. We can hear [gn:"+CompanionDB.Bree+"] and him arguing far from there.*");
                        }
                        Mes.Add("*How did [gn:"+CompanionDB.Sardine+"] managed to get stuck inside... No, nevermind. I don't want to know.*");
                        if (SardineBountyBoard.TalkedAboutBountyBoard)
                            Mes.Add("*I take some of [gn:"+CompanionDB.Sardine+"]'s Bounties sometimes, they help me not get rusty about combat, and make some coins too.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Bree))
                    {
                        if (SardineMet)
                        {
                            Mes.Add("*I sometimes have a chat with [gn:"+CompanionDB.Bree+"]. She spends most of the time speaking of her husband and his stupid things.*");
                            Mes.Add("*I wonder; If I had a husband, what kind of couple we would be? Probably not like [gn:"+CompanionDB.Bree+"] and [gn:"+CompanionDB.Sardine+", right?*");
                        }
                        else
                        {
                            Mes.Add("*You've also been asked by [gn:"+CompanionDB.Bree+"] to look for her husband? I'm doing that sometimes too. Remember, her husband is a black cat.*");
                        }
                        if (GlennMet)
                        {
                            Mes.Add("*It's so boring whenever [gn:"+CompanionDB.Bree+"] starts saying that [gn:"+CompanionDB.Glenn+"] is her source of pride and stuff. Even more when she compares him to her husband... I heard that, like, a lot.*");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Luna))
                    {
                        Mes.Add("*You didn't looked very surprised when you met me. Has [gn:"+CompanionDB.Luna+"] told you about us?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Celeste))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Celeste+"] is not what I expected from a priestess. She's not preachy.*");
                        Mes.Add("*It's surprising that a priestess from my realm religion appeared here. I wonder if she will ask to build a church for her.*");
                        Mes.Add("*The rite to be a Royal Guard involved also getting the blessing from "+MainMod.TgGodName+".*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                    {
                        Mes.Add("*I find it odd to see [gn:"+CompanionDB.Vladimir+"] hugging about half the village. He doesn't seems to mind that, either. It's just... Odd..*");
                        Mes.Add("*Just how much food [gn:"+CompanionDB.Vladimir+"] can eat? Does he have a hole in his stomach or something?*");
                    }
                }
                else
                {
                    Mes.Add("*Back off! Now's not the time.*");
                    Mes.Add("*You're starting to buzz my patience. What do you want?*");
                    Mes.Add("*Grr... Couldn't you talk to me on a less horrible time?*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I think I've begun growing attached to this world, and its people. Feels like I belong here.*");
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*Don't tell [gn:"+CompanionDB.Brutus+"], but I actually like talking to him.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return "*I like your enthusiasm, but I require nothing right now.*";
                case RequestContext.HasRequest: //[objective] tag useable for listing objective
                    return "*I'm glad that you mentioned that. I need someone to [objective] for me. Would you be that someone?*";
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "*I'm glad to hear that you managed to do it. I guess I shouldn't fear that it would be too much for you.*";
                    return "*Congratulations! You didn't disappointed me. I hope you do more requests for me in the future.*";
                case RequestContext.Accepted:
                    if (Main.rand.Next(2) == 0)
                        return "*Do be careful and do not be reckless when doing my request, okay?*";
                    return "*Try not to get yourself killed doing that.*";
                case RequestContext.TooManyRequests:
                    return "*I'm not a fan of quantity over quality, so I wont add my request to your list for now. Take care of your other tasks first.*";
                case RequestContext.Rejected:
                    if (Main.rand.Next(2) == 0)
                        return "*I guess I don't have the charm I thought I had. Oh well, at least was an attempt.*";
                    return "*Oh well, I guess I should be less lazy and try doing it myself, haha.*";
                case RequestContext.PostponeRequest:
                    return "*Got something more important than helping me? Cold hearted Terrarian, huh? Just kidding, just kidding.*";
                case RequestContext.Failed:
                    return "*You what?! I should have known my request would be too much for you, now I have some mess to clean.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Did you take care of my little request?*";
                case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                    return "*Do Terrarians have short spanned memory? Gladly I don't. I asked you to [objective], can you remember that now?*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*Hahaha, very funny [nickname]. I thought I heard you say you want to drop my request.*";
                case RequestContext.CancelRequestYes:
                    return "*It wasn't a joke...? Oh well... You.. Wasted my time, then..? Fine, you're relieved from my request.*";
                case RequestContext.CancelRequestNo:
                    return "*For a moment I thought it wasn't a joke. I'm so glad it is.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*If you say please, I will. Just kidding, I can stay here. I just hope I have a spacious and comfy house to live.*";
                case MoveInContext.Fail:
                    return "*I don't feel like this is the moment for moving in here.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I don't think this place gave me enough reasons why I should live here, instead of my actual house.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You what? I thought we were friends! Oh well, I'll pack my things then.*";
                case MoveOutContext.Fail:
                    return "*I'm going nowhere now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*You have the audacity to try kicking me out? Who do you think you are?*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*I shall be your sword for a while, then.*";
                case JoinMessageContext.Fail:
                    return "*I don't feel like joining your adventuring group right now.*";
                case JoinMessageContext.FullParty:
                    return "*Isn't there too many people in your group?*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Don't need my company? Fine, then this sword will be sheathed, and go back home.*";
                case LeaveMessageContext.Fail:
                    return "*You're not getting rid of me right now.*";
                case LeaveMessageContext.AskIfSure:
                    return "*Couldn't you ask me that once we got in a safe place?*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I guess that's a no. Well, I'll fight my way back home, then.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Thank you.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Sure that I could use some weight on my shoulder. Might even make me stronger too.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*I hope you wont let me fall.*";
                case MountCompanionContext.Fail:
                    return "*Keep using your own feet for now.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*I could still make use of my other arm, so no.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I'll carry them for you. I hope they don't use something loud on my ears.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*Yes, I can. Who should I carry?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*To the ground you go. It was a good exercise.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Thanks for the ride, but I doubt that was good for my fat. I'm not complaining, I'm not complaining.*";
                case DismountCompanionContext.Fail:
                    return "*You're not getting off my shoulder right now.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Yes, I don't mind but... Your beds are way too small for me, so where would you sleep at?*";
            return "*Maybe it's for the better that we take different beds.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Yes, I can share my chair. Just don't expect me to pet your head.*";
            return "*Fine. Then we take different chairs.*";
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                    case SleepingMessageContext.WhenSleeping:
                        switch(Main.rand.Next(3))
                        {
                            default:
                                return "(She seems to have blacked out, or is what it seems.)";
                            case 1:
                                return "(She's snoring a lot. You wonder how one could sleep with such snoring noises.)";
                            case 2:
                                return "(No kind of sound in the world would possibly wake her up right now.)";
                        }
                case SleepingMessageContext.OnWokeUp:
                    return "*Huh? Yawn~ You have 3 seconds to tell me why you woke me up. If you don't have an actual reason, I'll bonk your head.*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*Wha~ Oh, it's you. I hope it's about my request, or else I'll get violent.*";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*Need me to review how I'll take on in combat? You know I'm better at close range, right?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*Got it. I will give trouble to my foes.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Take some distance then? I guess that's fine.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*I guess I wont really be needing to use much my sword... But yes, I can do that.*";
                case TacticsChangeContext.Nevermind:
                    return "*Everything seems fine for you, then? Okay, then I'll keep taking on combat as previously.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*What is it? There is something specific you want to talk to me about? Or want to know more about me?*";
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

        public override void ManageOtherTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            Leona.LeonaCompanion Leona = (Leona.LeonaCompanion)companion;
            if (Leona.HoldingSword)
            {
                dialogue.AddOption("Stop using your Greatsword.", RemoveSwordDialogue);
            }
            else
            {
                dialogue.AddOption("Use your Greatsword.", EquipSwordDialogue);
            }
        }

        private void EquipSwordDialogue()
        {
            Leona.LeonaCompanion Leona = (Leona.LeonaCompanion)Dialogue.Speaker;
            Leona.HoldingSword = true;
            MessageDialogue md = new MessageDialogue("*I was waiting until you said that. Time to bathe my sword in blood.*");
            md.RunDialogue();
        }

        private void RemoveSwordDialogue()
        {
            Leona.LeonaCompanion Leona = (Leona.LeonaCompanion)Dialogue.Speaker;
            Leona.HoldingSword = false;
            MessageDialogue md = new MessageDialogue("*I really hate that, but I will do so. Do let me know when I should be able to use my sword again.*");
            md.RunDialogue();
        }
    }
}