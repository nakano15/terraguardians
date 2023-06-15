using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class NemesisDialogues : CompanionDialogueContainer
    {
        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "You and I are tied together, now.";
                case 1:
                    return "You look strong. I will be your shadow.";
                case 2:
                    return "Who I am doesn't matter. What matters is that now I'm yours.";
            }
            return base.GreetMessages(companion);
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            if (!Main.bloodMoon)
            {
                Mes.Add("I have no emotion, no memory, neither a physical body.");
            }
            else
            {
                Mes.Add("This night looks perfect for some random violence.");
                Mes.Add("Don't worry about me, I don't feel fear, or any other emotion.");
            }
            if (Main.eclipse)
            {
                Mes.Add("Those creatures seems to have spawned from a flat screen.");
            }
            Mes.Add("I can wear any outfit you give me.");
            Mes.Add("I don't have any goal, so I set my own goal to be of helping you on your quest.");
            Mes.Add("I have become so numb.");
            Mes.Add("Should I worry that you could make me look ridiculous?");
            Mes.Add("I am now your shadow, whenever you need, I will go with you, even if it means my demise.");
            Mes.Add("I were in this world for too long, I have seen several things that happened here.");
            if (Main.dayTime && !Main.eclipse)
                Mes.Add("The clothings I have are like my body parts, since they are visible all time. But that doesn't seems to help making the other citizens feel less alarmed about my presence.");
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide))
            {
                Mes.Add("[nn:" + Terraria.ID.NPCID.Guide + "] doesn't appreciate the fact that I know more things than him.");
            }
            if (!Main.dayTime && !Main.bloodMoon)
            {
                Mes.Add("It's night, I don't feel sleepy.");
                Mes.Add("Doesn't matter if It's day or night, I still am partially invisible anytime.");
            }
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("Everyone seems to be expressing themselves on this party. Me? I'll just stay drinking.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Clothier))
                Mes.Add("You have emotions, right? What should I be feeling after hearing that [nn:" + Terraria.ID.NPCID.Clothier + "] is a lady killer?");
            if (CanTalkAboutCompanion(1))
                Mes.Add("[gn:1] keeps forgetting to look where she sits.");
            if (CanTalkAboutCompanion(2))
                Mes.Add("I told [gn:2] that I don't feel anything about drinking, after he asked me about going out for a drink sometime.");
            if (CanTalkAboutCompanion(3))
                Mes.Add("Everyone seems uncomfortable about having [gn:3] and me around. I don't know where is the problem.");
            if (CanTalkAboutCompanion(0))
                Mes.Add("[gn:0] always runs away when he sees me. Did I do something to him?");
            if (CanTalkAboutCompanion(5))
            {
                Mes.Add("Before you ask: No, I'm not " + AlexRecruitmentScript.AlexOldPartner + ", but I once saw a cheerful woman playing with him during my night stalking, a long time ago.");
                Mes.Add("I don't know what it feels by tossing a ball to make [gn:5] fetch it. I just do it because he askes me to.");
            }
            if (CanTalkAboutCompanion(2) && CanTalkAboutCompanion(7))
                Mes.Add("I don't know what is love, or what it is to feel love, but I think [gn:2] and [gn:7] have a very divergent and disturbed relationship.");
            if (CanTalkAboutCompanion(8))
            {
                Mes.Add("I always win the stare contest, because [gn:8] ends up laughing after a few minutes staring my face. I don't know why.");
                Mes.Add("I think [gn:8] is super effective on the town, since she atracts attention of almost everyone in the town. Me? I don't care. \"Sips coffee\"");
            }
            if (CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("What? [gn:" + CompanionDB.Fluffles + "] wasn't doing a stare contest?");
                Mes.Add("No. [gn:" + CompanionDB.Fluffles + "] and I are different. Her soul wasn't devoured by a vile creature.");
            }
            if (CanTalkAboutCompanion(CompanionDB.Celeste))
            {
                Mes.Add("*I wonder if [gn:" + CompanionDB.Celeste + "] could clear the voidness of my soul.*");
            }
            /*if (guardian.IsPlayerRoomMate(player))
            {
                Mes.Add("I don't mind sharing the room with you. There's enough space.");
                Mes.Add("If you get a bed for yourself, you are free to stay as long as you want.");
            }*/
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            Mes.Add("Why does people here look at me like as if I would kill them in their sleep?");
            Mes.Add("I see all this colorful environment, but can't feel anything.");
            if (HasCompanionSummoned(4))
            {
                Mes.Add("Take me with you on your quest, sometime.");
            }
            if (Main.raining)
                Mes.Add("The rain passes through my body, but the armor still can take the drops.");
            Mes.Add("The dungeon in this world? It is a place where cultists sacrificed people to awaken some ancient god. A Terrarian has defeated that ancient god, but parts of it remains in this world.");
            if (HasCompanionSummoned(0))
            {
                Mes.Add("I don't know what it is to feel fun, [gn:0]. So stop doing jokes.");
                Mes.Add("I were wanting to talk to you, [gn:0]. Why do you take people trash with you?");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "The only thing I want to do is follow you.";
                    return "I don't need anything right now.";
                case RequestContext.HasRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "I want you to [objective] for me. Doesn't matter why.";
                    return "I have got a little task for you, If you don't mind. I need you to [objective]. Do it and I'll give you something in exchange.";
                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "Good. What? Were expecting something else, I can't really express anything for what you did.";
                    return "I can't cheer for you doing what I asked you to do.";
                case RequestContext.Accepted:
                    return "Good.";
                case RequestContext.TooManyRequests:
                    return "No. You have too many requests to do.";
                case RequestContext.Rejected:
                    return "Fine. Then don't do It.";
                case RequestContext.PostponeRequest:
                    return "It will be waiting here, then.";
                case RequestContext.Failed:
                    return "Failure was not an option.";
                case RequestContext.AskIfRequestIsCompleted:
                    return "You completed my request?";
                case RequestContext.RemindObjective:
                    return "[objective]. There, now do it.";
                case RequestContext.CancelRequestAskIfSure:
                    return "So, you don't want to do what I asked for anymore?";
                case RequestContext.CancelRequestYes:
                    return "Okay. You no longer need to do It.";
                case RequestContext.CancelRequestNo:
                    return "Then It was just a mistake of what to say.";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "If you say so.";
                case MoveInContext.Fail:
                case MoveInContext.NotFriendsEnough:
                    return "No.";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "Fine. Just call me if you need me.";
                case MoveOutContext.Fail:
                    return "Not this time.";
                case MoveOutContext.NoAuthorityTo:
                    return "Who do you think you are?";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "Yes, sure.";
                case JoinMessageContext.FullParty:
                    return "No. Too many people.";
                case JoinMessageContext.Fail:
                    return "Not now.";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "This doesn't seems like a good place to leave me.";
                case LeaveMessageContext.Success:
                    return "I'll stay then.";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "I know the way home.";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Alright.";
                case LeaveMessageContext.Fail:
                    return "No. I'll stay with you for longer.";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "I doubt I have anything useful you might want to know.";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "Anything else you want to know?";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "Fine.";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            return "Fine.";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "Which of your flaws I'll cover?";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "I'll take on the monsters then.";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "I'll assist you in combat then.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "I'll provide ranged attacks then.";
                case TacticsChangeContext.Nevermind:
                    return "Okay.";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "Say it.";
                case InteractionMessageContext.Accepts:
                    return "As you say.";
                case InteractionMessageContext.Rejects:
                    return "No.";
                case InteractionMessageContext.Nevermind:
                    return "Whatever.";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.Failed:
                    return "I'm not a TerraGuardian, so no, I can't be your buddy.";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "You have one already.";
            }
            return base.BuddiesModeMessage(companion, context);
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "Alright. Coming.";
                case InviteContext.SuccessNotInTime:
                    return "Not this time. But I'll be there by evening.";
                case InviteContext.Failed:
                    return "No.";
                case InviteContext.CancelInvite:
                    return "You don't need me anymore? Fine.";
                case InviteContext.ArrivalMessage:
                    return "I'm here.";
            }
            return "";
        }
    }
}