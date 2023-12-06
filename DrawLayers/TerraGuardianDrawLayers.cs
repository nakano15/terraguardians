using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria;
using Terraria.DataStructures;
using Terraria.Graphics;
using Terraria.ModLoader;
using Terraria.Graphics.Renderers;
using System.Collections.Generic;

namespace terraguardians
{
    public class TerraGuardianDrawLayersScript
    {
        //private static Color BodyColor = Color.White;
        private const float DivisionBy16 = 1f / 16;
        public static bool IgnoreLight = false;
        internal static bool DrawingOnTiles = false;

        public static void PreDrawSettings(ref PlayerDrawSet drawInfo)
        {
            if (DrawingOnTiles)
            {
                Vector2 PositionDiference = Vector2.Zero; //Main.Camera.ScaledPosition - Main.Camera.UnscaledPosition;
                if (!Main.drawToScreen)
                {
                    PositionDiference.X -= Main.offScreenRange;
                    PositionDiference.Y -= Main.offScreenRange;
                }
                drawInfo.Position -= PositionDiference;
            }
            if (drawInfo.drawPlayer is Companion companion)
            {
                if((drawInfo.drawPlayer as Companion).Base.IsInvalidCompanion)
                {
                    drawInfo.colorArmorBody = drawInfo.colorArmorHead = drawInfo.colorArmorLegs = drawInfo.colorBodySkin = 
                    drawInfo.colorEyes = drawInfo.colorEyeWhites = drawInfo.colorHair = drawInfo.colorHead = drawInfo.colorLegs =
                    drawInfo.colorPants = drawInfo.colorShirt = drawInfo.colorShoes = drawInfo.colorUnderShirt = drawInfo.armGlowColor = 
                    drawInfo.bodyGlowColor = drawInfo.headGlowColor = drawInfo.legsGlowColor = Color.Transparent;
                    drawInfo.headGlowMask = drawInfo.armGlowMask = drawInfo.headGlowMask = drawInfo.legsGlowMask = -1;
                }
                else
                {
                    TgDrawInfoHolder info = companion.GetNewDrawInfoHolder(drawInfo);
                    companion.PreDrawCompanions(ref drawInfo, ref info);
                    companion.Base.PreDrawCompanions(ref drawInfo, ref info);
                    if (companion.SubAttackInUse < 255)
                    {
                        companion.GetSubAttackActive.PreDraw(companion, ref drawInfo, ref info);
                    }
                    if (companion.GetSelectedSkin != null)
                    {
                        companion.GetSelectedSkin.PreDrawCompanions(companion, ref drawInfo, ref info);
                    }
                    if (companion.GetSelectedOutfit != null)
                    {
                        companion.GetSelectedOutfit.PreDrawCompanions(companion, ref drawInfo, ref info);
                    }
                    companion.GetGoverningBehavior().PreDrawCompanions(ref drawInfo, ref info);
                }
            }
            /*else if (drawInfo.drawPlayer is Companion companion)
            {
                TgDrawInfoHolder dhi = new TgDrawInfoHolder();
                //Companion companion = (Companion)drawInfo.drawPlayer;
                companion.PreDrawCompanions(ref drawInfo, ref dhi);
                companion.Base.PreDrawCompanions(ref drawInfo, ref dhi);
                if (companion.SubAttackInUse < 255)
                {
                    companion.GetSubAttackActive.PreDraw(companion, ref drawInfo, ref dhi);
                }
                companion.GetGoverningBehavior().PreDrawCompanions(ref drawInfo, ref dhi);
            }*/
        }

