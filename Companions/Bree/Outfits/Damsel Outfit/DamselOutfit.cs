using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Bree
{
    public class DamselOutfit : CompanionSkinInfo
    {
        public const string DamselOutfitTextureID = "damsel", DamselOutfitFrontTextureID = "damsel_f";
        public override string Name => "Damsel Outfit";
        public override string Description => "";
        protected override void OnLoad()
        {
            AddTexture(DamselOutfitTextureID, "terraguardians/Companions/Bree/Outfits/Damsel Outfit/damsel_outfit");
            AddTexture(DamselOutfitFrontTextureID, "terraguardians/Companions/Bree/Outfits/Damsel Outfit/damsel_outfit_front");
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            Texture2D OutfitTexture = GetTexture(DamselOutfitTextureID),
                OutfitFrontTexture = GetTexture(DamselOutfitFrontTextureID);
            for (int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == Holder.BodyTexture)
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    rect.Y += rect.Height;
                    DrawData dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                    if (c.head == 0)
                    {
                        rect.Y += rect.Height;
                        dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i + 1, dd);
                        i++;
                    }
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[1])
                {
                    DrawData dd = new DrawData(OutfitTexture, Holder.DrawPosition, (c as TerraGuardian).BodyFrame, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[0])
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    rect.Y += rect.Height * 3;
                    DrawData dd = new DrawData(OutfitTexture, Holder.DrawPosition,rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
                else if (DrawDatas[i].texture == Holder.BodyFrontTexture)
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrontFrame;
                    DrawData dd = new DrawData(OutfitFrontTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
            }
        }
    }
}