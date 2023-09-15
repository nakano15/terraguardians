using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class LunaDialogues : CompanionDialogueContainer
    {
        //Yep, need to work on it.
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*Hello Terrarian. I'm "+companion.GetName+", and I can help you with questions related to TerraGuardians.*";
                case 1:
                    return "*Hi. I'm "+companion.GetName+". I know many things about TerraGuardians, so I can help you in case you have questions.*";
                case 2:
                    return "*Please don't be scared, I'm friendly. We TerraGuardians are generally friendly. If you ever have questions about us, you can ask me.*";
            }
        }

        public override string NormalMessages(Companion guardian)
        {
            List<string> Mes = new List<string>();
            /*if (MainMod.IsPopularityContestRunning && !Main.bloodMoon)
            {
                Mes.Add("*The TerraGuardians Popularity Contest is running right now. Sorry, but I wont be hosting the event. Seek someone who is.*");
                string Hosts = "";
                if (WorldMod.HasCompanionNPCSpawned(Rococo))
                {
                    Hosts += "Rococo";
                }
                if (WorldMod.HasCompanionNPCSpawned(Blue))
                {
                    if (Hosts != "") Hosts += ", ";
                    Hosts += "Blue";
                }
                if (WorldMod.HasCompanionNPCSpawned(Mabel))
                {
                    if (Hosts != "") Hosts += ", ";
                    Hosts += "Mabel";
                }
                if(Hosts == "")
                {
                    Mes.Add("*If you're interested in participating in the popularity contest, I'm saddened to inform you that no known hosts are present in this world.*\n(Check the mod thread or discord server for the voting link.)");
                }
                else
                {
                    Mes.Add("*If you're interessed in participating in the popularity contest, you can speak with those companions to access the voting: "+Hosts+"*");
                }
            }*/
            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Those creatures... They doesn't seem native from here. Where are they coming from?*");
                    Mes.Add("*Be careful [nickname], they look tough.*");
                }
                else if (!Main.raining)
                {
                    Mes.Add("*This weather is so perfect. Want some lecturing?*");
                    Mes.Add("*What a fine day. Need something, [nickname]?*");
                    Mes.Add("*This sun will do wonders to my fur.*");
                }
                else
                {
                    Mes.Add("*I guess I should stay home...*");
                    Mes.Add("*Hm... I wonder when the rain will go away...*");
                    Mes.Add("*Need some questions answered? The rain may make the explanation less tedious.*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*Grrr... Blood Moons makes female citizens angry. I don't know why, so don't ask!*");
                    Mes.Add("*I'm... Not in the mood for talking... I don't think I can answer questions right now...*");
                    Mes.Add("*Leave me be...*");
                }
                else if (!Main.raining)
                {
                    Mes.Add("*Just a little longer until I can get some sleep.*");
                    Mes.Add("*I think I have some time to answer some questions. What troubles your mind?*");
                    Mes.Add("*You came to visit me. Are you planning on doing a pillow fight?*");
                }
                else
                {
                    Mes.Add("*Aahhh... This will go nice with hours of sleep...*");
                    Mes.Add("*This is the perfect time of day for rain. Sleeping with the rain sound, and the cold air around is so great.*");
                    Mes.Add("*Need my aid in something?*");
                }
            }
            if (!Main.bloodMoon)
            {
                Mes.Add("*Any question you have, feel free to ask. I may have an answer to it.*");
                Mes.Add("*Sometimes I hear from people rumors of other people around the world. They may be useful if you want to meet new people.*");
                Mes.Add("*Yes? Do you need something?*");
                Mes.Add("*I was expecting to see you!*");
                Mes.Add("*What can " + guardian.GetName + " help you with?*");
                if (CanTalkAboutCompanion(guardian.ID))
                {
                    Mes.Add("*I'm so happy that you let me live here. I love staying here with you Terrarians.*");
                }
                {
                    int TgNppCount = WorldMod.GetTerraGuardiansCount;
                    if (TgNppCount < 5)
                    {
                        Mes.Add("*I feel a little weird that there are not many people like me around. It's like as if I'm out of place.*");
                    }
                    else
                    {
                        if (TgNppCount >= 20)
                        {
                            Mes.Add("*Wow! There's a lot of TerraGuardians living here! It feel like I'm back home. I thank you for this.*");
                        }
                        else if (TgNppCount >= 10)
                        {
                            Mes.Add("*There's a lot of TerraGuardians living in your world. I'm glad to see you Terrarians and TerraGuardians are living well together.*");
                        }
                        else
                        {
                            Mes.Add("*I like seeing that there are people like me living here. Don't get me wrong, I like being around Terrarians, but I don't really feel out of place when people like me are in the world.*");
                        }
                    }
                }

                if (guardian.IsUsingToilet)
                {
                    Mes.Clear();
                    Mes.Add("*Yes, I can answer your questions, at least as long as you don't stare at me doing my business.*");
                    Mes.Add("*It is really embarrassing having someone watch you when you're.... You know what... People will not like being talked to while... Doing that.*");
                    Mes.Add("*Sorry, but you're distracting me from what I'm doing right now. Can't we talk later?*");
                }
                else
                {
                    if (guardian.IsPlayerRoomMate(MainMod.GetLocalPlayer))
                    {
                        Mes.Add("*I'm really happy for having a room mate, but I don't know if we can share the same bed.*");
                        Mes.Add("*Don't worry, I can leave the beds tidy until sleep time.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Rococo))
                    {
                        Mes.Add("*I'm so happy that you let [gn:"+CompanionDB.Rococo+"] stay around so many nice people, he deserves that.*");
                        Mes.Add("*I know [gn:"+CompanionDB.Rococo+"], he's from my hometown. People never cared about him, and he never managed to find a lair for him there. Only I gave him some affection sometimes.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Blue))
                    {
                        Mes.Add("*I really like talking with [gn:"+CompanionDB.Blue+"], she never makes me bored when talking to her.*");
                        Mes.Add("*Just like [gn:"+CompanionDB.Blue+"], I also care about my look. I just don't have long hair like she has.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Zacks))
                    {
                        Mes.Add("*Oh my, what happened to [gn:" + CompanionDB.Zacks + "]?*");
                        Mes.Add("*I know [gn:" + CompanionDB.Zacks + "] vaguely since I used to visit where he lived to collect some herbs.*");
                        Mes.Add("*I think... I think [gn:" + CompanionDB.Zacks + "] has been stalking around my house during the night. I hope I'm wrong.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Alex))
                    {
                        Mes.Add("*What a cute doggie you managed to get to your world. I really love petting [gn:"+CompanionDB.Alex+"]'s belly.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Mabel))
                    {
                        Mes.Add("*I don't mean to brag, but I won one edition of Miss North Pole once. Maybe I can help [gn:"+CompanionDB.Mabel+"] win an edition of it.*");
                        Mes.Add("*I didn't really needed to practice hard to participate of Miss North Pole. I think [gn:"+CompanionDB.Mabel+"] is exaggerating a bit.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Leopold))
                    {
                        Mes.Add("*Do you know about [gn:"+CompanionDB.Leopold+"]? He's a really famous sage from the Ether Realm. Anyone knows him, but I've only managed to talk to him after he moved to the Terra Realm.*");
                        Mes.Add("*If you manage to get [gn:"+CompanionDB.Leopold+"]'s mind focused on the discussion, I would be impressed.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Malisha))
                    {
                        Mes.Add("*If you ever wondered what a witch looks like, just look at [gn:"+CompanionDB.Malisha+"]. Or at least she does look like one.*");
                        Mes.Add("*Why does [gn:"+CompanionDB.Malisha+"] like wearing nothing?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                    {
                        Mes.Add("*At first, I was really scared about having [gn:" + CompanionDB.Fluffles + "] around, but then I found out she's really nice.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Minerva))
                    {
                        Mes.Add("*I really love the food [gn:"+CompanionDB.Minerva+"] makes, but I wish she cooked more things that don't have fat.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Liebre))
                    {
                        Mes.Add("*It seems like we got \'grimm\' company in this world... I'm now expecting news of people's death for some reason.*");
                        Mes.Add("*I know that [gn:"+CompanionDB.Liebre+"] said that he didn't come here to kill us but... I don't feel very safe knowing he's around.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Alexander))
                    {
                        /*if (AlexanderBase.HasAlexanderSleuthedGuardian(player, guardian.ID))
                        {
                            Mes.Add("*How did [gn:" + CompanionDB.Alexander + "] managed to know many things about me? I never spoke to anyone about most of it.*");
                        }*/
                        Mes.Add("*I really don't like people spying on me. If you can tell that to [gn:"+CompanionDB.Alexander+"], I would be grateful.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Miguel))
                    {
                        Mes.Add("*It's really good to have [gn:"+CompanionDB.Miguel+"] around. He's going to help me get fit.*");
                        Mes.Add("*Yes, I'm doing the exercises daily. Tell [gn:"+CompanionDB.Miguel+"] to stop sending people to tell me that.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
                    {
                        Mes.Add("*Oh my, [gn:" + CompanionDB.Cinnamon + "] is so cute, that every time I see her I want to hug.*");
                        Mes.Add("*I really wouldn't mind carrying [gn:" + CompanionDB.Cinnamon + "] around. She's so cute that I would look like I'm carrying a teddy.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Green))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Green+"] has a menacing face, but he's actually a good doctor. Visit him whenever you feel sick or hurt.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Cille))
                    {
                        Mes.Add("*I tried visiting [gn:"+CompanionDB.Cille+"], but she always refuses my company. I even tried to make her cheer up. Beside she giggled a bit, but she turned cold later, and told me to go away.*");
                        Mes.Add("*The other day I was lunching, until I noticed [gn:" + CompanionDB.Cille + "] watching. I offered some to her, and she quickly gobbled up my food. I think she must have been really hungry.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Celeste))
                    {
                        Mes.Add("*You want to know who "+MainMod.TgGodName+" is? I believe [gn:"+CompanionDB.Celeste+"] will be able to tell you.*");
                        Mes.Add("*Ever since [gn:" + CompanionDB.Celeste + "] appeared, I started feeling a lot better. I can't explain why.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Leona))
                    {
                        Mes.Add("*I was wanting to speak with you. Do you know what [gn:"+CompanionDB.Leona+"] means by spoiling the surprise?*");
                        Mes.Add("*Do you also try to keep distance when speaking with [gn:"+CompanionDB.Leona+"]? I mean... She's carrying that big sword, and seems ready to swing it, you know...*");
                    }
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*It's not like as if I'm a nerd or anything, but I really like answering questions people have.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*I don't have any requests right now. Check me out later, and I may have something for you.*";
                    return "*Not right now. Do you want to talk about anything else?*";
                case RequestContext.HasRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*Yes, I have something that I need your help with. Could you help me with.. [objective]?*";
                    return "*I'm so glad you asked, I was totally lost thinking about how I would solve this. Can you [objective] for me?*";
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "*Thank you so much! You don't have any idea how grateful I am right now. Thank you!*";
                    return "*I'm so happy that you managed to do what I asked. Thank you, [nickname].*";
                case RequestContext.Accepted:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*I'll be cheering for you.*";
                    return "*Go, [nickname]. Go.*";
                case RequestContext.TooManyRequests:
                    return "*[nickname], I recommend not overloading yourself with several requests, they can be detrimental to your health. My request can wait until you take care of one of them.*";
                case RequestContext.Rejected:
                    if (Main.rand.NextDouble() <= 0.5)
                        return "*Aww.... Alright. So... Want to talk about something else?*";
                    return "*No? Oh... I'll do that later then.*";
                case RequestContext.PostponeRequest:
                    return "*Well, there's no time limit, so it can wait.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*I'm so happy. You completed my request, right?*";
                case RequestContext.Failed:
                    return "*You couldn't manage to complete my request... I'm... I'm sorry... So... Anything else you need...?*";
                case RequestContext.RemindObjective:
                    return "*Did you forget what I asked you to do? It's fine, I'll tell you again. I asked you to [objective]. Don't hesitate to come back to me if you forget again.*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*Are you sure that you want to cancel my request? You can't complete it or is it too hard?*";
                case RequestContext.CancelRequestYes:
                    return "*Oh... I guess I should try doing it instead then... Alright, you no longer need to do my request.*";
                case RequestContext.CancelRequestNo:
                    return "*Then you asked that by mistake? Alright. Do you need anything else?*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    if(WorldMod.GetTerraGuardiansCount >= 5)
                        return "*With this many TerraGuardians living here, I would even feel like I'm at home. Thank you.*";
                    return "*Yes, I can move in here. There are not many TerraGuardians around, but it's fine.*";
                case MoveInContext.Fail:
                    return "*I'm sorry, but I don't think moving in right now is a good idea.*";
                case MoveInContext.NotFriendsEnough:
                    return "*I'd like to know the neighborhood first, before attempting to move in.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You don't want me around anymore? Oh well, I can't argue about that. If you ever have questions, feel free to call me.*";
                case MoveOutContext.Fail:
                    return "*I'll keep the house just for a bit longer if you don't mind.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*I'm sorry, but you're not the one who gave me this house.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*Sure thing. Be careful not to fall.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*Thank you [nickname].*";
                case MountCompanionContext.Fail:
                    return "*Not at this moment.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*I don't actually like that idea.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*Sure. I will do that.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*I can carry someone. Who do you want me to carry?*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*There you go, feet on the ground.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Yes, I can walk again. Thank you.*";
                case DismountCompanionContext.Fail:
                    return "*I'm sorry, but just wait a bit longer..*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*I'd be happy to accompany you on your adventure. Let's go.*";
                case JoinMessageContext.Fail:
                    return "*Sorry, but I can't join you right now...*";
                case JoinMessageContext.FullParty:
                    return "*I'd feel uncomfortable in the middle of so many people...*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*I don't like being left here, can't I leave the group in a safe place?*";
                case LeaveMessageContext.Success:
                    return "*Alright, I'll be heading home then. If you ever have any questions, come see me.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I'll try fighting my way home, then.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Alright. When we get into a town, you can ask me to leave again, if you want.*";
                case LeaveMessageContext.Fail:
                    return "*I can't leave you alone right now.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share) return "*I think there's enough bed for the both of us, as long as we respect each other space.*";
            return "*I hope there's another vacant bed when we get some sleep.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share) return "*Yes, you can sit on my lap whenever we use a chair.*";
            return "*Alright, let's use different chairs.*";
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    List<string> Mes = new List<string>();
                    Mes.Add("(She seems to be sleeping comfortably, beside the way she sleeps doesn't look like it...)");
                    Mes.Add("(You can hear her snoring softly.)");
                    Mes.Add("(She's speaking about a number of things during her sleep. She seems to be dreaming about tutoring someone.)");
                    return Mes[Main.rand.Next(Mes.Count)];
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "*Yawn.. Terrarian... It's late, do you need something?*";
                        case 1:
                            return "*I know that questions can rise any time, but couldn't you wait until tomorrow?*";
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return "*Huh, oh? So... Did you do my request?*";
                        case 1:
                            return "*I'm so sleepy... Tell me you completed my request...*";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*Should I review the way I act in combat?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*Take on monsters up close? Got it.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*Fight monsters in mid-range? Got it.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Keep distance while firing? Got it.*";
                case TacticsChangeContext.Nevermind:
                    return "*Is there something else you need?*";
                case TacticsChangeContext.FollowAhead:
                    return "*For you, I will do.*";
                case TacticsChangeContext.FollowBehind:
                    return "*I'll be following you, then.*";
                case TacticsChangeContext.AvoidCombat:
                    return "*I got it. Do not engage in combat, right?*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*I will help in combat too, then.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Is there something you need to know about TerraGuardians? Or is it about me?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Do you have any other question?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*I hope I helped.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override void ManageOtherTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            dialogue.AddOptionAtTop("I have some questions regarding TerraGuardians.", Companions.Luna.TutoringDialogues.StartTutoringDialogue);
        }

        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            dialogue.AddOption("Anything new recently?", CompanionSpawningTips.ShowTip);
        }
        
        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*May our fate be linked for a while, [nickname].*";
                case ControlContext.SuccessReleaseControl:
                    return "*I hope I was useful for you, [nickname].*";
                case ControlContext.FailTakeControl:
                    return "*Not right now.*";
                case ControlContext.FailReleaseControl:
                    return "*I'm sorry, but you'll be part of me for a little longer. I can't unmerge you right now.*";
                case ControlContext.NotFriendsEnough:
                    return "*This is too private to do with someone I don't know well..*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Do you need any assistance, [nickname]? Even though I can't control my body, we can still talk.*";
                        case 1:
                            return "*You are doing well, [nickname].*";
                        case 2:
                            return "*Be careful with my body. If I die, you will die too, [nickname]. We are Bond-Merged together, so whatever happens to me, will happen to you too.*";
                    }
                case ControlContext.GiveCompanionControl:
                    return "*Hm? If you say so, but I don't think other companions might like hearing that.*";
                case ControlContext.TakeCompanionControl:
                    return "*There. You have the control again.*";
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
                    return "*Please don't be shocked. If you ever need to do something dangerous that you need someone strong to do that, you can use me to do that. Don't worry, I trust you enough that my life is in your hands.*";;
                case UnlockAlertMessageContext.FollowUnlock:
                    return "";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*Hey [nickname], if you want, I can carry you on my shoulder. My legs are bigger than yours, so maybe it's faster if I carry you, while you tell me where to go.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*Hello again, [nickname]. You do look well, probably because the questions I answered helped in your quest. By the way, that's not what I wanted to talk about.. We know each other for a long time, and you hardly got disappointment, so... Should you think about picking me as your Buddy, I will wholeheartedly say yes.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*Hey [nickname], ready for a newly-turned-buddy lesson? Since we are Buddies, we transcended friendship, so whatever you ask of me, I will not deny it. Did you like learning about that?*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*What do you need help with?*";
                case InteractionMessageContext.Accepts:
                    return "*I'll do that.*";
                case InteractionMessageContext.Rejects:
                    return "*Sorry. No.*";
                case InteractionMessageContext.Nevermind:
                    return "*If you need anything else, feel free to ask me.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*I will take the lead then.*";
                case ChangeLeaderContext.Failed:
                    return "*I'm not leaving this spot.*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*Y-you want to pick me as your buddy? I-is that true, [nickname]?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*I... Ah... Thank you! You... Uh... You won't regret picking me. Thanks!*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*I... Understand...*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I still have questions about you which I need to find the answers to by myself so.. We can't be Buddies yet.*";
                case BuddiesModeContext.Failed:
                    return "*I have to reject, [nickname].*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*Oh [nickname]... You have a Buddy, did you forget? They're even following you.*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*I can visit you. I should be there any time soon.*";
                case InviteContext.SuccessNotInTime:
                    return "*I can visit you, but you'll need to wait until the next day for me to arrive.*";
                case InviteContext.Failed:
                    return "*Not a good idea at this moment.*";
                case InviteContext.CancelInvite:
                    return "*Oh, alright.*";
                case InviteContext.ArrivalMessage:
                    return "*I'm here. Do you need something? Or have questions for me?*";
            }
            return "";
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*Don't worry, you're safe with me. Just rest a bit.*";
                case ReviveContext.RevivingMessage:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("*How do I do this... How do I do this..?*");
                        Mes.Add("*I-Is this how I heal people?*");
                        Mes.Add("*I'm really better at tutoring, but not really that good with mending...*");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*Hang on, I'll help you!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Got 'em. Now we need a safe place.*";
                case ReviveContext.RevivedByItself:
                    if (Main.rand.NextFloat() <= 0.5f)
                        return "*I'm still a bit hurt... But I could have got some help...*";
                    return "*I'm awake, but this hurts a lot...*";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextFloat() <= 0.5f)
                        return "*Thank you all, I really mean it.*";
                    return "*I'm so grateful to have you all to help me.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldMessage1:
                    return "*Leopold! That's an extremely rude assumption!*";
                case MessageIDs.LeopoldMessage2:
                    return "*What do you mean? That means you're not that Terrarian's hostage?*";
                case MessageIDs.LeopoldMessage3:
                    return "*I joined their adventure because they asked me to, and I accepted. Where did that crazy story of yours come from?*";
                case MessageIDs.LeopoldEscapedMessage:
                    return "*No Leopold, wait! Oh snap.. He's gone.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Well... I'm glad he's friendly. Someone got a camera?*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}
