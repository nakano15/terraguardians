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
using Terraria.Localization;
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
        const int QuestListHeight = Height - 34 * 3 - 16 - 4, QuestInfoHeightWithCurObjective = Height - 34 * 6 - 16, QuestInfoHeightFull = Height - 34 * 4 - 20;

        static string[] ActiveQuestNames = new string[0], CompletedQuestNames = new string[0];
        static int [] ActiveQuestIndexes = new int[0], CompletedQuestIndexes = new int[0];
        readonly static string[] QuestTabsText = new string[] { "Active", "Completed" };
        static byte QuestTab = 0;
        static int SelectedQuest = -1;
        static int MaxQuestsInListDisplay = 0, MaxLinesOnQuestInfo = 0;
        static int QuestListScroll = 0;
        static int QuestStoryPage = 0, MaxQuestStoryPages = 0;
        const float QuestListIndexGap = 30, QuestStoryGap = 24;
        static string QuestName = "";
        static string[] SelectedQuestProgress = new string[0], SelectedQuestObjective = new string[0];
        const string InterfaceKey = "Mods.terraguardians.Interface.QuestLog.";

        public QuestInterface() : base("TerraGuardians: Quest Interface", DrawInterface, InterfaceScaleType.UI)
        {
            
        }

        static string GetTranslation(string Key)
        {
            return Language.GetTextValue(InterfaceKey + Key);
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
                Utils.DrawBorderString(Main.spriteBatch, GetTranslation("Title"), TitlePosition, Color.White, anchorx:.5f, anchory:.5f);
            }
            InterfacePosition.Y += 34;
            DrawQuestTabs(InnerPannelColor, ref InterfacePosition);
            InterfacePosition.Y += 34;
            DrawQuestList(InnerPannelColor, ref InterfacePosition);
            if (Active)
            {
                InterfacePosition.X += QuestListWidth + 4;
                DrawQuestInfos(InnerPannelColor, ref InterfacePosition);
            }
            return true;
        }

        static void DrawQuestInfos(Color InnerPannelColor, ref Vector2 InterfacePosition)
        {
            Vector2 Position = InterfacePosition;
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, 30, InnerPannelColor);
                Vector2 TextPosition = Position + new Vector2(QuestInfoWidth * .5f, 4);
                string Text = QuestName;
                Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, Color.White, anchorx: .5f);
            }
            Position.Y += 34;
            bool ActiveTab = SelectedQuest > -1 && QuestTab == 0;
            if (ActiveTab)
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, 60, InnerPannelColor);
                for (int i = 0; i < 2; i++)
                {
                    if (i >= SelectedQuestObjective.Length) break;
                    string Text = SelectedQuestObjective[i];
                    Vector2 ThisPosition = Position + new Vector2(4, QuestListIndexGap * i + 4);
                    Utils.DrawBorderString(Main.spriteBatch, Text, ThisPosition, Color.White);
                }
                Position.Y += 64;
            }
            int QuestInfoHeight = ActiveTab ? QuestInfoHeightWithCurObjective : QuestInfoHeightFull;
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, QuestInfoHeight, InnerPannelColor);
                int MaxQuestInfoLines = MaxLinesOnQuestInfo;
                if (ActiveTab)
                    MaxQuestInfoLines -= 2;
                for (int i = 0; i < MaxQuestInfoLines; i++)
                {
                    int Index = QuestStoryPage * MaxQuestInfoLines + i;
                    string Text = ""; 
                    if (Index >= SelectedQuestProgress.Length)
                        break;
                    Text = SelectedQuestProgress[Index];
                    Vector2 ThisPosition = Position + new Vector2(QuestInfoWidth * .5f, QuestStoryGap * i + 4);
                    Utils.DrawBorderString(Main.spriteBatch, Text, ThisPosition, Color.White, anchorx: 0.5f);
                }
            }
            Position.Y += QuestInfoHeight + 4;
            {
                DrawBackgroundPanel(Position, QuestInfoWidth, 34, InnerPannelColor);
                bool ShowPageUpButton = QuestStoryPage < MaxQuestStoryPages, ShowPageDownButton = QuestStoryPage > 0;
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
                    Utils.DrawBorderString(Main.spriteBatch, GetTranslation("PreviousPage"), ButtonPos, c, anchorx: .5f);
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
                    Utils.DrawBorderString(Main.spriteBatch, GetTranslation("NextPage"), ButtonPos, c, anchorx: .5f);
                }
                Vector2 PagePosition = Position + new Vector2(QuestInfoWidth * .5f, 4);
                Utils.DrawBorderString(Main.spriteBatch, GetTranslation("PageNumber")
                    .Replace("{cur}", (QuestStoryPage + 1).ToString())
                    .Replace("{total}", (MaxQuestStoryPages + 1).ToString()), PagePosition, Color.White, anchorx:.5f);
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
            string[] QuestNames = QuestTab == 0 ? ActiveQuestNames : CompletedQuestNames;
            int MaxItems = QuestNames.Length;
            bool ShowUpButton = QuestListScroll > 0,
                ShowDownButton = QuestListScroll + MaxQuestsInListDisplay < MaxItems;
            for (int i = 0; i < MaxQuestsInListDisplay; i++)
            {
                int Index = i + QuestListScroll;
                Vector2 ThisPosition = Position + Vector2.UnitY * QuestListIndexGap * i;
                if (i > 0)
                {
                    DrawSeparator(ThisPosition + Vector2.UnitX * 4, QuestListWidth - 8, true, true);
                }
                if (Index >= MaxItems) continue;
                Color c = SelectedQuest == Index ? Color.Yellow : Color.White;
                string Text = QuestNames[Index];
                byte ButtonType = 0;
                const byte UpButton = 1, DownButton = 2;
                if (i == 0 && ShowUpButton)
                {
                    ButtonType = UpButton;
                    Text = GetTranslation("ScrollUp");
                }
                else if (i == MaxQuestsInListDisplay - 1 && ShowDownButton)
                {
                    ButtonType = DownButton;
                    Text = GetTranslation("ScrollDown");
                }
                if (Main.mouseX >= ThisPosition.X && Main.mouseX < ThisPosition.X + QuestListWidth && 
                    Main.mouseY >= ThisPosition.Y && Main.mouseY < ThisPosition.Y + QuestListIndexGap)
                {
                    c = Color.Cyan;
                    if (Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        switch (ButtonType)
                        {
                            default:
                                ChangeSelectedQuest(QuestTab == 0 ? ActiveQuestIndexes[Index] : CompletedQuestIndexes[Index]);
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
            Position.Y += QuestListHeight + 4;
            {
                DrawBackgroundPanel(Position, QuestListWidth, 34, InnerPannelColor);
                Color c = Color.White;
                if (Main.mouseX >= Position.X && Main.mouseX < Position.X + QuestListWidth && 
                    Main.mouseY >= Position.Y && Main.mouseY < Position.Y + 30)
                {
                    c = Color.Cyan;
                    if(Main.mouseLeft && Main.mouseLeftRelease)
                    {
                        Close();
                    }
                }
                Position.X += QuestListWidth * .5f;
                Position.Y += 18;
                Utils.DrawBorderString(Main.spriteBatch, Language.GetTextValue("Mods.terraguardians.Interface.Close"), Position, c, anchorx: 0.5f, anchory: 0.5f);
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
                string s = GetTranslation(QuestTabsText[i]);
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
            if (Main.gameMenu) return;
            List<QuestData> quests = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().QuestDatas;
            if (NewQuest < quests.Count && NewQuest > -1 && NewQuest != SelectedQuest && (quests[NewQuest].IsActive || quests[NewQuest].IsCompleted))
            {
                SelectedQuest = NewQuest;
                QuestName = quests[NewQuest].Name;
                ChangeQuestObjectiveText(quests[NewQuest].GetObjective);
                ChangeQuestStoryText(quests[NewQuest].GetStory);
                return;
            }
            SelectedQuest = -1;
            QuestName = GetTranslation("NoQuestSelected");
            ShowDefaultStatistics();
            ChangeQuestObjectiveText("");
        }

        public static void Open()
        {
            if (Active) return;
            Position.X = Main.screenWidth * 0.5f - Width * 0.5f;
            Position.Y = Main.screenHeight * 0.5f - Height * 0.5f;
            //QuestTab = 0;
            Active = true;
            Main.playerInventory = false;
            MaxQuestsInListDisplay = (int)(QuestListHeight / QuestListIndexGap);
            MaxLinesOnQuestInfo = (int)(QuestInfoHeightFull / QuestStoryGap);
            FillQuestsList();
            ChangeSelectedQuest(SelectedQuest);
        }

        static void ShowDefaultStatistics()
        {
            ChangeQuestStoryText(GetTranslation("DefaultQuestStatistics")
                .Replace("{active}", ActiveQuestIndexes.Length.ToString())
                .Replace("{completed}", CompletedQuestNames.Length.ToString()));
        }

        static void FillQuestsList()
        {
            List<string> ActiveNames = new List<string>(), CompleteNames = new List<string>();
            List<int> ActiveIndexes = new List<int>(), CompleteIndexes = new List<int>();
            List<QuestData> quests = MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().QuestDatas;
            for (int q = 0; q < quests.Count; q++)
            {
                QuestData quest = quests[q];
                if (quest.IsCompleted)
                {
                    CompleteNames.Add(quest.Name);
                    CompleteIndexes.Add(q);
                }
                else if (quest.IsActive)
                {
                    ActiveNames.Add(quest.Name);
                    ActiveIndexes.Add(q);
                }
            }
            ActiveQuestNames = ActiveNames.ToArray();
            CompletedQuestNames = CompleteNames.ToArray();
            ActiveQuestIndexes = ActiveIndexes.ToArray();
            CompletedQuestIndexes = CompleteIndexes.ToArray();
        }

        static void ChangeQuestStoryText(string NewText)
        {
            SelectedQuestProgress = MainMod.WordwrapText(NewText, FontAssets.MouseText.Value, QuestInfoWidth - 8);
            MaxQuestStoryPages = (int)MathF.Max(0, (SelectedQuestProgress.Length + 1) / MaxLinesOnQuestInfo);
        }

        static void ChangeQuestObjectiveText(string NewObjective)
        {
            SelectedQuestObjective = MainMod.WordwrapText(NewObjective, FontAssets.MouseText.Value, QuestInfoWidth - 8);
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
            SelectedQuestProgress = null;
            ActiveQuestIndexes = null;
            CompletedQuestIndexes = null;
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