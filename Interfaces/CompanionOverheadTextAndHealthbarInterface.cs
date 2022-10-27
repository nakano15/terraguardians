using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace terraguardians
{
    public class CompanionOverheadTextAndHealthbarInterface : LegacyGameInterfaceLayer
    {
        public CompanionOverheadTextAndHealthbarInterface() : base("TerraGuardians: Companion Chat and Health Bar", DrawInterface, InterfaceScaleType.Game)
        {

        }

        public static bool DrawInterface()
        {
            Vector2 ScreenCenter = new Vector2(Main.screenPosition.X + Main.screenWidth * 0.5f, Main.screenPosition.Y + Main.screenHeight * 0.5f);
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(Math.Abs(c.Center.X - ScreenCenter.X) >= (Main.screenWidth + c.width + 100) * 0.5f | 
                    Math.Abs(c.Center.Y - ScreenCenter.Y) >= (Main.screenHeight + c.height + 100) * 0.5f)
                    continue;
                c.DrawOverheadMessage();
                if (c.ghost || c.dead || c.invis || c.statLife >= c.statLifeMax2) continue;
                if(Main.HealthBarDrawSettings == 1)
                {
                    const float Offset = 10;
                    float lighting = Lighting.Brightness((int)((c.position.X + c.width * 0.5f) * (1f / 16)), (int)((c.position.Y + c.height * 0.5f + c.gfxOffY) * (1f / 16)));
                    Main.instance.DrawHealthBar(c.position.X + c.width * 0.5f, c.position.Y + c.height + Offset + c.gfxOffY, c.statLife, c.statLifeMax2, c.stealth * lighting);
                }
                else if (Main.HealthBarDrawSettings == 2)
                {
                    const float Offset = -20;
                    float lighting = Lighting.Brightness((int)((c.position.X + c.width * 0.5f) * (1f / 16)), (int)((c.position.Y + c.height * 0.5f + c.gfxOffY) * (1f / 16)));
                    Main.instance.DrawHealthBar(c.position.X + c.width * 0.5f, c.position.Y + Offset + c.gfxOffY, c.statLife, c.statLifeMax2, c.stealth * lighting);
                }
            }
            return true;
        }
    }
}