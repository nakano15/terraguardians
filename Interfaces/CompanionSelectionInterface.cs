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
        private const int WindowWidth = 520, WindowHeight = 366;
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
        private static FirstButtonType FirstButton = FirstButtonType.Hidden;
        private static SecondButtonType SecondButton = SecondButtonType.Hidden;
        static bool CheckingExtraInfos = false;
        static string Nickname = "";
        static string FullName = "";
        static string Age = "";
        static string Birthday = "";
        static string SleepTime = "";
        static string Race = "";

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
            UpdateInviteButtonState();
            UpdateCallButtonState();
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
            Nickname = null;
            FullName = null;
            Age = null;
            Birthday = null;
            SleepTime = null;
            Race = null;
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
                    float Width = Utils.DrawBorderString(Main.spriteBatch, Nickname, NamePanelPosition, c, 1, 0.5f, 1).X;
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
                    DrawCompanionCharacter(StartPosition);
                    /*Vector2 CompanionDrawPosition = StartPosition;
                    CompanionDrawPosition.X += CompanionInfoWidth * 0.5f;
                    CompanionDrawPosition.Y += ListHeight * 0.5f;
                    CompanionDrawPosition.X -= DrawCompanion.width * 0.5f;
                    CompanionDrawPosition.Y -= DrawCompanion.height;
                    CompanionDrawPosition.X = (int)(CompanionDrawPosition.X + Main.screenPosition.X);
                    CompanionDrawPosition.Y = (int)(CompanionDrawPosition.Y + Main.screenPosition.Y - 2);
                    
                    DrawCompanion.position = CompanionDrawPosition;
                    DrawCompanion.DrawCompanionInterfaceOnly(UseSingleDrawScript: true);*/
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
                    if (!CheckingExtraInfos)
                    {
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
                    else
                    {
                        const int FramesHeight = 30;
                        const int HalfCompanionInfoWidth = CompanionInfoWidth / 2;
                        for (int i = 0; i < 5; i++)
                        {
                            Vector2 Position = DescriptionPanelPosition + Vector2.UnitY * i * FramesHeight;
                            DrawBackgroundPanel(Position, HalfCompanionInfoWidth, FramesHeight, Color.LightCyan);
                            Position.Y += 6;
                            Position.X += HalfCompanionInfoWidth * .5f;
                            string Text = "";
                            switch(i)
                            {
                                case 0:
                                    Text = FullName;
                                    break;
                                case 1:
                                    Text = Age;
                                    break;
                                case 2:
                                    Text = Birthday;
                                    break;
                                case 3:
                                    Text = Race;
                                    break;
                                case 4:
                                    Text = SleepTime;
                                    break;
                            }
                            Utils.DrawBorderString(Main.spriteBatch, Text, Position, Color.White, .9f, .5f);
                        }
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
                        if (FirstButton != FirstButtonType.Hidden)
                        { //Call Dismiss Button
                            string Text = "";
                            switch(FirstButton)
                            {
                                case FirstButtonType.Call:
                                    Text = "Call";
                                    break;
                                case FirstButtonType.Dismiss:
                                    Text = "Dismiss";
                                    break;
                            }
                            bool MouseOver = false;
                            if(Main.mouseX >= ButtonsPosition.X && Main.mouseX < ButtonsPosition.X + ButtonWidth && Main.mouseY >= ButtonsPosition.Y && Main.mouseY < ButtonsPosition.Y + 30)
                            {
                                MouseOver = true;
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    switch(FirstButton)
                                    {
                                        case FirstButtonType.Call:
                                            if (DrawCompanion.CanFollowPlayer())
                                            {
                                                if (pm.CallCompanionByIndex(DrawCompanion.Index))
                                                {
                                                    DrawCompanion.SaySomethingOnChat(DrawCompanion.GetDialogues.JoinGroupMessages(DrawCompanion, JoinMessageContext.Success));
                                                    UpdateCallButtonState();
                                                }
                                                else
                                                {
                                                    DrawCompanion.SaySomethingOnChat(DrawCompanion.GetDialogues.JoinGroupMessages(DrawCompanion, JoinMessageContext.Fail));
                                                }
                                            }
                                            else
                                            {
                                                if (!pm.HasEmptyFollowerSlot())
                                                {
                                                    DrawCompanion.SaySomethingOnChat(DrawCompanion.GetDialogues.JoinGroupMessages(DrawCompanion, JoinMessageContext.FullParty));
                                                }
                                                else
                                                {
                                                    DrawCompanion.SaySomethingOnChat(DrawCompanion.GetDialogues.JoinGroupMessages(DrawCompanion, JoinMessageContext.Fail));
                                                }
                                            }
                                            break;
                                        case FirstButtonType.Dismiss:
                                            if (DrawCompanion.CanStopFollowingPlayer())
                                            {
                                                pm.DismissCompanionByIndex(DrawCompanion.Index);
                                                DrawCompanion.SaySomethingOnChat(DrawCompanion.GetDialogues.LeaveGroupMessages(DrawCompanion, LeaveMessageContext.Success));
                                                    UpdateCallButtonState();
                                            }
                                            else
                                            {
                                                DrawCompanion.SaySomethingOnChat(DrawCompanion.GetDialogues.LeaveGroupMessages(DrawCompanion, LeaveMessageContext.Fail));
                                            }
                                            break;
                                    }
                                }
                            }
                            Vector2 TextPosition = ButtonsPosition + new Vector2(ButtonWidth * 0.5f, 13f);
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, (MouseOver ? Color.Yellow : Color.White), 0.7f, 0.5f, 0.5f);
                        }
                        ButtonsPosition.X += ButtonWidth + 2;
                        if (SecondButton != SecondButtonType.Hidden)
                        { //Invite Or Send Home button
                            string Text = "Invite Over";
                            switch(SecondButton)
                            {
                                case SecondButtonType.ScheduleVisitButton:
                                    Text = "Invite Over";
                                    break;
                                case SecondButtonType.CancelScheduleButton:
                                    Text = "Cancel Invite";
                                    break;
                            }
                            bool MouseOver = false;
                            if(Main.mouseX >= ButtonsPosition.X && Main.mouseX < ButtonsPosition.X + ButtonWidth && Main.mouseY >= ButtonsPosition.Y && Main.mouseY < ButtonsPosition.Y + 30)
                            {
                                MouseOver = true;
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    //Do your magic
                                    switch(SecondButton)
                                    {
                                        case SecondButtonType.ScheduleVisitButton:
                                            if (DrawCompanion.CanInviteOver(MainMod.GetLocalPlayer))
                                            {
                                                WorldMod.AddCompanionToScheduleList(DrawCompanion);
                                                if (DrawCompanion.Base.IsNocturnal == !Main.dayTime)
                                                {
                                                    DrawCompanionSayMessage(DrawCompanion.GetDialogues.InviteMessages(DrawCompanion, InviteContext.Success));
                                                }
                                                else
                                                {
                                                    DrawCompanionSayMessage(DrawCompanion.GetDialogues.InviteMessages(DrawCompanion, InviteContext.SuccessNotInTime));
                                                }
                                                UpdateInviteButtonState();
                                            }
                                            else
                                            {
                                                DrawCompanionSayMessage(DrawCompanion.GetDialogues.InviteMessages(DrawCompanion, InviteContext.Failed));
                                            }
                                            break;
                                        case SecondButtonType.CancelScheduleButton:
                                            WorldMod.RemoveCompanionFromScheduleList(DrawCompanion);
                                            DrawCompanionSayMessage(DrawCompanion.GetDialogues.InviteMessages(DrawCompanion, InviteContext.CancelInvite));
                                            UpdateInviteButtonState();
                                            break;
                                    }
                                }
                            }
                            Vector2 TextPosition = ButtonsPosition + new Vector2(ButtonWidth * 0.5f, 13f);
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, (MouseOver ? Color.Yellow : Color.White), 0.7f, 0.5f, 0.5f);
                        }
                        ButtonsPosition.X += ButtonWidth + 2;
                        {
                            string Text = CheckingExtraInfos ? "Basic Infos" : "Extra Infos";
                            bool MouseOver = false;
                            if(Main.mouseX >= ButtonsPosition.X && Main.mouseX < ButtonsPosition.X + ButtonWidth && Main.mouseY >= ButtonsPosition.Y && Main.mouseY < ButtonsPosition.Y + 30)
                            {
                                MouseOver = true;
                                if(Main.mouseLeft && Main.mouseLeftRelease)
                                {
                                    CheckingExtraInfos = !CheckingExtraInfos;
                                }
                            }
                            Vector2 TextPosition = ButtonsPosition + new Vector2(ButtonWidth * 0.5f, 13f);
                            Utils.DrawBorderString(Main.spriteBatch, Text, TextPosition, (MouseOver ? Color.Yellow : Color.White), 0.7f, 0.5f, 0.5f);
                        }
                    }
                }
            }
        }

        static void DrawCompanionCharacter(Vector2 Position)
        {
            Vector2 CompanionDrawPosition = Position;
            CompanionDrawPosition.X += CompanionInfoWidth * 0.5f;
            CompanionDrawPosition.Y += ListHeight * 0.5f;
            CompanionDrawPosition.X -= DrawCompanion.width * 0.5f;
            CompanionDrawPosition.Y -= DrawCompanion.height;
            CompanionDrawPosition.X = (int)(CompanionDrawPosition.X + Main.screenPosition.X);
            CompanionDrawPosition.Y = (int)(CompanionDrawPosition.Y + Main.screenPosition.Y - 2);
            
            DrawCompanion.position = CompanionDrawPosition;
            DrawCompanion.DrawCompanionInterfaceOnly(UseSingleDrawScript: true);
        }

        private static void DrawCompanionSayMessage(string Message, Color? color = null)
        {
            if (DrawCompanion == null) return;
            DrawCompanion.SaySomethingOnChat(Message, color);
        }

        private static void UpdateCallButtonState()
        {
            if (DrawCompanion == null || !DrawCompanion.CanFollowPlayer())
            {
                FirstButton = FirstButtonType.Hidden;
                return;
            }
            if(PlayerMod.PlayerHasCompanionSummoned(MainMod.GetLocalPlayer, DrawCompanion))
            {
                FirstButton = FirstButtonType.Dismiss;
            }
            else
            {
                FirstButton = FirstButtonType.Call;
            }
        }

        private static void UpdateInviteButtonState()
        {
            if (DrawCompanion == null || PlayerMod.GetIsPlayerBuddy(MainMod.GetLocalPlayer, DrawCompanion) || MainMod.HasCompanionInWorld(DrawCompanion.GetCompanionID) || WorldMod.IsCompanionLivingHere(DrawCompanion))
            {
                SecondButton = SecondButtonType.Hidden;
                return;
            }
            if (WorldMod.IsCompanionScheduledToVisit(DrawCompanion))
                SecondButton = SecondButtonType.CancelScheduleButton;
            else
                SecondButton = SecondButtonType.ScheduleVisitButton;
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
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                            Page--;
                    }
                    Utils.DrawBorderString(Main.spriteBatch, "<", PageDisplayPosition + Vector2.UnitX * 4 + Vector2.UnitY * ListNameSpacing * 0.5f, 
                    (MouseOver ? Color.Yellow : Color.White), anchory: 0.5f);
                }
                if(RightArrowVisible)
                {
                    bool MouseOver = false;
                    if(Main.mouseX >= PageDisplayPosition.X + ListWidth / 2 && Main.mouseX < PageDisplayPosition.X + ListWidth && 
                        Main.mouseY >= PageDisplayPosition.Y && Main.mouseY < PageDisplayPosition.Y + ListNameSpacing) //Right Arrow
                    {
                        MouseOver = true;
                        if (Main.mouseLeft && Main.mouseLeftRelease)
                            Page++;
                    }
                    Utils.DrawBorderString(Main.spriteBatch, ">", PageDisplayPosition + Vector2.UnitX * (ListWidth - 4) + Vector2.UnitY * ListNameSpacing * 0.5f, 
                    (MouseOver ? Color.Yellow : Color.White), anchorx: 1, anchory: 0.5f);
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
                Nickname = null;
                FullName = null;
                Age = null;
                Birthday = null;
            }
            else
            {
                DrawCompanion = MainMod.GetCompanionBase(CompanionDatas[SelectedCompanion]).GetCompanionObject;
                DrawCompanion.Data = CompanionDatas[SelectedCompanion];
                IsInvalidCompanion = DrawCompanion.Base.IsInvalidCompanion;
                DrawCompanion.InitializeCompanion(true);
                DrawCompanion.active = true;
                DrawCompanion.ChangeDir(1);
                for(int i = 0; i < 20; i++)
                    DrawCompanion.UpdateEquips(i);
                FriendshipExpProgress = DrawCompanion.GetFriendshipProgress;
                string CurDescription;
                if (IsInvalidCompanion)
                    CurDescription = "Your memories of this companion are fragmented.\nID: "+DrawCompanion.ID+" Mod ID: [" + DrawCompanion.ModID + "]";
                else
                    CurDescription = DrawCompanion.Base.Description;
                int TotalLines;
                Description = Utils.WordwrapString(CurDescription, FontAssets.MouseText.Value, CompanionInfoWidth - 8, 6, out TotalLines);
                DescriptionMaxLines = (byte)TotalLines;
                Nickname = DrawCompanion.Data.GetNameWithNickname;
                FullName = DrawCompanion.Base.FullName;
                Age = "Age: " + DrawCompanion.GetAge + " Y.O";
                Birthday = "BD: " + DrawCompanion.GetBirthdayString;
                SleepTime = DrawCompanion.Base.IsNocturnal ? "Nocturnal" : "Diurnal";
                Race = DrawCompanion.Base.GetCompanionGroup.Name;
            }
            UpdateCallButtonState();
            UpdateInviteButtonState();
        }
        
        private static void DrawBackgroundPanel(Vector2 Position, int Width, int Height, Color color)
        {
            MainMod.DrawBackgroundPanel(Position, Width, Height, color);
        }

        private enum FirstButtonType : byte
        {
            Hidden = 0,
            Call = 1,
            Dismiss = 2
        }

        private enum SecondButtonType : byte
        {
            Hidden = 0,
            ScheduleVisitButton = 1,
            CancelScheduleButton = 2
        }
    }
}