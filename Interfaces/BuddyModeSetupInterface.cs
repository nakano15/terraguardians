using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.ModLoader;
using Terraria.Localization;

namespace terraguardians
{
    public class BuddyModeSetupInterface : LegacyGameInterfaceLayer
    {
        public static bool IsActive { get { return WindowActive; } }
        private static Vector2 WindowPosition = Vector2.Zero;
        private static int WindowWidth = 0, WindowHeight = 0;
        private static bool WindowActive = false;
        private static Companion DisplayCompanion = null;
        private static CompanionID[] PossibleCompanionIDs = new CompanionID[0];
        private static string[] PossibleCompanionNames = new string[0];
        private static readonly string[] GameModeInfo = new string[]{
            "The Buddies Mode is a special game mode where you pick a companion you",
            "will play the game with since the beggining of your adventure, until forever.",
            "The companion you pick ranges from starter companions, to companions met by other player characters.",
            "Becareful, having extra companions with your will reduce the buddy mode benefits.",
            "You have 12 in-game hours (12 real life minutes) to decide if you want to try It or not."
        };
        private static string[] CompanionDescription = new string[0];
        private static int MaxLines = 0;
        private static int Selected = -1, MenuScroll = 0;
        const string InterfaceKey = "Mods.terraguardians.Interface.BuddyModeInterface.";

        public BuddyModeSetupInterface() :
            base("TerraGuardians: Buddy Mode Setup", DrawInterface, InterfaceScaleType.UI)
        {
            
        }

        public static void Open(string SpecificModID = null)
        {
            Main.playerInventory = false;
            WindowPosition.X = Main.screenWidth * 0.2f;
            WindowPosition.Y = Main.screenHeight * 0.2f;
            WindowWidth = Main.screenWidth - (int)WindowPosition.X * 2;
            WindowHeight = Main.screenHeight - (int)WindowPosition.Y * 2;
            PossibleCompanionIDs = MainMod.GetPossibleStarterCompanions();
            PossibleCompanionNames = new string[PossibleCompanionIDs.Length];
            for (int i = 0; i < PossibleCompanionIDs.Length; i++)
            {
                CompanionBase cb = MainMod.GetCompanionBase(PossibleCompanionIDs[i]);
                if(cb.IsInvalidCompanion)
                    PossibleCompanionNames[i] = "Corrupted Memory";
                else
                    PossibleCompanionNames[i] = cb.DisplayName;
            }
            WindowActive = true;
            Selected = -1;
            MenuScroll = 0;
        }

        public static void Close()
        {
            PossibleCompanionIDs = new CompanionID[0];
            PossibleCompanionNames = new string[0];
            DisplayCompanion = null;
            WindowActive = false;
            Selected = -1;
            MenuScroll = 0;
        }

