using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class BuddyModeSetupInterface : LegacyGameInterfaceLayer
    {
        public BuddyModeSetupInterface() :
            base("TerraGuardians: Buddy Mode Setup", DrawInterface, InterfaceScaleType.None)
        {
            
        }

        public static bool DrawInterface()
        {
            return true;
        }
    }
}