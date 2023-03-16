using System;
using Terraria;
using Terraria.ModLoader;
using Terraria.GameContent.UI;

namespace terraguardians.Companions.Bree
{
    public class BreeRecruitmentBehavior : PreRecruitBehavior
    {
        private Companion self;
        private NpcState state = 0;
        private byte QuestionStep = 0, RightAnswers = 0, RepetitionTimes = 0;
        private bool InterruptedOnce = false;
        private byte DialogueStep { get { return QuestionStep; } set { QuestionStep = value; } }
        private byte MessageTime { get { return RightAnswers; } set { RightAnswers = value; } }
        private bool LastAnswerWasWrong = false;
        private Player TargetPlayer = null;
        private Companion Sardine = null, Glenn = null;
        private bool HasBree = false;

        public override bool AllowDespawning => true;
        public override string CompanionNameChange(Companion companion)
        {
            return "White Cat";
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return state != NpcState.PlayingScene;
        }

        public override void Update(Companion companion)
        {
            self = companion;
            if(Target == null)
            {
                Target = ViewRangeCheck(companion, companion.direction);
                if (Target != null)
                {
                    HasBree = PlayerMod.PlayerHasCompanion(Target, CompanionDB.Bree);
                    Sardine = PlayerMod.PlayerGetSummonedCompanion(Target, CompanionDB.Sardine);
                    Glenn = PlayerMod.PlayerGetSummonedCompanion(Target, CompanionDB.Glenn);
                    if (HasBree || Sardine != null || Glenn != null)
                    {
                        state = NpcState.PlayingScene;
                        QuestionStep = 0;
                        RightAnswers = 0;
                    }
                }
            }
            else
            {
                if(state == NpcState.PlayingScene)
                {
                    UpdateScene(companion);
                    return;
                }
            }
            WanderAI(companion);
        }

