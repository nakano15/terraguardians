using System.Collections.Generic;
using Terraria;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using nterrautils;

namespace terraguardians.Dialogues.Games
{
    public class RPSDialogue
    {
        static byte RPSChoice = 0, PlayerVictories = 0, CompanionVictories = 0;
        const byte RPS_Rock = 0, RPS_Paper = 1, RPS_Scissors = 2;
        public static void OnAskToPlayRPS()
        {
            PlayerVictories = 0;
            CompanionVictories = 0;
            StartRPSGame(Dialogue.Speaker.GetDialogues.GetOtherMessage(Dialogue.Speaker, MessageIDs.RPSAskToPlaySuccess));
        }

        private static void StartRPSGame(string Message)
        {
            MessageDialogue md = new MessageDialogue();
            md.ChangeMessage(Message + "\n" + Dialogue.GetTranslation("rpspickoptionprompt"));
            RPSChoice = 0;
            md.AddOption(Dialogue.GetTranslation("rpsrockoption"), RPSPickRock);
            md.AddOption(Dialogue.GetTranslation("rpspaperoption"), RPSPickPaper);
            md.AddOption(Dialogue.GetTranslation("rpsscissorsoption"), RPSPickScissors);
            md.RunDialogue();
        }

        private static void RPSPickRock(){ RPSPickOption(0); }

        private static void RPSPickPaper(){ RPSPickOption(1); }

        private static void RPSPickScissors(){ RPSPickOption(2); }

        private static void RPSPickOption(byte Choice)
        {
            RPSChoice = (byte)(Choice % 3);
            TimedDialogue md = new TimedDialogue(Dialogue.GetTranslation("rpsshout"), 3);
            md.ChangeNextStep(RPSShowResult);
            md.RunDialogue();
        }

        private static void RPSShowResult()
        {
            byte CompanionChoice = (byte)Main.rand.Next(3);
            byte Result = 0;
            const byte Result_Win = 1, Result_Tie = 0, Result_Lose = 2;
            switch (RPSChoice)
            {
                case RPS_Rock:
                    if (CompanionChoice == RPS_Scissors)
                    {
                        Result = Result_Win;
                    }
                    else if (CompanionChoice == RPS_Rock)
                    {
                        Result = Result_Tie;
                    }
                    else
                    {
                        Result = Result_Lose;
                    }
                    break;
                case RPS_Paper:
                    if (CompanionChoice == RPS_Rock)
                    {
                        Result = Result_Win;
                    }
                    else if (CompanionChoice == RPS_Paper)
                    {
                        Result = Result_Tie;
                    }
                    else
                    {
                        Result = Result_Lose;
                    }
                    break;
                case RPS_Scissors:
                    if (CompanionChoice == RPS_Paper)
                    {
                        Result = Result_Win;
                    }
                    else if (CompanionChoice == RPS_Scissors)
                    {
                        Result = Result_Tie;
                    }
                    else
                    {
                        Result = Result_Lose;
                    }
                    break;
            }
            string ResultMessage = "";
            string OptionMessage = "";
            switch (Result)
            {
                case Result_Win:
                    ResultMessage = Dialogue.Speaker.GetDialogues.GetOtherMessage(Dialogue.Speaker, MessageIDs.RPSCompanionLoseMessage);
                    OptionMessage = Dialogue.GetTranslation("rpswinoption");
                    PlayerVictories++;
                    break;
                case Result_Lose:
                    ResultMessage = Dialogue.Speaker.GetDialogues.GetOtherMessage(Dialogue.Speaker, MessageIDs.RPSCompanionWinMessage);
                    OptionMessage = Dialogue.GetTranslation("rpsloseoption");
                    CompanionVictories++;
                    break;
                case Result_Tie:
                    ResultMessage = Dialogue.Speaker.GetDialogues.GetOtherMessage(Dialogue.Speaker, MessageIDs.RPSCompanionTieMessage);
                    OptionMessage = Dialogue.GetTranslation("rpstieoption");
                    break;
            }
            ResultMessage += "\n" + Dialogue.GetTranslation("rpsresult");
            switch (RPSChoice)
            {
                case RPS_Rock:
                    ResultMessage = ResultMessage.Replace("[playerresult]", Dialogue.GetTranslation("rpsrockresult"));
                    break;
                case RPS_Paper:
                    ResultMessage = ResultMessage.Replace("[playerresult]", Dialogue.GetTranslation("rpspaperresult"));
                    break;
                case RPS_Scissors:
                    ResultMessage = ResultMessage.Replace("[playerresult]", Dialogue.GetTranslation("rpsscissorsresult"));
                    break;
            }
            switch (CompanionChoice)
            {
                case RPS_Rock:
                    ResultMessage = ResultMessage.Replace("[opponentresult]", Dialogue.GetTranslation("rpsrockresult"));
                    break;
                case RPS_Paper:
                    ResultMessage = ResultMessage.Replace("[opponentresult]", Dialogue.GetTranslation("rpspaperresult"));
                    break;
                case RPS_Scissors:
                    ResultMessage = ResultMessage.Replace("[opponentresult]", Dialogue.GetTranslation("rpsscissorsresult"));
                    break;
            }
            MessageDialogue md = new MessageDialogue(ResultMessage);
            md.AddOption(OptionMessage, RPSAskIfWantToPlayAgain);
            md.RunDialogue();
        }

        static void RPSAskIfWantToPlayAgain()
        {
            MessageDialogue md = new MessageDialogue(Dialogue.GetTranslation("rpsvictoriescount")
                .Replace("[playervictories]", PlayerVictories.ToString())
                .Replace("[opponentvictories]", CompanionVictories.ToString()));
            md.AddOption(Dialogue.GetTranslation("genericyes"), RPSRestartGame);
            md.AddOption(Dialogue.GetTranslation("genericno"), RPSEndGame);
            md.RunDialogue();
        }

        static void RPSRestartGame()
        {
            StartRPSGame(Dialogue.Speaker.GetDialogues.GetOtherMessage(Dialogue.Speaker, MessageIDs.RPSPlayAgainMessage));
        }

        static void RPSEndGame()
        {
            Dialogue.LobbyDialogue(Dialogue.Speaker.GetDialogues.GetOtherMessage(Dialogue.Speaker, MessageIDs.RPSEndGameMessage));
        }
    }
}