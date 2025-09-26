using System.Collections.Generic;
using nterrautils;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions.Green;

public class GreenDialogue : CompanionDialogueContainer
{
    public override string GreetMessages(Companion companion)
    {
        switch (Main.rand.Next(3))
        {
            default:
                return "*Need a doctor? You found one.*";
            case 1:
                return "*It's always good to meet someone new.*";
            case 2:
                return "*Are you injured? I can take care of that.*";
        }
    }

    public override string NormalMessages(Companion companion)
    {
        Player player = MainMod.GetLocalPlayer;
        if (PlayerMod.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
        {
            return "*Uh, [nickname]... There is a ghost on your shoulder.*";
        }
        List<string> Mes = new List<string>();
        if(player.statLife < player.statLifeMax2 * 0.33f)
        {
            Mes.Add("*[nickname]! Hang on! You need medical help, right now.*");
            Mes.Add("*What happened to you? Hang on, sit down a bit.*");
            Mes.Add("*You're bleeding too much. Let's see if I can help you with those wounds.*");
        }
        else
        {
            Mes.Add("*I have treated many people in the Ether Realm. Let's see if I can do the same here.*");
            Mes.Add("*Why people seems scared of me? Is it because of my profession?*");
            Mes.Add("*Whatever wound you show me, probably isn't the gruesomest thing I've ever seen.*");
            Mes.Add("*Are you in need of a check up?*");
            Mes.Add("*In case your back is aching, I can solve that with ease.*");
            Mes.Add("*Do I look angry, or scary? Sorry, my face is just like that.*");

            if (Main.dayTime)
            {
                if (Main.eclipse)
                {
                    Mes.Add("*Please direct the injured to my room.*");
                    Mes.Add("*I wont rest this eclipse, right?*");
                    Mes.Add("*Are you hurt? Those monsters look tough.*");
                }
                else
                {
                    Mes.Add("*Enjoying the time, [nickname]?*");
                    Mes.Add("*You visited me, that means either you are injured, sick, or wanted to check me out.*");
                    Mes.Add("*I'm not treating anyone right now, so I can spend some time talking.*");
                }
            }
            else
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("*What is wrong with the women here? They all look furious this night.*");
                    Mes.Add("*I don't know what's scarier this night: The monsters or the women.*");
                    Mes.Add("*I wont have a good night of sleep today, right?*");
                }
                else
                {
                    Mes.Add("*I was about to get some sleep. Need something before I do so?*");
                    Mes.Add("*Feeling tired? Me too.*");
                    Mes.Add("*You would not believe your eyes, if ten million fireflies, lit up the world as I fall asleep.*");
                }
            }

            if (IsPlayerRoomMate())
            {
                Mes.Add("*There is enough space on my house for both of us, so I can share it with you.*");
                Mes.Add("*I really hope I don't make you sick whenever I change my skin.*");
            }
            if (companion.IsPlayerBuddy(player))
            {
                Mes.Add("*Feeling fine, [nickname]? Just checking you up.*");
            }
            if (HasCompanion(CompanionDB.Sardine))
            {
                Mes.Add("*It has been many times I've had [gn:" + CompanionDB.Sardine + "] be brought by someone to my house, unconscious.*");
                Mes.Add("*Everytime is the same: [gn:" + CompanionDB.Sardine + "] is brought to me unconscious to me, I treat him, the he gets jump scared when he wakes up. He should have been used to my face.*");
            }
            if (HasCompanion(CompanionDB.Bree))
            {
                Mes.Add("*Why [gn:" + CompanionDB.Bree + "] takes so many vitamins?*");
            }
            if (HasCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*For someone who charges into fights, [gn:" + CompanionDB.Brutus + "] only visits me with light wounds.*");
                Mes.Add("*I can't help but notice that [gn:" + CompanionDB.Brutus + "] has his left arm stronger than the right. Is it because he practices swinging his sword?*");
            }
            if (HasCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*Ever since [gn:" + CompanionDB.Mabel + "] moved in, I had to get a new batch of tissues.*");
                Mes.Add("*I tried recommending some diet for [gn:" + CompanionDB.Mabel + "]. She didn't took that well.*");
            }
            if (HasCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*I'm glad that my old skins are being useful for [gn:" + CompanionDB.Malisha + "]. I only wonder what she uses it for.*");
            }
            if (HasCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*Why does [gn:" + CompanionDB.Cinnamon + "] thinks I will eat her?*");
                Mes.Add("*In some extreme cases, I have to make [gn:" + CompanionDB.Cinnamon + "] sleep in order to treat her disease or wounds.*");
            }
            if (HasCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*It seems like I managed to get into some synergy with [gn:" + CompanionDB.Miguel + "]'s work. He gives exercises to people, while I help with their nutrition.*");
            }
            if (HasCompanion(CompanionDB.Cille))
            {
                Mes.Add("*Ah, good that you came. Your friend [gn:" + CompanionDB.Cille + "] has visited me earlier. She said that had something wrong with her, but I did a checkup, and I didn't found anything wrong.*");
            }
        }
        return Mes[Main.rand.Next(Mes.Count)];
    }

    public override string TalkMessages(Companion companion)
    {  
        List<string> Mes = new List<string>();
        Mes.Add("*Initially, I didn't moved to this world to treat people, but was more for curiosity. I wanted to know how the Terra Realm look like.*");
        Mes.Add("*I would be grateful if people were less scared of me.*");
        Mes.Add("*I don't recommend you to make treatment harder to a medic. It can actually end up making the treatment worser and painful.*");
        Mes.Add("*Don't underestimate diseases. Even little coughs could be something serious if you don't be careful, so watch yourself.*");
        return Mes[Main.rand.Next(Mes.Count)];
    }

    public override string RequestMessages(Companion companion, RequestContext context)
    {
        switch (context)
        {
            case RequestContext.NoRequest:
                if (Main.rand.NextBool(2))
                    return "*I have no necessities right now.*";
                return "*There isn't much I need help with right now. Maybe another time.*";
            case RequestContext.HasRequest:
                if (Main.rand.NextBool(2))
                    return "*Perfect timing! I need you to do something for me. Can you [objective]?*";
                return "*Yes... As a matter of fact, I have something that I need your help with. Can you help me out with my problem, [objective]?*";
            case RequestContext.Completed:
                if (Main.rand.NextBool(2))
                    return "*Perfect job. You're reliable, [nickname].*";
                return "*I shouldn't expect less of you, [nickname].*";
            case RequestContext.Accepted:
                if (Main.rand.NextDouble() < 0.5)
                    return "*I'll await your return.*";
                return "*Don't disappoint me, [nickname].*";
            case RequestContext.TooManyRequests:
                return "*If I gave you my request, I would only help stressing you out. Only try doing what you can manage to do at a time.*";
            case RequestContext.Rejected:
                return "*Hm... Disappointing.*";
            case RequestContext.PostponeRequest:
                return "*It can wait, sure.*";
            case RequestContext.Failed:
                if (Main.rand.NextDouble() < 0.5)
                    return "*I shouldn't have entrusted that to you.*";
                return "*You disappointed me, [nickname]. Don't do that again.*";
            case RequestContext.AskIfRequestIsCompleted:
                return "*Bring me good news, [nickname]. Did you do what I asked?*";
            case RequestContext.RemindObjective:
                return "*You forgot my request? I asked you to [objective].*";
            case RequestContext.CancelRequestAskIfSure:
                return "*Are you sure you want to cancel my request?*";
            case RequestContext.CancelRequestYes:
                return "*Why did you accept it in first place...?*";
            case RequestContext.CancelRequestNo:
                return "*You nearly disappointed me, [nickname].*";
        }
        return base.RequestMessages(companion, context);
    }

    public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
    {
        switch (context)
        {
            case ReviveContext.ReachedFallenAllyMessage:
                switch (Main.rand.Next(3))
                {
                    default:
                        return "*Don't worry, you'll be fixed in a moment.*";
                    case 1:
                        return "*Stitching some wounds.*";
                    case 2:
                        return "*Bleeding stopped.*";
                }
            case ReviveContext.HelpCallReceived:
                return "*Don't worry, you'll be waking up soon.*";
            case ReviveContext.ReviveWithOthersHelp:
                return "*I'm really glad to see you people managed to heal my wounds.*";
            case ReviveContext.RevivedByItself:
                return "*I can fight again, Thanks.*";
            case ReviveContext.OnComingForFallenAllyNearbyMessage:
                return "*Hold on, I'm coming!*";
        }
        return base.ReviveMessages(companion, target, context);
    }

    public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
    {
        if(WhoJoined.ModID == MainMod.mod.Name)
        {
            if(WhoJoined.ID == CompanionDB.Leopold)
            {
                Weight = 1.5f;
                return "*You are here too, "+CompanionDB.Leopold+"? This world surelly is interesting.*";
            }
        }
        Weight = 1f;
        return "*One more person.*";
    }

    public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
    {
        if (WhoJoined.ModID == MainMod.mod.Name)
        {
            switch (WhoJoined.ID)
            {
                case CompanionDB.Leopold:
                    Weight = 1.5f;
                    return "*I think I will finally witness " + WhoJoined.GetNameColored() + "'s magic mastery.*";
                case CompanionDB.Malisha:
                    Weight = 1.5f;
                    return "*Oh, she's coming too.*";
                case CompanionDB.Cinnamon:
                    Weight = 1.2f;
                    return "*I hope you don't end up intoxicated again.*";
                case CompanionDB.Brutus:
                case CompanionDB.Sardine:
                    Weight = 1.5f;
                    return "*I can predict that I will have lots of work.*";
            }
        }
        Weight = 1f;
        return "*I'm glad to see you join us.*";
    }

    public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
    {
        switch (context)
        {
            case BuddiesModeContext.PlayerSaysYes:
                return "*I'm really happy that you picked me as your buddy, but that doesn't mean I wont charge for my services.*";
            case BuddiesModeContext.AskIfPlayerIsSure:
                return "*You want me to be your Buddy? Is that true, [nickname]? You can't change your mind once you say 'Yes'.*";
            case BuddiesModeContext.PlayerSaysNo:
                return "*So, that was just a prank, right? This is a serious thing to ask, [nickname].*";
            case BuddiesModeContext.NotFriendsEnough:
                return "*We hardly know each other enough for that.*";
            case BuddiesModeContext.AlreadyHasBuddy:
                return "*You've got a buddy already.*";
        }
        return base.BuddiesModeMessage(companion, context);
    }

    public override string SleepingMessage(Companion companion, SleepingMessageContext context)
    {
        switch (context)
        {
            case SleepingMessageContext.WhenSleeping:
                switch (Main.rand.Next(3))
                {
                    default:
                        return "(He's sleeping? I think he's sleeping. His eyes... Doesn't seems like his regular eyes.)";
                    case 1:
                        return "(You try waving your hand in front of his eye, but see no reaction. He must be sleeping.)";
                    case 2:
                        return "(You shiver while watching him sleeping.)";
                }
            case SleepingMessageContext.OnWokeUp:
                if(Main.rand.NextDouble() < 0.5)
                    return "*[nickname], many things I wont mind, but waking me up in the middle of my slumber isn't one of them.*";
                return "*You woke me up. I hope it is important.*";
            case SleepingMessageContext.OnWokeUpWithRequestActive:
                if(Main.rand.NextDouble() < 0.5)
                    return "*I really hope you woke me up because you completed my request.*";
                return "*You got me off my bed. You must have done my request, right?*";
        }
        return base.SleepingMessage(companion, context);
    }

    public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
    {
        switch (context)
        {
            case JoinMessageContext.Success:
                return "*Alright. I can be your field medic, then.*";
            case JoinMessageContext.FullParty:
                return "*There is too many people.*";
            case JoinMessageContext.Fail:
                return "*Right now, I don't think it's good to join your travels.*";
        }
        return base.JoinGroupMessages(companion, context);
    }

    public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
    {
        switch (context)
        {
            case LeaveMessageContext.AskIfSure:
                return "*This place is somewhat dangerous. Do you want to leave me here?*";
            case LeaveMessageContext.Success:
                return "*Visit me whenever you're wounded.*";
            case LeaveMessageContext.DangerousPlaceYesAnswer:
                return "*Well, I guess I'll have to fight my way home, then. And probably gather some snacks on the way.*";
            case LeaveMessageContext.DangerousPlaceNoAnswer:
                return "*If you want to remove me from your group, find a safe place with someone close.*";
        }
        return base.LeaveGroupMessages(companion, context);
    }

    public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
    {
        switch (context)
        {
            case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                return "*Do you want to know about something?*";
            case TalkAboutOtherTopicsContext.AfterFirstTime:
                return "*Anything else you want to talk about?*";
            case TalkAboutOtherTopicsContext.Nevermind:
                return "*Nevermind.*";
        }
        return base.TalkAboutOtherTopicsMessage(companion, context);
    }

    public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
    {
        switch (context)
        {
            case UnlockAlertMessageContext.FollowUnlock:
                return "*I think I had enough of studying terrarian anatomy day and night. I would like to know more about your world too, if you're willing to take me with you.*";
            case UnlockAlertMessageContext.MountUnlock:
                return "*If you want, I can carry you on my shoulder. That is, if you don't mind touching cold scale.*";
            case UnlockAlertMessageContext.ControlUnlock:
                return "*I think I know you enough to entrust you with myself. I can take on whatever adventures you can't go, if you want.*";
        }
        return base.UnlockAlertMessages(companion, context);
    }

    public override string GetOtherMessage(Companion companion, string Context)
    {
        switch (Context)
        {
            case MessageIDs.AlexanderSleuthingStart:
                    return "*Tell me more about yourself, doctor.*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... Seems like you've been treating many people...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*...What have you been eating? It sounds like...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*This... This guy is scary... I can now see why there's less critters around here.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Y-you don't plan on squeezing me, right?*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Healing his loneliness, [nickname]?*";
        }
        return base.GetOtherMessage(companion, Context);
    }

    public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
    {
        if (QuestContainer.HasQuestBeenCompleted(QuestDB.GreenHealingUnlock, MainMod.GetModName))
            dialogue.AddOption("I'm hurt.", HealDialogueLobby);
    }

    int Price = 0;
    bool NearDeath = false;

    void HealDialogueLobby()
    {
        PlayerMod pl = Main.LocalPlayer.GetModPlayer<PlayerMod>();
        Price = pl.Player.statLifeMax2 - pl.Player.statLife;
        //int WoundDebuffID = ModContent.BuffType<Buffs.LightWound>(),
        //    HeavyWoundDebuffID = ModContent.BuffType<Buffs.VeryWounded>();
        for (int b = 0; b < pl.Player.buffType.Length; b++)
        {
            int buffid = pl.Player.buffType[b];
            if(Main.debuff[buffid] && pl.Player.buffType[b] > 5 && !BuffID.Sets.NurseCannotRemoveDebuff[buffid])
            {
                Price += 800;
            }
            /*if (buffid == WoundDebuffID)
                Price += 200;
            if (buffid == HeavyWoundDebuffID)
                Price += 8000;*/
        }
        NearDeath = false;
        if(pl.Player.statLife < pl.Player.statLifeMax2 * 0.33f)
        {
            Price = (int)System.Math.Max(Price * 0.35f, 1f);
            NearDeath = true;
        }
        foreach(Companion tg in pl.GetSummonedCompanions)
        {
            if (tg != null && tg.KnockoutStates == KnockoutStates.Awake)
            {
                Price += tg.statLifeMax2 - tg.statLife;
                for (int b = 0; b < tg.buffType.Length; b++)
                {
                    int type = tg.buffType[b], time = tg.buffTime[b];
                    if(Main.debuff[type] && time > 5 && !BuffID.Sets.NurseCannotRemoveDebuff[type])
                    {
                        Price += 800;
                    }
                    /*if (type == WoundDebuffID)
                        Price += 200;
                    if (type == HeavyWoundDebuffID)
                        Price += 8000;*/
                }
            }
        }
        if (Price <= 0)
        {
            MessageDialogue md = new MessageDialogue("*Nobody seems to need medication or wounds treated.\nDon't spend my time.*");
            md.AddOption("Oh..", Dialogue.LobbyDialogue);
            md.RunDialogue();
        }
        else
        {
            Price = (int)System.Math.Max(Price * .35f, 1f);
            int c = Price, s = 0, g = 0, p = 0;
            if (c >= 100)
            {
                s += c / 100;
                c -= s * 100;
            }
            if (s >= 100)
            {
                g += s / 100;
                s -= g * 100;
            }
            if (g >= 100)
            {
                p += g / 100;
                g -= p * 100;
            }
            string PriceText = "";
            if (c > 0)
                PriceText = " " + c + " Coppers";
            if (s > 0)
            {
                if (PriceText != "")
                    PriceText = "," + PriceText;
                PriceText = " " + s + " Silvers";
            }
            if (g > 0)
            {
                if (PriceText != "")
                    PriceText = "," + PriceText;
                PriceText = " " + g + " Golds";
            }
            if (p > 0)
            {
                if (PriceText != "")
                    PriceText = "," + PriceText;
                PriceText = " " + p + " Platinum";
            }
            MessageDialogue md = new MessageDialogue(NearDeath ? "Due to the state you encounter yourself into, I'll only charge you\n"+PriceText+" for the treatment.*" :
                    "*I see that you're in need of treatment.\nI can take care of your wounds and ailments if you give me\n" + PriceText + ". Want me to begin treatment?*");
            md.AddOption("Yes, heal me.", DoHeal);
            md.AddOption("I changed my mind..", NevermindHeal);
            md.RunDialogue();
        }
    }

    void DoHeal()
    {
        PlayerMod pl = Main.LocalPlayer.GetModPlayer<PlayerMod>();
        if (pl.Player.BuyItem(Price, 1))
        {
            string Message = "";
            if (pl.Player.statLife < pl.Player.statLifeMax2 * 0.33f)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Message = "*I recommend you to lay down for a while and let your wounds heal, before returning to action.*";
                        break;
                    case 1:
                        Message = "*I doubt you can jump of happiness, but at least you will live.*";
                        break;
                    case 2:
                        Message = "*I think I can give you clearance to go. You will still need some time to recover your wounds.*";
                        break;
                }
            }
            else if (pl.Player.statLife < pl.Player.statLifeMax2 * 0.67f)
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Message = "*Your wounds have been treated, and all your ailments were cured. You may feel a bit of pain when exercising the areas that were wounded, but they will go away soon.*";
                        break;
                    case 1:
                        Message = "*It's all done. Just be careful not to open the wounds again during your travels.*";
                        break;
                    case 2:
                        Message = "*Next time you get injured, don't hexitate to visit me.*";
                        break;
                }
            }
            else
            {
                switch (Main.rand.Next(3))
                {
                    case 0:
                        Message = "*That was a minor wound. You'll be fine.*";
                        break;
                    case 1:
                        Message = "*It wasn't hard to take care of that issue. You can go now.*";
                        break;
                    case 2:
                        Message = "*All patched up.*";
                        break;
                }
            }
            pl.Player.statLife = pl.Player.statLifeMax2;
            for (int b = 0; b < pl.Player.buffType.Length; b++)
            {
                int buffid = pl.Player.buffType[b];
                if ((Main.debuff[buffid]/* || buffid == WoundDebuffID || buffid == HeavyWoundDebuffID*/) && pl.Player.buffType[b] > 5 && BuffID.Sets.NurseCannotRemoveDebuff[buffid])
                {
                    pl.Player.DelBuff(b);
                    b -= 1;
                }
            }
            foreach (Companion tg in pl.GetSummonedCompanions)
            {
                if (tg != null && !tg.dead)
                {
                    if (tg.KnockoutStates > KnockoutStates.Awake)
                    {
                        tg.Teleport(pl.Player.Bottom);
                        tg.GetPlayerMod.LeaveKnockoutState();
                    }
                    tg.statLife = tg.statLifeMax2;
                    for (int buff = tg.buffType.Length - 1; buff >= 0; buff--)
                    {
                        int type = tg.buffType[buff], time = tg.buffTime[buff];
                        if ((Main.debuff[type] /*|| type == WoundDebuffID || type == HeavyWoundDebuffID*/) && time > 5 && BuffID.Sets.NurseCannotRemoveDebuff[type])
                        {
                            tg.DelBuff(buff);
                        }
                    }
                }
            }
            Dialogue.LobbyDialogue(Message);
        }
        else
        {
            pl.Player.statLife = pl.Player.statLifeMax2;
            foreach (Companion tg in pl.GetSummonedCompanions)
            {
                if (tg != null && !tg.dead)
                {
                    if (tg.KnockoutStates > KnockoutStates.Awake)
                    {
                        tg.Teleport(pl.Player.Bottom);
                        tg.GetPlayerMod.LeaveKnockoutState();
                    }
                    tg.statLife = tg.statLifeMax2;
                }
            }
            Dialogue.LobbyDialogue("*If you cannot pay, at least I can treat your wounds, but I wont be able to take care of your health issues.*");
        }
    }

    void NevermindHeal()
    {
        PlayerMod pl = Main.LocalPlayer.GetModPlayer<PlayerMod>();
        if (NearDeath)
        {
            pl.Player.statLife = pl.Player.statLifeMax2;
            foreach (Companion tg in pl.GetSummonedCompanions)
            {
                if (tg != null && !tg.dead)
                {
                    if (tg.KnockoutStates > KnockoutStates.Awake)
                    {
                        tg.Teleport(pl.Player.Bottom);
                        tg.GetPlayerMod.LeaveKnockoutState();
                    }
                    tg.statLife = tg.statLifeMax2;
                }
            }
            Dialogue.LobbyDialogue("*No way. I can't simply let you leave like that.\nLet me at least take care of your wounds.*");
            return;
        }
        Dialogue.LobbyDialogue("*Changed your mind? It's fine.*");
    }
}