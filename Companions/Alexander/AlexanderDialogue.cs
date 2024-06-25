using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class AlexanderDialogue : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            return GetTranslationRandom("greet", 1, 3);
        }

        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Add("normal39");
            }
            else if (companion.IsUsingToilet)
            {
                GetTranslationKeyRange("normal", 1, 3, Mes);
            }
            else
            {
                GetTranslationKeyRange("normal", 4, 11, Mes);
                if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
                {
                    Mes.Add("normal12");
                }
                if (!Main.dayTime && !Main.bloodMoon)
                {
                    GetTranslationKeyRange("normal", 13, 14, Mes);
                }
                else if(Main.dayTime && !Main.eclipse)
                {
                    if (Main.raining)
                    {
                        Mes.Add("normal15");
                    }
                    else
                    {
                        Mes.Add("normal16");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    GetTranslationKeyRange("normal", 17, 18, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    if (!CanTalkAboutCompanion(CompanionDB.Zack))
                    {
                        GetTranslationKeyRange("normal", 19, 21, Mes);
                    }
                    else
                    {
                        GetTranslationKeyRange("normal", 22, 23, Mes);
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    if (!CanTalkAboutCompanion(CompanionDB.Sardine))
                    {
                        Mes.Add("normal24");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Mabel))
                {
                    GetTranslationKeyRange("normal", 25, 26, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Malisha))
                {
                    GetTranslationKeyRange("normal", 27, 28, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Wrath))
                {
                    Mes.Add("normal29");
                }
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                {
                    GetTranslationKeyRange("normal", 30, 31, Mes);
                }
                if (CanTalkAboutCompanion(CompanionDB.Michelle))
                {
                    Mes.Add("normal32");
                }
                if (CanTalkAboutCompanion(CompanionDB.Nemesis))
                {
                    Mes.Add("normal33");
                }
                if (CanTalkAboutCompanion(CompanionDB.Luna))
                {
                    if ((companion as AlexanderBase.AlexanderCompanion).HasAlexanderSleuthedGuardian(CompanionDB.Luna))
                    {
                        Mes.Add("normal34");
                    }
                    else
                    {
                        Mes.Add("normal35");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Green))
                {
                    Mes.Add("normal36");
                }
                if (companion.IsPlayerRoomMate(player))
                {
                    GetTranslationKeyRange("normal", 37, 38, Mes);
                }
            }
            return GetTranslation(Mes[Main.rand.Next(Mes.Count)]);
        }

        public override string TalkMessages(Companion companion)
        {
            return GetTranslationRandom("talk", 1, 3);
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
                case RequestContext.AskIfRequestIsCompleted:
                    return GetTranslation("request11");
                case RequestContext.RemindObjective:
                    return GetTranslation("request12");
                case RequestContext.CancelRequestAskIfSure:
                    return GetTranslation("request13");
                case RequestContext.CancelRequestYes:
                    return GetTranslation("request14");
                case RequestContext.CancelRequestNo:
                    return GetTranslation("request15");

            }
            return base.RequestMessages(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.RevivingMessage:
                    List<string> Mes = new List<string>();
                    if (target == companion.Owner)
                    {
                        GetTranslationKeyRange("revive", 1, 3, Mes);
                    }
                    else
                    {
                        Mes.Add("revive4");
                    }
                    GetTranslationKeyRange("revive", 5, 6, Mes);
                    return GetTranslation(Mes[Main.rand.Next(Mes.Count)]);
                case ReviveContext.HelpCallReceived:
                    return GetTranslation("revive7");
                case ReviveContext.ReachedFallenAllyMessage:
                    return GetTranslation("revive8");
                case ReviveContext.ReviveWithOthersHelp:
                    return GetTranslationRandom("revive", 9, 10);
                case ReviveContext.RevivedByItself:
                    return GetTranslationRandom("revive", 11, 12);
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    return GetTranslationRandom("sleep", 1, 3);
                case SleepingMessageContext.OnWokeUp:
                    return GetTranslation("sleep4");
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return GetTranslation("sleep5");
            }
            return base.SleepingMessage(companion, context);
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
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return GetTranslation("other1");
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return GetTranslation("other2");
                case TalkAboutOtherTopicsContext.Nevermind:
                    return GetTranslation("other3");
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldMessage1:
                    return GetTranslation("other4");
                case MessageIDs.LeopoldMessage2:
                    return GetTranslation("other5");
                case MessageIDs.LeopoldMessage3:
                    return GetTranslation("other6");
                case MessageIDs.LeopoldEscapedMessage:
                    return GetTranslation("other7");
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return GetTranslation("other8");
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}