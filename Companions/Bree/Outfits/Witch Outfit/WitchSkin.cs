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

        public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            Texture2D WitchBodyTexture = GetTexture(WitchBodyTextureID),
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
            }
        }
    }
}