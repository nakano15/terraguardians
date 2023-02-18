using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class ClearCompanionsFromPlayerListInterface : LegacyGameInterfaceLayer
    {
        public ClearCompanionsFromPlayerListInterface() :
            base("TerraGuardians: Clear Companions From Player List", DrawInterface, InterfaceScaleType.None)
        {
            
        }

        public static bool DrawInterface()
        {
            SystemMod.RestoreBackedUpPlayers();
            return true;
        }
    }
}