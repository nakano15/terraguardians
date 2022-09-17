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
            try
            {
                if(pm.TestCompanion != null)
                {
                    pm.TestCompanion.DrawCompanion();
                }
            }
            catch{}
        }
    }
}