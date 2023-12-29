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
    public class GhostFoxHauntDrawLayer : PlayerDrawLayer
    {
        public override string Name => "TerraGuardians: Ghost Fox Haunt";
        public override bool IsHeadLayer => false;

        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.Skin);
        }

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return !Main.gameMenu && drawInfo.drawPlayer.GetModPlayer<PlayerMod>().GhostFoxHaunt;
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.shadow != 0) return;
            Player player = drawInfo.drawPlayer;
            bool PlayerKOd = !player.dead && PlayerMod.GetPlayerKnockoutState(player) > KnockoutStates.Awake;
            const int Frame = 10, ReviveFrame = 15;
            CompanionBase cb = MainMod.GetCompanionBase(CompanionDB.Fluffles);
            float DirectionFace = player.direction;
            CompanionSpritesContainer csc = cb.GetSpriteContainer;
            SpriteEffects seffects = drawInfo.playerEffect;
            Vector2 FlufflesPosition = cb.GetAnimationPosition(AnimationPositions.HandPosition, 0).GetPositionFromFrame((short)(PlayerKOd ? ReviveFrame : Frame));
            FlufflesPosition.X -= cb.SpriteWidth * .5f;
            if (DirectionFace > 0)
            {
                FlufflesPosition.X *= -1f;
            }
            Vector2 HauntPosition = drawInfo.Position;
            HauntPosition.X += player.width * .5f - 6f * DirectionFace;
            if (PlayerKOd)
            {
                HauntPosition.Y += player.height * .35f;
                DirectionFace *= -1;
                if (seffects == SpriteEffects.None)
                    seffects = SpriteEffects.FlipHorizontally;
                else if (seffects == SpriteEffects.FlipHorizontally)
                    seffects = SpriteEffects.None;
                FlufflesPosition.X *= -1f;
            }
            HauntPosition.Y += player.height + (cb.SpriteHeight - FlufflesPosition.Y - 30) * cb.Scale;
            HauntPosition.X += FlufflesPosition.X * cb.Scale;
            Vector2 Origin = new Vector2(cb.SpriteWidth * .5f, cb.SpriteHeight);
            Rectangle DrawFrame = new Rectangle((PlayerKOd ? ReviveFrame : Frame) * cb.SpriteWidth, 0, cb.SpriteWidth, cb.SpriteHeight);
            float Opacity = MainMod.FlufflesHauntOpacity * .8f;
            if (Opacity < 0)
                Opacity = 0;
            Color color = Companions.FlufflesBase.FlufflesCompanion.GhostfyColor(Color.White, Opacity, Companions.FlufflesBase.FlufflesCompanion.GetColorMod);
            bool IgnoreRotation = PlayerKOd;
            DrawData dd = new DrawData(csc.ArmSpritesTexture[1], HauntPosition - Main.screenPosition, DrawFrame, color, 0, Origin, 1f, seffects, 0);
            dd.ignorePlayerRotation = IgnoreRotation;
            drawInfo.DrawDataCache.Add(dd);
            dd = new DrawData(csc.BodyTexture, HauntPosition - Main.screenPosition, DrawFrame, color, 0, Origin, 1f, seffects, 0);
            dd.ignorePlayerRotation = IgnoreRotation;
            drawInfo.DrawDataCache.Add(dd);
            dd = new DrawData(csc.ArmSpritesTexture[0], HauntPosition - Main.screenPosition, DrawFrame, color, 0, Origin, 1f, seffects, 0);
            dd.ignorePlayerRotation = IgnoreRotation;
            drawInfo.DrawDataCache.Add(dd);
        }
    }
}