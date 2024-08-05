using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.GameContent;

namespace terraguardians.Companions.Quentin
{
    public class QuentinCompanion : Companion
    {
        public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            
        }

        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (PlayerDrawLayers.Head.Visible)
            {
                Companion c = Holder.GetCompanion;
                Texture2D Texture = c.Base.GetSpriteContainer.GetExtraTexture(QuentinBase.HeadTextureID);
                Texture2D PlayerHeadTexture = TextureAssets.Players[0, 0].Value;
                byte SkinID = 0;
                if (c.head > 0 || c.SkinID == 1)
                    SkinID = 1;
                for(int i = 0; i < drawSet.DrawDataCache.Count; i++)
                {
                    if(drawSet.DrawDataCache[i].texture == PlayerHeadTexture)
                    {
                        Vector2 Position = Holder.DrawPosition;
                        if ((c.bodyFrame.Y >= 7 * 56 && c.bodyFrame.Y < 10 * 56) || 
                            (c.bodyFrame.Y >= 14 * 56 && c.bodyFrame.Y < 17 * 56))
                            Position.Y -= 2 * c.gravDir;
                        Rectangle rect = new Rectangle(0, 0, 40, 58);
                        DrawData dd = new DrawData(Texture, Position, rect, Holder.DrawColor, drawSet.rotation, Holder.Origin, 1, drawSet.playerEffect, 0);
                        drawSet.DrawDataCache[i] = dd;
                        rect.Y = rect.Height * (SkinID == 1 ? 2 : 1);
                        dd = new DrawData(Texture, Position, rect, Holder.DrawColor, drawSet.rotation, Holder.Origin, 1, drawSet.playerEffect, 0);
                        drawSet.DrawDataCache.Insert(i + 1, dd);
                        break;
                    }
                }
            }
        }
    }
}