using System;
using Terraria;
using Terraria.UI;
using Terraria.Localization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class ReviveInterface : LegacyGameInterfaceLayer
    {
        private static bool KnockedOutColdAlpha = false;
        internal static int ReviveBarStyle = 0;
        const string InterfaceKey = "Mods.terraguardians.Interface.Revive.";

        public ReviveInterface() : base("TerraGuardians: Revive Interface", DrawInterface, InterfaceScaleType.UI)
        {
            
        }

        static string GetTranslation(string Key)
        {
            return Language.GetTextValue(InterfaceKey + Key);
        }

        public static bool DrawInterface()
        {
            Player ReviveCharacter = MainMod.GetLocalPlayer;
            Companion controlled = PlayerMod.PlayerGetControlledCompanion(ReviveCharacter);
            if (controlled != null) ReviveCharacter = controlled;
            KnockoutStates state = PlayerMod.GetPlayerKnockoutState(ReviveCharacter);
            if(state == KnockoutStates.Awake)
            {
                KnockedOutColdAlpha = false;
                return true;
            }
            if (state == KnockoutStates.KnockedOutCold) KnockedOutColdAlpha = true;
            float Percentage = Math.Clamp((float)ReviveCharacter.statLife / ReviveCharacter.statLifeMax2, 0f, 1f);
            float RescueBarTime = state == KnockoutStates.KnockedOutCold ? (float)MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().GetRescueStack / PlayerMod.MaxRescueStack : 0;
            DrawVerticalBars(state, Percentage, RescueBarTime);
            DrawHealthBar(state, Percentage, ReviveCharacter);
            return true;
        }

        private static void DrawHealthBar(KnockoutStates state, float Percentage, Player player)
        {
            Vector2 BarPosition = new Vector2(Main.screenWidth * 0.5f - 80, Main.screenHeight * 0.7f - 12);
            if (Percentage > 0)
            {
                Rectangle DrawDimension = new Rectangle(0, 0, 160, 24);
                Main.spriteBatch.Draw(MainMod.ReviveHealthBarTexture.Value, BarPosition, DrawDimension, Color.White);
                BarPosition.X += 4;
                BarPosition.Y += 4;
                DrawDimension.X += 4;
                DrawDimension.Y += 28;
                DrawDimension.Width = (int)(152 * Percentage);
                DrawDimension.Height -= 8;
                Main.spriteBatch.Draw(MainMod.ReviveHealthBarTexture.Value, BarPosition, DrawDimension, Color.White);
                BarPosition.X += 80;
                BarPosition.Y += 52;
                string ReviveMessage;
                if (player.GetModPlayer<PlayerMod>().GetReviveBoost > 0)
                    ReviveMessage = GetTranslation("BeingRevived");
                else if (player.GetModPlayer<PlayerMod>().GetReviveStack > 0)
                    ReviveMessage = GetTranslation("RevivingByItself");
                else if (state == KnockoutStates.KnockedOut)
                {
                    ReviveMessage = GetTranslation("BleedingOut");
                }
                else
                {
                    ReviveMessage = GetTranslation("Incapacitated");
                }
                Utils.DrawBorderStringBig(Main.spriteBatch, ReviveMessage, BarPosition, Color.White, 1, 0.5f, 0.5f);
            }
            else
            {
                if (!player.dead)
                {
                    BarPosition.X += 80;
                    BarPosition.Y += 32;
                    string ReviveMessage = GetTranslation("Incapacitated");
                    Utils.DrawBorderStringBig(Main.spriteBatch, ReviveMessage, BarPosition, Color.White, 1, 0.5f, 0.5f);
                }
            }
            if (state == KnockoutStates.KnockedOutCold)
            {
                BarPosition.Y += 50;
                string Message;
                Player LocalPlayer = MainMod.GetLocalPlayer;
                if (LocalPlayer.GetModPlayer<PlayerMod>().GetRescueStack >= PlayerMod.MaxRescueStack / 2)
                    Message = GetTranslation("RescuedByMes");
                else if (LocalPlayer.controlHook)
                    Message = GetTranslation("CallForHelpMes");
                else
                    Message = GetTranslation("CallForHelpPromptMes");
                Utils.DrawBorderStringBig(Main.spriteBatch, Message, BarPosition, Color.White, 1, 0.5f, 0.5f);
            }
        }

        private static void DrawVerticalBars(KnockoutStates state, float Percentage, float RescueBarTime)
        {
            Rectangle DrawFrame = new Rectangle(ReviveBarStyle * 640, 0, 640, 480);
            Vector2 Scale = new Vector2((float)Main.screenWidth / 640, (float)Main.screenHeight / 480);
            Vector2 Position = new Vector2(0, (int)(Percentage * (Main.screenHeight * 0.6f)));
            float Opacity = 1;
            if (state == KnockoutStates.KnockedOutCold)
                Opacity = System.Math.Min(1, 0.5f + RescueBarTime);
            else
                Opacity = KnockedOutColdAlpha ? (1f - Percentage) : 0.5f * (1f - Percentage);
            Color color = Color.White * Opacity;
            Main.spriteBatch.Draw(MainMod.ReviveBarsEffectTexture.Value, Position, DrawFrame, color, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
            Position = new Vector2(0, (int)(Percentage * (-Main.screenHeight * 0.6f)));
            DrawFrame.Y += DrawFrame.Height;
            Main.spriteBatch.Draw(MainMod.ReviveBarsEffectTexture.Value, Position, DrawFrame, color, 0, Vector2.Zero, Scale, SpriteEffects.None, 0);
        }
    }
}