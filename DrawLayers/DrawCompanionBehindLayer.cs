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
    public class DrawCompanionBehindLayer : PlayerDrawLayer
    {
        public override bool IsHeadLayer => false;

        public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
        {
            return PlayerMod.IsPlayerCharacter(drawInfo.drawPlayer);
        }

        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.Leggings);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            PlayerMod pm = drawInfo.drawPlayer.GetModPlayer<PlayerMod>();
            //Is stopping rendering the rest of the character, because the draw layers are being erased after drawing companion.
            //Main.spriteBatch.End();
            try
            {
                if(pm.TestCompanion != null)
                {
                    IPlayerRenderer rendererbackup = Main.PlayerRenderer;
                    Main.PlayerRenderer = new LegacyPlayerRenderer();
                    /*Main.PlayerRenderer.DrawPlayer(Main.Camera, pm.TestCompanion, pm.TestCompanion.position, 
                    pm.TestCompanion.fullRotation, pm.TestCompanion.fullRotationOrigin);*/
                    SamplerState laststate = Main.graphics.GraphicsDevice.SamplerStates[0];
                    Main.spriteBatch.End();
                    Main.PlayerRenderer.DrawPlayers(Main.Camera, new Player[]{ pm.TestCompanion });
                    Main.spriteBatch.Begin((SpriteSortMode)1, BlendState.AlphaBlend, laststate, DepthStencilState.None, 
                        Main.Camera.Rasterizer, null, Main.Camera.GameViewMatrix.TransformationMatrix);
                    Main.PlayerRenderer = rendererbackup;
                }
            }
            catch{}
            //Main.spriteBatch.Begin();
        }
    }
}