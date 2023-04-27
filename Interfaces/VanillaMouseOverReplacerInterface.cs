using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;
using Terraria.GameContent.Creative;
using ReLogic.Graphics;

namespace terraguardians
{
    public class VanillaMouseOverReplacerInterface : LegacyGameInterfaceLayer
    {
        public VanillaMouseOverReplacerInterface() :
            base("TerraGuardians: Mouse Over Replacer", DrawInterface, InterfaceScaleType.UI)
        {

        }

        private static bool DrawInterface()
        {
            Player[] players = new Player[255];
            for(int p = 0; p < 255; p++)
            {
                players[p] = Main.player[p];
                if (Main.player[p] is Companion)
                {
                    Main.player[p] = Main.player[255];
                }
            }
            if (Main.ignoreErrors)
            {
                try
                {
                    Main.instance.DrawMouseOver();
                }
                catch (System.Exception e)
                {
                    TimeLogger.DrawException(e);
                }
            }
            else
            {
                Main.instance.DrawMouseOver();
            }
            for(int p = 0; p < 255; p++)
            {
                Main.player[p] = players[p];
            }
            return true;
        }
    }
}