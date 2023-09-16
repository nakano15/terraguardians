using Terraria;
using terraguardians;
using System.Collections.Generic;

namespace terraguardians.Companions.Castella
{
    
    public class CastellaDialogues : CompanionDialogueContainer 
    {
        public override string GreetMessages(Companion companion)
        {/*if (OnWerewolfForm(guardian))
            {
                switch (Main.rand.Next(3))
                {
                    default:
                        return "*Hello, I am " + guardian.Name + ", and I will hunt you.*";
                    case 1:
                        return "*Generally my prey doesn't try to speak to me, but at least you know that I am " + guardian.Name + ".*";
                    case 2:
                        return "*I am " + guardian.Name + ", unhappy to meet me? We'll have enough time to get acquaintanced.*";
                }
            }*/
            switch (Main.rand.Next(3))
            {
                default:
                    return "*Um... Hi.. I'm Castella";
                case 1:
                    return "*Oh... Um.. Hello,Castella is my name";
                case 2:
                    return "*Who.. Am I? I am Castella";
            }
        }

        public override string NormalMessages(Companion companion) 
        {
            List<string> Mes = new List<string>();
            Mes.Add("You came to check me?I'm not interessed in hunting you right now, if that was what you're wanting to know.");
            Mes.Add("This place is really friendly. I like that.I really don't like it when people watch me dine. Keep that in mind.");
            Mes.Add("Do I look different some nights? Sorry, I don't actually remember what happens during the nights.");
            Mes.Add("It's nice to be outside of my castle for a while.");
            Mes.Add("I hope my servants aren't thrashing my castle during my absence.");
            Mes.Add("If I catch my servants destroying my castle, they will not live to see tomorrow.");
            if (!Main.dayTime)
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("Sorry! I can't talk for much longer. I need to defend myself from hungry undeads!");
                    Mes.Add("Didn't you have a worse moment to talk to me?What? You want me to hunt you? Mind waiting for other day?");
                    Mes.Add("What are... Are you alive or dead? [nickname],Don't distract me, we have killing to do!");
                }
                else
                {
                    Mes.Add("I'm enjoying my time outside of that castle. There is more space for me to move, and hide.");
                    Mes.Add("If I catch my servants destroying my castle, they will not live to see tomorrow.");
                }
            }
            else
            {
                if (Main.eclipse)
                {
                    Mes.Add("Who made those things? I want to hire them.");
                    Mes.Add("Why all those monsters seems to be made from Terrarians? Who did that?");
                }
                else
                {
                    Mes.Add("I really missed this kind of weather when I was in my castle.");
                    Mes.Add("Those bird chirping makes this place quite noisy...");
                }
            }
            if (Main.raining && !Main.eclipse && !Main.bloodMoon)
            {
                Mes.Add("I'm feeling quite drowzy..");
                Mes.Add("I now feel like locking myself at home and have some sleep.");
                Mes.Add("This rain will not stop me from chewing something.");
            }
            

