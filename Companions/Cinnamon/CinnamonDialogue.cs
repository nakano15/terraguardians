using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Cinnamon
{   
    public class CinnamonDialogues : CompanionDialogueContainer
    {
        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*I think you're a cool person. You can call me anytime for your adventures.*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*If you want, I can carry you on my shoulder.*";
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "*This place seems actually nice. May I live here with you?*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*As long as you help me get more tasty food, I can let you control me.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string GreetMessages(Companion companion)
        {
            if (MainMod.GetLocalPlayer.HasItem(ItemID.BowlofSoup))
                return "*(Snif, Snif) Humm.... (Snif, Snif) You have something that smells delicious. Could you share It with me?*";
            if (Main.rand.NextBool(2))
                return "*Oh, hello. Do you like tasty food too?*";
            return "*Hi, I love tasty foods. What do you love?*";
        }

        public override string NormalMessages(Companion guardian)
        {
            if (guardian.IsSleeping)
            {
                switch (Main.rand.Next(3))
                {
                    default:
                        return "*Nom nom nom nom nom nom...* (She seems to be dreaming about eating lots of food)";
                    case 1: return "*Zzzzz.... (Snif snif) Hmmm....* (She smelled something good.)";
                    case 2: return "*Zzzz... Zzzzzz..... More food.... Seasoning.... Eat.... Zzzzz...*";
                }
            }
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextFloat() < .75f)
            {
                return "*Aahh!! There's a ghost on your back!!*";
            }
            List<string> Mes = new List<string>();
            if (Main.bloodMoon)
            {
                Mes.Add("*What now?!*");
                Mes.Add("*Don't you have someone else to bother?*");
                Mes.Add("*Enough!*");
                Mes.Add("*No! I wont cook for you!*");
            }
            else
            {
                Mes.Add("*I'm loving my time here.*");
                Mes.Add("*(singing) Lalala...*");
                Mes.Add("*Hello [nickname], want something?*");
                Mes.Add("*Are you cooking something? I want to see your cooking secrets, teehee.*");
                Mes.Add("*Happiness is contagious, am I right?*");

                if (!guardian.HasBuff(BuffID.WellFed))
                {
                    Mes.Add("(Growl) *Oh, my stomach is complaining... Is "+(Main.dayTime ? "lunch" : "dinner")+" ready?*");
                    Mes.Add("*I hate being hungry... I want to eat something...*");
                    Mes.Add("*Ow... I think I should be cooking something to eat.*");
                }
                if (guardian.IsUsingToilet)
                {
                    Mes.Clear();
                    Mes.Add("*I'm quite busy doing the number 2 here... Go away, please.*");
                    Mes.Add("*I really didn't wanted you to see me like this.*");
                    Mes.Add("*I can't concentrate with you staring at me.*");
                }
                else if (Main.eclipse)
                {
                    Mes.Add("*I'm sorry, but I really feel like locking myself inside my house now.*");
                    Mes.Add("*Do you think I'll be safe if I lock myself in the toilet?*");
                    Mes.Add("*Please, get those horrible creatures away!*");
                }
                else if (Main.dayTime)
                {
                    if (!Main.raining)
                    {
                        Mes.Add("*What a beautiful day.*");
                        Mes.Add("*I like seeing butterflies flying and critters around.*");
                    }
                    else
                    {
                        Mes.Add("*Oh nice... It's raining... Well.... I'll take the day off.*");
                        Mes.Add("*Acho~! Sorry... I'm alergic to this weather.*");
                        Mes.Add("*I'm getting a bit drowzy...*");
                    }
                }
                else
                {
                    Mes.Add("*What a silent night.*");
                    Mes.Add("*I think I saw something moving in the dark.*");
                    Mes.Add("*It's too quiet, and that doesn't makes me feel okay.*");
                }
                if (guardian.IsPlayerRoomMate(MainMod.GetLocalPlayer))
                {
                    Mes.Add("*I love having you as my room mate. Your help when making the morning set will be very helpful.*");
                    Mes.Add("*It's nice sharing the room with you.*");
                }
                //
                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    Mes.Add("*[gn:"+CompanionDB.Rococo+"] helps me with testing food. He seems to enjoy that.*");
                    Mes.Add("*Sometimes [gn:"+CompanionDB.Rococo+"] brings trash, wanting me to add them to the food. I keep denying but he keeps bringing them.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    Mes.Add("*[gn:" + CompanionDB.Blue + "] has a really cool hair.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    Mes.Add("*I were learning how to cook from [gn:" + CompanionDB.Bree + "].*");
                    Mes.Add("*[gn:" + CompanionDB.Bree + "] isn't that much grumpy when you talk about something she likes.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Glenn))
                {
                    Mes.Add("*Sometimes I play with [gn:"+CompanionDB.Glenn+"]. I like that there's someone of my age around.*");
                    Mes.Add("*There are some times where I don't like playing a game with [gn:"+CompanionDB.Glenn+"], he can't accept when I win.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                {
                    Mes.Add("*[gn:"+CompanionDB.Brutus+"] told me not to wander on the outside alone... He said that I should call him when I'm going to do so. But I'm already a grown girl, I can take care of myself!*");
                    Mes.Add("*I offered [gn:" + CompanionDB.Brutus + "] some food earlier, It looked like he liked It.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                {
                    Mes.Add("*A lot of people seems to be scared of [gn:" + CompanionDB.Fluffles + "], but I'm not.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Liebre))
                {
                    Mes.Add("*I saw [gn:" + CompanionDB.Liebre + "] watching me the other day. Am I going to die?*");
                    Mes.Add("*It's so scary! Sometimes I'm playing around my house, I see [gn:" + CompanionDB.Liebre + "] watching me. Is my time coming? I don't want to die!*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                {
                    Mes.Add("*Sometimes, I ask [gn:" + CompanionDB.Vladimir + "] to help me test food. He has an accurate taste for seasoning, he impresses me.*");
                    Mes.Add("*I'm curious to meet [gn:" + CompanionDB.Vladimir + "]'s brother. Is he as nice as him?*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Mabel))
                {
                    Mes.Add("*Why people keep staring at [gn:" + CompanionDB.Mabel + "]? It's so weird.*");
                    Mes.Add("*I asked [gn:" + CompanionDB.Mabel + "] if she wanted to test the newest food I cooked, but she said she can't, or else she would gain weight.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Minerva))
                {
                    Mes.Add("*I love hanging around with [gn:" + CompanionDB.Minerva + "], we keep testing each other's meal, to see if we make the best ones.*");
                    Mes.Add("*I actually don't exagerate when testing my meals, so you don't need to worry about me ending up like [gn:" + CompanionDB.Minerva + "].*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("*The other day, [gn:" + CompanionDB.Zacks + "] was at my house and pulled my blanked! I woke up and screamed so loud that he run away.*");
                    Mes.Add("*Sometimes, during the night, I see [gn:" + CompanionDB.Zacks + "] staring through the window.*");
                    Mes.Add("*I fear leaving my house at night, because [gn:" + CompanionDB.Zacks + "] may be out there.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Miguel))
                {
                    Mes.Add("*[gn:"+CompanionDB.Miguel+"] gave me some tips of things I could use as alternative on my meals.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Green))
                {
                    Mes.Add("*[gn:"+CompanionDB.Green+"] is really scary! He always look at us with a menacing face, and looks like he can swallow me whole.*");
                    Mes.Add("*I don't like getting sick or injured, because that means I'll have to visit [gn:"+CompanionDB.Green+"].*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Leona))
                {
                    Mes.Add("*You wouldn't believe how much food [gn:"+CompanionDB.Leona+"] can eat. Earlier she just ate 7 full plates of food.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Scaleforth))
                {
                    Mes.Add("*[gn:"+CompanionDB.Scaleforth+"] promissed me to help taste my meals. The only thing he asked for was a gallon full of water nearby.*");
                }
            }
            //
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I was a little kid when I got passion for tasty food.*");
            Mes.Add("*My name is the same of a seasoning, since I know. It's funny that my mom picked specifically that name to me.*");
            Mes.Add("*Once we had food shortage at home, because I cooked many different food with what we had at home... We had to eat apples for week.*");
            Mes.Add("*I wonder if my mom is worried about me...*");
            Mes.Add("*I heard from the Travelling Merchant that he travels through worlds, so I'm following him to meet new places, and find new kinds of foods.*");
            Mes.Add("*Every food tastes better with the right seasoning, but sometimes placing them is not necessary.*");
            bool HasSomeoneToTutorInCooking = false;
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*I have been learning how to cook food at the right point from [gn:" + CompanionDB.Minerva + "]. Even my food tastes better now.*");
                HasSomeoneToTutorInCooking = true;
            }
            if (CanTalkAboutCompanion(CompanionDB.Bree))
            {
                Mes.Add("*[gn:" + CompanionDB.Bree + "] have been teaching me how to not exagerate when placing seasoning, so my food no longer feels over or under seasoned.*");
                HasSomeoneToTutorInCooking = true;
            }
            if (!HasSomeoneToTutorInCooking)
            {
                Mes.Add("*I'm looking for someone to more about cooking from. I wonder if this world has someone like that.*");
            }
            bool HasSomeoneToTasteFood = false;
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*[gn:" + CompanionDB.Brutus + "] sometimes eat of the food I made. He always compliments It.*");
                HasSomeoneToTasteFood = true;
            }
            if (CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*[gn:" + CompanionDB.Rococo + "] is always happy when he tastes my food, but I always have to reject his seasoning suggestions.*");
                HasSomeoneToTasteFood = true;
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*I really like testing food with [gn:" + CompanionDB.Vladimir + "], we chat about so many things when we do so.*");
                HasSomeoneToTasteFood = true;
            }
            if (!HasSomeoneToTasteFood)
            {
                Mes.Add("*I really don't have a second opinion about the food I cook... I can't know if what I cook is good if only I test It.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch (context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextBool(2))
                        return "*I'm not in need of anything now, but come back later.*";
                    return "*No, I don't have anything I need. Beside we can chat, if you're interessed.*";
                case RequestContext.HasRequest:
                    if (Main.rand.NextBool(2))
                        return "*I have something that I reeeeeeeeeeally need done... Could you help me with it? It's.. Like... [objective].*";
                    return "*I'm so glad you asked, you're a life saver. I need help with this: [objective], can you give me a hand?*";
                case RequestContext.Completed:
                    if (Main.rand.NextBool(2))
                        return "*Yay! Thank you! You're the best person ever!*";
                    return "*I'm so glad to have met you. Thanks.*";
                case RequestContext.Accepted:
                    return "*Thank you! Tell me when you get It done.*";
                case RequestContext.TooManyRequests:
                    return "*Don't you have too many things to do?*";
                case RequestContext.Rejected:
                    return "*Aww....*";
                case RequestContext.PostponeRequest:
                    return "*Later? Fine, it can wait then.*";
                case RequestContext.Failed:
                    return "*It seems like you couldn't do it, then... (You see tears rolling down from her eyes)*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*My request? Did you do it?*";
                case RequestContext.RemindObjective:
                    return "*Huh? I asked you to [objective]. Was that what you were wanting to know?*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You can't do my request? You sure want to cancel it?*";
                case RequestContext.CancelRequestYes:
                    return "*Aww... Well... At least you tried.*";
                case RequestContext.CancelRequestNo:
                    return "*Okay, so what was that for?*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch (context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*You choose me as your buddy? Is that true? You can't change your mind later if you do.*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Then it's true! I'm so happy! We'll be Buddies forever now!*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*Aww... Then it was just a little mistake...*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I... Hm... I don't know you enough... How can I be buddies of someone I hardly know?*";
                case BuddiesModeContext.Failed:
                    return "*No... Not now...*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*Hm? But you have a Buddy already.*";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch (context)
            {
                case JoinMessageContext.Success:
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Brutus) && Main.rand.NextDouble() < 0.33)
                        return "*I think [gn:"+CompanionDB.Brutus+"] wont feel angry at me if I accompany you on your quest. You're a responsible adult, right?*";
                    if (Main.rand.NextDouble() < 0.5)
                        return "*Travel the world with you? Yes, let's go!*";
                    return "*Yes! That sounds so fun... Can I gather some ingredients on the way?*";
                case JoinMessageContext.FullParty:
                    return "*There's way too many people following you, I'd feel cramped in there.*";
                case JoinMessageContext.Fail:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I... Don't really trust you... Right now...*";
                    return "*Sorry, but... No...*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*But this place is dangerous! How can I make It home from here? Do you really want to make me leave the group here?*";
                case LeaveMessageContext.Success:
                    return "*Alright, I'll be heading home, then.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    if (WorldMod.HasCompanionNPCSpawned(CompanionDB.Brutus))
                        return "*... I really wish [gn:"+CompanionDB.Brutus+"] was here now...*";
                    return "*...Fine... I'll see if I can get home alive..*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*More travelling! Yay!*";
                case LeaveMessageContext.Fail:
                    return "*I don't think that's a good idea right now.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch (context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*Hm? [nickname]! I'm coming!*";
                case ReviveContext.RevivingMessage:
                    return "*Hold on! I'm here to help.*";
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*I'm coming!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Don't worry. I'm here.*";
                case ReviveContext.ReviveWithOthersHelp:
                    return "*I was so scary of dying! Thanks for helping.*";
                case ReviveContext.RevivedByItself:
                    return "*Uh, what happened? Why was I in the ground?*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch (context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Do you want to chat? It's good so I can take a little break. What do you want to know?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Do you want to speak about something else?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Alright, anything else?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch (context)
            {
                case MoveInContext.Success:
                    return "*Yes! I'd love to live here. The people here are so nice.*";
                case MoveInContext.Fail:
                    return "*Doesn't look like a good moment to move in here...*";
                case MoveInContext.NotFriendsEnough:
                    return "*I still don't know if it's a good idea to settle here..*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch (context)
            {
                case MoveOutContext.Success:
                    return "*What?! You want me to stop living here?! Aww... Okay... I'll pack my things...*";
                case MoveOutContext.Fail:
                    return "*I'm not moving out of here right now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*I'm not listening to you.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch (context)
            {
                case ChangeLeaderContext.Success:
                    return "*I'll be right behind you, then.*";
                case ChangeLeaderContext.Failed:
                    return "*I don't think I'm ready for that...*";
            }
            return base.ChangeLeaderMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*That's weird... But I guess that's fine. The bed is big enough for us, right?*";
            return "*Oh... I'll use another bed when going sleep then.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*If you tell me cool stories, then yes.*";
            return "*It will be good to use chairs again without having cramp.*";
        }

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                if(WhoJoined.ID == CompanionDB.Minerva)
                {
                    Weight = 1.5f;
                    return "*You cook too? That's great! We could be BFF!*";
                }
                if(WhoJoined.ID == CompanionDB.Glenn)
                {
                    Weight = 1.2f;
                    return "*Wow, you're nearly the same age as I! I'm "+ WhoReacts.GetNameColored()+", by the way.*";
                }
            }
            Weight = 1f;
            return "*A new person! Do you like tasty foods?*";
        }

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Minerva:
                        Weight = 1.2f;
                        return "*"+WhoJoined.GetNameColored()+" is coming too? Cool!*";
                    case CompanionDB.Glenn:
                        Weight = 1.2f;
                        return "*I'm so happy that you joined us.*";
                    case CompanionDB.Green:
                        Weight = 1.5f;
                        return "*I hope I don't need you for a check up.*";
                    case CompanionDB.Vladimir:
                        Weight = 1.5f;
                        return "*Teddy is coming with us!*";
                }
            }
            Weight = 1f;
            return "*Welcome!*";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch (Context)
            {
                case MessageIDs.AlexanderSleuthingStart:
                    return "*What can you tell me about you, rodent?*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... You've been in contact with many foods.*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*I'm starting to get hungry now...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*I guess I should visit your house sometimes when you're cooking...*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ouch! You didn't needed to slap my face!*";
                case MessageIDs.LeopoldMessage1:
                    return "*Hey, [nickname]. Who is that person? He seems weird?*";
                case MessageIDs.LeopoldMessage2:
                    return "*Who are you saying is weird, miss? If you keep talking will make it harder to rescue you.*";
                case MessageIDs.LeopoldMessage3:
                    return "*Rescue? [nickname]'s my friend. They can understand everything we're talking about here.*";
                case MessageIDs.LeopoldEscapedMessage:
                    return "*That looked unnecessary.*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}