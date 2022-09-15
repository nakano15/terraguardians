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
        public static bool IsTG = false;
        private static Color BodyColor = Color.White;

        public static void PreDrawSettings(Player player, PlayerDrawSet drawInfo)
        {
            if ((IsTG = player is TerraGuardian))
            {
                drawInfo.colorArmorBody = drawInfo.colorArmorHead = drawInfo.colorArmorLegs = drawInfo.colorBodySkin = 
                drawInfo.colorEyes = drawInfo.colorEyeWhites = drawInfo.colorHair = drawInfo.colorHead = drawInfo.colorLegs =
                drawInfo.colorPants = drawInfo.colorShirt = drawInfo.colorShoes = drawInfo.colorUnderShirt = Color.Transparent;
            }
        }

        public class DrawTerraGuardianBody : PlayerDrawLayer
        {
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.Skin);
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                
            }
        }
    }
}