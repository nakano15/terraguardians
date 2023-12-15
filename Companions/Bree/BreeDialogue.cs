using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class BreeDialogue : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "Don't you go thinking I'll stay here for too long. It's just temporary.";
                case 1:
                    return "Who are you? Have you see my husband anywhere?";
                case 2:
                    return "Ugh, I need someplace to put off some steam.";
            }
        }

        public override string NormalMessages(Companion guardian)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            bool HasSardineMet = CanTalkAboutCompanion(CompanionDB.Sardine), 
                HasGlennMet = CanTalkAboutCompanion(CompanionDB.Glenn);
            if (HasSardineMet)
            {
                Mes.Add("A year after that imbecile I call husband went out on one of his \"adventures\", I started searching for him.");
                if(!HasGlennMet)
                    Mes.Add("[gn:2] and I have a son, he's currently at home. He's old enough to take care of himself, but he's probably missing us.");
            }
            Mes.Add("The floor is awful, nobody cleans this place? Looks like I'll have to clean this place.");
            Mes.Add("The people in your town are nice, but I prefer a quiet and less noisy place.");
            Mes.Add("I won't place my things on the floor, soon I'll be going back home. I just need to remember which world I lived in.");
            Mes.Add("At first, this bag was quite heavy on my shoulders. As I kept using it, started to feel lighter. Did I grow stronger?");
            Mes.Add("Most of the time I'm busy cleaning up the place, looks like nobody else does.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
                Mes.Add("I tried asking [nn:" + Terraria.ID.NPCID.Dryad + "] for clues of which world I lived in. She said that she also visited several worlds, so she can't pinpoint places. I should be so lucky...");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("[nn:" + Terraria.ID.NPCID.Merchant + "] disappoints me every time I check his store. He should improve his store stock.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                Mes.Add("[nn:" + Terraria.ID.NPCID.ArmsDealer + "] should be ashamed of selling such outdated guns.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("Do you want to hear a joke? [nn:" + Terraria.ID.NPCID.Angler + "] doesn't know how to catch two fish at once, can you believe? Wait, you don't either? You must be kidding!");
            if (HasSardineMet && !CanTalkAboutCompanion(CompanionDB.Sardine) && !HasCompanionSummoned(CompanionDB.Sardine))
            {
                Mes.Add("Where is my stupid husband? I can't find him anywhere. Did he go on another adventure?");
                Mes.Add("[gn:"+CompanionDB.Sardine+"] is not with you? I was expecting to see him with you. Where did he go?");
            }
            if (HasSardineMet && HasGlennMet)
            {
                Mes.Add("Thank you for finding my son and my husband. We should now try finding out which world we came from, now...");
                Mes.Add("My son and my husband are fine, all thanks to you.");
                Mes.Add("I feel so good about having my son and my husband okay....");
            }
            else if (!HasSardineMet && HasGlennMet)
            {
                Mes.Add("My son came looking for me, but I still haven't found my husband.");
                Mes.Add("My son got quite sad when I told him that I haven't found his dad yet...");
                Mes.Add("[nickname], if you could help me find my husband, It will be great. He's a black cat with some spirit for adventure.");
            }
            /*switch (guardian.SkinID)
            {
                case BaglessSkinID:
                    Mes.Add("It's good to not have that weight on my back, It was already starting to ache.");
                    if(player.Male)
                        Mes.Add("You've been looking at me way too much since I removed the bag, why is that?");
                    Mes.Add("What? You're impressed that I'm actually strong? Housework isn't an easy thing. Or was it the bag?");
                    Mes.Add("I hope there are no thieves in your world, I really don't want to return home and find out my things are gone.");
                    break;
            }
            switch (guardian.OutfitID)
            {
                case DamselOutfitID:
                    Mes.Add("I also have taste for clothing, you know.");
                    Mes.Add("I'm glad that the Clothier managed to make this clothing, it just fits in me.");
                    Mes.Add("I feel like wanting to spend an afternoon at a beach now.");
                    break;
            }*/
            if (CanTalkAboutCompanion(0))
            {
                Mes.Add("I really love having [gn:0] in the town, I can ask him to do things without question.");
                Mes.Add("Every time [gn:0] asks me If I want to play some kid's game, I ask him what is his age. That creates a delay of a day before he asks me again.");
            }
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("Sometimes I try getting beauty tips from [gn:1]. She seems to be an expert on that.");
                if (CanTalkAboutCompanion(2))
                    Mes.Add("If I ever see [gn:1] bullying my husband again, she will regret it!");
                Mes.Add("Looks like [gn:1] and I had the same objective, but the result...");
                if(NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                    Mes.Add("Have you passed through [nn:" + Terraria.ID.NPCID.Stylist + "]'s shop? I'm waiting about 4 hours for her to finish [gn:1]'s hair treatment so she can start mine.");
            }
            if (HasSardineMet)
            {
                Mes.Add("Once I remember which world I lived in, I'm taking [gn:2] back with me.");
                Mes.Add("I used to be happy and cheerful, until [gn:2] happened. I should have listened to my mom.");
                Mes.Add("I once saw [gn:2] kill a giant monster alone, by using a Katana. I was so amazed by it, that I fell for him. Big mistake I did.");
                Mes.Add("Soon, [gn:2] and I will go back home and try to restart our life. Soon...");
                if (!HasCompanionSummoned( CompanionDB.Sardine))
                    Mes.Add("Have you seen [gn:2]? He's probably doing something stupid.");
            }
            if (HasCompanionSummoned(CompanionDB.Sardine) && !HasCompanionSummoned(CompanionDB.Bree))
            {
                Mes.Add("Since you're taking [gn:"+CompanionDB.Sardine+"] on your adventures, keep an eye on him for me, and bring him back home in one piece. Will you?");
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("Would you please tell [gn:3] to stay away from me? He creeps me out.");
                if (CanTalkAboutCompanion(2))
                    Mes.Add("I have to tell you something! I went outside for a walk, and I saw [gn:3] pulling my husband, and then biting him! BITING, HIM! I ran back home after that, and then suddenly, I saw my husband covered in some sticky goo complaining about something. Is he alright? Is [gn:2] going to be alright?! Wait, AM I EVEN SAFE HERE?!");
                if (CanTalkAboutCompanion(1))
                    Mes.Add("Wait, you're telling me that [gn:3] is [gn:1]'s boyfriend? That's one weird choice.");
            }
            if (CanTalkAboutCompanion(4))
            {
                Mes.Add("I can't really tell ya much about [gn:4], he doesn't say much, either.");
                Mes.Add("Sometimes I see [gn:4] starring at the dungeon entrance. I wonder what is on his mind.");
                Mes.Add("[gn:4] seems to have only one emotion. -_-");
            }
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("Sometimes [gn:5] makes my company. I love it when he lies down next to me while I'm doing something. I feel less alone.");
                Mes.Add("I'm not very fond of dogs, but [gn:5] is an exception. I guess I should thank his old owner for that.");
                Mes.Add("Sometimes I see [gn:5] staring at the moon. What could be coming on his mind?");
            }
            if (CanTalkAboutCompanion(6))
            {
                Mes.Add("[gn:6] keeps bragging how strong he is, until I challenged him on an arm wrestling.");
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                {
                    Mes.Add("Sometimes I think that [gn:6] should get a haircut from [nn:" + Terraria.ID.NPCID.Stylist + "], at least it would be better than that thing he has on his head.");
                }
                Mes.Add("I have some drinks with [gn:6] sometimes, he has some funny stories from the Ether World, like when a magician apprentice put fire on the king's robe during a celebration.");
            }
            if (CanTalkAboutCompanion(8))
            {
                if (CanTalkAboutCompanion(2))
                {
                    Mes.Add("I know that [gn:2] spends way too much time with [gn:8], I hope that cat doesn't plan to cheat on me.");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("If you want to make me feel annoyed, just leave me for 5 minutes with [gn:" + CompanionDB.Michelle + "] in the same room.");
                Mes.Add("I hate [gn:" + CompanionDB.Michelle + "], she just does not stop talking!");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("If [gn:" + CompanionDB.Malisha + "] causes one more explosion, I will have a serious talk with her.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("I don't really have anything bad to say about [gn:" + CompanionDB.Fluffles + "], maybe It's because she doesn't speak.");
                Mes.Add("Sometimes [gn:" + CompanionDB.Fluffles + "] presence makes my hair rise. You let really unusual people live in your world.");
                bool HasBlue = CanTalkAboutCompanion(CompanionDB.Blue), HasZacks = CanTalkAboutCompanion(CompanionDB.Zacks), HasSardine = CanTalkAboutCompanion(CompanionDB.Sardine);
                if (HasSardine)
                {
                    if (HasBlue && HasZacks)
                    {
                        Mes.Add("Tell me, will [gn:" + CompanionDB.Sardine + "] even survive one of [gn:" + CompanionDB.Blue + "]'s bullying? Even I am having bad times trying to get all those guardians out of him.");
                        Mes.Add("The other day I had to help my husband get some bath because he came home all slobbered, and with some bite marks on his body.");
                    }
                    else if (HasBlue && CanTalkAboutCompanion(CompanionDB.Fluffles))
                    {
                        Mes.Add("After [gn:" + CompanionDB.Fluffles + "] arrived, I had to stop [gn:" + CompanionDB.Blue + "] and her from chasing [gn:" + CompanionDB.Sardine + "] more frequently.");
                        Mes.Add("Do you have something that repels ghosts? I think [gn:"+CompanionDB.Sardine+"] might need something like that.");
                    }
                    Mes.Add("There was one time when [gn:"+CompanionDB.Sardine+"] returned home, and I got spooked after I saw [gn:"+CompanionDB.Fluffles+"] on his shoulder. I screamed so loud that she ran away, and I nearly dirtied the floor too.");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("[gn:" + CompanionDB.Minerva + "] still hasn't got into the level for my refined taste. She still has a lot to learn.");
                Mes.Add("I tried teaching [gn:" + CompanionDB.Minerva + "] how to cook properly, but she always misses the point when cooking.");
            }
            if (HasGlennMet)
            {
                Mes.Add("My son is very studious, he literally devours several books every week.");
                Mes.Add("My son is quite introvert, so the only moment you get him to talk, is when someone else does first.");
                if (CanTalkAboutCompanion(CompanionDB.Mabel) && NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                    Mes.Add("I really dislike seeing [gn:" + CompanionDB.Glenn + "] playing with [nn:" + Terraria.ID.NPCID.Angler + "], that kid is such a bad influence.");
                else
                {
                    Mes.Add("It kind of worries me that there aren't many kids around for my son to play with...");
                }
                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    Mes.Add("I like seeing that [gn:" + CompanionDB.Rococo + "] has been playing with [gn:" + CompanionDB.Glenn + "].");
                }
                if (CanTalkAboutCompanion(CompanionDB.Alex))
                {
                    Mes.Add("[gn:" + CompanionDB.Alex + "] is not only keeping me company sometimes but also plays with my son, [gn:" + CompanionDB.Glenn + "].");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("[gn:" + CompanionDB.Cinnamon + "] actually knows how to put seasoning to food well, but she sometimes exaggerates a bit.");
                if(HasSardineMet)
                    Mes.Add("Well, teaching [gn:" + CompanionDB.Cinnamon + "] makes me forget the stupidities my husband does.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("[gn:"+CompanionDB.Miguel+"] said that he's impressed on how strong I am. I don't see why that's impressive.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Celeste))
            {
                if (HasSardineMet)
                {
                    Mes.Add("*Speaking to [gn:" + CompanionDB.Celeste + "] is really uplifting. She always manages to douse a bit of the anger my husband causes to me.*");
                }
                else
                {
                    Mes.Add("*I wonder if that "+MainMod.TgGodName+" [gn:"+CompanionDB.Celeste+"] talks about will help me find my husband.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Leona))
            {
                Mes.Add("**");
            }
            if (guardian.IsPlayerRoomMate(player))
            {
                if (player.Male)
                    Mes.Add("Okay, I can share my bedroom. Just don't try anything funny during the night.");
                Mes.Add("As long as you keep It clean, you can use It for as long as you want.");
                Mes.Add("If you get a bed for yourself, I can let you stay in my bedroom.");
                if (HasSardineMet)
                {
                    Mes.Add("I'm sorry, but I kind of would prefer sharing my room with [gn:" + CompanionDB.Sardine + "].");
                }
            }
            if (PlayerMod.IsPlayerCompanionRoomMate(player, CompanionDB.Sardine))
            {
                if (!player.Male)
                    Mes.Add("I hope you aren't trying to get \'intimate\' with my husband. Remember that we are still married.");
                Mes.Add("I will never understand why [gn:"+CompanionDB.Sardine+"] shares his room with you, but not me. I didn't make him sleep on the sofa lately.");
            }
            if (Main.bloodMoon)
            {
                Mes.Clear();
                Mes.Add("What am I supposed to do to have A MOMENT ALONE!");
                Mes.Add("GO AWAY!");
                Mes.Add("Do you have to bother me, now?");
                Mes.Add("You're annoying me!");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("Eek!! Turn the other way!");
                Mes.Add("Do you really have to enter here and talk to me while I'm using the toilet?");
            }
            if (PlayerMod.IsPlayerControllingCompanion(player, CompanionDB.Sardine))
            {
                Mes.Add("You are not my husband. [nickname], why are you Bond-Linked to my husband?");
                Mes.Add("I know it's you, [nickname]. I can feel your presence alongside my husband's.");
                Mes.Add("Since you're Bond-Linked with my husband, can you try figuring out where his heads at?");
            }
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("Go away! I don't want to carry your burden.");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            if (CanTalkAboutCompanion(1))
                Mes.Add("Do you want to know how to anger [gn:1]? Easy, throw a bucket of water on her hair. Now, do you know how much longer it takes for her anger to pass?");
            if (CanTalkAboutCompanion(2))
            {
                Mes.Add("Sometimes I don't know if [gn:2] even cares about me. It's like, his adventures are his top priority.");
                Mes.Add("I don't entirely hate [gn:2], but what he has done isn't okay. Besides I shouldn't throw stones, either.");
            }
            Mes.Add("Maybe you can help me remember which world I came from. It had a grassland, then there was that evil land, It also had a dungeon and a jungle... All worlds have those? Oh...");
            Mes.Add("Sometimes I like to rest on a window.");
            Mes.Add("I like chasing butterflies, but they always seem to escape.");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            List<string> Mes = new List<string>();
            Mes.Add("It's not clean... yet... (She's sleeping.)");
            Mes.Add("(She has a way more peaceful look when sleeping than awaken.)");
            Mes.Add("(She's doing gestures, like as if was hitting something, or someone.)");
            if (!WorldMod.HasMetCompanion(CompanionDB.Sardine))
            {
                Mes.Add("...Sardine... Where are you... (She spoke while sleeping)");
                Mes.Add("...My home... How are my things... (She seems to be worried about her home.)");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "Just no. Not right now.";
                    return "Humph, no.";
                case RequestContext.HasRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "Even though I'm away from home, I still need some things done, but I'm busy right now. So... I ask you... Can you [objective]?";
                    return "I hope you are more reliable than my husband. I need a thing to be done. I need you to [objective]. Will you do it?";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "That doesn't mean I'll give you a star of a good person. But... You're nice.";
                    return "Maybe that will make me be less furious.";
                case RequestContext.Accepted:
                    return "Good. Don't delay too long with the request.";
                case RequestContext.TooManyRequests:
                    return "What? No way. You have too many things to do right now.";
                case RequestContext.Rejected:
                    return "Hmph. I should have known that it would be too hard for you.";
                case RequestContext.PostponeRequest:
                    return "Hey, but I need that now! *Sigh* Whatever, go do your things.";
                case RequestContext.Failed:
                    return "Good job, you managed to ruin everything. Now go away!";
                case RequestContext.AskIfRequestIsCompleted:
                    return "Wait, did you actually complete what I asked you for?";
                case RequestContext.RemindObjective:
                    return "Just like my husband, you forgot what I asked, right? Sigh... [objective] for me, right?";
                case RequestContext.CancelRequestAskIfSure:
                    return "YOU WHAT?! How can you... Wait, you're really going to drop what I asked for, are you?";
                case RequestContext.CancelRequestYes:
                    return "Grr... Fine. I'll do It myself, then.";
                case RequestContext.CancelRequestNo:
                    return "Ah, good. Well, do you need anything else?";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "I can stay here for a while. At least until I remember which world my house is at.";
                case MoveInContext.Fail:
                    return "No.";
                case MoveInContext.NotFriendsEnough:
                    return "I hardly know you. What if you're expecting me to unload my bag at one of your houses, so you can take whatever is in it?";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "Fine, keep your stinky house then. I won't need it anymore once I remember where my house is at.";
                case MoveOutContext.Fail:
                    return "Not at this time.";
                case MoveOutContext.NoAuthorityTo:
                    return "Who do you think you are to try and evict me?";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "I really needed a break from housekeeping, anyways. I hope you won't make me miss that.";
                case JoinMessageContext.FullParty:
                    return "I may be small, but I don't think I will fit in that group of yours.";
                case JoinMessageContext.Fail:
                    if (!PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Sardine))
                    {
                        return "I can't right now, I'm looking for clues about my husband's whereabouts.";
                    }
                    else
                    {
                        return "No way, I have many things to do in my house.";
                    }
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "Here?! Right here?! Do you want to leave me here all alone?! Are you nuts?";
                case LeaveMessageContext.Success:
                    return "I need to rest my legs anyway.";
                case LeaveMessageContext.Fail:
                    return "You're not leaving me here right now.";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "Yes, fine. Leave a damsel to fight her way back home all alone...";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Okay, let's find a safe place for me before I leave, then.";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "Fine, I can carry you for a while.";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "I hope you won't let me fall.";
                case MountCompanionContext.Fail:
                    return "Not this time.";
                case MountCompanionContext.NotFriendsEnough:
                    return "I don't trust you enough for that.";
                case MountCompanionContext.SuccessCompanionMount:
                    return "I won't mind a free ride. I hope [target] doesn't have fleas.";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "You want me to hop onto someone's shoulder? Whose shoulder?";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "You were making my shoulders sore, anyways.";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "I hope isn't because of my claws, or my weight.";
                case DismountCompanionContext.Fail:
                    return "Not now!";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            return "I'm married.";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share) return "Fine, but I hold onto my bad.";
            return "I'm not against that.";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "What? You're displeased with the way I fight? What is your idea, then?";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "I like that idea. Let them come.";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "Fine. I'll avoid contact with the enemies.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "Alright. I'll totally avoid contact with the enemies, then.";
                case TacticsChangeContext.Nevermind:
                    return "Then what was that for?";
                case TacticsChangeContext.FollowAhead:
                    return "I hope you aren't doing this to save yourself!";
                case TacticsChangeContext.FollowBehind:
                    return "I'll be following then.";
                case TacticsChangeContext.AvoidCombat:
                    return "I hate this. Fine, I will avoid combat then.";
                case TacticsChangeContext.PartakeInCombat:
                    return "I can attack things again? Good.";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "You want to talk? Fine, I think I have some time. What do you want to ask?";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "Is there something else you want to talk about?";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "Anything else?";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "Fine. I hope you know what you're doing.";
                case ControlContext.SuccessReleaseControl:
                    return "Finally, I can control my own body now.";
                case ControlContext.FailTakeControl:
                    return "No way.";
                case ControlContext.FailReleaseControl:
                    return "As much as I'd love to release you, right now isn't a good moment.";
                case ControlContext.NotFriendsEnough:
                    return "No way. I have no idea what you're capable of when controlling my body.";
                case ControlContext.ControlChatter:
                    if(PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Sardine))
                    {
                        if (Main.rand.Next(3) == 0)
                            return "Don't think of trying anything with my husband while in my body. Not only am I watching you, but he also knows it's you.";
                    }
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "You're not going to complain about the bag weight again, right? I had to carry it for years, so you can carry it for some hours.";
                        case 1:
                            return "There isn't much I can do in this state, so at least I can use the time to think.";
                        case 2:
                            return "You need to tell me something? I hope it's important.";
                    }
                case ControlContext.GiveCompanionControl:
                    return "If your goal is getting bored watching me do chores, then this is your opportunity.";
                case ControlContext.TakeCompanionControl:
                    return "There. Now be careful about what you do with my body!";
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
                    return "Don't make me regret saying this, but you can Bond-Merge yourself with me to go to places too dangerous for you. Period.";
                case UnlockAlertMessageContext.FollowUnlock:
                    if (PlayerMod.PlayerHasCompanionSummoned(Main.player[Main.myPlayer], 2))
                    {
                        return "Would you mind if I accompany my husband on your quest? In case he does something stupid, I mean.";
                    }
                    else if (PlayerMod.PlayerHasCompanion(Main.player[Main.myPlayer], 2))
                    {
                        return "I want to ask you, would you mind if I accompany you? A dame needs to take a walk sometimes, but I don't really know this world, or have any reason to explore it.";
                    }
                    else
                    {
                        return "I want to ask you, would you mind If I accompany you? You may end up bumping into my husband during your travels, so I want to be there, so I can pull his ear back home.";
                    }
                case UnlockAlertMessageContext.MountUnlock:
                    return "Say, would you mind if I mount on your back? This bag is weighing on my feet, and they're hurting.";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "I am a busy woman, so you could help me with some of my requests, while I try remembering where my house is at.";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "I have something important to tell you, [nickname]. Besides I still want to go back to my house, I'm at least grateful you let me stay here, and let me live comfortably meanwhile. What I say is.. If you're thinking about appointing me as your Buddy, I will allow it.";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "I assume you have an idea that since you're my buddy, I trust your decision on whatever you ask me to do. Just please don't make me regret that.";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "Depends on what you ask of me, [nickname].";
                case InteractionMessageContext.Accepts:
                    return "I can help with that.";
                case InteractionMessageContext.Rejects:
                    return "No. Simply no.";
                case InteractionMessageContext.Nevermind:
                    return "If you didn't need anything from me, then why did you ask for it?";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "Hmph. Fine.";
                case ChangeLeaderContext.Failed:
                    return "Not a chance.";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "Oh, so you're asking me if I want to be your Buddy. Are you sure?";
                case BuddiesModeContext.PlayerSaysYes:
                    return "Most of my heart is reserved for my husband, but I'll reserve a fraction for you. I hope you just don't use that as an excuse for me to do free housework.";
                case BuddiesModeContext.PlayerSaysNo:
                    return "I wondered so. Think well before asking someone such an important thing.";
                case BuddiesModeContext.NotFriendsEnough:
                    return "I hardly know you. How could I be your Buddy?";
                case BuddiesModeContext.Failed:
                    return "Not a chance.";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "What are you talking about? I can see that you are a Buddy, and can see the bonding line between both of you.";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "Yes, I can visit you. I hope you don't waste my time with triviality.";
                case InviteContext.SuccessNotInTime:
                    return "It's way too late for me to visit you now. Tomorrow I will visit you.";
                case InviteContext.Failed:
                    return "I'm busy here right now, so I can't visit you.";
                case InviteContext.CancelInvite:
                    return "Don't come with that excuse of the wrong number. If you didn't want me to visit, just don't ask.";
                case InviteContext.ArrivalMessage:
                    return "I'm here, [nickname]. What do you want of me?";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "Great. Even here we have crazy people wandering about.";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "Should... I leave you two alone?";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*I hope you don't mind if I collect some infos...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... You actually look cute when not with an angry face.*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    if(WorldMod.HasCompanionNPCSpawned(CompanionDB.Sardine))
                        return "*Hm... I catched [gn:"+CompanionDB.Sardine+"]'s scent...*";
                    return "*She's carrying a photo... Who's that black cat?*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*Alright, I now know you.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Wait, what are you going to do with that frying pan?*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "I really hope you don't die, because I'm having trouble carrying you home.";
                case ReviveContext.RevivingMessage:
                    {
                        bool IsPlayer = !(target is Companion);
                        List<string> Mes = new List<string>();
                        
                        if (!IsPlayer && (target as Companion).ModID == companion.ModID && (target as Companion).ID == CompanionDB.Sardine)
                        {
                            Mes.Add("Wait! Come on! Wake up! Don't leave me again!");
                            Mes.Add("Please, don't die! It took me a year to find you again! Your son is even waiting for you at home!");
                            Mes.Add("Open your eyes! Look at me! Please!");
                        }
                        else if (!IsPlayer && (target as Companion).ModID == companion.ModID && (target as Companion).ID == CompanionDB.Glenn)
                        {
                            Mes.Add("Oh my... [gn:"+CompanionDB.Glenn+"]! [gn:"+CompanionDB.Glenn+"]!! Please! Wake up!");
                            Mes.Add("No... Not my son! No!!");
                            Mes.Add("Don't worry, [gn:"+CompanionDB.Glenn+"], mommy is here!");
                        }
                        else
                        {
                            Mes.Add("You're safe... I'm here with you...");
                            Mes.Add("Here, this will make you feel better.");
                            Mes.Add("Shh... You'll be fine. Just rest.");
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "This is not the moment for you to take a rest!";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "Alright, you're with me now. Now wake up!";
                case ReviveContext.RevivedByItself:
                    return "Who leaves a damsel bleeding on the ground? You?";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "Yes, thank you. Maybe being around you all isn't that bad.";
                    return "I really hope you didn't try anything other than helping me.";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
