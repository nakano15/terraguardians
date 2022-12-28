using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class SardineDialogues : CompanionDialogueContainer
    {public override string GreetMessages(Companion companion)
        {
            if (Main.rand.NextDouble() < 0.5)
                return "Hey, aren't you an adventurer? Cool! I am too!";
            return "Tarararan-Taran! Meet the worlds biggest smallest bounty hunter ever! Me!";
        }

        public override string NormalMessages(Companion guardian)
        {
            Player player = Main.LocalPlayer;
            List<string> Mes = new List<string>();
            if (Main.dayTime)
            {
                Mes.Add("Why female humans keep wanting to try scratching the back of my head?");
                Mes.Add("This place surelly is livelly, but I'd rather go out and beat some creatures.");
            }
            else
            {
                Mes.Add("I'm so sleepy, do you know of any window I could be at?");
                Mes.Add("Looks like a perfect moment to explore, even more with my night eyes.");
            }
            if (Main.bloodMoon)
            {
                Mes.Add("Do you know what time it is? It's fun time! Let's go outside and beat some ugly creatures!");
            }
            if (Main.raining)
            {
                Mes.Add("I hate this weather, but at least gives me good reason to stay at home.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Where did those weird creatures came from?");
            }
            bool HasBreeMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Bree), HasGlennMet = PlayerMod.PlayerHasCompanionSummoned(player, CompanionDB.Glenn);
            /*switch (guardian.OutfitID)
            {
                case CaitSithOutfitID:
                    Mes.Add("I really like this outfit, but the cloak on my tail is bothering me a bit.");
                    Mes.Add("For some reason, this outfit also came with some kind of sword.");
                    Mes.Add("[nickname], do you know what Tokyo is?");
                    break;
            }*/
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
            if (MainMod.HasCompanionInWorld(1))
            {
                Mes.Add("Do you have some spare medicine? [gn:1] seems to be wanting to play with me again...");
                Mes.Add("I have tried to outrun [gn:1] so I don't play that stupid game, but she's faster than me on 4 legs.");
                Mes.Add("If you see [gn:1], say that you didn't see me. I'm tired of playing \"Cat and the Wolf\" with her, I didn't even knew that game existed, and my body is still wounded because of her teeth from the last game.");
            }
            if (MainMod.HasCompanionInWorld(3))
            {
                Mes.Add("I really don't want to play with [gn:3], I could even run away from him, but everytime I do so, he pulls me back with... Whatever is that thing! It's smelly and yuck!");
                Mes.Add("I really hate when [gn:3] plays his game with me, everytime he acts like as if was devouring my brain I feel like my heart was going to jump out of my mouth.");
                Mes.Add("Eugh, [gn:3] \"played\" a game with me, and now I'm not only bitten on many places, but also with smelly sticky stuffs around. Wait, will I turn into a Zombie because of that?! Should I begin panicking?");
                Mes.Add("I want to remove all that stinky stuff i've got from being bullied by [gn:3] from my fur, but I don't even know what is that, so I can't really lick it away. Maybe I should... *Gulp* Take a bath? With water?");
            }
            if (MainMod.HasCompanionInWorld(1) && MainMod.HasCompanionInWorld(3))
            {
                Mes.Add("You want to know what is worse than a wolf playing \"Cat and Wolf\" with you? Two wolves!!! And one is a Zombie!!");
                Mes.Add("First she invented that \"Cat and Wolf\" game, now that creepy [gn:3] invented the \"The Walking Guardian\" game. Why does they love bullying me? Is that a wolf thing?");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Mechanic) && NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("Everytime I chat with [nn:" + Terraria.ID.NPCID.Mechanic + "], she is always in good mood and happy. On other hand, [nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] stares at me with a killer face. Maybe I should start sharpening my knife, meow?");
            }
            if(PlayerMod.PlayerHasCompanionSummoned(player, 0))
                Mes.Add("Hey [gn:0], want to play some \"Hide and Seek\"? Don't take me wrong, I like games, depending on them...");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 1))
            {
                Mes.Add("Hello, I- Waaaaaah!!! *He ran away, as fast as he could.*");
                Mes.Add("No! Go away! I don't want to play with you, I don't even want to see you, I.. I... Have some important things to do, I mean... Somewhere veeeeeeery far away from you!");
            }
            if (PlayerMod.PlayerHasCompanionSummoned(player, 3))
            {
                Mes.Add("Yikes! Go away! Your \"game\" spooks me out so hard.");
                Mes.Add("No way, not again. *He ran away*");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("I really feel bad about [nn:" + Terraria.ID.NPCID.Angler + "], he'll never know my trick for catching more than one fish. Neither you will do.");
            if (NPC.downedBoss1)
                Mes.Add("So, you have defeated the Eye of Cthulhu, right? Psh, Easy. Do you ever wanted to know who killed Cthulhu? Well, It was me. Hey, what are you laughing at?");
            if (NPC.downedBoss3)
                Mes.Add("My next bounty is set to the Skeletron, at the Dungeon Entrance. Let's go face him!");
            /*if (!GuardianBountyQuest.SardineTalkedToAboutBountyQuests)
            {
                Mes.Add("Hey, do you have a minute? I want to discuss with you about my newest bounty hunting service.");
            }
            else
            {
                if (GuardianBountyQuest.SignID == -1)
                    Mes.Add("I need a sign in my house to be able to place my bounties in it. I only need one, no more than that. If It's an Announcement Box, will be even better.");
            }*/
            if (MainMod.HasCompanionInWorld(5))
            {
                Mes.Add("Playing with [gn:5] is really fun. But he has a little problem to know what \"enough\" is.");
                Mes.Add("One of the best things that ever happened was when you brought [gn:5] here, at least playing with him doesn't hurts or cause wounds... Most of the time.");
            }
            if (MainMod.HasCompanionInWorld(7))
            {
                Mes.Add("I really love [gn:7], but she keeps controlling me. She could at least give me more freedom, It's not like as if I would run or something.");
                Mes.Add("[gn:7] was the most lovely and cheerful person I've ever met, but for some reason, she started to get grumpy after we married. What happened?");
                Mes.Add("Even though [gn:7] tries to hog all my attention to her, I still love her.");
                Mes.Add("I wonder, does [gn:7] carrying that bag all day wont do bad for her back?");
                if (MainMod.HasCompanionInWorld(1))
                    Mes.Add("Woah, you should have seen [gn:7] fighting with [gn:1] earlier. That remembered me of the day we met.");
                if (MainMod.HasCompanionInWorld(3))
                    Mes.Add("Ever since [gn:7] saw [gn:3] playing that stupid hateful game with me, she has been asking frequently If I'm fine, and If I wont... Turn? What is that supposed to mean?");
            }
            if (MainMod.HasCompanionInWorld(8))
            {
                Mes.Add("I love having [gn:8] around, but she asks me to do too many things. It's a bit tiring. Refuse? Are you nuts? Did you look at her?!");
                Mes.Add("I have a favor to ask of you. If you see me staring at [gn:8] with a goof face, slap my face.");
                if (MainMod.HasCompanionInWorld(7))
                {
                    Mes.Add("[gn:7] keeps asking me if there is something happening between me and [gn:8]. No matter how many times I say no, she still remains furious.");
                }
                if (MainMod.HasCompanionInWorld(CompanionDB.Fluffles) && MainMod.HasCompanionInWorld(CompanionDB.Blue))
                {
                    Mes.Add("I have to tell you something bizarre that happened to me the other day. I managed to run away from [gn:" + CompanionDB.Blue + "] and [gn:" + CompanionDB.Fluffles + "] game, or so I thought, and managed to return home. When [gn:" + CompanionDB.Bree + "] looked at me, she saw [gn:" + CompanionDB.Fluffles + "] hanging on my shoulder. [gn:"+CompanionDB.Bree+"] screamed so loud that scared her off.");
                }
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Domino))
            {
                Mes.Add("Lies! I'm not buying catnip! Where did you brought that idea from?");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Vladimir))
            {
                Mes.Add("Hey, you know that guy, [gn:"+CompanionDB.Vladimir+"]? He's really helping me with some personal matters. If you feel troubled, have a talk with him.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Michelle))
            {
                Mes.Add("Whenever I tell stories about my adventures, [gn:" + CompanionDB.Michelle + "] listen attentiously to every details of it. I think I got a fan.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Fluffles))
            {
                bool HasBlue = MainMod.HasCompanionInWorld(CompanionDB.Blue),
                    HasZacks = MainMod.HasCompanionInWorld(CompanionDB.Zacks);
                if (HasBlue && HasZacks)
                {
                    Mes.Add("Great, now I have a narcisistic wolf, a rotting wolf and a hair rising ghost fox trying to have a piece of me. You have some kind of grudge against me?");
                    Mes.Add("If you see [gn:"+CompanionDB.Blue+"], [gn:"+CompanionDB.Zacks+"] and [gn:"+CompanionDB.Fluffles+"] looking like they are biting something on the floor, that must be me. Help me if you can?");
                }
                else if (HasBlue)
                {
                    Mes.Add("I really hate having [gn:" + CompanionDB.Fluffles + "] around, because there are now TWO to bite me on [gn:" + CompanionDB.Blue + "]'s stupid game.");
                    Mes.Add("How can you escape from something you can't even see? [gn:"+CompanionDB.Fluffles+"] always catches me because she's nearly invisible during day!");
                }
                Mes.Add("I have to tell you what happened to me the other day. I was on the toilet doing my things, having a hard time, until [gn:"+CompanionDB.Fluffles+"] surged from nowhere. She spooked me really hard! But at least solved my constipation issue.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Minerva))
            {
                Mes.Add("Like [gn:" + CompanionDB.Minerva + "], you're wondering why I only eat fish? It's because fishs are the best!");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Glenn))
            {
                Mes.Add("[gn:" + CompanionDB.Glenn + "] is more the studious type than a fighter.");
                Mes.Add("Sometimes I don't have the chance of doing something with [gn:" + CompanionDB.Glenn + "], our interests are different.");
                Mes.Add("[gn:" + CompanionDB.Glenn + "] should stop reading so many fairy tales books, since we are literally living in one.");
                if (MainMod.HasCompanionInWorld(CompanionDB.Zacks))
                {
                    Mes.Add("If [gn:"+CompanionDB.Zacks+"] keep scaring my son whenever he's outside at dark, I'll show him a version of me that he didn't met when biting me!");
                }
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Miguel))
            {
                Mes.Add("Could [gn:"+CompanionDB.Miguel+"] stop making jokes about my belly? They hurt!");
                Mes.Add("I'm really getting some exercise tips from [gn:"+CompanionDB.Miguel+"] to turn my fat into muscles, but he keeps making jokes about my belly.");
            }
            if (MainMod.HasCompanionInWorld(CompanionDB.Green))
            {
                Mes.Add("You may think ghosts and stuff are scary, but you wont know what is scary, until you wake up and see [gn:"+CompanionDB.Green+"] staring directly at your face.");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            Mes.Add("Hey, are you interessed into going on a treasure hunting? Haha, I was just wanting to start a chat, If I had an idea of hidden treasure, I'd already have got it.");
            Mes.Add("Say, how many worlds have you visited? Can you count it on your toes? Because I have visited too many worlds.");
            Mes.Add("What is the point of an Angel Statue? Not even rigging them with wire does anything.");
            Mes.Add("I know a cool world we could visit, maybe one day I'll bring you there.");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 2))
            {
                Mes.Add("Do you think that we will bump on my house during our travels? Beside I don't really remember how it looked like...");
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
            if (PlayerMod.PlayerHasCompanionSummoned(player, 0))
                Mes.Add("Hey [gn:0], want to play some Dodgeball?");
            if (PlayerMod.PlayerHasCompanionSummoned(player, 1))
                Mes.Add("What? No! No Way! Go away! I don't want to play some more of that painful game.");
            if (WorldMod.HasCompanionNPCSpawned(1))
            {
                Mes.Add("May not look like it, but [gn:1] has very sharp teeth, don't ask how I found out that... Ouch...");
                Mes.Add("Sometimes I think that [gn:1] uses that \"game\" of her just to bully me.");
            }
            if (WorldMod.HasCompanionNPCSpawned(3))
            {
                Mes.Add("I have to say, from all the things that could haunt me in my life, [gn:3] had to happen? He's even my neighbor!!");
                Mes.Add("I don't really think that [gn:3] is a bad guy, but I really hate playing that game of his. Even If I deny he plays it with me. I just can't run away, since he pulls me back using his... Whatever is that thing.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.GoblinTinkerer))
            {
                Mes.Add("[nn:" + Terraria.ID.NPCID.GoblinTinkerer + "] isn't that plumberer, but looks with that exact same death stare when he sees me.");
            }
            if (WorldMod.HasCompanionNPCSpawned(0))
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
                        return "I don't really need anything right now. All that I want is to beat some monsters.";
                    return "Hum, nothing right now. Later, maybe?";
                case RequestContext.HasRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "I feel weird for asking this but... I need your help with a particular something... It's about... [objective]. Hey, don't laugh.";
                    return "I'm not really a fan of asking for help, but I really need help for this. I need you to [objective]. Can you help me with that?";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "You're the best, did you knew? Of course you knew!";
                    return "I knew you would be able to help me with my little request. Here a token of my affection.";
                case RequestContext.Failed:
                    return "Well, It's not everyday you can have success, right. I'm not angry. It's fine.";
                case RequestContext.Accepted:
                    return "Ok. See me when you get that done.";
                case RequestContext.Rejected:
                    return "Oh, fine.";
                case RequestContext.TooManyRequests:
                    return "Don't you have many things to do right now?";
                case RequestContext.PostponeRequest:
                    return "Come see me if you decide to help me with this.";
                case RequestContext.AskIfRequestIsCompleted:
                    return "Hey [nickname], completed my request?";
                case RequestContext.RemindObjective:
                    return "Short memory, eh? I asked you to [objective].";
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
                    return "There's way too many people in your group. There is no way I can join.";
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
                    return "Awww... I were having so much fun. Let's adventure some more in the future.";
                case LeaveMessageContext.Fail:
                    return "Now is not the best moment for that.";
                case LeaveMessageContext.AskIfSure:
                    return "Yes, I can leave the group but... Here?";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "Okay, okay. I wont judge your decision. I see you back at home. Be safe.";
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
                    return "*I think I might be able to carry you.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*You're offering to give me a ride? Of course I will accept!*";
                case MountCompanionContext.Fail:
                    return "*Maybe another time.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*Seems handy, but I think I'll use my own feet to travel for now.*";
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

        public override string OnToggleShareChairMessage(bool Share)
        {
            if(Share)
                return "Yes, but only with one condition: You have to pet my head.";
            return "Aww... Fine.";
        }
    }
}