using System;
using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class GroupMembersInterface : LegacyGameInterfaceLayer
    {
        private static float[] BarValues = new float[4];

        private static void SetBarValues(float Value1, float Value2 = 0, float Value3 = 0, float Value4 = 0)
        {
            BarValues[0] = Value1;
            BarValues[1] = Value2;
            BarValues[2] = Value3;
            BarValues[3] = Value4;
        }

        public GroupMembersInterface() : base("TerraGuardians: Group Members", DrawInterface, InterfaceScaleType.UI)
        {
            
        }

        public static void Unload()
        {
            BarValues = null;
        }

        public static bool DrawInterface()
        {
            if(Main.playerInventory) return true;
            Vector2 DrawPosition = new Vector2(8, 120f);
            List<Player> GroupMembers = new List<Player>();
            PlayerMod MyCharacter = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            foreach(Companion c in MyCharacter.GetSummonedCompanions)
            {
                if(c != null)
                    GroupMembers.Add(c);
            }
            string MouseOverText = "";
            foreach(Player p in GroupMembers)
            {
                DrawPosition.X += 32;
                {
                    DrawPosition.X -= 16;
                    DrawPosition.Y += 16;
                    PlayerMod.DrawPlayerHead(p, DrawPosition, false, MaxDimension: 32);
                    DrawPosition.X += 16;
                    DrawPosition.Y -= 16;
                }
                Utils.DrawBorderString(Main.spriteBatch, p.name, DrawPosition, Color.White);
                DrawPosition.Y += 22;
                {
                    float HealthValue = Math.Clamp((float)p.statLife / p.statLifeMax2, 0f, 1f);
                    float LCValue = Math.Clamp((p.statLifeMax - 100) * 0.003333333f, 0f, HealthValue);
                    float LFValue = Math.Clamp((p.statLifeMax - 400) * 0.01f, 0, HealthValue);
                    SetBarValues(HealthValue, LCValue, LFValue);
                }
                if(DrawBar(0, DrawPosition, BarValues))
                {
                    MouseOverText = "Health [" + p.statLife + "/" + p.statLifeMax2 + "]\n"+
                    "Life Crystals [" + Math.Clamp((p.statLifeMax - 100) * 0.05f, 0, 15) + "/15]\n"+
                    "Life Fruits [" + Math.Clamp((p.statLifeMax - 400) * 0.2f, 0, 20) + "/20]";
                }
                DrawPosition.Y += 18;
                DrawPosition.X -= 32;
                {
                    float ManaValue = Math.Clamp((float)p.statMana / p.statManaMax2, 0, 1);
                    float ManaCrystalValue = Math.Clamp((float)(p.statManaMax - 20) * 0.00555555556f, 0, ManaValue);
                    SetBarValues(ManaValue, ManaCrystalValue);
                }
                if(DrawBar(3, DrawPosition, BarValues))
                {
                    MouseOverText = "Mana [" + p.statMana + "/" + p.statManaMax2 + "]\n"
                     + "Mana Crystals [" + Math.Clamp((p.statManaMax - 20) * 0.05f, 0, 9) + "/9]";
                }
                DrawPosition.Y += 18;
                if(p.breath< p.breathMax)
                {
                    float BreathValue = Math.Clamp((float)p.breath / p.breathMax, 0, 1);
                    SetBarValues(BreathValue);
                    DrawBar(1, DrawPosition, BarValues);
                    DrawPosition.Y += 18;
                }
                if(p.lavaTime < p.lavaMax)
                {
                    float LavaValue = Math.Clamp((float)p.lavaTime / p.lavaMax, 0, 1);
                    SetBarValues(LavaValue);
                    DrawBar(2, DrawPosition, BarValues);
                    DrawPosition.Y += 18;
                }
                DrawPosition.Y += 4;
            }
            //for debug
            
            {
                List<string> ExtraMessages = new List<string>();
                foreach(uint i in MainMod.ActiveCompanions.Keys)
                {
                    Companion c = MainMod.ActiveCompanions[i];
                    ExtraMessages.Add(i + "# " + c.name);
                    FriendshipSystem f = c.Data.FriendshipProgress;
                    ExtraMessages.Add("Friendship Level: " + f.Level + " [" + f.Progress + "/" + f.MaxProgress + "]");
                    ExtraMessages.Add("Comfort Stack: " + f.GetComfortStack + "/" + FriendshipSystem.MaxComfortStack);
                    ExtraMessages.Add("Comfort Points: " + f.GetComfortPoints + "/" + f.MaxComfortPoints);
                    //IdleBehavior behavior = (IdleBehavior)MainMod.ActiveCompanions[i].idleBehavior;
                    ///ExtraMessages.Add("Current Behavior: " + behavior.CurrentState.ToString() + " Time: " + behavior.IdleTime);
                }
                foreach(string s in ExtraMessages)
                {
                    Utils.DrawBorderString(Main.spriteBatch, s, DrawPosition, Color.White, 0.7f);
                    DrawPosition.Y += 20;
                }
            }
            //
            if(MouseOverText.Length > 0)
            {
                Vector2 MouseTextPosition = new Vector2(Main.mouseX + 16, Main.mouseY + 16);
                Utils.DrawBorderString(Main.spriteBatch, MouseOverText, MouseTextPosition, Color.White);
            }
            return true;
        }

        private static bool DrawBar(byte BarID, Vector2 BarPosition, float[] BarValues)
        {
            const int BarSpriteWidth = 124, BarSpriteHeight = 16,
                        DistanceUntilBarStartX = 22, BarWidth = 98;
            Rectangle DrawFrame = new Rectangle(BarSpriteWidth * BarID, 0, BarSpriteWidth, BarSpriteHeight);
            Texture2D BarTexture = MainMod.GuardianHealthBarTexture.Value;
            Main.spriteBatch.Draw(BarTexture, BarPosition, DrawFrame, Color.White);
            BarPosition.X += DistanceUntilBarStartX;
            DrawFrame.X += DistanceUntilBarStartX;
            for(byte i = 0; i < BarValues.Length; i++)
            {
                DrawFrame.Y += BarSpriteHeight;
                if(BarValues[i] <= 0) continue;
                DrawFrame.Width = (int)(BarWidth * BarValues[i]);
                Main.spriteBatch.Draw(BarTexture, BarPosition, DrawFrame, Color.White);
            }
            return Main.mouseX >= BarPosition.X && Main.mouseX < BarPosition.X + BarWidth && 
                Main.mouseY >= BarPosition.Y + 4 && Main.mouseY < BarPosition.Y + 12;
        }
    }
}