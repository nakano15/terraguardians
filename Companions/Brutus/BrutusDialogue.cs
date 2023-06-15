using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class BrutusDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch(Main.rand.Next(3))
            {
                default:
                    return "*If you need a bodyguard, you just need to call me.*";
                case 1:
                    return "*I wont charge a fee for my services now, since someone already paid it. Just call me whenever you need help.*";
                case 2:
                    return "*I will get rusty if I don't get into constant action. If there is trouble in your path, give me a call.*";
            }
        }

        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            TerraGuardian guardian = (companion as TerraGuardian);
            List<string> Mes = new List<string>();
            Mes.Add("*Don't worry, your town citizens are safe with me around.*");
            Mes.Add("*One thing I disliked about my last job, was standing around, waiting for something to happen. Here is not so much different, but at least the people are cool.*");
            Mes.Add("*When possible, take me with you on your travels, I need to strengthen myself with the heat of the battle.*");
            Mes.Add("*Don't tell anyone, but I like dancing when there is nobody around. I've got to keep the \"Tough guy\" fame, you know.*");
            Mes.Add("*I have trained my left arm with several kinds of swords, so I can even wield two handed swords with it only.*");
            Mes.Add("*My right arm helps on my protection, so I can shield myself from a number of attacks.*");
            Mes.Add("*My job as a Royal Guard on the Ether Realm... Wasn't good...*");
            Mes.Add("*Many Terrarians usually get shocked when I showed them my weapons, so I'll tell you why I have them. Those are the weapons the guards on my world uses. The day I lost my job, they at least let me keep my weapons.*");
            Mes.Add("*If you're asking yourself about what's the contract about, don't worry. The contract only expires if either of us dies for good. Until that doesn't happens, I'm bound to you for life.*");
            if(companion.IsHungry)
            {
                Mes.Add("*I'm so hungry that I could eat a shark whole.*");
                Mes.Add("*I need something to eat. I feel like there's a hole in my belly.*");
            }
            if (Main.bloodMoon)
                Mes.Add("*Hey, let's go outside? That's the perfect occasion to hone our combat skills.*");
            if (Main.bloodMoon || Main.eclipse)
                Mes.Add("*Don't worry, no creatures will harm your citizens. I guarantee.*");
            if (Main.eclipse)
                Mes.Add("*Where did those creatures came from?*");
            if (NPC.AnyNPCs(19))
                Mes.Add("*Sometimes [nn:19] helps me with my training.*");
            if (Main.hardMode && NPC.AnyNPCs(22))
                Mes.Add("*Do you know why sometimes [nn:22] simply... Explodes?*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.TravellingMerchant))
                Mes.Add("*Part of the products [nn:" + Terraria.ID.NPCID.TravellingMerchant + "] carries comes from the Ether Realm. I wonder how he acquires those, since that place is too dangerous for Terrarians.*");
            if(Main.invasionType == Terraria.ID.InvasionID.MartianMadness && Main.invasionSize > 0)
                Mes.Add("*Don't stay outside for too long, I don't know If I'll be able to rescue you if you get beamed up.*");
            if (Terraria.GameContent.Events.DD2Event.DownedInvasionAnyDifficulty)
                Mes.Add("*The Old One's Army is the perfect event for training my bodyguard skills. If I be able to protect the crystal, I can protect anyone.*");
            Mes.Add("*You want to visit the Ether Realm? I'm not sure if you will be able to, things are very tough there for Terrarians.*");
            /*switch (guardian.OutfitID)
            {
                case RoyalGuardOutfitID:
                    Mes.Add("*Halt! What do you think? It fits me, right?*");
                    Mes.Add("*This outfit means so much for me: I used it on my last job for years, and using it again brings me memories.*");
                    Mes.Add("*There is no way I can convince you to tell me who gave you this, right?*");
                    break;
            }*/
            if (CanTalkAboutCompanion(0))
                Mes.Add("*[gn:0] has what it takes to be a good warrior, he just needs to take things a bit more serious.*");
            if (CanTalkAboutCompanion(1))
            {
                if (!CanTalkAboutCompanion(3))
                {
                    Mes.Add("*I think [gn:1] is really cute, do you think she would date me?*");
                    Mes.Add("*I think I should try asking [gn:1] for a date, she has some... Good moves.*");
                }
                else
                {
                    Mes.Add("*My plans on asking [gn:1] on a date went downhill, since she still loves that creepy zombie.*");
                    Mes.Add("*I got the chance to talk with [gn:1] early this day, but had to back away when [gn:3] appeared.*");
                }
                if (CanTalkAboutCompanion(2))
                {
                    Mes.Add("*I'm not a fool, I'm certain that what [gn:1] does to [gn:2] is just to bully him. I hope he has an iron will, or else...*");
                    Mes.Add("*Do you think I should extend my bodyguard job to [gn:2] too? I don't like what [gn:1] does to him. But I don't want her to think badly of me, that could hurt my chan- I mean, my professionalism. What should I do?*");
                    Mes.Add("*Earlier this day, [gn:2] tried to hire me to give a lesson to [gn:1]. I refused it for... Some reason.*");
                }
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("*You got a zombie on your town? How were you able to do that? I mean... Wow!*");
                Mes.Add("*Do I need to look out for [gn:3]? He may offer some danger to the people in the town?*");
                Mes.Add("*If [gn:3] tries something funny, I'll slice him in half with my sword.*");
                Mes.Add("*[gn:3] seems to be a cool guy, but I keep having a chill going down my spine whenever I get near him.*");
            }
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("*I think [gn:5] isn't going to protect anyone from danger just by playing all the time.*");
                Mes.Add("*I have to say, sometimes when I'm on a break, I play a bit with [gn:5]. But I hate it when he ruins my mane.*");
                Mes.Add("*I wonder who is that " + AlexRecruitmentScript.AlexOldPartner + ", If I were there at the time she died, would she have survived? I keep wondering about that kind of thing.*");
            }
            Mes.Add("*The chairs you placed on my house are too small. If you open a hole in the middle of them I could use them for something else.*");
            if (CanTalkAboutCompanion(7))
            {
                Mes.Add("*Before you ask, [gn:7] didn't exactly won clean the arm wrestling, she unsheathed the claws before the countdown ended.*");
                if (CanTalkAboutCompanion(3))
                {
                    Mes.Add("*Sometimes I do a side job for [gn:7], I don't mean to talk about my contracts but... She asked me to protect her from [gn:3]. Did he threaten her or something?*");
                }
                Mes.Add("*I am lucky for having pretty girls around this world. But you had to find some that already have a partner?!*");
                Mes.Add("*Sometimes I have a few drinks with [gn:7], she always complains about her husband. Well, almost aways, sometimes she tells funny stories about his frustrated adventures, that's why I keep drinking with her.*");
            }
            if (CanTalkAboutCompanion(8))
            {
                Mes.Add("*Everytime I speak with [gn:8], meat comes on my mind.*");
                Mes.Add("*My luck seems to be changing, [gn:8] is single and cute. Do you think I have a chance?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                Mes.Add("*Of all Guardians you could have let move in, you had to allow [gn:" + CompanionDB.Domino + "] to open a shop here? Do you know how much trouble he made me pass through on the Ether Realm?*");
                Mes.Add("*Remember when I promissed to keep all your town citizens safe? May I do an exception for [gn:" + CompanionDB.Domino + "]?*");
                Mes.Add("*Hey, do you have any laws against smuggling in your world? I would be glad to arrest [gn:" + CompanionDB.Domino + "] for that.*");
                Mes.Add("*[gn:" + CompanionDB.Domino + "] always managed to escape from the guards on the Ether Realm somehow. I never managed to find out how.*");
                Mes.Add("*Keep an eye close on [gn:"+CompanionDB.Domino+"]. If that mutt does something you disapprove, come tell me.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("*I sure don't take a break on this world. Now [gn:"+CompanionDB.Leopold+"] want me to protect him from [gn:"+CompanionDB.Zacks+"]. Maybe I should start charging per hour?*");
                }
                Mes.Add("*You're looking for [gn:" + CompanionDB.Leopold + "]? I saw him being chased by bees earlier this day.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*Yo, tell [gn:"+CompanionDB.Vladimir+"] to keep his paws off me. I can't be seen being hugged by other people, that can put my career down the drain.*");
                Mes.Add("*Can I tell you something? I also have my own troubles that I want to confess, but I can't show any sign of weakness, or else people around will start doubting that I can protect them.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*If you call me giant kitty, I will punch your stomach.*");
                Mes.Add("*Don't listen to what [gn:"+CompanionDB.Michelle+"] says about me, I'm as tough as a rock.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*Hey, did you came from [gn:" + CompanionDB.Malisha + "]'s house? Do you know if she needs a test subject for something?*");
                Mes.Add("*I really like participating of [gn:" + CompanionDB.Malisha + "]'s experiement, that way I can stay close to her for quite a long time.*");
                Mes.Add("*[gn:" + CompanionDB.Malisha + "] once casted a shrinking spell on me, I would normally have been scared of that, if It wasn't for the view of her I had. I mean... Wow! I hope she repeats that experiement in the future.*");
                Mes.Add("*Do you think [gn"+CompanionDB.Malisha+"] and I... No... Nevermind... Why am I talking about this to you?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Wrath))
            {
                Mes.Add("*You're saying that [gn:" + CompanionDB.Wrath + "]'s punches hurts? Funny, I didn't felt anything whenever he punched me.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fear))
            {
                Mes.Add("*I would like [gn:" + CompanionDB.Fear + "] to stop coming seek me everytime he gets scared at anything.*");
                Mes.Add("*With [gn:" + CompanionDB.Fear + "] screaming around, it's really hard to be ready for a true emergency.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*A ghost now? Your life seems full of weird frienship.*");
                Mes.Add("*I was alerted at first by [gn:" + CompanionDB.Fluffles + "], until I noticed that she doesn't seems like a bad ghost... Err... Person.*");
                Mes.Add("*I kind of would like seeing [gn:"+CompanionDB.Fluffles+"]... Float... In front of me.*");
                if (CanTalkAboutCompanion(CompanionDB.Sardine) && CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    if (CanTalkAboutCompanion(CompanionDB.Zacks))
                    {
                        Mes.Add("*Sometimes I have to stop [gn:" + CompanionDB.Blue + "] and their friends game on [gn:" + CompanionDB.Sardine + "] because things goes way too far.*");
                    }
                    Mes.Add("*It seems like [gn:" + CompanionDB.Sardine + "] is in a even bigger problem now that [gn:"+CompanionDB.Fluffles+"] joined [gn:"+CompanionDB.Blue+"]'s game.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*[nickname], where did you find that angel known as [gn:"+CompanionDB.Minerva+"]? That woman cooks several tasty foods for me. I'm really glad that you found her.*");
                Mes.Add("*Aaaahh... I'm stuffed. [gn:"+CompanionDB.Minerva+"] really cooks very well. I'll see her in again only in about 8 hours.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                bool HasBreeMet = HasCompanion(CompanionDB.Bree), HasSardineMet = HasCompanion(CompanionDB.Sardine);
                if (!HasBreeMet && !HasSardineMet)
                {
                    Mes.Add("*Sometimes I'm walking the lands around the town, to see if I can find [gn:"+CompanionDB.Glenn+"]'s parents. I have the feeling that they are alive, and out there.*");
                }
                else if (HasBreeMet && HasSardineMet)
                {
                    Mes.Add("*I'm glad that [gn:"+CompanionDB.Glenn+"] managed to find his parents.*");
                }
                else if (HasBreeMet)
                {
                    Mes.Add("*It seems like [gn:" + CompanionDB.Glenn + "]'s mother has already been found, but his father is still missing.*");
                }
                else if (HasSardineMet)
                {
                    Mes.Add("*It seems like [gn:" + CompanionDB.Glenn + "]'s father has already been found, but his mother is still missing.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*It's very reckless of [gn:" + CompanionDB.Cinnamon + "] to go gather ingredients alone outside of the town. From now on, she need to tell me so I can ensure her safety when doing so.*");
                Mes.Add("*I don't think [gn:" + CompanionDB.Cinnamon + "] has what It takes to survive outside city walls. If she gets in danger or hurt, I wont feel good.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("*[gn:"+CompanionDB.Green+"] keeps saying everytime I visit him that my wounds are light. Maybe I'm exagerating a bit on the visits.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*[gn:" + CompanionDB.Miguel + "] is a good addition to your world. I can strain my muscles and get even stronger for my job.*");
                Mes.Add("*[gn:" + CompanionDB.Miguel + "] thought he could beat me on arm wrestling. Hahaha.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cille))
            {
                Mes.Add("*I'm really interessed in offering my bodyguard job to [gn:" + CompanionDB.Cille + "], but she always tells me to leave her alone.*");
                Mes.Add("*If you manage to make [gn:" + CompanionDB.Cille + "] open up, please come tell me how you did that.*");
                Mes.Add("*I've been hearing rummors of [gn:" + CompanionDB.Cille + "] attacking people, but I can't believe she would do such a thing.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Celeste))
            {
                Mes.Add("*I never thought our religion would come to this realm, but it seems like [gn:"+CompanionDB.Celeste+"] did it.*");
                Mes.Add("*Yes, I know about "+MainMod.TgGodName+". I actually was blessed by one of his priests when I finally joined the Royal Guard.*");
                Mes.Add("*I wonder if I would have any luck of calling [gn:"+CompanionDB.Celeste+"] for a few drinks. Priestesses generally are busy, but maybe...*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*I have to say, the way I'm sitting, is easier for me to do this. But... Do you really have to keep staring at me?*");
                Mes.Add("*I wonder how many times I will have to flush this thing.*");
            }
            if (PlayerMod.IsPlayerControllingAnyOfThoseCompanions(MainMod.GetLocalPlayer, 
                new CompanionID(CompanionDB.Blue), new CompanionID(CompanionDB.Bree), new CompanionID(CompanionDB.Luna), 
                new CompanionID(CompanionDB.Celeste), new CompanionID(CompanionDB.Cille), new CompanionID(CompanionDB.Fluffles), 
                new CompanionID(CompanionDB.Mabel), new CompanionID(CompanionDB.Malisha), new CompanionID(CompanionDB.Minerva)))
            {
                Mes.Add("*Y-yes [nickname], I know it's you. My look? I'm just... Surprised. That's it.*");
                if (!HasCompanionSummoned(CompanionDB.Brutus))
                {
                    Mes.Add("*[nickname]? You're Bond-Merged with [controlled]? May I... Protect you on your journey?*");
                }
                else
                {
                    Mes.Add("*Don't worry, [nickname]. I'm following.. Behind..*");
                }
            }
            /*if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*I promisse to protect you while you sleep. I wont close my eyes during the entire night.*");
                Mes.Add("*If anything tries to attack you while sleeping, will never expect me to be here. You will be safe.*");
                Mes.Add("*So, you need my protection during the night? I can help you with that.*");
            }*/
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*[nickname], there is... A ghost... On your shoulder...*");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*Huh? Nothing. Nothing right now, try other time.*";
                    return "*I don't think so. I have everything I need right now.*";
                case RequestContext.HasRequest:
                    //if (guardian.request.Base is TravelRequestBase)
                    //    return "*I need to patrol the world for troubles. Can you [objective]?*";
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I have no time to look for this myself, so I ask you to do something for me. I need you to [objective], can you do it?*";
                    return "*I can't leave the town unprotected, so I have a thing to ask you for. Can you [objective]?*";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*Great job, I knew you would do it.*";
                    return "*You were able to complete the task, very nice.*";
                case RequestContext.Accepted:
                    return "*Try not to get yourself killed while doing my request, [nickname]. If necessary, bring me with you.*";
                case RequestContext.TooManyRequests:
                    return "*I think you may end up getting stressed out if I give you another request. Maybe do something about the ones you have.*";
                case RequestContext.Rejected:
                    return "*Maybe I asked too much. I'll try doing that later, when I'm off service.*";
                case RequestContext.PostponeRequest:
                    return "*I guess I can put this on hold.*";
                case RequestContext.Failed:
                    return "*You couldn't do what I asked, right? I should have thought before giving you such a request.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Say, have you do what I asked?*";
                case RequestContext.RemindObjective:
                    return "*Hm... [objective] is what I asked you for.*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*What? Is It too dangerous for you, [nickname]? If is, I shouldn't have given it to you.*";
                case RequestContext.CancelRequestYes:
                    return "*Hm... Okay. I'll see if I can do that in my spare time.*";
                case RequestContext.CancelRequestNo:
                    return "*Oh, okay.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Those evil zones have no chance of getting near the town, right?*");
            if (CanTalkAboutCompanion(1))
                Mes.Add("*Do you know of any gift that would be good for [gn:1]?*");
            Mes.Add("*The day I lost my job? My king tasked me into joining an attack to a village of farmers, but I refused. I just didn't got exiled due to the help of a friend of mine, who also were a captain of the royal guard.*");
            Mes.Add("*I was once the captain of the Royal Guard in the ether realm, you know. Right now, I'm just a guard on your town. Maybe I can make use of what I learned on my path on your world, too.*");
            Mes.Add("*Does your world have some kind of ruler? Are you some kind of ruler? Or does your people live free?*");
            Mes.Add("*Sometimes I visit the Ether realm. You know, I have parents there. Maybe one day you'll meet them.*");
            Mes.Add("*For some reason, several people from the Ether Realm are leaving it recently. I wonder why is that happening.*");
            Mes.Add("*If you are wondering why so many TerraGuardians are appearing on your world, is because they are leaving the Ether Realm. I don't know why, but I don't think It's a really bad thing.*");
            Mes.Add("*Most TerraGuardians have family and friends, and they are also friendly. But of course there are some who aren't very friendly, or follow ethics and law.*");
            if(CanTalkAboutCompanion(5))
                Mes.Add("*I wonder, does riding on [gn:5]'s back makes me into a Knight?*");
            //if (CanTalkAboutCompanion(2) && GuardianBountyQuest.SignID > -1)
            //    Mes.Add("*I have interest on [gn:2]'s bounties, I think that will help improve safety on your world... And also my combat abilities.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*Yes, I can stay here and ensure the safety of the citizens.*";
                case MoveInContext.Fail:
                    return "*I know that we have a contract, but I don't want to move in to this world right now.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I can protect you on your travels, but I'm not willing to stay here now.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*That will make my job harder, but I will respect your choice.*";
                case MoveOutContext.Fail:
                    return "*This is not a good moment for that.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*It wasn't you who put me stationary here.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*You have my sword and shield, [nickname].*";
                case JoinMessageContext.Fail:
                    return "*Not a good moment right now.*";
                case JoinMessageContext.FullParty:
                    return "*It will be very difficulty protecting you, when I wont even be able to walk due to so many people.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Understud. Call me whenever you get into a dangerous mission.*";
                case LeaveMessageContext.Fail:
                    return "*I will not leave you right now. I must ensure your safety first.*";
                case LeaveMessageContext.AskIfSure:
                    return "*I think It would be better if I leaved the company in a safe place. Not that anything out there could hurt me, you know.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Very well. I will return to the town then.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*I shall guard your life some more then.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Yes. Sit on my shoulder. I will protect you from attacks.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*I will protect your rear guard.*";
                case MountCompanionContext.Fail:
                    return "*Not a good moment for that.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*I need both arms to guard your life. Sorry.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I wont mind carrying a little weight.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*I can do that. Who should I carry?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Understud.*";
                case DismountCompanionContext.Fail:
                    return "*No way. Not now.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*I hope the bed is big enough for both of us.*";
            return "*I will find a bed for myself, then.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*That's odd, but might make it easier for me to protect you from attacks.*";
            return "*Fine. I will take another chair. At least will be less odd.*";
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    List<string> Mes = new List<string>();
                    Mes.Add("(You wonder if the way he sleeps wont make him have pain all over the body, during the morning.)");
                    Mes.Add("(His bad breath from his snoring reaches your nose, making you plug It.)");
                    Mes.Add("(He's sleeping like a stone, you wonder if you could wake him up whenever something happens.)");
                    return Mes[Main.rand.Next(Mes.Count)];
                case SleepingMessageContext.OnWokeUp:                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "*W-Wha?! No, I wasn't sleeping, I was just... Reflecting!*";
                        case 1:
                            return "*[nickname]! It's not what you're thinking, I didn't closed my eyes a second.*";
                        case 2:
                            return "*What am I doing in this bed? I was watching the town for troubles.*";
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "*Huh? I can explain... Oh, request? Yes. Request, what is It? Did It?*";
                        case 1:
                            return "*When did you... I mean... Yes, I was totally expecting you, did you do what I asked?*";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*I need to change how I take on combat? Very well, what is your suggestion?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*Let our foes come, I will be taking on them.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*I shall take on combat a bit cautiously.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Not exactly my kind of combat, but I will do that.*";
                case TacticsChangeContext.Nevermind:
                    return "*Very well.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*You want to hangout? Fine, let's have come chatting.*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Anything else you want to talk about?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Understud. Anything else you want to talk about?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return Main.rand.Next(2) == 0 ? "*We may, [nickname]. Will even make my job of protecting you easier.*" : "*Yes, [nickname]. I want to see any foe try reaching you now.*";
                case ControlContext.SuccessReleaseControl:
                    return "*Alright [nickname]. I will still keep doing my best to keep you safe from harm.*";
                case ControlContext.FailTakeControl:
                    return "*Not the moment for that right now.*";
                case ControlContext.FailReleaseControl:
                    return "*I'm sorry, but I deny. Your safety comes first.*";
                case ControlContext.NotFriendsEnough:
                    return "*I think I can better protect you while having full control of my body.*";
                case ControlContext.ControlChatter:
                    if(Main.rand.NextFloat() < 0.6f && PlayerMod.PlayerHasAnyOfThoseCompanionsSummoned(MainMod.GetLocalPlayer, 
                        new CompanionID(CompanionDB.Blue), new CompanionID(CompanionDB.Bree), new CompanionID(CompanionDB.Luna), 
                        new CompanionID(CompanionDB.Celeste), new CompanionID(CompanionDB.Cille), new CompanionID(CompanionDB.Fluffles), 
                        new CompanionID(CompanionDB.Mabel), new CompanionID(CompanionDB.Malisha), new CompanionID(CompanionDB.Minerva)))
                    {
                        switch(Main.rand.Next(3))
                        {
                            default:
                                return "*Ah, [nickname]... Uh... We Have some good.. Company... Good job..*";
                            case 1:
                                return "*What, [nickname]? Why I'm acting weird? D-don't mind that. Keep your head on the battle.*";
                            case 2:
                                return "*Mind uh.. Staring more often that way..? We gotta keep an eye on our friends, you know.*";
                        }
                    }
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*My body is your shield, [nickname], and my weapons are yours too. Use them well.*";
                        case 1:
                            return "*I'm not really a fan of being on the backseat, but I trust you know what you're doing.*";
                        case 2:
                            return "*Need any combat advices, [nickname]? Watching you fight is making me see some flaws.*";
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
                    return "*Hey boss, If you have any mission you need someone to do, feel free to Bond-Merge with me. My body is the best shield I can provide you.*";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*If we end up getting surrounded by creatures, just tell me to put you on my shoulders, I can handle the attacks while you help me mow down the creatures.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*[nickname], I think that this contract of ours has leaded to a great friendship. I want to let you know that if you want me as your bodyguard forever, you may appoint me as your Buddy. It would be a great honor for me to serve and protect you as your Buddy.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*As your recently appointed buddy, I feel obligated to let you know about this: Whatever you ask of me, I will do without questioning. This is my way of saying I completelly trust your decisions, and what you say shall be.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*Depending on what is it, I can do it. What is it that you want?*";
                case InteractionMessageContext.Accepts:
                    return "*I will do so.*";
                case InteractionMessageContext.Rejects:
                    return "*I can't do that.*";
                case InteractionMessageContext.Nevermind:
                    return "*Understud.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*I shall lead this group as you ask.*";
                case ChangeLeaderContext.Failed:
                    return "*I refuse to do that.*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*You finally came to ask me to be your Buddy, huh? Doing this, means we are bound for life. Do you want me to be your \"buddyguard\" for life?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Take this as my vow to you, that I will protect you until the end of our lives, my buddy.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*Oh.. Maybe I shouldn't have used too many fancy words.*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I want just to stick to the contract for now. Maybe if we're more friends.*";
                case BuddiesModeContext.Failed:
                    return "*No.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*You're already bound to someone else. We shouldn't even be talking about this, since could make that person heartbroken.*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*Yes, I can visit you. I might be there anytime now.*";
                case InviteContext.SuccessNotInTime:
                    return "*I can visit you, but you'll have to wait until tomorrow for me to arrive.*";
                case InviteContext.Failed:
                    return "*This is not a good moment for that. Try inviting me another time.*";
                case InviteContext.CancelInvite:
                    return "*I see. Well then, I'll return to where I was, then.*";
                case InviteContext.ArrivalMessage:
                    return "*I arrived, [nickname].*";
            }
            return "";
        }
    }
}