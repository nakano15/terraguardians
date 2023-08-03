using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class MichelleDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Hello! Are you a beginner too?");
            Mes.Add("Oh, Hi! I didn't know there were someone else in this world.");
            Mes.Add("Are you an adventurer? Cool! Me too. We should be friends!");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion guardian)
        {
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("What now?!");
                Mes.Add("Don't you have anything else to do?");
                Mes.Add("What's with that look? Are you indirectly calling me ugly?!");
            }
            else
            {
                int GNPCCount = WorldMod.GetTerraGuardiansCount;
                bool HasTerraGuardians = GNPCCount > 0;
                Mes.Add("Oh, checking on me? I'm fine, thanks for caring.");
                if (HasTerraGuardians)
                {
                    Mes.Add("This place is like a dream came true!");
                    Mes.Add("I can't stop petting those TerraGuardians, they are soooooooooo cute!!");
                    Mes.Add("How are we able to understand what the TerraGuardians says? It's like as If what they say comes from inside my mind.");
                }
                if (Main.dayTime)
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("What are those things?! They aren't cute at all! Maybe Mothron but... Everything else...");
                    }
                    else
                    {
                        Mes.Add("(As soon as she started singing, several animals started to gather around her.)");
                    }
                }
                else
                {
                    Mes.Add("I surely would like to take a nap, and enjoy this night.");
                    if(HasTerraGuardians)
                        Mes.Add("I wonder if any TerraGuardian would let me accompany them during the night.");
                    Mes.Add("Yawn~ I'm really getting sleepy.");
                }
                if (Main.raining)
                {
                    Mes.Add("Eww... Rain... I hope I don't catch the flu.");
                    Mes.Add("The weather seems ugly outside.");
                }

                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                {
                    Mes.Add("What is wrong with my look? [nn:" + Terraria.ID.NPCID.Stylist + "] keeps making fun of my look.");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                {
                    Mes.Add("It's absurd that [nn:" + Terraria.ID.NPCID.Merchant + "] doesn't have any Orange or Yellow Potions for sale. Doesn't he knows that they heal more?");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic))
                {
                    Mes.Add("I was kicked out of [nn:" + Terraria.ID.NPCID.Mechanic + "]'s room... She didn't like it when I turned off the light switches after she turned them on...");
                }

                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    Mes.Add("I like [gn:" + CompanionDB.Rococo + "] because he's so easy to befriend. Soon We'll be BFFs.");
                    Mes.Add("somedays [gn:" + CompanionDB.Rococo + "] takes me outside to watch a meteor shower, at the top of a mountain. It was so beautiful...");
                }
                if (CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    Mes.Add("I like touching [gn:" + CompanionDB.Blue + "]'s hair, but she seems to not like it when I do that.");
                    Mes.Add("Say, do you think I'll find someone for me? Like [gn:" + CompanionDB.Blue + "] found.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Sardine))
                {
                    Mes.Add("[gn:" + CompanionDB.Sardine + "] has so many interesting adventure stories, I wonder if someday I'll have many stories to tell, too.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Alex))
                {
                    Mes.Add("I keep giving treats to [gn:" + CompanionDB.Alex + "]. He deserves them, he's a really good boy, one time he got my head stuck in the sand when he jumped on me. I think I still have sand in my nose.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("[gn:" + CompanionDB.Zacks + "] is so creepy! He's a cute wolf, but creepy.");
                    Mes.Add("I think that [gn:" + CompanionDB.Zacks + "] is actually a good person, but I keep fearing him because he's a zombie.");
                    Mes.Add("Aren't zombies supposed to burn during the day? [gn:" + CompanionDB.Zacks + "] seems fine while walking around during the day. At least I saw them burning in another universe.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                {
                    Mes.Add("I broke [gn:" + CompanionDB.Brutus + "]'s seriousness easily by petting his head. He started to purr afterward, too.");
                    Mes.Add("[gn:" + CompanionDB.Brutus + "]'s stories about the Ether Realm are amazing! I want to meet that place someday.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    Mes.Add("I have a new goal, becoming BFFs with [gn:" + CompanionDB.Bree + "]. Wait, why the long face?");
                }
                if (CanTalkAboutCompanion(CompanionDB.Mabel))
                {
                    Mes.Add("Why do the male people of your town keep drooling at [gn:" + CompanionDB.Mabel + "]?");
                    Mes.Add("Miss North Pole contest? Maybe [gn:" + CompanionDB.Mabel + "] could help me get in it too? It sounds fun!");
                }
                if (CanTalkAboutCompanion(CompanionDB.Domino))
                {
                    Mes.Add("Whenever I try petting [gn:" + CompanionDB.Domino + "], he tries to bite my hand.");
                    Mes.Add("Why is [gn:" + CompanionDB.Domino + "] so difficult to deal with?");
                }
                if (CanTalkAboutCompanion(CompanionDB.Leopold))
                {
                    Mes.Add("What a cute bunny [gn:" + CompanionDB.Leopold + "] is. I'd like to hug him so hard!");
                    Mes.Add("[gn:" + CompanionDB.Leopold + "] asked me earlier If I could do a test to check for Hyperactivity. ???");
                    if (!MainMod.GetLocalPlayer.Male)
                        Mes.Add("Do you like touching [gn:" + CompanionDB.Leopold + "]'s tail too? It's soft!");
                }
                if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                {
                    Mes.Add("You didn't see me last night? Sorry, I was sleeping on [gn:" + CompanionDB.Vladimir + "]'s arms during the entire night. His hug is warm.");
                    Mes.Add("Every time [gn:" + CompanionDB.Vladimir + "] hugs someone, he looks very happy and satisfied. I think he really loves that.");
                    Mes.Add("A number of people in the town think that [gn:" + CompanionDB.Vladimir + "] hugging people is exquisite, but I see them being hugged by him too.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Malisha))
                {
                    Mes.Add("I'm trying my best not to hate [gn:" + CompanionDB.Malisha + "], but she keeps turning me into a different critter whenever I try to pet her.");
                    Mes.Add("Do you think [gn:" + CompanionDB.Malisha + "] hates me? Yeah, I think not too.");
                }
                if (GNPCCount >= 10)
                {
                    Mes.Add("So many TerraGuardians around! It's like my personal heaven! Thank you! Thank you for finding them!");
                    Mes.Add("I love this place, so many cute and different looking TerraGuardians! This place is my gift!");
                }
                if (GNPCCount >= 5)
                {
                    Mes.Add("There's a lot of TerraGuardians here, I like that! So many options of things to pet.");
                    Mes.Add("Everywhere I go, I see a TerraGuardian walking around going to some place, or hanging around somewhere. I love It!");
                }
                if (GNPCCount >= 1 && GNPCCount < 5)
                {
                    Mes.Add("I liked to meet the TerraGuardians, I wonder If there are more around.");
                }
                /*if (guardian.IsPlayerRoomMate(player))
                {
                    Mes.Add("Yes! I can share my room with you. We could play pillow fight sleeping.");
                    Mes.Add("I like having you as a room mate, but I would like having a TerraGuardian more.");
                }*/
            }
            if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("(She's moving her hands, like as if she was petting something.)");
                Mes.Add("(You notice her blushing, and with a happy face, she must be in the middle of many TerraGuardians.)");
            }
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("Why there is a TerraGuardian on your shoulder?");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("Say, do you think I need a change my look?");
            Mes.Add("I keep wondering, what new thing we could add to improve the town.");
            Mes.Add("I have to say, I don't really feel like I'm fit for adventuring. I think I'm more into meeting new people.");
            if(CanTalkAboutCompanion(CompanionDB.Nemesis))
                Mes.Add("I asked [gn:" + CompanionDB.Nemesis + "] to follow me on an adventure, It followed me almost exactly like I moved. If I ran, It ran too. If I walked, It walked too. I felt so annoyed that I dismissed him.");
            if (WorldMod.GetTerraGuardiansCount >= 10)
            {
                Mes.Add("So many TerraGuardians around! I'm never sad or alone when one of them is around.");
            }
            if (WorldMod.GetTerraGuardiansCount >= 5)
            {
                Mes.Add("I like the company of the TerraGuardians.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("Um... No, I'm fine.");
                        Mes.Add("Maybe another time. Right now I have everything I need.");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.HasRequest:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("Hey, Hey! I need your help to [objective]. Can you give me a hand?");
                        Mes.Add("Saaaaaay... Can you help me [objective]?");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.Completed:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("Yay! Thanks! Hehe.");
                        Mes.Add("You did it?! You're my hero! Thanks!");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.Accepted:
                    return "Nice. Come see me when you complete it.";
                case RequestContext.TooManyRequests:
                    return "Aren't you a little overloaded with requests?";
                case RequestContext.Rejected:
                    return "Aww...";
                case RequestContext.PostponeRequest:
                    return "Later?";
                case RequestContext.Failed:
                    return "You failed? It's so disappointing...";
                case RequestContext.AskIfRequestIsCompleted:
                    return "Did you do my request?";
                case RequestContext.RemindObjective:
                    return "Easy, I asked you to [objective]. Can you remember that again?";
                case RequestContext.CancelRequestAskIfSure:
                    return "Is my request too tough for you? I can try dealing with it myself if you want.";
                case RequestContext.CancelRequestYes:
                    return "Okay, I'll be in charge of this then.";
                case RequestContext.CancelRequestNo:
                    return "Oh... Be sure to give me an update when you do what I asked for.";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    if(WorldMod.GetTerraGuardiansCount >= 3)
                        return "You are asking me to live here, alongside all those TerraGuardians wandering around? Of course I'll stay! Point me to a house and I will take it!";
                    return "Yes, I can move into your world, but I would be happier if there were more TerraGuardians around..";
                case MoveInContext.Fail:
                    return "I'd prefer not to right now.";
                case MoveInContext.NotFriendsEnough:
                    return "I don't know... I think I'll just return to my world instead.";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    if(WorldMod.GetTerraGuardiansCount >= 3)
                        return "Whaaaaat? Nooooooo you monster! I'll not be able to pet TerraGuardians anymore.";
                    return "Oh, fine. Keep your house then.";
                case MoveOutContext.Fail:
                    return "I'm keeping the house for longer...";
                case MoveOutContext.NoAuthorityTo:
                    return "You can't kick me out. It wasn't you who let me move in here.";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    if(WorldMod.GetTerraGuardiansCount > 0)
                        return "Yes! I love being around with TerraGuardians, but I love adventuring too.";
                    return "I don't have much to do anyways. Adventuring will be more fun.";
                case JoinMessageContext.Fail:
                    return "I don't want to go on an adventure right now.";
                case JoinMessageContext.FullParty:
                    return "There is no way I can join the group right now. Too many people.";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "Okay. Come see me again whenever you go on another adventure.";
                case LeaveMessageContext.Fail:
                    return "No way! I'll stay with you for longer.";
                case LeaveMessageContext.AskIfSure:
                    return "Are you sure? I don't really like being left in this place.";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "I think I know the way home. It's that way, right?";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Let's try getting into a safe place before leaving the group.";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "You want to talk? Alright, what do you want to talk about?";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "Understood. Anything else?";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "Is there something else you want to talk to me about?";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "This is a really weird thing to ask. As long as you respect your space on the bed, fine by me.";
            return "Do I get my own bed?";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "You don't like the way I fight? Then what is your suggestion, smart one?";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "I'm not actually a fan of that, but if you say so..";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "I can do that. If those monsters don't get close to me it will be even better.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "As long as you take the monsters away from me, it's fine.";
                case TacticsChangeContext.Nevermind:
                    return "Then what was this about?";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("(She's moving her hands, like as if she was petting something.)");
                        Mes.Add("(You notice her blushing, and with a happy face, she must be in the middle of many TerraGuardians.)");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "Hey! Why did you wake me up?";
                        case 1:
                            return "I was trying to get some sleep here! Respect, please.";
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "Why did you wake me up? Did you do my request?";
                        case 1:
                            return "Hey! I was trying to sleep. What? Request? Did you do It?";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "You need my help? Let me know with what!";
                case InteractionMessageContext.Accepts:
                    return "Ah, that? I can do that.";
                case InteractionMessageContext.Rejects:
                    return "No way!";
                case InteractionMessageContext.Nevermind:
                    return "What? I thought you needed my help!";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.Failed:
                    return "Isn't that a TerraGuardians thing? I'm sure we can't do that.";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "From what I know, you already got yourself a buddy.";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "You need my help there? Incoming!";
                case InviteContext.SuccessNotInTime:
                    return "I can visit you, but I'll only arrive tomorrow. Ok?";
                case InviteContext.Failed:
                    return "I'm fighting Eye of Cthulhu right now. Call me later.";
                case InviteContext.CancelInvite:
                    return "Don't need my help anymore?";
                case InviteContext.ArrivalMessage:
                    return "I'm here. What did you call me here for?";
            }
            return "";
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "Come on, for how much longer are you going to sleep?";
                case ReviveContext.RevivingMessage:
                    {
                        List<string> Mes = new List<string>();
                        if (!(target is Companion))
                        {
                            Mes.Add("Come on! You can't be a hero by lying down.");
                            Mes.Add("I think that's not the worst beating you can get.");
                            Mes.Add("You're not doing that on purpose, right?");
                        }
                        else
                        {
                            if ((target as Companion).GetGroup.IsTerraGuardian)
                            {
                                Mes.Add("*Petting intensifies*");
                                Mes.Add("Sooooooo cuuuuute!!");
                                Mes.Add("Awww... You're even cuter when knocked out.");
                            }
                            else
                            {
                                Mes.Add("Are you fine? I'll try helping you.");
                                Mes.Add("Here, I'll try doing something about those wounds.");
                            }
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "Hey! Hang on!";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "Got you, now we need less danger here.";
                case ReviveContext.RevivedByItself:
                    return "I'm fine! I'm fine... Ow...";
                case ReviveContext.ReviveWithOthersHelp:
                    return "Thanks! Like... Really. Thanks!";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "Why didn't you do anything? He was so fluffy!";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "I really need to wash my eyes, now.";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}
