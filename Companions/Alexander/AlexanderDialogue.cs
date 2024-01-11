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
            List<string> Mes = new List<string>();
            Mes.Add("*You! No, you aren't the Terrarian I'm looking for. I can smell it.*");
            Mes.Add("*You, stop! *snif snif* Hmmm... No, you're not who I wanted to find.*");
            Mes.Add("*Hold on. *snif, snif snif, snif* Hm... I was checking if you were who I'm looking for, and you are not.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string NormalMessages(Companion companion)
        {
            Player player = MainMod.GetLocalPlayer;
            List<string> Mes = new List<string>();
            if (companion.IsUsingToilet)
            {
                Mes.Add("*Whatever It is, couldn't wait?*");
                Mes.Add("*[nickname], I'm trying to process some things here.*");
                Mes.Add("*Your presence here is making It harder for me to finish what I'm doing.*");
            }
            else
            {
                Mes.Add("*Any update on our little investigation?*");
                Mes.Add("*This mystery is making me intrigued. I've never found something like this before.*");
                Mes.Add("*Is there something you seek, [nickname]?*");
                Mes.Add("*Did you found the Terrarian we are looking for? I can't blame if you didn't, my description is really vague.*");

                Mes.Add("*I can identify anyone by sleuthing them, but not everyone may like that idea.*");
                Mes.Add("*Everytime I sleuth someone, I can replicate part of their strength.*");
                Mes.Add("*I can identify Terrarians by sleuthing them, but I don't get stronger by doing so.*");
                Mes.Add("*The only clue I've got from my investigation, is the scent of a unknown person that was at the place. I will find the one who has that scent.*");

                if (!Main.dayTime && !Main.bloodMoon)
                {
                    Mes.Add("*I have troubles getting into sleep, my head is always filled with thoughts when I get my head on the pillow.*");
                    Mes.Add("*The stars sometimes help me with my thoughts. Lying down on the ground and staring upwards has a mind opening effect.*");
                }
                else if(Main.dayTime && !Main.eclipse)
                {
                    if (Main.raining)
                    {
                        Mes.Add("*The sound of rain drops have a soothening effect.*");
                    }
                    else
                    {
                        Mes.Add("*I don't feel comfortable investigating while exposed to the sun, I get some annoying headaches when I do that.*");
                    }
                }

                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    Mes.Add("*How old is [gn:" + CompanionDB.Rococo + "]? It doesn't seems like he's that old.*");
                    Mes.Add("*I'm intrigued at how [gn:" + CompanionDB.Rococo + "] lives his life. Maybe that's another mystery.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    if (!CanTalkAboutCompanion(CompanionDB.Zack))
                    {
                        Mes.Add("*[gn:" + CompanionDB.Blue + "] came earlier wanting me to help her find someone. I'm trying to do something to help her, but that's delaying my personal investigation.*");
                        Mes.Add("*Looks like [gn:" + CompanionDB.Blue + "] lost someone too... I wonder if It's the same as... No, certainly not...*");
                        Mes.Add("*I think I discovered some clue about [gn:" + CompanionDB.Blue + "]'s missing person. I heard that seems to show up during Blood Moons, on the edges of the world. Better you be careful.*");
                    }
                    else
                    {
                        Mes.Add("*It's good to see that [gn:" + CompanionDB.Blue + "] has found who she was looking for, but the result even I didn't expected.*");
                        Mes.Add("*I was shocked when I discovered [gn:" + CompanionDB.Zack + "] fate, but [gn:" + CompanionDB.Blue + "]'s reception to that seems to be the reward by Itself.*");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    if (!CanTalkAboutCompanion(CompanionDB.Sardine))
                    {
                        Mes.Add("*[gn:" + CompanionDB.Bree + "] told me that she's looking for her husband. The only clue I got is that he were pursuing the King Slime. I think that may be dangerous, actually.*");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Mabel))
                {
                    Mes.Add("*I can't really stay in the same room as [gn:" + CompanionDB.Mabel + "], she simply distracts me.*");
                    Mes.Add("*I wonder what kind of thing [gn:" + CompanionDB.Mabel + "] would like... Wait, I shouldn't be thinking about that kind of thing.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Malisha))
                {
                    Mes.Add("*You should keep a closer eye to [gn:" + CompanionDB.Malisha + "], she caused many troubles in the ether realm, on every village she came from.*");
                    Mes.Add("*Never speak with [gn:" + CompanionDB.Malisha + "] during a Blood Moon. She catches anyone who tries speaking with her to force doing experiments on.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Wrath))
                {
                    Mes.Add("*A TerraGuardian fragmented into 4 emotions... Where could the others be...?*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                {
                    Mes.Add("*Sometimes I hang around with [gn:"+CompanionDB.Brutus+"], he sometimes gives me ideas for clues I need on my investigations.*");
                    Mes.Add("*I often have to ask [gn:"+CompanionDB.Brutus+"] for help to try investigating some place.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Michelle))
                {
                    Mes.Add("*I sleuthed [gn:" + CompanionDB.Michelle + "], and she's not the one I'm looking for.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Nemesis))
                {
                    Mes.Add("*I was unable to caught [gn:" + CompanionDB.Nemesis + "] scent, no matter how hard I tried. I really hope he isn't the one I'm looking for.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Luna))
                {
                    if ((companion as AlexanderBase.AlexanderCompanion).HasAlexanderSleuthedGuardian(CompanionDB.Luna))
                    {
                        Mes.Add("*[gn:" + CompanionDB.Luna + "] is really furious about the fact that I know many things about her.*");
                    }
                    else
                    {
                        Mes.Add("*I will get to know [gn:"+CompanionDB.Luna+"] more some time...*");
                    }
                }
                if (CanTalkAboutCompanion(CompanionDB.Green))
                {
                    Mes.Add("*It's really great having a medic around. Or at least one that knows how TerraGuardian bodies works.*");
                }
                if (companion.IsPlayerRoomMate(player))
                {
                    Mes.Add("*Yeah, I don't mind sharing my room with you, as long as you have a bed for yourself.*");
                    Mes.Add("*You think I sleuth you during your sleep? What else do I need to discover about you?*");
                }
            }
            if (PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
            {
                Mes.Clear();
                Mes.Add("*I don't mean to alarm you, but there is a ghost behind you.*");
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*I wonder if they are fine... Who? Oh... Nevermind..*");
            Mes.Add("*I used to be part of a group with 3 Terrarians, but then they disappeared after... Something happened..*");
            Mes.Add("*My only hope is finding that Terrarian who made seems involved into my friends disappearance. If I find him, I may find a clue of where they could be.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.Next(2) == 0) return "*No.*";
                    return "*Not yet.*";
                case RequestContext.HasRequest:
                    if (Main.rand.Next(2) == 0)
                        return "*I can't get myself distracted from my investigation, so I need your help to [objective]. Can you do that for me?*";
                    return "*There is something else catching my attention, but It would be a great time saving if you did It for me. I need you to [objective], and then report back your progress. What do you say?*";
                case RequestContext.Completed:
                    if (Main.rand.Next(2) == 0)
                        return "*It's good to know that I can count on you. I think this will suit the time you spent.*";
                    return "*You completed what I asked for, then? Good. This is a compensation for the time you spent.*";
                case RequestContext.Accepted:
                    return "*Perfect. Return to me when you manage to finish It.*";
                case RequestContext.TooManyRequests:
                    return "*Even I don't get myself overloaded with things to do.*";
                case RequestContext.Rejected:
                    return "*I don't think I will manage to have the time to do It, but I'll try.*";
                case RequestContext.PostponeRequest:
                    return "*I may end up doing It myself that way...*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Have you don't what I asked you to do?*";
                case RequestContext.RemindObjective:
                    return "*You don't really have good memory, right? Just do this to me, [objective].*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*Is this request too complicated for you? I may relieve you from It if you want.*";
                case RequestContext.CancelRequestYes:
                    return "*Hm... Okay, you may get it out of your head now.*";
                case RequestContext.CancelRequestNo:
                    return "*Good.*";

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
                        Mes.Add("*Come on... You've got to help me solve the mystery.*");
                        Mes.Add("*I think I just need to do this... It's working.*");
                        Mes.Add("*Those wounds...*");
                    }
                    else
                    {
                        Mes.Add("*Hold on.*");
                    }
                    Mes.Add("*I know you're feeling pain, but try not to move.*");
                    Mes.Add("*If It helps, hold my paw.*");
                    return Mes[Main.rand.Next(Mes.Count)];
                case ReviveContext.HelpCallReceived:
                    return "*You're in a safe place now, let me take care of those wounds.*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*I'm here.*";
                case ReviveContext.ReviveWithOthersHelp:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*You all did good. Thanks.*";
                    return "*I apreciate the help, I really mean It.*";
                case ReviveContext.RevivedByItself:
                    if (Main.rand.NextDouble() < 0.5f)
                        return "*You could have helped me. I'm fine now, by the way.*";
                    return "*That was because of my sleuthing, right?*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch(Main.rand.Next(3))
                    {
                        default: return "(You noticed him sniffing a while, and then he slept with a smile on the face.\nI think he knows I'm near him.)";
                        case 1: return "*Snif.. Snif... [nickname] close... Zzzz...*";
                        case 2: return "(As he sleeps, he says the name of anyone who comes close to him, including yours.)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    return "*...Yawn... I really need a rest, can you be quick so I can return to sleep?*";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "*Yaaaaawn... Did you do what I asked for? Or want to talk about something else?*";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "*That will be handy for my investigation. I can move in here.*";
                case MoveInContext.Fail:
                    return "*Not right now, [nickname].*";
                case MoveInContext.NotFriendsEnough:
                    return "*Would be handy, but I don't know if I should trust you as of yet.*";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "*It's a pity. I'll pack my things.*";
                case MoveOutContext.Fail:
                    return "*Not the best moment to discuss that.*";
                case MoveOutContext.NoAuthorityTo:
                    return "*I'm giving you nothing.*";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "*Ist nicht allein, [nickname].*";
                case JoinMessageContext.FullParty:
                    return "*I wouldn't feel comfortable in such a big group.*";
                case JoinMessageContext.Fail:
                    return "*Not now.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*I don't think this is the best place to be left, can't we find a better place for me to leave?*";
                case LeaveMessageContext.Success:
                    return "*I'll return to my investigations, then.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*I hope I make It safelly to home.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Good, let's look for a town or some safe place for me to leave.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Do you want to know something?*";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "*Is there anything else you'd want to ask?*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Had enough questions?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                case MessageIDs.LeopoldMessage1:
                    return "*Save us from what? What is he talking about? Why is he openly saying all that?*";
                case MessageIDs.LeopoldMessage2:
                    return "*Shh... It will be hard to rescue you if the Terrarian realizes that we are talking.*";
                case MessageIDs.LeopoldMessage3:
                    return "*The Terrarian is already listening to us talking, and we're not their hostages. You're worrying way too much.*";
                case MessageIDs.LeopoldEscapedMessage:
                    return "*[nickname], why did you only watched all that?*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*And people call me weird when they see me sleuthing someone.*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}