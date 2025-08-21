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
    public class EffectsDrawLayer : PlayerDrawLayer
    {
        public override Position GetDefaultPosition()
        {
            return new BeforeParent(PlayerDrawLayers.ElectrifiedDebuffFront);
        }

        protected override void Draw(ref PlayerDrawSet drawInfo)
        {
            if (drawInfo.drawPlayer.HasBuff<Buffs.Love>() && Main.rand.NextBool(15))
            {
                Vector2 Velocity = new Vector2(Main.rand.Next(-10, 11), Main.rand.Next(-10, 11));
                Velocity.Normalize();
                Velocity.X *= 0.66f;
                int gore = Gore.NewGore(new EntitySource_Misc(""), drawInfo.drawPlayer.position + new Vector2(Main.rand.Next(drawInfo.drawPlayer.width + 1), Main.rand.Next(drawInfo.drawPlayer.height + 1)), Velocity * Main.rand.Next(3, 6) * 0.33f, 331, Main.rand.Next(40, 121) * 0.01f);
                Main.gore[gore].sticky = false;
                Main.gore[gore].velocity *= 0.4f;
                Main.gore[gore].velocity.Y -= 0.6f;
            }
        }
    }
}