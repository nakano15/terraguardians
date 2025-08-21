using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class LeonaDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*I guess I'll never get tired of seeing Terrarians. Hello, I'm Leona.*";
                case 1:
                    return "*Look at what I found here. The name's Leona. Need something killed?*";
                case 2:
                    return "*Oh, a friendly face! Happy to meet me? I'm Leona, Swordswoman.*";
            }
        }
        
        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            bool LeonaHasSword = (companion as Leona.LeonaCompanion).HoldingSword;
            if (companion.IsUsingToilet)
            {
                if (LeonaHasSword)
                    Mes.Add("*You sure that you want to speak with me right now? Not only the smell isn't good, but I also have a long blade sword.*");
                Mes.Add("*Couldn't you find a more improper moment to talk with me?*");
                Mes.Add("*I was having problems doing my business here, and you staring at me isn't helping either.*");
                Mes.Add("*Would it be possible for you to get a bigger toilet? I'm tired of the constantly flushing.*");
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    Mes.Add("*Heeey shortie, came to check on me?*");
                    Mes.Add("*What is it? Someone bullying you? Tell me who and I will take care of them.*");
                    Mes.Add("*I was expecting to see you.*");
                    Mes.Add("*I'm not fat! This is just stocking for days without food. Wait, that's fat...*");

                    Mes.Add("*I was curious to see where I would go next, so I kept entering the portals that popped up in my travels.*");
                    Mes.Add("*I was surprised to see at least something friendly on the other side of this world portal. Not that all places I went were unfriendly, by the way, but yours was the first where I got to speak with someone new.*");
                    Mes.Add("*You better be careful when fighting things off portals. You never know what will pop up before it begins disappearing.*");

                    Mes.Add("*My time as a Royal Guard is over, even more since a member I liked to mess with left for unknown reasons.*");
                    Mes.Add("*I think you would like visiting the Ether Realm. At least, as long as I'm with you, you'd be safe there.*");
                    Mes.Add("*I actually like this place. The Terrarians here are really nice to me. I just need to be careful about where I walk to.*");
                    Mes.Add("*My job as a Royal Guard was dull. I either were protecting the Kings chamber or spending my time training or in the barracks. I got to use more my weapons since I arrived here.*");

                    if (Main.dayTime)
                    {
                        if (Main.raining)
                        {
                            Mes.Add("*Mr Raindrop, falling away from me now...*");
                            Mes.Add("*Do you like rainy weather too? It make me kind of drowzy, but I'll survive.*");
                            Mes.Add("*Oh, look at you: Dripping. Need a towel? I don't have one.*");
                        }
                        else
                        {
                            Mes.Add("*My beautiful fur will only get beautier with sunlight help.*");
                            Mes.Add("*Are you here to get some tanning too? Great!*");
                            if (LeonaHasSword)
                                Mes.Add("*It's being quite hard to wield the sword. Its metal is getting hotter.*");
                        }
                    }
                    else
                    {
                        if (Main.raining)
                        {
                            Mes.Add("*I hope it keeps raining while I'm asleep.*");
                            Mes.Add("*What a nice surprise. Yes, we can chat for a while.*");
                        }
                        Mes.Add("*I can't wait for the moment of hitting the bed...*");
                        Mes.Add("*Oh, you came visit me. Could you be brief? I'm going to bed soon.*");
                        Mes.Add("*A nice meal and then I'm ready for bed.*");
                    }
                    if (LeonaHasSword)
                    {
                        Mes.Add("*This sword? It doesn't weight on me that much.*");
                        Mes.Add("*Are you trying to keep distance from me? Don't worry, I don't plan on using this on you.*");
                        Mes.Add("*Sadly, as a part of my old job, I got the habit of keeping my weapon ready all the time.*");
                    }
                    else
                    {
                        Mes.Add("*I feel defenseless without my sword... Oh, you shouldn't have heard that.*");
                        Mes.Add("*It's so odd to have so much freedom of movement with my right arm. What could I do with it..?*");
                        Mes.Add("*Where I stored my sword? Why do you want to know? You think you can lift it? Someday you'll have to let me see you try lifting it, haha.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Blue))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Blue+"] keeps picking on me because of my belly. She always forgets that I have a huge sword.*");
                        if (player.Male)
                            Mes.Add("*If you had to choose between [gn:"+CompanionDB.Blue+"] and me, who you'd pick? I'm just asking, there's no particular reason..*");
                        if (CanTalkAboutCompanion(CompanionDB.Zack))
                            Mes.Add("*I guess [gn:"+CompanionDB.Blue+"] has something more important to mind than my belly, even more with that boyfriend of hers looking at people like they're walking beefs.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Alex))
                    {
                        Mes.Add("*If I didn't had enough responsibility already, I'd adopt [gn:"+CompanionDB.Alex+"]. He's such a good dog.*");
                        Mes.Add("*Do you even know that "+AlexRecruitmentScript.AlexOldPartner+" [gn:"+CompanionDB.Alex+"] talks about? No? I guess it wasn't your fault her death, then?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Brutus))
                    {
                        Mes.Add("*I wasn't actually expecting to see [gn:"+CompanionDB.Brutus+"] here. I guess fate made us bump into each other again.*");
                        Mes.Add("*How I know [gn:"+CompanionDB.Brutus+"]? I was also a Royal Guard, just like him. Lets say that I'm more into offensive and he's into defensive.*");
                        Mes.Add("*I love teasing [gn:"+CompanionDB.Brutus+"] every now and then. I just like seeing him mad.*");
                        Mes.Add("*It seems like [gn:"+CompanionDB.Brutus+"] old habits didn't died since he left the Ether Realm. I wonder if he still have that magazines collection...*");
                        Mes.Add("*It's mildly annoying how [gn:"+CompanionDB.Brutus+"] have eyes for other females... W-wait, are you listening.. Nevermind what I said!!*");

                        Mes.Add("*You say that [gn:"+CompanionDB.Brutus+"] is your bodyguard? I don't believe that. Knowing him, he think everyone here is under his protection.*");
                        Mes.Add("*[gn:"+CompanionDB.Brutus+"] was always best known for two things: his loyalty, and sense of protection.*");
                        Mes.Add("*I wonder if [gn:"+CompanionDB.Brutus+"] still has his magazines collection... What magazines collection? You wouldn't want to know...*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Domino))
                    {
                        Mes.Add("*Ah, so [gn:"+CompanionDB.Domino+"] is here too? He managed to cause serious headaches to the guards in the Ether Realm. Hopefully he wont do the same here.*");
                        Mes.Add("*Do let me know if [gn:"+CompanionDB.Domino+"]'s presence here is hazardous. I wouldn't mind having a talk with him, if you know what I mean.*");
                        if (CanTalkAboutCompanion(CompanionDB.Brutus))
                        {
                            Mes.Add("*If you knew how many times [gn:"+CompanionDB.Brutus+"] got yelled for failing at capturing [gn:"+CompanionDB.Domino+"]... Lets say that turned them into nemesis...*");
                            Mes.Add("*I once offered myself to capture [gn:"+CompanionDB.Domino+"], but [gn:"+CompanionDB.Brutus+"] intervened, saying that this matter was personal.");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Leopold))
                    {
                        Mes.Add("*It seems like this world is getting more and more popular for TerraGuardians, huh? Even [gn:"+CompanionDB.Leopold+"] joined in.*");
                        Mes.Add("*Magic, magic, magic. Only I feel satisfied with slicing things in half?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Mabel))
                    {
                        Mes.Add("*I'm glad I don't get as much attention from people like [gn:"+CompanionDB.Mabel+"] does. I really dislike crowding, but she doesn't seem to mind that.*");
                        Mes.Add("*If I was [gn:"+CompanionDB.Mabel+"]'s bodyguard, I would have LOTS of work...*");
                    }
                    bool GlennMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Glenn),
                        SardineMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Sardine),
                        BreeMet = PlayerMod.PlayerHasCompanion(player, CompanionDB.Bree);
                    if (CanTalkAboutCompanion(CompanionDB.Glenn))
                    {
                        if (!SardineMet && !BreeMet)
                        {
                            Mes.Add("*How irresponsible could someone be to leave their cub living alone in a house?! Once we find [gn:"+CompanionDB.Glenn+"]'s parents, I will have a SERIOUS conversation with them.*");
                        }
                        else if (BreeMet && !SardineMet)
                        {
                            Mes.Add("*I wonder where could be [gn:"+CompanionDB.Glenn+"]'s idiotic father. Even his husband is looking for him, and he's nowhere to be found.*");
                        }
                        else if (!BreeMet && SardineMet)
                        {
                            Mes.Add("*So... We found [gn:"+CompanionDB.Glenn+"]'s father, but his mother is missing. Couldn't they make my job easier?*");
                        }
                        else
                        {
                            Mes.Add("*It's really good to have [gn:"+CompanionDB.Glenn+"]'s family finally reunited, but it was really irresponsible of them to leave him alone. At least they are unharmed.*");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Sardine))
                    {
                        if (CanTalkAboutCompanion(CompanionDB.Bree))
                        {
                            Mes.Add("*[gn:"+CompanionDB.Sardine+"]'s house is really noisy. We can hear [gn:"+CompanionDB.Bree+"] and him arguing far from there.*");
                        }
                        Mes.Add("*How did [gn:"+CompanionDB.Sardine+"] managed to get stuck inside... No, nevermind. I don't want to know.*");
                        if (SardineBountyBoard.TalkedAboutBountyBoard)
                            Mes.Add("*I take some of [gn:"+CompanionDB.Sardine+"]'s Bounties sometimes, they help me not get rusty about combat, and make some coins too.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Bree))
                    {
                        if (SardineMet)
                        {
                            Mes.Add("*I sometimes have a chat with [gn:"+CompanionDB.Bree+"]. She spends most of the time speaking of her husband and his stupid things.*");
                            Mes.Add("*I wonder; If I had a husband, what kind of couple we would be? Probably not like [gn:"+CompanionDB.Bree+"] and [gn:"+CompanionDB.Sardine+"], right?*");
                        }
                        else
                        {
                            Mes.Add("*You've also been asked by [gn:"+CompanionDB.Bree+"] to look for her husband? I'm doing that sometimes too. Remember, her husband is a black cat.*");
                        }
                        if (GlennMet)
                        {
                            Mes.Add("*It's so boring whenever [gn:"+CompanionDB.Bree+"] starts saying that [gn:"+CompanionDB.Glenn+"] is her source of pride and stuff. Even more when she compares him to her husband... I heard that, like, a lot.*");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Luna))
                    {
                        Mes.Add("*You didn't looked very surprised when you met me. Has [gn:"+CompanionDB.Luna+"] told you about us first?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Celeste))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Celeste+"] is not what I expected from a priestess. She's not preachy.*");
                        Mes.Add("*It's surprising that a priestess from my realm religion appeared here. I wonder if she will ask to build a church for her.*");
                        Mes.Add("*The rite to be a Royal Guard involved also getting the blessing from "+MainMod.TgGodName+".*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                    {
                        Mes.Add("*I find it odd to see [gn:"+CompanionDB.Vladimir+"] hugging about half the village. He doesn't seems to mind that, either. It's just... Odd..*");
                        Mes.Add("*Just how much food [gn:"+CompanionDB.Vladimir+"] can eat? Does he have a hole in his stomach or something?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Malisha))
                    {
                        Mes.Add("*I guess things wont be as peaceful as they were, huh..? [gn:"+CompanionDB.Malisha+"] is here...*");
                        Mes.Add("*It wasn't inusual receiving news about troubles regarding [gn:"+CompanionDB.Malisha+"] in the ether realm. A number of them also involved her mentor.*");
                        if (CanTalkAboutCompanion(CompanionDB.Leopold))
                        {
                            Mes.Add("*Let me guess, [gn:"+CompanionDB.Malisha+"] used [gn:"+CompanionDB.Leopold+"] as test subject on some experiment?*");
                            Mes.Add("*If you ever get to hear a scream out of somewhere, then might be one thing: [gn:"+CompanionDB.Leopold+"] and [gn:"+CompanionDB.Malisha+"] are together.*");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Minerva))
                    {
                        Mes.Add("*I'm loving even more hanging around this world. The food [gn:"+CompanionDB.Minerva+"] makes can convince anyone to stay.*");
                        Mes.Add("(Burp) *Oh, sorry. I just ate a lot and I am stuffed. Wait, why are you looking at my belly?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
                    {
                        Mes.Add("*We got quite a cute addition to this world, huh?*");
                        Mes.Add("*I always have to keep a cup of water ready whenever I try the foods [gn:"+CompanionDB.Cinnamon+"] makes.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                    {
                        Mes.Add("*(Pst... Is there someone on my shoulder?)*");
                        Mes.Add("*I know that [gn:"+CompanionDB.Fluffles+"] doesn't mean any harm to us, but her presence here is still quite creepy.*");
                        if (CanTalkAboutCompanion(CompanionDB.Leopold))
                        {
                            Mes.Add("*It seems like [gn:"+CompanionDB.Leopold+"] got his own ghost haunting him..*");
                        }
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Liebre))
                    {
                        Mes.Add("*Nobody died yet, or at least since I know. Wait, you aren't here to ask me about the reaper, right?*");
                        Mes.Add("*It's quite disturbing to see through [gn:"+CompanionDB.Liebre+"]'s shell. Are those... People.. Floating in his shell?*");
                        Mes.Add("*I wonder, what happened to [gn:"+CompanionDB.Liebre+"]'s second half of his body? Ask him? Are you crazy?!*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Miguel))
                    {
                        Mes.Add("*If [gn:"+CompanionDB.Miguel+"] doesn't stop making fun of my belly, I will bonk his head so hard that he will instead go for astronomy, instead of body building.*");
                        Mes.Add("*Who does [gn:"+CompanionDB.Miguel+"] think he's calling fatty? Do I even look fatty? I mean.. Yeah, but... What a rude...*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Scaleforth))
                    {
                        Mes.Add("*If anyone asked me what the perfect man for me would look like, I'd tell them to look at [gn:"+CompanionDB.Scaleforth+"].*");
                        Mes.Add("*I love spending my lunch with [gn:"+CompanionDB.Scaleforth+"]. His words, his eyes, his paws... Everything on him... Interests me..*");
                    }
                }
                else
                {
                    Mes.Add("*Back off! Now's not the time.*");
                    Mes.Add("*You're starting to buzz my patience. What do you want?*");
                    Mes.Add("*Grr... Couldn't you talk to me on a less horrible time?*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I think I've begun growing attached to this world, and its people. Feels like I belong here.*");
            Mes.Add("*This place is actually more interesting than the Ether Realm. Is it the people?*");
            Mes.Add("*I guess I should thank you for my time here, huh?*");
            if (CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*Don't tell [gn:"+CompanionDB.Brutus+"], but I actually like talking to him.*");
                if (CanTalkAboutCompanion(CompanionDB.Celeste))
                {
                    Mes.Add("*I'm not much of a religious person, but since finding out of [gn:"+CompanionDB.Brutus+"] whereabouts, I've begun praying. Just don't tell this to him.*");
                }

            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return "*I like your enthusiasm, but I need nothing else right now.*";
                case RequestContext.HasRequest: //[objective] tag useable for listing objective
                    if (Main.rand.NextBool(2))
                        return "*You have triggered my trap card. Haha, just kidding. I just need you to [objective], if possible.*";
                    return "*I'm glad that you mentioned that. I need someone to [objective] for me. Would you be that someone?*";
                case RequestContext.Completed:
                    if (Main.rand.NextBool(2))
                        return "*I'm glad to hear that you managed to do it. I guess I shouldn't fear that it would be too much for you.*";
                    return "*Congratulations! You didn't disappointed me. I hope you do more requests for me in the future.*";
                case RequestContext.Accepted:
                    if (Main.rand.NextBool(2))
                        return "*Do be careful and do not be reckless when doing my request, okay?*";
                    return "*Try not to get yourself killed doing that.*";
                case RequestContext.TooManyRequests:
                    return "*I'm not a fan of quantity over quality, so I wont add my request to your list for now. Take care of your other tasks first.*";
                case RequestContext.Rejected:
                    if (Main.rand.NextBool(2))
                        return "*I guess I don't have the charm I thought I had. Oh well, at least was an attempt.*";
                    return "*Oh well, I guess I should be less lazy and try doing it myself, haha.*";
                case RequestContext.PostponeRequest:
                    return "*Got something more important than helping me? Cold hearted Terrarian, huh? Just kidding, just kidding.*";
                case RequestContext.Failed:
                    return "*You what?! I should have known my request would be too much for you, now I have some mess to clean.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Did you take care of my little request?*";
                case RequestContext.RemindObjective: //[objective] tag useable for listing objective
                    return "*Do Terrarians have short spanned memory? Gladly I don't. I asked you to [objective], can you remember that now?*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*Hahaha, very funny [nickname]. I thought I heard you say you want to drop my request.*";
                case RequestContext.CancelRequestYes:
                    return "*It wasn't a joke...? Oh well... You.. Wasted my time, then..? Fine, you're relieved from my request.*";
                case RequestContext.CancelRequestNo:
                    return "*For a moment I thought it wasn't a joke. I'm so glad it is.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*If you say please, I will. Just kidding, I can stay here. I just hope I have a spacious and comfy house to live.*";
                case MoveInContext.Fail:
                    return "*I don't feel like this is the moment for moving in here.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I don't think this place gave me enough reasons why I should live here, instead of my actual house.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You what? I thought we were friends! Just kidding, hehe.. Oh well... I'll pack my things then.*";
                case MoveOutContext.Fail:
                    return "*I'm going nowhere now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*You have the audacity to try kicking me out? Who do you think you are?*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    if (PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Brutus) && Main.rand.NextBool(2))
                        return "*I see that [gn:"+CompanionDB.Brutus+"] is coming too, so how can I refuse?*";
                    if (Main.rand.NextBool(2))
                        return "*I don't mind adding up to your numbers. I'm in.*";
                    return "*I shall be your sword for a while, then.*";
                case JoinMessageContext.Fail:
                    return "*I don't feel like joining your adventuring group right now.*";
                case JoinMessageContext.FullParty:
                    return "*Isn't there too many people in your group?*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    if (PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, CompanionDB.Brutus) && Main.rand.NextBool(2))
                        return "*See ya [gn:"+CompanionDB.Brutus+"], return in one piece, alright?*";
                    return "*Don't need my company? Fine, then this sword will be sheathed, and go back home.*";
                case LeaveMessageContext.Fail:
                    return "*You're not getting rid of me right now.*";
                case LeaveMessageContext.AskIfSure:
                    return "*Couldn't you ask me that once we got in a safe place?*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I guess that's a no. Well, I'll fight my way back home, then.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Thank you.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Sure that I could use some weight on my shoulder. Might even make me stronger too.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*I hope you wont let me fall.*";
                case MountCompanionContext.Fail:
                    return "*Keep using your own feet for now.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*I could still make use of my other arm, so no.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I'll carry them for you. I hope they don't use something loud on my ears.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*Yes, I can. Who should I carry?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*To the ground you go. It was a good exercise.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Thanks for the ride, but I doubt that was good for my fat. I'm not complaining, I'm not complaining.*";
                case DismountCompanionContext.Fail:
                    return "*You're not getting off my shoulder right now.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Yes, I don't mind but... Your beds are way too small for me, so where would you sleep at?*";
            return "*Maybe it's for the better that we take different beds.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*Yes, I can share my chair. Just don't expect me to pet your head.*";
            return "*Fine. Then we take different chairs.*";
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                    case SleepingMessageContext.WhenSleeping:
                        switch(Main.rand.Next(3))
                        {
                            default:
                                return "(She seems to have blacked out, or is what it seems.)";
                            case 1:
                                return "(She's snoring a lot. You wonder how one could sleep with such snoring noises.)";
                            case 2:
                                return "(No kind of sound in the world would possibly wake her up right now.)";
                        }
                case SleepingMessageContext.OnWokeUp:
                    return "*Huh? Yawn~ You have 3 seconds to tell me why you woke me up. If you don't have an actual reason, I'll bonk your head.*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*Wha~ Oh, it's you. I hope it's about my request, or else I'll get violent.*";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*Need me to review how I'll take on in combat? You know I'm better at close range, right?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*Got it. I will give trouble to my foes.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Take some distance then? I guess that's fine.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*I guess I wont really be needing to use much my sword... But yes, I can do that.*";
                case TacticsChangeContext.Nevermind:
                    return "*Everything seems fine for you, then? Okay, then I'll keep taking on combat as previously.*";
                case TacticsChangeContext.FollowAhead:
                    return "*Perfect! Allow me to show you what sword fight actually is.*";
                case TacticsChangeContext.FollowBehind:
                    return "*I'll be following you, then.*";
                case TacticsChangeContext.AvoidCombat:
                    return "*What? You can't be serious!*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*I was itching for when you'd tell me that.*";
                case TacticsChangeContext.AllowSubattackUsage:
                    return "*Finally. Time to distribute pain.*";
                case TacticsChangeContext.UnallowSubattackUsage:
                    return "*Fine. But I hope you know when I should use my attacks then.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*What is it? There is something specific you want to talk to me about? Or want to know more about me?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Got it. Anything else you want to speak about?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*It's okay. I'm better at fighting than talking, anyways.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    if (Main.rand.NextBool(2))
                        return "*May we be fused as one, and my body be the shield that protects you, and my arms be the tools for your means.*";
                    return "*Taking over control of this fool. Haha, did I scared you? Don't worry, you're in charge here.*";
                case ControlContext.SuccessReleaseControl:
                    return "*There goes your extra layer of protection, but don't worry, I'll still be following you.*";
                case ControlContext.FailTakeControl:
                    return "*This is not the moment for that.*";
                case ControlContext.FailReleaseControl:
                    return "*Doing that now might endanger you.*";
                case ControlContext.NotFriendsEnough:
                    return "*I'll keep having control of my body, if you don't mind.*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Oooooooohh.. I'm your conscieeeeeence... Tee hee. Need something from me?*";
                        case 1:
                            return "*It's really odd to just watch. I'm generally taking on creatures by myself.*";
                        case 2:
                            return "*Aww... Missed me talking?*";
                    }
                case ControlContext.GiveCompanionControl:
                    return "*I thought you needed my protection, not me to do the job for you. Oh well... At least is still an extra layer of protection.*";
                case ControlContext.TakeCompanionControl:
                    return "*Giving back control. And now I'm beginning to get bored again.*";
            }
            return base.ControlMessage(companion, context);
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "*You know, [nickname], I'm starting to like this place. If you have a vacant house somewhere, do let me know if I can take it. I wouldn't mind hanging around here for a while.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*You Terrarians look squishy compared to TerraGuardians, but don't worry, I will let you bond-merge onto me to offer you some extra protection.*";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*Stop right there! Your days of going without me on your adventures are over. From now on, I will not mind at all making you company on whatever dangers you get yourself into. Liked this piece of info, [nickname]?*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*You look tired, [nickname]. Need someone to carry you? I might be able to do that for you. Having some weight on my left shoulder should strengthen it too.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "*Hey [nickname], I've been hearing that you're really handy at taking care of people requests. Would you please be able to help me with my needs some times?*";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*[nickname], we have to talk. I loved every minute we spent talking and adventuring with each other, and I'd love to offer you something meaningful to that. Should you be looking for someone to be your buddy, know that I, [gn:"+CompanionDB.Leona+"], will gladly accept if you ever ask me, and I will be your sword for the rest of your life.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*Hey, newly-buddy. There are a few things I wanted to let you know. I shall carry on mostly anything you ask of me, as one of the benefits from our buddiship. I just hope our buddiship last for years, and that should you die, be of old age, because I wouldn't bear losing someone dear to me under my watch. Oops, I shouldn't have said that. Tee Hee. Lets keep that between us.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*If I can help, I will do. What is it that you ask of me?*";
                case InteractionMessageContext.Accepts:
                    return "*Yes, I can do that.*";
                case InteractionMessageContext.Rejects:
                    return "*No way.*";
                case InteractionMessageContext.Nevermind:
                    return "*Changed your mind, huh?*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*I'm a natural leader, so glad you recognize that.*";
                case ChangeLeaderContext.Failed:
                    return "*I don't think this is the moment for that.*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*Oh, you finally asked me then. This is a once in a life time propposal, [nickname], so think this well. Will you pick me as your Buddy for life, and have me as your sword?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*I.. That's... I'm sorry.. I tried to play tough but... I couldn't countain this surge of emotions... We... I accept being your Buddy, [nickname]. I shall be your sword, and I will be by your side for the rest of our lives. Thank you for picking me, [nickname]. Shall our lives be long and full of adventures.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*I'm so frustrated that I could bonk you in the head for that. Oh well... I guess that's why you'd not pick me...*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I'd preffer to discuss that with someone I like more.*";
                case BuddiesModeContext.Failed:
                    return "*Maybe this is not the best moment to discuss that.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*([nickname], you shouldn't be talking about that to me. You might end up hurting your buddy's feelings.)*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*Sure thing. I'll be on my way there, then.*";
                case InviteContext.SuccessNotInTime:
                    return "*Not right now, but I will be showing up in a more proper time. Don't worry.*";
                case InviteContext.Failed:
                    return "*I can't leave this place right now, sorry.*";
                case InviteContext.CancelInvite:
                    return "*Oh, you don't need me there anymore? Fine...*";
                case InviteContext.ArrivalMessage:
                    return "*I have arrived. What did you needed of me, [nickname]?*";
            }
            return "";
        }

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.GetModName)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Brutus:
                        Weight = 1.5f;
                        return "*Ah, so that's where you went to. You could have let me know.*";
                    case CompanionDB.Domino:
                        Weight = 1.5f;
                        return "*Since you're here, that means [gn:"+CompanionDB.Brutus+"] must be around.*";
                }
            }
            Weight = 1f;
            return "*A new face! Hello.*";
        }

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.GetModName)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Brutus:
                        Weight = 1.5f;
                        return "*Coming to show me your sword moves, [gn:"+CompanionDB.Brutus+"]? I've watched it many times before.*";
                    case CompanionDB.Domino:
                        Weight = 1.5f;
                        return "*Maybe I'll find out how you managed to outsmart the Royal Guard.*";
                }
            }
            Weight = 1f;
            return "*Hey! Welcome to our little party.*";
        }

        public override string CompanionLeavesGroupMessage(Companion WhoReacts, Companion WhoLeft, out float Weight)
        {
            if (WhoLeft.ModID == MainMod.GetModName)
            {
                switch (WhoLeft.ID)
                {
                    case CompanionDB.Brutus:
                        Weight = 1.5f;
                        return "*Going already? Aww...*";
                }
            }
            Weight = 1f;
            return base.CompanionLeavesGroupMessage(WhoReacts, WhoLeft, out Weight);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "*Why did you let him escape? He could have helped us!*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*I wish I could immortalize that as a portrait in my house wall.*";
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Well, Miss Knight. Let me know more about you...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Ugh... You smell...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*Your breath is also foul... Ew...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*I think that's enough aggression to my nostrils.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*You-You're not planning to use that sword, right?*";
                case MessageIDs.RPSAskToPlaySuccess:
                    return "*Hahaha, nice one [nickname]... Wait.. You're serious, aren't you? Fine.. Lets play then.*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*Hang on! I'm coming for you. Just resist a bit longer.*";
                case ReviveContext.RevivingMessage:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Come on... Mend faster...*";
                        case 1:
                            return "*I'm starting to regret dozing off on first aid classes...*";
                        case 2:
                            return "*Those monsters will pay for what they did to you.*";
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*Oh no! Hold on! I'm coming!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Got you. Try not to move too much.*";
                case ReviveContext.RevivedByItself:
                    return "*You thought you could kill me? I will get you now!*";
                case ReviveContext.ReviveWithOthersHelp:
                    return "*Ow, thanks for the help. Don't worry much about this, it's just a flesh wound.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override void ManageOtherTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            Leona.LeonaCompanion Leona = (Leona.LeonaCompanion)companion;
            if (Leona.HoldingSword)
            {
                dialogue.AddOption("Stop using your Greatsword.", RemoveSwordDialogue);
            }
            else
            {
                dialogue.AddOption("Use your Greatsword.", EquipSwordDialogue);
            }
        }

        private void EquipSwordDialogue()
        {
            Leona.LeonaCompanion Leona = (Leona.LeonaCompanion)Dialogue.Speaker;
            Leona.HoldingSword = true;
            MessageDialogue md = new MessageDialogue("*I was waiting until you said that. Time to bathe my sword in blood.*");
            md.RunDialogue();
        }

        private void RemoveSwordDialogue()
        {
            Leona.LeonaCompanion Leona = (Leona.LeonaCompanion)Dialogue.Speaker;
            Leona.HoldingSword = false;
            MessageDialogue md = new MessageDialogue("*I really hate that, but I will do so. Do let me know when I should be able to use my sword again.*");
            md.RunDialogue();
        }
    }
}