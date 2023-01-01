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
        public static DrawContext Context = DrawContext.AllParts;
        public static bool IgnoreLight = false;

        public static void PreDrawSettings(ref PlayerDrawSet drawInfo)
        {
            if (DrawCompanionOnPlayerLayers.TakingDrawSets && drawInfo.drawPlayer is Companion)
            {
                DrawCompanionOnPlayerLayers.AddPlayerDrawSet(drawInfo);
            }
            if (drawInfo.drawPlayer is TerraGuardian)
            {
                TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
                drawInfo.colorArmorBody = drawInfo.colorArmorHead = drawInfo.colorArmorLegs = drawInfo.colorBodySkin = 
                drawInfo.colorEyes = drawInfo.colorEyeWhites = drawInfo.colorHair = drawInfo.colorHead = drawInfo.colorLegs =
                drawInfo.colorPants = drawInfo.colorShirt = drawInfo.colorShoes = drawInfo.colorUnderShirt = Color.Transparent;
                TgDrawInfoHolder info = tg.GetNewDrawInfoHolder(drawInfo);
                tg.Base.PreDrawCompanions(ref drawInfo, ref info);
            }
        }

        private static void DrawBehindLayer(ref PlayerDrawSet drawInfo)
        {
            TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
            TgDrawInfoHolder info = tg.GetDrawInfo;
            if(info.Context == DrawContext.FrontLayer) return;
            CompanionSpritesContainer spritecontainer = tg.Base.GetSpriteContainer;
            if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
            {
                Vector2 TgOrigin = info.Origin;
                Color BodyColor = info.DrawColor;
                List<DrawData> dd = new List<DrawData>();
                dd.Add(new DrawData(spritecontainer.ArmSpritesTexture[1], info.DrawPosition, tg.ArmFrame[1], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                dd.Add(new DrawData(spritecontainer.BodyTexture, info.DrawPosition, tg.BodyFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                //DrawHat(tg, info, dd, ref drawInfo);
                tg.Base.CompanionDrawLayerSetup(false, drawInfo, ref info, ref dd);
                drawInfo.DrawDataCache.AddRange(dd);
            }
            drawInfo.drawPlayer = tg;
        }

        private static void DrawFrontLayer(ref PlayerDrawSet drawInfo)
        {
            if(!(drawInfo.drawPlayer is TerraGuardian)) return; //Even with the visibility setting, seems to activate on player. Projectile drawing seems to bypass visibility checking.
            TerraGuardian tg = (TerraGuardian)drawInfo.drawPlayer;
            TgDrawInfoHolder info = tg.GetDrawInfo;
            if(info.Context == DrawContext.BackLayer) return;
            CompanionSpritesContainer spritecontainer = tg.Base.GetSpriteContainer;
            if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
            {
                Vector2 TgOrigin = info.Origin;
                Color BodyColor = info.DrawColor;
                List<DrawData> dd = new List<DrawData>();
                if (tg.BodyFrontFrameID > -1)
                    dd.Add(new DrawData(spritecontainer.BodyFrontTexture, info.DrawPosition, tg.BodyFrontFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                dd.Add(new DrawData(spritecontainer.ArmSpritesTexture[0], info.DrawPosition, tg.ArmFrame[0], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                tg.Base.CompanionDrawLayerSetup(true, drawInfo, ref info, ref dd);
                drawInfo.DrawDataCache.AddRange(dd);
            }
            for(int d = 0; d < drawInfo.DrawDataCache.Count; d++)
            {
                if (drawInfo.DrawDataCache[d].color.A == 0)
                    drawInfo.DrawDataCache.RemoveAt(d);
            }
        }

        private static void DrawHat(TerraGuardian tg, TgDrawInfoHolder info, List<DrawData> drawdatas, ref PlayerDrawSet drawInfo)
        {
            if(tg.head < 0) return;
            Vector2 HatPosition = tg.GetAnimationPosition(AnimationPositions.HeadVanityPosition, tg.BodyFrameID, AlsoTakePosition: false, ConvertToCharacterPosition: false);
            if (HatPosition.X == HatPosition.Y && HatPosition.Y <= -10000)
                return;
            HatPosition = info.DrawPosition + HatPosition;
            //Main.NewText("Draw position: " + HatPosition.ToString());
            Texture2D headgear = Terraria.GameContent.TextureAssets.ArmorHead[tg.head].Value;
            int FrameX = headgear.Width, FrameY = (int)(headgear.Height * (1f / 20));
            drawdatas.Add(new DrawData(headgear, HatPosition, new Rectangle(0, 0, FrameX, FrameY), info.DrawColor, drawInfo.rotation, new Vector2(FrameX * 0.5f, FrameY * 0.5f), tg.Scale, drawInfo.playerEffect, 0));
        }

        public class DrawTerraGuardianBodyBehindMount : PlayerDrawLayer
        {
            public override bool IsHeadLayer => false;
            
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.MountBack);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer.mount.Active && drawInfo.drawPlayer is TerraGuardian;
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                DrawBehindLayer(ref drawInfo);
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
                return !drawInfo.drawPlayer.mount.Active && drawInfo.drawPlayer is TerraGuardian;
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                DrawBehindLayer(ref drawInfo);
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
                DrawFrontLayer(ref drawInfo);
            }
        }
    }
}