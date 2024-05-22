using System;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class MessageBase
    {
        public virtual void OnDialogueTrigger()
        {
            
        }

        public void RunDialogue()
        {
            Dialogue.ChangeMessage(this);
        }

        public void RunThisMessage()
        {
            Dialogue.ChangeMessage(this);
        }
    }

    public class MessageDialogue : MessageBase
    {
        public string MessageText = "";
        public Companion Speaker = null;
        public List<DialogueOption> Options = new List<DialogueOption>();
        public MessageDialogue(string Message = "", DialogueOption[] options = null)
        {
            MessageText = Message;
            if(options != null) this.Options.AddRange(options);
        }

        public void ChangeSpeaker(Companion Speaker)
        {
            this.Speaker = Speaker;
        }

        public void ChangeMessage(string NewMessage, Companion Speaker = null)
        {
            MessageText = NewMessage;
            if (Speaker != null) this.Speaker = Speaker;
        }

        public void AddOptionAtTop(string Text, Action Result)
        {
            Options.Insert(0, new DialogueOption(Text, Result));
        }

        public void AddOption(string Text, Action Result)
        {
            Options.Add(new DialogueOption(Text, Result));
        }

        public void AddOption(DialogueOption NewOption)
        {
            Options.Add(NewOption);
        }

        public override void OnDialogueTrigger()
        {
            Dialogue.ChangeDialogueMessage(MessageText);
            Dialogue.ChangeCurrentSpeaker(Speaker);
            if(Options.Count == 0)
                Dialogue.ChangeOptions(Dialogue.GetDefaultCloseDialogue);
            else
                Dialogue.ChangeOptions(Options);
        }
    }

    public class MultiStepDialogue : MessageBase
    {
        private byte CurrentStep = 0;
        public List<DialogueStep> Steps = new List<DialogueStep>();
        public List<DialogueOption> Options = new List<DialogueOption>();
        private DialogueOption[] DummyOption = new DialogueOption[]{ new DialogueOption("???", ProceedDialogue) };

        public MultiStepDialogue()
        {

        }

        public MultiStepDialogue(string[] DialogueMessages, Companion Speaker = null)
        {
            foreach(string m in DialogueMessages)
            {
                AddDialogueStep(m, Speaker: Speaker);
            }
        }

        public override void OnDialogueTrigger()
        {
            SetCurrentMessageStep();
        }

        public void AddDialogueStep(string Message, string ProceedText = "Continue", Companion Speaker = null)
        {
            Steps.Add(new DialogueStep(Message, ProceedText, Speaker));
        }

        public void AddOptionAtTop(string Text, Action Result)
        {
            if(Options.Count > 0)
                Options.Insert(0, new DialogueOption(Text, Result));
            else
                Options.Add(new DialogueOption(Text, Result));
        }

        public void AddOption(string Text, Action Result)
        {
            Options.Add(new DialogueOption(Text, Result));
        }

        public void AddOption(DialogueOption NewOption)
        {
            Options.Add(NewOption);
        }

        private void SetCurrentMessageStep()
        {
            if(CurrentStep < Steps.Count)
            {
                DialogueStep Step = Steps[CurrentStep];
                Dialogue.ChangeDialogueMessage(Step.Text);
                Dialogue.ChangeCurrentSpeaker(Step.Speaker);
                if(CurrentStep < Steps.Count - 1)
                {
                    DummyOption[0].Text = Step.ProceedText;
                    DummyOption[0].ParseText();
                    Dialogue.ChangeOptions(DummyOption);
                }
                else
                {
                    if(Options.Count == 0)
                    {
                        Dialogue.ChangeOptions(Dialogue.GetDefaultCloseDialogue);
                    }
                    else
                    {
                        Dialogue.ChangeOptions(Options);
                    }
                }
            }
            else
            {
                Dialogue.ChangeDialogueMessage("");
                Dialogue.ChangeOptions(Dialogue.GetDefaultCloseDialogue);
            }
        }

        public static void ProceedDialogue()
        {
            MultiStepDialogue d = (MultiStepDialogue)Dialogue.CurrentMessage;
            d.CurrentStep++;
            d.SetCurrentMessageStep();
        }

        public struct DialogueStep
        {
            public string Text, ProceedText;
            public Companion Speaker;

            public DialogueStep(string Text, string ProceedText = "Continue", Companion Speaker = null)
            {
                this.ProceedText = ProceedText;
                this.Text = Text;
                this.Speaker = Speaker;
            }
        }
    }

    public class DialogueOption
    {
        public string Text = "";
        public Action ResultAction;
        public List<TextSnippet[]> ParsedText = new List<TextSnippet[]>();

        public DialogueOption(string Text, Action Result)
        {
            this.Text = Text;
            ResultAction = Result;
        } 

        public void ParseText()
        {
            string NewText = Dialogue.ParseText(Text);
            ParsedText.Clear();
            List<List<TextSnippet>> ResultText = Utils.WordwrapStringSmart(NewText, Color.White, Dialogue.GetDialogueFont, (int)((CompanionDialogueInterface.DialogueWidth - 16) * (2f - Main.UIScale)), 5);
            foreach(List<TextSnippet> text in ResultText)
            {
                ParsedText.Add(text.ToArray());
            }
        }
    }
}