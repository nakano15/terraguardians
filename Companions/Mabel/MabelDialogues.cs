using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class MabelDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "*Hello, is this the place for Miss North Pole selection?*";
                case 1:
                    return "*Oh, Hello. Do you know the direction to the North Pole?*";
                case 2:
                    return "*Hi, have you seen any reindeer around?*";
                default:
                    return "*Uh... I don't know what to say... Hi?*";
            }
        }

        public override string NormalMessages(Companion guardian)
        {
            Player player = MainMod.GetLocalPlayer;
            bool AnglerInTown = NPC.AnyNPCs(Terraria.ID.NPCID.Angler);
            List<string> Mes = new List<string>();
            Mes.Add("*Hey! How do I look? I've been practicing all the day.*");
            Mes.Add("*The citizens of your town are very kind to me.*");
            Mes.Add("*Hey, do you listen to Deadraccoon5 too? Whaaaaat? You don't know who he is?*");
            if (player.Male)
            {
                Mes.Add("*Where are you looking at? My face is a bit more above.*");
            }
            Mes.Add("*Do you think I have even a bit of chance on the Miss North Pole contest?*");
            Mes.Add("*I was trying to fly earlier, to see If I can get into the contest. Maybe they only accept reindeer on it, so If I could fly, I could at least participate?*");
            Mes.Add("*Everytime I try to join the Miss North Pole contest, they come with a different excuse.*");
            Mes.Add("*The Miss North Pole is a contest that happens some days before new year. Before happens what your people calls \"X-mas\".*");
            Mes.Add("*My mom used to say that I had the luck of being attracted to merry places. Maybe that explains how I got here.*");
            if (Main.bloodMoon)
            {
                Mes.Add("*How do I look? How do I look-How do I look-How do I look? \"She's blinking her eyes very fast, about 30 frames per second.\"*");
                Mes.Add("*The night will be ended soon-The night will be ended soon.*");
                Mes.Add("(She seems to be drinking a mug of coffee, or half a dozen, throughout the night.)");
                Mes.Add("*You'll keep me safe, right? Right? RIGHT?!*");
                if (AnglerInTown)
                    Mes.Add("*[nn:" + Terraria.ID.NPCID.Angler + "] is safe. Kids don't die, right? They turn into smoke and goes away, right? He's safe, right? Right?*");
            }
            else
            {
                /*if (MainMod.IsPopularityContestRunning)
                {
                    Mes.Add("*Hi! The TerraGuardians Popularity Contest is currently ongoing. Will you be voting?*");
                    Mes.Add("*If you are interessed in participating of the TerraGuardians Popularity Contest, I can take you to the votings.*");
                }*/
                if (!Main.dayTime)
                {
                    Mes.Add("*Zzzzz... I'm the prettiest in the contest... Zzzz.... I will win... Zzzz....*");
                    Mes.Add("*What is it? I'm preparing to go sleep, since It seems like beauty is also related to how well you sleep.*");
                    Mes.Add("*Oh, hello. Can't sleep?*");
                }
                else
                {
                    if (Main.eclipse)
                    {
                        Mes.Add("*My mother would be scared of those kinds of creatures outside, she was from around that time.*");
                        Mes.Add("*Why this world have such weird things happening?*");
                    }
                    if (Main.raining)
                    {
                        Mes.Add("*Rain aways hides a beautiful sunny day. That always makes me sad.*");
                        Mes.Add("*The day is so gray outside, I makes me feel gray too.*");
                        Mes.Add("*If the rain doesn't go away before night, It will surelly ruin a great day.*");
                    }
                    else
                    {
                        Mes.Add("*It's a beautiful day outside. I guess I'll go have a walk.*");
                        Mes.Add("*Don't you just love this kind of weather? It always makes me want to go out for a walk.*");
                        Mes.Add("(She's humming while looking through the window.)");
                    }
                }
            }
            if (AnglerInTown)
            {
                string anglername = "[nn:" + Terraria.ID.NPCID.Angler + "]";
                Mes.Add("*I think that "+anglername+" may be in need of a mother. What If I could try being one?*");
                Mes.Add("*I tried to give "+anglername+" some vegetables to eat, instead of just fish. He was very rude at me.*");
                Mes.Add("*How old is "+anglername+"? He's old enough to eat fish, so I guess he doesn't need...*");
                Mes.Add("*It's so sad, "+anglername+" having no parents and living alone. Will I be able to be like a mother for him?*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Add("*There is the correct time and place to do this. But you had to talk to me while I'm using the toilet?*");
                Mes.Add("*Could you... Just... Return another time?*");
                Mes.Add("*Don't you know that there are a few moments one needs privacy? I'm trying to lose some weight here.*");
            }
            /*if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("*Huh? You're going to sleep in my room? That is so cool! I wonder what is It like to share a room with someone.*");
                Mes.Add("*Sharing my room with you is amazing! I really like that.*");
                Mes.Add("*Sometimes gets cold at night, so It's nice having someone help me get warm.*");
            }*/

            if (CanTalkAboutCompanion(0))
            {
                Mes.Add("*I think I have met [gn:0] before somewhere... I don't remember where.*");
                Mes.Add("*Why [gn:0] eats things from trash cans? I remember seeing him eating food earlier. Where did he get the food, anyway?*");
            }
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("*[gn:1] thinks she's prettier than me. I will prove that she's wrong.*");
                Mes.Add("*Earlier this day, [gn:1] complained about the way I walk. She didn't like it when I talked about her hair, though.*");
                if(CanTalkAboutCompanion(2))
                    Mes.Add("*Why is [gn:1] so mean to [gn:2]? He's the nicest guy I've met in a while.*");
            }
            if (CanTalkAboutCompanion(2))
            {
                Mes.Add("*[gn:2] is a true gentleman. He's always available to help me with whatever I ask.*");
                Mes.Add("*[gn:2] always look at me with a very happy face. But I have to keep reminding him that my face is a bit more above.*");
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("*I should be scared by the fact a Zombie is living in your town. But I wont judge your decision. I'll just say that It's fine. \"Sips coffee\"*");
                Mes.Add("*Every time I talk with [gn:3], he keeps looking around, like as if were checking if there isn't someone around.*");
                if(CanTalkAboutCompanion(1))
                    Mes.Add("*So... [gn:3] is [gn:1]'s boyfriend? No. I wont ask.*");
            }
            if (CanTalkAboutCompanion(4))
            {
                Mes.Add("*[gn:4] and I play the stare game sometimes. He always wins, though...*");
                Mes.Add("*I have to say, It gives me chills when I bump into [gn:4] at night.*");
            }
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("*Where did you find that cute dog? I want to hug [gn:5] and never stop.*");
                Mes.Add("*I always wanted to have a dog, but my mom always said that they were \"dirty and spacious\". She's kind of right, but hey! There is a dog in the town!*");
            }
            if (CanTalkAboutCompanion(6))
            {
                Mes.Add("*I think I am very lucky. [gn:6] personally came to me, saying that If I want a bodyguard, he's available anytime.*");
                Mes.Add("*It's quite weird that sometimes when I talk with [gn:6], he makes some puns with meat. What is that supposed to mean?*");
                Mes.Add("*Sometimes I see [gn:6] watching me from afar. He probably cares about my safety.*");
            }
            if (CanTalkAboutCompanion(7))
            {
                if (CanTalkAboutCompanion(2))
                {
                    Mes.Add("*[gn:7] came to me earlier, and said that she got what I'm doing and that I should stop doing that with [gn:2], or else she'll give me a beating. But I wonder, what have I been doing to him?*");
                    Mes.Add("*I don't know why [gn:7] is so upset about me, I don't remember being rude or mean to her.*");
                }
                Mes.Add("*I have to say, sometimes I have problems seeing [gn:7] when she's right under me.*");
                Mes.Add("*It seems like [gn:7] is avoiding talking to me. Did I anger her somehow?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Domino))
            {
                if (NPC.AnyNPCs(Terraria.ID.NPCID.ArmsDealer))
                {
                    Mes.Add("*[nn:"+Terraria.ID.NPCID.ArmsDealer+"] gave me an idea of something I can use for the Miss contest after I asked him what Terrarian Miss uses on his hometown. Gladly, [gn:"+CompanionDB.Domino+"] had some for sale, but I feel weird wearing that kind of thing.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*I don't understand. I was talking about some things related to the Miss Contest to [gn:"+CompanionDB.Vladimir+"], until he suddenly dropped me on the floor and said that had to go to the toilet urgently. My behind is still hurting from the fall. Ouch~.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Michelle))
            {
                Mes.Add("*Did you know that [gn:" + CompanionDB.Michelle + "] will try joining the Miss North Pole contest? I'm so happy, I never wondered I would have a rival. Wait... Is that good or bad?*");
                Mes.Add("*I'm so glad to have met [gn:"+CompanionDB.Michelle+"]. She's a great person to have around.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*I wonder if I'm not accepted into the Miss North Pole contest because I'm not a reindeer. Maybe [gn:"+CompanionDB.Malisha+"] could help me solve that?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Wrath))
            {
                Mes.Add("*I tried helping [gn:"+CompanionDB.Wrath+"] getting less angry, until he yelled out loud, that made me leave the room very quickly. He's very scary.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*What's with [gn:" + CompanionDB.Fluffles + "]? Sometimes when she looks at me, she looks me from the head to the feet.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*When visiting [gn:" + CompanionDB.Cinnamon + "], I always have to resist the temptation of nibbling everything she cooked.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Luna))
            {
                Mes.Add("*Whaaaaaaaaaaat? [gn:" + CompanionDB.Luna + "] won an edition of Miss North Pole? Like... Whaaaaaaaaaaaaat?*");
                Mes.Add("*[gn:" + CompanionDB.Luna + "] thinks I'm exaggerating a bit about the practice of the contest, but I don't think like that.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("*I really don't like [gn:"+CompanionDB.Green+"]. every time I visit him he gives me some diet. Do I look fat, to you?*");
                Mes.Add("*I was expecting to get vitamins from [gn:"+CompanionDB.Green+"], not a diet! Hmph...*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cille))
            {
                Mes.Add("*I really don't like [gn:" + CompanionDB.Cille + "]. I was all happy, gave her a 'hi', and she told me to 'go away'! Why she had to be so rude?*");
                Mes.Add("*What kind of clothing [gn:" + CompanionDB.Cille + "] uses? Is that the kind of thing you Terrarians use?*");
            }
            /*if (FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*Who's she, [nickname]? Did you met a new friend?*");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    List<string> Mes = new List<string>();
                    Mes.Add("*I won... I won! Yes...* (She must be dreaming about winning the contest)");
                    Mes.Add("*I will prove you wrong... I will be the best miss ever...* (She's speaks while she sleeps)");
                    Mes.Add("*I am like this... You can't change me... I'll try anyway...* (She's speaks while she sleeps)");
                    if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
                    {
                        Mes.Add("*Am I... A good mother...Doing things right...?* (She's speaks while she sleeps)");
                        Mes.Add("*I'm... A failure... Of mother...?* (She's speaks while she sleeps)");
                        Mes.Add("*I'm trying... my best...* (She's speaks while she sleeps)");
                    }
                    Mes.Add("*" + MainMod.GetLocalPlayer.name + "...* (Looked like she was going to ask something in her sleep)");
                    return Mes[Main.rand.Next(Mes.Count)];
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "*Oh, hi [nickname]. Did you do what I asked you?*";
                        case 1:
                            return "*I'm so sleepy, did you wake me up to tell me that you did what I asked?*";
                    }
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "*Yawn... Oh, hello [nickname]. I really need some sleep to keep my beauty for the contest. What do you need?*";
                        case 1:
                            return "*Aaaah! Oh, sorry, you scared me. I wasn't having a good dream...*";
                        case 2:
                            return "*Huh? Oh. Do you need something? I was dreaming about... Nevermind...*";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TalkMessages(Companion guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My mom and I had to move out of a town once. I think It was because the citizens were way too good to me.*");
            Mes.Add("*Sometimes one of my feet starts to hurt, because I place most of my weight on it.*");
            Mes.Add("*I have to admit, I fell sometimes when trying to maintain this pose. That also explains why I'm missing a teeth.*");
            Mes.Add("*I'm pursuing my lifetime goal, I'm going to be a Miss North Pole. But I wonder, what should I pursue next when I achieve that?*");
            if(guardian.FriendshipLevel >= guardian.Base.GetFriendshipUnlocks.FollowerUnlock)
                Mes.Add("*Don't blush, but I accompany you on your adventures because I like your company.*");
            Mes.Add("*Do you think I'm naive? Too many people say that that's my problem. But I don't see myself as being naive.*");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Angler))
            {
                Mes.Add("*I worry about [nn:"+Terraria.ID.NPCID.Angler+"], what should I do If he feels sick? I don't have any idea of what being a Mom is.*");
                Mes.Add("*Do you think that I'm being a good mother to [nn:" + Terraria.ID.NPCID.Angler + "]? Sometimes I think I'm not...*");
                Mes.Add("*How you takes care of your children? Oh, you're single. Sorry that I asked...*");
                Mes.Add("*Being a mother is scary, most of the time I keep asking myself If what I'm doing is enough, or if is right.*");
            }
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("*Don't you dare tell [gn:1], but I like what she has done to her hair.*");
            }
            if (CanTalkAboutCompanion(2))
            {
                Mes.Add("*Sometimes when I'm alone talking with [gn:2], he looks to the side, and then says that has something else to do, and leaves.*");
                Mes.Add("*There were a few times where I tried to speak with [gn:2], but he seemed to be having difficulties to talk.*");
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("*I saw [gn:3] drolling earlier this day. I wonder why is that.*");
                Mes.Add("*I could swear that I heard a faint howling earlier. Are there wolves around?*");
            }
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("*Oh great one! [gn:5] is so cute! Even more when we pet him and he turns upside down, so we can pet his belly.*");
            }
            if (CanTalkAboutCompanion(6))
            {
                Mes.Add("*Every time I ask [gn:6] to be my bodyguard, he insists on following me from behind, because most dangerous attacks come from behind, or something.*");
                Mes.Add("*When someone smiles, is because It's happy. But what kind of facial expression [gn:6] does when looking at me?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion guardian, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return (Main.rand.NextDouble() < 0.5 ? "*Huh?! No, I don't want anything.*" : "*Oh! No, there is nothing I want right now.*");
                case RequestContext.HasRequest:
                    return (Main.rand.NextDouble() < 0.5  ?
                        "*I've been so busy practicing to be a model, that I forgot that I have something to do. Can you [objective]?*" :
                        "*I just don't have the time to deal with this, so can you [objective] for me?*");
                case RequestContext.Completed:
                    return (Main.rand.NextDouble() < 0.5 ? "*Amazing! I knew you would be able to do it.*" : "*Thank you, I.. You don't know how happy I am for this.*");
                case RequestContext.Accepted:
                    return "*Yay! You know where to find me when you do It, right?*";
                case RequestContext.TooManyRequests:
                    return "*It may be dangerous to your health to have many things to do. Why don't you try doing your other requests before?*";
                case RequestContext.Rejected:
                    return "*Aww... No problem. I'm sure you have a good reason to reject my request.*";
                case RequestContext.PostponeRequest:
                    return "*Alright. Come ask me again if you want to do my request. Bye!*";
                case RequestContext.Failed:
                    return "*You failed? Don't worry, It's fine. You tried, right? There, don't feel sad.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*My request? You completed it?*";
                case RequestContext.RemindObjective:
                    return "*I asked you to [objective]. Is that what you wanted to know?*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You can't do my request?*";
                case RequestContext.CancelRequestYes:
                    return "*Oh... I hope I didn't make you nervous, or anything... You don't need to do what I asked anymore.*";
                case RequestContext.CancelRequestNo:
                    return "*I'm so glad...*";
            }
            return base.RequestMessages(guardian, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*Yes, I can live here. I hope my house will have enough space for me to practice.*";
                case MoveInContext.Fail:
                    return "*I don't like the idea of moving in right now. Maybe another time.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I have my own house, but thanks anyways.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You don't want me around here? Alright.. I'll pack my things back home, then.*";
                case MoveOutContext.Fail:
                    return "*I'm sorry, but I can't leave right now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*Someone who's really dear to me let me stay here. Why are you trying to evit me?*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*You're calling me to an adventure? Cool! The walking will help me get fit for the contest.*";
                case JoinMessageContext.Fail:
                    return "*Sorry, but I need to get myself ready for the contest.*";
                case JoinMessageContext.FullParty:
                    return "*There are way too many people following you right now...*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Alright. I enjoyed traveling with you, now back to practicing.*";
                case LeaveMessageContext.Fail:
                    return "*No way! I can't leave you right now.*";
                case LeaveMessageContext.AskIfSure:
                    return "*I really dislike the idea of being left here all alone. Do you know what those horrible creatures do to people like me?!*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Okay, better start running, then?*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*I'm soooooooo happy that you changed your mind.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Sure. Hop on my shoulder.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*Thank you for the ride.*";
                case MountCompanionContext.Fail:
                    return "*Better not...*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*You seem like a nice person, but I'm still not sure if I trust you...*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*Yes, I can carry [target] on my shoulder.*";
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
                    return "*There you go. I hope your feet got some rest.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Thank you again for the ride.*";
                case DismountCompanionContext.Fail:
                    return "*I have to disagree. Now is not a good moment.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*I allow it, let's Bond-Merge. You will exercise my body while using it, right?*";
                case ControlContext.SuccessReleaseControl:
                    return "*Done. I hope you managed to do what you were wanting to do.*";
                case ControlContext.FailTakeControl:
                    return "*This doesn't seem like the time for that.*";
                case ControlContext.FailReleaseControl:
                    return "*I can't do that right now. Better do that at a more appropriate time.*";
                case ControlContext.NotFriendsEnough:
                    return "*That's the kind of thing I only do with people I trust... Sorry...*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Yes, do you need to talk something with me?*";
                        case 1:
                            return "*It's so strange to only be able to watch. Gladly I know you know what you are doing.*";
                        case 2:
                            return "*Is there something you want to talk about?*";
                    }
            }
            return base.ControlMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*I believe the bed is big enough for both of us, so I don't mind.*";
            return "*I'll take another bed then.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*You can sit on my lap. I don't mind at all.*";
            return "*I'll try finding another chair should I need to use one.*";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*Do I need to change how I take on combat? What do you suggest me to do?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*I should take on physical combat? I don't really like that idea... But okay.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Keep distance from my targets? I can do that.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Attack my targets from far away? I'll do that.*";
                case TacticsChangeContext.Nevermind:
                    return "*It's fine how I take on combat? Phew...*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*You want to talk? I like talking. What do you want to talk about?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*That's it, or want to talk about something else?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*It was nice talking to you. Do you need something else now?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*Heeey! Say, your travels involve a lot of walking? Yes?! Woohoo! Take me with you then! That may help me stay fit for the contest.*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*It must be troublesome to walk around with those small legs of yours. I know! You can sit on my shoulders! That can help us move faster. I'm a genius, I know. And also pretty.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*Hey, do you want to meet the Ether Realm? What? You can't visit it? Don't worry, I can take you there.*";
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "*Hey [nickname]. I'm starting to like this place. Would you mind if I move in here? I could practice for the contest here.*";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*Oh hi! I was wanting to talk with you about something uber-important. Can you listen to me? So... I'm really happy for having you as a very dear friend, and I wanted to let you know that, if you think about picking me as your Buddy, I will be very happy to accept it. So... Will you pick me as your Buddy? Oh, sorry. I shouldn't be pushing such an important matter.. Well, do let me know if you think about picking me.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*Oh, hi my Buddy. Oh, I shouldn't call you that? Sorry, I'll call you by [nickname]. Did you know that you can ask me anything and I will do it? It's like a Buddy thing, so there's no reason why I should feel unsure about your decisions. If you need me to do something, do let me know.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*You need my help? With what?*";
                case InteractionMessageContext.Accepts:
                    return "*I'll help you with that.*";
                case InteractionMessageContext.Rejects:
                    return "*I'll not do that..*";
                case InteractionMessageContext.Nevermind:
                    return "*Oh, you didn't mean to ask for my help?*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*I will follow ahead then.*";
                case ChangeLeaderContext.Failed:
                    return "*Not right now.*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*What? You.. What? You asked me to be your Buddy?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Buddy? You? Mine? Yes! Let's be buddies!*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*[nickname]! You got me happy for nothing now!*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I don't know you well enough for that.*";
                case BuddiesModeContext.Failed:
                    return "*I can't be your Buddy at this moment.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*You've got yourself a Buddy. Why are you asking me that?*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*Sure. I'm on my way. The walk might even help to keep me fit.*";
                case InviteContext.SuccessNotInTime:
                    return "*I can visit you, but wait until tomorrow. I'll be there by the day.*";
                case InviteContext.Failed:
                    return "*I'm busy practicing for the contest right now.*";
                case InviteContext.CancelInvite:
                    return "*You don't need me anymore? Well... I'll walk back home then...*";
                case InviteContext.ArrivalMessage:
                    return "*I arrived, [nickname].*";
            }
            return "";
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*Ahh!! Wait! I'm coming!*";
                case ReviveContext.RevivingMessage:
                    {
                        bool IsPlayer = !(target is Companion);
                        List<string> Mes = new List<string>();
                        if (IsPlayer && target == companion.Owner)
                        {
                            Mes.Add("*Wake up! You still need to see me winning the contest!*");
                            Mes.Add("*Come on, wake up! There's a lot for you to see!*");
                            Mes.Add("(Crying)");
                        }
                        else
                        {
                            Mes.Add("*Hey, I've been practicing hard for the contest to lose someone who could watch me on it. Wake up!*");
                            Mes.Add("*I'll take care of those wounds you have. Don't worry.*");
                            Mes.Add("*You people have been so nice to me, It's time for me to retribute that.*");
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*Don't you worry! I'll give you a hand!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Here I am. Now let me try keeping you safe.*";
                case ReviveContext.RevivedByItself:
                    return "*Ouch... It still hurts... But I can still walk..*";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextFloat() < 0.5f)
                        return "*Oh, my hero! Thank you!*";
                    return "*Thank you, I'm so glad to have you around.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "*I'm so confused right now.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Just how much did you miss having another person around?*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}
