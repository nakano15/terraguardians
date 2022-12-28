using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians
{
    public class CompanionDialogueContainer
    {
        private CompanionBase OwnerCompanion;
        public CompanionBase GetCompanionBase { get { return OwnerCompanion; } }
        internal void SetOwnerCompanion(CompanionBase Base)
        {
            OwnerCompanion = Base;
        }

        public virtual string GreetMessages(Companion companion)
        {
            return "*[name] liked to meet you.*";
        }
        public virtual string NormalMessages(Companion companion)
        {
            return "*[name] stares at you, waiting for you to say something.*";
        }
        public virtual string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*[name] is asking you what else you want to talk about.*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*[name] asks what else you want to talk about.*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*[name] asks if you want to talk about something else.*";
            }
            return "**";
        }
        public virtual string TalkMessages(Companion companion)
        {
            return "*[name] told you something.*";
        }
        public virtual string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return "*[name] seems to have nothing to ask you for.*";
                case RequestContext.HasRequest:
                    return "*[name] wants you to [objective].*";
                case RequestContext.Completed:
                    return "*[name] thanked you deeply.*";
                case RequestContext.Accepted:
                    return "*[name] tells you that he will wait for your return.*";
                case RequestContext.Rejected:
                    return "*[name] is sad that you rejected their request.*";
                case RequestContext.Failed:
                    return "*[name] seems disappointed at you failing the request.*";
                case RequestContext.TooManyRequests:
                    return "*[name] told you that you have too many requests active.*";
                case RequestContext.PostponeRequest:
                    return "*[name] said that you can return later to check their request.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*[name] asked if you completed their request.*";
                case RequestContext.RemindObjective:
                    return "*[name] told you that you need to [objective].*";
            }
            return "**";
        }
        public virtual string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "([name] join your adventure.)";
                case JoinMessageContext.Fail:
                    return "([name] refused.)";
                case JoinMessageContext.FullParty:
                    return "(There is no space for [name] in the group.)";
            }
            return "";
        }
        public virtual string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "([name] left your group.)";
                case LeaveMessageContext.Fail:
                    return "([name] refuses to leave your group.)";
                case LeaveMessageContext.AskIfSure:
                    return "([name] asks if you're sure you want them to leave your group.)";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "([name] stays on your group.)";
            }
            return "";
        }
        public virtual string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*[name] let you mount on their shoulder.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*[name] climbed your back and is mounted on your shoulder.*";
                case MountCompanionContext.Fail:
                    return "*[name] refused.*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*[name] said you're not friends enough for that.*";
            }
            return "";
        }

        public virtual string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch (context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*[name] placed you on the ground.*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*[name] got off your shoulder.*";
                case DismountCompanionContext.Fail:
                    return "*[name] doesn't think it's a good moment for that.*";
            }
            return "";
        }

        public virtual string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*[name] happily accepts moving into your world.*";
                case MoveInContext.Fail:
                    return "*[name] doesn't seems to be wanting to move in right now.*";
                case MoveInContext.NotFriendsEnough:
                    return "*[name] doesn't know you well enough to move in closer.*";
            }
            return "";
        }

        public virtual string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*[name] says that will begin packing its things.*";
                case MoveOutContext.Fail:
                    return "*[name] tells you that is not leaving now.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*[name] tells you that is not going to listen to you.*";
            }
            return "";
        }

        public virtual string OnToggleShareChairMessage(bool Share)
        {
            if(Share) return "*[name] doesn't mind letting you sit on their lap.*";
            return "*[name] tells you that will seek another chair next time.*";
        }

        public virtual string OnToggleShareBedsMessage(bool Share)
        {
            if(Share) return "*[name] doesn't mind sharing their bed with you.*";
            return "*[name] hopes there's another bed for them.*";
        }
    }

    public enum TalkAboutOtherTopicsContext : byte
    {
        FirstTimeInThisDialogue,
        AfterFirstTime,
        Nevermind
    }
    
    public enum JoinMessageContext : byte
    {
        Success,
        Fail,
        FullParty
    }

    public enum LeaveMessageContext : byte
    {
        Success,
        Fail,
        AskIfSure,
        DangerousPlaceYesAnswer,
        DangerousPlaceNoAnswer
    }

    public enum MoveInContext : byte
    {
        Success,
        Fail,
        NotFriendsEnough
    }

    public enum MoveOutContext : byte
    {
        Success,
        Fail,
        NoAuthorityTo
    }

    public enum RequestContext : byte
    {
        NoRequest,
        HasRequest,
        Accepted,
        Rejected,
        Completed,
        Failed,
        TooManyRequests,
        PostponeRequest,
        AskIfRequestIsCompleted,
        RemindObjective
    }
}