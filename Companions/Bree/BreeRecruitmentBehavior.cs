using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Bree
{
    public class BreeRecruitmentBehavior : PreRecruitBehavior
    {
        private Companion self;
        private NpcState state = 0;
        private byte QuestionStep = 0, RightAnswers = 0;
        private bool LastAnswerWasWrong = false;

        public override bool AllowDespawning => true;
        public override string CompanionNameChange(Companion companion)
        {
            return "White Cat";
        }

        public override void Update(Companion companion)
        {
            self = companion;
            WanderAI(companion);
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
            ScoldingSardine,
            QuizFail,
            QuizSuccess
        }
    }
}