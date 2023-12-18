using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using ReLogic.Text;
using ReLogic.Content;
using ReLogic.Graphics;

namespace terraguardians
{
    public class CompanionInventoryInterface : LegacyGameInterfaceLayer
    {
        private static int SelectedCompanion = -1;
        private static ButtonIDs SelectedButton = 0;
        private static byte SelectedSubButton = 0;
        static int SkinOutfitScroll = 0;
        public static ButtonIDs GetCurrentButton { get { return SelectedButton; } }
        internal static short CompanionToMoveHouse = -1;
        private static CompanionSkillData[] SkillDatas = new CompanionSkillData[0];
        static string[] SkinName = new string[0], OutfitName = new string[0];
        static KeyValuePair<byte, string>[] SkinID = new KeyValuePair<byte, string>[0], OutfitID = new KeyValuePair<byte, string>[0];
        static bool LastWasOpened = false;
        static int SubAttackDisplayCount = -1;
        static byte HeldSubAttackID = 255;

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
            bool Visible = Main.playerInventory && !Main.CreativeMenu.Enabled && Main.npcShop == 0 && !Main.InReforgeMenu && MainMod.GetLocalPlayer.chest == -1;
            if (!Visible) 
            {
                SelectedButton = 0;
                SelectedSubButton = 0;
                LastWasOpened = false;
                SubAttackDisplayCount = -1;
                HeldSubAttackID = 255;
                CompanionToMoveHouse = -1;
                return true;
            }
            if (!LastWasOpened)
            {
                UpdateCompanionInfos(SelectedCompanion == -1 ? null : Main.LocalPlayer.GetModPlayer<PlayerMod>().GetSummonedCompanions[SelectedCompanion]);
            }
            LastWasOpened = Visible;
            if (MainMod.GetLocalPlayer.chest > -1)
                return true;
            string MouseText = "";
            const float StartX = 68;
            const float ButtonSize = 0.7f;
            Vector2 ButtonStartPosition = new Vector2(StartX, 267);
            if (MainMod.StarlightRiverModInstalled) ButtonStartPosition.Y += 96;
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
                        PlayerMod.DrawPlayerHeadInterface(Companions[i], ButtonStartPosition + Vector2.UnitX * 18 + Vector2.UnitY * (18 + 6 * (i == SelectedCompanion ? -1 : 1)), false, 1, 36);
                        //Companions[i].DrawCompanionHead(ButtonStartPosition + Vector2.UnitX * 18 + Vector2.UnitY * (18 + 6 * (i == SelectedCompanion ? -1 : 1)), false, 1, 36);
                        if(Main.mouseX >= ButtonStartPosition.X && Main.mouseX < ButtonStartPosition.X + 36 && Main.mouseY >= ButtonStartPosition.Y && Main.mouseY < ButtonStartPosition.Y + 48)
                        {
                            MouseText = Companions[i].name;
                            Player.Player.mouseInterface = true;
                            if(Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                HeldSubAttackID = 255;
                                if (i == SelectedCompanion)
                                {
                                    SelectedCompanion = -1;
                                    SelectedButton = ButtonIDs.None;
                                }
                                else
                                {
                                    SelectedCompanion = i;
                                    companion = Companions[SelectedCompanion];
                                    UpdateCompanionInfos(companion);
                                }
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
                if (companion is TerraGuardian) Buttons.Add(ButtonIDs.Skills);
                Buttons.Add(ButtonIDs.SubAttack);
            }
            Buttons.Add(ButtonIDs.Requests);
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
                        SelectedSubButton = 0;
                        Main.InGuideCraftMenu = false;
                        CompanionToMoveHouse = -1;
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
                        const byte EquipTab = 0, MiscEquipTab = 1, SkinsTab = 2, OutfitsTab = 3;
                        DynamicSpriteFont Font = FontAssets.MouseText.Value;
                        float SlotSize = 56 * Main.inventoryScale;
                        {
                            Vector2 TabPosition = new Vector2(ButtonStartPosition.X, ButtonStartPosition.Y);
                            for (byte i = 0; i < 4; i++)
                            {
                                string Text = "";
                                switch(i)
                                {
                                    case 0:
                                        Text = "Equipment";
                                        break;
                                    case 1:
                                        Text = "Misc. Equip.";
                                        break;
                                    case 2:
                                        Text = "Skins";
                                        break;
                                    case 3:
                                        Text = "Outfits";
                                        break;
                                }
                                Vector2 Dimension = Font.MeasureString(Text);
                                Color c = SelectedSubButton == i ? Color.Yellow : Color.White;
                                if (Main.mouseX >= TabPosition.X + 2 && Main.mouseX < TabPosition.X + Dimension.X - 2 && 
                                    Main.mouseY >= TabPosition.Y && Main.mouseY < TabPosition.Y + 20f)
                                {
                                    c = Color.Cyan;
                                    MainMod.GetLocalPlayer.mouseInterface = true;
                                    if (Main.mouseLeft && Main.mouseLeftRelease)
                                    {
                                        SelectedSubButton = i;
                                        SkinOutfitScroll = 0;
                                    }
                                }
                                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)TabPosition.X, (int)TabPosition.Y - 2, (int)Dimension.X + 4, (int)24), null, Color.Black);
                                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)TabPosition.X + 2, (int)TabPosition.Y, (int)Dimension.X, (int)20), null, Color.Blue);
                                Utils.DrawBorderString(Main.spriteBatch, Text, TabPosition + new Vector2(Dimension.X * 0.5f + 2, 10), c, 1, 0.5f, 0.5f);
                                TabPosition.X += Dimension.X + 4;
                            }
                            ButtonStartPosition.Y += 30;
                        }
                        switch (SelectedSubButton)
                        {
                            case EquipTab:
                                {
                                    string SetBonusBackup = Main.LocalPlayer.setBonus;
                                    Item[] PlayerArmorBackup = Main.LocalPlayer.armor;
                                    Main.LocalPlayer.armor = companion.armor;
                                    byte ExtraSlotsCount = 0;
                                    if (companion.CanDemonHeartAccessoryBeShown()) ExtraSlotsCount++;
                                    if (companion.CanMasterModeAccessoryBeShown()) ExtraSlotsCount++;
                                    byte Rows = 1;
                                    Vector2 SlotPosition = new Vector2(ButtonStartPosition.X, ButtonStartPosition.Y + 20);
                                    float SlotStartPosition = ButtonStartPosition.X;
                                    for (int s = 0; s < 10; s++)
                                    {
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
                                                if (Slot == 0 && s < 3)
                                                    Main.LocalPlayer.setBonus = companion.setBonus;
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
                                        SlotPosition.X = SlotStartPosition;
                                        SlotPosition.Y += SlotSize;
                                        //Vector2 SlotPosition = new Vector2(ButtonStartPosition.X, ButtonStartPosition.Y + s * SlotSize + 20);
                                        while(SlotPosition.Y + SlotSize >= Main.screenHeight)
                                        {
                                            SlotPosition.X += (56 + 20) * 3 * Main.inventoryScale;
                                            SlotStartPosition = SlotPosition.X;
                                            SlotPosition.Y -= SlotPosition.Y - ButtonStartPosition.Y;
                                            Rows++;
                                        }
                                    }
                                    Main.LocalPlayer.armor = PlayerArmorBackup;
                                    ButtonStartPosition.X += (56 + 20) * 3 * Rows * Main.inventoryScale + 8;
                                    ButtonStartPosition.Y = Main.screenHeight - 40;
                                    Utils.DrawBorderString(Main.spriteBatch, "Defense: " + companion.statDefense, ButtonStartPosition, Color.White, 0.75f);
                                    ButtonStartPosition.Y -= 20;
                                    Utils.DrawBorderString(Main.spriteBatch, "Defense Rate: " + System.Math.Round(companion.DefenseRate, 2) + "%", ButtonStartPosition, Color.White, 0.75f);
                                    ButtonStartPosition.Y -= 20;
                                    Utils.DrawBorderString(Main.spriteBatch, "Dodge Rate: " + System.Math.Round(companion.DodgeRate, 1) + "%", ButtonStartPosition, Color.White, 0.75f);
                                    ButtonStartPosition.Y -= 20;
                                    Utils.DrawBorderString(Main.spriteBatch, "Block Rate: " + System.Math.Round(companion.BlockRate, 1) + "%", ButtonStartPosition, Color.White, 0.75f);
                                    ButtonStartPosition.Y -= 20;
                                    Utils.DrawBorderString(Main.spriteBatch, "Accuracy: " + System.Math.Round(companion.Accuracy * 100) + "%", ButtonStartPosition, Color.White, 0.75f);
                                    ButtonStartPosition.Y -= 20;
                                    Utils.DrawBorderString(Main.spriteBatch, "Trigger Rate: " + System.Math.Round(companion.Trigger) + "%", ButtonStartPosition, Color.White, 0.75f);
                                }
                                break;
                            case MiscEquipTab:
                                {
                                    Utils.DrawBorderString(Main.spriteBatch, "Look at that!\n  There's nothing here!", ButtonStartPosition, Color.White);
                                }
                                break;
                            case SkinsTab:
                            case OutfitsTab:
                                {
                                    bool IsOutfit = SelectedSubButton == OutfitsTab;
                                    ButtonStartPosition.Y += Utils.DrawBorderString(Main.spriteBatch, IsOutfit ? "Outfits" : "Skins", ButtonStartPosition, Color.White).Y + 6;
                                    int SkinsRows = (Main.screenHeight - (int)ButtonStartPosition.Y) / 20;
                                    bool ShowUpButton = SkinOutfitScroll > 0, ShowDownButton = SkinOutfitScroll + SkinsRows < (IsOutfit ? OutfitID.Length : SkinID.Length);
                                    for (int i = 0; i < SkinsRows; i++)
                                    {
                                        byte ButtonFunction = 0;
                                        const byte UpButton = 1, DownButton = 2;
                                        string Text = "", Prefix = "";
                                        int Index = i + SkinOutfitScroll;
                                        bool SkinActive = false, Available = true;
                                        if (i == 0 && ShowUpButton)
                                        {
                                            Text = " = Up = ";
                                            ButtonFunction = UpButton;
                                        }
                                        else if (i == SkinsRows - 1 && ShowDownButton)
                                        {
                                            Text = " = Down = ";
                                            ButtonFunction = DownButton;
                                        }
                                        else
                                        {
                                            if (Index >= (IsOutfit ? OutfitID.Length : SkinID.Length)) break;
                                            SkinActive = IsOutfit ? companion.IsOutfitActive(OutfitID[Index].Key, OutfitID[Index].Value) : companion.IsSkinActive(SkinID[Index].Key, SkinID[Index].Value);
                                            Prefix = SkinActive ? "[ON]" : "[OFF]";
                                            Text = IsOutfit ? OutfitName[Index] : SkinName[Index];
                                            Available = SkinActive || (IsOutfit ? companion.Base.GetOutfit(OutfitID[Index].Key, OutfitID[Index].Value).Availability(companion) : companion.Base.GetSkin(SkinID[Index].Key, SkinID[Index].Value).Availability(companion));
                                        }
                                        Vector2 Dimension = Font.MeasureString(Prefix + Text);
                                        Vector2 DrawPos = ButtonStartPosition + new Vector2(0, i * 20);
                                        Color c = Available ? Color.White : Color.Gray;
                                        if (Main.mouseX >= DrawPos.X && Main.mouseX < DrawPos.X + Dimension.X && 
                                            Main.mouseY >= DrawPos.Y && Main.mouseY < DrawPos.Y + 20)
                                        {
                                            MainMod.GetLocalPlayer.mouseInterface = true;
                                            if (ButtonFunction == 0)
                                            {
                                                if (IsOutfit)
                                                {
                                                    MouseText = companion.Base.GetOutfit(OutfitID[Index].Key, OutfitID[Index].Value).Description;
                                                }
                                                else
                                                {
                                                    MouseText = companion.Base.GetSkin(SkinID[Index].Key, SkinID[Index].Value).Description;
                                                }
                                            }
                                            if (Available)
                                            {
                                                c = Color.Yellow;
                                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                                {
                                                    switch(ButtonFunction)
                                                    {
                                                        default:
                                                            {
                                                                if (SkinActive)
                                                                {
                                                                    if (IsOutfit)
                                                                        companion.ChangeOutfit(0);
                                                                    else
                                                                        companion.ChangeSkin(0);
                                                                }
                                                                else
                                                                {
                                                                    if (IsOutfit)
                                                                        companion.ChangeOutfit(OutfitID[Index].Key, OutfitID[Index].Value);
                                                                    else
                                                                        companion.ChangeSkin(SkinID[Index].Key, SkinID[Index].Value);
                                                                }
                                                            }
                                                            break;
                                                        case UpButton:
                                                            SkinOutfitScroll--;
                                                            break;
                                                        case DownButton:
                                                            SkinOutfitScroll++;
                                                            break;
                                                    }
                                                }
                                            }
                                        }
                                        DrawPos.X += Utils.DrawBorderString(Main.spriteBatch, Prefix, DrawPos, SkinActive ? Color.Green : Color.Red).X;
                                        Utils.DrawBorderString(Main.spriteBatch, Text, DrawPos, c);
                                    }
                                }
                                break;
                        }
                        
                        // For Test
                        /*ButtonStartPosition.X += SlotSize * 3 * Rows + 8;
                        ButtonStartPosition.Y = Main.screenHeight - 40;
                        foreach(string n in SkinName)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, n, ButtonStartPosition, Color.White, 0.75f);
                            ButtonStartPosition.Y -= 20;
                        }
                        ButtonStartPosition.X += SlotSize * 3 * Rows + 8;
                        ButtonStartPosition.Y = Main.screenHeight - 40;
                        foreach(string n in OutfitName)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, n, ButtonStartPosition, Color.White, 0.75f);
                            ButtonStartPosition.Y -= 20;
                        }*/
                    }
                    break;

                case ButtonIDs.Requests:
                    {
                        ButtonStartPosition.Y += 20;
                        bool HasRequestActive = false;
                        foreach(uint i in Player.GetCompanionDataKeys)
                        {
                            CompanionData c = Player.GetCompanionDataByIndex(i);
                            RequestData rd = c.GetRequest;
                            if(rd.IsActive)
                            {
                                string Text = rd.GetBase.GetRequestObjective(rd) + " [" + rd.GetTimeLeft() + "]";
                                Utils.DrawBorderString(Main.spriteBatch, Text, ButtonStartPosition, Color.White);
                                ButtonStartPosition.Y += 30;
                                HasRequestActive = true;
                            }
                        }
                        if (!HasRequestActive)
                        {
                            Utils.DrawBorderString(Main.spriteBatch, "No requests active.", ButtonStartPosition, Color.White);
                        }
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
                            PlayerMod.DrawPlayerHeadInterface(c, HousingButtonPosition + Vector2.One * 26 * Main.inventoryScale, false, Main.inventoryScale, 40);
                            //c.DrawCompanionHead(HousingButtonPosition + Vector2.One * 26 * Main.inventoryScale, false, Main.inventoryScale, 40);
                            if (Main.mouseX >= HousingButtonPosition.X && Main.mouseX < HousingButtonPosition.X + HouseButtonSize && 
                                Main.mouseY >= HousingButtonPosition.Y && Main.mouseY < HousingButtonPosition.Y + HouseButtonSize)
                            {
                                MouseText = c.GetName;
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
                    
                case ButtonIDs.Skills:
                    {
                        ButtonStartPosition.Y += 20;
                        foreach(CompanionSkillData skill in SkillDatas)
                        {
                            Vector2 Dimension = Utils.DrawBorderString(Main.spriteBatch, skill.GetSkillInfo, ButtonStartPosition, Color.White, 0.75f);
                            if (Main.mouseX >= ButtonStartPosition.X && Main.mouseX < ButtonStartPosition.X + Dimension.X && 
                                Main.mouseY >= ButtonStartPosition.Y && Main.mouseY < ButtonStartPosition.Y + Dimension.Y)
                            {
                                MouseText = skill.GetDescription;
                            }
                            ButtonStartPosition.Y += 25;
                        }
                    }
                    break;
                
                case ButtonIDs.SubAttack:
                    {
                        ButtonStartPosition.Y += 20;
                        for (int i = 0; i < 4; i++)
                        {
                            const int SlotSize = 36;
                            Vector2 SlotPosition = new Vector2(ButtonStartPosition.X + (SlotSize + 4) * i, ButtonStartPosition.Y);
                            SubAttackData sad = null;
                            byte Index = companion.GetSubAttackIndexFromSlotIndex(i);
                            if (Index < companion.SubAttackList.Count)
                            {
                                sad = companion.SubAttackList[Index];
                            }
                            if (DrawSubattackInfo(SlotPosition, sad, ref MouseText))
                            {
                                if (HeldSubAttackID < 255)
                                {
                                    MouseText = "Assign " + companion.SubAttackList[HeldSubAttackID].GetBase.Name + " here?";
                                }
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    bool Repeated = false;
                                    if (HeldSubAttackID < 255)
                                    {
                                        for (byte e = 0; e < 4; e++)
                                        {
                                            if (companion.SubAttackIndexes[e] == HeldSubAttackID)
                                            {
                                                Repeated = true;
                                                break;
                                            }
                                        }
                                    }
                                    if (!Repeated)
                                    {
                                        companion.SubAttackIndexes[i] = HeldSubAttackID;
                                        if (companion.SelectedSubAttack == i && companion.SubAttackIndexes[i] == 255)
                                        {
                                            for (byte e = 0; e < CompanionData.MaxSubAttackSlots; e++)
                                            {
                                                if (companion.SubAttackIndexes[e] < 255 && companion.SubAttackIndexes[e] < companion.SubAttackList.Count)
                                                {
                                                    companion.SelectedSubAttack = e;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    HeldSubAttackID = 255;
                                }
                            }
                            Utils.DrawBorderString(Main.spriteBatch, (i + 1).ToString(), SlotPosition + Vector2.One * 2, Color.White, .7f);
                            //Need more work.
                            //Need not only to list the sub attacks, but also buttons to scroll through the list when necessary.
                        }
                        ButtonStartPosition.Y += 44;
                        if(SubAttackDisplayCount == -1)
                        {
                            SubAttackDisplayCount = (int)(Main.screenHeight - ButtonStartPosition.Y - 16) / 36;
                        }
                        bool ShowUpButton = SelectedSubButton > 0, ShowDownButton = SelectedSubButton < companion.SubAttackList.Count - SubAttackDisplayCount;
                        for (byte i = 0; i < SubAttackDisplayCount; i++)
                        {
                            byte Index = (byte)(SelectedSubButton + i);
                            if (Index >= companion.SubAttackList.Count)
                                break;
                            Vector2 Position = ButtonStartPosition;
                            Position.Y += 36 * i;
                            if (DrawSubattackInfo(Position, companion.SubAttackList[Index], ref MouseText))
                            {
                                if (Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    HeldSubAttackID = Index;
                                    return true;
                                }
                            }
                            else
                            {
                                Position.X += 36;
                                Utils.DrawBorderString(Main.spriteBatch, companion.SubAttackList[Index].GetBase.Name, Position, Color.White);
                            }
                        }
                    }
                    break;
            }
            if (HeldSubAttackID < 255)
            {
                MainMod.GetLocalPlayer.mouseInterface = true;
                if (Main.mouseLeft && Main.mouseLeftRelease)
                {
                    HeldSubAttackID = 255;
                }
                else
                {
                    DrawSubattackInfo(new Vector2(Main.mouseX + 16, Main.mouseY + 16), HeldSubAttackID < companion.SubAttackList.Count ? companion.SubAttackList[HeldSubAttackID] : null, ref MouseText);
                }
            }
            if(MouseText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
            else if(CompanionToMoveHouse > -1)
            {
                Vector2 HeadPosition = new Vector2(Main.mouseX + 20, Main.mouseY + 20);
                PlayerMod.DrawPlayerHeadInterface(WorldMod.CompanionNPCsInWorld[CompanionToMoveHouse].GetCompanion, HeadPosition, false);
                //WorldMod.CompanionNPCsInWorld[CompanionToMoveHouse].GetCompanion.DrawCompanionHead(HeadPosition, false);
            }
            return true;
        }

        static bool DrawSubattackInfo(Vector2 Position, SubAttackData Data, ref string MouseText)
        {
            const int SlotSize = 36, IconMaxSize = 26;
            Vector2 SlotPosition = new Vector2(Position.X, Position.Y);
            DrawBackgroundPanel(SlotPosition, SlotSize, SlotSize, Color.White);
            bool MouseOver = Main.mouseX >= SlotPosition.X && Main.mouseX < SlotPosition.X + SlotSize && 
                Main.mouseY >= SlotPosition.Y && Main.mouseY < SlotPosition.Y + SlotSize;
            if (MouseOver)
            {
                MainMod.GetLocalPlayer.mouseInterface = true;
            }
            if (Data != null)
            {
                Texture2D texture = Data.GetBase.GetIcon;
                if (texture == null)
                {
                    texture = MainMod.ErrorTexture.Value;
                }
                float Scale = texture.Width;
                if (texture.Height > Scale)
                {
                    Scale = texture.Height;
                }
                Scale = IconMaxSize / Scale;
                SlotPosition.X = (int)(SlotPosition.X + SlotSize * .5f);
                SlotPosition.Y = (int)(SlotPosition.Y + SlotSize * .5f);
                Main.spriteBatch.Draw(texture, SlotPosition, null, Color.White, 0, new Vector2(texture.Width, texture.Height) * .5f, Scale, SpriteEffects.None, 0);
                if (MouseOver)
                {
                    MouseText = "[" + Data.GetBase.Name + "]\n" + Data.GetBase.Description + "\nCooldown: " + Data.GetBase.Cooldown + " Seconds";
                }
            }
            else
            {
                if (MouseOver)
                {
                    MouseText = "No Subattack set.";
                }
            }
            return MouseOver;
        }
        
        private static void DrawBackgroundPanel(Vector2 Position, int Width, int Height, Color color)
        {
            MainMod.DrawBackgroundPanel(Position, Width, Height, color);
        }

        static void UpdateCompanionInfos(Companion companion)
        {
            if (companion != null)
            {
                SkillDatas = companion.GetCommonData.GetSkillDatas();
                companion.Base.GetSkinsList(out SkinName, out SkinID);
                companion.Base.GetOutfitsList(out OutfitName, out OutfitID);
            }
            else
            {
                SkillDatas = null;
                SkinName = null;
                OutfitName = null;
                SkinID = null;
                OutfitID = null;
            }
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

        internal static void Unload()
        {
            SkillDatas = null;
            SkinName = null;
            OutfitName = null;
            SkinID = null;
            OutfitID = null;
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
            Housing = 9,
            SubAttack = 10
        }
    }
}