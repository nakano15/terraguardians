using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Terraria.Graphics.Renderers;
using System.Collections.Generic;

namespace terraguardians
{
    public class TerraGuardianDrawLayersScript
    {
        //private static Color BodyColor = Color.White;
        private static Vector2 TgOrigin = Vector2.UnitY;

        public static void PreDrawSettings(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer is TerraGuardian)
            {
                const float DivisionBy16 = 1f / 16;
                TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
                //BodyColor = Lighting.GetColor((int)(drawInfo.drawPlayer.Center.X * DivisionBy16), (int)(drawInfo.drawPlayer.Center.Y * DivisionBy16), Color.White);
                drawInfo.colorArmorBody = drawInfo.colorArmorHead = drawInfo.colorArmorLegs = drawInfo.colorBodySkin = 
                drawInfo.colorEyes = drawInfo.colorEyeWhites = drawInfo.colorHair = drawInfo.colorHead = drawInfo.colorLegs =
                drawInfo.colorPants = drawInfo.colorShirt = drawInfo.colorShoes = drawInfo.colorUnderShirt = Color.Transparent;
                drawInfo.Position.X += drawInfo.drawPlayer.width * 0.5f;
                drawInfo.Position.Y += drawInfo.drawPlayer.height + 2;
                drawInfo.Position -= Main.screenPosition;
                drawInfo.Position.X = (int)drawInfo.Position.X;
                drawInfo.Position.Y = (int)drawInfo.Position.Y;
                TgOrigin = new Vector2(tg.Base.SpriteWidth * 0.5f, tg.Base.SpriteHeight);
                //drawInfo.Position.Y += drawInfo.drawPlayer.height;
            }
        }

        public class DrawTerraGuardianBody : PlayerDrawLayer
        {
            public override Position GetDefaultPosition()
            {
                return new AfterParent(PlayerDrawLayers.Skin);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer is TerraGuardian;
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
                CompanionSpritesContainer spritecontainer = tg.Base.GetSpriteContainer;
                if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
                {
                    Color BodyColor = Color.White;
                    drawInfo.DrawDataCache.Add(new DrawData(spritecontainer.ArmSpritesTexture[1], drawInfo.Position, tg.RightArmFrame, BodyColor, drawInfo.rotation, TgOrigin, 1f, drawInfo.playerEffect, 0));
                    drawInfo.DrawDataCache.Add(new DrawData(spritecontainer.BodyTexture, drawInfo.Position, tg.BodyFrame, BodyColor, drawInfo.rotation, TgOrigin, 1f, drawInfo.playerEffect, 0));
                    drawInfo.DrawDataCache.Add(new DrawData(spritecontainer.ArmSpritesTexture[0], drawInfo.Position, tg.LeftArmFrame, BodyColor, drawInfo.rotation, TgOrigin, 1f, drawInfo.playerEffect, 0));
                }
            }
        }
    }
}