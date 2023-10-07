using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.Audio;

namespace terraguardians
{
    public class CompanionHousesInWorldInterface : LegacyGameInterfaceLayer
    {
        public static bool IsVisible { get { return Main.playerInventory && CompanionInventoryInterface.GetCurrentButton == CompanionInventoryInterface.ButtonIDs.Housing; } }
        internal static short CompanionToMoveHouse { get { return CompanionInventoryInterface.CompanionToMoveHouse; } set { CompanionInventoryInterface.CompanionToMoveHouse = value; }}

        public CompanionHousesInWorldInterface() : 
            base ("TerraGuardians: Companion Houses In World Interface", DrawInterface, InterfaceScaleType.Game)
        {
        }

        public static bool DrawInterface()
        {
            if(!IsVisible)
            {
                return true;
            }
            string MouseText = "";
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
            for(int i = 0; i < WorldMod.MaxCompanionNpcsInWorld; i++)
            {
                CompanionTownNpcState tns = WorldMod.CompanionNPCsInWorld[i];
                if (tns == null)
                    continue;
                if (!tns.Homeless)
                {
                    Companion c = tns.GetCompanion;
                    if (c == null)
                        continue;
                    int BannerX = tns.HomeX, BannerY = tns.HomeY;
                    if (BannerX < 0 || BannerY < 0) continue;
                    BannerY--;
                    if (Main.tile[BannerX, BannerY] == null) continue;
                    bool EndsOnNullTile = false;
                    while (!Main.tile[BannerX, BannerY].HasTile || !Main.tileSolid[Main.tile[BannerX, BannerY].TileType])
                    {
                        BannerY--;
                        if (BannerY < 10) break;
                        if (Main.tile[BannerX, BannerY] == null)
                        {
                            EndsOnNullTile = true;
                            break;
                        }
                    }
                    if (EndsOnNullTile) continue;
                    const int PaddingX = 8;
                    int PaddingY = 18;
                    if (Main.tile[BannerX, BannerY].TileType == 19)
                    {
                        PaddingY = 10;
                    }
                    BannerY++;
                    Vector2 BannerPosition = new Vector2(BannerX * 16 + PaddingX, BannerY * 16 + PaddingY) - Main.screenPosition;
                    Texture2D BannerTexture = TextureAssets.HouseBanner.Value;
                    Rectangle BannerFrame = BannerTexture.Frame(2, 2);
                    SpriteEffects seffects = SpriteEffects.None;
                    if (MainMod.GetLocalPlayer.gravDir == -1)
                    {
                        BannerPosition.Y -= Main.screenPosition.Y;
                        BannerPosition.Y = Main.screenPosition.Y + Main.screenHeight - BannerPosition.Y;
                        BannerPosition.Y -= BannerFrame.Height - 4;
                    }
                    Main.spriteBatch.Draw(BannerTexture, BannerPosition, BannerFrame, Lighting.GetColor(BannerX, BannerY), 0, new Vector2((int)(BannerFrame.Width * 0.5f), (int)(BannerFrame.Height * 0.5f)), 1, seffects, 0);
                    PlayerMod.DrawPlayerHead(c, BannerPosition, false, 1, 40);
                    //c.DrawCompanionHead(BannerPosition, false, 1, 40);
                    if (Main.mouseX >= BannerPosition.X - BannerFrame.Width * 0.5f && Main.mouseX < BannerPosition.X + BannerFrame.Width * 0.5f && 
                        Main.mouseY >= BannerPosition.Y - BannerFrame.Height * 0.5f && Main.mouseY < BannerPosition.Y + BannerFrame.Height * 0.5f)
                    {
                        MouseText = c.GetName;
                        MainMod.GetLocalPlayer.mouseInterface = true;
                        if (Main.mouseRight && Main.mouseRightRelease)
                        {
                            Main.mouseRightRelease = true;
                            tns.KickCompanionOut();
                        }
                    }
                }
            }
            if(MouseText != "")
            {
                Utils.DrawBorderString(Main.spriteBatch, MouseText, new Vector2(Main.mouseX + 16, Main.mouseY + 16), Color.White);
            }
            return true;
        }
    }
}