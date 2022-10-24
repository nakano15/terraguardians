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
        public string MessageText = "";

        public virtual void OnDialogueTrigger()
        {

        }
    }

    public class MessageDialogue : MessageBase
    {
        public List<DialogueOption> Options = new List<DialogueOption>();
        public MessageDialogue(string Message, DialogueOption[] options = null)
        {
            MessageText = Message;
            if(options != null) this.Options.AddRange(options);
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
            Dialogue.ChangeOptions(Options.ToArray());
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
            List<List<TextSnippet>> ResultText = Utils.WordwrapStringSmart(Text, Color.White, Dialogue.GetDialogueFont, CompanionDialogueInterface.DialogueWidth, 5);
            foreach(List<TextSnippet> text in ResultText)
            {
                ParsedText.Add(text.ToArray());
            }
            ResultAction = Result;
        } 
    }
}