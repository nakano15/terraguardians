using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Malisha
{
    public class MalishaDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "*Oh, a Terrarian. I think I may have a use for you.*";
                case 1:
                    return "*Funny, I thought here was a naturalist colony, but you're wearing clothes. Well, whatever. I may hang around here for a while.*";
                default:
                    return "*You're really small, my neck aches a bit trying to look at you. Say, would you mind participating of some experiments?*";
            }
        }
        
        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I once tried to conjure demons, that's when I had to leave the first village I lived in hurry.*");
            Mes.Add("*Always wondered why you get stronger by using specific kinds of outfits? Well, me too.*");
            Mes.Add("*It's not easy being a prodigy, but when you're one, you have to keep working hard to continue being.*");
            Mes.Add("*I have perfect control of my magic! Now, at least. Let's not revive past experiences.*");
            Mes.Add("*Nobody really complained about my experiments. Here.*");

            Mes.Add("*Well, I could go to the Ether Realm to get some of my clothes, but It's too much work...*");
            Mes.Add("*You ever wondered why the TerraGuardians have no clothes in this world? Well, I never, that's the main reason I came here.*");
            Mes.Add("*I feel like people actually look directly into my \'mana depository\' when they look at me.*");
            Mes.Add("*I'm a bit disappointed that this isn't a naturalist colony like I initially thought, but I'm glad I can do my experiments here.*");
            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("*Mwahahaha! You have triggered my trap card!*");
                Mes.Add("*Don't worry, this wont hurt a bit.*");
                Mes.Add("*How did you know that I needed someone for my experiment?*");
                Mes.Add("*Good, I was needing someone for my brain transplant machine. Ready to turn into a Squirrel?*");
                Mes.Add("*I got you! Now drink this potion!*");
            }
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*No, those creatures didn't came from my lab.*");
                    Mes.Add("*Interesting. Would you mind catching one of those creatures alive for my researches?*");
                }
                else
                {
                    Mes.Add("*I really like this time of day, I can find test subjects with ease at this moment, I just need to walk a bit.*");
                    Mes.Add("*Say, would you mind if I messed with your molecular structure? No? Too bad.*");
                }
            }
            else
            {
                Mes.Add("*To reduce the annoyance levels, I try to do quiet experiments during this time, to avoid annoying neighbors of bothering me.*");
                Mes.Add("*I'm glad you came, would you mind sitting on that chair? I will just need to tie your arms and legs afterwards, though.*");
                Mes.Add("*Came visit me? Or did someone sent you? Because I'm pretty sure someone must have been annoyed by my experiments.*");
                Mes.Add("*What's with those Demon Eyes? It's like as they didn't see a TerraGuardian before.*");
                Mes.Add("*When a Demon Eye charges on someone, isn't supposed that they would get hurt too?*");
            }
            if (Main.raining)
            {
                Mes.Add("*I really love It when the rain drips through my body.*");
                Mes.Add("*Yes! Keep on raining! Bring It on!*");
                Mes.Add("*I love rain, but there is 95% chance I'll have a serious case of flu after It ends.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide))
            {
                Mes.Add("* [nn:" + Terraria.ID.NPCID.Guide + "] got really pale when he saw me doing experiments with a doll that looks like him.*");
                Mes.Add("*Say, do you know why [nn:" + Terraria.ID.NPCID.Guide + "] is on flames?*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Wizard))
            {
                Mes.Add("*I tried to cast a conjuration spell with [nn:" + Terraria.ID.NPCID.Wizard + "] once, we ended up spawning a rain of Corrupt Bunnies.*");
                Mes.Add("*I tend to share my work with [nn:" + Terraria.ID.NPCID.Wizard + "] sometimes, at least one wont blame the other if something explodes.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
            {
                Mes.Add("* [nn:" + Terraria.ID.NPCID.Stylist + "] says that wants to do magic with my hair, but I sense that her magic level is 0.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
            {
                Mes.Add("*Can you tell [nn:" + Terraria.ID.NPCID.Dryad + "] that I don't need a baby sitter? If the fauna suddenly tries to eat you alive is because... Well, probably not my fault.*");
                Mes.Add("* [nn:" + Terraria.ID.NPCID.Dryad + "] says that I'm a living sign of bad omen. No matter what she says, I will keep experimenting.*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
            {
                Mes.Add("*I wonder if [nn:" + Terraria.ID.NPCID.Clothier + "] could make me an outfit that doesn't bother me while wearing It.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*I tried to analyze [gn:" + CompanionDB. Rococo + "]'s intelligence once, I got a NotANumber Exception Error at line 411.*"); //Refferences the GreetMessage script from Rococo.
                Mes.Add("*Sometimes I wonder that [gn:" + CompanionDB. Rococo + "] is like a link between this world and Ether Realm. That may probably be wrong.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*[gn:" + CompanionDB. Blue + "] seems a bit bothered for having another girl in the town.*");
                Mes.Add("*It seems like [gn:" + CompanionDB. Blue + "] got really interessed on a spell I discovered, of turning others into humanoid bunnies. Watch your back.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*I could have tried using a spell of turning someone into a giant on [gn:" + CompanionDB. Sardine + "], but I don't think someone would be happy of having a Rowdy Avatar Cait Sith around.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Zacks))
            {
                Mes.Add("*Interesting what happened to [gn:" + CompanionDB. Zacks + "], I wonder if that isn't related to... Uh... Nevermind.*");
                Mes.Add("*Impressive, [gn:" + CompanionDB. Zacks + "] not only is a walking dead, but also a sentient one... Hm...*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Alex))
            {
                Mes.Add("*[gn:" + CompanionDB. Alex + "] doesn't really look like a TerraGuardian, I wonder if the creator has something to do with him.*");
                Mes.Add("*[gn:" + CompanionDB. Alex + "] keeps talking about Alex Old Partner, I never ever heard about her, or him. Touche?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Nemesis))
            {
                Mes.Add("*I like having [gn:" + CompanionDB. Nemesis + "] around, at least he doesn't look with angry eyes on me.*");
                Mes.Add("*You say that [gn:" + CompanionDB. Nemesis + "] willingly joined your travel after defeating It's armor? Do you know what are the chances of that happening? About 1%!*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*It's not really hard to find test subjects in this world, I just need to tell [gn:" + CompanionDB. Brutus + "] that I need some help with something.*");
                Mes.Add("*For someone who claims to be a bodyguard, [gn:" + CompanionDB. Brutus + "] main weakness is women. Gladly I know how to use that to my advantage.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Bree))
            {
                Mes.Add("*I'd hate having [gn: " + CompanionDB.Bree + "] as a neighbor. If I wanted to hear complaints about my experiments, I would have remained on the Ether Realm.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*[gn:" + CompanionDB. Mabel + "] seems to have some kind of effect on male people in this world. That's actually interesting, I would have several test subjects with It.*");
                Mes.Add("*I heard from [gn:" + CompanionDB. Mabel + "] that she's trying to participate of some kind of contest, and she asked if I didn't had anything to grow Antleers on her head, to possibly increase the chances of entering It.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                Mes.Add("*It's interesting to see someone interessed in my experiments, [gn:" + CompanionDB. Domino + "] buys them from me often for resale. At least I got someone to fund my experiments.*");
                Mes.Add("*Some may call my researchs junk, [gn:" + CompanionDB. Domino + "], calls them profit.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*Interesting having [gn:" + CompanionDB. Leopold + "] around, I could torment him with my experiments.*");
                Mes.Add("*[gn:" + CompanionDB. Leopold + "] and I have quite a story back then, he call me an example of how not to research magic. That doesn't stops me of using what I learn on him.*");
                Mes.Add("*[gn:" + CompanionDB. Leopold + "] is my mentor, I wouldn't say that I'm his best studdent, even more when I test what I learned on him.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*[gn:" + CompanionDB. Vladimir + "] comes from a lineage of warriors, but he seems to be the opposite of his parents. Would he mind if I did a research to investigate why?*");
                Mes.Add("*Whenever I need to process my thoughts, I visit [gn:" + CompanionDB. Vladimir + "]. I tried talking about them with him, but he looked uninteressed, so I remain quiet.*");
                Mes.Add("*Aaack! I fell asleep when processing my thoughts with [gn:" + CompanionDB. Vladimir + "]! I must get back to researching.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*Everytime [gn:" + CompanionDB. Michelle + "] comes bother me, I transform her into a different critter.*");
                Mes.Add("*[gn:" + CompanionDB. Michelle + "] always arrives just in time I need someone to test my experiments.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Wrath))
            {
                Mes.Add("*Hmph, [gn:" + CompanionDB. Wrath + "] thinks he's safe from me, but my experimenting hunger will eventually reach him. Just he wait.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fear))
            {
                Mes.Add("*Whaaaaaaaaaat? I was just having a friendly chat with [gn:" + CompanionDB. Fear + "], how menacing can that be? Hehe.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*Hmph. It looks like I got concorrence. I got to work harder on making my mentor's life not be easy.*");
                Mes.Add("*I wonder if [gn:" + CompanionDB. Fluffles + "] would mind If I tested a vaccuum I've created on her.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*[gn:" + CompanionDB. Minerva + "] seems to be a good test subject, but she's been rejecting my requests for food, so It's hard to lure her...*");
                Mes.Add("*It's not my fault that [gn:" + CompanionDB. Minerva + "] is fat. Now, if there's any other collateral effects... Maybe...*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                Mes.Add("*That kid, [gn:" + CompanionDB. Glenn + "], always manages to escape from me... I mean... Never accepts my invites.*");
                Mes.Add("*I really would like [gn:" + CompanionDB. Glenn + "] to participate of a little experiment... But how could I bypass his luck..?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*Beef guy is really useful for me and my body, so I will not try anything on him.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("*Having [gn:" + CompanionDB. Green + "] around is really useful for me. At least I have infinite supply of... Nevermind, you don't need to know.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I wont burn your town to cinder, If that's what's on your mind.*");
            Mes.Add("*After I told the people on the town I was living before that I was going away for a vacation, a party has started in the town. I think they were wishing me good luck.*");
            Mes.Add("*I feel so alive when testing things on living things. It is unfortunate if they end up no longer living after the testing.*");
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*Don't tell [gn:" + CompanionDB.Leopold + "], but I love having his company. He also helps me with my experiments, even though he clearly doesn't want.*");
                Mes.Add("*I tried several times to earn [gn:" + CompanionDB.Leopold + "]'s respect, but he always complains of my methods, so I no longer care about that.*");
                Mes.Add("*I really love scaring [gn:" + CompanionDB.Leopold + "], I even have a stack of leaves for when I take It too far.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Zacks))
            {
                Mes.Add("*I may be wrong, but the moment [gn:" + CompanionDB.Zacks + "] died was perfect. Well, he could have died for good if wasn't.*");
                Mes.Add("*Maybe I have something to do with what happened to [gn:" + CompanionDB.Zacks + "], but I may be wrong. Just try not to tell anyone about that.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*[gn:" + CompanionDB.Blue + "], seems a bit too obsessed with the bunny transformation spell. What she could possibly used It on?*");
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("*I'm impressed at how [gn:" + CompanionDB.Blue + "] still loves [gn:" + CompanionDB.Zacks + "]. Tell me when something bad happen to them, I would like to analyze their brains while It's still fresh.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return "*I have something else to experiment on right now.*";
                case RequestContext.HasRequest: //[objective] tag useable for listing objective
                    return "*I need to feel the air hit my fur, and also some blood too. Can you [objective]?*";
                case RequestContext.Completed:
                    return "*Thank you! Time for the experiment. I hope this time It doesn't explodes.*";
                case RequestContext.Accepted:
                    return "*Good, that will keep me alone with my experiments. Try not to come back too soon.*";
                case RequestContext.TooManyRequests:
                    return "*No no no no no. Go deal with your other requests. I can't have you doing a bad job at my request because you're overloaded.*";
                case RequestContext.Rejected:
                    return "*Pft. Fine. Go away, now.*";
                case RequestContext.PostponeRequest:
                    return "*No no no, come back here.*";
                case RequestContext.Failed:
                    return "*You what?! Now, try thinking of reasons as to why I shouldn't turn you into a squirrel.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*I like the look in your face. You completed my request, right?*";
                case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                    return "*I really want to examine your brain right now, but I still need you to do my request, so here it is again: I need you to [objective]. Copied that?*"; 
                case RequestContext.CancelRequestAskIfSure:
                    return "*Wait, you came to me, and said that wont do what I asked for? Are you really sure?*";
                case RequestContext.CancelRequestYes:
                    return "*Okay, you're relieved. Get out of my sight, NOW! Before I decide to do something to you.*";
                case RequestContext.CancelRequestNo:
                    return "*The clock is ticking, [nickname].*";
            }
            return RequestMessages(companion, context);
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
                    return "*Yes, I may end up finding guinea pigs for my experiments on the way.*";
                case JoinMessageContext.Fail:
                    return "*No. I have many experiments to do.*";
                case JoinMessageContext.FullParty:
                    return "*I hate mobs.*";
            }
            return JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*I found the walking pleasant. Please call me again in the future, I may find new guinea pigs on the trip.*";
                case LeaveMessageContext.Fail:
                    return "*Okay, I think you are able to think rationally sometimes.*";
                case LeaveMessageContext.AskIfSure:
                    return "*You want to leave me here in the wilds? Are you out of your mind?*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*You're going to regret that when I see you at the town.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "";
            }
            return LeaveGroupMessages(companion, context);
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
                    return "(She's speaking about many different things, in a different language.)";
                case SleepingMessageContext.OnWokeUp:
                    return "*I really should transform you into something for waking me up. Say It, what do you want?*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*The only reason why you didn't turned into a toad, is because I'm really waiting for my request.*";
            }
            return SleepingMessage(companion, context);
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