using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Sardine
{
    public class CaitSithOutfit : CompanionSkinInfo
    {
        public override string Name => "Cait Sith Outfit";
        public override string Description => "Is said to have came from a Post Apocalyptic Tokyo. What is Tokyo?";

        protected override void OnLoad()
        {
            AddTexture("outfit", "terraguardians/Companions/Sardine/Cait Sith Outfit/caitsith_outfit");
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            Texture2D OutfitTexture = GetTexture("outfit");
            Vector2 DrawPos = Holder.DrawPosition;
            DrawPos.Y -= 16;
            for (int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == Holder.ArmTexture[1])
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    rect.Height += 16;
                    DrawData dd = new DrawData(OutfitTexture, DrawPos, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i, dd);
                    i++;
                }
                else if (DrawDatas[i].texture == Holder.BodyTexture)
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    rect.Height += 16;
                    DrawData dd;// = new DrawData(OutfitTexture, DrawPos, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    //DrawDatas.Insert(i, dd);
                    //i++;
                    rect.Y += rect.Height;
                    for (int x = 0; x < 3; x++)
                    {
                        rect.Y += rect.Height;
                        dd = new DrawData(OutfitTexture, DrawPos, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i + 1, dd);
                        i++;
                    }
                    if (c.head <= 0)
                    {
                        rect.Y += rect.Height;
                        dd = new DrawData(OutfitTexture, DrawPos, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i + 1, dd);
                        i++;
                    }
                }
                else if (DrawDatas[i].texture == Holder.BodyFrontTexture)
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrame;
                    rect.Height += 16;
                    rect.Y += rect.Height * 5;
                    DrawData dd = new DrawData(OutfitTexture, DrawPos, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
            }
        }
    }
}