using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class Companion2PMouseInterface : LegacyGameInterfaceLayer
    {
        public Companion2PMouseInterface() : 
            base("TerraGuardians: 2P Mouse Interface", DrawInterface, InterfaceScaleType.Game)
        {
            
        }

        public static bool DrawInterface()
        {
            Companion LeaderCompanion = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer);
            if (false && LeaderCompanion != null)
            {
                Vector2 MousePosition = LeaderCompanion.GetAimedPosition - Main.screenPosition;
                Main.spriteBatch.Draw(MainMod.TGMouseTexture.Value, MousePosition, Color.White);
            }
            return true;
        }
    }
}