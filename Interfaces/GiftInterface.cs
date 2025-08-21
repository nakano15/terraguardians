using Terraria;
using Terraria.UI;
using Terraria.Localization;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ID;

namespace terraguardians
{
    public class GiftInterface : LegacyGameInterfaceLayer
    {
        public static Item[] GiftSlots = new Item[0];
        static bool IsInitialized = false;

        public GiftInterface() :
            base("TerraGuardians: Gift Interface", DrawInterface, InterfaceScaleType.UI)
        {
            
        }

        public static void Initialize()
        {
            GiftSlots = new Item[MainMod.MaxCompanionFollowers];
            for (int i = 0; i < GiftSlots.Length; i++)
            {
                GiftSlots[i] = new Item();
            }
            IsInitialized = true;
        }

        public static void Unload()
        {
            for (int i = 0; i < GiftSlots.Length; i++)
            {
                GiftSlots[i] = null;
            }
            GiftSlots = null;
        }

        public static bool DrawInterface()
        {
            if (!IsInitialized)
                Initialize();
            bool CompanionInventoryMode = Main.playerInventory && MainMod.GetLocalPlayer.chest == -1 && CompanionInventoryInterface.GetCurrentButton == CompanionInventoryInterface.ButtonIDs.None;
            if (Main.npcShop > 0 || MainMod.GetLocalPlayer.chest > -1 || CompanionInventoryMode)
            {
                const float StartX = 160, StartYDefault = 478, StartYInv = 352 + 48;
                float StartY = CompanionInventoryMode ? StartYInv : StartYDefault;
                Companion[] companions = PlayerMod.PlayerGetSummonedCompanions(MainMod.GetLocalPlayer);
                Utils.DrawBorderString(Main.spriteBatch, "Send to...", new Vector2(StartX, StartY - 48), Color.White);
                const int Context = ItemSlot.Context.ChestItem;
                for (int y = 0; y < 5; y++)
                {
                    for (int x = 0; x < 6; x++)
                    {
                        int Index = x + y * 6;
                        if (Index >= companions.Length) break;
                        Vector2 DrawPos = new Vector2(StartX + x * 50, StartY + y * 50);
                        if (companions[Index] != null)
                        {
                            companions[Index].DrawCompanionHead(DrawPos - Vector2.One * 4, false);
                            if (Main.mouseX >= DrawPos.X && Main.mouseX < DrawPos.X + 40 * Main.inventoryScale &&
                                Main.mouseY >= DrawPos.Y && Main.mouseY < DrawPos.Y + 40 * Main.inventoryScale)
                            {
                                MainMod.GetLocalPlayer.mouseInterface = true;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    ItemSlot.LeftClick(ref GiftSlots[Index], Context);
                                    if (GiftSlots[Index].type > ItemID.None)
                                    {
                                        companions[Index].AddItem(GiftSlots[Index]);
                                    }
                                }
                            }
                            ItemSlot.Draw(Main.spriteBatch, ref GiftSlots[Index], Context, DrawPos);
                        }
                    }
                }
            }
            return true;
        }
    }
}