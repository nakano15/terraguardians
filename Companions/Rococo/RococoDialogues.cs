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
            return GetTranslation("greet" + (Main.rand.Next(4) + 1));
            /*switch (Main.rand.Next(4))
            {
                case 0:
                    return "\"At first, the creature got surprised after seeing me, then starts laughing out of happiness.\"";
                case 1:
                    return "\"That creature waves at you while smiling, It must be friendly, I guess?\"";
                case 2:
                    return "\"For some reason, that creature got happy after seeing you, maybe It wasn't expecting another human in this world?\"";
                default:
                    return "\"What sort of creature is that? Is it dangerous? No, It doesn't look like it.\"";
            }*/
        }

        public override string NormalMessages(Companion guardian)
        {
            bool MerchantInTheWorld = NPC.AnyNPCs(NPCID.Merchant), SteampunkerInTheWorld = NPC.AnyNPCs(NPCID.Steampunker);
            List<string> Mes = new List<string>();
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Add("normal48");
            }
            else if (guardian.IsUsingToilet)
            {
                GetTranslationKeyRange("normal", 46, 47, Mes);
            }
            else
            {
                if (!Main.bloodMoon && !Main.eclipse)
                {
                    GetTranslationKeyRange("normal", 1, 5, Mes);
                    if(!guardian.IsFollower)
                        Mes.Add("normal6");
                    if (guardian.HasBuff(Terraria.ID.BuffID.WellFed) || guardian.HasBuff(Terraria.ID.BuffID.WellFed2) || guardian.HasBuff(Terraria.ID.BuffID.WellFed3))
                    {
                        GetTranslationKeyRange("normal", 7, 8, Mes);
                    }
                    else
                    {
                        GetTranslationKeyRange("normal", 9, 10, Mes);
                    }
                }
                if (Main.dayTime)
                {
                    if (!Main.eclipse)
                    {
                        GetTranslationKeyRange("normal", 11, 12, Mes);
                    }
                    else
                    {
                        GetTranslationKeyRange("normal", 13, 14, Mes);
                    }
                }
                else
                {
                    if (!Main.bloodMoon)
                    {
                        if (MerchantInTheWorld)
                            Mes.Add("normal15");
                        GetTranslationKeyRange("normal", 16, 18, Mes);
                    }
                    else
                    {
                        GetTranslationKeyRange("normal", 19, 21, Mes);
                    }
                }
                if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
                {
                    Mes.Add("normal22");
                }
                if (SteampunkerInTheWorld)
                    Mes.Add("normal23");
                if (NPC.AnyNPCs(NPCID.Golfer))
                    Mes.Add("normal24");
                if (CanTalkAboutCompanion(1))
                {
                    GetTranslationKeyRange("normal", 25, 26, Mes);
                }
                if (CanTalkAboutCompanion(3))
                {
                    Mes.Add("normal27");
                }
                Player player = Main.LocalPlayer;
                if (HasCompanionSummoned(2) && HasCompanionSummoned(1))
                {
                    Mes.Add("normal28");
                }
                if (HasCompanionSummoned(1))
                {
                    Mes.Add("normal29");
                }
                if (HasCompanionSummoned(3) && HasCompanionSummoned(1))
                {
                    Mes.Add("normal30");
                }
                if (CanTalkAboutCompanion(5))
                {
                    GetTranslationKeyRange("normal", 31, 32, Mes);
                }
                if (CanTalkAboutCompanion(8))
                {
                    Mes.Add("normal33");
                }
                if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                {
                    Mes.Add("normal34");
                }
                if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                {
                    GetTranslationKeyRange("normal", 35, 36, Mes);
                    if (CanTalkAboutCompanion(CompanionDB.Alex))
                    {
                        Mes.Add("normal37");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Glenn))
                {
                    GetTranslationKeyRange("normal", 38, 39, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
                {
                    GetTranslationKeyRange("normal", 40, 41, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Luna))
                {
                    GetTranslationKeyRange("normal", 42, 43, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Celeste))
                {
                    GetTranslationKeyRange("normal", 44, 45, Mes);
                }
            }
            return GetTranslation(Mes[Main.rand.Next(Mes.Count)]);
        }

        public override string TalkMessages(Companion guardian)
        {
            List<string> Mes = new List<string>();
            GetTranslationKeyRange("talk", 1, 4, Mes);
            Player player = MainMod.GetLocalPlayer;
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                Mes.Add("talk5");
            if (!HasCompanionSummoned(0))
                Mes.Add("talk6");
            else
            {
                GetTranslationKeyRange("talk", 7, 8, Mes);
                if (guardian.wet || guardian.HasBuff(Terraria.ID.BuffID.Wet))
                    Mes.Add("talk9");
            }
            if (HasCompanionSummoned(1))
                Mes.Add("talk10");
            if (HasCompanionSummoned(2))
                Mes.Add("talk11");
            return GetTranslation(Mes[Main.rand.Next(Mes.Count)]);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return GetTranslation("othertopicfirsttime");
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return GetTranslation("othertopicafterfirst");
                case TalkAboutOtherTopicsContext.Nevermind:
                    return GetTranslation("othertopicnevermind");
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return GetTranslationRandom("request", 1, 2);
                case RequestContext.HasRequest:
                    return GetTranslationRandom("request", 3, 4);
                case RequestContext.Completed:
                    return GetTranslationRandom("request", 5, 6);
                case RequestContext.Failed:
                    return GetTranslation("request7");
                case RequestContext.Accepted:
                    return GetTranslation("request8");
                case RequestContext.Rejected:
                    return GetTranslation("request9");
                case RequestContext.TooManyRequests:
                    return GetTranslation("request10");
                case RequestContext.PostponeRequest:
                    return GetTranslation("request11");
                case RequestContext.AskIfRequestIsCompleted:
                    return GetTranslation("request12");
                case RequestContext.RemindObjective:
                    return GetTranslation("request13");
                case RequestContext.CancelRequestAskIfSure:
                    return GetTranslation("request14");
                case RequestContext.CancelRequestYes:
                    return GetTranslation("request15");
                case RequestContext.CancelRequestNo:
                    return GetTranslation("request16");
            }
            return base.RequestMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return GetTranslation("join1");
                case JoinMessageContext.FullParty:
                    return GetTranslation("join2");
                case JoinMessageContext.Fail:
                    return GetTranslation("join3");
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.Success:
                    return GetTranslation("leave1");
                case LeaveMessageContext.AskIfSure:
                    return GetTranslation("leave2");
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return GetTranslation("leave3");
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return GetTranslation("leave4");
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return GetTranslation("mount1");
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return GetTranslation("mount2");
                case MountCompanionContext.Fail:
                    return GetTranslation("mount3");
                case MountCompanionContext.NotFriendsEnough:
                    return GetTranslation("mount4");
                case MountCompanionContext.SuccessCompanionMount:
                    return GetTranslation("mount5");
                case MountCompanionContext.AskWhoToCarryMount:
                    return GetTranslation("mount6");
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return GetTranslation("dismount1");
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return GetTranslation("dismount2");
                case DismountCompanionContext.Fail:
                    return GetTranslation("dismount3");
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return GetTranslation("movein1");
                case MoveInContext.Fail:
                    return GetTranslation("movein2");
                case MoveInContext.NotFriendsEnough:
                    return GetTranslation("movein3");
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return GetTranslation("moveout1");
                case MoveOutContext.Fail:
                    return GetTranslation("moveout2");
                case MoveOutContext.NoAuthorityTo:
                    return GetTranslation("moveout3");
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if(Share)
                return GetTranslation("sharechair1");
            else
                return GetTranslation("sharechair2");
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return GetTranslation("sharebed1");
            return GetTranslation("sharebed2");
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return GetTranslation("tactics1");
                case TacticsChangeContext.ChangeToCloseRange:
                    return GetTranslation("tactics2");
                case TacticsChangeContext.ChangeToMidRanged:
                    return GetTranslation("tactics3");
                case TacticsChangeContext.ChangeToLongRanged:
                    return GetTranslation("tactics4");
                case TacticsChangeContext.Nevermind:
                    return GetTranslation("tactics5");
                case TacticsChangeContext.FollowAhead:
                    return GetTranslation("tactics6");
                case TacticsChangeContext.FollowBehind:
                    return GetTranslation("tactics7");
                case TacticsChangeContext.AvoidCombat:
                    return GetTranslation("tactics8");
                case TacticsChangeContext.PartakeInCombat:
                    return GetTranslation("tactics9");
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    {
                        return GetTranslationRandom("sleep", 1, 3);
                    }
                case SleepingMessageContext.OnWokeUp:
                    {
                        return GetTranslationRandom("sleep", 4, 6);
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    {
                        return GetTranslationRandom("sleep", 7, 8);
                    }
            }
            return base.SleepingMessage(companion, context);
        }
        
        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return GetTranslation("control1");
                case ControlContext.SuccessReleaseControl:
                    return GetTranslation("control2");
                case ControlContext.FailTakeControl:
                    return GetTranslation("control3");
                case ControlContext.FailReleaseControl:
                    return GetTranslation("control4");
                case ControlContext.NotFriendsEnough:
                    return GetTranslation("control5");
                case ControlContext.ControlChatter:
                    return GetTranslationRandom("control", 6, 8);
                case ControlContext.GiveCompanionControl:
                    return GetTranslation("control9");
                case ControlContext.TakeCompanionControl:
                    return GetTranslation("control10");
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
                    return GetTranslation("unlock1");
                case UnlockAlertMessageContext.FollowUnlock:
                    return "";
                case UnlockAlertMessageContext.MountUnlock:
                    return GetTranslation("unlock2");
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return GetTranslation("unlock3");
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return GetTranslation("unlock4");
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return GetTranslation("interaction1");
                case InteractionMessageContext.Accepts:
                    return GetTranslation("interaction2");
                case InteractionMessageContext.Rejects:
                    return GetTranslation("interaction3");
                case InteractionMessageContext.Nevermind:
                    return GetTranslation("interaction4");
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return GetTranslation("changeleader1");
                case ChangeLeaderContext.Failed:
                    return GetTranslation("changeleader2");
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return GetTranslation("buddy1");
                case BuddiesModeContext.PlayerSaysYes:
                    return GetTranslation("buddy2");
                case BuddiesModeContext.PlayerSaysNo:
                    return GetTranslation("buddy3");
                case BuddiesModeContext.NotFriendsEnough:
                    return GetTranslation("buddy4");
                case BuddiesModeContext.Failed:
                    return GetTranslation("buddy5");
                case BuddiesModeContext.AlreadyHasBuddy:
                    return GetTranslation("buddy6");
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return GetTranslation("invite1");
                case InviteContext.SuccessNotInTime:
                    return GetTranslation("invite2");
                case InviteContext.Failed:
                    return GetTranslation("invite3");
                case InviteContext.CancelInvite:
                    return GetTranslation("invite4");
                case InviteContext.ArrivalMessage:
                    return GetTranslation("invite5");
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return GetTranslation("other1");
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return GetTranslation("other2");
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return GetTranslation("revive1");
                case ReviveContext.RevivingMessage:
                    {
                        bool IsPlayer = !(target is Companion);
                        if (IsPlayer && target == companion.Owner)
                        {
                            return GetTranslationRandom("revive", 2, 7);
                        }
                        else
                        {
                            return GetTranslationRandom("revive", 8, 10);
                        }
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return GetTranslation("revive11");
                case ReviveContext.ReachedFallenAllyMessage:
                    return GetTranslation("revive12");
                case ReviveContext.RevivedByItself:
                    return GetTranslation("revive13");
                case ReviveContext.ReviveWithOthersHelp:
                    return GetTranslationRandom("revive", 14, 15);
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
