using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionInventoryInterface : LegacyGameInterfaceLayer
    {
        private static ButtonIDs SelectedButton = 0;

        public CompanionInventoryInterface() : base("TerraGuardians: Companion Inventory Interface", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static bool DrawInterface()
        {
            bool Visible = Main.playerInventory && !Main.CreativeMenu.Enabled;
            if (!Visible) 
            {
                SelectedButton = 0;
                return true;
            }
            const float StartX = 68;
            const float ButtonSize = 0.7f;
            Vector2 ButtonStartPosition = new Vector2(StartX, 267);
            const int HorizontalDistancingButtons = (int)(40 * ButtonSize);
            //if(Main.LocalPlayer.difficulty == 3) ButtonStartPosition.X += 40;
            PlayerMod Player = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            List<ButtonIDs> Buttons = new List<ButtonIDs>();
            Companion companion = Player.GetSummonedCompanions[0];
            //Buttons.Add(ButtonIDs.SelectionUI);
            if(companion != null && companion.active)
            {
                Buttons.Add(ButtonIDs.Inventory);
                Buttons.Add(ButtonIDs.Equipments);
            }
            string MouseText = "";
            foreach(ButtonIDs button in Buttons)
            {
                Vector2 ButtonPosition = new Vector2((int)(ButtonStartPosition.X + (36 * ((1f - ButtonSize) * 0.5f))), (int)(ButtonStartPosition.Y + (36 * ((1f - ButtonSize) * 0.5f))));
                if(Main.mouseX >= ButtonPosition.X && Main.mouseX < ButtonPosition.X + 36 * ButtonSize && 
                   Main.mouseY >= ButtonPosition.Y && Main.mouseY < ButtonPosition.Y + 36 * ButtonSize)
                {
                    MouseText = GetButtonName(button);
                    Player.Player.mouseInterface = true;
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        if(SelectedButton == button)
                            SelectedButton = ButtonIDs.None;
                        else
                            SelectedButton = button;
                        Main.InGuideCraftMenu = false;
                    }
                }
                Main.spriteBatch.Draw(MainMod.GuardianInventoryInterfaceButtonsTexture.Value, ButtonPosition, new Rectangle((int)(button - 1) * 36, 0, 36, 36), Color.White, 0f, Vector2.Zero, ButtonSize, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                ButtonStartPosition.X += HorizontalDistancingButtons;
            }
            ButtonStartPosition.X = StartX + 60;
            ButtonStartPosition.Y += 56;
            if(SelectedButton != ButtonIDs.None)
            {
                Utils.DrawBorderString(Main.spriteBatch, GetButtonName(SelectedButton), ButtonStartPosition, Color.White);
                ButtonStartPosition.Y += 28f;
                for(int i = 0; i < Main.numAvailableRecipes; i++)
                {
                    Main.availableRecipeY[i] = Main.screenHeight + i * 36;
                }
                Main.craftingHide = true;
            }
            switch(SelectedButton)
            {
                case ButtonIDs.Inventory:
                    {
                        Main.inventoryScale = 0.755f;
                        float SlotSize = 56 * Main.inventoryScale;
                        for (byte y = 0; y < 5; y++)
                        {
                            for(byte x = 0; x < 10; x++)
                            {
                                byte i = (byte)(x + y * 10);
                                Vector2 SlotPosition = new Vector2(ButtonStartPosition.X + SlotSize * x, ButtonStartPosition.Y + SlotSize * y);
                                DrawInventorySlot(companion, i, 0, SlotPosition, SlotSize);
                            }
                        }
                        float MiniSlotSize = 40 * Main.inventoryScale;
                        Main.inventoryScale *= 0.8f;
                        for(byte Extra = 0; Extra < 2; Extra++)
                        {
                            byte Context = (byte)(Extra == 0 ? 1 : 2);
                            float ExtraSlotX = ButtonStartPosition.X + Extra * 4 + SlotSize * 10 + MiniSlotSize * Extra;
                            Utils.DrawBorderString(Main.spriteBatch, (Extra == 0 ? "Coins" : "Ammo"), new Vector2(ExtraSlotX + MiniSlotSize * 0.5f, ButtonStartPosition.Y), Color.White, 0.6f, 0.5f);
                            for (byte y = 0; y < 4; y++)
                            {
                                byte i = (byte)(50 + Extra * 4 + y);
                                Vector2 SlotPosition = new Vector2(ExtraSlotX, ButtonStartPosition.Y + (MiniSlotSize + 4) * y + 10);
                                DrawInventorySlot(companion, i, Context, SlotPosition, MiniSlotSize);
                            }
                        }
                    }
                    break;

                case ButtonIDs.Equipments:
                    {
                        float SlotSize = 56 * Main.inventoryScale;
                        for (int s = 0; s < 9; s++)
                        {
                            Vector2 SlotPosition = new Vector2(ButtonStartPosition.X, ButtonStartPosition.Y + s * SlotSize + 20);
                            if(SlotPosition.Y + SlotSize >= Main.screenHeight)
                            {
                                SlotPosition.X += SlotSize + 20;
                                SlotPosition.Y -= SlotPosition.Y - 20 - ButtonStartPosition.Y;
                            }
                            int context = 8;
                            if(s > 2)
                            {
                                context = 10;
                                SlotPosition.Y += 4;
                            }
                            if (s == 8 && !(companion.extraAccessory || (!Main.expertMode && companion.armor[8].type == 0)))
                                continue;
                            if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + SlotSize && 
                                Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + SlotSize)
                            {
                                Player.Player.mouseInterface = true;
                                ItemSlot.OverrideHover(companion.armor, context, s);
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    bool CanEquip = false;
                                    switch(s)
                                    {
                                        case 0:
                                            CanEquip = Main.mouseItem.type == 0 || Main.mouseItem.headSlot > 0;
                                            break;
                                        case 1:
                                            CanEquip = Main.mouseItem.type == 0 || Main.mouseItem.bodySlot > 0;
                                            break;
                                        case 2:
                                            CanEquip = Main.mouseItem.type == 0 || Main.mouseItem.legSlot > 0;
                                            break;
                                        default:
                                            CanEquip = Main.mouseItem.type == 0 || Main.mouseItem.accessory;
                                            if(Main.mouseItem.type != 0)
                                            {
                                                for(byte a = 3; a < 9; a++)
                                                {
                                                    if(companion.armor[a].type == Main.mouseItem.type)
                                                    {
                                                        CanEquip = false;
                                                        break;
                                                    }
                                                }
                                            }
                                            break;
                                    }
                                    if(CanEquip)
                                    {
                                        Main.mouseItem.favorited = false;
                                        ItemSlot.LeftClick(companion.armor, context, s);
                                    }
                                }
                            }
                            ItemSlot.Draw(Main.spriteBatch, companion.armor, context, s, SlotPosition);
                        }
                    }
                    break;
            }
            if(MouseText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
            return true;
        }

        private static void DrawInventorySlot(Companion companion, byte Index, byte Context, Vector2 SlotPosition, float SlotSize)
        {
            if(Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + SlotSize && 
            Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + SlotSize)
            {
                Main.LocalPlayer.mouseInterface = true;
                ItemSlot.OverrideHover(companion.inventory, Context, Index);
                if(Main.mouseLeft && Main.mouseLeftRelease)
                {
                    if(Main.keyState.IsKeyDown(Main.FavoriteKey))
                    {
                        companion.inventory[Index].favorited = !companion.inventory[Index].favorited;
                    }
                    else if (ItemSlot.ShiftInUse)
                    {
                        if (!companion.inventory[Index].favorited && companion.inventory[Index].type != 0)
                        {
                            Item item = Main.LocalPlayer.GetItem(Main.LocalPlayer.whoAmI, companion.inventory[Index], GetItemSettings.InventoryEntityToPlayerInventorySettings);
                            companion.inventory[Index] = item;
                        }
                    }
                    else
                    {
                        ItemSlot.LeftClick(companion.inventory, Context, Index);
                    }
                }
                else
                {
                    ItemSlot.RightClick(companion.inventory, Context, Index);
                }
                ItemSlot.MouseHover(companion.inventory, Context, Index);
            }
            ItemSlot.Draw(Main.spriteBatch, companion.inventory, Context, Index, SlotPosition);
        }

        private static string GetButtonName(ButtonIDs button)
        {
            switch(button)
            {
                case ButtonIDs.SelectionUI:
                    return "Companion List";
                case ButtonIDs.Inventory:
                    return "Inventory";
                case ButtonIDs.Equipments:
                    return "Equipments";
                case ButtonIDs.Quests:
                    return "Quests";
                case ButtonIDs.Requests:
                    return "Requests";
                case ButtonIDs.Skills:
                    return "Skills";
                case ButtonIDs.Behaviour:
                    return "Behaviour";
            }
            return button.ToString();
        }

        public enum ButtonIDs : byte
        {
            None = 0,
            SelectionUI = 1,
            Inventory = 2,
            Equipments = 3,
            Bank = 4,
            Behaviour = 5,
            Requests = 6,
            Skills = 7,
            Quests = 8
        }
    }
}