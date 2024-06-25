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
            Mes.Add("*Upon noticing TerraGuardians appearing in this realm, I felt that it was my mission to bring "+TgGodName+"'s influence to this realm.*");
            Mes.Add("*Blessings of "+TgGodName+" upon you, [nickname].*");

            Mes.Add("*Your soul sparkles like a little star, [nickname]. I pray you have a safe journey.*");
            Mes.Add("*This seems like a nice place to live. No wonder other TerraGuardians came here.*");
            Mes.Add("*You built all those houses by yourself? I'm actually impressed at your dedication.*");
            Mes.Add("*You know, since the opening of the portals in the Ether Realm, TerraGuardians have been travelling through different worlds.*");

            if (companion.IsFollower && !CelesteBase.PrayedToday)
            {
                Mes.Add("*Do let me know when I can pray, or the blessings of "+MainMod.TgGodName+" won't stay much longer.*");
            }

            if (WorldMod.GetTerraGuardiansCount >= 11)
            {
                Mes.Add("*Impressive how many TerraGuardians came to this world. I'm starting to worry about the Ether Realm.*");
                Mes.Add("*It has been quite a looong time since TerraGuardian last were living in the Terra Realm. It feels like the story repeats itself, and hopefully, with no calamity this time.*");
                Mes.Add("*With the amount of TerraGuardians in this world, I don't feel out of place anymore. "+TgGodName+" would love seeing this.*");
            }
            if (WorldMod.GetTerraGuardiansCount >= 6)
            {
                Mes.Add("*You've met a lot of TerraGuardians, [nickname].*");
                Mes.Add("*I'm glad that I came here. I can spread "+TgGodName+"'s blessing to his children, and to the Terrarians who live with them.*");
                Mes.Add("*I'm happy that there's a lot of TerraGuardians living here.*");
            }

            if(CanTalkAboutCompanion(CompanionDB.Rococo))
            {
                Mes.Add("*I sense a very friendly presence whenever I'm close to [gn:"+CompanionDB.Rococo+"]. It's odd, it's like he's someone I'm familiar with.*");
                Mes.Add("*I feel very comfortable when close to [gn:"+CompanionDB.Rococo+"]. I don't know why.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Blue))
            {
                Mes.Add("*I wonder if I can have such a majestic hairstyle, like the one [gn:"+CompanionDB.Blue+"] has.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Brutus))
            {
                Mes.Add("*I feel a faint holy aura on [gn:"+CompanionDB.Brutus+"]... I only sensed such aura on Royal Guards, in the Ether Realm.*");
                Mes.Add("*Sometimes when I'm doing prayers, [gn:"+CompanionDB.Brutus+"] appears to take me for a drink or two. He's really nice.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Alex))
            {
                Mes.Add("*I love having [gn:"+CompanionDB.Alex+"]'s company with me when I'm praying.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Zack))
            {
                Mes.Add("*It is sad what happened to [gn:"+CompanionDB.Zack+"]. I pray he ends up getting his body full of life again.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Bree))
            {
                Mes.Add("*[gn:"+CompanionDB.Bree+"] isn't a bad person, but she needs her husband by her side.*");
                Mes.Add("*I do like to talk with [gn:"+CompanionDB.Bree+"]. Even though she's grumpy, she has quite interesting stories about her husband.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Mabel))
            {
                Mes.Add("*You say [gn:"+CompanionDB.Mabel+"] came from the sky? She's not an angel, I assure you.*");
                Mes.Add("*What is that odd pose [gn:"+CompanionDB.Mabel+"] does when standing? Why is she like that?*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Leopold))
            {
                Mes.Add("*Magic and religion sometimes clashes, so I'm unable to reason with [gn:"+CompanionDB.Leopold+"].*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Vladimir))
            {
                Mes.Add("*[gn:"+CompanionDB.Vladimir+"] does a good job at comforting people, sometimes people want someone to listen to them. The only problem is when they ask if they understand.*");
                Mes.Add("*I tried hugging [gn:"+CompanionDB.Vladimir+"], and we both ended up getting hit by a static shock. I guess we should never do that again.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Malisha))
            {
                Mes.Add("*I'm not comfortable with [gn:"+CompanionDB.Malisha+"] doing evil sorcery here. She already caused many troubles in the Ether Realm.");
                Mes.Add("*If I could exorcise [gn:"+CompanionDB.Malisha+"] away from here, I would.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Fluffles))
            {
                Mes.Add("*I sense [gn:"+CompanionDB.Fluffles+"] pain, and yet, I can't do anything about that...*");
                Mes.Add("*The only thing I can do for [gn:"+CompanionDB.Fluffles+"], is pray that she finds her inner peace. You can help by being kind to her too.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Minerva))
            {
                Mes.Add("*I think [gn:"+CompanionDB.Minerva+"] should take it easy on eating. That's unhealthy.*");
                Mes.Add("*Sometimes [gn:"+CompanionDB.Minerva+"] calls me to lunch with her. You won't believe how much she eats.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Liebre))
            {
                Mes.Add("*You think [gn:"+CompanionDB.Liebre+"] is one of "+MainMod.TgGodName+"? You thought right. They're TerraGuardians reapers. Don't worry, they know where to deliver the souls.*");
                Mes.Add("*Don't fear [gn:"+CompanionDB.Liebre+"]'s presence here. Should your time comes, it will happen. He will not cause it.*");
                Mes.Add("*I actually feel quite comfortable that [gn:"+CompanionDB.Liebre+"] is around. At least I know if should one end up deceased, they will be taken to their final destination.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Glenn))
            {
                Mes.Add("*[gn:"+CompanionDB.Glenn+"] is such a blessed child. Try not to teach him bad things.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Cinnamon))
            {
                Mes.Add("*I like that [gn:"+CompanionDB.Cinnamon+"] is really interested in mastering cooking, but she needs to take it easy on the seasonings.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Miguel))
            {
                Mes.Add("*H-how did [gn:"+CompanionDB.Miguel+"] get such... Body? Wow!*");
                Mes.Add("*I can't stop looking at [gn:"+CompanionDB.Miguel+"]... I can't stop..*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Luna))
            {
                Mes.Add("*[gn:"+CompanionDB.Luna+"] has been answering questions about TerraGuardians that you have? Then you know we are not your enemies, right?*");
                Mes.Add("*Beside [gn:"+CompanionDB.Luna+"] knows many things about TerraGuardians, I'm better suited to speak about our religion.*");
            }
            if(CanTalkAboutCompanion(CompanionDB.Green))
            {
                Mes.Add("*Please don't rely only on prayers, should you have illness. [gn:"+CompanionDB.Green+"] is here to take care of that.*");
                Mes.Add("*Whenever I'm being treated by [gn:"+CompanionDB.Green+"], I have an irrational fear that makes me want to run away.*");
            }
            if (CanTalkAboutCompanion(CompanionDB.Leona))
            {
                Mes.Add("*I sensed that aura again. Another member of the Royal Guard is here. I believe [gn:"+CompanionDB.Leona+"] was one of them. Such holy aura she carries I only sensed on their members.*");
                Mes.Add("*If you question yourself if [gn:"+CompanionDB.Leona+"]'s arms get sore from carrying that sword? She told me that sometimes does.*");
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                    Mes.Add("*Whenever [gn:"+CompanionDB.Leona+"] is about to leave the town to do something dangerous, she always prays for her, and [gn:"+CompanionDB.Brutus+"]'s safety.*");
            }

            if (companion.IsUsingToilet)
            {
                Mes.Clear();
                Mes.Add("*[nickname]... This is not a good moment..*");
                Mes.Add("*It's a bit hard to concentrate with you staring at me.*");
            }

            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*Having your presence around is also a blessing on my life, [nickname]. I hope I see you more often.*");
            Mes.Add("*I enjoy the company of everyone here. It's so good to see Terrarians and TerraGuardians together.*");
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
                    return "*No... Sorry.. My mission is not over yet..*";
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
                    return "*I don't mind, [nickname]. I'm glad to help.*";
                case MountCompanionContext.SuccessMountedOnPlayer:
                    return "*I'm happy that you'll offer me a ride. Thank you.*";
                case MountCompanionContext.Fail:
                    return "*Not at this moment..*";
                case MountCompanionContext.NotFriendsEnough:
                    return "*I apologize, but no..*";
                case MountCompanionContext.SuccessCompanionMount:
                    return "*I don't mind, [nickname]. I can carry [target].*";
                case MountCompanionContext.AskWhoToCarryMount:
                    return "*I can carry someone on my shoulder, Who should I carry?*";
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
                    return "*Not right now. Just wait a bit longer..*";
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
                    return "*Very well.. I will find my way back home then. Safe travels, [nickname].*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Just a little longer. Let's find someplace with a friend or more so I can leave.*";
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
                    return "*Thank you [nickname]. I will pray that you will be able to finish my task.*";
                case RequestContext.Rejected:
                    return "*I understand. I'll scrap this task then.*";
                case RequestContext.Completed:
                    return "*Thank you, [nickname]. Just a blessing wouldn't be enough to express my gratitude, so take these material rewards.*";
                case RequestContext.Failed:
                    return "*You failed..? There, don't feel bad. Next time you'll be able to do my request. Don't worry.*";
                case RequestContext.TooManyRequests:
                    return "*It is unhealthy to stack too many obligations on yourself. Do your other jobs first, and then come back for mine.*";
                case RequestContext.PostponeRequest:
                    return "*It's fine. I understand you might be too busy right now. Come see me later when you are able to take my request.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Did you complete my request, [nickname]?*";
                case RequestContext.RemindObjective:
                    return "*I asked you to [objective], [nickname].*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*You think you can't do my request? Are you sure you want to cancel it?*";
                case RequestContext.CancelRequestYes:
                    return "*That's sad, [nickname]. But if you think you can't do it... Alright, you no longer need to worry about my request.*";
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
                            return "(Looks like she's having a dream. A dream about missing the Ether Realm? Or a dream of missing someone?)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    return "*Yawn... [nickname]? What is it? Is there an emergency?*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*My request..? Is it about..? I'm so sleepy, [nickname]..*";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share) return "*Yes, we can share the same bed. At least we can warm each other on cold nights.*";
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
                    return "*I will take on foes up close then.*";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "*I shall avoid some distance from my foes.*";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "*I will attack foes from a distance then.*";
                case TacticsChangeContext.Nevermind:
                    return "*Alright, [nickname]. Is there something else you want to speak to me about?*";
                case TacticsChangeContext.FollowAhead:
                    return "*I don't think that's a good idea...*";
                case TacticsChangeContext.FollowBehind:
                    return "*I'll be right behind you, then.*";
                case TacticsChangeContext.AvoidCombat:
                    return "*Does that mean I can't defend myself, either?*";
                case TacticsChangeContext.PartakeInCombat:
                    return "*I can defend myself again.*";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            if (companion.IsFollower && !CelesteBase.PrayedToday)
            {
                dialogue.AddOption("You can pray now.", OnTellHerToPray);
            }
        }

        private void OnTellHerToPray()
        {
            MessageDialogue m = new MessageDialogue("*I will. Thank you [nickname].\nThis shouldn't take long.*");
            m.AddOption("Ok", DoPrayAndEndDialogue);
            m.RunDialogue();
        }

        private void DoPrayAndEndDialogue()
        {
            Dialogue.Speaker.RunBehavior(new Celeste.CelestePrayerBehavior());
            Dialogue.EndDialogue();
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Yes, I can talk for a while. What do you want to talk about?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there something else you want to discuss?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*I hope I answered all the questions you had.*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override void ManageOtherTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            if (CelesteBase.CanPrayHere)
            {
                dialogue.AddOption("Stop praying here.", OnProhibitPrayer);
            }
            else
            {
                dialogue.AddOption("You may pray here.", OnAllowPrayer);
            }
        }

        public override void ManageChatTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            dialogue.AddOption("I have some questions.", OnAskForQuestions);
        }

        private void OnProhibitPrayer()
        {
            CelesteBase.CanPrayHere = false;
            Dialogue.LobbyDialogue("*Huh? I don't understand. I just want to spread "+MainMod.TgGodName+"'s blessings throughout the world...\nSigh... Fine, I will stop praying. It's your world, anyways.\nIf you change your mind, just talk to me again.*");
        }

        private void OnAllowPrayer()
        {
            CelesteBase.CanPrayHere = true;
            Dialogue.LobbyDialogue("*I can? Thank you. "+MainMod.TgGodName+"'s blessings will now be present in this world.*");
        }

        private void OnAskForQuestions()
        {
            MessageDialogue m = new MessageDialogue("*Do you have any question?\nPlease, ask. I will be happy to answer it.*");
            m.AddOption("Who is " +MainMod.TgGodName+ "?", OnAskWhoTgGodIs);
            m.AddOption("What are TerraGuardians?", OnAskAboutTerraGuardiansThemselves);
            m.AddOption("What do you know of the war?", OnAskAboutTheWar);
            m.AddOption("I want to know more about the blessings.", OnAskAboutTheBlessings);
            m.AddOption("I have nothing else to ask.", Dialogue.ChatDialogue);
            m.RunDialogue();
        }

        private void OnAskWhoTgGodIs()
        {
            MultiStepDialogue m = new MultiStepDialogue();
            m.AddDialogueStep("*"+MainMod.TgGodName+" created the TerraGuardians.*");
            m.AddDialogueStep("*It is said that he's a giant striped creature, but I've never seen him myself.*");
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
            m.AddDialogueStep("*Ah yes, the war that separated TerraGuardians and Terrarians into different dimensions.*");
            m.AddDialogueStep("*Legends says that TerraGuardians are way stronger than Terrarians.*");
            m.AddDialogueStep("*That made the Terrarians at the time fear that they could end up being subjugated by us, and that's when the complications begun.*");
            m.AddDialogueStep("*Failed dialoguing, confusion and the death of a lot of Terrarians made us move to the Ether Realm, to stop the bloodshed.*");
            m.AddDialogueStep("*I question myself why "+MainMod.TgGodName+" hadn't intervened at the time, but he most likely had a reason why.*");
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
                    return "*I believe I might put you in danger if I did that now. Let's stay Bond-Merged for a bit longer.*";
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
                case ControlContext.GiveCompanionControl:
                    return "*If you say so. Then, I will just wander around meanwhile.*";
                case ControlContext.TakeCompanionControl:
                    return "*You take full control again.*";
            }
            return base.ControlMessage(companion, context);
        }

        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch(context)
            {
                case UnlockAlertMessageContext.MoveInUnlock:
                    return "*I believe it should be easier to bless this world if I stayed here with everyone. If you have a house for me, I may move in.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*I believe that part of my mission is aiding you in your journey, and I believe the maximum I can do to help you with that, is allow you to Bond-Merge with me. I hope this helps you on your journey, [nickname].*";
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*I need to meet this world I pray daily. May you help me meet it by taking me on your journeys?*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*Are you feeling a bit worn out, [nickname]? I may carry you if you want. I won't mind at all.*";
                case UnlockAlertMessageContext.RequestsUnlock:
                    return "*I do not intend to put this on your shoulders, [nickname], but I might need your help to do some of my tasks. If you may, could you check some of my requests?*";
                case UnlockAlertMessageContext.BuddiesModeUnlock:
                    return "*[nickname], as you know, as a servant of "+MainMod.TgGodName+", I have a commitment to worshipping him, but.. As time passed, and you and I were interacting with each other, I've been thinking about having a commitment with someone else, and that's why I ask you: would you pick me as your Buddy, and be my second commitment?*";
                case UnlockAlertMessageContext.BuddiesModeBenefitsMessage:
                    return "*[nickname], you do know about the benefits of being a TerraGuardian Buddy, right? That means not only are we being bound by life, and each getting stronger by friendship, but also means I will trust anything you ask of me, without saying otherwise, unless it endangers you. And I trust that your decisions are right.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string InteractionMessages(Companion companion, InteractionMessageContext context)
        {
            switch(context)
            {
                case InteractionMessageContext.OnAskForFavor:
                    return "*Do you require my assistance with something?*";
                case InteractionMessageContext.Accepts:
                    return "*I'll be happy to help you with that.*";
                case InteractionMessageContext.Rejects:
                    return "*I'm sorry. I can't help you with that.*";
                case InteractionMessageContext.Nevermind:
                    return "*You changed your mind? It's alright.*";
            }
            return base.InteractionMessages(companion, context);
        }

        public override string ChangeLeaderMessage(Companion companion, ChangeLeaderContext context)
        {
            switch(context)
            {
                case ChangeLeaderContext.Success:
                    return "*In the name of "+MainMod.TgGodName+", I shall lead this group.*";
                case ChangeLeaderContext.Failed:
                    return "*I can't lead the group for you right now..*";
            }
            return "";
        }

        public override string BuddiesModeMessage(Companion companion, BuddiesModeContext context)
        {
            switch(context)
            {
                case BuddiesModeContext.AskIfPlayerIsSure:
                    return "*Oh [nickname], that was so sudden. I mean, I'm not against but... Are you sure you want to pick me as your Buddy?*";
                case BuddiesModeContext.PlayerSaysYes:
                    return "*Then I accept, [nickname]. I hope "+MainMod.TgGodName+" also approves and blesses our Buddiship.*";
                case BuddiesModeContext.PlayerSaysNo:
                    return "*Oh.. Hm.. This is.. Unexpected.. Sorry about the awkwardness, [nickname].*";
                case BuddiesModeContext.NotFriendsEnough:
                    return "*I can't be Buddies with someone I hardly know..*";
                case BuddiesModeContext.Failed:
                    return "*We can't right now...*";
                case BuddiesModeContext.AlreadyHasBuddy:
                    return "*You already have a Buddy, [nickname].*";
            }
            return "";
        }

        public override string InviteMessages(Companion companion, InviteContext context)
        {
            switch(context)
            {
                case InviteContext.Success:
                    return "*You need me there? Sure. It's no problem for me.*";
                case InviteContext.SuccessNotInTime:
                    return "*I can go there, but not right now since it's quite late. Tomorrow I will be showing up. I promise.*";
                case InviteContext.Failed:
                    return "*Sorry, but I am busy right now.*";
                case InviteContext.CancelInvite:
                    return "*Oh, you don't need me to visit anymore. Okay, it's fine.*";
                case InviteContext.ArrivalMessage:
                    return "*I am here [nickname].*";
            }
            return "";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldEscapedMessage:
                    return "*Did we offend him, somehow?*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*Is that some odd way of showing gratitude?*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch(context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*Praise "+MainMod.TgGodName+", you're alive. Don't worry, we're going somewhere safe.*";
                case ReviveContext.RevivingMessage:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("*Please don't move, it might be harder to heal you.*");
                        Mes.Add("*Don't worry about anything, you'll be okay.*");
                        Mes.Add("*You'll be walking again soon I promise.*");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case ReviveContext.OnComingForFallenAllyNearbyMessage:
                    return "*No! I'm coming!! Hang on!*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*You're safe with me now. Allow me to heal your wounds.*";
                case ReviveContext.RevivedByItself:
                    return "*I'm fine.. Thanks for at least protecting me...*";
                case ReviveContext.ReviveWithOthersHelp:
                    return "*Thank you all, and I'm sorry for worrying everyone like that..*";
            }
            return base.ReviveMessages(companion, target, context);
        }
    }
}
