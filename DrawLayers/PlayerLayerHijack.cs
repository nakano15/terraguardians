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
    public class PlayerLayerHijack
    {
        public class BehindPlayerArm : PlayerDrawLayer
        {
            public override bool IsHeadLayer => false;
            public override Position GetDefaultPosition()
            {
                return new BeforeParent(PlayerDrawLayers.ArmOverItem);
            }

            public override bool GetDefaultVisibility(PlayerDrawSet drawInfo)
            {
                return true;
            }

            protected override void Draw(ref PlayerDrawSet drawInfo)
            {
                switch(TerraGuardiansPlayerRenderer.GetDrawRule)
                {
                    case DrawContext.BackLayer:
                        drawInfo.colorArmorBody = 
                        drawInfo.colorBodySkin = 
                        drawInfo.itemColor = 
                        drawInfo.colorUnderShirt = 
                        drawInfo.colorShirt =
                        drawInfo.armGlowColor = 
                        drawInfo.bodyGlowColor =
                        drawInfo.ArkhalisColor = 
                            new Color(0, 0, 0, 0);
                        //drawInfo.heldProjOverHand = false;
                        //drawInfo.projectileDrawPosition = -1;
                        break;
                    case DrawContext.FrontLayer:
                        drawInfo.DrawDataCache.Clear();
                        break;
                }
            }
        }
    }
}