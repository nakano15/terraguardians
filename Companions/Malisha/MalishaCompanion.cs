using System.Collections.Generic;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians.Companions.Malisha
{
    public class MalishaCompanion : TerraGuardian
    {
        public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            if (IsMountedOnSomething)
                Holder.ThroneMode = true;
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            Rectangle rect = GetAnimationFrame(BodyFrameID);
            Texture2D TailTexture = Base.GetSpriteContainer.GetExtraTexture(MalishaBase.TailTextureID);
            if (TailTexture == null) return;
            if (!IsDrawingFrontLayer)
            {
                int InsertIndex = -1;
                Texture2D BodyTexture = Base.GetSpriteContainer.BodyTexture;
                for(int i = 0; i < DrawDatas.Count; i++)
                {
                    if (DrawDatas[i].texture == BodyTexture)
                    {
                        InsertIndex = i;
                        break;
                    }
                }
                if (IsMountedOnSomething)
                {
                    rect.Y += rect.Height * 2;
                }
                DrawData dd = new DrawData(TailTexture, Holder.DrawPosition, rect, Holder.DrawColor, 0f, Holder.Origin, Scale, drawSet.playerEffect, 0);
                dd.shader = Holder.BodyShader;
                if (InsertIndex > -1)
                    DrawDatas.Insert(InsertIndex++, dd);
                else
                    DrawDatas.Add(dd);
            }
            else
            {
                DrawData dd;
                if (IsMountedOnSomething)
                {
                    if (BodyFrameID == 14 || BodyFrameID == 28 || BodyFrameID == 27)
                    {
                        rect.Y += rect.Height * 2;
                        dd = new DrawData(TailTexture, Holder.DrawPosition, rect, Holder.DrawColor, 0f, Holder.Origin, Scale, drawSet.playerEffect, 0);
                        dd.shader = Holder.BodyShader;
                        DrawDatas.Add(dd);
                        rect.Y += rect.Height * 2;
                    }
                    else
                    {
                        rect.Y += rect.Height * 4;
                    }
                    dd = new DrawData(TailTexture, Holder.DrawPosition, rect, Holder.DrawColor, 0f, Holder.Origin, Scale, drawSet.playerEffect, 0);
                    dd.shader = Holder.BodyShader;
                    DrawDatas.Add(dd);
                }
                else
                {
                    if (BodyFrameID == 14 || BodyFrameID == 28 || BodyFrameID == 27)
                    {
                        dd = new DrawData(TailTexture, Holder.DrawPosition, rect, Holder.DrawColor, 0f, Holder.Origin, Scale, drawSet.playerEffect, 0);
                        dd.shader = Holder.BodyShader;
                        DrawDatas.Add(dd);
                    }
                }
            }
        }
    }
}