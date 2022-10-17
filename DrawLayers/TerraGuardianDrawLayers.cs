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
        private const float DivisionBy16 = 1f / 16;

        public static void PreDrawSettings(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer is TerraGuardian)
            {
                TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
                drawInfo.colorArmorBody = drawInfo.colorArmorHead = drawInfo.colorArmorLegs = drawInfo.colorBodySkin = 
                drawInfo.colorEyes = drawInfo.colorEyeWhites = drawInfo.colorHair = drawInfo.colorHead = drawInfo.colorLegs =
                drawInfo.colorPants = drawInfo.colorShirt = drawInfo.colorShoes = drawInfo.colorUnderShirt = Color.Transparent;
                TgDrawInfoHolder info = tg.GetNewDrawInfoHolder(drawInfo);
            }
        }

        public class DrawTerraGuardianBody : PlayerDrawLayer
        {

            public override bool IsHeadLayer => false;
            
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
                TgDrawInfoHolder info = tg.GetDrawInfo;
                CompanionSpritesContainer spritecontainer = tg.Base.GetSpriteContainer;
                if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
                {
                    Vector2 TgOrigin = info.Origin;
                    Color BodyColor = info.DrawColor;
                    drawInfo.DrawDataCache.Add(new DrawData(spritecontainer.ArmSpritesTexture[1], info.DrawPosition, tg.RightArmFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    drawInfo.DrawDataCache.Add(new DrawData(spritecontainer.BodyTexture, info.DrawPosition, tg.BodyFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                }
                drawInfo.drawPlayer = tg;
            }
        }
        
        public class DrawTerraGuardianLeftArm : PlayerDrawLayer
        {
            public override Position GetDefaultPosition()
            {
                return new AfterParent(PlayerDrawLayers.ArmOverItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer is TerraGuardian;
            }

            public override bool IsHeadLayer => false;

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                if(!(drawInfo.drawPlayer is TerraGuardian)) return; //Even with the visibility setting, seems to activate on player. Projectile drawing seems to bypass visibility checking.
                TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
                TgDrawInfoHolder info = tg.GetDrawInfo;
                CompanionSpritesContainer spritecontainer = tg.Base.GetSpriteContainer;
                if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
                {
                    Vector2 TgOrigin = info.Origin;
                    Color BodyColor = info.DrawColor;
                    drawInfo.DrawDataCache.Add(new DrawData(spritecontainer.ArmSpritesTexture[0], info.DrawPosition, tg.LeftArmFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                }
            }
        }
    }
}