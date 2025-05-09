using Terraria;
using Terraria.UI;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionMouseOverInterface : LegacyGameInterfaceLayer
    {
        const string InterfaceKey = "Mods.terraguardians.Interface.MouseOver.";

        public CompanionMouseOverInterface() : base("TerraGuardians: Companion Mouse Over", DrawInterface, InterfaceScaleType.Game)
        {

        }

        public static bool DrawInterface()
        {
            if(Main.LocalPlayer.mouseInterface) return true;
            Vector2 MousePosition = new Vector2(Main.mouseX, Main.mouseY) + Main.screenPosition;
            //bool RevivingSomeone = false;
            Player player = MainMod.GetLocalPlayer;
            if (player.dead || player.ghost) return true;
            List<string> CompanionMouseOverInfos = new List<string>();
            {
                Companion Controlled = PlayerMod.PlayerGetControlledCompanion(player);
                if (Controlled != null)
                {
                    player = Controlled;
                }
            }
            bool RevivingSomeone = false;
            bool InNeedOfHelpNotification = false;
            foreach(Companion companion in MainMod.GetActiveCompanions)
            {
                if(MousePosition.X >= companion.position.X && MousePosition.X < companion.position.X + companion.width && 
                   MousePosition.Y >= companion.position.Y && MousePosition.Y < companion.position.Y + companion.height && 
                   !companion.invis && companion.GetGoverningBehavior().IsVisible)
                {
                    if (companion.KnockoutStates == KnockoutStates.Awake)
                        CompanionMouseOverInfos.Add(Language.GetTextValue(InterfaceKey + "CompanionStatsInfoAwake")
                            .Replace("{name}", companion.GetName)
                            .Replace("{health}", companion.statLife.ToString())
                            .Replace("{maxhealth}", companion.statLifeMax2.ToString()));
                    else
                        CompanionMouseOverInfos.Add(Language.GetTextValue(InterfaceKey + "CompanionStatsInfoUnconscious")
                            .Replace("{name}", companion.GetName)
                            .Replace("{hppercent}", MathF.Round(companion.statLife * 100f / companion.statLifeMax2).ToString()));
                    if(!companion.dead && !Dialogue.InDialogue && MathF.Abs(MainMod.GetLocalPlayer.Center.X - companion.Center.X) < companion.width * 0.5f + 80 && 
                        MathF.Abs(MainMod.GetLocalPlayer.Center.Y - companion.Center.Y) < companion.height * 0.5f + 80 && !player.dead && PlayerMod.GetPlayerKnockoutState(player) == KnockoutStates.Awake)
                    {
                        if (companion.KnockoutStates >= KnockoutStates.KnockedOut && companion.GetPlayerMod.CanBeHelpedToRevive)
                        {
                            MainMod.GetLocalPlayer.mouseInterface = true;
                            if (!RevivingSomeone)
                            {
                                if (Main.mouseLeft)
                                {
                                    companion.GetPlayerMod.ChangeReviveStack(1);
                                    CompanionMouseOverInfos.Add(Language.GetTextValue(InterfaceKey + "RevivingMes").Replace("{name}", companion.GetName));
                                    RevivingSomeone = true;
                                }
                                else
                                {
                                    InNeedOfHelpNotification = true;
                                }
                            }
                        }
                        else if(Main.mouseRight && Main.mouseRightRelease && companion.KnockoutStates == KnockoutStates.Awake && companion.GetGoverningBehavior().AllowStartingDialogue(companion))
                        {
                            Dialogue.StartDialogue(companion);
                        }
                    }
                }
            }
            if (InNeedOfHelpNotification)
                CompanionMouseOverInfos.Add(Language.GetTextValue(InterfaceKey + "LeftClickResMessage"));
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