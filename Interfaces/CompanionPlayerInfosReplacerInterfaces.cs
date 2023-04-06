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
    public class CompanionPlayerHealthReplacerInterface : LegacyGameInterfaceLayer
    {
        public CompanionPlayerHealthReplacerInterface() :
            base("TerraGuardians: Resource Bars", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static bool DrawInterface()
        {
            Player backup = Main.player[Main.myPlayer];
            Companion Controlled = PlayerMod.PlayerGetControlledCompanion(backup);
            if (Controlled != null) //Check if null to avoid errors when changing control.
            {
                Main.player[Main.myPlayer] = Controlled;
                Main.instance.GUIBarsDraw();
                Main.player[Main.myPlayer] = backup;
            }
            return true;
        }
    }
    public class CompanionPlayerHotbarReplacerInterface : LegacyGameInterfaceLayer
    {
        public CompanionPlayerHotbarReplacerInterface() :
            base("TerraGuardians: Hotbar", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static bool DrawInterface()
        {
            if (Main.playerInventory) return true;
            Player backup = Main.player[Main.myPlayer];
            Companion Controlled = PlayerMod.PlayerGetControlledCompanion(backup);
            if (Controlled != null && !Controlled.ghost) //Check if null to avoid errors when changing control.
            {
                Main.player[Main.myPlayer] = Controlled;
                
                string Text = !String.IsNullOrEmpty(Controlled.HeldItem.Name) ? Controlled.HeldItem.AffixName() : Lang.inter[37].Value;
                Vector2 Dimension = FontAssets.MouseText.Value.MeasureString(Text) * 0.5f;
                Main.spriteBatch.DrawString(FontAssets.MouseText.Value, Text, new Vector2(236f - Dimension.X, 0), new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), 0, default(Vector2), 1f, SpriteEffects.None, 0f);
                float SlotX = 20f;
                Color LightColor = default(Color);
                for(int i = 0; i < 10; i++)
                {
                    if (i == Controlled.selectedItem)
                    {
                        if (Main.hotbarScale[i] < 1)
                            Main.hotbarScale[i] += 0.05f;
                    }
                    else if (Main.hotbarScale[i] > 0.75f)
                        Main.hotbarScale[i] -= 0.05f;
                    float Scale = Main.hotbarScale[i];
                    float SlotY = 20f + 22f * (1f - Scale);
                    int Alpha = (int)(75f + 150f * Scale);
                    LightColor = new Color(255, 255, 255, Alpha);
                    float InvScaleBackup = Main.inventoryScale;
                    Main.inventoryScale = Scale;
                    ItemSlot.Draw(Main.spriteBatch, Controlled.inventory, 13, i, new Vector2(SlotX, SlotY), LightColor);
                    Main.inventoryScale = InvScaleBackup;
                    SlotX += TextureAssets.InventoryBack.Width() * Scale;
                }
                int SelectedItem = Controlled.selectedItem;
                if (SelectedItem >= 10 && (SelectedItem != 58 || Main.mouseItem.type > 0))
                {
                    const float Scale = 1f;
                    const float SlotY = 20f + 22f * (1f - Scale);
                    const int Alpha = (int)(75f + 150f * Scale);
                    LightColor = new Color(255, 255, 255, Alpha);
                    float ScaleBackup = Main.inventoryScale;
                    Main.inventoryScale = Scale;
                    ItemSlot.Draw(Main.spriteBatch, Controlled.inventory, 13, SelectedItem, new Vector2(SlotX, SlotY), LightColor);
                    Main.inventoryScale = ScaleBackup;
                }

                Main.player[Main.myPlayer] = backup;
            }
            return true;
        }
    }
}