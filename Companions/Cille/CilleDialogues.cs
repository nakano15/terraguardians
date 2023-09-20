using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Cille
{   
    public class CilleDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "Don't come closer. Leave me alone.";
                case 1:
                    return "Stay away from me, please.";
                case 2:
                    return "(She look scared,should i approach her?)";
            }
            return base.GreetMessages(companion);
        }

        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            CilleCompanion guardian = (CilleCompanion)companion;
            bool InBeastState = guardian.InBeastState;
            List<string> Mes = new List<string>();
            if (InBeastState)
            {
                if (guardian.IsPlayerBuddy(player))
                {
                    Mes.Add("*Grrr.... Rrrr....*");
                    Mes.Add("*Rrrr.... Purrr... Rrr...*");
                }
                Mes.Add("*Grr!! Rawr!!*");
                Mes.Add("*Arrgh!! Grrr!! Rrr!!*");
            }
            else if (!guardian.IsStarter && guardian.FriendshipLevel < 3)
            {
                Mes.Add("*Leave me alone... Please...*");
                if (guardian.FriendshipLevel > 0)
                    Mes.Add("*You returned..*");
                Mes.Add("*Don't stay around me. It's for your own safety.*");
                Mes.Add("*Please, stay away from me..*");
                Mes.Add("*I hurt someone in the past... I don't know how.. I couldn't stop either.. Please... Leave me alone..*");
                Mes.Add("*I'm nobody.. Go away, please..*");
                if (!Main.dayTime)
                {
                    Mes.Add("*You... Avoid the dark around me..*");
                    Mes.Add("*For your safety, please don't stay near me..*");
                }
            }
            else
            {
                if (guardian.IsUsingToilet)
                {
                    Mes.Add("*Is it really necessary of you to speak with me right now?*");
                    Mes.Add("*You're distracting me right now. Please, go away.*");
                    Mes.Add("*I can talk to you any other time than this.*");
                }
                else
                {
                    Mes.Add("*I wonder how are things back home...*");
                    Mes.Add("*You always came back to see me... Thank you..*");
                    Mes.Add("*You came to see me?*");
                    Mes.Add("*It's not that I don't like people... It's just... Safer for us if we don't stay close.*");
                    Mes.Add("*...*");
                    if (Main.dayTime)
                    {
                        Mes.Add("*Are you finding it weird that I only eat fruits and bugs? Sorry, I'm avoiding touching meat.*");
                        if (!Main.raining)
                        {
                            Mes.Add("*It seems like a good day for running.*");
                            Mes.Add("*Do you want to race against me, [nickname]?*");
                        }
                        else
                            Mes.Add("Aww... It's raining..");
                        Mes.Add("*I think I can talk, while it's still daytime.*");
                    }
                    else
                    {
                        Mes.Add("*Feeling tired..? Go home get some sleep.*");
                        Mes.Add("*I can't forget that night... What night? Forget that I said that.*");
                    }
                    if (guardian.IsPlayerRoomMate(player))
                    {
                        Mes.Add("*I don't like the idea of sharing a room with you. What if I...*");
                    }
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Have you ever hurt someone dear for you? I did when I was younger. I'm avoiding people because of that..*");
            Mes.Add("*I used to have everything when I was younger. Due to not controlling myself, I hurt someone, and now am a run away, wandering through worlds..*");
            Mes.Add("*There was one person who used to visit me, before I moved to this place. I wonder if they miss me..*");
            Mes.Add("*I wonder how are things back home..*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            List<string> Mes = new List<string>();
            Mes.Add("(She seems to be having a nightmare.)");
            Mes.Add("(She seems to be telling someone to stop. Did she say her own name?)");
            Mes.Add("(You see tears falling from her eyes.)");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*No. Go away.*";
                    return "*I don't. Please leave.*";
                case RequestContext.HasRequest:
                    if (companion.FriendshipLevel < 3)
                    {
                        if (Main.rand.NextDouble() < 0.5)
                            return "*If you help me, will you go away? Then please [objective].*";
                        return "*Fine... If you say so.. [objective] is what I need. Can you do that?*";
                    }
                    else
                    {
                        if (Main.rand.NextDouble() < 0.5)
                            return "*I'm glad you asked. I need you to [objective]. Can you?*";
                        return "*There is something I need done.. [objective] is it. Can you help me with it?*";
                    }
                case RequestContext.Completed:
                    {
                        if (companion.FriendshipLevel < 3)
                            return "*...Thanks..*";
                        if (Main.rand.Next(2) == 0)
                            return "*Thank you so much..*";
                        return "*I'm happy that you helped me..*";
                    }
                case RequestContext.Accepted:
                    return "*You know where to find me.*";
                case RequestContext.TooManyRequests:
                    return "*Don't you have too many things on your hands?*";
                case RequestContext.Rejected:
                    return "*I can do that later, then.*";
                case RequestContext.PostponeRequest:
                    return "*Have more important things to do?*";
                case RequestContext.Failed:
                    return "*You what? Please... Go away..*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Did you do my request?*";
                case RequestContext.RemindObjective:
                    return "*You forgot what I asked for? It was just to [objective].*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You want to cancel your request?*";
                case RequestContext.CancelRequestYes:
                    return "*Why did you accepted it in first place? Sigh... Fine, you no longer need to do it.*";
                case RequestContext.CancelRequestNo:
                    return "*You surprised me.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*I can join you, just please run away if I begin acting strange.*";
                case JoinMessageContext.Fail:
                    return "*There's too many people...*";
                case JoinMessageContext.FullParty:
                    return "*No... Leave me here.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*Couldn't you take me close to my home, first?*";
                case LeaveMessageContext.Success:
                    return "*I'll be back home, then..*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I guess I'll have to find my way home, then...*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Thank you.*";
                case LeaveMessageContext.Fail:
                    return "*Nope.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*Wow...you give me this place?*";
                case MoveInContext.Fail:
                    return "*I feel uncomfortable around here.*";
                case MoveInContext.NotFriendsEnough:
                    return "*Leave me alone.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*You kick me out?*";
                case MoveOutContext.Fail:
                    return "*No, i will not leave.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*Get out!*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Do you want to talk to me?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there anything else you want to talk about?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Ok.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "*You'll let me sleep on your bed? Are you sure about that?*";
            return "*Wise choice.*";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*My... Combat behavior...?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*Fine... I will do that...*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*I'll keep some distance, then...*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*Keep away from foes..? Right.*";
                case TacticsChangeContext.Nevermind:
                    return "*...*";
                case TacticsChangeContext.FollowAhead:
                    return "*Sure...*";
                case TacticsChangeContext.FollowBehind:
                    return "*I'll do.*";
                case TacticsChangeContext.AvoidCombat:
                    return "*I'll avoid combat then..*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*I will fight again...*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*You need my help..?*";
                case InteractionMessageContext.Accepts:
                    return "*Sounds easy..*";
                case InteractionMessageContext.Rejects:
                    return "*No.*";
                case InteractionMessageContext.Nevermind:
                    return "*...Then why..?*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*Sure..*";
                case ChangeLeaderContext.Failed:
                    return "*I refuse.*";
            }
            return "";
        }

        

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*If you say so, I will be there soon..*";
                case InviteContext.SuccessNotInTime:
                    return "*Now is not a good time, but tomorrow I will be there.*";
                case InviteContext.Failed:
                    return "*No.. Leave me alone...*";
                case InviteContext.CancelInvite:
                    return "*I think it's better this way...*";
                case InviteContext.ArrivalMessage:
                    return "*I'm here [nickname]. Please come quickly talk to me before I go.*";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "*I instinctivelly nearly went to catch him...*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*That looks soooo cutee.*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*That voice... Asks for help.*";
                case ReviveContext.RevivingMessage:
                    {
                        if(companion.FriendshipLevel >= 3)
                        {
                            if (Main.rand.Next(2) == 0)
                                return "*Yes.. Continue breathing..*";
                            return "*I'll help you...*";
                        }
                        return "*...*";
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*I'm coming.*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Good. You're still alive.*";
                case ReviveContext.RevivedByItself:
                    return "*Uh? What happened?*";
                case ReviveContext.ReviveWithOthersHelp:
                    return "*Thank you for caring of me.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch (context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*..You want to pick me as your Buddy..? Out of everyone? .. I don't know if it's a good idea.. Are you sure? You can't change buddies if you say 'Yes'.*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*..I don't know why you picked me, but.. Thank you...*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*... Go away...*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*...*";
                case BuddiesModeContext.Failed:
                    return "*No..*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*I can see your bonding rope. You already have someone as buddy.*";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "**";
                case ControlContext.FailTakeControl:
                    return "**";
                case ControlContext.SuccessReleaseControl:
                    return "**";
                case ControlContext.FailReleaseControl:
                    return "**";
                case ControlContext.NotFriendsEnough:
                    return "**";
                case ControlContext.ControlChatter:
                    return "**";
                case ControlContext.GiveCompanionControl:
                    return "**";
                case ControlContext.TakeCompanionControl:
                    return "**";
            }
            return base.ControlMessage(companion, context);
        }
    }
}
