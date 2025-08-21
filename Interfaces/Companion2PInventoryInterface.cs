using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ID;

namespace terraguardians
{
    public class Companion2PInventoryInterface : LegacyGameInterfaceLayer
    {
        static EquipTabs EquipTab = EquipTabs.Inventory;
        static int SelectedSlot = 0;
        static int SelectedEquip = 0;
        static int HeldSlot = -1;
        static EquipTabs HeldItemTab = EquipTabs.Inventory;
        static bool IsEquipManagement = false, CanEquip = false;
        public Companion2PInventoryInterface() : 
            base("TerraGuardians: 2P Inventory Interface", DrawInterface, InterfaceScaleType.Game)
        {
            
        }

        public static bool DrawInterface()
        {
            Companion LeaderCompanion = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer);
            if (LeaderCompanion != null)
            {
                if (MainMod.Gameplay2PInventory)
                {
                    Vector2 Position = Main.playerInventory ? new Vector2(178 + 18f, 480 + 36f) : new Vector2(232, 154 + 72f);
                    switch (EquipTab)
                    {
                        case EquipTabs.Inventory:
                            DrawInventory(LeaderCompanion, Position);
                            break;
                        case EquipTabs.Equipment:
                            DrawEquipments(LeaderCompanion, Position);
                            break;
                    }
                    DrawGuideText(Position);
                }
                else
                {
                    HeldSlot = -1;
                }
            }
            return true;
        }

        static void DrawEquipments(Companion LeaderCompanion, Vector2 Position)
        {
            int TabX = SelectedEquip / 10;
            int Row = SelectedEquip - TabX * 10; 
            for (int i = 2; i >= 0; i--)
            {
                for (int o = -1; o <= 1; o += 2)
                {
                    if (o == 1 && i == 0) continue;
                    int EquipSlot = Row + i * o;
                    if (EquipSlot < 0 || EquipSlot >= 10)
                    {
                        continue;
                    }
                    for (byte Slot = 0; Slot < 3; Slot++)
                    {
                        int Index = EquipSlot;
                        int context = 8;
                        Item[] slots = LeaderCompanion.armor;
                        Vector2 SlotPosition = Position + new Vector2(36f * Slot, 36f * i * o);
                        if (EquipSlot > 2)
                        {
                            context = 10;
                        }
                        switch (Slot)
                        {
                            case 1:
                                context ++;
                                Index += 10;
                                break;
                            case 2:
                                context = 12;
                                slots = LeaderCompanion.dye;
                                break;
                        }
                        if (i == 0 && Slot == TabX)
                        {
                            continue;
                        }
                        ItemSlot.Draw(Main.spriteBatch, slots, context, Index, SlotPosition);
                    }
                }
            }
            {
                Item[] slots = TabX == 2 ? LeaderCompanion.dye : LeaderCompanion.armor;
                int Slot = SelectedEquip;
                if (TabX == 2)
                    Slot -= 20;
                ItemSlot.Draw(Main.spriteBatch, ref slots[Slot], ItemSlot.Context.ShopItem, Position + Vector2.UnitX * TabX * 36f);
            }
            DrawHeldSlot(Position + Vector2.UnitX * TabX * 36f, CanEquip ? Color.White : Color.Red);
        }

        static void DrawInventory(Companion LeaderCompanion, Vector2 Position)
        {
            int SlotX = SelectedSlot, SlotY = 0;
            if (SlotX >= 10)
            {
                SlotY = SlotX / 10;
                SlotX -= SlotY * 10;
            }
            const float SlotDimension = 36f;
            for (int y = 2; y >= 0; y--)
            {
                for (int x = 2; x >= 0; x--)
                {
                    if (x == 0 && y == 0) continue;
                    for (int yo = -1; yo <= 1; yo += 2)
                    {
                        if (y == 0 && yo == 1) continue;
                        for (int xo = -1; xo <= 1; xo += 2)
                        {
                            if (x == 0 && xo == 1) continue;
                            int SX = SlotX + x * xo;
                            int SY = SlotY + y * yo;
                            Vector2 SlotPosition = Position + new Vector2((SlotDimension * x) * xo, (SlotDimension * y) * yo);
                            if (SX >= 0 && SX < 10 && SY >= 0 && SY < 5)
                            {
                                int Index = SX + SY * 10;
                                ItemSlot.Draw(Main.spriteBatch, LeaderCompanion.inventory, ItemSlot.Context.InventoryItem, Index, SlotPosition);
                            }
                        }
                    }
                }
            }
            ItemSlot.Draw(Main.spriteBatch, ref LeaderCompanion.inventory[SelectedSlot], ItemSlot.Context.ShopItem, Position);
            DrawHeldSlot(Position);
        }

