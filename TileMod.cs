using System.Collections.Generic;
using Terraria;
using Terraria.Graphics;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class TileMod : GlobalTile
    {
        public override bool PreDraw(int i, int j, int type, SpriteBatch spriteBatch)
        {
            DrawBehindTileCompanion();
            return base.PreDraw(i, j, type, spriteBatch);
        }

        private void DrawBehindTileCompanion()
        {
            if (!SystemMod.DrawCompanionBehindTileFlag)
            {
                return;
            }
            SystemMod.DrawCompanionBehindTileFlag = false;
            SamplerState samplerState = Main.Camera.Sampler;
            Camera camera = Main.Camera;
            Vector2 PositionDiference = Main.Camera.ScaledPosition - Main.Camera.UnscaledPosition;
            if (!Main.drawToScreen)
            {
                PositionDiference.X -= Main.offScreenRange;
                PositionDiference.Y -= Main.offScreenRange;
            }
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(c.GetDrawMomentType() == CompanionDrawMomentTypes.BehindTiles && c.InDrawRange())
                {
                    /*if (c.mount.Active && c.fullRotation != 0f)
                    {
                        samplerState = LegacyPlayerRenderer.MountedSamplerState;
                    }*/
                    c.position += PositionDiference;
                    c.DrawCompanion();
                    c.position -= PositionDiference;
                }
            }
        }
    }
}