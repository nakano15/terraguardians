using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Zack
{
    public class MeatBagOutfit : CompanionSkinInfo
    {
        public override string Name => "Meat Bag Outfit";
        public override string Description => "";

        public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            Holder.BodyFrontTexture = GetTexture("outfitf");
            Holder.ArmTexture[0] = GetTexture("leftarm");
            Holder.ArmTexture[1] = GetTexture("rightarm");
        }

        protected override void OnLoad()
        {
            AddTexture("outfit", "terraguardians/Companions/Zack/Meat Bag Outfit/meatbagoutfit");
            AddTexture("outfitf", "terraguardians/Companions/Zack/Meat Bag Outfit/meatbagoutfit_f");
            AddTexture("leftarm", "terraguardians/Companions/Zack/Meat Bag Outfit/meatbag_leftarm");
            AddTexture("rightarm", "terraguardians/Companions/Zack/Meat Bag Outfit/meatbag_rightarm");
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            const bool OldSkin = false;
            Texture2D Outfit = GetTexture("outfit");
            for (int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == Holder.BodyTexture)
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    DrawData dd = new DrawData(Outfit, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                    if (OldSkin)
                    {
                        rect.Y += rect.Height * 4;
                    }
                    else
                    {
                        rect.Y += rect.Height * 2;
                    }
                    dd = new DrawData(Outfit, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
                /*else if (DrawDatas[i].texture == Holder.ArmTexture[0]) //Needs review on spritework.
                {
                    Rectangle rect = (c as TerraGuardian).ArmFrame[0];
                    rect.Y += rect.Height * 10;
                    DrawData dd = new DrawData(Outfit, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }*/
            }
        }
    }
}