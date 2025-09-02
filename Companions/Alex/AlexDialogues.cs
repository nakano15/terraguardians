using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class AlexDialogues : CompanionDialogueContainer
    {
        const string IreneKey = "[oldpartner]";
        
        public override string GreetMessages(Companion companion)
        {
            return GetTranslationRandom("greet", 1, 3);
        }

        public override string NormalMessages(Companion companion)
        {
            TerraGuardian guardian = (TerraGuardian)companion;
            List<string> Mes = new List<string>();
            if (guardian.IsSleeping)
            {
                Mes.Clear();
                GetTranslationKeyRange("normal", 53, 55, Mes);
            }
            else if(PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("normal56");
            }
            else
            {
                GetTranslationKeyRange("normal", 1, 6, Mes);
                if(NPC.AnyNPCs(22))
                    Mes.Add("normal7");

                if (Main.bloodMoon)
                {
                    GetTranslationKeyRange("normal", 8, 9, Mes);
                }
                if (Main.eclipse)
                {
                    GetTranslationKeyRange("normal", 10, 11, Mes);
                }
                if (Main.dayTime)
                {
                    if (!Main.eclipse)
                    {
                        if (!Main.raining)
                            Mes.Add("normal12");
                        else
                            Mes.Add("normal13");
                        Mes.Add("normal14");
                    }
                }
                else
                {
                    if (!Main.bloodMoon)
                    {
                        if (Main.moonPhase == 2)
                            Mes.Add("normal15");
                    }
                }
                if (CanTalkAboutCompanion(0))
                {
                    Mes.Add("normal16");
                    if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
                        Mes.Add("normal17");
                }
                if (CanTalkAboutCompanion(1))
                {
                    GetTranslationKeyRange("normal", 18, 19, Mes);
                    if (CanTalkAboutCompanion(2))
                        Mes.Add("normal20");
                }
                if (CanTalkAboutCompanion(2))
                {
                    Mes.Add("normal21");
                }
                if (CanTalkAboutCompanion(3))
                {
                    GetTranslationKeyRange("normal", 22, 23, Mes);
                    if (CanTalkAboutCompanion(1))
                        Mes.Add("normal24");
                }
                if (CanTalkAboutCompanion(4))
                {
                    GetTranslationKeyRange("normal", 25, 26, Mes);
                }
                if (CanTalkAboutCompanion(0) && CanTalkAboutCompanion(2))
                    Mes.Add("normal27");
                if (CanTalkAboutCompanion(7))
                {
                    GetTranslationKeyRange("normal", 28, 29, Mes);
                }
                if (CanTalkAboutCompanion(8))
                {
                    GetTranslationKeyRange("normal", 30, 31, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                {
                    GetTranslationKeyRange("normal", 32, 34, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Michelle))
                {
                    Mes.Add("normal35");
                }
                if (CanTalkAboutCompanion(CompanionDB.Wrath))
                {
                    GetTranslationKeyRange("normal", 36, 37, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                {
                    GetTranslationKeyRange("normal", 38, 39, Mes);
                    if (CanTalkAboutCompanion(CompanionDB.Rococo))
                    {
                        Mes.Add("normal40");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Miguel))
                {
                    GetTranslationKeyRange("normal", 41, 42, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Luna))
                {
                    Mes.Add("normal43");
                }
                if (CanTalkAboutCompanion(CompanionDB.Celeste))
                {
                    GetTranslationKeyRange("normal", 44, 45, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Leona))
                {
                    GetTranslationKeyRange("normal", 46, 48, Mes);
                }
                if (guardian.IsUsingToilet)
                {
                    GetTranslationKeyRange("normal", 49, 50, Mes);
                }
                if (guardian.IsPlayerRoomMate(MainMod.GetLocalPlayer))
                {
                    GetTranslationKeyRange("normal", 51, 52, Mes);
                }
                if (NPC.downedBoss1)
                {
                    Mes.Add("normal57");
                }
                else
                {
                    if (!Main.dayTime)
                    {
                        Mes.Add("normal58");
                    }
                }
                if (NPC.downedBoss3)
                {
                    Mes.Add("normal59");
                }
                if (Main.hardMode)
                {
                    GetTranslationKeyRange("normal", 60, 61, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Scaleforth))
                {
                    Mes.Add("normal62");
                }
                if (CanTalkAboutCompanion(CompanionDB.Ich))
                {
                    GetTranslationKeyRange("normal", 63, 64, Mes);
                }
            }
            return GetTranslation(Mes[Main.rand.Next(Mes.Count)]).Replace(IreneKey, AlexRecruitmentScript.AlexOldPartner);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    return GetTranslationRandom("sleeping", 1, 3);
            }
            return base.SleepingMessage(companion, context);
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
                case RequestContext.Accepted:
                    return GetTranslation("request7");
                case RequestContext.TooManyRequests:
                    return GetTranslation("request8");
                case RequestContext.Rejected:
                    return GetTranslation("request9");
                case RequestContext.PostponeRequest:
                    return GetTranslation("request10");
                case RequestContext.Failed:
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
                case JoinMessageContext.Fail:
                    return GetTranslation("join2");
                case JoinMessageContext.FullParty:
                    return GetTranslation("join3");
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return GetTranslation("leave1");
                case LeaveMessageContext.Success:
                    return GetTranslation("leave2");
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return GetTranslation("leave3");
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return GetTranslation("leave4");
                case LeaveMessageContext.Fail:
                    return GetTranslation("leave5");
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            GetTranslationKeyRange("talk", 1, 3, Mes);
            if (CanTalkAboutCompanion(0))
                Mes.Add("talk4");
            if(!Main.dayTime && Main.moonPhase == 2)
                Mes.Add("talk5");
            return GetTranslation(Mes[Main.rand.Next(Mes.Count)]).Replace(IreneKey, AlexRecruitmentScript.AlexOldPartner);
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
                    return GetTranslation("movein3").Replace(IreneKey, AlexRecruitmentScript.AlexOldPartner);
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

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return GetTranslation("talkother1");
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return GetTranslation("talkother2");
                case TalkAboutOtherTopicsContext.Nevermind:
                    return GetTranslation("talkother3");
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
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
                    return GetTranslation("interact1");
                case InteractionMessageContext.Accepts:
                    return GetTranslation("interact2");
                case InteractionMessageContext.Rejects:
                    return GetTranslation("interact3");
                case InteractionMessageContext.Nevermind:
                    return GetTranslation("interact4");
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

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if (WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Rococo:
                    case CompanionDB.Michelle:
                    case CompanionDB.Bree:
                        Weight = 1.5f;
                        return "*"+WhoJoined.GetNameColored()+" is coming too? Yay!*";
                }
            }
            Weight = 1f;
            return "*Bark Bark* Welcome! Welcome!";
        }

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            Weight = 1f;
            return "*Bark! Bark! *A new friend join us! *Bark! Bark!*";
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
                            return GetTranslationRandom("revive", 2, 4).Replace(IreneKey, AlexRecruitmentScript.AlexOldPartner);
                        }
                        else
                        {
                            return GetTranslationRandom("revive", 5, 7);
                        }
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return GetTranslation("revive8");
                case ReviveContext.ReachedFallenAllyMessage:
                    return GetTranslation("revive9");
                case ReviveContext.RevivedByItself:
                    return GetTranslation("revive10");
                case ReviveContext.ReviveWithOthersHelp:
                    return GetTranslationRandom("revive", 11, 12);
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
