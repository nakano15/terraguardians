using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;

namespace terraguardians
{
    public class CompanionInventoryInterface : LegacyGameInterfaceLayer
    {
        private static int SelectedCompanion = -1;
        private static ButtonIDs SelectedButton = 0;
        public static ButtonIDs GetCurrentButton { get { return SelectedButton; } }
        private static short CompanionToMoveHouse = -1;

        public CompanionInventoryInterface() : base("TerraGuardians: Companion Inventory Interface", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static bool IsInterfaceOpened
        {
            get
            {
                return Main.playerInventory && SelectedButton > 0;
            }
        }

        public static bool DrawInterface()
        {
            bool Visible = Main.playerInventory && !Main.CreativeMenu.Enabled;
            if (!Visible) 
            {
                SelectedButton = 0;
                return true;
            }
            string MouseText = "";
            const float StartX = 68;
            const float ButtonSize = 0.7f;
            Vector2 ButtonStartPosition = new Vector2(StartX, 267);
            const int HorizontalDistancingButtons = (int)(40 * ButtonSize);
            //if(Main.LocalPlayer.difficulty == 3) ButtonStartPosition.X += 40;
            PlayerMod Player = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            List<ButtonIDs> Buttons = new List<ButtonIDs>();
            Companion[] Companions = Player.GetSummonedCompanions;
            Companion companion = SelectedCompanion == -1 ? null : Companions[SelectedCompanion];
            {
                for(int i = 0; i < Companions.Length; i++)
                {
                    if(Companions[i] != null)
                    {
                        Companions[i].DrawCompanionHead(ButtonStartPosition + Vector2.UnitX * 18 + Vector2.UnitY * (18 + 6 * (i == SelectedCompanion ? -1 : 1)), false, 1, 36);
                        if(Main.mouseX >= ButtonStartPosition.X && Main.mouseX < ButtonStartPosition.X + 36 && Main.mouseY >= ButtonStartPosition.Y && Main.mouseY < ButtonStartPosition.Y + 48)
                        {
                            MouseText = Companions[i].name;
                            Player.Player.mouseInterface = true;
                            if(Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                if (i == SelectedCompanion)
                                {
                                    SelectedCompanion = -1;
                                    SelectedButton = ButtonIDs.None;
                                }
                                else
                                    SelectedCompanion = i;
                            }
                        }
                        ButtonStartPosition.X += 36;
                    }
                }
                ButtonStartPosition.X = StartX;
                ButtonStartPosition.Y += 48;
            }
            //Interface buttons go here.
            Buttons.Add(ButtonIDs.SelectionUI);
            if(companion != null && companion.active)
            {
                Buttons.Add(ButtonIDs.Inventory);
                Buttons.Add(ButtonIDs.Equipments);
            }
            Buttons.Add(ButtonIDs.Housing);
            //
            foreach(ButtonIDs button in Buttons)
            {
                Vector2 ButtonPosition = new Vector2((int)(ButtonStartPosition.X + (36 * ((1f - ButtonSize) * 0.5f) - 2 * ButtonSize)), (int)(ButtonStartPosition.Y + (36 * ((1f - ButtonSize) * 0.5f)) - 2 * ButtonSize));
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
                Main.spriteBatch.Draw(MainMod.GuardianInventoryInterfaceButtonsTexture.Value, ButtonPosition, new Rectangle((int)(button - 1) * 40, 0, 40, 40), Color.White, 0f, Vector2.Zero, ButtonSize, Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0f);
                ButtonStartPosition.X += HorizontalDistancingButtons;
            }
            ButtonStartPosition.X = StartX + 60;
            ButtonStartPosition.Y += 36;
            if(SelectedButton != ButtonIDs.None)
            {
                Utils.DrawBorderString(Main.spriteBatch, GetButtonName(SelectedButton), ButtonStartPosition, Color.White);
                ButtonStartPosition.Y += 22f;
                for(int i = 0; i < Main.numAvailableRecipes; i++)
                {
                    Main.availableRecipeY[i] = Main.screenHeight + i * 36;
                }
                Main.craftingHide = true;
            }
            if (CompanionToMoveHouse > -1)
            {
                CompanionTownNpcState tns = WorldMod.CompanionNPCsInWorld[CompanionToMoveHouse];
                MainMod.GetLocalPlayer.mouseInterface = true;
                if (tns == null || tns.GetCompanion == null)
                {
                    CompanionToMoveHouse = -1;
                }
                else if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    int TileX = (int)((Main.mouseX + Main.screenPosition.X) * (1f / 16)),
                        TileY = (int)((Main.mouseY + Main.screenPosition.Y) * (1f / 16));
                    if (WorldMod.TryMovingCompanionIn(TileX, TileY, tns.CharID, false, false))
                    {
                        Main.NewText(tns.GetCompanion.GetName + " will now move in to this house.");
                    }
                    CompanionToMoveHouse = -1;
                }
            }
            switch(SelectedButton)
            {
                case ButtonIDs.SelectionUI:
                    {
                        CompanionSelectionInterface.OpenInterface();
                    }
                    break;
                case ButtonIDs.Inventory:
                    {
                        Main.inventoryScale = 0.755f;
                        float SlotSize = 56 * Main.inventoryScale;
                        int PlayerInventoryBackup = MainMod.GetLocalPlayer.selectedItem;
                        MainMod.GetLocalPlayer.selectedItem = companion.selectedItem;
                        for (byte y = 0; y < 5; y++)
                        {
                            for(byte x = 0; x < 10; x++)
                            {
                                byte i = (byte)(x + y * 10);
                                Vector2 SlotPosition = new Vector2(ButtonStartPosition.X + SlotSize * x, ButtonStartPosition.Y + SlotSize * y);
                                DrawInventorySlot(companion, i, 0, SlotPosition, SlotSize);
                            }
                        }
                        MainMod.GetLocalPlayer.selectedItem = PlayerInventoryBackup;
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
                        Item[] PlayerArmorBackup = Main.LocalPlayer.armor;
                        Main.LocalPlayer.armor = companion.armor;
                        byte ExtraSlotsCount = 0;
                        if (companion.CanDemonHeartAccessoryBeShown()) ExtraSlotsCount++;
                        if (companion.CanMasterModeAccessoryBeShown()) ExtraSlotsCount ++;
                        for (int s = 0; s < 10; s++)
                        {
                            Vector2 SlotPosition = new Vector2(ButtonStartPosition.X, ButtonStartPosition.Y + s * SlotSize + 20);
                            while(SlotPosition.Y + SlotSize >= Main.screenHeight)
                            {
                                SlotPosition.X += (SlotSize + 20) * 3;
                                SlotPosition.Y -= SlotPosition.Y - 20 - ButtonStartPosition.Y;
                            }
                            if (s >= 8)
                            {
                                if (ExtraSlotsCount == 0) continue;
                                ExtraSlotsCount--;
                            }
                            for(byte Slot = 0; Slot < 3; Slot++)
                            {
                                byte Index = (byte)(s + 10 * Slot);
                                int context = 8;
                                Item[] slots = companion.armor;
                                if(s > 2)
                                {
                                    context = 10;
                                    SlotPosition.Y += 4;
                                }
                                switch(Slot)
                                {
                                    case 1:
                                        context++;
                                        break;
                                    case 2:
                                        context = 12;
                                        slots = companion.dye;
                                        Index -= 20;
                                        break;
                                }
                                if (Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + SlotSize && 
                                    Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + SlotSize)
                                {
                                    Player.Player.mouseInterface = true;
                                    ItemSlot.OverrideHover(slots, context, Index);
                                    if(Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        bool CanEquip = false;
                                        if(Slot < 2)
                                        {
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
                                                            if(slots[Index].type == Main.mouseItem.type)
                                                            {
                                                                CanEquip = false;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            CanEquip = Main.mouseItem.type == 0 || Main.mouseItem.dye > 0;
                                        }
                                        if(CanEquip)
                                        {
                                            Main.mouseItem.favorited = false;
                                            ItemSlot.LeftClick(slots, context, Index);
                                        }
                                    }
                                    ItemSlot.MouseHover(slots, context, Index);
                                }
                                ItemSlot.Draw(Main.spriteBatch, slots, context, Index, SlotPosition);
                                SlotPosition.X += SlotSize + 4;
                                if(s > 2) SlotPosition.Y -= 4;
                            }
                        }
                        Main.LocalPlayer.armor = PlayerArmorBackup;
                    }
                    break;

                case ButtonIDs.Housing:
                    {
                        float HouseButtonSize = 56 * Main.inventoryScale;
                        Vector2 HousingButtonPosition = new Vector2(ButtonStartPosition.X, ButtonStartPosition.Y);
                        Texture2D background = TextureAssets.InventoryBack11.Value;
                        for(int i = 0; i < WorldMod.MaxCompanionNpcsInWorld; i++)
                        {
                            CompanionTownNpcState tns = WorldMod.CompanionNPCsInWorld[i];
                            if (tns == null) continue;
                            Companion c = tns.GetCompanion;
                            if (c == null) continue;
                            Main.spriteBatch.Draw(background, HousingButtonPosition, null, Color.White, 0, Vector2.Zero, Main.inventoryScale, SpriteEffects.None, 0);
                            c.DrawCompanionHead(HousingButtonPosition + Vector2.One * 26 * Main.inventoryScale, false, Main.inventoryScale, 40);
                            if (Main.mouseX >= HousingButtonPosition.X && Main.mouseX < HousingButtonPosition.X + HouseButtonSize && 
                                Main.mouseY >= HousingButtonPosition.Y && Main.mouseY < HousingButtonPosition.Y + HouseButtonSize)
                            {
                                MouseText = c.GetRealName;
                                MainMod.GetLocalPlayer.mouseInterface = true;
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    if(i == CompanionToMoveHouse)
                                        CompanionToMoveHouse = -1;
                                    else
                                        CompanionToMoveHouse = (short)i;
                                }
                            }
                            HousingButtonPosition.Y += HouseButtonSize + 4 * Main.inventoryScale;
                            if(HousingButtonPosition.Y + HouseButtonSize + 4 * Main.inventoryScale >= Main.screenHeight)
                            {
                                HousingButtonPosition.Y = ButtonStartPosition.Y;
                                HousingButtonPosition.X += HouseButtonSize + 4 * Main.inventoryScale;
                            }
                        }
                    }
                    break;
            }
            if(MouseText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
            else if(CompanionToMoveHouse > -1)
            {
                Vector2 HeadPosition = new Vector2(Main.mouseX + 20, Main.mouseY + 20);
                WorldMod.CompanionNPCsInWorld[CompanionToMoveHouse].GetCompanion.DrawCompanionHead(HeadPosition, false);
            }
            return true;
        }

        private static void DrawInventorySlot(Companion companion, byte Index, byte Context, Vector2 SlotPosition, float SlotSize)
        {
            if(Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + SlotSize && 
            Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + SlotSize)
            {
                bool AllowInteraction = companion.selectedItem != Index || companion.itemAnimation == 0;
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
                    else if(AllowInteraction)
                    {
                        ItemSlot.LeftClick(companion.inventory, Context, Index);
                    }
                }
                else if(AllowInteraction)
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
                case ButtonIDs.Housing:
                    return "Housing";
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
            Quests = 8,
            Housing = 9
        }
    }
}