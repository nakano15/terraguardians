using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class CelesteDialogues : CompanionDialogueContainer
    {
        string TgGodName {get{ return MainMod.TgGodName; }}

        public override string GreetMessages(Companion companion)
        {
            switch(Main.rand.Next(2))
            {
                default:
                    return "*Hello, Terrarian. I am "+companion.GetNameColored()+", priestess of "+MainMod.TgGodName+".*";
                case 1:
                    return "*Hi. I am "+companion.GetNameColored()+", and I'm here to spread "+MainMod.TgGodName+"+'s blessing through this realm.*";
            }
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Since you've been taking good care of the TerraGuardians living here, "+TgGodName+"'s blessing may fall onto you too.*");
            Mes.Add("*Upon noticing TerraGuardians appearing on this realm, I felt that it was my mission to bring "+TgGodName+"'s influence to this realm.*");
            Mes.Add("*Blessings of "+TgGodName+" upon you, [nickname].*");

            Mes.Add("*This seems actually like a nice place to live. No wonder other TerraGuardians came here.*");
            Mes.Add("*You built all those houses by yourself? I'm actually impressed at your dedication.*");
            Mes.Add("*You know, since the opening of the portals on the Ether Realm, TerraGuardians have been travelling through different worlds.*");

            if (WorldMod.GetTerraGuardiansCount >= 11)
            {
                Mes.Add("*Impressive how many TerraGuardians came to this world. I'm starting to worry about the Ether Realm.*");
                Mes.Add("*It has been quite a looong time since TerraGuardian last were living in the Terra Realm. It feels like story repeats itself, and hopefully, with no calamity this time.*");
                Mes.Add("*With the amount of TerraGuardians in this world, I don't feel offset anymore. "+TgGodName+" would love seeing this.*");
            }
            if (WorldMod.GetTerraGuardiansCount >= 6)
            {
                Mes.Add("*You've met quite a number of TerraGuardians, [nickname].*");
                Mes.Add("*I'm glad that I came here. I can spread "+TgGodName+"'s blessing to his children, and to the Terrarians who live with them.*");
                Mes.Add("*I'm happy that there's quite a number of TerraGuardians living here.*");
            }

            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Having your presence around is also a blessing on my life, [nickname]. I hope I see you more often.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*Yes, [nickname]. That will make it easier for me to bless this land too.*";
                case MoveInContext.Fail:
                    return "*I can't... Not right now..*";
                case MoveInContext.NotFriendsEnough:
                    return "*I'm sorry [nickname], but I'm also expected in the Ether Realm...*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*This is sad, [nickname]. I considered this place like my second home... I will be moving away my things.*";
                case MoveOutContext.Fail:
                    return "*No... Sorry.. My mission is not over here..*";
                case MoveOutContext.NoAuthorityTo:
                    return "*I'm sorry.. I don't mean to be rude, but you are not the person who let me stay here.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string MountCompanionMessage(Companion companion, MountCompanionContext context)
        {
            switch(context)
            {
                case MountCompanionContext.Success:
                    return "*I don't mind, [nickname]. I glad to help.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*I'm happy that you'll offer me a ride. Thank you.*";
                case MountCompanionContext.Fail:
                    return "*Not at this moment..*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*I apologise, but no..*";
            }
            return base.MountCompanionMessage(companion, context);
        }

        public override string DismountCompanionMessage(Companion companion, DismountCompanionContext context)
        {
            switch(context)
            {
                case DismountCompanionContext.SuccessMount:
                    return "*Sure. I hope you were able to rest your legs, [nickname].*";
                case DismountCompanionContext.SuccessMountOnPlayer:
                    return "*Thank you for offering me a ride, [nickname].*";
                case DismountCompanionContext.Fail:
                    return "*Not right now. Just a bit longer..*";
            }
            return base.DismountCompanionMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*I can accompany you on your travels, [nickname].*";
                case JoinMessageContext.Fail:
                    return "*I can't go with you right now. I have something else I must do first.*";
                case JoinMessageContext.FullParty:
                    return "*You have too many people with you. I can't travel with you right now..*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "*Okay [nickname]. I'll be praying that you and your companions continue your journey safe and well.*";
                case LeaveMessageContext.Fail:
                    return "*I don't think this is a good moment to leave your group.*";
                case LeaveMessageContext.AskIfSure:
                    return "*I can leave your group, but may I do that in a safe place?*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Very well.. I will find my way back home then. Have a safe travel, [nickname].*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Just a little longer. Let's find some place with a friend or more so I can leave.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    return "*I have nothing I need done right now.*";
                case RequestContext.HasRequest:
                    return "*I do have something I need to get done. It is [objective]. Could you help this priestess on this task?*";
                case RequestContext.Accepted:
                    return "*Thank you [nickname]. I will pray that you be able to finish my task.*";
                case RequestContext.Rejected:
                    return "*I understand. I'll scrap this task then.*";
                case RequestContext.Completed:
                    return "*Thank you, [nickname]. Just a blessing wouldn't be enough to express my gratitude, so take those material rewards.*";
                case RequestContext.Failed:
                    return "*You failed..? There, don't feel bad. Next time you'll manage to do my request. Don't worry.*";
                case RequestContext.TooManyRequests:
                    return "*It is unhealthy to stack too many obligations on yourself. Do your other jobs first, and then come see mine.*";
                case RequestContext.PostponeRequest:
                    return "*It's fine. I understand you might be too busy right now. Come see me later when you are able to take my request.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Did you complete my request, [nickname]?*";
                case RequestContext.RemindObjective:
                    return "*I asked you to [object], [nickname].*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You think you can't do my request? Are you sure you want to cancel it?*";
                case RequestContext.CancelRequestYes:
                    return "*That's sad, [nickname]. But if you think you can't do it... Alright, you no longer need to mind about my request.*";
                case RequestContext.CancelRequestNo:
                    return "*You can do it, [nickname]. Now, is there something else you need?*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "(She's sleeping soundly.)";
                        case 1:
                            return "(She seems to be chanting prayers while sleeping.)";
                        case 2:
                            return "(Looks like she's having a dream. A dream about missing Ether Realm? Or a dream of misisng someone?)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    return "*Yawn... [nickname]? What is it? Is there any emergency?*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*My request..? Is it about..? I'm so sleepy, [nickname]..*";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share) return "*Yes, we can share the same bed. At least we can warm each other in cold nights.*";
            return "*I will take another bed then.*";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share) return "*I don't mind sharing my chair with you. I don't mind having something to hug.*";
            return "*I will pick another chair to use then.*";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context)
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "*You want me to change how I will act in combat? What do you suggest I do?*";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "*I will take foes on close encounter then.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*I shall avoid some distance from my foes.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*I will attack foes by distance then.*";
                case TacticsChangeContext.Nevermind:
                    return "*Alright, [nickname]. Is there something else you want to speak to me about?*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Yes, I can talk for a while. What do you want to talk about?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there something else you want to discuss about?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*I hope I cleared all questions you had.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override void ManageOtherTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            dialogue.AddOption("I have some questions.", OnAskForQuestions);
        }

        private void OnAskForQuestions()
        {
            MessageDialogue m = new MessageDialogue("*Do you have any question?\nPlease, ask. I will be happy to answer it.*");
            m.AddOption("Who " +MainMod.TgGodName+ " is?", OnAskWhoTgGodIs);
            m.AddOption("Who the TerraGuardians are?", OnAskAboutTerraGuardiansThemselves);
            m.AddOption("What do you know of the war?", OnAskAboutTheWar);
            m.AddOption("I want to know more about the blessings.", OnAskAboutTheBlessings);
            m.AddOption("I have nothing else to ask.", Dialogue.TalkAboutOtherTopicsDialogue);
            m.RunDialogue();
        }

        private void OnAskWhoTgGodIs()
        {
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*"+MainMod.TgGodName+" created the TerraGuardians.*");
            m.AddDialogueStep("*Is said that he's a giant striped creature, but I never saw him myself.*");
            m.AddDialogueStep("*He seems to actually like dealing with Terrarians, which also lead to our creation, we were supposed to be your protectors.*");
            m.AddDialogueStep("*I wonder if some day I will get to see him myself.*");
            m.AddOption("Return", OnAskForQuestions);
            m.RunDialogue();
        }

        private void OnAskAboutTerraGuardiansThemselves()
        {
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*Like I mentioned before, the TerraGuardians are "+MainMod.TgGodName+"'s creation.*");
            m.AddDialogueStep("*We were created to not only to protect the Terrarians, but also to live our lives with them.*");
            m.AddDialogueStep("*And by the look of it, that's happening again.*");
            m.AddOption("Return", OnAskForQuestions);
            m.RunDialogue();
        }

        private void OnAskAboutTheWar()
        {
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*Ah yes, the war that separated TerraGuardians and Terrarians in different dimensions.*");
            m.AddDialogueStep("*Legends says that TerraGuardians are way stronger than Terrarians.*");
            m.AddDialogueStep("*That made the Terrarians at the time fear that they could end up being subjugated by us, and that's when the complications begun.*");
            m.AddDialogueStep("*Failed dialoguing, confusion and the death of a lot of Terrarians made us move to the Ether Realm, to stop the bloodshed.*");
            m.AddDialogueStep("*I question myself why "+MainMod.TgGodName+" hadn't intervened at the time, but he most likelly had a reason why.*");
            m.AddDialogueStep("*I'm sorry, but I really don't like speaking about the war..*");
            m.AddOption("Return", OnAskForQuestions);
            m.RunDialogue();
        }

        private void OnAskAboutTheBlessings()
        {
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*You want to know more about the blessings, I will tell you about them.*");
            m.AddDialogueStep("*The "+MainMod.TgGodName+"'s Claw blessing is a blessing of Wellbeing.*");
            m.AddDialogueStep("*I pray for "+MainMod.TgGodName+" to bestow them to everyone in the world, so they live their life well and happy.*");
            m.AddDialogueStep("*There is the other blessing too, but I only pray for it when the world or someone is in danger.*");
            m.AddDialogueStep("*The "+MainMod.TgGodName+"'s Tail blessing protects the people I pray for from danger.*");
            m.AddDialogueStep("*When you're facing a tough foe, you can be assured that I have called upon "+MainMod.TgGodName+"'s Tail blessing upon you and your companions.*");
            m.AddOption("Return", OnAskForQuestions);
            m.RunDialogue();
        }
        
        public override string ControlMessage(Companion companion, ControlContext context)
        {
            switch(context)
            {
                case ControlContext.SuccessTakeControl:
                    return "*Let us Bond-Merge, and may "+MainMod.TgGodName+" protect us in your task.*";
                case ControlContext.SuccessReleaseControl:
                    return "*I hope I helped you with what you needed my help for.*";
                case ControlContext.FailTakeControl:
                    return "*I'm sorry, I won't Bond-Merge with you right now.*";
                case ControlContext.FailReleaseControl:
                    return "*I believe I might do an attack against your safety if I did that now. Let's keep Bond-Merged for a bit longer.*";
                case ControlContext.NotFriendsEnough:
                    return "*I'm sorry, but no.*";
                case ControlContext.ControlChatter:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "*Do you need to ask me something, [nickname]?*";
                        case 1:
                            return "*Don't worry about me, I'm praying that you achieve your goal.*";
                        case 2:
                            return "*Keep your head focused on your goal, [nickname]. May my body be the vehicle to achieve it.*";
                    }
            }
            return base.ControlMessage(companion, context);
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "*I believe it should be easier to bless this world, if I stayed here with everyone. If you have a house for me, I may move in.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*I believe that part of my mission is aiding you in your journey, and I believe the maximum I can do to help you with that, is allow you to Bond-Merge with me. I hope this help you on your journey, [nickname].*";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*I need to meet this world I pray on daily. May you help me meet it by taking me on your journeys?*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*Are you feeling a bit worn out, [nickname]? I may carry you if you want. I won't mind at all.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "*I do not intend to put this on your shoulders, [nickname], but I might need your help to do some of my tasks. If you may, could you check some of my requests?*";
            }
            return base.UnlockAlertMessages(companion, context);
        }
    }
}