        private static void DrawBehindLayer(ref PlayerDrawSet drawInfo)
        {
            Companion companion = (Companion)drawInfo.drawPlayer;
            TgDrawInfoHolder info = companion.GetDrawInfo;
            if(info.Context == DrawContext.FrontLayer) return;
            CompanionSpritesContainer spritecontainer = companion.Base.GetSpriteContainer;
            if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
            {
                Vector2 TgOrigin = info.Origin;
                Color BodyColor = info.DrawColor;
                List<DrawData> dd = new List<DrawData>();
                if(companion is TerraGuardian tg)
                {
                    if (tg.ArmFramesID.Length >= 2) dd.Add(new DrawData(info.ArmTexture[1], info.DrawPosition + tg.ArmOffset[1], info.ArmFrame[1], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    DrawHat(true, tg, info, dd, ref drawInfo);
                    dd.Add(new DrawData(info.BodyTexture, info.DrawPosition + tg.BodyOffset, info.BodyFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    if (info.ThroneMode && tg.ArmFramesID.Length >= 1) dd.Add(new DrawData(info.ArmTexture[0], info.DrawPosition + tg.ArmOffset[0], info.ArmFrame[0], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    DrawHat(false, tg, info, dd, ref drawInfo);
                }
                companion.CompanionDrawLayerSetup(false, drawInfo, ref info, ref dd);
                companion.Base.CompanionDrawLayerSetup(false, drawInfo, ref info, ref dd);
                if (companion.SubAttackInUse < 255)
                {
                    companion.GetSubAttackActive.Draw(companion, false, drawInfo, ref info, ref drawInfo.DrawDataCache);
                }
                if (companion.GetSelectedSkin != null)
                {
                    companion.GetSelectedSkin.CompanionDrawLayerSetup(companion, false, drawInfo, ref info, ref dd);
                }
                if (companion.GetSelectedOutfit != null)
                {
                    companion.GetSelectedOutfit.CompanionDrawLayerSetup(companion, false, drawInfo, ref info, ref dd);
                }
                companion.GetGoverningBehavior().CompanionDrawLayerSetup(companion, false, drawInfo, ref info, ref dd);
                drawInfo.DrawDataCache.AddRange(dd);
            }
            //drawInfo.drawPlayer = tg;
        }

        private static void DrawFrontLayer(ref PlayerDrawSet drawInfo)
        {
            //if(!(drawInfo.drawPlayer is TerraGuardian)) return; //Even with the visibility setting, seems to activate on player. Projectile drawing seems to bypass visibility checking.
            Companion companion = (Companion)drawInfo.drawPlayer;
            TgDrawInfoHolder info = companion.GetDrawInfo;
            if(info.Context == DrawContext.BackLayer) return;
            CompanionSpritesContainer spritecontainer = companion.Base.GetSpriteContainer;
            if(spritecontainer.LoadState == CompanionSpritesContainer.SpritesLoadState.Loaded)
            {
                Vector2 TgOrigin = info.Origin;
                Color BodyColor = info.DrawColor;
                List<DrawData> dd = new List<DrawData>();
                if (companion is TerraGuardian tg)
                {
                    if (tg.BodyFrontFrameID > -1)
                        dd.Add(new DrawData(info.BodyFrontTexture, info.DrawPosition + tg.BodyOffset, info.BodyFrontFrame, BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    if (tg.ArmFramesID.Length >= 2 && info.ArmFrontTexture[1] != null)
                        dd.Add(new DrawData(info.ArmFrontTexture[1], info.DrawPosition + tg.ArmOffset[1], info.ArmFrontFrame[1], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    if (tg.ArmFramesID.Length >= 1)
                    {
                        if (!info.ThroneMode) dd.Add(new DrawData(info.ArmTexture[0], info.DrawPosition + tg.ArmOffset[0], info.ArmFrame[0], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                        if (tg.ArmFrontFramesID[0] > -1) dd.Add(new DrawData(info.ArmFrontTexture[0], info.DrawPosition + tg.ArmOffset[0], info.ArmFrontFrame[0], BodyColor, drawInfo.rotation, TgOrigin, tg.Scale, drawInfo.playerEffect, 0));
                    }
                }
                companion.CompanionDrawLayerSetup(true, drawInfo, ref info, ref dd);
                companion.Base.CompanionDrawLayerSetup(true, drawInfo, ref info, ref dd);
                if (companion.SubAttackInUse < 255)
                {
                    companion.GetSubAttackActive.Draw(companion, true, drawInfo, ref info, ref drawInfo.DrawDataCache);
                }
                companion.GetGoverningBehavior().CompanionDrawLayerSetup(companion, true, drawInfo, ref info, ref dd);
                if (companion.GetSelectedSkin != null)
                {
                    companion.GetSelectedSkin.CompanionDrawLayerSetup(companion, true, drawInfo, ref info, ref dd);
                }
                if (companion.GetSelectedOutfit != null)
                {
                    companion.GetSelectedOutfit.CompanionDrawLayerSetup(companion, true, drawInfo, ref info, ref dd);
                }
                drawInfo.DrawDataCache.AddRange(dd);
            }
            /*float LastDrawProjPos = drawInfo.projectileDrawPosition;
            for(int d = 0; d < drawInfo.DrawDataCache.Count; d++)
            {
                if (d != drawInfo.projectileDrawPosition && drawInfo.DrawDataCache[d].color.A == 0)
                {
                    drawInfo.DrawDataCache.RemoveAt(d);
                    if (d < drawInfo.projectileDrawPosition) drawInfo.projectileDrawPosition--;
                }
            }*/
        }

        internal static void HideFrontLayers(Player player)
        {
            PlayerDrawLayers.ArmOverItem.Hide();
            PlayerDrawLayers.BladedGlove.Hide();
            PlayerDrawLayers.EyebrellaCloud.Hide();
            PlayerDrawLayers.FrontAccFront.Hide();
            PlayerDrawLayers.HandOnAcc.Hide();
            PlayerDrawLayers.MountFront.Hide();
        }

        internal static void HideBackLayers(Player player)
        {
            PlayerDrawLayers.BalloonAcc.Hide();
            PlayerDrawLayers.ArmorLongCoat.Hide();
            PlayerDrawLayers.BackAcc.Hide();
            PlayerDrawLayers.FaceAcc.Hide();
            PlayerDrawLayers.FrontAccBack.Hide();
            PlayerDrawLayers.HairBack.Hide();
            PlayerDrawLayers.Head.Hide();
            PlayerDrawLayers.HeadBack.Hide();
            PlayerDrawLayers.JimsCloak.Hide();
            PlayerDrawLayers.Leggings.Hide();
            PlayerDrawLayers.NeckAcc.Hide();
            PlayerDrawLayers.OffhandAcc.Hide();
            PlayerDrawLayers.Robe.Hide();
            PlayerDrawLayers.Shoes.Hide();
            PlayerDrawLayers.Skin.Hide();
            PlayerDrawLayers.SkinLongCoat.Hide();
            PlayerDrawLayers.Tails.Hide();
            PlayerDrawLayers.Torso.Hide();
            PlayerDrawLayers.WaistAcc.Hide();
            PlayerDrawLayers.Wings.Hide();
            PlayerDrawLayers.MountBack.Hide();
            PlayerDrawLayers.Shield.Hide();
        }

        internal static void HideLayers(Player player)
        {
            if (player is TerraGuardian)
            {
                PlayerDrawLayers.ArmorLongCoat.Hide();
                PlayerDrawLayers.ArmOverItem.Hide();
                PlayerDrawLayers.BackAcc.Hide();
                PlayerDrawLayers.BalloonAcc.Hide();
                PlayerDrawLayers.BladedGlove.Hide();
                PlayerDrawLayers.EyebrellaCloud.Hide();
                PlayerDrawLayers.FaceAcc.Hide();
                PlayerDrawLayers.FrontAccBack.Hide();
                PlayerDrawLayers.FrontAccFront.Hide();
                PlayerDrawLayers.HairBack.Hide();
                PlayerDrawLayers.HandOnAcc.Hide();
                PlayerDrawLayers.Head.Hide();
                PlayerDrawLayers.HeadBack.Hide();
                PlayerDrawLayers.JimsCloak.Hide();
                PlayerDrawLayers.Leggings.Hide();
                PlayerDrawLayers.NeckAcc.Hide();
                PlayerDrawLayers.OffhandAcc.Hide();
                PlayerDrawLayers.Robe.Hide();
                PlayerDrawLayers.Shield.Hide();
                PlayerDrawLayers.Shoes.Hide();
                PlayerDrawLayers.Skin.Hide();
                PlayerDrawLayers.SkinLongCoat.Hide();
                PlayerDrawLayers.Tails.Hide();
                PlayerDrawLayers.Torso.Hide();
                PlayerDrawLayers.WaistAcc.Hide();
                PlayerDrawLayers.Wings.Hide();
                PlayerDrawLayers.Backpacks.Hide();
            }
            else
            {
                
            }
        }

        private static void DrawLosangle(ref PlayerDrawSet drawInfo)
        {
            Vector2 LosanglePosition = drawInfo.Position;
            LosanglePosition.X += drawInfo.drawPlayer.width * 0.5f;
            LosanglePosition.Y += drawInfo.drawPlayer.height - 2;
            drawInfo.DrawDataCache.Add(new DrawData(MainMod.LosangleOfUnknown.Value, LosanglePosition - Main.screenPosition, null, Color.White, 0, new Vector2(12, 48), 1f, SpriteEffects.None, 0));
        }

        private static void DrawHat(bool Back, TerraGuardian tg, TgDrawInfoHolder info, List<DrawData> drawdatas, ref PlayerDrawSet drawInfo)
        {
            if (!tg.LastHatCompatibility.IsCompatible) return; //Broken?
            int id;
            if (Back)
            {
                if (tg.head < 0)
                    id = -1;
                else
                    id = Terraria.ID.ArmorIDs.Head.Sets.FrontToBackID[tg.head];
            }
            else
            {
                id = tg.head;
            }
            if(id < 0 || tg.IsUsingThroneOrBench || tg.IsUsingBed) return;
            Vector2 HatPosition = tg.Base.GetAnimationPosition(AnimationPositions.HeadVanityPosition).GetPositionFromFrame(tg.BodyFrameID);
            if (!Terraria.ID.ArmorIDs.Head.Sets.DrawHead[id] || (HatPosition.X == HatPosition.Y && HatPosition.Y <= -1000))
                return;
            HatPosition = tg.GetAnimationPosition(AnimationPositions.HeadVanityPosition, tg.BodyFrameID);
            if (tg.sitting.isSitting)
            {
                HatPosition.X -= tg.sitting.offsetForSeat.X;
                HatPosition.Y += tg.sitting.offsetForSeat.Y;
            }
            HatPosition -= Main.screenPosition;//info.DrawPosition;
            HatPosition.Y += tg.gfxOffY;
            Texture2D headgear = Terraria.GameContent.TextureAssets.ArmorHead[id].Value;
            int FrameX = headgear.Width, FrameY = (int)(headgear.Height * (1f / 20));
            HatPosition.X = (int)HatPosition.X;
            HatPosition.Y = (int)HatPosition.Y;
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
                return drawInfo.drawPlayer.mount.Active && drawInfo.drawPlayer is TerraGuardian && !(drawInfo.drawPlayer as Companion).Base.IsInvalidCompanion;
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                if(drawInfo.drawPlayer is Companion && (drawInfo.drawPlayer as Companion).Base.IsInvalidCompanion)
                {
                    DrawLosangle(ref drawInfo);
                }
                else
                {
                    DrawBehindLayer(ref drawInfo);
                }
            }
        }

        public class DrawLosangleLayer : PlayerDrawLayer
        {
            public override bool IsHeadLayer => true;
            
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.HeldItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer is Companion && (drawInfo.drawPlayer as Companion).Base.IsInvalidCompanion;
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                DrawLosangle(ref drawInfo);
            }
        }

        public class DrawTerraGuardianBody : PlayerDrawLayer
        {
            public override bool IsHeadLayer => false;
            
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.HeldItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return !drawInfo.drawPlayer.mount.Active && drawInfo.drawPlayer is Companion && !(drawInfo.drawPlayer as Companion).Base.IsInvalidCompanion;
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
                return new AfterParent(PlayerDrawLayers.HeldItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer is Companion && !(drawInfo.drawPlayer as Companion).Base.IsInvalidCompanion;
            }

            public override bool IsHeadLayer => false;

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                DrawFrontLayer(ref drawInfo);
            }
        }

        public class DrawTerraGuardianWeapons : PlayerDrawLayer
        {
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.HeldItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return drawInfo.drawPlayer is TerraGuardian && !(drawInfo.drawPlayer as TerraGuardian).Base.IsInvalidCompanion;
            }

            public override bool IsHeadLayer => false;

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                TerraGuardian tg = drawInfo.drawPlayer as TerraGuardian;
                Vector2 ItemLocationBackup = drawInfo.ItemLocation;
                float ItemRotation = tg.itemRotation;
                int ItemAnimationBackup = tg.itemAnimation;
                for(int i = 1; i < tg.ArmFramesID.Length; i++) //Remove the false on getdefaultvisibility before testing this out.
                {
                    TerraGuardian.HeldItemSetting held = tg.HeldItems[i];
                    if(held.SelectedItem > -1)
                    {
                        drawInfo.heldItem = tg.inventory[held.SelectedItem];
                        drawInfo.ItemLocation = (drawInfo.Position + held.ItemPosition - tg.position);
                        tg.itemRotation = held.ItemRotation;
                        tg.itemAnimation = held.ItemAnimation;
                        Terraria.DataStructures.PlayerDrawLayers.DrawPlayer_27_HeldItem(ref drawInfo);
                    }
                }
                drawInfo.ItemLocation = ItemLocationBackup;
                tg.itemRotation = ItemRotation;
                tg.itemAnimation = ItemAnimationBackup;
                drawInfo.heldItem = tg.inventory[tg.HeldItems[0].SelectedItem];
                //tg.JustDroppedAnItem = true;
            }
        }

        public class DrawPathingGuide : PlayerDrawLayer
        {
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.HeldItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return MainMod.DebugMode && drawInfo.drawPlayer is Companion;
            }

            public override bool IsHeadLayer => false;

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                Companion c = drawInfo.drawPlayer as Companion;
                if(c.Path.Path.Count == 0)
                {
                    return;
                }
                foreach(PathFinder.Breadcrumb node in c.Path.Path)
                {
                    Vector2 NodePos = new Vector2(node.X * 16 - Main.screenPosition.X, node.Y * 16 - Main.screenPosition.Y);
                    Main.spriteBatch.Draw(MainMod.PathGuideTexture.Value, NodePos, new Rectangle(16 * node.NodeOrientation, 0, 16, 16), Color.White);
                }
            }
        }
    }
}