        private void UpdateScene(Companion Bree)
        {
            if (MessageTime == 0)
            {
                if (Glenn != null)
                {
                    Companion NewGlenn = PlayerMod.PlayerGetSummonedCompanion(Target, CompanionDB.Glenn);
                    if (NewGlenn != null)
                    {
                        Glenn = NewGlenn;
                        if (DialogueStep >= (byte)SceneIds.BreePersuadesThePlayerToCallHimBack && DialogueStep <= (byte)SceneIds.BreePersuadesThePlayerALittleCloser)
                        {
                            if (InterruptedOnce)
                            {
                                ChangeScene(SceneIds.SardineIsCalledBackByPlayerAfterInterruption);
                            }
                            else
                            {
                                ChangeScene(SceneIds.PlayerCalledSardineBackAfterBreeAsked);
                            }
                        }
                    }
                    else
                    {
                        if (DialogueStep >= (byte)SceneIds.GlennAnswer && DialogueStep < (byte)SceneIds.BreeJoinsToTakeCareOfGlenn)
                        {
                            InterruptedOnce = true;
                            ChangeScene(SceneIds.PlayerUnsummonedGlenn);
                        }
                    }
                }
                else if (Sardine != null)
                {
                    Companion NewSardine = PlayerMod.PlayerGetSummonedCompanion(Target, CompanionDB.Sardine);
                    if (NewSardine != null)
                    {
                        Sardine = NewSardine;
                        if (DialogueStep >= (byte)SceneIds.BreePersuadesThePlayerToCallHimBack && DialogueStep <= (byte)SceneIds.BreePersuadesThePlayerALittleCloser)
                        {
                            if (InterruptedOnce)
                            {
                                ChangeScene(SceneIds.SardineIsCalledBackByPlayerAfterInterruption);
                            }
                            else
                            {
                                ChangeScene(SceneIds.PlayerCalledSardineBackAfterBreeAsked);
                            }
                        }
                    }
                    else
                    {
                        if (DialogueStep >= (byte)SceneIds.SardineStaysAndTalksToBree && DialogueStep < (byte)SceneIds.BreeTurnsToTownNpc)
                        {
                            InterruptedOnce = true;
                            ChangeScene(SceneIds.PlayerUnsummonedSardine);
                        }
                    }
                }
                if(Bree.chatOverhead.timeLeft == 0 && (Sardine == null || Sardine.chatOverhead.timeLeft == 0) && (Glenn == null || Glenn.chatOverhead.timeLeft == 0))
                {
                    switch((SceneIds)DialogueStep)
                    {
                        case 0:
                            if (HasBree)
                            {
                                ChangeScene(SceneIds.BreeSeesFriendlyFace);
                                state = NpcState.PlayingScene;
                            }
                            else if (Sardine != null && Glenn != null)
                            {
                                Bree.SaySomething("Are they... Glenn! Sardine!");
                                ChangeScene(SceneIds.BreeSpotsSardineAndGlenn);
                                state = NpcState.PlayingScene;
                            }
                            else if (Glenn != null)
                            {
                                Bree.SaySomething("Gl... Glenn?!");
                                ChangeScene(SceneIds.GlennSpotted);
                                state = NpcState.PlayingScene;
                            }
                            else if (Sardine != null)
                            {
                                Bree.SaySomething("Sardine! I finally found you!");
                                ChangeScene(1);
                                state = NpcState.PlayingScene;
                            }
                            break;
                        case SceneIds.SardineSpotted:
                            if (Sardine.FriendshipLevel < 5)
                            {
                                ChangeScene(SceneIds.SardineFlees);
                                Sardine.SaySomething("Uh oh, I gotta go.");
                            }
                            else
                            {
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                                Sardine.SaySomething("Uh oh.");
                            }
                            break;
                        case SceneIds.SardineFlees:
                            if (Sardine.active)
                            {
                                PlayerMod.PlayerDismissCompanion(Target, CompanionDB.Sardine);
                            }
                            Bree.SaySomething("What? Where did he go?");
                            ChangeScene(SceneIds.BreeAsksWhereSardineWent);
                            break;
                        case SceneIds.BreeAsksWhereSardineWent:
                            Bree.SaySomething("You, call him back. Do so.");
                            ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBack);
                            break;
                        case SceneIds.BreePersuadesThePlayerToCallHimBack:
                            Bree.SaySomething("Call him back. NOW!");
                            ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBackAgain);
                            break;
                        case SceneIds.BreePersuadesThePlayerToCallHimBackAgain:
                            Bree.SaySomething("I said... NOW!!!!");
                            ChangeScene(SceneIds.BreePersuadesThePlayerALittleCloser);
                            break;
                        case SceneIds.BreePersuadesThePlayerALittleCloser:
                            {
                                string Message = "";
                                switch (RepetitionTimes)
                                {
                                    case 0:
                                        Message = "Call him! JUST. CALL. HIM!";
                                        break;
                                    case 1:
                                        Message = "You are making me angry.... EVEN MORE ANGRY!!!";
                                        break;
                                    case 2:
                                        Message = "GRRRRRRR!!! RRRRRRRRRRRRRRRR!!! RRRRRRRRRRRRRRGHHHH!!";
                                        break;
                                    case 3:
                                        Message = "MY PATIENCE IS ALREADY GOING DOWN THE DRAIN! CALL HIM BACK, NOW!!";
                                        break;
                                    case 4:
                                        Message = "ENOUGH!! CALL HIM, NOW!";
                                        ChangeScene(SceneIds.BreeForcedPlayerToCallSardineBack);
                                        break;
                                }
                                if (DialogueStep == (byte)SceneIds.BreePersuadesThePlayerALittleCloser && RepetitionTimes < 4)
                                {
                                    ChangeScene(SceneIds.BreePersuadesThePlayerALittleCloser);
                                    RepetitionTimes++;
                                }
                                Bree.SaySomething(Message);
                            }
                            break;
                        case SceneIds.BreeForcedPlayerToCallSardineBack:
                            {
                                if (Sardine != null)
                                {
                                    PlayerMod.PlayerCallCompanion(Target, CompanionDB.Sardine);
                                }
                                Bree.SaySomething("Good.");
                                if (Sardine != null && Glenn != null)
                                {
                                    ChangeScene(SceneIds.BreeAsksWhereWasSardine);
                                }
                                else if (Glenn != null)
                                {
                                    ChangeScene(SceneIds.GlennSpotted);
                                }
                                else
                                {
                                    ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                                }
                            }
                            break;
                        case SceneIds.SardineStaysAndTalksToBree:
                            {
                                Sardine.SaySomething("H-Hello dear, how have you been latelly?");
                                ChangeScene(SceneIds.BreeScoldsSardine);
                            }
                            break;
                        case SceneIds.BreeScoldsSardine:
                            {
                                Bree.SaySomething("Seriously?! That is all you have to say?");
                                ChangeScene(SceneIds.BreeContinues);
                            }
                            break;
                        case SceneIds.BreeContinues:
                            {
                                Bree.SaySomething("You abandon me and your son at home and that is what you have to say?");
                                ChangeScene(SceneIds.SardineTriesToApologise);
                            }
                            break;
                        case SceneIds.SardineTriesToApologise:
                            {
                                Sardine.SaySomething("I'm sorry?");
                                ChangeScene(SceneIds.BreeDoesntAcceptTheApology);
                            }
                            break;
                        case SceneIds.BreeDoesntAcceptTheApology:
                            {
                                Bree.SpawnEmote(EmoteID.EmotionAnger, 150);
                                Bree.SaySomething("That is it? You numbskull, you've been missing for " + 2 + " WHOLE YEARS!!");
                                ChangeScene(SceneIds.BreeContinuesAfterNotAcceptingTheApology);
                            }
                            break;
                        case SceneIds.BreeContinuesAfterNotAcceptingTheApology:
                            {
                                Bree.SaySomething("Do you know how many worlds I travelled trying to find you! Even the planet of the tentacles I had to travel through!");
                                ChangeScene(SceneIds.SardineTriesToApologiseAgain);
                            }
                            break;
                        case SceneIds.SardineTriesToApologiseAgain:
                            {
                                Sardine.SaySomething("I already said that I'm sorry. I... Kind of forgot what world we lived so... I couldn't return.");
                                //Show sweat emote
                                Sardine.SpawnEmote(Terraria.GameContent.UI.EmoteID.EmotionCry, 150);
                                ChangeScene(SceneIds.BreeTalksAboutTakingSardineBackWithHer);
                            }
                            break;
                        case SceneIds.BreeTalksAboutTakingSardineBackWithHer:
                            {
                                Bree.SaySomething("Then there is no problem, since YOU are going back home with ME!");
                                ChangeScene(SceneIds.SardineTriesTheButs);
                            }
                            break;
                        case SceneIds.SardineTriesTheButs:
                            {
                                Sardine.SaySomething("But... But... I have a job here and...");
                                ChangeScene(SceneIds.BreeSaysNoToButs);
                            }
                            break;
                        case SceneIds.BreeSaysNoToButs:
                            {
                                Bree.SaySomething("No buts! We are going back now! I just need to remember which world we-");
                                ChangeScene(SceneIds.BreeTakesAPauseToRealizeSheForgotToo);
                            }
                            break;
                        case SceneIds.BreeTakesAPauseToRealizeSheForgotToo:
                            {
                                Bree.SaySomething("...");
                                Bree.SpawnEmote(EmoteID.EmotionCry, 150);
                                ChangeScene(SceneIds.BreeForgotTheWorldTheyLived);
                            }
                            break;
                        case SceneIds.BreeForgotTheWorldTheyLived:
                            {
                                Bree.SaySomething("This can't be happening... I forgot which world we lived on!");
                                ChangeScene(SceneIds.SardineLaughsOfBree);
                            }
                            break;
                        case SceneIds.SardineLaughsOfBree:
                            {
                                Sardine.SaySomething("Haha! Looks like you'll have to stay here for a while, until we remember.");
                                Sardine.SpawnEmote(EmoteID.EmoteLaugh, 150);
                                ChangeScene(SceneIds.BreeAgrees);
                            }
                            break;
                        case SceneIds.BreeAgrees:
                            {
                                Bree.SaySomething("Grrr... Alright! I'll stay for a while, but only until I remember the world we lived!");
                                ChangeScene(SceneIds.BreeIntroducesHerself);
                            }
                            break;
                        case SceneIds.BreeIntroducesHerself:
                            {
                                Bree.SaySomething("My name is Bree, don't expect me to stay for long.");
                                ChangeScene(SceneIds.BreeTurnsToTownNpc);
                            }
                            break;
                        case SceneIds.BreeTurnsToTownNpc:
                            {
                                PlayerMod.PlayerAddCompanion(Target, CompanionDB.Bree);
                                WorldMod.AddCompanionMet(CompanionDB.Bree);
                                Target = null;
                                Sardine = null;
                                Glenn = null;
                            }
                            return;
                            
                        case SceneIds.SardineIsCalledBackByPlayer:
                            {
                                Bree.SaySomething("There he is.");
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                            }
                            break;
                            
                        case SceneIds.SardineIsCalledBackByPlayerAfterInterruption:
                            {
                                Bree.SaySomething("Don't do that again!");
                                ChangeScene(SceneIds.BreeTalksAboutSardineGoingBackWithHer);
                            }
                            break;
                        case SceneIds.BreeTalksAboutSardineGoingBackWithHer:
                            {
                                Bree.SaySomething("We are returning to home right now!");
                                ChangeScene(SceneIds.SardineTriesTheButs);
                            }
                            break;
                            
                        case SceneIds.PlayerUnsummonedSardine:
                            {
                                Bree.SaySomething("Hey! We were talking!");
                                InterruptedOnce = true;
                                ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBack);
                            }
                            break;
                            
                        case SceneIds.PlayerCalledSardineBackAfterBreeAsked:
                            {
                                Bree.SaySomething("Thank you. Now where was I?");
                                ChangeScene(SceneIds.SardineStaysAndTalksToBree);
                            }
                            break;
                            
                        case SceneIds.BreeSeesFriendlyFace:
                            {
                                Bree.SaySomething("Oh, It's you again.");
                                ChangeScene(SceneIds.BreeSaysHowSheAppearedThere);
                            }
                            break;
                        case SceneIds.BreeSaysHowSheAppearedThere:
                            {
                                Bree.SaySomething("I'm still looking for the world I lived, but It's funny to bump into you on the way.");
                                ChangeScene(SceneIds.BreeJoinsYou);
                            }
                            break;
                        case SceneIds.BreeJoinsYou:
                            {
                                Bree.SaySomething("Anyway, I'm here, If you need me.");
                                ChangeScene(SceneIds.BreeTurnsToTownNpc);
                            }
                            break;
                            
                        case SceneIds.GlennSpotted:
                            {
                                Bree.SaySomething("Glenn! What are you doing here? You should be at home.");
                                ChangeScene(SceneIds.GlennAnswer);
                            }
                            break;
                        case SceneIds.GlennAnswer:
                            {
                                Glenn.SaySomething("You and Dad were taking too long to come home, so I came looking for you two.");
                                ChangeScene(SceneIds.AsksGlennIfHesOkay);
                            }
                            break;
                        case SceneIds.AsksGlennIfHesOkay:
                            {
                                Bree.SaySomething("But It's dangerous out there! Are you hurt?");
                                ChangeScene(SceneIds.GlennSaysThatIsFine);
                            }
                            break;
                        case SceneIds.GlennSaysThatIsFine:
                            {
                                Glenn.SaySomething("I'm okay, don't worry. This Terrarian let me stay here, and It's safe here.");
                                ChangeScene(SceneIds.BreeJoinsToTakeCareOfGlenn);
                            }
                            break;
                        case SceneIds.BreeJoinsToTakeCareOfGlenn:
                            {
                                Bree.SaySomething("I can't let you stay here alone, I shouldn't have let you stay alone at home either. I'll stay here to take care of you, and look for your father.");
                                ChangeScene(SceneIds.BreeIntroducesHerself);
                            }
                            break;
                            
                        case SceneIds.BreeSpotsSardineAndGlenn:
                            {
                                Bree.SaySomething("Are you two alright?");
                                ChangeScene(SceneIds.BothAnswers);
                            }
                            break;
                        case SceneIds.BothAnswers:
                            {
                                Sardine.SaySomething("Yes, we're fine.");
                                Glenn.SaySomething("I'm okay.");
                                ChangeScene(SceneIds.BreeAsksWhereWasSardine);
                            }
                            break;
                        case SceneIds.BreeAsksWhereWasSardine:
                            {
                                Bree.SaySomething("I'm so glad you two are fine. Sardine, where did you go? Why didn't you returned home?");
                                ChangeScene(SceneIds.SardineAnswers);
                            }
                            break;
                        case SceneIds.SardineAnswers:
                            {
                                Sardine.SaySomething("I was trying to find treasures for you two... And then I was saved by that Terrarian from a bounty hunt that gone wrong.");
                                ChangeScene(SceneIds.BreeThenFeelsRelievedAndAsksGlennWhatIsHeDoingHere);
                            }
                            break;
                        case SceneIds.BreeThenFeelsRelievedAndAsksGlennWhatIsHeDoingHere:
                            {
                                Bree.SaySomething("I think I should think you then, Terrarian. Now Glenn, I told you to wait for us at home!");
                                ChangeScene(SceneIds.GlennAnswers);
                            }
                            break;
                        case SceneIds.GlennAnswers:
                            {
                                Glenn.SaySomething("I stayed at home, but I was worried that you two didn't returned yet, so I explored worlds trying to find you two.");
                                ChangeScene(SceneIds.BreeSuggestsToSpendSomeTimeTogetherBeforeReturningToTheWorld);
                            }
                            break;
                        case SceneIds.BreeSuggestsToSpendSomeTimeTogetherBeforeReturningToTheWorld:
                            {
                                Bree.SaySomething("That's really reckless and dangerous, but I'm glad you two are unharmed. Let's spend a little time here and then return home.");
                                ChangeScene(SceneIds.SardineSaysThatForgotWhereTheWorldIsAt);
                            }
                            break;
                        case SceneIds.SardineSaysThatForgotWhereTheWorldIsAt:
                            {
                                Sardine.SaySomething("I hope you know our way home, because I forgot.");
                                ChangeScene(SceneIds.GlennAlsoSaysThatForgot);
                            }
                            break;
                        case SceneIds.GlennAlsoSaysThatForgot:
                            {
                                Glenn.SaySomething("I also forgot my way back home, sorry mom.");
                                ChangeScene(SceneIds.BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen);
                            }
                            break;
                        case SceneIds.BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen:
                            {
                                Bree.SaySomething("I can't remember either.... Well, I hope you don't mind if we stay here for longer, Terrarian.");
                                ChangeScene(SceneIds.BreeIntroducesHerself);
                            }
                            break;
                            
                        case SceneIds.PlayerUnsummonedGlenn:
                            {
                                Bree.SaySomething("Where did you sent my son? Call him back now!");
                                InterruptedOnce = true;
                                ChangeScene(SceneIds.BreePersuadesThePlayerToCallHimBack);
                            }
                            break;
                    }
                    MessageTime = 180;
                }
            }
            else
            {
                MessageTime--;
            }
            ChangeFollowTargetByScene(Bree);
            MakeDialogueActorsFaceBree(Bree);
        }

        private void MakeDialogueActorsFaceBree(Companion Bree)
        {
            if (Glenn != null && Glenn.velocity.X == 0) Glenn.FaceSomething(Bree);
            if (Sardine != null && Sardine.velocity.X == 0) Sardine.FaceSomething(Bree);
        }

        private void ChangeFollowTargetByScene(Companion Bree)
        {
            Player Follow = Target;
            if (Sardine != null && Sardine.active)
                Follow = Sardine;
            if (Follow != null)
            {
                float BreeCenterX = Bree.Center.X, ToFollowCenterX = Follow.Center.X;
                float Distance = Math.Abs(Bree.Center.X - Follow.Center.X);
                if (Distance > 40)
                {
                    if (BreeCenterX > ToFollowCenterX)
                        Bree.MoveLeft = true;
                    else
                        Bree.MoveRight = true;
                    Bree.WalkMode = Distance > 80;
                }
                else if (Distance < 20)
                {
                    if (BreeCenterX < ToFollowCenterX)
                        Bree.MoveLeft = true;
                    else
                        Bree.MoveRight = true;
                    Bree.WalkMode = true;
                }
                else if (Bree.velocity.X == 0)
                {
                    if (BreeCenterX > ToFollowCenterX)
                        Bree.direction = -1;
                    else
                        Bree.direction = 1;
                }
            }
        }

        private void ChangeScene(SceneIds scene)
        {
            ChangeScene((byte)scene);
        }

        private void ChangeScene(byte NewScene)
        {
            DialogueStep = NewScene;
            MessageTime = 180;
        }

        public override MessageBase ChangeStartDialogue(Companion companion)
        {
            QuestionStep = 0;
            RightAnswers = 0;
            return Questions();
        }

        public MessageBase Questions()
        {
            switch(state)
            {
                case NpcState.QuizFail:
                    return GetFailStartDialogue();
                case NpcState.QuizSuccess:
                    return GetSuccessRecruitStartDialogue();
            }
            return Questions(false);
        }

        public MessageBase Questions(bool wrong)
        {
            MessageDialogue md = new MessageDialogue();
            switch(QuestionStep)
            {
                case 0:
                    {
                        md.ChangeMessage("Hello, have you seen a black cat around?");
                        md.AddOption("Why are you looking for a black cat?", CorrectAnswer);
                        string WrongAnswerMessage = "No, I haven't.";
                        if(Main.rand.Next(2) == 0)
                            md.AddOption(WrongAnswerMessage, WrongAnswer);
                        else
                            md.AddOptionAtTop(WrongAnswerMessage, WrongAnswer);
                    }
                    break;
                case 1:
                    {
                        if (!wrong)
                            md.ChangeMessage("The black cat is my husband, he's been missing for quite some time.");
                        else
                            md.ChangeMessage("I need to find that black cat. I looked everywhere and couldn't find him.");
                        md.AddOption("I can help you find him.", CorrectAnswer);
                        string WrongAnswerMessage = "Have you looked on the other towns?";
                        if(Main.rand.Next(2) == 0)
                            md.AddOption(WrongAnswerMessage, WrongAnswer);
                        else
                            md.AddOptionAtTop(WrongAnswerMessage, WrongAnswer);
                        break;
                    }
                case 2:
                    {
                        if (!wrong)
                            md.ChangeMessage("Thank you, but I'm not sure if he may be found in this world, but I really need all the help I can find to locate him.");
                        else
                            md.ChangeMessage("I didn't yet, this place is huge, and I've been travelling through several worlds. I'm kind of tired.");
                        md.AddOption("Any tips on how I can find him?", CorrectAnswer);
                        string WrongAnswerMessage = "What about forgetting him?";
                        if(Main.rand.Next(2) == 0)
                            md.AddOption(WrongAnswerMessage, WrongAnswer);
                        else
                            md.AddOptionAtTop(WrongAnswerMessage, WrongAnswer);
                        break;
                    }
                case 3:
                    {
                        if (!wrong)
                            md.ChangeMessage("He's an adventurer, so probably he's doing something dangerous or stupid. That also makes me worried, what If he's dead? No, wait, I shouldn't be pessimistic.");
                        else
                            md.ChangeMessage("What?! No! I can't do that! I really need to find him. If the worst didn't happened to him.");
                        md.AddOption("Don't give up.", CorrectAnswer);
                        string WrongAnswerMessage = "Maybe the worst has happened.";
                        if(Main.rand.Next(2) == 0)
                            md.AddOption(WrongAnswerMessage, WrongAnswer);
                        else
                            md.AddOptionAtTop(WrongAnswerMessage, WrongAnswer);
                        break;
                    }
                case 4:
                    {
                        if (!wrong)
                            md.ChangeMessage("You're right, I have to keep looking. But I'm getting worn out after so much travelling.");
                        else
                            md.ChangeMessage("You are awful! But maybe you are right? I don't know, all I know is that I'm tired.");
                        md.AddOption("Take a rest before you search for him.", CorrectAnswer);
                        string WrongAnswerMessage = "Why don't you stay here?";
                        if(Main.rand.Next(2) == 0)
                            md.AddOption(WrongAnswerMessage, WrongAnswer);
                        else
                            md.AddOptionAtTop(WrongAnswerMessage, WrongAnswer);
                        break;
                    }
            }
            return md;
        }

        public void CorrectAnswer()
        {
            RightAnswers++;
            QuestionStep++;
            LastAnswerWasWrong = false;
            if (QuestionStep < 5)
                Questions(false).RunDialogue();
            else
                ResultDialogue();
        }

        public void WrongAnswer()
        {
            QuestionStep++;
            LastAnswerWasWrong = true;
            if (QuestionStep < 5)
                Questions(true).RunDialogue();
            else
                ResultDialogue();
        }

        public void ResultDialogue()
        {
            MessageDialogue md = new MessageDialogue();
            const string Name = "Bree";
            if (!LastAnswerWasWrong)
            {
                if (RightAnswers >= 4)
                {
                    md.ChangeMessage("You know, you're right. I'll stay for a while and see If he shows up. I can recover some of my strength meanwhile.\nBy the way, my name is " + Name + ".");
                }
                else
                {
                    md.ChangeMessage("No way, I have to keep looking for him. I'll just stay here for a while to recover some strength.");
                }
            }
            else
            {
                if (RightAnswers >= 4)
                {
                    md.ChangeMessage("Actually, you're right, I'll stay here for a while. I'll try gathering information around, to see If I have clues of where he went. \nMy name is " + Name + ".");
                }
                else
                {
                    md.ChangeMessage("I will do, I still have some places to look for him.");
                }
            }
            if (RightAnswers >= 4)
            {
                state = NpcState.QuizSuccess;
                md.AddOption("Feel free to stay.", RecruitBree);
            }
            else
            {
                state = NpcState.QuizFail;
                md.AddOption("Good luck.", Dialogue.EndDialogue);
            }
            md.RunDialogue();
        }

        public void RecruitBree()
        {
            PlayerMod.PlayerAddCompanion(MainMod.GetLocalPlayer, self);
            WorldMod.AddCompanionMet(self);
            Dialogue.LobbyDialogue();
        }

        public MessageBase GetSuccessRecruitStartDialogue()
        {
            MessageDialogue md = new MessageDialogue("Thank you so much for speaking with me. I should keep trying to look for him around.");
            md.AddOption("Feel free to.", RecruitBree);
            return md;
        }

        public MessageBase GetFailStartDialogue()
        {
            MessageDialogue md = new MessageDialogue("I'm just taking a rest before leaving. Don't worry.");
            md.AddOption("Alright.", Dialogue.EndDialogue);
            return md;
        }

        enum NpcState : byte
        {
            Normal,
            PlayingScene,
            QuizFail,
            QuizSuccess
        }
        public enum SceneIds : byte
        {
            NoScene = 0,
            SardineSpotted,
            SardineFlees,
            BreeAsksWhereSardineWent,
            BreePersuadesThePlayerToCallHimBack,
            BreePersuadesThePlayerToCallHimBackAgain,
            BreePersuadesThePlayerALittleCloser,
            BreeForcedPlayerToCallSardineBack,

            SardineStaysAndTalksToBree = 50,
            BreeScoldsSardine,
            BreeContinues,
            SardineTriesToApologise,
            BreeDoesntAcceptTheApology,
            BreeContinuesAfterNotAcceptingTheApology,
            SardineTriesToApologiseAgain,
            BreeTalksAboutTakingSardineBackWithHer,
            SardineTriesTheButs,
            BreeSaysNoToButs,
            BreeTakesAPauseToRealizeSheForgotToo,
            BreeForgotTheWorldTheyLived,
            SardineLaughsOfBree,
            BreeAgrees,
            BreeIntroducesHerself,
            BreeTurnsToTownNpc,

            SardineIsCalledBackByPlayer = 100,
            SardineIsCalledBackByPlayerAfterInterruption,
            PlayerUnsummonedSardine,
            BreeTalksAboutSardineGoingBackWithHer,
            PlayerCalledSardineBackAfterBreeAsked,

            BreeSeesFriendlyFace = 150,
            BreeSaysHowSheAppearedThere,
            BreeJoinsYou,

            GlennSpotted = 200,
            GlennAnswer,
            AsksGlennIfHesOkay,
            GlennSaysThatIsFine,
            BreeJoinsToTakeCareOfGlenn,

            PlayerUnsummonedGlenn,

            BreeSpotsSardineAndGlenn,
            BothAnswers,
            BreeAsksWhereWasSardine,
            SardineAnswers,
            BreeThenFeelsRelievedAndAsksGlennWhatIsHeDoingHere,
            GlennAnswers,
            BreeSuggestsToSpendSomeTimeTogetherBeforeReturningToTheWorld,
            SardineSaysThatForgotWhereTheWorldIsAt,
            GlennAlsoSaysThatForgot,
            BreeThenSaysThatTheyShouldStayInTheWorldForAWhileThen
        }
    }
}