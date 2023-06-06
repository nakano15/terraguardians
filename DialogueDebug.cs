using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.UI;
using Terraria.UI.Chat;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public partial class Dialogue
    {
        private static void DebugLobby()
        {
            MessageDialogue mb = new MessageDialogue("(What do you want to test?)");
            mb.AddOption("Remove Companion Met.", DEBUGRemoveCompanionMet);
            mb.AddOption("Remove Companion Met and From Player.", DEBUGRemoveCompanionFromPlayer);
            mb.AddOption("Nevermind.", EndDialogue);
            mb.RunDialogue();
        }

        private static void DEBUGRemoveCompanionMet()
        {
            WorldMod.RemoveCompanionMet(Speaker);
            MessageDialogue mb = new MessageDialogue("(Who are you?)");
            mb.RunDialogue();
        }

        private static void DEBUGRemoveCompanionFromPlayer()
        {
            MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().RemoveCompanion(Speaker);
            WorldMod.RemoveCompanionMet(Speaker);
            MessageDialogue mb = new MessageDialogue("(I thought I knew you?)");
            mb.RunDialogue();
        }
    }
}