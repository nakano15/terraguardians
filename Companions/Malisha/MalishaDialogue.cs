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
            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("*Mwahahaha! You have triggered my trap card!*");
                Mes.Add("*Don't worry, this wont hurt a bit.*");
                Mes.Add("*How did you know that I needed someone for my experiment?*");
                Mes.Add("*Good, I was needing someone for my brain transplant machine. Ready to turn into a Squirrel?*");
                Mes.Add("*I got you! Now drink this potion!*");
                Player player = MainMod.GetLocalPlayer;
                int BuffID = -1;
                switch (Main.rand.Next(10))
                {
                    case 0:
                        BuffID = Terraria.ID.BuffID.Endurance;
                        break;
                    case 1:
                        BuffID = Terraria.ID.BuffID.Inferno;
                        break;
                    case 2:
                        BuffID = Terraria.ID.BuffID.Lifeforce;
                        break;
                    case 3:
                        BuffID = Terraria.ID.BuffID.MagicPower;
                        break;
                    case 4:
                        BuffID = Terraria.ID.BuffID.Titan;
                        break;
                    case 5:
                        BuffID = Terraria.ID.BuffID.Darkness;
                        break;
                    case 6:
                        BuffID = Terraria.ID.BuffID.Cursed;
                        break;
                    case 7:
                        BuffID = Terraria.ID.BuffID.Confused;
                        break;
                    case 8:
                        BuffID = Terraria.ID.BuffID.Weak;
                        break;
                    case 9:
                        BuffID = 164; //Distorted
                        break;
                }
                if (BuffID > -1)
                {
                    player.AddBuff(BuffID, 10 * 60 * 60);
                }
            }
            else
            {
                Mes.Add("*Don't mind what people says, I'm one of the best magicians in the Ether Realm.*");
                if (MainMod.GetLocalPlayer.Male)
                {
                    Mes.Add("*You're making me a bit uncomfortable with the angle you're looking at me. Just a bit.*");
                }
                else
                {
                    Mes.Add("*You can't see my head, or something?*");
                }
                Mes.Add("*I once tried to conjure demons, and that's when I had to leave the first village I lived in hurry.*");
                Mes.Add("*Always wondered why you get stronger by using specific kinds of outfits? Well, me too.*");
                Mes.Add("*It's not easy being a prodigy, but when you're one, you have to keep working hard to continue being.*");
                Mes.Add("*I have perfect control of my magic! Now, at least. Lets not revive past experiences.*");
                Mes.Add("*Nobody really complained about my experiments. Here.*");

                Mes.Add("*Well, I could go to the Ether Realm to get some of my clothes, but It's too much work...*");
                Mes.Add("*You ever wondered why the TerraGuardians have no clothes in this world? Well, I never, that's the main reason I came here.*");
                Mes.Add("*I feel like people actually look directly into my \'mana containers\' when they look at me.*");
                Mes.Add("*I'm a bit disappointed that this isn't a naturalist colony like I initially thought, but I'm glad I can do my experiments here.*");
                
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
                    if (MainMod.GetLocalPlayer.Male)
                        Mes.Add("*I really love It when the rain drips through my body... Uh... Where are you looking at?*");
                    else
                        Mes.Add("*I really love It when the rain drips through my body.*");
                    Mes.Add("*Yes! Keep on raining! Bring It on!*");
                    Mes.Add("*I love rain, but there is 95% chance I'll have a serious case of flu after It ends.*");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide))
                {
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Guide + "] got really pale when he saw me doing experiments with a doll that looked like him.*");
                    Mes.Add("*Say, do you know why [nn:" + Terraria.ID.NPCID.Guide + "] is in flames?*");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Wizard))
                {
                    Mes.Add("*I tried to cast a conjuration spell with [nn:" + Terraria.ID.NPCID.Wizard + "] once, we ended up spawning a rain of Corrupt Bunnies.*");
                    Mes.Add("*I tend to share my work with [nn:" + Terraria.ID.NPCID.Wizard + "] sometimes, at least one wont blame the other if something explodes.*");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                {
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Stylist + "] says that wants to do magic with my hair, but I sense that her magic level is 0.*");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
                {
                    Mes.Add("*Can you tell [nn:" + Terraria.ID.NPCID.Dryad + "] that I don't need a baby sitter? If the fauna suddenly tries to eat you alive is because... Well, probably not my fault.*");
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Dryad + "] says that I'm a living sign of bad omen. No matter what she says, I will keep experimenting.*");
                }
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
                {
                    Mes.Add("*I wonder if [nn:" + Terraria.ID.NPCID.Clothier + "] could make me an outfit that doesn't bother me while wearing It.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    Mes.Add("*I tried to analyze [gn:" + CompanionDB. Rococo + "]'s intelligence once, I got a NotANumber Exception Error at line 12.*"); //References the GreetMessage script from Rococo.
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
                    Mes.Add("*Everytime [gn:" + CompanionDB.Michelle + "] comes bother me, I transform her into a different critter.*");
                    Mes.Add("*[gn:" + CompanionDB.Michelle + "] always arrives just in time I need someone to test my experiments.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Wrath))
                {
                    Mes.Add("*Hmph, [gn:" + CompanionDB.Wrath + "] thinks he's safe from me, but my experimenting hunger will eventually reach him. Just he wait.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Fear))
                {
                    Mes.Add("*Whaaaaaaaaaat? I was just having a friendly chat with [gn:" + CompanionDB.Fear + "], how menacing can that be? Hehe.*");
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
                    Mes.Add("*Having [gn:" + CompanionDB.Green + "] around is really useful for me. At least I have infinite supply of... Nevermind, you don't need to know.*");
                }
                if (IsPlayerRoomMate())
                {
                    Mes.Add("*Yes, I would love having you as a room mate. Heh.*");
                    Mes.Add("*Feel free to get some sleep anytime you want. If you do so now, would be perfect.*");
                    Mes.Add("*Feeling drowzy? No worry, you can lie down in any bed in our room.*");
                    Mes.Add("*It's good to have company during the night, even more when I'm working on my experiments.*");
                    if (CanTalkAboutCompanion(CompanionDB.Leopold))
                    {
                        Mes.Add("*How silly, like I would use my dear room mate as test subject. Psh...*");
                    }
                }
                if (PlayerMod.IsPlayerCompanionRoomMate(MainMod.GetLocalPlayer, CompanionDB.Leopold))
                {
                    Mes.Add("*You're sharing room with [gn:" + CompanionDB.Leopold + "]? Would you mind moving somewhere else? Huh? I have no actual reason for asking. Sigh..*");
                }
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
                    return "*I need to feel the air hit my fur, and also some blood too. Can you [objective] for me meanwhile?*";
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
                    return "*I can stay here. I hope you don't mind if I move some few experimenting aparatus here.*";
                case MoveInContext.Fail:
                    return "*Here? No. Not now.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I'm not sure if it's a good idea for me to move to this world right now.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You want me out? Fine. But you'll have to help moving my things back to my house.*";
                case MoveOutContext.Fail:
                    return "*You're not getting rid of me that easily.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*Kind of audacious of you to ask me out. Wasn't you who let me stay in here. Maybe you need to help me with a new magic or two.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*Yes. I may end up finding guinea pigs for my experiments on the way.*";
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
                    return "*You're going to regret that when I see you back at the town.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Maybe your braincells still work correctly. Lets find a safe place before you remove me from your group.*";
            }
            return LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*I have better use for my arms, but I can hold you on my tail.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*It's nice to get a free ride.*";
                case MountCompanionContext.Fail:
                    return "*No way I will do that.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*Something wrong with your feet? It is not a side effect of my experimenting, right?*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I'll be carrying them, then.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*I can carry someone with my tail, sure. Who should I carry then?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*There, you can walk again. I feel that my tail got stronger, too.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*I guess my feet thanks you.*";
                case DismountCompanionContext.Fail:
                    return "*Not right now.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*I can share my bed, as long as you don't make me fall off it.*";
            return "*A bed all for myself, again.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Mwahahaha. In my reach... I mean... Sure, I can share chair. Hehe.*";
            return "*Oh, okay. I'll take another chair then, should I need to use one.*";
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("(She's speaking about many different things, in a different language.)");
                        Mes.Add("(Is she saying elements of the periodic table?)");
                        Mes.Add("(She accidentally casted a spell while asleep, and turned the table nearby into a toad.)");
                        Mes.Add("(She accidentally casted a spell while asleep, and turned the chair nearby into a potato.)");
                        Mes.Add("(She's doing an evil laugh while sleeps)");
                        if (CanTalkAboutCompanion(CompanionDB.Leopold))
                        {
                            Mes.Add("*Here's my newest invention, now time to drink It...* (That brings flashbacks...)");
                            Mes.Add("*I've just learned this spell, I'm glad you offered yourself help me test It...* (Run [gn:" + CompanionDB.Leopold + "], Run!)");
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case SleepingMessageContext.OnWokeUp:
                    if (Main.rand.Next(2) == 0)
                        return "*There. You woke me up. Now say whatever you want to say before I turn you into a plushie.*";
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
                    return "*You think I should change how I fight? How does little Terrarian think this great magician should fight?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*In close range? Are you nuts? Oh well, I guess I have to show my moves then.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*In mid range, huh? Seems reasonable.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Avoiding contact, huh? I like that. Our foes will not even see what hit them.*";
                case TacticsChangeContext.Nevermind:
                    return "*Yes, I knew my fighting style was perfect. There was no need to assure me.*";
                case TacticsChangeContext.FollowAhead:
                    return "*Sure thing. I hope you don't have second intentions about that, though.*";
                case TacticsChangeContext.FollowBehind:
                    return "*You're not the most interesting thing to look at, but it's fine anyways.*";
                case TacticsChangeContext.AvoidCombat:
                    return "*And what am I supposed to do if a monster charge us? Plead for my life?*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*I was waiting for that moment.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*You're curious, aren't you? Alright, what can I \'enlighten\' you with?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*There, you heard what I had to say. Anything else?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Finally.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*Shall you deliver pain in my guise, [nickname].*";
                case ControlContext.SuccessReleaseControl:
                    return "*There you go, released. I have better use of my body when I'm in control.*";
                case ControlContext.FailTakeControl:
                    return "*No way I'm doing that now.*";
                case ControlContext.FailReleaseControl:
                    return "*I'm not letting you go right now.*";
                case ControlContext.NotFriendsEnough:
                    return "*Would you give a remote controller to a toddler? No, that's what I think of you.*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*What? Feeling lonely? I'm right here, remember?*";
                        case 1:
                            return "*Hm? I'm lost in thoughts here about potions I need to try next. Are you use to some testing later?*";
                        case 2:
                            return "*It's interesting to see how you deal with things, hehe.*";
                    }
                case ControlContext.GiveCompanionControl:
                    return "*Need to go to the toilet or something? Wait, no. Well, whatever.*";
                case ControlContext.TakeCompanionControl:
                    return "*There. Now back to thinking.*";
            }
            return base.ControlMessage(companion, context);
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "*I think It will be benefitial for us if I move my things to this world. There are so many potential subj... Study creatures around here that may be useful for my experiments. Just give me a call when you have a house ready.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*Okay, It's not working either way. I have an idea, what If I go alone in the travels? Lets bond-merge together and you decide where we should go to.*";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*You seems to have seens various kinds of places during your travels, maybe that can help me find test subjects.*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*It's not me, It's you, you're slowing down the expedition. I can carry you around during the exploration, If you want. I have better uses for my hands than holding you, so I guess I can use my tail, if you don't mind.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*Nice timing, [nickname]. I wanted to speak with you. Of all the experiments I have to do, one that I can't do myself is regarding having a Buddy. If you don't mind, pick me as one when possible. And yeah, one time thing and stuff, just so you know.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*Oh wow... I feel better already. Well, it's tradition of being a Buddy to not doubt your Buddiship partner, so I wont question whatever you ask me, or at least most of what you ask me.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*Need my intellectual help? Or my help with something else? Ask it. Lets see if I can do it.*";
                case InteractionMessageContext.Accepts:
                    return "*Sure, I will do it.*";
                case InteractionMessageContext.Rejects:
                    return "*No way you're asking me that.*";
                case InteractionMessageContext.Nevermind:
                    return "*Changed your mind, huh? Fine.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*It's about time you asked me that. I'm more than suitable to lead everyone.*";
                case ChangeLeaderContext.Failed:
                    return "*I'd rather not get the spotlight, for now.*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*So, you want to be my buddy? I like the idea of having a familiar pet. You do know you can't pick another one once you pick a buddy, right? You still want to pick me as your buddy?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Fine then. I will take you as my buddy. I guess I should avoid using you as guinea pig for potentially lethal experiments then.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*Hmph. I should have known you weren't serious about that.*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*As much interesting as testing what that feels like, I don't think you're the one I should try that with. Or at least for now.*";
                case BuddiesModeContext.Failed:
                    return "*Huh? What? I was lost in thoughts, thinking about the many things I could test on you.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*Don't you have one already? They're following you, and I can see that clearly.*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*I don't really have much going on right now. I will show up in a few moments.*";
                case InviteContext.SuccessNotInTime:
                    return "*I'm busy with something right now. I will show up after I'm done here.*";
                case InviteContext.Failed:
                    return "*I have something else to do right now. Now, you, say \'Aaaahhh\'. Oops, you shouldn't have heard that.*";
                case InviteContext.CancelInvite:
                    return "*Regretted your decision or something? Fine, I wont be showing up there then.*";
                case InviteContext.ArrivalMessage:
                    return "*Here I am [nickname]. I hope it's important.*";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                /*case MessageIDs.LeopoldEscapedMessage:
                    return "";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "";*/
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Please don't wake up.. Please don't wake up...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Even her scent rises my fur...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*So many different things she came in contact with... Many are dangerous...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    if(WorldMod.HasCompanionNPCSpawned(CompanionDB.Leopold))
                        return "*I feel pity of [gn:"+CompanionDB.Leopold+"] now.*";
                    return "*I'm so glad I wont need to identify her again.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Uh oh... HEEEEELP!!*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*I'll be pouncing towards you. Just try to stay alive until then.*";
                case ReviveContext.RevivingMessage:
                    {
                        List<string> Mes = new List<string>();
                        if (target is not Companion)
                        {
                            Mes.Add("*If you don't make It, would you mind If I do experiments with your body?*");
                            Mes.Add("*I think this may allow me to learn more about your anathomy.*");
                            Mes.Add("*If I close here, may solve your bleeding problem.*");
                            if(target.Male)
                                Mes.Add("*If It stops you from groaning, you may keep looking in that direction.*");
                            Mes.Add("*Your body isn't much different from the one of a TerraGuardian. Maybe easy to fix.*");
                            Mes.Add("*This will ease your pain while I work, drink It.*");
                        }
                        else
                        {
                            Companion ReviveGuardian = target as Companion;
                            bool GotAnotherMessage = false;
                            if (ReviveGuardian.ModID == "")
                            {
                                GotAnotherMessage = true;
                                switch (ReviveGuardian.ID)
                                {
                                    default:
                                        GotAnotherMessage = false;
                                        break;
                                    case CompanionDB.Leopold:
                                        Mes.Add("*I'm just saying, but that isn't a good example.*");
                                        Mes.Add("*I'm not interessed in taking your place, for now.*");
                                        Mes.Add("*Here something for your wounds. You'll be fine. Not bad coming from your worst studdent.*");
                                        Mes.Add("*Okay, okay, I'm closing the open wounds.*");
                                        break;
                                    case CompanionDB.Brutus:
                                        Mes.Add("*I can't let you die. I need you.*");
                                        Mes.Add("*Come on, big boy, don't disappoint me.*");
                                        Mes.Add("*Okay, If you wake up, I'll use the shrinking spell on you again. Now wake up!*");
                                        Mes.Add("*I need you for my researches, If you please wake up.*");
                                        break;
                                    case CompanionDB.Zacks:
                                        Mes.Add("*Your nerves will be connected soon, just let me work.*");
                                        Mes.Add("*You could help me pointing where you can't move.*");
                                        Mes.Add("*I can't help you with your left leg, It has been too damaged. Anything else that needs fix?*");
                                        break;
                                    case CompanionDB.Vladimir:
                                        Mes.Add("*I preffer you when smiling.*");
                                        Mes.Add("*Too much ground to fix...*");
                                        Mes.Add("*Come on, big guy. You wont let such a thing kill you, right?*");
                                        break;
                                    case CompanionDB.Alex:
                                        Mes.Add("*Okay, I think I've got a problem.*");
                                        Mes.Add("*I don't really know animal anathomy, so I may commit mistakes here.*");
                                        Mes.Add("*Is this... Oh! Sorry for touching It.*");
                                        break;
                                }
                            }
                            if (!GotAnotherMessage)
                            {
                                if (ReviveGuardian.Base.CompanionType == CompanionTypes.Terrarian)
                                {
                                    Mes.Add("*Okay, you'll be fine... If you survive...*");
                                    Mes.Add("*If you don't make It, you wont mind if I use your body on my future experiments, right?*");
                                    Mes.Add("*This will ease your pain while I work, drink It.*");
                                }
                                else
                                {
                                    Mes.Add("*Time to make use of the anathomy classes.*");
                                    Mes.Add("*I'll just take a bit of blood, for research purpose.*");
                                    Mes.Add("*You look in pain, drink this to ease It.*");
                                    Mes.Add("*One. One, two. Two, three. Three, four. What am I doing?*");
                                }
                            }
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*Another one falls...*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Oh ho ho, the many experiments I could do with you.*";
                case ReviveContext.RevivedByItself:
                    return "*Okay, who will be the first one I'll turn into a frog?*";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*Awww... You remembered me... Thank you.*";
                    return "*You couldn't go on without me, couldn't you?*";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}