using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class MinervaDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Hello... I'm.. A cook... Do you mind... If I cook for you..?*");
            Mes.Add("*You're a... Terrarian..? I could... cook for you, if you... If you don't mind...*");
            Mes.Add("*Hi... I'm a bit lost... I love cooking... So I could cook something for you..*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            if (Main.eclipse)
            {
                Mes.Add("*Hmm... Maybe I could have use... For a Chainsaw...*");
                Mes.Add("*I can't concentrate on the cooking, with all those weird creatures running outside.*");
                Mes.Add("*I'm sorry... I can't cook for you right now... I'm busy trying to survive this day.*");
            }
            else if (!Main.bloodMoon)
            {
                if (MainMod.GetLocalPlayer.HasBuff(Terraria.ID.BuffID.WellFed) || 
                    MainMod.GetLocalPlayer.HasBuff(Terraria.ID.BuffID.WellFed2) ||
                    MainMod.GetLocalPlayer.HasBuff(Terraria.ID.BuffID.WellFed3))
                {
                    Mes.Add("*You're looking healthy...*");
                    Mes.Add("*It's good to see you well.*");
                    Mes.Add("*I'm glad to help keep you nourished.*");
                }
                else
                {
                    Mes.Add("*You look hungry... I can make something for you.*");
                    Mes.Add("*It's not good to keep the belly empty. I can try making something for you to eat.*");
                    Mes.Add("*The look in your face... Just let me know what would you like me to make.*");
                }
                Mes.Add("*I will need ingredients to cook... If you find something good to cook, give me, so I can make food for you anytime you want.*");
                Mes.Add("*I didn't watched my belly when testing my food, so now I'm fat. I needed to see if what I cooked was tasting better.*");
                Mes.Add("*I love cooking... The mix of several different ingredients, can lead to the ultimate tasty food.*");
                Mes.Add("*Are you here to get some food?*");
                Mes.Add("*Do you... Have anything to say... About the foods I make?*");
                Mes.Add("*Smelled something awful? Sorry, I couldn't hold.*");
                if (!Main.raining)
                {
                    if (Main.dayTime)
                    {
                        Mes.Add("*I like this weather.*");
                        Mes.Add("*Enjoying the weather, [nickname]?*");
                    }
                    else
                    {
                        Mes.Add("*I'm feeling a bit drowzy right now...*");
                        Mes.Add("*You're preparing yourself for sleep?*");
                    }
                }
                else
                {
                    Mes.Add("*The smell of wet leaves in the air makes me nostalgic.*");
                    Mes.Add("*Are you watching the rain, [nickname]?*");
                    Mes.Add("*Achoo~! Better, not get too close to me, [nickname]. I don't want to give you flu.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    Mes.Add("*The easiest food I make in this place is for [gn:0], he always eats Sweet Potatoes.*");
                    Mes.Add("*I'm peeling some Sweet Potatoes, because [gn:0] may be coming soon to eat something.*");
                    Mes.Add("*[gn:0] seems to be very grateful for the food I make everyday. I wonder if he always had something to eat?*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    Mes.Add("*I thought It would be a good idea to make a Bunny Stew to [gn:1], but she got very angry when she saw It.*");
                    Mes.Add("*What [gn:1] eats? Well, It ranges from Squirrels in a Stick to Marshmellows. She seems very addicted to Marshmellows, for some reason.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Sardine))
                {
                    Mes.Add("*[gn:2]'s diet must really be composed of fish, like his name...*");
                    Mes.Add("*I tried offering something other than fish to [gn:2], but he refused, and said that wanted to eat cooked fish.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Zacks))
                {
                    Mes.Add("*Whenever I see [gn:3], I give him the first piece of food I have around. I don't want to end up being part of his meal.*");
                    Mes.Add("*The other day, [gn:3] asked if he could have some hamburguer. I was so scared, but gladly could make him go away by giving him some Bunny Stew.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                {
                    Mes.Add("*You wouldn't believe how much [gn:6] eats. I end up exausted after cooking for him.*");
                    Mes.Add("*I asked [gn:6] one day, if he could take a bit ligher on eating. He answered saying that he needs all that food for all the physical effort he does daily.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    Mes.Add("*The most annoying citizen to please with food is [gn:7]. Anything I make is not good enough for her.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Mabel))
                {
                    Mes.Add("*[gn:8] asked me earlier to give her some food that didn't had a lot of things. I gave her a cup of water, but she didn't seems to have liked that.*");
                    Mes.Add("*You would think that [gn:8] eats salad to lose weight. She not only eats a lot of salad, but you can expect at least a piece of meat on her plate.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                {
                    Mes.Add("*I like cooking to [gn:11]. You will not believe what I will say, but he actually doesn't eats a mountain of food.*");
                    Mes.Add("*I heard some creepy stories from [gn:11] parents. He said that one of his parents can even swallow someone whole. Better you watch out for that If you manage to meet It.*");
                    Mes.Add("*I find [gn:11]'s story with his brother sad.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Wrath))
                {
                    Mes.Add("*I can understand that [gn:14] can't control his anger, but could he at least stop yelling at me when I'm cooking?*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Fear))
                {
                    Mes.Add("*Sorry if I'm not really into talking too much right now... [gn:"+CompanionDB.Fear+"] has been screaming due to being scared of random things, and I'm getting a minor headache...*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                {
                    Mes.Add("*[nickname], is [gn:16] on my shoulder? No? Good. She's very scary, and I hate when she does that.*");
                    Mes.Add("*[gn:16] asked if I could make some food for her, but she couldn't manage to eat It... The food fell through her body.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
                {
                    Mes.Add("*Having [gn:" + CompanionDB.Cinnamon + "] around is perfect to try improving my cooking. Maybe I could ask her to be my assistant in the future.*");
                    Mes.Add("*While my strength is knowing how to cook things, [gn:" + CompanionDB.Cinnamon + "] is good at setting the correct seasonings and their amount on the food.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Miguel))
                {
                    Mes.Add("*[gn:"+CompanionDB.Miguel+"] is trying to help me lose my fat, but my belly isn't going away. He told me that is because I eat too much, but how else can I find out if the meal is good?*");
                }
                if (IsPlayerRoomMate())
                {
                    Mes.Add("*I like sharing my room with you, but I can't do anything about the gas related issue...*");
                    Mes.Add("*What kind of breakfast do you like? I ask so I can prepare something when you wake up.*");
                }
            }
            else
            {
                Mes.Add("*...*");
                Mes.Add("*...Grr.*");
                Mes.Add("*...NO!*");
                Mes.Add("(She's staring at you with a angry face, better not bother her.)");
                Mes.Add("(Her eyes are closed, anger is seen on her face, and she's seems to be trying to breath calmly. If I want to talk to her, better I do that cautiously.)");
            }
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*..You came wanting some food? Does she wants some too...?*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It fills me with joy, when I see people happy when eating my food.*");
            Mes.Add("*Have you been eating sufficiently latelly? I'm glad to hear that you are.*");
            Mes.Add("*I don't mind cooking for anyone, It actually fills me with joy.*");
            Mes.Add("*I would like to apologize to you for any noxious gas I release, but I really needed to check if the food was good for the people to eat.*");
            if (CanTalkAboutCompanion(CompanionDB.Zack))
            {
                Mes.Add("*I don't know why you let [gn:3] live here. Whenever he shows up on my kitchen, he's salivating while looking at me. Is he planning to eat me?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*Beside [gn:6] eats a lot, I don't mind cooking for him, because the stories he tells when I'm eating are interesting. I still want to know the rest of the story he told me last night.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*I tried several times to offer food to [gn:10], but he always says that he conjures his own food and water.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*I think... I'm starting to like [gn:11]. He's very different from many guys I've met.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*I refuse to cook for [gn:12]. Everytime I try making food for her, I end up tied to a chair, and spending the entire night being forced to drink awfully tasting potions.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*Thanks for asking, but I have nothing that I need right now.*";
                    return "*Hm... No. I don't need anything.*";
                case RequestContext.HasRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*[nickname], I'm a bit busy making some food, so I can't [objective] myself. Could you do that for me?*";
                    //else
                    //    return "*I need to lose some weight, can you [objective]?*"; //Travel request dialogue
                    return "*You're a life saver! I need your help to [objective] for me. I'm currently busy doing some things, so I can't do that myself.*";
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "*You're a gift, [nickname]. Thank you for helping me.*";
                    return "*I prepared this specially for you, I hope you like It.*";
                case RequestContext.Accepted:
                    return "*Thank you. While you do what I asked, I will try preparing something special for you.*";
                case RequestContext.TooManyRequests:
                    return "*You look a bit overloaded. Better you do your other requests, before doing mine.*";
                case RequestContext.Rejected:
                    return "*You can't do It... I'll try doing It later, when I get a break from cooking.*";
                case RequestContext.PostponeRequest:
                    return "*It can wait, take your time, but not too long.*";
                case RequestContext.Failed:
                    return "*You couldn't fulfill what I asked for... It's fine...*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*You completed my request..? I can offer you those for it..*";
                case RequestContext.RemindObjective:
                    return "*It's fine.. Anyone can end up forgetting things. [objective] is what I asked you to do.*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*What? Are you serious?*";
                case RequestContext.CancelRequestYes:
                    return "*I... Okay... If that's what you want...*";
                case RequestContext.CancelRequestNo:
                    return "*Uff... Well.. Do you want anything else?*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "(She's really deep on her sleep. She looks very tired.)";
                        case 1:
                            return "(You heard a weird air sound, and now is smelling something awful.)";
                        case 2:
                            return "(She seems to be snoring a bit.)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    return "*You woke me up... If you want to eat something, couldn't you wait until I woke up?*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*Hm... Oh, [nickname]. Why did you wake me up? Did you do what I asked?*";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch (context)
            {
                case JoinMessageContext.Success:
                    return "*I can. Maybe I will lose some weight as we travel, or get more ingredients.*";
                case JoinMessageContext.FullParty:
                    return "*I'm sorry, but I dislike aglomerations...*";
                case JoinMessageContext.Fail:
                    return "*I can't at the moment... I'm sorry to disappoint...*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*... I dislike this place... Seems pretty unsafe. Do you really want to leave me here?*";
                case LeaveMessageContext.Success:
                    return "*Be sure to visit me at "+(!Main.dayTime ? "lunch" : "dinner")+" time. It's not good to skip meals.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I will try getting at home in one piece. Probably.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Then I'm with you for longer.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*I can cook while talking, sure. What do you want to talk about?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Was that all... Or is there more..?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Feel free to call me for a chatting anytime you want.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*I... Um... Thank you...*";
                    return "*I'm thankful for your help.*";
                case ReviveContext.RevivedByItself:
                    return "*...*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldMessage1:
                    return "*Uh.. We're not... Hostages...*";
                case MessageIDs.LeopoldMessage2:
                    return "*What? Then why...*";
                case MessageIDs.LeopoldMessage3:
                    return "*The Terrarian... Is leading us.. On their adventure... And can understand what you're saying...*";
                
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Let's see what are you cooking...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... So many different food smell...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*Huh... What was that noise...?*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*Eww... This smell... I'm glad I already identified you but... Ugh..*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Ah... Eh... Good morning?*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}