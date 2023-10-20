using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Bree
{
    public class WitchOutfit : CompanionSkinInfo
    {
        public const string WitchBodyTextureID = "witch_b", WitchLeftArmTextureID = "witch_left_arm", WitchRightArmTextureID = "witch_right_arm", WitchOutfitTextureID = "witch_outfit", WitchBodyFrontTextureID = "witch_bf", WitchBroomTextureID = "witch_broom";
        public override string Name => "Witch Outfit";
        public override string Description => "Matches the grumpy face.";

        protected override void OnLoad()
        {
            AddTexture(WitchOutfitTextureID, "terraguardians/Companions/Bree/Outfits/Witch Outfit/witch_outfit");
            AddTexture(WitchBroomTextureID, "terraguardians/Companions/Bree/Outfits/Witch Outfit/witch_broom");
        }
        public override bool Availability(Companion companion)
        {
            return companion.HasItem(ModContent.ItemType<Items.Outfits.Bree.WitchOutfit>());
        }

        public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            Texture2D WitchBodyTexture = GetTexture(WitchOutfitTextureID),
                WitchBroomTexture = GetTexture(WitchBroomTextureID);
            Texture BreeBag = c.Base.GetSpriteContainer.GetExtraTexture("bag");
            for(int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == BreeBag)
                {
                    DrawDatas.RemoveAt(i);
                    i--;
                    /*DrawData dd = DrawDatas[i];
                    ReplaceTexture(WitchBroomTexture, ref dd);
                    DrawDatas[i] = dd;*/
                }
                else if (DrawDatas[i].texture == Holder.BodyTexture)
                {
                    //Broom
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    DrawData dd = new DrawData(WitchBroomTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i - 1, dd);
                    i++;
                    rect.Y += rect.Height;
                    dd = new DrawData(WitchBroomTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                    //Outfit
                    //rect.Y += rect.Height * 2;
                    dd = new DrawData(WitchBodyTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                    rect.Y += rect.Height;
                    dd = new DrawData(WitchBodyTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[0])
                {
                    Rectangle rect = (c as TerraGuardian).ArmFrame[0];
                    rect.Y += rect.Height * 3;
                    DrawData dd = new DrawData(WitchBodyTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[1])
                {
                    Rectangle rect = (c as TerraGuardian).ArmFrame[1];
                    //rect.Y += rect.Height;
                    DrawData dd = new DrawData(WitchBodyTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
            }
        }
    }
}