        public static bool DrawInterface()
        {
            if (!WindowActive) return true;
            if (Main.playerInventory)
            {
                Main.playerInventory = false;
                WindowActive = false;
                return true;
            }
            Player player = MainMod.GetLocalPlayer;
            if (Main.mouseX >= WindowPosition.X && Main.mouseX < WindowPosition.X + WindowWidth && 
                Main.mouseY >= WindowPosition.Y && Main.mouseY < WindowPosition.Y + WindowHeight)
            {
                player.mouseInterface = true;
            }
            DrawRectangle(WindowPosition.X - 4, WindowPosition.Y - 4, WindowWidth + 8, WindowHeight + 8, Color.Black);
            DrawRectangle(WindowPosition.X, WindowPosition.Y, WindowWidth, WindowHeight, Color.Green);

            Vector2 DrawPosition = new Vector2(WindowPosition.X + WindowWidth * 0.5f,
                WindowPosition.Y - 2);
            Utils.DrawBorderString(Main.spriteBatch, Language.GetTextValue(InterfaceKey + "Title"), DrawPosition, Color.Yellow, 1.2f, 0.5f);
            DrawPosition.Y += 28;
            for (int i = 0; i < 5; i++)
            {
                Utils.DrawBorderString(Main.spriteBatch, Language.GetTextValue(InterfaceKey + "line" + (i + 1)), DrawPosition, Color.White, 0.9f, 0.5f);
                DrawPosition.Y += 26;
            }
            /*foreach(string s in GameModeInfo)
            {
                Utils.DrawBorderString(Main.spriteBatch, s, DrawPosition, Color.White, 0.9f, 0.5f);
                DrawPosition.Y += 26;
            }*/
            DrawPosition.X = WindowPosition.X + 2;
            float ElementStartPosY = DrawPosition.Y;
            int MenuWidth = 228, MenuHeight = (int)(WindowPosition.Y - DrawPosition.Y + WindowHeight - 2);
            DrawRectangle(DrawPosition.X, DrawPosition.Y, MenuWidth, MenuHeight, Color.LightBlue);
            int MaxElements = MenuHeight / 30;
            bool PreviouslyPickedSomeone = false;
            for (int i = 0; i < MaxElements; i++)
            {
                int Index = i + MenuScroll;
                byte MenuElement = 1; //0 = Up Arrow, 1 = Companion, 2 = Down Arrow
                if (MenuScroll > 0 && i == 0)
                {
                    MenuElement = 0;
                }
                if (MenuScroll + MaxElements < PossibleCompanionIDs.Length && i == MaxElements - 1)
                {
                    MenuElement = 2;
                }
                if (Index < 0 || i >= PossibleCompanionIDs.Length) continue;
                Vector2 OptionPosition = DrawPosition;
                OptionPosition.X += MenuWidth * 0.5f;
                OptionPosition.Y += 30 * i;
                string Text = "";
                switch(MenuElement)
                {
                    case 0:
                        Text = Language.GetTextValue("Mods.terraguardians.Interface.ScrollUp");
                        break;
                    case 2:
                        Text = Language.GetTextValue("Mods.terraguardians.Interface.ScrollDown");
                        break;
                    case 1:
                        Text = PossibleCompanionNames[Index];
                        break;
                }
                if (DrawTextButton(Text, OptionPosition) && !PreviouslyPickedSomeone)
                {
                    PreviouslyPickedSomeone = true;
                    switch(MenuElement)
                    {
                        case 0:
                            MenuScroll--;
                            break;
                        case 2:
                            MenuScroll ++;
                            break;
                        case 1:
                            if (DisplayCompanion == null || !DisplayCompanion.GetCompanionID.IsSameID(PossibleCompanionIDs[Index]))
                            {
                                CompanionBase cb = MainMod.GetCompanionBase(PossibleCompanionIDs[Index]);
                                DisplayCompanion = cb.GetCompanionObject;
                                DisplayCompanion.Data = cb.CreateCompanionData;
                                DisplayCompanion.Data.IsStarter = true;
                                DisplayCompanion.Data.ChangeCompanion(PossibleCompanionIDs[Index].ID, PossibleCompanionIDs[Index].ModID, true);
                                DisplayCompanion.Data.AssignGenericID();
                                DisplayCompanion.InitializeCompanion();
                                DisplayCompanion.ChangeDir(1);
                                DisplayCompanion.active = true;
                                CompanionDescription = Utils.WordwrapString(cb.Description, FontAssets.MouseText.Value, WindowWidth - MenuWidth - 4, 5, out MaxLines);
                            }
                            Selected = Index;
                            break;
                    }
                }
            }
            DrawPosition.X += MenuWidth + 2 + (WindowWidth - MenuWidth) * 0.5f;
            DrawPosition.Y = WindowPosition.Y + WindowHeight - 152;
            if (Selected > -1 && DisplayCompanion != null)
            {
                Vector2 CompanionDrawPos = DrawPosition + Main.screenPosition;
                Utils.DrawBorderString(Main.spriteBatch, PossibleCompanionNames[Selected], DrawPosition, Color.White, 1.1f, 0.5f);
                Vector2 DescriptionPosition = CompanionDrawPos;
                for(int i = 0; i <= MaxLines; i++)
                {
                    DescriptionPosition.Y += 28;
                    Utils.DrawBorderString(Main.spriteBatch, CompanionDescription[i], DescriptionPosition, Color.White, 1, 0.5f);
                }
                CompanionDrawPos.X -= DisplayCompanion.width * 0.5f;
                CompanionDrawPos.Y -= DisplayCompanion.height;
                CompanionDrawPos.X = (int)CompanionDrawPos.X;
                CompanionDrawPos.Y = (int)CompanionDrawPos.Y;
                DisplayCompanion.position = CompanionDrawPos;
                DisplayCompanion.DrawCompanionInterfaceOnly(UseSingleDrawScript: true);
            }
            DrawPosition.X = WindowPosition.X + WindowWidth * 0.5f;
            DrawPosition.Y = WindowPosition.Y + WindowHeight - 22f;
            if (Selected > -1 && !DisplayCompanion.Base.IsInvalidCompanion && 
                DrawTextButton(Language.GetTextValue(InterfaceKey + "PickBuddy"), DrawPosition, 1.2f))
            {
                if (player.GetModPlayer<PlayerMod>().SetPlayerBuddy(PossibleCompanionIDs[Selected]))
                {
                    Close();
                    int PortraitID = ModContent.ItemType<Items.Consumables.PortraitOfAFriend>();
                    for(int i = 0; i < player.inventory.Length; i++)
                    {
                        if (player.inventory[i].type == PortraitID)
                            player.inventory[i].SetDefaults(0);
                    }
                    if (Main.mouseItem.type == PortraitID)
                        Main.mouseItem.SetDefaults(0);
                    return true;
                }
            }
            DrawPosition.X += 120;
            if (DrawTextButton(Language.GetTextValue("Mods.terraguardians.Interface.Close"), DrawPosition, 1.2f))
                Close();
            return true;
        }

        private static bool DrawTextButton(string Text, Vector2 Position, float Scale = 1f)
        {
            Vector2 Dimension = Utils.DrawBorderString(Main.spriteBatch, Text, Position, Color.White, Scale, 0.5f);
            if (Main.mouseX >= Position.X - Dimension.X * 0.5f && Main.mouseX < Position.X + Dimension.X * 0.5f && 
                Main.mouseY >= Position.Y && Main.mouseY < Position.Y + Dimension.Y)
            {
                Utils.DrawBorderString(Main.spriteBatch, Text, Position, Color.Yellow, Scale, 0.5f);
                if (Main.mouseLeft && Main.mouseLeftRelease) return true;
            }
            return false;
        }

        private static void DrawRectangle(float x, float y, int width, int height, Color c)
        {
            MainMod.DrawBackgroundPanel(new Vector2(x, y), width, height, c);
        }

        internal static void Unload()
        {
            DisplayCompanion = null;
            PossibleCompanionIDs = null;
            PossibleCompanionNames = null;
            CompanionDescription = null;
        }
    }
}