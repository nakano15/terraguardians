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
            TerraGuardian guardian = (TerraGuardian)companion;
            List<string> Mes = new List<string>();
            Mes.Add("*Have you ever hurt someone dear for you? I did when I was younger. I'm avoiding people because of that..*");
            Mes.Add("*I used to have everything when I was younger. Due to not controlling myself, I hurt someone, and now am a run away, wandering through worlds..*");
            Mes.Add("*There was one person who used to visit me, before I moved to this place. I wonder if they miss me..*");
            Mes.Add("*I wonder how are things back home..*");

            
            

            
            
            if (Main.dayTime)
            {
                Mes.Add("*Are you finding it weird that I only eat fruits and bugs? Sorry, I'm avoiding touching meat.*");
                if (!Main.eclipse)
                {
                    if (!Main.raining)
                        Mes.Add("It seems like a good day for running.");
                    else
                        Mes.Add("Aww... It's raining..");
                        Mes.Add("*I think I can talk, while it's still daytime.*");
                }
            }
            else
            {
                if (!Main.bloodMoon)
                {
                    if (Main.moonPhase == 2)
                        Mes.Add("No...No....i don't want to hurt anyone anymore");
                }
            }
            
            
           
            if (guardian.IsUsingToilet)
            {
                Mes.Add("Is it really necessary of you to speak with me right now?");
                Mes.Add("I can talk to you any other time than this.");
            }
            /*if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("You'll sleep in my bedroom? That's awesome! I will keep you protected while you sleep.");
                Mes.Add("You'll share your bed with me? This is the best day ever!");
            }*/
            if (guardian.IsSleeping)
            {
                Mes.Clear();
                Mes.Add("(She seems to be having a nightmare.)");
                Mes.Add("(She seems to be telling someone to stop. Did she say her own name?)");
                Mes.Add("(You see tears falling from her eyes.)");
            }
            /*if(FlufflesBase.IsHauntedByFluffles(player) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("Who's she? Is she your friend? Can she play with me?");
            }*/
            /*if (FlufflesBase.IsCompanionHaunted(guardian) && Main.rand.Next(2) == 0)
            {
                Mes.Clear();
                Mes.Add("");
            }*/
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
                        return "No. Go away.";
                    return "I don't. Please leave.";
                case RequestContext.HasRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "If you help me, will you go away? Then please [objective].";
                    return "Fine... If you say so.. [objective] is what I need. Can you do that?";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "...Thanks..";
                    return "Thank you so much..";
                case RequestContext.Accepted:
                    return "You know where to find me.";
                case RequestContext.TooManyRequests:
                    return "Don't you have too many things on your hands?";
                case RequestContext.Rejected:
                    return "I can do that later, then.";
                case RequestContext.PostponeRequest:
                    return "Have more important things to do?";
                case RequestContext.Failed:
                    return "You what? Please... Go away..*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "Did you do my request?";
                case RequestContext.RemindObjective:
                    return "You forgot what I asked for? It was just to [objective].";
                case RequestContext.CancelRequestAskIfSure:
                    return "You want to cancel your request?";
                case RequestContext.CancelRequestYes:
                    return "Why did you accepted it in first place? Sigh... Fine, you no longer need to do it.";
                case RequestContext.CancelRequestNo:
                    return "You surprised me.";
            }
            return base.RequestMessages(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "I can join you, just please run away if I begin acting strange.";
                case JoinMessageContext.Fail:
                    return "There's too many people...";
                case JoinMessageContext.FullParty:
                    return "No... Leave me here.";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "Couldn't you take me close to my home, first?";
                case LeaveMessageContext.Success:
                    return "I'll be back home, then..";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "I guess I'll have to find my way home, then...";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Thank you.";
                case LeaveMessageContext.Fail:
                    return "Nope";
            }
            return base.LeaveGroupMessages(companion, context);
        }

      

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "Wow...you give me this place?";
                case MoveInContext.Fail:
                    return "I feel uncomfortable around here";
                case MoveInContext.NotFriendsEnough:
                    return "leave me alone";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "You kick me out?";
                case MoveOutContext.Fail:
                    return "No,i will not leave";
                case MoveOutContext.NoAuthorityTo:
                    return "Get out!";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "Do you want to talk to me?";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "Is there anything else you want to talk about?";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "Ok";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share)
                return "You'll let me sleep on your bed? Are you sure about that?";
            return "Wise choice";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "What is your suggestion? ";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "It is better to unleash my power near them anyway";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "Fair enough";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "You want me to keep a safe distant?";
                case TacticsChangeContext.Nevermind:
                    return "Alright....";
            }
            return base.TacticChangeMessage(companion, context);
        }

        

        

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "What do you need ?";
                case InteractionMessageContext.Accepts:
                    return " Maybe I can do that.";
                case InteractionMessageContext.Rejects:
                    return "Sorry,i can't do that";
                case InteractionMessageContext.Nevermind:
                    return "you don't need anything anymore?";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "Yes, [nickname]!";
                case ChangeLeaderContext.Failed:
                    return "I will not..";
            }
            return "";
        }

        

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "ok...i will come.";
                case InviteContext.SuccessNotInTime:
                    return "I'll be there the next day.";
                case InviteContext.Failed:
                    return "No,leave me alone..";
                case InviteContext.CancelInvite:
                    return "ok....";
                case InviteContext.ArrivalMessage:
                    return "I'm here. Just stay away from me if i acting strange";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "Who is he?";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*She look worried*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "Please don't dieee...";
                case ReviveContext.RevivingMessage:
                    {
                        bool IsPlayer = !(target is Companion);
                        List<string> Mes = new List<string>();
                        if (IsPlayer && target == companion.Owner)
                        {
                            Mes.Add("Yes.. Continue breathing..");
                            Mes.Add("I'll help you...");
                            
                        }
                        else
                        {
                            Mes.Add("I'll help you!");
                            Mes.Add("I can take care of your wounds.");
                            
                        }
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "I'll help you,keep your breath";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "Ok,it is safe now,stay there";
                case ReviveContext.RevivedByItself:
                    return "Uh? What happened?";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextFloat() < 0.5f)
                        return "Thank you, Buddy-Buddy!";
                    return "Thank you for caring of me.";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
