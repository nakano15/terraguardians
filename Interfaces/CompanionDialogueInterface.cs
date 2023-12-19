using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionDialogueInterface : GameInterfaceLayer
    {
        private static byte SelectedOption = 255;
        public static float DialogueStartX
        {
            get
            {
                return (Main.screenWidth - DialogueWidth) * 0.5f;
            }
        }
        public static float DialogueStartY
        {
            get
            {
                return 100;
            }
        }
        public static int DialogueWidth
        {
            get
            {
                return (int)(Main.screenWidth * 0.5f);
            }
        }
        public static int DialogueHeight
        {
            get
            {
                return (int)(System.Math.Max(0, Dialogue.Message.Count - 1) * 30 + 60);
            }
        }

        public CompanionDialogueInterface() : 
            base("TerraGuardians: Dialogue Interface", InterfaceScaleType.UI)
        {
            
        }

        protected override bool DrawSelf()
        {
            return DrawInterface();
        }

        public static bool DrawInterface()
        {
            if (!Dialogue.InDialogue) return true;
            PlayerMod pm = Main.LocalPlayer.GetModPlayer<PlayerMod>();
            if(pm.TalkPlayer == null || Main.playerInventory) 
            {
                Dialogue.EndDialogue();
                return true;
            }
            Player player = Main.LocalPlayer;
            if(player.talkNPC > -1) player.SetTalkNPC(-1);
            if(player.sign > -1) player.sign = -1;
            Vector2 DrawPosition = new Vector2(DialogueStartX, DialogueStartY);
            if(Dialogue.Speaker != null)
            {
                Vector2 NamePanelPosition = new Vector2(DrawPosition.X, DrawPosition.Y);
                if(Main.mouseX >= NamePanelPosition.X && Main.mouseX < NamePanelPosition.X + DialogueWidth && 
                   Main.mouseY >= NamePanelPosition.Y && Main.mouseY < NamePanelPosition.Y + 48)
                {
                    player.mouseInterface = true;
                }
                DrawBackgroundPanel(NamePanelPosition, 48, 48, Color.White);
                Companion companion = (Companion)Dialogue.Speaker;
                PlayerMod.DrawPlayerHeadInterface(companion, NamePanelPosition + (Vector2.One * 24), false, 1, 36);
                MainMod.DrawFriendshipHeart(NamePanelPosition + new Vector2(12, 36), companion.FriendshipLevel, companion.GetFriendshipProgress);
                NamePanelPosition.X += 48;
                DrawBackgroundPanel(NamePanelPosition, DialogueWidth - 48, 48, Color.White);
                NamePanelPosition.X += 4;
                NamePanelPosition.Y += 32;
                Utils.DrawBorderStringBig(Main.spriteBatch, companion.GetName, NamePanelPosition, Color.White, scale: 0.9f, anchory: 0.5f);
                DrawPosition.Y += 48;
            }
            Color PanelBackground = new Color(200, 200, 200, 200);
            if(Dialogue.Message.Count > 0)
            {
                DrawBackgroundPanel(DrawPosition, DialogueWidth, DialogueHeight, PanelBackground);
                if (Main.mouseX >= DrawPosition.X && Main.mouseX < DrawPosition.X + DialogueWidth && 
                    Main.mouseY >= DrawPosition.Y && Main.mouseY < DrawPosition.Y + DialogueHeight)
                {
                    player.mouseInterface = true;
                }
                {
                    Vector2 DialogueTextPosition = new Vector2(DrawPosition.X, DrawPosition.Y);
                    DialogueTextPosition.X += 8;
                    DialogueTextPosition.Y += 16;
                    foreach(TextSnippet[] text in Dialogue.Message)
                    {
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Dialogue.GetDialogueFont, text, 
                        DialogueTextPosition, 0, Color.White, Vector2.Zero, Vector2.One, out int hover, DialogueWidth);
                        if (hover > -1)
                        {
                            text[hover].OnHover();
                            if (Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                text[hover].OnClick();
                            }
                        }
                        DrawPosition.Y += 30;
                        DialogueTextPosition.Y += 30;
                    }
                }
            }
            else
            {
                DrawPosition.Y -= 30;
            }
            byte NewSelectedOption = 255;
            bool ClickedOption = false;
            if(Dialogue.Options != null)
            {
                DrawPosition.Y += 30;
                int Count = 0;
                foreach(DialogueOption option in Dialogue.Options)
                {
                    Count += option.ParsedText.Count;
                }
                if (Main.mouseX >= DrawPosition.X && Main.mouseX < DrawPosition.X + DialogueWidth && 
                    Main.mouseY >= DrawPosition.Y && Main.mouseY < DrawPosition.Y + 30 * Count + 60)
                {
                    player.mouseInterface = true;
                }
                Count = System.Math.Max(0, Count - 1);
                DrawBackgroundPanel(DrawPosition, DialogueWidth, 30 * Count + 60, PanelBackground);
                Vector2 OptionsPosition = new Vector2(DrawPosition.X, DrawPosition.Y);
                OptionsPosition.Y += 18;
                OptionsPosition.X += 8;
                for(int o = 0; o < Dialogue.Options.Length; o++)
                {
                    DialogueOption option = Dialogue.Options[o];
                    if(SelectedOption == o)
                    {
                        DrawBackgroundPanel(DrawPosition + Vector2.One * 8, DialogueWidth - 16, option.ParsedText.Count * 30 + 12, new Color(100, 100, 100, 100));
                    }
                    foreach(TextSnippet[] text in option.ParsedText)
                    {
                        if (Main.mouseX >= DrawPosition.X + 8 && Main.mouseX < DrawPosition.X + DialogueWidth - 16 && 
                            Main.mouseY >= DrawPosition.Y + 12 && Main.mouseY < DrawPosition.Y + 38)
                        {
                            NewSelectedOption = (byte)o;
                            if(Main.mouseLeft && Main.mouseLeftRelease)
                            {
                                ClickedOption = true;
                            }
                        }
                        ChatManager.DrawColorCodedStringWithShadow(Main.spriteBatch, Dialogue.GetDialogueFont, text, 
                            OptionsPosition, 0, Vector2.Zero, 
                            Vector2.One, out int hover, DialogueWidth);
                        OptionsPosition.Y += 30;
                        DrawPosition.Y += 30;
                    }
                }
            }
            SelectedOption = NewSelectedOption;
            if(ClickedOption && SelectedOption < 255)
            {
                Dialogue.Options[SelectedOption].ResultAction();
            }
            return true;
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