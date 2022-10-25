using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class Dialogue
    {
        public static ReLogic.Graphics.DynamicSpriteFont GetDialogueFont => 
            FontAssets.MouseText.Value;

        public static bool InDialogue = false;
        public static Companion Speaker;
        private static Companion DialogueStarterSpeaker;
        public static List<Companion> DialogueParticipants = new List<Companion>();
        public static List<TextSnippet[]> Message = null;
        public static MessageBase CurrentMessage { get; private set;}
        public static DialogueOption[] Options { get; private set; }
        private static DialogueOption[] DefaultClose = new DialogueOption[]{ new DialogueOption("Close", EndDialogue) };
        public static DialogueOption[] GetDefaultCloseDialogue { get{ return DefaultClose; } }

        public static void Unload()
        {
            Message = null;
            CurrentMessage = null;
            Options = null;
            Speaker = null;
            DialogueStarterSpeaker = null;
            DialogueParticipants.Clear();
            DialogueParticipants = null;
            DefaultClose = null;
        }

        public static void StartDialogue(Companion Target)
        {
            PlayerMod.PlayerTalkWith(Main.LocalPlayer, Target);
            DialogueParticipants.Clear();
            DialogueParticipants.Add(Target);
            ChangeDialogueMessage("");
            ChangeOptions(DefaultClose);
            Main.playerInventory = false;
            DialogueStarterSpeaker = Speaker = Target;
            InDialogue = true;
            LobbyDialogue();
        }

        public static void AddParticipant(Companion Participant)
        {

        }

        //Always call after setting up message infos.
        public static void ChangeMessage(MessageBase NewMessage)
        {
            CurrentMessage = NewMessage;
            NewMessage.OnDialogueTrigger();
        }

        public static void ChangeDialogueMessage(string NewDialogue)
        {
            if(NewDialogue == "")
            {
                if(Message == null)
                    Message = new List<TextSnippet[]>();
                else
                    Message.Clear();
                return;
            }
            //MessageContainer.SetContents(NewDialogue, Color.White, CompanionDialogueInterface.DialogueWidth - 16);
            //Message = Utils.WordwrapString(NewDialogue, FontAssets.MouseText.Value, CompanionDialogueInterface.DialogueWidth, 10, out DialogueLines);
            List<List<TextSnippet>> Parsed = Utils.WordwrapStringSmart(NewDialogue, Color.White, GetDialogueFont, CompanionDialogueInterface.DialogueWidth - 16, 10);
            Message = new List<TextSnippet[]>();
            foreach(List<TextSnippet> Text in Parsed)
            {
                Message.Add(Text.ToArray());
            }
        }

        public static void ChangeCurrentSpeaker(Companion NewSpeaker)
        {
            if(NewSpeaker == null)
                Speaker = DialogueStarterSpeaker;
            else
                Speaker = NewSpeaker;
        }

        public static void ChangeOptions(List<DialogueOption> NewOptions)
        {
            ChangeOptions(NewOptions.ToArray());
        }

        public static void ChangeOptions(DialogueOption[] NewOptions)
        {
            Options = NewOptions;
        }

        public static void EndDialogue()
        {
            InDialogue = false;
            PlayerMod pm = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            if(pm.TalkPlayer != null)
            {
                pm.TalkPlayer.GetModPlayer<PlayerMod>().TalkPlayer = null;
            }
            pm.TalkPlayer = null;
            Message.Clear();
            Message = null;
            Options = null;
        }

        public static void LobbyDialogue()
        {
        }

        private static void TestDialogue()
        {
            MessageDialogue md = new MessageDialogue("*Thank you " + Main.LocalPlayer.name+ ", but the other TerraGuardians are in another realm.\nAt least I got [i/s1:357] Bowl of Soup.*", new DialogueOption[0]);
            md.AddOption(new DialogueOption("Ok?", delegate(){ EndDialogue(); }));
            md.AddOption(new DialogueOption("Give me some too", delegate(){ 
                MessageDialogue nmd = new MessageDialogue("*Of course I will give you one. Here it goes.*", new DialogueOption[0]);
                Main.LocalPlayer.QuickSpawnItem(Speaker.GetSource_GiftOrReward(), Terraria.ID.ItemID.BowlofSoup);
                nmd.AddOption("Yay!", delegate(){ EndDialogue(); });
                nmd.RunDialogue();
            }));
            md.AddOption(new DialogueOption("Tell me a story.", delegate()
            {
                MultiStepDialogue msd = new MultiStepDialogue();
                msd.AddDialogueStep("*There was this story about a Terrarian.*");
                msd.AddDialogueStep("*That Terrarian was wielding the legendary Zenith weapon.*", "Oh, wow!");
                msd.AddDialogueStep("*No, wait. It wasn't the Zenith. It was a Copper Shortsword.*");
                msd.AddDialogueStep("*Then, a giant creature came from the sky. The Terrible Moon Lord.*", "Cool!");
                msd.AddDialogueStep("*No, no. Wasn't the Moon Lord. I think was the Eye of Cthulhu.*");
                msd.AddDialogueStep("*No, wasn't Eye of Cthulhu either. Maybe was a Blue Slime.*");
                msd.AddDialogueStep("*The Terrarian sliced the Blue Slime in half with the Copper Shortsword, and saved the entire world.\nThe End.*");
                msd.AddOption("Amazing story!", delegate(){ 
                    new MessageDialogue("*Thank you, Thank you very much.*").RunDialogue();
                });
                msd.AddOption("Boo!", delegate(){ 
                    new MessageDialogue("*You didn't liked it? :(*").RunDialogue();
                });
                msd.RunDialogue();
            }));
            md.RunDialogue();
        }
    }
}