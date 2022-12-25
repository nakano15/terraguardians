using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionMouseOverInterface : LegacyGameInterfaceLayer
    {
        public CompanionMouseOverInterface() : base("TerraGuardians: Companion Mouse Over", DrawInterface, InterfaceScaleType.Game)
        {

        }

        public static bool DrawInterface()
        {
            if(Main.LocalPlayer.mouseInterface) return true;
            Vector2 MousePosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            List<string> CompanionMouseOverInfos = new List<string>();
            foreach(Companion companion in MainMod.GetActiveCompanions)
            {
                if(MousePosition.X >= companion.position.X && MousePosition.X < companion.position.X + companion.width && 
                   MousePosition.Y >= companion.position.Y && MousePosition.Y < companion.position.Y + companion.height)
                {
                    CompanionMouseOverInfos.Add(companion.GetName + ": " + companion.statLife + "/" + companion.statLifeMax2);
                    if(!companion.dead && !Dialogue.InDialogue && MathF.Abs(MainMod.GetLocalPlayer.Center.X - companion.Center.X) < companion.width * 0.5f + 80 && 
                        MathF.Abs(MainMod.GetLocalPlayer.Center.Y - companion.Center.Y) < companion.height * 0.5f + 80)
                    {
                        if(Main.mouseRight && Main.mouseRightRelease && companion.GetGoverningBehavior().AllowStartingDialogue(companion))
                        {
                            Dialogue.StartDialogue(companion);
                        }
                    }
                }
            }
            if(CompanionMouseOverInfos.Count > 0)
            {
                const float TextVerticalDistancing = 22f;
                MousePosition.X += 16 - Main.screenPosition.X;
                MousePosition.Y += 16 - Main.screenPosition.Y;
                float ListEndPosition = MousePosition.Y + TextVerticalDistancing * CompanionMouseOverInfos.Count;
                if(ListEndPosition > Main.screenHeight)
                {
                    MousePosition.Y -= ListEndPosition - Main.screenHeight;
                }
                for(byte i = 0; i < CompanionMouseOverInfos.Count; i++)
                {
                    Utils.DrawBorderString(Main.spriteBatch, CompanionMouseOverInfos[i], MousePosition, Color.White, 1f);
                    MousePosition.Y += TextVerticalDistancing;
                }
            }
            return true;
        }
    }
}