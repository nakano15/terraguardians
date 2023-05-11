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
                    return "Who are you? Did you see my husband somewhere?";
                case 2:
                    return "Ugh, I need some place to put off some steam.";
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
            Mes.Add("The people in your town are nice, but I preffer a quiet and less noisy place.");
            Mes.Add("I wont place my things on the floor, soon I'll be going back home. I just need to remember which world I lived.");
            Mes.Add("At first, this bag was being quite heavy on my shoulders. As I kept using it, started to feel ligher. Did I grow stronger?");
            Mes.Add("Most of the time I'm busy cleaning up the place, looks like nobody else does.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Dryad))
                Mes.Add("I tried asking [nn:" + Terraria.ID.NPCID.Dryad + "] for clues of which world I lived. She said that she also visited several worlds, so can't pinpoint places. I should be so lucky...");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("[nn:" + Terraria.ID.NPCID.Merchant + "] disappoints me everytime I check his store. He should improve his store stock.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                Mes.Add("[nn:" + Terraria.ID.NPCID.ArmsDealer + "] should be ashamed of selling such outdated guns.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                Mes.Add("Do you want to hear a joke? [nn:" + Terraria.ID.NPCID.Angler + "] doesn't knows how to catch two fishs at once, can you believe? Wait, you don't either? You must be kidding!");
            if (HasSardineMet && !CanTalkAboutCompanion(CompanionDB.Sardine) && !HasCompanionSummoned(CompanionDB.Sardine))
            {
                Mes.Add("Where is my stupid husband? I can't find him anywhere. Did he go to another adventure?");
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
                Mes.Add("My son came looking for me, but I still didn't found my husband.");
                Mes.Add("My son got quite sad when I told him that I didn't found his dad yet...");
                Mes.Add("[nickname], if you could help finding my husband, It will be great. He's a black cat with some spirit for adventure.");
            }
            /*switch (guardian.SkinID)
            {
                case BaglessSkinID:
                    Mes.Add("It's good to not have that weight on my back, It was already starting to ache.");
                    if(player.Male)
                        Mes.Add("You've been looking at me way too much since I removed the bag, why is that?");
                    Mes.Add("What? You're impressed that I'm actually strong? House work isn't easy thing. Or was it the bag?");
                    Mes.Add("I hope there's no thieves in your world, I really don't want to return home and find out my things are gone.");
                    break;
            }
            switch (guardian.OutfitID)
            {
                case DamselOutfitID:
                    Mes.Add("I also have taste for clothing, you know.");
                    Mes.Add("I'm glad that the Clothier managed to make this clothing, it just fits in me.");
                    Mes.Add("I feel like wanting to spend and afternoon at a beach now.");
                    break;
            }*/
            if (CanTalkAboutCompanion(0))
            {
                Mes.Add("I really love having [gn:0] in the town, I can ask him to do things without questioning.");
                Mes.Add("Everytime [gn:0] asks If I want to play some kids game, I ask him what is his age. That creates a delay of a day before he asks me again.");
            }
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("Sometimes I try getting beauty tips from [gn:1]. She seems to be expert on that.");
                if (CanTalkAboutCompanion(2))
                    Mes.Add("If I ever see [gn:1] bullying my husband again, she will regret!");
                Mes.Add("Looks like [gn:1] and I had the same objective, but the result...");
                if(NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                    Mes.Add("Have you passed through [nn:" + Terraria.ID.NPCID.Stylist + "]'s shop? I'm waiting about 4 hours for her to finish [gn:1]'s hair treatment so I can start mine.");
            }
            if (HasSardineMet)
            {
                Mes.Add("Once I remember which world I lived, I'm taking [gn:2] back with me.");
                Mes.Add("I used to be happy and cheerful, until [gn:2] happened. I should have heard my mom.");
                Mes.Add("I once say [gn:2] kill a giant monster alone, by using a Katana. I was so amazed with it, that I fell for him. Big mistake I did.");
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
                    Mes.Add("Wait, you're telling me that [gn:3] is [gn:1]'s boyfriend? She did one weird choice.");
            }
            if (CanTalkAboutCompanion(4))
            {
                Mes.Add("I can't really tell much about [gn:4], he doesn't say much, either.");
                Mes.Add("Sometimes I see [gn:4] starting at the dungeon entrance. I wonder what is on his mind.");
                Mes.Add("[gn:4] seems to have only one emotion. -_-");
            }
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("Sometimes [gn:5] makes me company. I love it when he lies down next to me while I'm doing somethings. I feel less alone.");
                Mes.Add("I'm not very fond of dogs, but [gn:5] is an exception. I guess I should thank his old owner for that.");
                Mes.Add("Sometimes I see [gn:5] staring at the moon. What could be coming on his mind?");
            }
            if (CanTalkAboutCompanion(6))
            {
                Mes.Add("[gn:6] keeps bragging how strong he is, until I challenged him on a arm wrestling.");
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Stylist))
                {
                    Mes.Add("Sometimes I think that [gn:6] should get a haircut from [nn:" + Terraria.ID.NPCID.Stylist + "], at least would be better than that thing he has on his head.");
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
                Mes.Add("If you want to make me feel annoyed, just leave me 5 minutes with [gn:" + CompanionDB.Michelle + "] in the same room.");
                Mes.Add("I hate [gn:" + CompanionDB.Michelle + "], she just don't stop talking!");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("If [gn:" + CompanionDB.Malisha + "] cause one more explosion, I will go have some serious talking with her.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("I don't really have something bad to say about [gn:" + CompanionDB.Fluffles + "], maybe It's because she doesn't speaks.");
                Mes.Add("Sometimes [gn:" + CompanionDB.Fluffles + "] presence makes my hair rise. You find really unusual people to live in your world.");
                bool HasBlue = CanTalkAboutCompanion(CompanionDB.Blue), HasZacks = CanTalkAboutCompanion(CompanionDB.Zacks), HasSardine = CanTalkAboutCompanion(CompanionDB.Sardine);
                if (HasSardine)
                {
                    if (HasBlue && HasZacks)
                    {
                        Mes.Add("Tell me, will [gn:" + CompanionDB.Sardine + "] even survive one of [gn:" + CompanionDB.Blue + "]'s bullying? Even I am having bad times trying to get all those guardians out of him.");
                        Mes.Add("The other day I had to help my husband get some bath, because he came home all slobbered, and with some bite marks on his body.");
                    }
                    else if (HasBlue && CanTalkAboutCompanion(CompanionDB.Fluffles))
                    {
                        Mes.Add("After [gn:" + CompanionDB.Fluffles + "] arrived, I had to stop [gn:" + CompanionDB.Blue + "] and her from chasing [gn:" + CompanionDB.Sardine + "] more frequently.");
                        Mes.Add("Do you have something that repells ghosts? I think [gn:"+CompanionDB.Sardine+"] might need something like that.");
                    }
                    Mes.Add("There was one time when [gn:"+CompanionDB.Sardine+"] returned home, and I got spooked after I saw [gn:"+CompanionDB.Fluffles+"] on his shoulder. I screamed so loud that she ran away, and I nearly dirtied the floor too.");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("[gn:" + CompanionDB.Minerva + "] still haven't got into the level for my refined taste. She still has a lot to cook.");
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
                    Mes.Add("[gn:" + CompanionDB.Alex + "] is not only keeping me company sometimes, but also plays with my son, [gn:" + CompanionDB.Glenn + "].");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("[gn:" + CompanionDB.Cinnamon + "] actually knows how to put seasoning to food well, but she sometimes exagerate a bit.");
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
                    Mes.Add("*Speaking to [gn:" + CompanionDB.Celeste + "] is really uplifting. She always manages to douse a bit the anger my husband causes to me.*");
                }
                else
                {
                    Mes.Add("*I wonder if that "+MainMod.TgGodName+" [gn:"+CompanionDB.Celeste+"] talks about will help me find my husband.*");
                }
            }
            /*if (guardian.IsPlayerRoomMate(player))
            {
                if (player.Male)
                    Mes.Add("Okay, I can share my bedroom. Just don't try anything funny during the night.");
                Mes.Add("As long as you keep It clean, you can use It for as long as you want.");
                Mes.Add("If you get a bed for yourself, I can let you stay in my bedroom.");
                if (HasSardineMet)
                {
                    Mes.Add("I'm sorry, but I kind of would preffer sharing my room with [gn:" + Sardine + "].");
                }
            }
            if (NpcMod.IsGuardianPlayerRoomMate(player, Sardine))
            {
                if (!player.Male)
                    Mes.Add("I hope you aren't trying to get \'intimate\' with my husband. Remember that we are still married.");
                Mes.Add("I will never understand why [gn:"+Sardine+"] shares his room with you, but not me. I didn't made him sleep in the sofa latelly.");
            }*/
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
                Mes.Add("Eek!! Turn the other side!");
                Mes.Add("Do you really have to enter here and talk to me while I'm using the toilet?");
            }
            if (PlayerMod.IsPlayerControllingCompanion(player, CompanionDB.Sardine))
            {
                Mes.Add("You are not my husband. [nickname], why are you Bond-Linked to my husband?");
                Mes.Add("I know it's you, [nickname]. I can feel your presence alongside my husband's.");
                Mes.Add("Since you're Bond-Linked with my husband, can you try figuring out where is his head at?");
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
                Mes.Add("Do you want to know how to anger [gn:1]? Easy, throw a bucket of water on her hair. Now, do you know how much long it takes for her anger to pass?");
            if (CanTalkAboutCompanion(2))
            {
                Mes.Add("Sometimes I don't know if [gn:2] even cares about me. It's like, his adventures are the top priority.");
                Mes.Add("I don't entirelly hate [gn:2], but what he has done isn't okay. Beside I shouldn't throw a stone, either.");
            }
            Mes.Add("Maybe you can help me remember which world I came from. It had a grass land, then there were that evil land, It also had a dungeon, and a jungle... All worlds have those? Oh...");
            Mes.Add("Sometimes I like to rest on a window.");
            Mes.Add("I like chasing butterflies, but they always escape.");
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
                Mes.Add("...My home... How are things... (She seems to be worried about her home.)");
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
                        return "That doesn't mean I'll give you a star of good person. But... You're nice.";
                    return "Maybe that will make me be less furious.";
                case RequestContext.Accepted:
                    return "Good. Don't delay too long with the request.";
                case RequestContext.TooManyRequests:
                    return "What? No way. You have many things to do right now.";
                case RequestContext.Rejected:
                    return "Hmph. I should have wondered that It would be too hard for you.";
                case RequestContext.PostponeRequest:
                    return "Hey, but I need that now! *Sigh* Whatever, go do your things.";
                case RequestContext.Failed:
                    return "Good job, you managed to ruin everything. Now go away!";
                case RequestContext.AskIfRequestIsCompleted:
                    return "Wait, did you actually completed what I asked you for?";
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
                    return "Fine, keep your stinky house then. I wont need it anymore once I remember where is my house at.";
                case MoveOutContext.Fail:
                    return "Not at this time.";
                case MoveOutContext.NoAuthorityTo:
                    return "Who do you think you are to try evicting me?";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "I really needed a break from housekeeping, anyway. I hope you don't make me miss that.";
                case JoinMessageContext.FullParty:
                    return "I may be small, but I don't think I will fit in that group of yours.";
                case JoinMessageContext.Fail:
                    if (!PlayerMod.PlayerHasCompanion(MainMod.GetLocalPlayer, CompanionDB.Sardine))
                    {
                        return "I can't right now, I'm looking for clues about where is my husband.";
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
                    return "Here?! Right here?! You want to leave me here all alone?! Are you nuts?";
                case LeaveMessageContext.Success:
                    return "I need to rest my legs anyway.";
                case LeaveMessageContext.Fail:
                    return "You're not leaving me here right now.";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "Yes, fine. Leave a damsel fight her way back to home all alone...";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Okay, let's find a safe place for me before we leave, then.";
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
                    return "I hope you wont let me fall.";
                case MountCompanionContext.Fail:
                    return "Not this time.";
                case MountCompanionContext.NotFriendsEnough:
                    return "I don't trust you enough for that.";
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
                    return "Fine. I'll avoid keeping contact of foes.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "Alright. I'll totally avoid contact with foes, then.";
                case TacticsChangeContext.Nevermind:
                    return "Then what was that for?";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "You want to talk? Fine, I think I have some time. What you want to ask?";
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
                            return "Don't think of trying something with my husband while in my body. Not only I'm watching, but he also knows it's you.";
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
                    return "Don't make me regret saying this, but you can Bond-Merge yourself with me to go to places too dangerous for oyu. Period.";
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
                    return "Say, would you mind if I mount on your back? This bag is weightening my feet, and they are hurting.";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "I am a busy woman, so you could help me with some of my requests, while I try remembering where my house is at.";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "I have something important to tell you, [nickname]. Beside I still want to go back to my house, I'm at least grateful you let me stay here, and let me live comfortably meanwhile. What I say is.. If you think about appointing me as your Buddy, I will not deny.";
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
                    return "If you didn't needed anything from me, then why you asked for?";
            }
            return base.InteractionMessages(companion, context);
        }
    }
}