using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Bree
{
    public class WitchSkin : WitchOutfit
    {
        public override string Name => "Witch Skin";
        public override string Description => "She might be needing a medic; urgently.";

        protected override void OnLoad()
        {
            AddTexture(WitchBodyTextureID, "terraguardians/Companions/Bree/Outfits/Witch Outfit/witch_body");
            AddTexture(WitchLeftArmTextureID, "terraguardians/Companions/Bree/Outfits/Witch Outfit/witch_left_arm");
            AddTexture(WitchRightArmTextureID, "terraguardians/Companions/Bree/Outfits/Witch Outfit/witch_right_arm");
            AddTexture(WitchBodyFrontTextureID, "terraguardians/Companions/Bree/Outfits/Witch Outfit/witch__leg");
        }

        public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            Holder.BodyTexture = GetTexture(WitchBodyTextureID);
            Holder.ArmTexture[0] = GetTexture(WitchLeftArmTextureID);
            Holder.ArmTexture[1] = GetTexture(WitchRightArmTextureID);
            Holder.BodyFrontTexture = GetTexture(WitchBodyFrontTextureID);
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            /*Texture2D WitchBodyTexture = GetTexture(WitchBodyTextureID),
                WitchBodyFrontTexture = GetTexture(WitchBodyFrontTextureID);
            for(int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == Holder.BodyTexture)
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    rect.Y += rect.Height * 2;
                    DrawData dd = DrawDatas[i];
                    ReplaceTexture(WitchBodyTexture, rect, ref dd);
                    DrawDatas[i] = dd;
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[0])
                {
                    Rectangle rect = (c as TerraGuardian).ArmFrame[0];
                    rect.Y += rect.Height * 5;
                    DrawData dd = DrawDatas[i];
                    ReplaceTexture(WitchBodyTexture, rect, ref dd);
                    DrawDatas[i] = dd;
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[1])
                {
                    Rectangle rect = (c as TerraGuardian).ArmFrame[1];
                    DrawData dd = DrawDatas[i];
                    ReplaceTexture(WitchBodyTexture, rect, ref dd);
                    DrawDatas[i] = dd;
                }
            }*/
        }
    }
}