        static void DrawGuideText(Vector2 Position)
        {
            Position.Y += 36 * 4;
            ChatManager.DrawColorCodedString(Main.spriteBatch, FontAssets.MouseText.Value, GuideText, Position, Color.White, 0f, Vector2.Zero, Vector2.One);
        }

        const string GuideText = "[g:16][g:13][g:15][g:14] = Move | [g:0] = Hold/Swap | [g:1] = Cancel | [g:8] = Previous Menu | [g:9] = Next Menu | [g:4] = Close Menu";

        static void DrawHeldSlot(Vector2 Position, Color color = default(Color))
        {
            if (HeldSlot > -1)
            {
                Position += Vector2.One * 18f;
                Item[] slots;
                int slot;
                GetHeldItemSlots(out slots, out slot);
                ItemSlot.Draw(Main.spriteBatch, slots, ItemSlot.Context.MouseItem, slot, Position);
            }
        }

        internal static void UpdateInterface()
        {
            if (MainMod.Gameplay2PInventory)
            {
                Companion LeaderCompanion = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer);
                switch (EquipTab)
                {
                    case EquipTabs.Inventory:
                        if (MainMod.Is2PButtonPressed(Buttons.DPadUp))
                        {
                            SelectedSlot -= 10;
                        }
                        if (MainMod.Is2PButtonPressed(Buttons.DPadDown))
                        {
                            SelectedSlot += 10;
                        }
                        if (MainMod.Is2PButtonPressed(Buttons.DPadLeft))
                        {
                            SelectedSlot--;
                        }
                        if (MainMod.Is2PButtonPressed(Buttons.DPadRight))
                        {
                            SelectedSlot++;
                        }
                        if (SelectedSlot < 0)
                            SelectedSlot += 50;
                        if (SelectedSlot >= 50)
                            SelectedSlot -= 50;
                        break;
                    case EquipTabs.Equipment:
                        bool Changed = false;
                        if (MainMod.Is2PButtonPressed(Buttons.DPadUp))
                        {
                            SelectedEquip--;
                            Changed = true;
                        }
                        if (MainMod.Is2PButtonPressed(Buttons.DPadDown))
                        {
                            SelectedEquip++;
                            Changed = true;
                        }
                        if (MainMod.Is2PButtonPressed(Buttons.DPadLeft))
                        {
                            SelectedEquip -= 10;
                            Changed = true;
                        }
                        if (MainMod.Is2PButtonPressed(Buttons.DPadRight))
                        {
                            SelectedEquip += 10;
                            Changed = true;
                        }
                        if (SelectedEquip < 0)
                            SelectedEquip += 30;
                        if (SelectedEquip >= 30)
                            SelectedEquip -= 30;
                        if (Changed)
                        {
                            if (HeldSlot > -1)
                            {
                                CanEquip = false;
                                Item Held = GetHeldItem();
                                if (SelectedEquip < 20)
                                {
                                    switch (SelectedEquip % 10)
                                    {
                                        case 0:
                                            CanEquip = Held.headSlot >= 0;
                                            break;
                                        case 1:
                                            CanEquip = Held.bodySlot >= 0;
                                            break;
                                        case 2:
                                            CanEquip = Held.legSlot >= 0;
                                            break;
                                        default:
                                            CanEquip = Held.accessory;
                                            for(byte a = 3; a < 9; a++)
                                            {
                                                if(LeaderCompanion.armor[a].type == Held.type)
                                                {
                                                    CanEquip = false;
                                                    break;
                                                }
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    CanEquip = Held.dye > 0;
                                }
                            }
                            else
                            {
                                CanEquip = true;
                            }
                        }
                        break;
                }
                if (MainMod.Is2PButtonPressed(Buttons.RightShoulder))
                {
                    EquipTab++;
                    if (EquipTab >= EquipTabs.Total)
                    {
                        EquipTab = EquipTabs.Min + 1;
                    }
                }
                if (MainMod.Is2PButtonPressed(Buttons.LeftShoulder))
                {
                    EquipTab--;
                    if (EquipTab == EquipTabs.Min)
                    {
                        EquipTab = EquipTabs.Total - 1;
                    }
                }
                if (MainMod.Is2PButtonPressed(Buttons.A))
                {
                    switch (EquipTab)
                    {
                        case EquipTabs.Inventory:
                            if (HeldSlot == -1)
                            {
                                if (LeaderCompanion.inventory[SelectedSlot].type > ItemID.None)
                                {
                                    SetHeldSlot(SelectedSlot);
                                }
                            }
                            else
                            {
                                if (!CheckIfCantSwapHeldItem() && !CheckIfCantSwapItem(SelectedSlot))
                                {
                                    SwapForHeldItem(ref LeaderCompanion.inventory[SelectedSlot]);
                                    HeldSlot = -1;
                                }
                            }
                            break;
                        case EquipTabs.Equipment:
                            if (HeldSlot == -1)
                            {
                                if (SelectedEquip < 20)
                                {
                                    if (LeaderCompanion.armor[SelectedEquip].type > ItemID.None)
                                    {
                                        SetHeldSlot(SelectedEquip);
                                    }
                                }
                                else
                                {
                                    if (LeaderCompanion.dye[SelectedEquip - 20].type > 0)
                                    {
                                        SetHeldSlot(SelectedEquip);
                                    }
                                }
                            }
                            else
                            {
                                if (CanEquip)
                                {
                                    SwapForHeldItem(ref LeaderCompanion.armor[SelectedEquip]);
                                }
                                HeldSlot = -1;
                            }
                            break;
                    }
                }
                if (MainMod.Is2PButtonPressed(Buttons.B))
                {
                    HeldSlot = -1;
                }
            }
        }

        static bool CheckIfCantSwapHeldItem()
        {
            return HeldItemTab == EquipTabs.Inventory && CheckIfCantSwapItem(HeldSlot);
        }

        static bool CheckIfCantSwapItem(int Slot)
        {
            Companion LeaderCompanion = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer);
            return LeaderCompanion.selectedItem == Slot && LeaderCompanion.itemAnimation > 0;
        }

