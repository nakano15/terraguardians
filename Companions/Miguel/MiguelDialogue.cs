using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions.Miguel
{
    public class MiguelDialogue : CompanionDialogueContainer
    {
        public override string UnlockAlertMessages(Companion companion, UnlockAlertMessageContext context)
        {
            switch (context)
            {
                case UnlockAlertMessageContext.FollowUnlock:
                    return "*Hey, you! I need to find out if you are doing the exercises correctly. If you need me to train you personally, all you need is just to call. I can help you on your adventure too, on the way.*";
                case UnlockAlertMessageContext.MountUnlock:
                    return "*You look in bad shape, [nickname]. I need some weight on my arm to keep it's muscle strong, so you may help me with that.*";
                case UnlockAlertMessageContext.ControlUnlock:
                    return "*I believe there are some things you may not be able to do yourself. I can do it for you, as long as you keep doing your exercises.*";
            }
            return base.UnlockAlertMessages(companion, context);
        }

        public override string GreetMessages(Companion companion)
        {
            switch (Main.rand.Next(3))
            {
                default:
                    return "*I see that you need some help making your muscles show up. Gladly I can help you with that.*";
                case 1:
                    return "*It seems like you have been eating more than burning fat. I will prepare some exercises for you to do.*";
                case 2:
                    return "*How are you able to use that weapon with those thin arms? Time to make you grow some muscles in those arms.*";
            }
        }

        public override string NormalMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            if (companion.IsUsingToilet)
            {
                Mes.Add("*[nickname], this is not the place and time for talking. If you want another training, or just to talk, you could wait until I'm done with my things.*");
                Mes.Add("*I think this toilet is going to overflow... Ah... Wh.. [nickname]! When did you appeared?*");
                Mes.Add("*Yes, I do my business like anyone else. You watching me is making it harder for me to finish this.*");
            }
            else if (Main.eclipse)
            {
                Mes.Add("*Don't overdo yourself, [nickname]. Sometimes you must retreat.*");
                Mes.Add("*Those creatures looks challenging. Let's hope the exercises I gave you helps.*");
                Mes.Add("*You can defend yourself today, right?*");
            }
            else if (Main.bloodMoon)
            {
                Mes.Add("*Practicing your arm through the night, [nickname]? I hope you have enough energy to spend.*");
                Mes.Add("*You know, this moment is perfect for doing your thing: Killing hostile creatures.*");
                Mes.Add("*I wonder if the moon being red is because of all the killing you're doing.*");
            }
            else
            {
                if(PlayerMod.IsHauntedByFluffles(MainMod.GetLocalPlayer) && Main.rand.NextDouble() < 0.75)
                {
                    Mes.Add("*She's not gonna jump onto my shoulder, right?*");
                }
                else
                {
                    Mes.Add("*My objective regarding you is to make you grow some muscle, not to make you like me, unless you want.*");
                    Mes.Add("*When doing exercises, you must feel that you are making effort during it. Do not mistake that by pain, because if you do, stop right away.*");
                    Mes.Add("*If you're feeling pain during your exercises, stop right away what you're doing and seek me. You may end up injuring yourself that way.*");
                    Mes.Add("*You're ready for the next exercise?*");
                    Mes.Add("*Just because you're doing exercises doesn't means you must eat poorly or light food. You need energy to burn, so eat a moderate plate of food. If you don't have energy, you'll pass out. Got it?*");
                    Mes.Add("*I wonder if my wife will visit this world some day.*");
                    Mes.Add("*Only because you get stronger doesn't means you get stupidier. The same is valid for the inverse. Keep that on mind.*");

                    if (MainMod.GetLocalPlayer.HasBuff(ModContent.BuffType<Buffs.Fit>()))
                    {
                        Mes.Add("*I can already see some muscles on your body. Nice job.*");
                        Mes.Add("*Aren't you feeling better now that you are fit, [nickname]?*");
                        Mes.Add("*I see that you have been doing exercises frequently. That's really good.*");
                    }
                    else
                    {
                        Mes.Add("*You look a bit skinny right now. I can fix that with daily exercises specially for you.*");
                        Mes.Add("*Why your belly has more volume than the rest of your body? Let's change that.*");
                        Mes.Add("*Have you been eating many chips and junk food? Let's convert that fat into muscles.*");
                    }

                    if (Main.dayTime)
                    {
                        if (!Main.raining)
                        {
                            Mes.Add("*The weather looks perfect.*");
                            Mes.Add("*How are you doing? Came to get some advices?*");
                            Mes.Add("*Wait. Take a deep breath. Nice. It's good, isn't?*");
                        }
                        else
                        {
                            Mes.Add("*I love the peace the rain brings.*");
                            Mes.Add("*Fearing a few rain drops, [nickname]? Or are you fearing catching flu?*");
                            Mes.Add("*I really hope nothing ruins this weather. I really wanted to stay at home right now.*");
                        }
                    }
                    else
                    {
                        if (!Main.raining)
                        {
                            Mes.Add("*Preparing yourself for a rest, [nickname]? Don't forget to eat something before sleeping.*");
                            Mes.Add("*If you're feeling depleted, don't feel bad about falling on a bed and sleeping. Your muscles need time to recover from the exercises.*");
                            Mes.Add("*Yawn~ What? I'm not made of rock either, I also feel sleepy during night.*");
                        }
                        else
                        {
                            Mes.Add("*I will love sleeping with the sound of rain drops and chill weather.*");
                            Mes.Add("*Can't sleep, [nickname]? Let the rain help you fall asleep.*");
                            Mes.Add("*Seeking an more training? I was wanting to enjoy the weather.*");
                        }
                    }

                    if (CanTalkAboutCompanion(CompanionDB.Rococo))
                    {
                        Mes.Add("*Can you help me convince [gn:"+CompanionDB.Rococo+"] to do some exercises?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Sardine))
                    {
                        Mes.Add("*I'm having to spend some time trying to convert [gn:"+CompanionDB.Sardine+"]'s belly fat into muscles.*");
                        Mes.Add("*[gn:"+CompanionDB.Sardine+"] is already fast, even though he is quite fat. Wonder how faster he will get once he gets muscles.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Zacks))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Zacks+"] asked me to give him some exercises for strengthening himself, but his body is decaying. I doubt there will be any effect if he did.*");
                        Mes.Add("*Why [gn:"+CompanionDB.Zacks+"] sometimes stares at me, like as if was seeing a beef?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Alex))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Alex+"] said that wanted to get stronger to protect you. I gave him some exercises that may work on him.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Brutus))
                    {
                        Mes.Add("*Do you know why [gn:"+CompanionDB.Brutus+"] left arm is stronger than his right arm?*");
                        Mes.Add("*I need to find ways of controlling [gn:"+CompanionDB.Brutus+"] stomach. He's creates more fat than being able to burn it.*");
                        Mes.Add("*I don't think [gn:"+CompanionDB.Brutus+"] needs much advices from me. I challenged him to a arm wrestling and lost. But I will try to invest into making him stronger.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Bree))
                    {
                        Mes.Add("*You wouldn't believe how strong [gn:"+CompanionDB.Bree+"] is. I recommended her some weights to lift as exercise, and she only felt something when she got ones with double the weight.*");
                        Mes.Add("*You think [gn:"+CompanionDB.Bree+"] is what made her stronger? I don't think so. She seems to be strong herself. She must do something that strains her muscles.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Malisha))
                    {
                        Mes.Add("*Did you knew that [gn:"+CompanionDB.Malisha+"] asked me to give her some exercises for her tail? I wonder what is that for.*");
                        Mes.Add("*I really hate it when [gn:"+CompanionDB.Malisha+"] calls me a walking beef.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Leopold))
                    {
                        Mes.Add("*[gn:"+CompanionDB.Leopold+"] keeps refusing my proposal of training him. He keeps saying that would rather watch slimes procreate.*");
                        Mes.Add("*It's funny how for someone who uses magical weapons, [gn:" + CompanionDB.Leopold + "] charges full on melee when his mana runs out.*");
                        Mes.Add("*If you see [gn:" + CompanionDB.Leopold + "], can you try convincing him to do some exercises?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Mabel))
                    {
                        Mes.Add("*It's quite complicated to train [gn:" + CompanionDB.Mabel + "]. She makes me sweat... A lot....*");
                        Mes.Add("*Normally I would ask [gn:"+CompanionDB.Mabel+"] out for a date, but I'm a married man, so I wont.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Vladimir))
                    {
                        Mes.Add("*I don't even need to tell [gn:"+CompanionDB.Vladimir+"] to carry some weight and grow muscle, since eventually he's carrying someone.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Alexander))
                    {
                        Mes.Add("*I asked [gn:"+CompanionDB.Alexander+"] why his torax and legs are so fit, and he said that was because of running from many ghosts.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Fluffles))
                    {
                        Mes.Add("*You really have some weird people living here. [gn:"+CompanionDB.Fluffles+"] gives me some chills, beside she seems nice.*");
                        Mes.Add("*The other day, [gn:"+CompanionDB.Fluffles+"] was on my shoulder when I went to visit my wife. Things didn't ended very well.*");
                    }
                    if(CanTalkAboutCompanion(CompanionDB.Minerva))
                    {
                        Mes.Add("*I'm giving [gn:"+CompanionDB.Minerva+"] some rigorous exercises to make her lose fat.*");
                        Mes.Add("*I can wonder how much [gn:"+CompanionDB.Minerva+"] likes to taste her own food, but that is very unhealthy for her.*");
                        Mes.Add("*If you're hungry, go visit [gn:"+CompanionDB.Minerva+"] so she gives you something for you to eat. That way you'll refill your energy.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Liebre))
                    {
                        Mes.Add("*I can't believe. [gn:" + CompanionDB.Liebre + "] actually asked me for some exercises he cold do. I hope his plasma shell actually gets stronger.*");
                        Mes.Add("*Seeing [gn:" + CompanionDB.Liebre + "] watching around, makes me feel like someone is about to die soon. It's creepy.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Leona))
                    {
                        Mes.Add("*I'd love seeing [gn:"+CompanionDB.Leona+"] turn all that fat into muscles. I'd love to have some competition on work outs.*");
                        Mes.Add("*I don't think the amount of fibers [gn:" + CompanionDB.Leona + "] eats day by day even gets any use.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Quentin))
                    {
                        Mes.Add("*[gn:" + CompanionDB.Quentin + "] may have really strong willpower, but he need to have stronger arm and body too. Nobody wants to be knocked out by a punch in the face, right?*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Wrath))
                    {
                        Mes.Add("*[gn:" + CompanionDB.Wrath + "] is very hostile, I'm neglecting to ask them to train.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Fear))
                    {
                        Mes.Add("*I don't get why [gn:" + CompanionDB.Fear + "] fears me so much? Is it my exercises or my appearance? Probably my exercises. I should try explaining them to them and see if they get less scared.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.CaptainStench))
                    {
                        Mes.Add("*For some reason, [gn:" + CompanionDB.CaptainStench + "] said she would do exercise if she gets some materialistic reward as compensation. Can you believe that? As if being fit wasn't a reward in itself.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Luna))
                    {
                        Mes.Add("*I really would love seeing fit [gn:" + CompanionDB.Luna + "]... I mean... I... I mean... Err... Forget it.*");
                    }
                    if (CanTalkAboutCompanion(CompanionDB.Green))
                    {
                        Mes.Add("*Hey [nickname]. With the help of [gn:"+CompanionDB.Green+"], I can give exercise to people, while he helps with the diet and nutrients. Doesn't that seem good?*");
                    }
                    if (companion.IsPlayerRoomMate(MainMod.GetLocalPlayer))
                    {
                        Mes.Add("*I'm okay with sharing my room with you. I hope my morning exercises don't end up waking you.*");
                        Mes.Add("*Sorry if I sound really beaten during the night, but I really get depleted when I go sleep, so I literally sleep like a log.*");
                    }
                }
            }
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string TalkMessages(Companion companion)
        {
            List<string> Mes = new List<string>();
            Mes.Add("*My wife is currently unsure about visiting this world. She doesn't think it's safe.*");
            Mes.Add("*I've been exercising my muscles since 15, and learned about methods to exercise them at 17. It has been a long road.*");
            Mes.Add("*People tend to avoid me because they think I will only speak to them about exercises, but I also speak about other things.*");
            Mes.Add("*Some day I'll introduce you to my wife. I'm sure you'll like her since the greeting.*");
            Mes.Add("*Why some of your citizens want to mount on my back? Do I look like a chariot?*");
            return Mes[Main.rand.Next(Mes.Count)];
        }

        public override string RequestMessages(Companion companion, RequestContext context)
        {
            switch (context)
            {
                case RequestContext.NoRequest:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*I don't have anything I need right now, but if you want some exercises, I can make up something for you.*";
                    return "*No, I don't need anything done right now. Do you want to speak about something else?*";

                case RequestContext.HasRequest:
                    {
                        /*if (companion.request.IsTravelRequest && Main.rand.NextDouble() < 0.5)
                        {
                            return "*I need to exercise my legs, and I think you can help me with that. Can you [objective]?*";
                        }
                        if (!companion.request.IsTravelRequest && Main.rand.NextDouble() < 0.5)
                            return "*This isn't an exercise, I really need this done. I'm busy with some other things, and can't [objective] right now. Can you do it for me, instead?*";*/
                        return "*I can give you a break of training. There is a thing that I need done, which is [objective]. Can you fulfill that?*";
                    }

                case RequestContext.Completed:
                    if (Main.rand.NextDouble() < 0.5)
                        return "*Perfect. That is exactly what I wanted done. Nice job.*";
                    return "*That's great. You can take some rest before returning to your things.*";

                case RequestContext.Accepted:
                    return "*Thank you, [nickname]. Tell me when you got the request done.*";
                case RequestContext.TooManyRequests:
                    return "*By overdoing yourself, I also talk about not stressing yourself with many tasks to do at once. Solve your other friends requests first before taking mine. Health is a serious thing.*";
                case RequestContext.Rejected:
                    return "*I see... When I get a free time, I will see to it myself. You're relieved, [nickname].*";
                case RequestContext.PostponeRequest:
                    return "*I guess it can wait.*";
                case RequestContext.Failed:
                    return "*I'm disappointed, [nickname], but I wont punish you with exercises for that.*";
                case RequestContext.AskIfRequestIsCompleted:
                    return "*Tell me, [nickname]. You did my request?*";
                case RequestContext.RemindObjective:
                    return "*Short fused memory? I asked you to [objective]. Maybe I can recommend you some vitamines for memory.*";
                case RequestContext.CancelRequestAskIfSure:
                    return "*Do you really want to cancel my request?*";
                case RequestContext.CancelRequestYes:
                    return "*I'm disappointed at you, [nickname]. If you couldn't do it, why you accepted?*";
                case RequestContext.CancelRequestNo:
                    return "*Good. Return to me if you manage to fulfill it.*";
            }
            return base.RequestMessages(companion, context);
        }

        public override string TalkAboutOtherTopicsMessage(Companion companion, TalkAboutOtherTopicsContext context)
        {
            switch (context)
            {
                case TalkAboutOtherTopicsContext.FirstTimeInThisDialogue:
                    return "*Want to know about something? Feel free to ask.*";
                case TalkAboutOtherTopicsContext.Nevermind:
                    return "*Then, what else you want to talk about?*";
            }
            return base.TalkAboutOtherTopicsMessage(companion, context);
        }

        public override string ReviveMessages(Companion companion, Player target, ReviveContext context)
        {
            switch (context)
            {
                case ReviveContext.HelpCallReceived:
                    return "*You seems to have exagerated on your exploration, but whatever. Let me see how bad that is.*";
                case ReviveContext.RevivingMessage:
                    switch (Main.rand.Next(4))
                    {
                        default: return "*You overdid yourself. Let me take a look at that wound.*";
                        case 1: return "*It's just a flesh wound. You'll be walking soon.*";
                        case 2: return "*Rest for a while, I'll take care of those wounds.*";
                        case 3: return "*Gladly I know a bit of first aid.*";
                    }
                case ReviveContext.ReviveWithOthersHelp:
                    return "*Thank you for that, I really mean it.*";
                case ReviveContext.RevivedByItself:
                    return "*Ouch, ouch... I'm back... I'm back..*";
                case ReviveContext.ReachedFallenAllyMessage:
                    return "*Alright, got you. Now I need to keep you safe from more harm.*";
            }
            return base.ReviveMessages(companion, target, context);
        }

        public override string SleepingMessage(Companion companion, SleepingMessageContext context)
        {
            switch (context)
            {
                case SleepingMessageContext.WhenSleeping:
                    switch (Main.rand.Next(3))
                    {
                        default: return "*(Sleep talking) Pull... Push... Pull... Push... Now do that 10 more times...*";
                        case 1: return "(He's snoring quite loud. It seems like he really blacked out.)";
                        case 2: return "*Stop! You're going strain a tendon... (He's dreaming about training someone)*";
                    }
            }
            return base.SleepingMessage(companion, context);
        }

        public override string JoinGroupMessages(Companion companion, JoinMessageContext context)
        {
            switch (context)
            {
                case JoinMessageContext.Success:
                    return "*Yes. While you travel, I can oversee your exercises.*";
                case JoinMessageContext.FullParty:
                    return "*There's a lot of people with you already. I can't seem to fit in that.*";
                case JoinMessageContext.Fail:
                    return "*No. I have my own things to do right now.*";
            }
            return base.JoinGroupMessages(companion, context);
        }

        public override string LeaveGroupMessages(Companion companion, LeaveMessageContext context)
        {
            switch (context)
            {
                case LeaveMessageContext.AskIfSure:
                    return "*I don't mind walking the way back to home, but the monsters are what worries me. Are you sure that you want to ditch me here?*";
                case LeaveMessageContext.Success:
                    return "*I will stay then. Try not to overdo yourself on your exercises, [nickname].*";
                case LeaveMessageContext.Fail:
                    return "*You're not escaping my personal training that easily, [nickname]. You still have more to go.*";
                case LeaveMessageContext.DangerousPlaceYesAnswer:
                    return "*Well, time to see how good I am at running.*";
                case LeaveMessageContext.DangerousPlaceNoAnswer:
                    return "*Then let's find a place with a friendly person before I leave.*";
            }
            return base.LeaveGroupMessages(companion, context);
        }

        public override string CompanionMetPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Leopold:
                        Weight = 1.2f;
                        return "*Hey, nerd guy, time to gain some muscles.*";
                    case CompanionDB.Rococo:
                        Weight = 1.2f;
                        return "*You need to work out that chest.*";
                    case CompanionDB.Brutus:
                        Weight = 1.2f;
                        return "*Amazing, we just met a keg of ale.*";
                    case CompanionDB.Malisha:
                        Weight = 1.2f;
                        return "*Interessed in exercising your glutes, lady?*";
                    case CompanionDB.Mabel:
                        Weight = 1.2f;
                        return "*Oh my... T-that... Physique...*";
                    case CompanionDB.Vladimir:
                        Weight = 1.2f;
                        return "*Well, that's one big guy. I think we can make you get some muscles.*";
                    case CompanionDB.Domino:
                        Weight = 1.2f;
                        return "*Your upper body will need some exercises, but your legs seems fit.*";
                    case CompanionDB.Leona:
                        Weight = 1.2f;
                        return "*Nice to meet you. Want to lose that belly?*";
                }
            }
            Weight = 1f;
            return "*You're getting more popular, [nickname].*";
        }

        public override string CompanionJoinPartyReactionMessage(Companion WhoReacts, Companion WhoJoined, out float Weight)
        {
            if(WhoJoined.ModID == MainMod.mod.Name)
            {
                switch (WhoJoined.ID)
                {
                    case CompanionDB.Minerva:
                    case CompanionDB.Brutus:
                    case CompanionDB.Vladimir:
                        Weight = 1.5f;
                        return "*Let's try burning that fat.*";
                    case CompanionDB.Malisha:
                        Weight = 1.5f;
                        return "*Time to gain some muscles, witch.*";
                    case CompanionDB.Mabel:
                        Weight = 1.2f;
                        return "*Y-yes, good to see you too.*";
                    case CompanionDB.Leona:
                        Weight = 1.2f;
                        return "*Ready to lose that belly, [gn:"+CompanionDB.Leona+"]?*";
                }
            }
            Weight = 1f;
            return "*You're getting more popular, [nickname].*";
        }

        public override string GetOtherMessage(Companion companion, string Context)
        {
            switch (Context)
            {
                case MessageIDs.LeopoldMessage1:
                    return "*Murmuring... Murmuring... Murmuring...*";
                case MessageIDs.LeopoldMessage2:
                    return "*Hey! Don't mock me. Why are you following that Terrarian?*";
                case MessageIDs.LeopoldMessage3:
                    return "*Pft. I'm their personal trainer, and that Terrarian is not stupid, they heard everything you said.*";
                case MessageIDs.VladimirRecruitPlayerGetsHugged:
                    return "*That just got weird.*";
            }
            return base.GetOtherMessage(companion, Context);
        }

        public override void ManageLobbyTopicsDialogue(Companion companion, MessageDialogue dialogue)
        {
            GetExercisesDialogues(companion, dialogue);
        }

        #region Exercises Related
        void GetExercisesDialogues(Companion companion, MessageDialogue dialogue)
        {
            ExerciseTypes exercise = (companion.Data as MiguelData).ExerciseType;
            if (exercise == ExerciseTypes.None)
            {
                dialogue.AddOption("Do you have any exercise I can do?", GiveExerciseAction);
            }
            else
            {
                string Message = "What is my exercise progress?";
                if (exercise == ExerciseTypes.WaitUntilNextDay)
                    Message = "Do you have any other exercise for me?";
                else if ((companion.Data as MiguelData).ExerciseCounter <= 0)
                    Message = "I have completed the exercise.";
                dialogue.AddOption(Message, CheckExerciseButtonAction);
            }
        }

        void GiveExerciseAction()
        {
            MiguelData data = Dialogue.Speaker.Data as MiguelData;
            string Message = data.GiveNewExercise();
            MessageDialogue md = new MessageDialogue(Message);
            md.RunDialogue();
            MiguelBase.OnCheckForAttackExercise();
        }

        void CheckExerciseButtonAction()
        {
            TerraGuardian Miguel = Dialogue.Speaker as TerraGuardian;
            MiguelData Data = Miguel.Data as MiguelData;
            if (Data.ExerciseType == ExerciseTypes.WaitUntilNextDay)
            {
                MessageDialogue md = new MessageDialogue("*There is no other exercise for you today. For your muscles to recover from today's exercise, wait until tomorrow.*");
                md.RunDialogue();
            }
            else if (Data.ExerciseCounter <= 0)
            {
                int FitBuffID = ModContent.BuffType<Buffs.Fit>();
                int NewBuffTime = 30 * 60 * 60;
                Player player = MainMod.GetLocalPlayer;
                if (player.HasBuff(FitBuffID))
                {
                    int index = player.FindBuffIndex(FitBuffID);
                    NewBuffTime += player.buffTime[index];
                    const int MaxBuffTime = 60 * 60 * 60;
                    if (NewBuffTime > MaxBuffTime)
                        NewBuffTime = MaxBuffTime;
                }
                player.AddBuff(FitBuffID, NewBuffTime);
                Data.ExercisesDone++;
                MessageDialogue md = new MessageDialogue();
                if (Data.ExercisesDone % 10 == 0)
                {
                    Miguel.IncreaseFriendshipPoint(1);
                    md.ChangeMessage("*Good job, [nickname]. You have really impressed me those days. Let your muscles take a rest until tomorrow and then I will give you another exercise.*");
                }
                else
                {
                    md.ChangeMessage("*Good job, [nickname]. Now take a rest and return to me tomorrow for another exercise.*");
                }
                MiguelBase.DeleteRequestData();
                Data.ExerciseType = ExerciseTypes.WaitUntilNextDay;
                md.RunDialogue();
            }
            else
            {
                MessageDialogue md = new MessageDialogue();
                switch (Data.ExerciseType)
                {
                    case ExerciseTypes.AttackTimes:
                        md.ChangeMessage("*I tasked you into attacking anything a number of times. It seems like you still need to hit anything "+Data.ExerciseCounter+" times. I'm sure you know how you will do that.*");
                        break;
                    case ExerciseTypes.JumpTimes:
                        md.ChangeMessage("*I told you to jump a number of times. You still need to jump "+Data.ExerciseCounter+" more times to complete this exercise.*");
                        break;
                    case ExerciseTypes.TravelDistance:
                        md.ChangeMessage("*You need to travel "+(int)(Data.ExerciseCounter * 0.5f)+" feets more to complete this exercise.*");
                        break;
                }
                md.RunDialogue();
            }
        }
        #endregion
    }
}