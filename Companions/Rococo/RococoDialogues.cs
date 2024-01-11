using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class RococoDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(4))
            {
                case 0:
                    return "\"At first, the creature got surprised after seeing me, then starts laughing out of happiness.\"";
                case 1:
                    return "\"That creature waves at you while smiling, It must be friendly, I guess?\"";
                case 2:
                    return "\"For some reason, that creature got happy after seeing you, maybe It wasn't expecting another human in this world?";
                default:
                    return "\"What sort of creature is that? Is it dangerous? No, It doesn't look like it.\"";
            }
        }

        public override string NormalMessages(Companion guardian)
        {
            bool MerchantInTheWorld = NPC.AnyNPCs(NPCID.Merchant), SteampunkerInTheWorld = NPC.AnyNPCs(NPCID.Steampunker);
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon && !Main.eclipse)
            {
                Mes.Add("*[name] is happy seeing you.*");
                Mes.Add("*[name] asks if you brought him something to eat.*");
                Mes.Add("*[name] is asking if you want to play with him.*");
                Mes.Add("*[name] wants you to check out some of his toys.*");
                Mes.Add("*[name] seems very glad to see that you are safe.*");
                if(!guardian.IsFollower)
                    Mes.Add("*[name] is asking you if you came to ask him to go on an adventure.*");
                if (guardian.HasBuff(Terraria.ID.BuffID.WellFed) || guardian.HasBuff(Terraria.ID.BuffID.WellFed2) || guardian.HasBuff(Terraria.ID.BuffID.WellFed3))
                {
                    Mes.Add("*[name] thanks you for the food.*");
                    Mes.Add("*[name] seems to be relaxing after eating something.*");
                }
                else
                {
                    Mes.Add("*You can hear [name]'s stomach growl.*");
                    Mes.Add("*[name] seems to be a bit hungry.*");
                }
            }
            if (Main.dayTime)
            {
                if (!Main.eclipse)
                {
                    Mes.Add("*[name] asks you what's up.*");
                    Mes.Add("*[name] is telling that he is liking the weather.*");
                }
                else
                {
                    Mes.Add("*[name] seems to be watching some classic horror movie on the tv... No, wait, that's a window.*");
                    Mes.Add("*[name] is trying to hide behind you, he seems scared of the monsters.*");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    if (MerchantInTheWorld)
                        Mes.Add("*As soon as [name] started talking, you hastily asked him to stop, because of the bad trash breath that comes from his mouth.*");
                    Mes.Add("*[name] is sleeping while awake.*");
                    Mes.Add("*[name] is trying hard to keep It's eyes opened.*");
                    Mes.Add("*[name] seems sleepy.*");
                }
                else
                {
                    Mes.Add("*[name] looks scared.*");
                    Mes.Add("*[name] is trembling in terror..*");
                    Mes.Add("*[name] asks if his house is secure.*");
                }
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("*[name] seems to be enjoying the party.*");
            }
            if (SteampunkerInTheWorld)
                Mes.Add("*[name] is talking about a jetpack joyride?*");
            if (NPC.AnyNPCs(NPCID.Golfer))
                Mes.Add("*[name] told you that got the highest score at his last golf match.*");
            if (CanTalkAboutCompanion(1))
            {
                Mes.Add("*[name] seems to be crying, and with a purple left eye, I guess his dialogue with [gn:1] went wrong.*");
                Mes.Add("*[name] seems to be crying, and with his right cheek having a huge red paw marking, I wonder what he was talking about with [gn:1].*");
            }
            if (CanTalkAboutCompanion(3))
            {
                Mes.Add("*[name] seems to have gotten kicked in his behind. Maybe he annoyed [gn:3]?*");
            }
            Player player = Main.LocalPlayer;
            if (HasCompanionSummoned(2) && HasCompanionSummoned(1))
            {
                Mes.Add("*[gn:2] is telling [name] that he's lucky that [gn:1] doesn't play her terrible games with him. But [name] insists that he wanted to play.*");
            }
            if (HasCompanionSummoned(1))
            {
                Mes.Add("*[name] asked [gn:1] why she doesn't play with him, she told him that she can't even bear seeing him.*");
            }
            if (HasCompanionSummoned(3) && HasCompanionSummoned(1))
            {
                Mes.Add("*[name] asked [gn:3] why he doesn't play with him, and he told him that It's because he makes [gn:1] upset.*");
            }
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("*[name] says that he loves playing with [gn:5], but wonders why he always finds him during hide and seek.*");
                Mes.Add("*[name] says that finding [gn:5] made the town very lively.*");
            }
            if (CanTalkAboutCompanion(8))
            {
                Mes.Add("*[name] said that [gn:8] looks familiar, have they met before?*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*[name] hugs you. It feels a bit weird. He's never hugged you without a reason before.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*[name] says that sometimes he feels weird when [gn:" + CompanionDB.Fluffles + "] stares at him for too long.*");
                Mes.Add("*[name] is asking you if you know why [gn:" + CompanionDB.Fluffles + "] looks at him, with her paw on her chin.*");
                if (CanTalkAboutCompanion(CompanionDB.Alex))
                {
                    Mes.Add("*[name] says that playing with [gn:"+CompanionDB.Alex+"] and [gn:"+CompanionDB.Fluffles+"] has been one of the most enjoyable things he has done, and asks you to join too.*");
                }
            }
            if (CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                Mes.Add("*[name] is telling you that [gn:" + CompanionDB.Glenn + "] is his newest friend.*");
                Mes.Add("*[name] says that loves playing with [gn:" + CompanionDB.Glenn + "].*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*[name] says that after meeting [gn:" + CompanionDB.Cinnamon + "], he has been eating several tasty foods.*");
                Mes.Add("*[name] asks what is wrong with the seasonings he brings to [gn:" + CompanionDB.Cinnamon + "].*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Luna))
            {
                Mes.Add("*[name] is really happy for having [gn:"+CompanionDB.Luna+"] around. He really seems to like her.*");
                Mes.Add("*[name] seems to be expecting [gn:"+CompanionDB.Luna+"] to visit him soon.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Celeste))
            {
                Mes.Add("*[name] wishes "+MainMod.TgGodName+" blessings upon you.*");
                Mes.Add("*[name] seems to like having [gn:" + CompanionDB.Celeste + "] around.*");
            }
            if (guardian.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*[name] is telling me to plug my nose.*");
                Mes.Add("*[name] is asking if there is no other moment to chat.*");
            }
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*[name] seems about scared of the ghost on your shoulders.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion guardian)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*[name] showed you a rare insect he found, he seems very happy about that.*");
            Mes.Add("*[name] is asking you when another party will happen.*");
            Mes.Add("*[name] seems to want a new toy, but what could I give him?*");
            Mes.Add("*[name] wants to explore the dungeon sometime.*");
            Player player = MainMod.GetLocalPlayer;
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("*[name] is asking me if [nn:" + Terraria.ID.NPCID.Merchant + "] has put his trash can outside.*");
            if (!HasCompanionSummoned(0))
                Mes.Add("*[name] seems to want to go on an adventure with you.*");
            if (HasCompanionSummoned(0))
            {
                Mes.Add("*[name] is enjoying travelling with me.*");
                Mes.Add("*[name] seems to like killing insects with gasoline, I wonder where he acquired that.*");
                if (guardian.wet || guardian.HasBuff(Terraria.ID.BuffID.Wet))
                    Mes.Add("*[name] is soaked and cold.*");
            }
            if (HasCompanionSummoned(1))
                Mes.Add("*[name] looks surprised at [gn:1], and suddenly forgets what he was going to talk about.*");
            if (HasCompanionSummoned(2))
                Mes.Add("*[name] is asking if you could let him play with [gn:2].*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*[name] asks what you want to talk about.*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*[name] asks if there is something else you want to talk about.*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*[name] seems happy to have talked with you.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*[name] says that he doesn't need anything right now..*";
                    return "*[name] told me that he wants nothing right now.*";
                case RequestContext.HasRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*[name] is asking me to [objective] for him.*";
                    return "*[name] is looking at me with a funny face while telling me that he wants you to [objective], like as If he didn't want to ask for help.*";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*[name] was so happy that started laughing out loud.*";
                    return "*[name] is so impressed that you did what he asked, that even gave you a hug.*";
                case RequestContext.Failed:
                    return "*[name] looks at you with a sad face.*";
                case RequestContext.Accepted:
                    return "*[name] smiles to you.*";
                case RequestContext.Rejected:
                    return "*[name] seems sad.*";;
                case RequestContext.TooManyRequests:
                    return "*[name] is worried because you have too many requests.*";
                case RequestContext.PostponeRequest:
                    return "*[name] waves you goodbye.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*[name] awaits anxiously for you to tell him the request is completed.*";
                case RequestContext.RemindObjective:
                    return "*[name] reminds you that you have to [objective].*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*[name] scratches his head, then asks if you really don't want to do his request anymore.*";
                case RequestContext.CancelRequestYes:
                    return "*[name] shows you a sad face, and then say that It's fine.*";
                case RequestContext.CancelRequestNo:
                    return "*[name] shows a little smile.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*[name] seems happy that you asked, and agrees to follow you.*";
                case JoinMessageContext.FullParty:
                    return "*[name] says that he is worried about the number of people in your group. He sees no way of fitting in it.*";
                case JoinMessageContext.Fail:
                    return "*[name] doesn't feel okay with joining your group right now.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.Success:
                    return "*[name] gives you a farewell, and tells you that he had fun on the adventure.*";
                case LeaveMessageContext.AskIfSure:
                    return "*[name] seems worried about leaving the group outside of a safe place.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*[name] tells you to be careful on your travels, and that he will see you back at home.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*[name] seems relieved when you changed your mind.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*[name] happily allows. He then picked you up and put you on his shoulder.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*[name] is happy that you're willing to carry him.*";
                case MountCompanionContext.Fail:
                    return "*[name] doesn't think this is a good moment for that.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*[name] refused. Maybe he doesn't entirely trust you.*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*[name] says that will carry [target] for you.*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*[name] asks you who should he carry.*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*[name] nodded, and then placed you on the ground.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*[name] said yes, and then got off your shoulder.*";
                case DismountCompanionContext.Fail:
                    return "*[name] doesn't think this is a good moment for that.*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*[name] happily accepted to live here with you.*";
                case MoveInContext.Fail:
                    return "*[name] is saddened to tell you that he can't.*";
                case MoveInContext.NotFriendsEnough:
                    return "*[name] doesn't fully trust you to stay here in this world.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*[name] begins crying as he packs his things to leave.*";
                case MoveOutContext.Fail:
                    return "*[name] tells you that now he can't leave.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*[name] told you that he won't be moving out.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if(Share)
                return "*[name] says that he will gladly do that, and wishes you tell him stories meanwhile.*";
            else
                return "*[name] tells you that it's fine, and asks if you will still tell him stories.*";
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*[name] told you that he will share his bed with you.*";
            return "*[name] said that it's fine.*";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*[name] asks what should he do in combat.*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*[name] says that he will not let anything get close to you.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*[name] told you that he will try keeping his distance from monsters.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*[name] tells you that he will take on the monsters by distance.*";
                case TacticsChangeContext.Nevermind:
                    return "*[name] asks if you want to talk about something else.*";
                case TacticsChangeContext.FollowAhead:
                    return "*[name] acknowledges, and says that will be following you from ahead.*";
                case TacticsChangeContext.FollowBehind:
                    return "*[name] tells you that will be following behind.*";
                case TacticsChangeContext.AvoidCombat:
                    return "*[name] doesn't seems to like the idea, but says he's okay with that.*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*[name] tells you that will help on the defense too.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("(He must be dreaming about playing with someone.)");
                        Mes.Add("(You got startled when he looked at your direction and smiled.)");
                        Mes.Add("(He seems to be sleeping fine.)");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case SleepingMessageContext.OnWokeUp:
                    {
                        switch (Main.rand.Next(3))
                        {
                            default:
                                return "*[name] woke up, and smiled upon seeing you.*";
                            case 1:
                                return "*[name] looks at you after waking up, and asks what do you need.*";
                            case 2:
                                return "*[name] seems quite tired, but stood up to see what you want.*";
                        }
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    {
                        switch (Main.rand.Next(2))
                        {
                            default:
                                return "*[name] asks you if you did his request.*";
                            case 1:
                                return "*[name] smiles at you, and asks if you did his request.*";
                        }
                    }
            }
            return base.SleepingMessage(companion, context);
        }
        
        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*[name] accepts and Bond-Merged your bodies together.*";
                case ControlContext.SuccessReleaseControl:
                    return "*[name] released the Bond-Merge with your bodies.*";
                case ControlContext.FailTakeControl:
                    return "*[name] apologizes, saying that can't Bond-Merge right now.*";
                case ControlContext.FailReleaseControl:
                    return "*[name] tells you that can't undo the Bond-Merge at the moment.*";
                case ControlContext.NotFriendsEnough:
                    return "*[name] tells you no.*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*[name] seems happy that you're talking to him.*";
                        case 1:
                            return "*[name] asks if you need something from him.*";
                        case 2:
                            return "*[name] is cheering for you.*";
                    }
                case ControlContext.GiveCompanionControl:
                    return "*[name] says that will take the control then, and that you should let him know when to return control.*";
                case ControlContext.TakeCompanionControl:
                    return "*[name] gives back control to you.*";
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
                    return "*[name] says that he entrusts his life to you.*";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*[name] says that he can let you ride on his shoulder If your feet are tired.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*[name] seems really excited to tell you something. He said that you've been a great friend to him, and that wanted to retribute that by telling you that if you pick him as your Buddy, he would say yes right away.*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*[name] says that he's very happy with being picked as a Buddy, and let you know that since you two are Buddies, you can ask him to do anything and he will do it since you two are way over simple friendship.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*[name] is curious to know what you need his help for.*";
                case InteractionMessageContext.Accepts:
                    return "*[name] says that he will happily do that.*";
                case InteractionMessageContext.Rejects:
                    return "*[name] tells you that he can't do that.*";
                case InteractionMessageContext.Nevermind:
                    return "*[name] questions himself why you asked at first.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*[name] acknowledges.*";
                case ChangeLeaderContext.Failed:
                    return "*[name] refuses.*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*[name] seems surprised when you asked him to be your Buddy. He then asked if you're sure of that.*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*[name] is surprised that was picked as a buddy. He seems so happy that he is even crying.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*[name] looks disappointed, and says that it's fine.*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*[name] apologizes, and says that you're not his friend enough for that.*";
                case BuddiesModeContext.Failed:
                    return "*[name] rejected that for now.*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*[name] reminds you that you've picked a Buddy already.*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*[name] says that he will happily visit you. And that you should expect him to show up soon.*";
                case InviteContext.SuccessNotInTime:
                    return "*[name] says that he can visit you, but only tomorrow.*";
                case InviteContext.Failed:
                    return "*[name] says that he can't right now.*";
                case InviteContext.CancelInvite:
                    return "*[name] seems sad that you cancelled his visit.*";
                case InviteContext.ArrivalMessage:
                    return "*[name] tells you he arrived.*";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "*[name] is puzzled by what just happened.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*[name] stares at you and the big bear, without understanding what is happening.*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*You heard someone saying that you'll be fine.*";
                case ReviveContext.RevivingMessage:
                    {
                        bool IsPlayer = !(target is Companion);
                        List<string> Mes = new List<string>();
                        if (IsPlayer && target == companion.Owner)
                        {
                            Mes.Add("*" + companion.GetName + " is telling you to hold on.*");
                            Mes.Add("*" + companion.GetName + " tells you to not try moving.*");
                            Mes.Add("*" + companion.GetName + " tells you to rest while he heals the wounds.*");
                            Mes.Add("*" + companion.GetName + " told you to rest a bit.*");
                            Mes.Add("*" + companion.GetName + " is very focused into helping you.*");
                            Mes.Add("*" + companion.GetName + " holds you tight.*");
                        }
                        else
                        {
                            Mes.Add("*" + companion.GetName + " is trying to calm down the knocked out person.*");
                            Mes.Add("*" + companion.GetName + " is tending the wounds.*");
                            Mes.Add("*" + companion.GetName + " is attempting to stop the bleeding.*");
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*[name] tells the fallen ally they're coming.*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*[name] told the fallen ally that he will keep them safe.*";
                case ReviveContext.RevivedByItself:
                    return "*[name] says that he's fine.*";
                case ReviveContext.ReviveWithOthersHelp:
                    if(Main.rand.NextFloat() <= 0.5f)
                    {
                        return "*[name] thanks you all.*";
                    }
                    return "*[name] seems happy for the help.*";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
