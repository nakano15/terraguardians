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
        private static MessageBase[] LobbyMessage;

        public static void Load()
        {

        }

        private static void UnloadDefaultDialogues()
        {
            LobbyMessage = null;
        }


    }
}