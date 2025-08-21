using Terraria;
using terraguardians;
using System.Collections.Generic;
using System;

namespace terraguardians.Companions.Castella
{
    public class CastellaDialogues : CompanionDialogueContainer 
    {
        public override string GreetMessages(Companion companion)
        {
            if ((companion as CastellaCompanion).OnWerewolfForm)
            {
                switch (Main.rand.Next(3))
                {
                    default:
                        return "*Hello, I am [name], and I will hunt you. Another time.*";
                    case 1:
                        return "*Generally my prey doesn't try to speak to me, but at least you know that I am [name].*";
                    case 2:
                        return "*I am [name], unhappy to meet me? We'll have enough time to get acquaintanced.*";
                }
            }
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
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            List<string> Mes = new List<string>();
            Action<string, string> M = new Action<string, string>(delegate(string normal, string were) { if (Wereform) { Mes.Add(were); } else { Mes.Add(normal); } });

            /*if (guardian.IsSleeping)
            {
                M("(She seems to be sleeping soundly.)", "(As you got close, she gave a sinister smile. Better you back off.)");
                M("(She's murmuring about some kind of king.)", "(She started growling as you got close. If you walk backward slowly...)");
            }*/
            if (Main.bloodMoon)
            {
                M("*Sorry! I can't talk for much longer. I need to defend myself from hungry undeads!*", "*Don't you see I'm busy killing things?*");
                M("*Didn't you have a worse moment to talk to me?*", "*What? You want me to hunt you? Mind waiting for other day?*");
                M("*What are... Are you alive or dead? Alive?*", "*Don't distract me, we have killing to do!*");
            }
            else if (Main.eclipse)
            {
                M("*Who made those things? I want to hire them.*", "*Good to have things to rip apart.*");
                M("*Why all those monsters seems to be made from Terrarians? Who did that?*", "*I'm busy turning monsters to piece.*");
            }
            else if (companion.IsUsingToilet)
            {
                M("*Yes, I have necessities too. Mind letting me finish this?*", "*If you don't leave now, I will black you out.*");
                M("*[nickname], this is not the time and place for a conversation.*", "*You really have the nerve of watching me doing my things.*");
            }
            else
            {
                M("*You came to check me?*", "*I'm not interessed in hunting you right now, if that was what you're wanting to know.*");
                M("*This place is really friendly. I like that.*", "*I really don't like it when people watch me dine. Keep that in mind.*");
                M("*Do I look different some nights? Sorry, I don't actually remember what happens during the nights.*", "*I always leave my prey alive after I finish nibbling them, that way I can try catching them again another time.*");
                M("*It's nice to be outside of my castle for a while.*", "*I'm enjoying my time outside of that castle. There is more space for me to move, and hide.*");
                M("*I hope my servants aren't thrashing my castle during my absence.*", "*If I catch my servants destroying my castle, they will not live to see tomorrow.*");

                if (Main.dayTime)
                {
                    if (!Main.raining)
                    {
                        M("*I really missed this kind of weather when I was in my castle.*", "*It's no fun to chase something during the day, so I will conserve energy for the night.*");
                        M("*Those bird chirping makes this place quite noisy...*", "*There is a reason why I preffer the night: Silence.*");
                        M("*I could surelly nap under a tree like this.*", "*I'm starting to feel a bit bored.*");
                    }
                    else
                    {
                        M("*What a horrible weather out be outside.*", "*Great. Not only it's day, but it's also raining...*");
                        M("*I always enjoyed this kind of weather when I was at my castle.*", "*I hope it isn't raining during the night..*");
                    }
                }
                else
                {
                    M("*Why are you so scared of me?*", "*Want to play a game, [nickname]? Hehehe.*");
                    if (Main.raining)
                    {
                        M("*I'm feeling quite drowzy..*", "*This rain will not stop me from chewing something.*");
                        M("*I now feel like locking myself at home and have some sleep.*", "*You're planning on going outside? Okay.*");
                    }
                    else
                    {
                        M("*Do you know why there are zombies roaming this world?*", "*I really hate it when zombies appear when I'm busy.*");
                        M("*Me? I'm just enjoying the night.*", "*Watch yourself outside, [nickname].*");
                        M("*It's so peaceful here. I like that.*", "*I should definitelly look for something to nibble.*");
                    }
                }
                if (IsPlayerRoomMate())
                {
                    M("*As long as you have your own bed, I don't mind sharing my room with you.*", "*I really enjoy having a chew toy inside my house. It's really convenient, but It's also very boring.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Rococo))
                {
                    M("*[gn:" + CompanionDB.Rococo + "] keeps asking me what I'm doing, frequently.*", "*[gn:"+CompanionDB.Rococo+"] is the only one I actually don't try catching.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Blue))
                {
                    M("*[gn:" + CompanionDB.Blue + "] seems to have really liked my hair.*", "*Aaahhh!!! Why my hair can't stay lie [gn:"+CompanionDB.Blue+"]'s when I transform?!*");
                    M("*If I am [gn:" +CompanionDB. Blue + "]'s parent? No, I'm not.*", "*I wonder why people thinkg [gn:"+CompanionDB.Blue+"] and I are parents.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Brutus))
                {
                    M("*I could use a strong bodyguard on my castle, one just like [gn:" + CompanionDB.Brutus + "].*", "*For a bodyguard, I can easily manage to blackout [gn:"+CompanionDB.Brutus+"] with ease.*");
                    M("*I don't know why [gn:" + CompanionDB.Brutus + "] is so eager for the coming of full moon nights.*", "*[gn:"+CompanionDB.Brutus+"] looks happy when I nibble him. I think is my eyes playing tricks on me.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Bree))
                {
                    M("*The other day, [gn:" + CompanionDB.Bree + "] came to me telling to stop making her husband vanish during nights, and stop using him as chew toy. I can't really do much about that.*", "*If [gn:"+CompanionDB.Bree+"] keep bothering me, I'll use her as my chewtoy next time.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Sardine))
                {
                    M("*I had to apologize to [gn:" + CompanionDB.Sardine + "] some time ago... For what happened that night...*", "*[gn:"+CompanionDB.Sardine+"] really tried to escape from me, but he didn't noticed the tree on his way.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Malisha))
                {
                    M("*[gn:" + CompanionDB.Malisha + "] is a witch? I guess this place might be fitting for me then..*", "*That hag [gn:"+CompanionDB.Malisha+"] burned my fur when I tried to catch her the other day. I will have my revenge in the future.*");
                    M("*I wonder if [gn:" + CompanionDB.Malisha + "] could help me with something... Oh, um... Nevermind.*", "*I think [gn:"+CompanionDB.Malisha+"] might actually have a use for me.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Cinnamon))
                {
                    M("*What a cute little girl [gn:" + CompanionDB.Cinnamon + "] is, and it's even more impressive that she can cook too.*", "*Whenever I see cute things like [gn:"+CompanionDB.Cinnamon+"], I want to try catching them.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Minerva))
                {
                    M("*Hm... I wonder if [gn:" + CompanionDB.Minerva + "] would agree to cook for me on my castle, as long as she stays away from me when I'm eating.*", "*Everytime I corner [gn:"+CompanionDB.Minerva+"], she buys me with food.*");
                }
                if (CanTalkAboutCompanion(CompanionDB.Liebre))
                {
                    M("*You feel uncomfortable with [gn:" + CompanionDB.Liebre + "] presence? I wondered so.*", "*It is quite creepy to notice [gn:"+CompanionDB.Liebre+"] watching me when I'm nibbling my prey. He must have been disappointed everytime I don't kill them.*");
                }
                if (NPC.downedBoss3)
                {
                    M("*Hm... Would you mind if I explore the dungeon in your world..?*", "*I'm really curious about the Dungeon. I should go there some time.*");
                }
                if (NPC.downedGoblins)
                {
                    M("*I'm glad that Goblin Army issue was taken care of... From here on it will get worser, right..?*", "*Those Goblins were quite a nice game of catch for me. I only hated that they were too easy to take care of.*");
                }
                if (Main.hardMode)
                {
                    M("*I felt that, [nickname]. Did you unleashed something onto this world?*", "*Yes! Challenge! Now that's about it! This world was too boring before.*");
                    M("*I hope you're ready for the new challenges, [nickname].*", "*What is it, [nickname]? Want to call me to rip some monsters?*");
                }
                if (NPC.downedMechBossAny)
                {
                    M("*Who built those mechanical creatures that attacked us? And why they look like creatures you faced before?*", "*I really don't like those mechanical creatures. Biting them hurts.*");
                }
                if (NPC.downedPlantBoss)
                {
                    M("*Something changed at the Dungeon, I felt it. Maybe we should check it out.*", "*Did you feel that? Something at the Dungeon. We should check out.*");
                }
                if (NPC.LunarApocalypseIsUp)
                {
                    M("*There- There are pillars! All over the world! Invoking creatures!*","*Lets go, [nickname]! There are pillars spawned in the world, and they wont be destroyed by themselves!*");
                }
                if (NPC.downedMoonlord)
                {
                    M("*It seems like you managed to bring peace here... Thank you very much...*", "*There are no more dangers popping up here, so I can focus on catching people again.*");
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion) //This message only appears if you speak with a companion whose friendship exp increases.
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            List<string> Mes = new List<string>();
            Action<string, string> M = new Action<string, string>(delegate (string normal, string were) { if (Wereform) { Mes.Add(were); } else { Mes.Add(normal); } });
            M("*As you may notice, I have a kind of lycantropy. My case is specific, since I can't remember things, but I don't end up as feral like other werewolves. That's why you can talk to me in that state.*",
                "*What am I? I'm the feral version of myself, a werewolf you'd say. We are only talking because I'm not as savage as one.*");
            M("*You want to visit my castle? Sorry, but I have to object that.*", "*You lost your mind? Letting you visit my castle? My servants would turn you into their next meal.*");
            M("*Why I was locked inside my castle on my world? It's a complicated story...*", "*I'm so glad to have found this world. This world is a lot better and bigger than the castle I live on. I hardly had anything to hunt there.*");
            M("*Now that you mentioned... I don't remember how I got here.*", "*It's really good that I jumped into that portal, or else I wouldn't be here.*");
            M("*You say that people are disappearing from town sometimes? I don't know what to say... I wonder what my other self is doing.*", "*I hope you don't mind if some of your citizens end up missing. Don't worry, they might be back by morning.*");
            M("*I don't think that my lycantrope version hunts people just to chew them.*", "*I like it when my prey tries to escape from me. It fills me with adrenaline and fun.*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context) //Messages regarding requests. The contexts are used to let you know which cases the message will use.
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch(context)
            {
                case RequestContext.NoRequest:
                    {
                        if (Main.rand.NextDouble() < 0.5f)
                            return M("*I don't need anything right now*", "*Other than something to chew, I don't need anything else.*");
                        return M("*Right now I don't need anything done.*", "*Maybe if you begin running, and then I... No? Okay.*");
                    }
                case RequestContext.HasRequest:
                    {
                        if (Main.rand.NextDouble() < 0.5f)
                            return M("*There is actually one thing I need... [objective]. Can you do that?*", "*If you help me [objective], I wont use you as my chew toy for a while. What do you say?*");
                        return M("*Yes. [objective] is what I need. What do you say?*", "*Do you think you could help me with [objective]? I have more important things to do right now.*");
                    }
                case RequestContext.Completed:
                    {
                        if (Main.rand.NextDouble() < 0.5f)
                            return M("*Thank you very much for this.*", "*That's my Terrarian. A kiss on the cheek would do for a reward?*");
                        return M("*I'm really glad that you managed to help me with that. Thank you.*", "*Congratulations [nickname]. I wont hunt you down for a while as a reward.*");
                    }
                case RequestContext.Accepted:
                    return M("*Please have my request completed.*", "*Leave me happy and I'll give you a friendly nibble.*");
                case RequestContext.TooManyRequests:
                    return M("*I want you to focus on my task, so get your others done.*", "*I don't like having to compete with others requests, take care of those before you try mine.*");
                case RequestContext.Rejected:
                    return M("*Oh... Okay..*", "*Now I didn't liked that. Get out of my sight.*");
                case RequestContext.PostponeRequest:
                    return M("*I have no hurry, anyways..*", "*Grrr... Fine.*");
                case RequestContext.Failed:
                    return M("*What a disappointing result, [nickname]. Should I ask someone else to help me?*", "*Grrr... I'm so furious that I could shred something to pieces.*");
                case RequestContext.AskIfRequestIsCompleted:
                    return M("*My request? You completed it, right?*", "*Huh? What about my request? Did you do it?*");
                case RequestContext.RemindObjective:
                    return M("*You forgot?! Sigh... [objective] is what I need of you... Anything else?*", "*You disturbed me because you forgot my request? [objective] is what I asked you to do, and is what you should be doing now. Go!*");
            }
            return base.RequestMessages(companion, context);
        }

        public override string AskCompanionToMoveInMessage(Companion companion, MoveInContext context)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch(context)
            {
                case MoveInContext.Success:
                    return M("*I think... Yeah... I think I can spend some time here...*", "*You must be nuts to ask me to stay here, but I will. If you did that to stop me from hunting you, then I will say that your plan failed.*");
                case MoveInContext.Fail:
                    return M("*I don't really have a good reason to move here right now...*", "*Now's not the moment!*");
                case MoveInContext.NotFriendsEnough:
                    return M("*I already have a place to live.*", "*I'd rather spend my time hunting things.*");
            }
            return base.AskCompanionToMoveInMessage(companion, context);
        }

        public override string AskCompanionToMoveOutMessage(Companion companion, MoveOutContext context)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch(context)
            {
                case MoveOutContext.Success:
                    return M("*You want your house back..? Okay. At least was some vacation off my castle.*", "*It's not like I would mind you taking the house off me. I'm more used to the woods anyways.*");
                case MoveOutContext.Fail:
                    return M("*No... I will stay here for a bit longer..*", "*I'll stay here a bit longer, unless you want to fight over it.*");
                case MoveOutContext.NoAuthorityTo:
                    return M("*Someone else let me stay here... Why you're trying to kick me out..?*", "*[nickname], you're not the one who let me stay here, and I have huge claws and teeths.*");
            }
            return base.AskCompanionToMoveOutMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch(context)
            {
                case JoinMessageContext.Success:
                    return M("*I will join you in your travels.*", "*You want to take me for a walk? Alright. I hope I don't scare you with what I'll do with the monsters.*");
                case JoinMessageContext.FullParty:
                    return M("*There's too many people.*", "*That group is too big. Maybe I could take one member with me, if you don't mind.*");
                case JoinMessageContext.Fail:
                    return M("*I'm not interessed in travelling right now.*", "*No way. I'd rather try catching someone, and your presence would ruin it.*");
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch(context)
            {
                case LeaveMessageContext.Success:
                    return M("*I'll be back to my house then.*", "*[nickname], remember that you're no longer safe from me.*");
                case LeaveMessageContext.Fail:
                    return M("*I'm not leaving your company now.*", "*Better I keep you within my reach for now.*");
                case LeaveMessageContext.AskIfSure:
                    return M("*Can't I leave the group somewhere safe?*", "*I don't mind going back like this, but wouldn't it be better if I left in a town?*");
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return M("*If you say so, I'll try surviving my way back.*", "*I hope you can run really fast, [nickname].*");
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return M("*Good to see that you're reasonable.*", "*You're safer for now.*");
            }
            return base.LeaveGroupMessages(companion, context);
        }

        //Messages for when speaking with a companion that is sleeping.
        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            switch(context)
            {
                case SleepingMessageContext.WhenSleeping:
                    if (Main.rand.NextBool(2))
                        return M("(She seems to be sleeping soundly.)", "(As you got close, she gave a sinister smile. Better you back off.)");
                    return M("(She's murmuring about some kind of king.)", "(She started growling as you got close. If you walk backward slowly...)");
                case SleepingMessageContext.OnWokeUp:
                    switch (Main.rand.Next(3))
                    {
                        default:
                            return M("*[nickname], it's too late to have a formal chatting. Please, be brief.*", "*What? Just because I'm like this I shouldn't sleep? Or you want me to hunt you?*");
                        case 1:
                            return M("*I'm not in the mood for chatting. What do you want?*", "*Can't I be able to get some rare moment to sleep?*");
                        case 2:
                            return M("*What do you want? It's a really dark night and you want to talk. Go sleep!*", "*[nickname], if you don't stop waking me up, I'll use you as my personal body pillow for the rest of the night.*");
                    }
                case SleepingMessageContext.OnWokeUpWithRequestActive:
                    switch (Main.rand.Next(2))
                    {
                        default:
                            return M("*W-what? Oh, is it about my request? Be brief, please.*", "*You woke me up. It is really rare of me to sleep, so I hope it's important.*");
                        case 1:
                            return M("*What is it? You did my request? You couldn't have woke me up for no reason, right?*", "*I hope It's about my request, or else I'll make you wake up with pain throughout your body.*");
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string OnToggleShareBedsMessage(Companion companion, bool Share)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (Share) M("*You w-what? O... Okay...*", "*Yes, I wouldn't mind having something to hug as I sleep. Or something to bite as I sleep.*");
            return M("*Oh... Okay... It was weird, anyways...*", "*So... I will no longer have something to hug as I sleep..?*");
        }

        public override string OnToggleShareChairMessage(Companion companion, bool Share)
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (Share) return M("*Yes, I can hold you when using a chair.*", "*Mwahahahaha... Within my reach.*");
            return M("*Okay. I shall take another chair then, when needed.*", "*Oh, fine. Like I care. Pft.*");
        }

        public override string TacticChangeMessage(Companion companion, TacticsChangeContext context) //For when talking about changing their combat behavior.
        {
            bool Wereform = (companion as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
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

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight) //Need the companion who triggered the message to be referenced.
        {
            bool Wereform = (WhoReacts as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            Weight = 1f;
            return M("*Oh, hello. I hope... You're friendly.*", "*I hope you remember my face, you will see it whenever I catch you.*");
        }

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            bool Wereform = (WhoReacts as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Malisha:
                        Weight = 1.5f;
                        return M("*I'm starting to like this already.*", "*Finally, a wise choice.*");
                    case CompanionDB.Brutus:
                        Weight = 1.5f;
                        return M("*He's going to protect me, right?*", "*I really want to put my paws on you.*");
                    case CompanionDB.Bree:
                        Weight = 1.5f;
                        return M("*I'm disliking this already...*", "*I hope you don't bother me.*");
                    case CompanionDB.Sardine:
                        Weight = 1.5f;
                        return M("*You think it's safe for him to come with me.*", "*My teeth were needing to bite something.*");
                }
            }
            Weight = 1f;
            return M("*I like having more company.*", "*Just because you joined us doesn't means you're safe from me.*");
        }

        public override string CompanionLeavesGroupMessage(Companion WhoReacts, Companion WhoLeft, out float Weight)
        {
            bool Wereform = (WhoReacts as CastellaCompanion).OnWerewolfForm;
            Func<string, string, string> M = new Func<string, string, string>(delegate (string normal, string were) { if (Wereform) { return (were); } else { return (normal); } });
            if (WhoLeft.ModID == MainMod.mod.Name)
            {
                switch (WhoLeft.ID)
                {
                    case CompanionDB.Bree:
                        Weight = 1.5f;
                        return M("*I feel relieved now.*", "*Good riddance.*");
                }
            }
            Weight = 1f;
            return M("*You gotta go? Goodbye.*", "*Watch yourself on your way back... Hehehe..*");
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch(Context)
            {
                //Alexander
                case MessageIDs.AlexanderSleuthingStart:
                    return "*Allow me to find out more about yourself...*";
                case MessageIDs.AlexanderSleuthingProgress:
                    return "*Hm... Interesting...*";
                case MessageIDs.AlexanderSleuthingNearlyDone:
                    return "*You're a carnival of scents...*";
                case MessageIDs.AlexanderSleuthingFinished:
                    return "*Okay, that's enough information about you.*";
                case MessageIDs.AlexanderSleuthingFail:
                    return "*Uh oh. I'm in trouble, am I?*";
            }
            return base.GetOtherMessage(companion, Context);
        }
    }
}