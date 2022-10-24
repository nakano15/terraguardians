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
        public static Player Speaker;
        public static List<TextSnippet[]> Message = null;
        public static MessageBase CurrentMessage { get; private set;}
        public static DialogueOption[] Options { get; private set; }

        public static void Unload()
        {
            Message = null;
            CurrentMessage = null;
            Options = null;
        }

        public static void StartDialogue(Player Target)
        {
            if(!(Target is Companion)) return;
            PlayerMod.PlayerTalkWith(Main.LocalPlayer, Target);
            Main.playerInventory = false;
            Speaker = Target;
            InDialogue = true;
            LobbyDialogue();
        }

        //Always call after setting up message infos.
        public static void ChangeMessage(MessageBase NewMessage)
        {
            CurrentMessage = NewMessage;
            ChangeDialogueMessage(CurrentMessage.MessageText);
            NewMessage.OnDialogueTrigger();
        }

        public static void ChangeDialogueMessage(string NewDialogue)
        {
            //MessageContainer.SetContents(NewDialogue, Color.White, CompanionDialogueInterface.DialogueWidth - 16);
            //Message = Utils.WordwrapString(NewDialogue, FontAssets.MouseText.Value, CompanionDialogueInterface.DialogueWidth, 10, out DialogueLines);
            List<List<TextSnippet>> Parsed = Utils.WordwrapStringSmart(NewDialogue, Color.White, GetDialogueFont, CompanionDialogueInterface.DialogueWidth - 16, 10);
            Message = new List<TextSnippet[]>();
            foreach(List<TextSnippet> Text in Parsed)
            {
                Message.Add(Text.ToArray());
            }
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
            MessageDialogue md = new MessageDialogue("*Thank you " + Main.LocalPlayer.name+ ", but the other TerraGuardians are in another realm.\nAt least I got [i/s1:357] Bowl of Soup.*", new DialogueOption[0]);
            md.AddOption(new DialogueOption("Ok?", delegate(){ EndDialogue(); }));
            md.AddOption(new DialogueOption("Give me some too", delegate(){ 
                MessageDialogue nmd = new MessageDialogue("*Of course I will give you one. Here it goes.*", new DialogueOption[0]);
                Main.LocalPlayer.QuickSpawnItem(Speaker.GetSource_GiftOrReward(), Terraria.ID.ItemID.BowlofSoup);
                nmd.AddOption("Yay!", delegate(){ EndDialogue(); });
                ChangeMessage(nmd);
            }));
            ChangeMessage(md);
        }
    }
}