        static void SetHeldSlot(int Index)
        {
            HeldSlot = Index;
            HeldItemTab = EquipTab;
        }

        static void SwapItem(ref Item Item1, ref Item Item2)
        {
            Item Held = Item2;
            Item2 = Item1;
            Item1 = Held;
        }

        static void SwapForHeldItem(ref Item item)
        {
            Item[] Slots;
            int Slot;
            GetHeldItemSlots(out Slots, out Slot);
            if (Slots[Slot] == item) return;
            Item Held = Slots[Slot];
            Slots[Slot] = item;
            item = Held;
        }

        static Item GetHeldItem()
        {
            if (HeldSlot > -1)
            {
                Item[] Slots;
                int Slot;
                GetHeldItemSlots(out Slots, out Slot);
                return Slots[Slot];
            }
            return null;
        }

        static void GetHeldItemSlots(out Item[] Slots, out int Index)
        {
            if (HeldSlot > -1)
            {
                switch (HeldItemTab)
                {
                    case EquipTabs.Inventory:
                        Slots = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer).inventory;
                        Index = HeldSlot;
                        return;
                    case EquipTabs.Equipment:
                        if (HeldSlot >= 20)
                        {
                            Index = HeldSlot - 20;
                            Slots = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer).dye;
                            return;
                        }
                        Slots = PlayerMod.GetPlayerLeaderCompanion(MainMod.GetLocalPlayer).armor;
                        Index = HeldSlot;
                        return;
                }
            }
            Slots = null;
            Index = -1;
        }

        enum EquipTabs : byte
        {
            Min = 0,
            Inventory = 1,
            Equipment = 2,
            Total = 3
            //Misc = 2,

        }
    }
}