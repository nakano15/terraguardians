using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSelectionInterface : LegacyGameInterfaceLayer
    {
        private const int WindowWidth = 520, WindowHeight = 360;
        const int ListWidth = 180, ListHeight = WindowHeight - 8;
        const float ListNameSpacing = 20;
        const int ListNameCount = (int)(ListHeight / ListNameSpacing) - 1;
        const int CompanionInfoWidth = WindowWidth - ListWidth - 12;
        private static bool active = false;
        public static bool IsActive { get{ return active; } }
        private static uint[] CompanionIDs = new uint[0];
        private static CompanionData[] CompanionDatas = new CompanionData[0];
        private static Companion DrawCompanion = null;
        private static byte Page = 0;
        private static byte TotalPages = 0;
        private static uint SelectedCompanion = uint.MaxValue;
        private static string[] Description = new string[0];
        private static byte DescriptionMaxLines = 0;
        static float FriendshipExpProgress = .5f;
        static bool IsInvalidCompanion = false;

        public CompanionSelectionInterface() : base("TerraGuardians: Guardian Selection Interface", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static void OpenInterface()
        {
            PlayerMod pm = Main.player[MainMod.MyPlayerBackup].GetModPlayer<PlayerMod>();
            CompanionIDs = pm.GetCompanionDataKeys;
            CompanionDatas = new CompanionData[CompanionIDs.Length];
            for(int i = 0; i < CompanionIDs.Length; i++)
            {
                CompanionDatas[i] = pm.GetCompanionDataByIndex(CompanionIDs[i]);
            }
            ChangeSelectedCompanion(uint.MaxValue);
            Page = 0;
            TotalPages = (byte)(CompanionIDs.Length / ListNameCount);
            active = true;
            Main.playerInventory = false;
        }

        public static void CloseInterface()
        {
            CompanionIDs = null;
            CompanionDatas = null;
            DrawCompanion = null;
            active = false;
            Description = null;
        }

        public static bool DrawInterface()
        {
            if(!active) return true;
            if(Main.playerInventory)
            {
                CloseInterface();
                return true;
            }
            string MouseText = null;
            PlayerMod pm = Main.player[MainMod.MyPlayerBackup].GetModPlayer<PlayerMod>();
            Vector2 WindowPosition = new Vector2((Main.screenWidth - WindowWidth) * 0.5f, (Main.screenHeight - WindowHeight) * 0.5f);
            {
                Vector2 BorderPosition = WindowPosition - Vector2.One * 2;
                int BorderWidth = WindowWidth + 4;
                int BorderHeight = WindowHeight + 4;
                DrawBackgroundPanel(BorderPosition, BorderWidth, BorderHeight, Color.Black);
                if (Main.mouseX >= BorderPosition.X && Main.mouseX < BorderPosition.X + BorderWidth && 
                    Main.mouseY >= BorderPosition.Y && Main.mouseY < BorderPosition.Y + BorderHeight)
                {
                    Main.LocalPlayer.mouseInterface = true;
                }
            }
            DrawBackgroundPanel(WindowPosition, WindowWidth, WindowHeight, Color.Blue);
            DrawCompanionList(WindowPosition + Vector2.One * 4, ref MouseText);
            DrawCompanionInfoInterface(WindowPosition + Vector2.One * 4 + Vector2.UnitX * (ListWidth + 4), pm, ref MouseText);
            DrawCloseButton(WindowPosition + Vector2.UnitX * WindowWidth);
            if (MouseText != null)
            {
                Vector2 MousePos = new Vector2(Main.mouseX + 8, Main.mouseY + 8);
                Utils.DrawBorderString(Main.spriteBatch, MouseText, MousePos, Color.White);
            }
            return true;
        }

        private static void DrawCloseButton(Vector2 CloseButtonPosition)
        {
            bool MouseOver = false;
            const int Padding = 12;
            if (Main.mouseX >= CloseButtonPosition.X - Padding && Main.mouseX < CloseButtonPosition.X + Padding && 
                Main.mouseY >= CloseButtonPosition.Y - Padding && Main.mouseY < CloseButtonPosition.Y + Padding)
            {
                MouseOver = true;
                Main.LocalPlayer.mouseInterface = true;
                if(Main.mouseLeft && Main.mouseLeftRelease)
                {
                    CloseInterface();
                }
            }
            Utils.DrawBorderString(Main.spriteBatch, "X", CloseButtonPosition, MouseOver ? Color.Yellow : Color.Red, anchorx: 0.5f, anchory: 0.5f);
        }

        private static void DrawCompanionInfoInterface(Vector2 StartPosition, PlayerMod pm, ref string MouseText)
        {
            if(DrawCompanion == null)
            {

            }
            else
            {
                {
                    Vector2 NamePanelPosition = StartPosition + Vector2.Zero;
                    DrawBackgroundPanel(NamePanelPosition, CompanionInfoWidth, 30, Color.LightCyan);
                    NamePanelPosition.X += CompanionInfoWidth * 0.5f;
                    NamePanelPosition.Y += 30 + 4;
                    Color c = IsInvalidCompanion ? Color.Red : Color.White;
                    float Width = Utils.DrawBorderString(Main.spriteBatch, DrawCompanion.Data.GetNameWithNickname, NamePanelPosition, c, 1, 0.5f, 1).X;
                    NamePanelPosition.X -= Width * 0.5f + 16;
                    NamePanelPosition.Y -= 18;
                    MainMod.DrawFriendshipHeart(NamePanelPosition, DrawCompanion.FriendshipLevel, FriendshipExpProgress);
                    {
                        NamePanelPosition.X += 32 + Width;
                        NamePanelPosition.Y += 1;
                        Main.spriteBatch.Draw(MainMod.RenamePencilTexture.Value, NamePanelPosition, null, Color.White, 0, Vector2.One * 8, 1f, SpriteEffects.None, 0);
                        if (Main.mouseX >= NamePanelPosition.X - 8 && Main.mouseX < NamePanelPosition.X + 8 && 
                            Main.mouseY >= NamePanelPosition.Y - 8 && Main.mouseY < NamePanelPosition.Y + 8)
                        {
                            MouseText = "Nickname Companion?";
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                Main.NewText("Input the new nickname " + DrawCompanion.Data.GetName +" will get.");
                                Main.OpenPlayerChat();
                                Main.chatText = "/renamecompanion " + DrawCompanion.ID + " \"" + DrawCompanion.ModID + "\" ";
                                CloseInterface();
                            }
                        }
                    }
                }
                {
                    Vector2 CompanionDisplayBackground = StartPosition + Vector2.UnitY * 30;
                    int Height = (int)(ListHeight * 0.5f - 30);
                    DrawBackgroundPanel(CompanionDisplayBackground, CompanionInfoWidth, Height, Color.LightCyan);
                }
                {
                    Vector2 CompanionDrawPosition = StartPosition + Vector2.Zero;
                    CompanionDrawPosition.X += CompanionInfoWidth * 0.5f;
                    CompanionDrawPosition.Y += ListHeight * 0.5f;
                    CompanionDrawPosition.X -= DrawCompanion.width * 0.5f;
                    CompanionDrawPosition.Y -= DrawCompanion.height;
                    CompanionDrawPosition.X = (int)(CompanionDrawPosition.X + Main.screenPosition.X);
                    CompanionDrawPosition.Y = (int)(CompanionDrawPosition.Y + Main.screenPosition.Y);
                    DrawCompanion.position = CompanionDrawPosition - Vector2.UnitY * 2;
                    DrawCompanion.DrawCompanionInterfaceOnly(UseSingleDrawScript: true); //Why aren't you being drawn?
                    if (!IsInvalidCompanion)
                    {
                        Vector2 WikiButtonPosition = StartPosition + new Vector2(CompanionInfoWidth - 30, 50);
                        Color WikiHoverColor = Color.White;
                        if (Main.mouseX >= WikiButtonPosition.X - 15 && Main.mouseX < WikiButtonPosition.X + 15 && 
                            Main.mouseY >= WikiButtonPosition.Y - 10 && Main.mouseY < WikiButtonPosition.Y + 10)
                        {
                            WikiHoverColor = Color.Yellow;
                            if(Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                string URL = "https://nakano15-mods.fandom.com/wiki/" + DrawCompanion.Base.WikiName;
                                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo{ FileName = URL, UseShellExecute = true});
                            }
                        }
                        Utils.DrawBorderString(Main.spriteBatch, "Wiki", WikiButtonPosition, WikiHoverColor, 0.8f, 0.5f, 0.5f);
                    }
                }
                {
                    Vector2 DescriptionPanelPosition = StartPosition + Vector2.Zero;
                    DescriptionPanelPosition.Y += (int)(ListHeight * 0.5f);
                    int Height = (int)(WindowHeight - 38 - ListHeight * 0.5f);
                    DrawBackgroundPanel(DescriptionPanelPosition, CompanionInfoWidth, Height, Color.LightCyan);
                    DescriptionPanelPosition.X += CompanionInfoWidth * 0.5f;
                    for(byte i = 0; i <= DescriptionMaxLines; i++)
                    {
                        if(Description[i] == null)
                            break;
                        Vector2 ThisTextPosition = DescriptionPanelPosition + Vector2.UnitY * (20 * (i + 1) + 6);
                        Utils.DrawBorderString(Main.spriteBatch, Description[i], ThisTextPosition, Color.White, 0.9f, anchorx: 0.5f);
                    }

                }
                {
                    Vector2 ButtonsPosition = StartPosition + Vector2.Zero;
                    ButtonsPosition.Y += WindowHeight - 38;
                    DrawBackgroundPanel(ButtonsPosition, CompanionInfoWidth, 30, Color.LightCyan);
                    ButtonsPosition.X += 4;
                    ButtonsPosition.Y += 4;
                    const int ButtonWidth = (int)(CompanionInfoWidth - 4 - 4) / 3;
                    for(int i = 1; i < 3; i++)
                        Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle((int)(ButtonsPosition.X + ButtonWidth * i), (int)(ButtonsPosition.Y), 2, 22), null, Color.Cyan);
                    if (!IsInvalidCompanion)
                    {
                        { //Call Dismiss Button
                            const byte Call = 0, Dismiss = 1;
                            byte Context = Call;
                            string Text = "Call";
                            if(pm.HasCompanionSummonedByIndex(DrawCompanion.Index))
                            {
                                Context = Dismiss;
                                Text = "Dismiss";
                            }
                            bool MouseOver = false;
                            if(Main.mouseX >= ButtonsPosition.X && Main.mouseX < ButtonsPosition.X + ButtonWidth && Main.mouseY >= ButtonsPosition.Y && Main.mouseY < ButtonsPosition.Y + 30)
                            {
                                MouseOver = true;
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    switch(Context)
                                    {
                                        case Call:
                                            pm.CallCompanionByIndex(DrawCompanion.Index);
                                            break;
                                        case Dismiss:
                                            pm.DismissCompanionByIndex(DrawCompanion.Index);
                                            break;
                                    }
                                }
                            }
                            Vector2 TextPosition = ButtonsPosition + new Vector2(ButtonWidth * 0.5f, 13f);
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, (MouseOver ? Color.Yellow : Color.White), 0.7f, 0.5f, 0.5f);
                        }
                        ButtonsPosition.X += ButtonWidth + 2;
                        /*{ //Invite Or Send Home button
                            const byte MoveIn = 0, SendHome = 1;
                            byte Context = MoveIn;
                            string Text = "Move In";
                            bool MouseOver = false;
                            if(Main.mouseX >= ButtonsPosition.X && Main.mouseX < ButtonsPosition.X + ButtonWidth && Main.mouseY >= ButtonsPosition.Y && Main.mouseY < ButtonsPosition.Y + 30)
                            {
                                MouseOver = true;
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    //Do your magic
                                    switch(Context)
                                    {
                                        case MoveIn:
                                            break;
                                        case SendHome:
                                            break;
                                    }
                                }
                            }
                            Vector2 TextPosition = ButtonsPosition + new Vector2(ButtonWidth * 0.5f, 13f);
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, (MouseOver ? Color.Yellow : Color.White), 0.7f, 0.5f, 0.5f);
                        }*/
                        ButtonsPosition.X += ButtonWidth + 2;
                        /*{ //Third button, no idea what for.
                            byte Context = 0;
                            string Text = "Relationship";
                            bool MouseOver = false;
                            if(Main.mouseX >= ButtonsPosition.X && Main.mouseX < ButtonsPosition.X + ButtonWidth && Main.mouseY >= ButtonsPosition.Y && Main.mouseY < ButtonsPosition.Y + 30)
                            {
                                MouseOver = true;
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {

                                }
                            }
                            Vector2 TextPosition = ButtonsPosition + new Vector2(ButtonWidth * 0.5f, 13f);
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, (MouseOver ? Color.Yellow : Color.White), 0.7f, 0.5f, 0.5f);
                        }*/
                    }
                }
            }
        }

        private static void DrawCompanionList(Vector2 StartPosition, ref string MouseText)
        {
            DrawBackgroundPanel(StartPosition, ListWidth, ListHeight - 30, Color.LightCyan);
            Vector2 NameStartPosition = StartPosition + Vector2.One * 4;
            for(int i = 0; i < ListNameCount; i++)
            {
                int index = i + ListNameCount * Page;
                if(index >= CompanionIDs.Length)
                    break;
                string Text = CompanionDatas[index].GetName;
                Vector2 Position = NameStartPosition + Vector2.UnitY * ListNameSpacing * i;
                bool IsMouseOver = false;
                if(Main.mouseX >= Position.X && Main.mouseX < Position.X + ListWidth && 
                    Main.mouseY >= Position.Y && Main.mouseY < Position.Y + ListNameSpacing)
                {
                    IsMouseOver = true;
                    if(Main.mouseLeft  && Main.mouseLeftRelease)
                    {
                        if(SelectedCompanion == index)
                            ChangeSelectedCompanion(uint.MaxValue);
                        else
                            ChangeSelectedCompanion((uint)index);
                    }
                }
                Position.Y += ListNameSpacing + 6;
                Utils.DrawBorderString(Main.spriteBatch, Text, Position, IsMouseOver ? Color.Cyan : (index == SelectedCompanion ? Color.Yellow : Color.White), 1, 0, 1);
            }
            {
                Vector2 PageDisplayPosition = StartPosition + Vector2.UnitY * (ListHeight - ListNameSpacing);
                bool LeftArrowVisible = Page > 0;
                bool RightArrowVisible = Page < TotalPages;
                {
                    Vector2 ButtonsPosition = StartPosition + Vector2.Zero;
                    ButtonsPosition.Y += WindowHeight - 38;
                    DrawBackgroundPanel(ButtonsPosition, ListWidth, 30, Color.LightCyan);
                }
                if(LeftArrowVisible)
                {
                    bool MouseOver = false;
                    if(Main.mouseX >= PageDisplayPosition.X && Main.mouseX < PageDisplayPosition.X + ListWidth / 2 && 
                        Main.mouseY >= PageDisplayPosition.Y && Main.mouseY < PageDisplayPosition.Y + ListNameSpacing) //Left Arrow
                    {
                        MouseOver = true;
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "<", PageDisplayPosition + Vector2.UnitY * ListNameSpacing * 0.5f, 
                    (MouseOver ? Color.Yellow : Color.White));
                }
                if(RightArrowVisible)
                {
                    bool MouseOver = false;
                    if(Main.mouseX >= PageDisplayPosition.X + ListWidth / 2 && Main.mouseX < PageDisplayPosition.X + ListWidth && 
                        Main.mouseY >= PageDisplayPosition.Y && Main.mouseY < PageDisplayPosition.Y + ListNameSpacing) //Right Arrow
                    {
                        MouseOver = true;
                    }
                    Utils.DrawBorderString(Main.spriteBatch, ">", PageDisplayPosition + Vector2.UnitX * ListWidth + Vector2.UnitY * ListNameSpacing * 0.5f, 
                    (MouseOver ? Color.Yellow : Color.White), anchorx: 1);
                }
                Utils.DrawBorderString(Main.spriteBatch, "Page: " + (Page + 1) + "/" + (TotalPages + 1), PageDisplayPosition + Vector2.UnitX * ListWidth * 0.5f + Vector2.UnitY * ListNameSpacing * 0.5f, 
                Color.White, anchorx: 0.5f, anchory: 0.5f);
            }
        }

        private static void ChangeSelectedCompanion(uint NewCompanion)
        {
            SelectedCompanion = NewCompanion;
            if(NewCompanion >= CompanionIDs.Length)
            {
                DrawCompanion = null;
                Description = null;
                SelectedCompanion = uint.MaxValue;
            }
            else
            {
                DrawCompanion = MainMod.GetCompanionBase(CompanionDatas[SelectedCompanion]).GetCompanionObject;
                DrawCompanion.Data = CompanionDatas[SelectedCompanion];
                IsInvalidCompanion = DrawCompanion.Base.IsInvalidCompanion;
                DrawCompanion.InitializeCompanion();
                DrawCompanion.active = true;
                DrawCompanion.ChangeDir(1);
                for(int i = 0; i < 20; i++)
                    DrawCompanion.UpdateEquips(i);
                FriendshipExpProgress = DrawCompanion.GetFriendshipProgress;
                string CurDescription;
                if (IsInvalidCompanion)
                    CurDescription = "Your memories of this companion are fragmented.\nMod ID: [" + DrawCompanion.ModID + "]";
                else
                    CurDescription = DrawCompanion.Base.Description;
                int TotalLines;
                Description = Utils.WordwrapString(CurDescription, FontAssets.MouseText.Value, CompanionInfoWidth - 8, 6, out TotalLines);
                DescriptionMaxLines = (byte)TotalLines;
            }
        }
        
        private static void DrawBackgroundPanel(Vector2 Position, int Width, int Height, Color color)
        {
            MainMod.DrawBackgroundPanel(Position, Width, Height, color);
        }
    }
}