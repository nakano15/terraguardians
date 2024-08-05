using Terraria;
using terraguardians;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    
    public class QuentinDialogues : CompanionDialogueContainer 
    {
        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch (context)
            {
                case UnlockAlertMessageContext.ControlUnlock:
                    return "If you really need it, I can lead the group.";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string GreetMessages(Companion companion) 
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "let's discover together the mysteries of magic.";
                case 1:
                    return "I may still be an apprentice but I assure you I can be of help.";
                case 2:
                    return "I am a mage's apprentice trying to gather more knowledge.";
            }
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
            
            switch(Main.invasionType)
            {
                case Terraria.ID.InvasionID.PirateInvasion:
                    Mes.Add("I always knew that the Pale Pirate would come seeking revenge for stealing his treasure.");
                    Mes.Add("don't let the Pale Pirate capture me please.");
                    break;
            }
            /*if(companion.GetTileCount(Terraria.ID.TileID.MinecartTrack) > 0)
            {
                Mes.Add("this reminds me when me and my master got on the Tricky Train to try to stop it before it fell off a ravine.");
            }*/
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
            if (Terraria.GameContent.Events.BirthdayParty.PartyIsUp)
            {
                Mes.Add("I really love parties, nothing better than filling my stomach with cake.");
            }
            bool Forest = companion.ZoneOverworldHeight;
            if (companion.ZoneHallow)
            {
                Forest = false;
                Mes.Add("Hmm those colorful trees look like cotton candy, can I try one?.");
                Mes.Add("I tried to pet one of those unicorns and it almost attacked me with its horn.");
            }
            if(companion.ZoneCorrupt || companion.ZoneCrimson)
            {
                Forest = false;
                Mes.Add("this place looks even more scary than forest on night.");
                Mes.Add("I feel very observed and not in a good way.");
            }
            if (companion.ZoneSnow)
            {
                Forest = false;
                Mes.Add("with this cold my mustache freezes.");
                Mes.Add("Brrr... my robe is not thick enough to shelter me from this cold.");
            }
            if (companion.ZoneJungle)
            {
                Forest = false;
                Mes.Add("why are we here? It is full of carnivorous plants and bats everywhere.");
            }
            if (companion.ZoneDungeon)
            {
                Mes.Add("this place only brings back bad memories.");
            }
            if (Forest)
            {
                Mes.Add("I always try to talk to the rabbits around here but they seem to be very shy.");
            }
            if(Main.raining)
            {
                Mes.Add("If I can't find a place to shelter from this rain, my robe will shrink.");
            }
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion) 
        {
            List<string> Mes = new List<string>();
            if (!Main.dayTime)
                Mes.Add("You know, i am afraid of being alone in a dark night, thats why i try to collect as many glowsticks when i see a jellyfish or buy some to that merchant old man.");
            if (companion.statManaMax > 20)
                Mes.Add("I can sense your magic power is growing stronger, at least share some of those crystals the next time you have some to spare, after all i need to grow stronger too to achieve my objectives.");
            Mes.Add("I hate the beach, the sand gets everywhere and my books end up ruined with the mix of sand and water.");
            Mes.Add("Once i tried to fish but i caught a tuna so big that when i tried to get him out of the water it almost swallowed me in one bite.");
            Mes.Add("when I grow up I will open a magic store or a bookstore or better yet, a magic bookstore");
            Mes.Add("I only take baths in the lakes and rivers, the sea water makes my body completely bristle, also in the sea are sharks.");
            Mes.Add("I heard that there are caves in this world where everything glows a deep blue color and giant mushrooms grow, you must take me to see that.");
            if (CanTalkAboutCompanion(CompanionDB.Miguel))
                Mes.Add("That horse keep saying that i am weakling, when he is gonna get that i am not a fighter,i am a wizard, i train my mind, not my body.");
            if (CanTalkAboutCompanion(CompanionDB.Minerva))
                Mes.Add("I love that minerva joined us, she cooks really well and make some really tasty cookies for the tea time.");
            if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
                Mes.Add("How i can I explain to cinnamon that I don't hate her but the spice of the same name?");
            return Mes[Terraria.Main.rand.Next(Mes.Count)];
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch (context)
            {
                case ReviveContext.RevivingMessage:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return "You will be back.";
                        case 1:
                            return "I'll take care of you, don't worry.";
                        case 2:
                            return "You look terrible, let me help you.";
                    }
                case ReviveContext.ReviveWithOthersHelp:
                    return "Thanks i feel a lot better now.";
                case ReviveContext.RevivedByItself:
                    return "I thought i wasn't gonna make it.";
                case ReviveContext.HelpCallReceived:
                    return "I'm going to try to heal you, don't move too much.";
            }
            return base.ReviveMessages(companion, target, context);
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
                case SleepingMessageContext.OnWokeUp:
                    if(Main.rand.NextFloat() < 0.5f)
                        return "good morning friend, a new day means a new adventure.";
                    return "ooh, i was dreaming about candies and chocolate.";
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    if (Main.rand.NextFloat() < 0.5f)
                        return "Did you complete my task?";
                    return "What about my quest?";
            }
            return base.SleepingMessage(companion, context);
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