            if (WorldMod.HasCompanionNPCSpawned(terraguardians.CompanionDB.Rococo)) 
            {
                
                Mes.Add("[gn:" + terraguardians.CompanionDB.Rococo + "] keeps asking me what I'm doing, frequently");
            }
            if (WorldMod.HasCompanionNPCSpawned(terraguardians.CompanionDB.Blue))
            {
                Mes.Add("[gn:" + terraguardians.CompanionDB.Blue + "] seems to have really liked my hair.");
            }
            if (WorldMod.HasCompanionNPCSpawned(terraguardians.CompanionDB.Sardine) || WorldMod.HasCompanionNPCSpawned(terraguardians.CompanionDB.Bree))
            {
                Mes.Add("I had to apologize to [gn:" + terraguardians.CompanionDB.Sardine + "] some time ago... For what happened that night...");
            }
            
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion) //This message only appears if you speak with a companion whose friendship exp increases.
        {
            List<string> Mes = new List<string>();
            Mes.Add("As you may notice, I have a kind of lycantropy. My case is specific, since I can't remember things, but I don't end up as feral like other werewolves. That's why you can talk to me in that state.");
            Mes.Add("What am I? I'm the feral version of myself, a werewolf you'd say. We are only talking because I'm not as savage as one.");
            Mes.Add("You want to visit my castle? Sorry, but I have to object that.");
            Mes.Add("Why I was locked inside my castle on my world? It's a complicated story...");
            
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context) //Messages regarding requests. The contexts are used to let you know which cases the message will use.
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("I don't need anything right now.");
                        Mes.Add("Right now I don't need anything done.");
                        return Mes[Terraria.Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.HasRequest:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("It is really weird for me to ask, but I need your help for something. I need you to [objective], can you do it?");
                        Mes.Add("There is actually one thing I need... [objective]. Can you do that?");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.Completed:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("Thank you very much for this.");
                        Mes.Add("I'm really glad that you managed to help me with that. Thank you.");
                        return Mes[Terraria.Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.Accepted:
                    return "Please have my request completed.*\", \"*Leave me happy and I'll give you a friendly nibble.";
                case RequestContext.TooManyRequests:
                    return "I want you to focus on my task, so get your others done.";
                case RequestContext.Rejected:
                    return "Oh... Okay..*\", \"*Now I didn't liked that. Get out of my sight.";
                case RequestContext.PostponeRequest:
                    return "I have no hurry, anyways..";
                case RequestContext.Failed:
                    return "What a disappointing result, [nickname]. Should I ask someone else to help me?";
                case RequestContext.AskIfRequestIsCompleted:
                    return "My request? You completed it, right?";
                case RequestContext.RemindObjective:
                    return "You forgot?! Sigh... [objective] is what I need of you... Anything else?";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "Yes, I can live here.";
                case MoveInContext.Fail:
                    return "I don't actually want to.";
                case MoveInContext.NotFriendsEnough:
                    return "I don't know...";
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            switch(context)
            {
                case MoveOutContext.Success:
                    return "Yes...";
                case MoveOutContext.Fail:
                    return "Not a good moment for that.";
                case MoveOutContext.NoAuthorityTo:
                    return "No.";
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch(context)
            {
                case JoinMessageContext.Success:
                    return "I will join you in your travels.";
                case JoinMessageContext.FullParty:
                    return "There's too many people.";
                case JoinMessageContext.Fail:
                    return "I'm not interessed in travelling right now.";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "I'll be back to my house then.";
                case LeaveMessageContext.Fail:
                    return "Good to see that you're reasonable";
                case LeaveMessageContext.AskIfSure:
                    return "Can't I leave the group somewhere safe";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "If you say so, I'll try surviving my way back.";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "Thanks [nickname], I wasn't really wanting to leave the group.";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        

        

        //Messages for when speaking with a companion that is sleeping.
        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch(Main.rand.Next(3))
                    {
                        default:
                            return "(She seems to be sleeping soundly.)";
                        case 1:
                            return "(She's murmuring about some kind of king.)";
                        case 2:
                            return "(As you got close, she gave a sinister smile. Better you back off.)";
                    }
                case SleepingMessageContext.OnWokeUp:
                    return "[nickname], It's too early... Let me sleep some more.";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    return "[nickname], you woke me up. Did you do my request?";
            }
            return base.SleepingMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            if (Share) return "As long as you have your own bed, I don't mind sharing it with you.";
            return ".....";
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            if (Share) return "Okay, just don't let me fall.";
            return "I'll take another chair then.";
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context) //For when talking about changing their combat behavior.
        {
            switch(context)
            {
                case TacticsChangeContext.OnAskToChangeTactic:
                    return "[nickname], you want me to change how I fight?";
                case TacticsChangeContext.ChangeToCloseRange:
                    return "Yes! That's what I've been made for.";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "Alright, if you say so.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "I don't actually like that idea, but I will do as you say.";
                case TacticsChangeContext.Nevermind:
                    return "ok";
            }
            return base.TacticChangeMessage(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context) //FOr when going to speak about other things.
        {
            switch(context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "Do you want to speak about something else?";
                case TalkAboutOtherTopicsContext.AfterFirstTime:
                    return "Is there something else you want to talk about?";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "Alright.";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        
    }
}