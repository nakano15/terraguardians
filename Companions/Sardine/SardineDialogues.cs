using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class SardineDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Hey, aren't you an adventurer? Cool! I'm one too!";
            return "Tarararan-Taran! Meet the world's biggest smallest bounty hunter ever! Me!";
        }

        public override string NormalMessages(Companion guardian)
        {
            Player player = Main.LocalPlayer;
            List<string> Mes = new List<string>();
            if (Main.dayTime)
            {
                Mes.Add("Why do female humans keep wanting to try scratching the back of my head?");
                Mes.Add("This place sure is lively, but I'd rather go out and beat up some creatures.");
            }
            else
            {
                Mes.Add("I'm so sleepy, do you know of any window I could sleep in?");
                Mes.Add("Looks like a perfect moment to explore, even more with my night eyes.");
            }
            if (Main.bloodMoon)
            {
                Mes.Add("Do you know what time it is? It's fun time! Let's go outside and beat up some ugly creatures!");
            }
            if (Main.raining)
            {
                Mes.Add("I hate this weather, but at least it gives me good reason to stay at home.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Where did those weird creatures come from?");
            }
            bool HasBreeMet = HasCompanion(CompanionDB.Bree), HasGlennMet = HasCompanion(CompanionDB.Glenn);
            /*switch (guardian.OutfitID)
            {
                case CaitSithOutfitID:
                    Mes.Add("I really like this outfit, but the cloak on my tail is bothering me a bit.");
                    Mes.Add("For some reason, this outfit also came with some kind of sword.");
                    Mes.Add("[nickname], do you know what Tokyo is?");
                    break;
            }*/
            if (HasBreeMet && !HasCompanion(CompanionDB.Bree) && !HasCompanionSummoned(CompanionDB.Bree))
            {
                Mes.Add("Odd. We have found my wife, but why isn't she here with me?");
                Mes.Add("Did you see [gn:"+CompanionDB.Bree+"]? Have you seen my wife? We found her, but she's not here.");
            }
            if (HasBreeMet && HasGlennMet)
            {
                Mes.Add("I'm so glad that my son and my wife are safe.");
                Mes.Add("I caused my son and wife so much trouble when I disappeared, now neither of us knows the way home...");
                Mes.Add("I thank you for finding my wife and my son, [nickname]. You have my eternal gratitude.");
            }
            else if (!HasBreeMet && HasGlennMet)
            {
                Mes.Add("[nickname], I heard from my son that my wife left home to look for me. If you find a white cat during your travels, please bring her here.");
                Mes.Add("I'm sorry for asking this [nickname], but I just heard from my son that my wife is still looking for me. She's a white cat, please find her.");
            }
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("Do you have some spare medicine? [gn:1] seems to be wanting to play with me again...");
                Mes.Add("I have tried to outrun [gn:1] so I don't play that stupid game, but she's faster than me on 4 legs.");
                Mes.Add("If you see [gn:1], tell her that you haven't seen me. I'm tired of playing \"Cat and the Wolf\" with her, I didn't even know that game existed, and my body is still wounded because of her teeth from the last game.");
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("I really don't want to play with [gn:3], I try and run away from him, but every time I do so, he pulls me back with... Whatever that thing is! It's smelly and yuck!");
                Mes.Add("I really hate when [gn:3] plays his game with me, every time he acts like as if was devouring my brain I feel like my heart was going to jump out of my mouth.");
                Mes.Add("Eugh, [gn:3] \"played\" a game with me, and now I'm not only bitten in many places but I'm also covered with smelly sticky stuff. Wait, will I turn into a zombie because of that?! Should I begin panicking?");
                Mes.Add("I want to remove all that stinky stuff I've got from being bullied by [gn:3] from my fur, but I don't even know what is that, so I can't really lick it away. Maybe I should... *Gulp* Take a bath? With water?");
            }
            if (CanTalkAboutCompanion(1) && CanTalkAboutCompanion(3))
            {
                Mes.Add("You want to know what is worse than a wolf playing \"Cat and Wolf\" with you? Two wolves!!! And one is a Zombie!!");
                Mes.Add("First she invented that \"Cat and Wolf\" game, now that creepy [gn:3] invented the \"The Walking Guardian\" game. Why do they love bullying me? Is that a wolf thing?");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic) && NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("Every time I chat with [nn:" + Terraria.ID.NPCID.Mechanic + "], she is always in a good mood and happy. On the other hand, [nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] stares at me with a killer face. Maybe I should start sharpening my knife, meow?");
            }
            if(HasCompanionSummoned(0))
                Mes.Add("Hey [gn:0], want to play some \"Hide and Seek\"? Don't take me wrong, I like games, depending on them...");
            if (HasCompanionSummoned(1))
            {
                Mes.Add("Hello, I- Waaaaaah!!! *He ran away, as fast as he could.*");
                Mes.Add("No! Go away! I don't want to play with you, I don't even want to see you, I.. I... Have some important things to do, I mean... Somewhere veeeeeeery far away from you!");
            }
            if (HasCompanionSummoned(3))
            {
                Mes.Add("Yikes! Go away! Your \"game\" spooks me out so hard.");
                Mes.Add("No way, not again. *He ran away*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("I really feel bad about [nn:" + Terraria.ID.NPCID.Angler + "], he'll never know my trick for catching more than one fish. Neither will you.");
            if (NPC.downedBoss1)
                Mes.Add("So, you have defeated the Eye of Cthulhu, right? Psh, Easy. Have you ever wanted to know who killed Cthulhu? Well, It was me. Hey, what are you laughing at?");
            if (NPC.downedBoss3)
                Mes.Add("My next bounty is set to Skeletron, at the Dungeon Entrance. Let's go face him!");
            /*if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
            {
                Mes.Add("Hey, do you have a minute? I want to discuss with you about my newest bounty hunting service.");
            }
            else
            {
                if (GuardianBountyQuest.SignID == -1)
                    Mes.Add("I need a sign in my house to be able to place my bounties in it. I only need one, no more than that. If It's an Announcement Box, it will be even better.");
            }*/
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("Playing with [gn:5] is really fun. But he has a little problem knowing what \"enough\" is.");
                Mes.Add("One of the best things that ever happened was when you brought [gn:5] here, at least playing with him doesn't hurt or cause wounds... Most of the time.");
            }
            if (CanTalkAboutCompanion(7))
            {
                Mes.Add("I really love [gn:7], but she keeps controlling me. She could at least give me more freedom, It's not like as if I would run or something.");
                Mes.Add("[gn:7] was the most lovely and cheerful person I've ever met, but for some reason, she started to get grumpy after we got married. What happened?");
                Mes.Add("Even though [gn:7] tries to hog all my attention to her, I still love her.");
                Mes.Add("I wonder, does [gn:7] carrying that bag all day won't do badly for her back?");
                if (CanTalkAboutCompanion(1))
                    Mes.Add("Woah, you should have seen [gn:7] fighting with [gn:1] earlier. That reminded me of the day we first met.");
                if (CanTalkAboutCompanion(3))
                    Mes.Add("Ever since [gn:7] saw [gn:3] playing that stupid hateful game with me, she has been asking frequently If I'm fine, and If I won't... Turn? What is that supposed to mean?");
            }
            if (CanTalkAboutCompanion(8))
            {
                Mes.Add("I love having [gn:8] around, but she asks me to do too many things. It's a bit tiring. Refuse? Are you nuts? Have you looked at her?!");
                Mes.Add("I have a favor to ask of you. If you see me staring at [gn:8] with a goof face, slap my face.");
                if (CanTalkAboutCompanion(7))
                {
                    Mes.Add("[gn:7] keeps asking me if there is something happening between me and [gn:8]. No matter how many times I say no, she still remains furious.");
                }
                if (CanTalkAboutCompanion(CompanionDB.Fluffles) && CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    Mes.Add("I have to tell you something bizarre that happened to me the other day. I managed to run away from [gn:" + CompanionDB.Blue + "] and [gn:" + CompanionDB.Fluffles + "] game, or so I thought and managed to return home. When [gn:" + CompanionDB.Bree + "] looked at me, she saw [gn:" + CompanionDB.Fluffles + "] hanging on my shoulder. [gn:"+CompanionDB.Bree+"] screamed so loud that scared her off.");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                Mes.Add("Lies! I'm not buying catnip! Where did you get that idea from?");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("Hey, you know that guy, [gn:"+CompanionDB.Vladimir+"]? He's really into helping me with some personal matters. If you ever feel troubled, have a talk with him.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("Whenever I tell stories about my adventures, [gn:" + CompanionDB.Michelle + "] listen attentively to every detail of it. I think I got a fan.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                bool HasBlue = CanTalkAboutCompanion(CompanionDB.Blue),
                    HasZacks = CanTalkAboutCompanion(CompanionDB.Zacks);
                if (HasBlue && HasZacks)
                {
                    Mes.Add("Great, now I have a narcissistic wolf, a rotting wolf, and a hair-raising ghost fox trying to have a piece of me. Do you have some kind of grudge against me?");
                    Mes.Add("If you see [gn:"+CompanionDB.Blue+"], [gn:"+CompanionDB.Zacks+"] and [gn:"+CompanionDB.Fluffles+"] looking like they are biting something on the floor, that must be me. Help me if you can?");
                }
                else if (HasBlue)
                {
                    Mes.Add("I really hate having [gn:" + CompanionDB.Fluffles + "] around, because there are now TWO to bite me on [gn:" + CompanionDB.Blue + "]'s stupid game.");
                    Mes.Add("How can you escape from something you can't even see? [gn:"+CompanionDB.Fluffles+"] always catches me because she's nearly invisible during the day!");
                }
                Mes.Add("I have to tell you what happened to me the other day. I was on the toilet doing my things, having a hard time, until [gn:"+CompanionDB.Fluffles+"] surged from nowhere. She spooked me really hard! But at least she solved my constipation issue.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("Like [gn:" + CompanionDB.Minerva + "], you're wondering why I only eat fish? It's because fish are the best!");
            }
            if (CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                Mes.Add("[gn:" + CompanionDB.Glenn + "] is more the studious type than a fighter.");
                Mes.Add("Sometimes I don't have the chance of doing something with [gn:" + CompanionDB.Glenn + "], our interests are different.");
                Mes.Add("[gn:" + CompanionDB.Glenn + "] should stop reading so many fairy tales books, since we are literally living in one.");
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("If [gn:"+CompanionDB.Zacks+"] keep scaring my son whenever he's outside at dark, I'll show him a version of me that he hasn't meet when biting me!");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("Could [gn:"+CompanionDB.Miguel+"] stop making jokes about my belly? They hurt!");
                Mes.Add("I'm really getting some exercise tips from [gn:"+CompanionDB.Miguel+"] to turn my fat into muscles, but he keeps making jokes about my belly.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("You may think ghosts and stuff are scary, but you wont know what is scary, until you wake up and see [gn:"+CompanionDB.Green+"] staring directly at your face.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leona))
            {
                Mes.Add("I wonder if would be a mistake to ask [gn:"+CompanionDB.Leona+"] for a spar match.");
                Mes.Add("[gn:"+CompanionDB.Leona+"] has been taking on some bounties too. She always brings back both sides of the monster she went to hunt for.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Monica))
            {
                Mes.Add("Be glad that you're tall, at least you can't be crushed by [gn:"+CompanionDB.Monica+"]'s massive backside.");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("Do you humans always visit bathrooms when others are using them?");
                Mes.Add("I'm trying to concentrate here, If you'll excuse me.");
            }
            if(IsControllingCompanion(CompanionDB.Bree))
            {
                Mes.Add("Haha, not even with you being Bond-Linked with my wife, makes her show at least a smile.");
                Mes.Add("It feels so odd speaking to you when you're linked with my wife. I mean, you're not screaming or calling me stupid.");
                Mes.Add("I guess you're not here to tell me that we're going home, right? Hahaha, sorry [nickname], I know it's you.");
            }
            if(IsControllingCompanion(CompanionDB.Blue))
            {
                Mes.Add("Aaahhh!! [nickname]! You scared me. I thought you were [controlled].");
                Mes.Add("Please let me know before you undo bond-link with her. I want to go somewhere safe before you do that.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            Mes.Add("Hey, are you interested in going on a treasure hunt? Haha, I was just wanting to start a chat, If I had an idea of where some hidden treasure was, I'd already have gotten it.");
            Mes.Add("Say, how many worlds have you visited? Can you count it on your toes? Because I have visited too many worlds.");
            Mes.Add("What is the point of an Angel Statue? Not even rigging them with wire does anything.");
            Mes.Add("I know a cool world we could visit, maybe one day I'll bring you there.");
            if (HasCompanionSummoned(2))
            {
                Mes.Add("Do you think that we will bump into my house during our travels? Besides I don't really remember what it looked like...");
            }
            else
            {
                Mes.Add("I'm starting to get rusty from all this standing around, let's go on an adventure!");
                Mes.Add("Uh, I'm a little short on coins right now, let's go farm for some?");
            }
            if (!Main.dayTime)
            {
                Mes.Add("I'm getting soooooo sleepy... Oh! I'm awake. I'm awake.");
            }
            if (HasCompanionSummoned(0))
                Mes.Add("Hey [gn:0], want to play some Dodgeball?");
            if (HasCompanionSummoned(1))
                Mes.Add("What? No! No Way! Go away! I don't want to play some more of that painful game.");
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("It may not look like it, but [gn:1] has very sharp teeth, don't ask how I found out that... Ouch...");
                Mes.Add("Sometimes I think that [gn:1] uses that \"game\" of her just to bully me.");
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("I have to say, from all the things that could haunt me in my life, [gn:3] had to happen? He's even my neighbor!!");
                Mes.Add("I don't really think that [gn:3] is a bad guy, but I really hate playing that game of his. Even If I deny he plays it with me anyways. I can't just run away, since he pulls me back using his... Whatever that thing is.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("[nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] isn't that plumber, but looks with that exact same death stare when he sees me.");
            }
            if (CanTalkAboutCompanion(0))
            {
                Mes.Add("[gn:0] may be stupid and childish, but I really like talking to him.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "Want to share some adventure stories? I really like that idea!";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "Is there something else you want to talk to me?";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "Had enough of chatting? Me too. Okay, we can chat more later. Anything else?";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "I don't really need anything right now. All that I want is to beat up some monsters.";
                    return "Hum, nothing right now. Later, maybe?";
                case RequestContext.HasRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "I feel weird asking for this but... I need your help with a particular something... It's about... [objective]. Hey, don't laugh.";
                    return "I'm not really a fan of asking for help, but I really need help for this. I need you to [objective]. Can you help me with that?";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "You're the best, did you know? Of course, you knew!";
                    return "I knew you would be able to help me with my little request. Here is a token of my affection.";
                case RequestContext.Failed:
                    return "Well, It's not every day you can have success, right? I'm not angry. It's fine.";
                case RequestContext.Accepted:
                    return "Ok. See me when you get that done.";
                case RequestContext.Rejected:
                    return "Oh, fine.";
                case RequestContext.TooManyRequests:
                    return "Don't you have too many things to do right now?";
                case RequestContext.PostponeRequest:
                    return "Come see me again if you decide to help me with this.";
                case RequestContext.AskIfRequestIsCompleted:
                    return "Hey [nickname], have you completed my request?";
                case RequestContext.RemindObjective:
                    return "Short memory, eh? I asked you to [objective].";
                case RequestContext.CancelRequestAskIfSure:
                    return "You want to cancel my request? Why? Is It tough, or no time? You're really sure?";
                case RequestContext.CancelRequestYes:
                    return "Oh man... Better I get into doing that, then...";
                case RequestContext.CancelRequestNo:
                    return "Whew... Good.";
            }
            return base.RequestMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "I'm always ready for a new adventure.";
                case JoinMessageContext.FullParty:
                    return "There are way too many people in your group. There is no way I can join.";
                case JoinMessageContext.Fail:
                    return "I can't go on an adventure right now...";
            }
            return base.JoinGroupMessages(companion, context);
        }
        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "Awww... I was having so much fun. Let's adventure some more in the future.";
                case LeaveMessageContext.Fail:
                    return "Now is not the best moment for that.";
                case LeaveMessageContext.AskIfSure:
                    return "Yes, I can leave the group but... Here?";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "Okay, okay. I won't judge your decision. I see you back at home. Be safe.";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Yes, let's look for a town so I can leave the group.";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "I think I might be able to carry you.";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "You're offering to give me a ride? Well of course I'll accept!";
                case MountCompanionContext.Fail:
                    return "Maybe another time.";
                case MountCompanionContext.NotFriendsEnough:
                    return "Seems handy, but I think I'll use my own feet to travel for now.";
                case MountCompanionContext.SuccessCompanionMount:
                    return "Yes! Free ride!";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "Yes! Yes! Please! Who's shoulder should I ride on?";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*Here you go.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Aww... It's over already?*";
                case DismountCompanionContext.Fail:
                    return "*Not right now.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "Perfect! I could stay with you guys while I search for the world my house is at.";
                case MoveInContext.Fail:
                    return "I don't feel like it's safe enough for me to move in.";
                case MoveInContext.NotFriendsEnough:
                    return "I don't know... Maybe another time?";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "What? Oh well.. Where will I live now..? Well... Keep the house, then..";
                case MoveOutContext.Fail:
                    return "No, I will stay here for now.";
                case MoveOutContext.NoAuthorityTo:
                    return "I hardly know you and you want me to move out? Are you crazy?";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if(Share)
                return "Yes, but only with one condition: You have to pet my head.";
            return "Aww... Fine.";
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if(Share)
                return "I don't mind, but I must let you know that I tend to end up sleeping in warm places.";
            return "Aww...";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "I need to change my combat tactic? What do you suggest?";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "Alright! Time to make some minced monsters!";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "I'll take foes by range then, until some of them manage to get close to me.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "I'll shower arrows on the monsters.";
                case TacticsChangeContext.Nevermind:
                    return "Alright! Something else you need to talk about?";
                case TacticsChangeContext.FollowAhead:
                    return "I will do that.";
                case TacticsChangeContext.FollowBehind:
                    return "Fine, you lead the way.";
                case TacticsChangeContext.AvoidCombat:
                    return "I don't like that idea, but you're the leader here.";
                case TacticsChangeContext.PartakeInCombat:
                    return "Good to hear that I can kill things again.";
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
                        Mes.Add("Ha! Take that! And that! (He must be sleeping about facing some creature.)");
                        Mes.Add("(He's sleeping happily)");
                        Mes.Add("Yuck! Go away! Let me go! (He must be having nightmares with the King Slime)");
                        if (PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Bree))
                        {
                            Mes.Add("I'm home! I brought all those treasures for us. (He must be dreaming about returning home with [gn:" + CompanionDB.Bree + "].)");
                        }
                        else
                        {
                            Mes.Add("I'm home! I brought all those treasures for us. (He must be dreaming about returning home for someone.)");
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "It's really late, couldn't you speak with me during the day?";
                        case 1:
                            return "I need to recharge my energy for my adventures, buddy. Could you make It quick? I want to get back to sleep.";
                        case 2:
                            return "Aww man... Couldn't you wait until the sun rises?";
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "What? The request is done? I'm happy about that but... Couldn't it wait?";
                        case 1:
                            return "Woah! Oh, It's you. Did you do my request?";
                    }
            }
            return base.SleepingMessage(companion, context);
        }
        
        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "Let's do it!";
                case ControlContext.SuccessReleaseControl:
                    return "Done. Now I can fight alongside you again.";
                case ControlContext.FailTakeControl:
                    return "I don't like that idea right now.";
                case ControlContext.FailReleaseControl:
                    return "This looks like a bad moment for that.";
                case ControlContext.NotFriendsEnough:
                    return "Why should I? I don't know you well yet.";
                case ControlContext.ControlChatter:
                    if(PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Bree))
                    {
                        if (Main.rand.Next(3) == 0)
                            return "If you're scared of [gn:"+CompanionDB.Bree+"]'s face, don't worry, she's angry at me, not you.";
                    }
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "Wait, I can't do much while Bond-Merged? Dude, watching is plain boring...";
                        case 1:
                            return "Riding your shoulder is a lot more fun than watching you fight.";
                        case 2:
                            return "Since I can only speak with you, does that makes me your conscience?";
                    }
                case ControlContext.GiveCompanionControl:
                    return "I? Take control? That's weird but... Okay? I wonder what I will do now, then..";
                case ControlContext.TakeCompanionControl:
                    return "You want control back? Here it goes.";
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
                    return "If you need someone fast for something dangerous, don't think twice about sending me there.";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "I will get rusty If I stay locked in my house. Take me on your adventures too, I like loot too!";
                case UnlockAlertMessageContext.MountUnlock:
                    return "Hey pal, my feet are getting kind of sore. Would you mind If I ride on your back? Don't worry, I can still fight meanwhile.";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "Hey friend, have a moment? You're one of the greatest adventurers I know, and you're a really cool person, so I wanted to let you know that if you ever need to pick someone as your buddy, I am one option for you. And more, there's no reason why I should deny it, it's because you are great.";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "Oh yeah, I nearly forgot. You know that being Buddies is way over than Friendship, right? Then that means if you ask me something, I will do it. If you want me to ride your back, I will do so. How's this? Liked knowing about this?";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "Huh? Do you need me to do something? What is it?";
                case InteractionMessageContext.Accepts:
                    return "I can do that. Just watch me.";
                case InteractionMessageContext.Rejects:
                    return "What? No way!";
                case InteractionMessageContext.Nevermind:
                    return "Wha~ Changed your mind?!";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "I will do that.";
                case ChangeLeaderContext.Failed:
                    return "Not now.";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "So, you picked me as your buddy? Are you sure about that?";
                case BuddiesModeContext.PlayerSaysYes:
                    return "I like the idea of having you as my buddy too. Alright, we're now buddies. Two adventurers, one buddiship. Sounds cool, right?";
                case BuddiesModeContext.PlayerSaysNo:
                    return "Hm, then make sure you want to be my buddy before asking that.";
                case BuddiesModeContext.NotFriendsEnough:
                    return "So, you picked me as your buddy? But I hardly know you enough to accept that.";
                case BuddiesModeContext.Failed:
                    return "Not right now.";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "You know that you already have a buddy, right?";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "You need my help for something? Sure, I can go there.";
                case InviteContext.SuccessNotInTime:
                    return "I'll be there tomorrow. Right now is too late.";
                case InviteContext.Failed:
                    return "I'm really busy right now.";
                case InviteContext.CancelInvite:
                    return "Alright. Then I'll stay here.";
                case InviteContext.ArrivalMessage:
                    return "I'm here, [nickname].";
            }
            return "";
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "I'm glad you called, It's fun trying to carry you home.";
                case ReviveContext.RevivingMessage:
                    {
                        List<string> Mes = new List<string>();
                        bool IsPlayer = !(target is Companion);
                        if (IsPlayer)
                        {
                            Mes.Add("It won't result in a good story having you laying down on the ground.");
                            Mes.Add("Come on, we have more adventures to make!");
                            Mes.Add("You'll be okay, your adventure isn't over.");
                        }
                        else
                        {
                            bool GotAMessage = false;
                            Companion ReviveCompanion = target as Companion;
                            if (ReviveCompanion.ModID == companion.ModID)
                            {
                                GotAMessage = true;
                                switch (ReviveCompanion.ID)
                                {
                                    default:
                                        GotAMessage = false;
                                        break;
                                    case CompanionDB.Blue:
                                        {
                                            Mes.Add("I think I will regret this...");
                                            Mes.Add("I wonder, helping her right now, will make her stop bullying me?");
                                            Mes.Add("Look at those teeth... Wait, better I look somewhere else, I may lose motivation.");
                                        }
                                        break;
                                    case CompanionDB.Zacks:
                                        {
                                            Mes.Add("How am I supposed to heal him? His entire body has problems.");
                                            Mes.Add("I'm having flashbacks... Don't think about them...");
                                            Mes.Add("My heart is racing whenever I get near him. It's scary.");
                                        }
                                        break;
                                    case CompanionDB.Bree:
                                        {
                                            Mes.Add(ReviveCompanion.GetName + " wake up! Please wake up!");
                                            Mes.Add("I never wanted to place you in danger, don't make me feel guilty now.");
                                            Mes.Add("Please open your eyes! Say something! Insult me! Anything! " + ReviveCompanion.GetName + "!!");
                                        }
                                        break;
                                    case CompanionDB.Glenn:
                                        {
                                            Mes.Add(ReviveCompanion.GetName + "! " + ReviveCompanion.GetName + "! Can you hear me?!");
                                            Mes.Add("No no no no NO! " + ReviveCompanion.GetName+"! Hang on! Your father is here!");
                                            Mes.Add("Don't worry "+ReviveCompanion.GetName+", I won't let It end like this.");
                                        }
                                        break;
                                }
                            }
                            if (!GotAMessage)
                            {
                                Mes.Add("Don't worry, I'll help you!");
                                Mes.Add("You'll be 100% soon.");
                                Mes.Add("I'll take care of those wounds, no worries.");
                            }
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "Oh no, hang on!";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "You'll be fine, I'm with you, just don't die.";
                case ReviveContext.RevivedByItself:
                    return "I can still stand.";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextFloat() < 0.5f)
                        return "So glad to have you guys around.";
                    return "I'm fine, thanks for the help.";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == CompanionDB.Bree)
                {
                    Weight = 1.5f;
                    return "Don't worry "+WhoJoined.GetNameColored()+", we'll find out where our house is at*";
                }
                if(WhoJoined.ID == CompanionDB.Glenn)
                {
                    Weight = 1.5f;
                    return "I'm disappointed that you disobeyed your mother, but I'm also happy that you made it here safelly.";
                }
                if(WhoJoined.ID == CompanionDB.Blue)
                {
                    Weight = 1.5f;
                    return "Wait, why is she showing me her teeth?";
                }
            }
            Weight = 1f;
            return "A new person! Nice to meet you.";
        }

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Bree:
                        Weight = 1.5f;
                        return "She's coming with us? I hope isn't for keeping an eye on me.";
                    case CompanionDB.Glenn:
                        Weight = 1.5f;
                        return "I don't mind him coming with us, as long as he doesn't get in danger.";
                    case CompanionDB.Vladimir:
                        Weight = 1.5f;
                        return "Hey big guy, carry me.";
                    case CompanionDB.Blue:
                        Weight = 1.5f;
                        return "No! Please! Don't!!";
                    case CompanionDB.Zacks:
                        Weight = 1.5f;
                        return "You aren't here to bite me, right?";
                    case CompanionDB.Fluffles:
                        Weight = 1.5f;
                        return "You don't plan on mounting on my shoulder again, don't you?";
                }
            }
            Weight = 1f;
            return "Hi! Nice to see you joining us.";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "Uh... Did you scare him? Haha... Hahaha..";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "That is so weird.";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Tell me, what are you hiding, cat?*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Alright...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*Uh huh...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*I see... So you have the scent of many dead creatures...*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*What am I doing...? Uh... Checking your pulse. You seems alive! Phew...*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override MessageBase MessageDialogueOverride(Companion companion)
        {
            if(!SardineBountyBoard.TalkedAboutBountyBoard && !companion.IsHomeless)
            {
                MessageDialogue m = new MessageDialogue("Hey [nickname], you came at the right moment.\nSince staying at home is boring, I will open a Bounty Board in your world.");
                m.AddOption("A Bounty Board?", BB_AskWhatIsBountyBoard);
                return m;
            }
            return base.MessageDialogueOverride(companion);
        }

        private void BB_AskWhatIsBountyBoard()
        {
            MessageDialogue m = new MessageDialogue("Yes. People can come to me, and ask me to track a dangerous monster terrorizing your world, while also offering a reward once the threat is defeated.");
            m.AddOption("Sounds nice.", BB_SoundsNice);
            m.AddOption("Sounds boring.", BB_SoundsBoring);
            m.RunDialogue();
        }

        private void BB_SoundsNice()
        {
            MessageDialogue m = new MessageDialogue("Yes, it is. But I will need you to do something for me before I do that..");
            m.AddOption("What do you need?", BB_AskWhatHeNeeds);
            m.RunDialogue();
        }

        private void BB_SoundsBoring()
        {
            MessageDialogue m = new MessageDialogue("Yeah.. The paperwork surely is boring, but facing creatures for loot isn't. But I need you to do something for me before that...");
            m.AddOption("What do you need?", BB_AskWhatHeNeeds);
            m.RunDialogue();
        }

        private void BB_AskWhatHeNeeds()
        {
            MessageDialogue m = new MessageDialogue("I need a sign at my home, so I can place the information about the bounties.");
            m.AddOption("Ok.", BB_TellThatWillLookIntoThat);
            m.RunDialogue();
        }

        private void BB_TellThatWillLookIntoThat()
        {
            SardineBountyBoard.TalkedAboutBountyBoard = true;
            SardineBountyBoard.ActionCooldown = 300;
            Dialogue.LobbyDialogue("When you place a sign at my house, I will let you know if I found it or not.");
        }

        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            if (SardineBountyBoard.TalkedAboutBountyBoard && !companion.IsHomeless)
            {
                if (!SardineBountyBoard.TalkedAboutBountyBoard)
                    dialogue.AddOption("What do you need for the Bounty Board?", TalkAboutBountyBoard);
                else
                {
                    if(SardineBountyBoard.PlayerCanRedeemReward(MainMod.GetLocalPlayer))
                    {
                        dialogue.AddOption("I want my reward for my Bounty.", TalkAboutBountyBoard);
                    }
                    else
                    {
                        dialogue.AddOption("Check Bounty Status.", TalkAboutBountyBoard);
                    }
                }
            }
        }

        public void TalkAboutBountyBoard()
        {
            if (!SardineBountyBoard.SignExists())
                Dialogue.LobbyDialogue("I need a sign in my house, so I can place the bounty information on it. I will let you know when I find the sign.");
            else
            {
                if (SardineBountyBoard.TargetMonsterID == 0)
                {
                    Dialogue.LobbyDialogue("I've got no bounty for you right now. I will let you know once a new one comes.");
                }
                else if(SardineBountyBoard.PlayerCanRedeemReward(MainMod.GetLocalPlayer))
                {
                    if (SardineBountyBoard.PlayerRedeemReward(MainMod.GetLocalPlayer))
                        Dialogue.LobbyDialogue("Nice job! Here's your reward.");
                    else
                        Dialogue.LobbyDialogue("Reward? What reward? You can't claim it right now. Clear your inventory first.");
                }
                else
                {
                    if (SardineBountyBoard.GetBountyState(MainMod.GetLocalPlayer) == SardineBountyBoard.Progress.BountyKilled)
                    {
                        Dialogue.LobbyDialogue("You've already killed this bounty. Wait until another one if you want something to kill.");
                    }
                    else
                    {
                        Dialogue.LobbyDialogue("The bounty is currently active. Check the sign for informations about it.");
                    }
                }
            }
        }
    }
}
