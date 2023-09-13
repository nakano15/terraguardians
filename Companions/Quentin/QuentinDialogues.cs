using Terraria;
using terraguardians;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    
    public class QuentinDialogues : CompanionDialogueContainer 
    {
        public override string GreetMessages(Companion companion) 
        {
            return "let's discover together the mysteries of magic.";
        }

        public override string NormalMessages(Companion companion) 
        {
            List<string> Mes = new List<string>();
            Mes.Add("since I joined you I feel that I have become more powerful, if we continue like this soon we will be unstoppable.");
            Mes.Add("my master was a great sorcerer, I miss him a lot since I got here.");
            Mes.Add("what? What do you mean I look like a clown?.");
            Mes.Add("I am a bunny, not a rabbit or a hare, learn to distinguish them.");
            Mes.Add("this hat and this robe were gifts from my Master for my birthday.");
            Mes.Add("I am amazed at the amount of mysteries that still remain to be unveiled in this new world.");
            if (!Main.dayTime)
            {
                if (Main.bloodMoon)
                {
                    Mes.Add("Did the moon just turn red? I hope nothing bad happens tonight");
                    Mes.Add("What are those scary things? as if zombies weren't bad enough.");
                    Mes.Add("even the water looks scary tonight.");
                }
                
            }
            else
            {
                if (Main.eclipse)
                {
                    Mes.Add("The only thing that terrifies me more than a dark night is when it's just as dark during the day.");
                    Mes.Add("You saw the size of that thing, I hope it doesn't come any closer to us.");
                }
               
            }
            
           
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Guide)) 
            {
                Mes.Add("That man always dresses in such a boring way, wasn't there more striking clothes in his closet?.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Merchant))
            {
                Mes.Add("I love that old man, he is very nice to me and he sells that blue juice that makes me feel like I regain my powers with every sip.");
            }
            if (NPC.AnyNPCs(Terraria.ID.NPCID.Wizard))
            {
                Mes.Add("Finally i find another wizard here, I hope that he is willing to teach me more about magic.");
            }

           

           
           
           
            if (WorldMod.HasCompanionNPCSpawned(terraguardians.CompanionDB.Leopold))
            {
                Mes.Add("when i asked leopold if he wanted to help me my magic show, he didn't speak to me for a week, I don't know why he got so upset.");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion) 
        {
            List<string> Mes = new List<string>();
            Mes.Add("I hate the beach, the sand gets everywhere and my books end up ruined with the mix of sand and water.");
            Mes.Add("Once i tried to fish but i caught a tuna so big that when i tried to get him out of the water it almost swallowed me in one bite.");
            Mes.Add("when I grow up I will open a magic store or a bookstore or better yet, a magic bookstore");
            Mes.Add("I only take baths in the lakes and rivers, the sea water makes my body completely bristle, also in the sea are sharks.");
            
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context) 
        {
            switch(context)
            {
                case RequestContext.NoRequest:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("I don't need anything yet I'm just enjoying this adventure.");
                        Mes.Add("I don't need your help right now, don't worry.");
                        return Mes[Terraria.Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.HasRequest:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("I want you to [objective] for me. Its for research purposes.");
                        Mes.Add("I have a mission for you, and I don't mind helping you with it.. [objective].");
                        return Mes[Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.Completed:
                    {
                        List<string> Mes = new List<string>();
                        Mes.Add("I always knew that you could achieve it without problem.");
                        Mes.Add("Wonderful, I never doubt that you could make it.");
                        return Mes[Terraria.Main.rand.Next(Mes.Count)];
                    }
                case RequestContext.Accepted:
                    return "Awesome.";
                case RequestContext.TooManyRequests:
                    return "No. you look too busy to take care of this.";
                case RequestContext.Rejected:
                    return "I hope you can help me another time then.";
                case RequestContext.PostponeRequest:
                    return "don't worry I'm not in a hurry.";
                case RequestContext.Failed:
                    return "I never expected this result, well there will always be more chances to achieve it.";
                case RequestContext.AskIfRequestIsCompleted:
                    return "I sense that you did my request. Am I right?";
                case RequestContext.RemindObjective:
                    return "Don't worry! The great [name] will remind you of your objective. You have to [objective].";
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            switch(context)
            {
                case MoveInContext.Success:
                    return "Yes, I can live here";
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
                    return "Yes";
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
                    return "Of, course, let me grab my things";
                case JoinMessageContext.FullParty:
                    return "No, i hate crowds.";
                case JoinMessageContext.Fail:
                    return "i am busy now.";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return "No problem then.";
                case LeaveMessageContext.Fail:
                    return "Not right now.";
                case LeaveMessageContext.AskIfSure:
                    return "Are you sure about that? i thought we were having fun.";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "I know the way";
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
                            return "zzz";
                        
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
            if (Share) return "Fine. Try not being greedy and take my share of the bed.";
            return "I hope there's another bed for me.";
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
                    return "Ok";
                case TacticsChangeContext.ChangeToMidRanged:
                    return "Alright, if you say so.";
                case TacticsChangeContext.ChangeToLongRanged:
                    return "Ok";
                case TacticsChangeContext.Nevermind:
                    return "???";
                case TacticsChangeContext.FollowAhead:
                    return "F-follow ahead? I hope no skeleton catch me again.";
                case TacticsChangeContext.FollowBehind:
                    return "Finally!";
                case TacticsChangeContext.AvoidCombat:
                    return "T-that doesn't look like a good idea.";
                case TacticsChangeContext.PartakeInCombat:
                    return "G-great. I can use magic then.";
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