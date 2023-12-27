using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class QuestInterface : LegacyGameInterfaceLayer
    {
        new static bool Active = false;
        public static bool IsActive => Active;
        static Vector2 Position = Vector2.Zero;
        const int Width = 600, Height = 500;
        const int QuestListWidth = 198, QuestInfoWidth = Width - QuestListWidth - 20;
        const int QuestListHeight = Height - 34 * 2 - 16, QuestInfoHeight = Height - 34 * 4 - 20;

        static string[] ActiveQuestNames = new string[0], CompletedQuestNames = new string[0];
        public static string[] QuestTabsText = new string[] { "Active", "Completed" };
        static byte QuestTab = 0;
        static int SelectedQuest = -1;
        static int MaxQuestsInListDisplay = 0, MaxLinesOnQuestInfo = 0;
        static int QuestListScroll = 0;
        static int QuestStoryPage = 0, MaxQuestStoryPages = 0;
        const int QuestListIndexGap = 30;
        static string[] SelectedQuestProgress = new string[0];

        public QuestInterface() : base("TerraGuardians: Quest Interface", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static bool DrawInterface()
        {
            if (!Active) return true;
            if (Main.playerInventory)
            {
                Close();
                return true;
            }
            if (Main.mouseX >= Position.X && Main.mouseX < Position.X + Width && 
                Main.mouseY >= Position.Y && Main.mouseY < Position.Y + Height)
            {
                MainMod.GetLocalPlayer.mouseInterface = true;
            }
            Color InnerPannelColor = Color.Cyan;
            DrawBackgroundPanel(Position, Width, Height, Color.Blue);
            Vector2 InterfacePosition = Position;
            InterfacePosition += Vector2.One * 8f;
            {
                Vector2 TitlePosition = InterfacePosition + new Vector2(Width * .5f, 22);
                DrawBackgroundPanel(InterfacePosition, Width - 16, 30, InnerPannelColor);
                Utils.DrawBorderString(Main.spriteBatch, "Quests", TitlePosition, Color.White, anchorx:.5f, anchory:.5f);
            }
            InterfacePosition.Y += 34;
            DrawQuestTabs(InnerPannelColor, ref InterfacePosition);
            InterfacePosition.Y += 34;
            DrawQuestList(InnerPannelColor, ref InterfacePosition);
            InterfacePosition.X += QuestListWidth + 4;
            DrawQuestInfos(InnerPannelColor, ref InterfacePosition);
            return true;
        }

        static void DrawQuestInfos(Color InnerPannelColor, ref Vector2 InterfacePosition)
        {
            Vector2 Position = InterfacePosition;
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, 30, InnerPannelColor);
                Vector2 TextPosition = Position + new Vector2(QuestInfoWidth * .5f, 4);
                string Text = "Quest Name";
                Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, Color.White, anchorx: .5f);
            }
            Position.Y += 34;
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, QuestInfoHeight, InnerPannelColor);
                for (int i = 0; i < MaxLinesOnQuestInfo; i++)
                {
                    int Index = QuestStoryPage * MaxLinesOnQuestInfo + i;
                    string Text = "Index#" + Index; 
                    if (Index < SelectedQuestProgress.Length)
                        Text = SelectedQuestProgress[Index];
                    Vector2 ThisPosition = Position + new Vector2(QuestInfoWidth * .5f, QuestListIndexGap * i + 4);
                    Utils.DrawBorderString(Main.spriteBatch, Text, ThisPosition, Color.White, anchorx: 0.5f);
                }
            }
            Position.Y += QuestInfoHeight + 4;
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, 34, InnerPannelColor);
                bool ShowPageUpButton = true, ShowPageDownButton = QuestStoryPage > 0;
                if(ShowPageDownButton)
                {
                    Vector2 ButtonPos = Position + new Vector2(QuestInfoWidth * .25f, 4);
                    Color c = Color.White;
                    if (Main.mouseX >= ButtonPos.X - 60 && Main.mouseX < ButtonPos.X + 60 && 
                        Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 30)
                    {
                        c = Color.Cyan;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            QuestStoryPage--;
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "Previous", ButtonPos, c, anchorx: .5f);
                }
                if(ShowPageUpButton)
                {
                    Vector2 ButtonPos = Position + new Vector2(QuestInfoWidth * .75f, 4);
                    Color c = Color.White;
                    if (Main.mouseX >= ButtonPos.X - 60 && Main.mouseX < ButtonPos.X + 60 && 
                        Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 30)
                    {
                        c = Color.Cyan;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                        {
                            QuestStoryPage++;
                        }
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "Next", ButtonPos, c, anchorx: .5f);
                }
                Vector2 PagePosition = Position + new Vector2(QuestInfoWidth * .5f, 4);
                Utils.DrawBorderString(Main.spriteBatch, "Page: " + (QuestStoryPage + 1) + "/" + (MaxQuestStoryPages + 1), PagePosition, Color.White, anchorx:.5f);
            }
            Position.X += QuestInfoWidth * .35f - 1;
            Position.Y += 4;
            DrawSeparator(Position, 22, false, true);
            Position.X += QuestInfoWidth * .3f;
            DrawSeparator(Position, 22, false, true);
        }

        static void DrawQuestList(Color InnerPannelColor, ref Vector2 InterfacePosition)
        {
            Vector2 Position = InterfacePosition;
            DrawBackgroundPanel(Position, QuestListWidth, QuestListHeight, InnerPannelColor);
            bool ShowUpButton = QuestListScroll > 0,
                ShowDownButton = true; //QuestListScroll + MaxQuestsInListDisplay < ;
            for (int i = 0; i < MaxQuestsInListDisplay; i++)
            {
                int Index = i + QuestListScroll;
                Vector2 ThisPosition = Position + Vector2.UnitY * QuestListIndexGap * i;
                if (i > 0)
                {
                    DrawSeparator(ThisPosition + Vector2.UnitX * 4, QuestListWidth - 8, true, true);
                }
                Color c = SelectedQuest == Index ? Color.Yellow : Color.White;
                string Text = "Index#" + Index;
                byte ButtonType = 0;
                const byte UpButton = 1, DownButton = 2;
                if (i == 0 && ShowUpButton)
                {
                    ButtonType = UpButton;
                    Text = "Scroll Up";
                }
                else if (i == MaxQuestsInListDisplay - 1 && ShowDownButton)
                {
                    ButtonType = DownButton;
                    Text = "Scroll Down";
                }
                if (Main.mouseX >= ThisPosition.X && Main.mouseX < ThisPosition.X + QuestListWidth && 
                    Main.mouseY >= ThisPosition.Y && Main.mouseY < ThisPosition.Y + 20)
                {
                    c = Color.Cyan;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        switch (ButtonType)
                        {
                            default:
                                ChangeSelectedQuest(Index);
                                break;
                            case UpButton:
                                QuestListScroll--;
                                break;
                            case DownButton:
                                QuestListScroll++;
                                break;
                        }
                    }
                }
                ThisPosition.X += 4;
                ThisPosition.Y += 4;
                Utils.DrawBorderString(Main.spriteBatch, Text, ThisPosition, c);
            }
        }

        static void DrawQuestTabs(Color InnerPannelColor, ref Vector2 InterfacePosition)
        {
            DrawBackgroundPanel(InterfacePosition, Width - 16, 30, InnerPannelColor);
            Vector2 Position = new Vector2(InterfacePosition.X, InterfacePosition.Y);
            DynamicSpriteFont Font = FontAssets.MouseText.Value;
            for (int i = 0; i < QuestTabsText.Length; i++)
            {
                if (i > 0)
                {
                    DrawSeparator(Position + Vector2.UnitY * 4, 22, false);
                    Position.X += 2;
                }
                string s = QuestTabsText[i];
                float Dimension = Font.MeasureString(s).X + 8;
                Vector2 TabPos = Position;
                Color c = i == QuestTab ? Color.Yellow : Color.White;
                if (Main.mouseX >= TabPos.X && Main.mouseX < TabPos.X + Dimension && 
                    Main.mouseY >= TabPos.Y && Main.mouseY < TabPos.Y + 30)
                {
                    c = Color.Cyan;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        ChangeSelectedTab((byte)i);
                    }
                }
                TabPos += new Vector2(Dimension * .5f, 20);
                Utils.DrawBorderString(Main.spriteBatch, s, TabPos, c, anchorx: 0.5f, anchory: 0.5f);
                Position.X += Dimension;
            }
        }

        static void ChangeSelectedTab(byte NewTab)
        {
            if (QuestTab != NewTab)
            {
                QuestTab = NewTab;
            }
        }

        static void ChangeSelectedQuest(int NewQuest)
        {
            
        }

        public static void Open()
        {
            if (Active) return;
            Position.X = Main.screenWidth * 0.5f - Width * 0.5f;
            Position.Y = Main.screenHeight * 0.5f - Height * 0.5f;
            QuestTab = 0;
            Active = true;
            SelectedQuest = -1;
            Main.playerInventory = false;
            MaxQuestsInListDisplay = (int)(QuestListHeight / QuestListIndexGap);
            MaxLinesOnQuestInfo = (int)(QuestInfoHeight / QuestListIndexGap);
            ChangeQuestStoryText("Hello! This is a quest text.\n\nWhat do you think of this, huh?\nDo you plan on doing many quests? And fill this place?\n\nI do hope so.\n\n - Nakano15");
        }

        static void ChangeQuestStoryText(string NewText)
        {
            SelectedQuestProgress = MainMod.WordwrapText(NewText, FontAssets.MouseText.Value, QuestInfoWidth - 8);
            MaxQuestStoryPages = (int)MathF.Max(0, (SelectedQuestProgress.Length - 1) / MaxLinesOnQuestInfo);
        }

        public static void Close()
        {
            Active = false;
            ActiveQuestNames = null;
            CompletedQuestNames = null;
            SelectedQuestProgress = null;
        }

        internal static void Unload()
        {
            ActiveQuestNames = null;
            CompletedQuestNames = null;
            QuestTabsText = null;
            SelectedQuestProgress = null;
        }

        static void DrawSeparator(Vector2 Position, int Length, bool Horizontal, bool DarkBlueColor = false)
        {
            Rectangle rect = new Rectangle((int)Position.X, (int)Position.Y, 2, 2);
            if (Horizontal)
                rect.Width = Length;
            else
                rect.Height = Length;
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, rect, null, DarkBlueColor ? Color.Blue : Color.Cyan);
        }
        
        private static void DrawBackgroundPanel(Vector2 Position, int Width, int Height, Color color)
        {
            int HalfHeight = (int)(Height * 0.5f);
            Texture2D ChatBackground = TextureAssets.ChatBack.Value;
            for(byte y = 0; y < 3; y++)
            {
                for(byte x = 0; x < 3; x++)
                {
                    const int DrawDimension = 30;
                    int px = (int)Position.X, py = (int)Position.Y, pw = DrawDimension, ph = DrawDimension, 
                        dx = 0, dy = 0, dh = DrawDimension;
                    if (x == 2)
                    {
                        px += Width - pw;
                        dx = ChatBackground.Width - DrawDimension;
                    }
                    else if (x == 1)
                    {
                        px += pw;
                        pw = Width - pw * 2;
                        dx = DrawDimension;
                    }
                    if (y == 2)
                    {
                        py += Height - ph;
                        dy = ChatBackground.Height - DrawDimension;
                        if (ph > HalfHeight)
                        {
                            dy += DrawDimension - HalfHeight;
                            py += (int)(DrawDimension - HalfHeight);
                            ph = dh = HalfHeight;
                        }
                    }
                    else if (y == 1)
                    {
                        py += ph;
                        ph = Height - ph * 2;
                        dy = DrawDimension;
                    }
                    else
                    {
                        if (ph > HalfHeight)
                        {
                            ph = dh = HalfHeight;
                        }
                    }
                    if (pw > 0 && ph > 0)
                    {
                        Main.spriteBatch.Draw(ChatBackground, new Rectangle(px, py, pw, ph), new Rectangle(dx, dy, DrawDimension, dh), color);
                    }
                }
            }
        }
    }
}