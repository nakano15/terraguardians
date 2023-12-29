using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Blue
{
    public class RedHoodOutfit : CompanionSkinInfo
    {
        public const string RedHoodSkinOutfitID = "red_hood_outfit", RedHoodSkinOutfitBodyFrontID = "red_hood_outfit_body_f";
        bool IsWithCloak = true;
        public override string Name
        {
            get
            {
                if (!IsWithCloak)
                    return "Adventurer Outfit";
                return "Red Riding Hood Outfit";
            }
        }
        public override string Description
        {
            get
            {
                if (!IsWithCloak)
                    return "Same as Red Riding Hood outfit, but without the cloak.";
                return "A wolf will not question another's appearance, right?";
            }
        }

        public RedHoodOutfit(bool WithCloak)
        {
            IsWithCloak = WithCloak;
        }

        protected override void OnLoad()
        {
            AddTexture(RedHoodSkinOutfitID, "terraguardians/Companions/Blue/Red Hood Outfit/red_hood_outfit");
            AddTexture(RedHoodSkinOutfitBodyFrontID, "terraguardians/Companions/Blue/Red Hood Outfit/red_hood_outfit_body_f");
        }

        public override void CompanionDrawLayerSetup(Companion c, bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            Texture2D OutfitTexture = GetTexture(RedHoodSkinOutfitID), OutfitFrontTexture = GetTexture(RedHoodSkinOutfitBodyFrontID);
            CompanionSpritesContainer sprites = c.Base.GetSpriteContainer;
            const int TextureGap = 96 * 2;
            int CloakFrame = c.BodyFrameID;
            if(IsWithCloak && c.head > 0)
            {
                if(CloakFrame < 11 || (CloakFrame >= 16 && CloakFrame <= 20) || (CloakFrame >= 24 && CloakFrame <= 26) || CloakFrame == 29 || CloakFrame == 38/* || CloakFrame == 39*/)
                {
                    CloakFrame = 13;
                }
                else if ((CloakFrame >= 20 && CloakFrame <= 23) || CloakFrame == 33)
                {
                    CloakFrame = 14;
                }
                else if (CloakFrame == 27 || CloakFrame == 31)
                {
                    CloakFrame = 15;
                }
            }
            DrawData dd;
            for (int i = 0; i < DrawDatas.Count; i++)
            {
                if (DrawDatas[i].texture == Holder.BodyTexture)
                {
                    Rectangle rect;
                    //Shirt
                    rect = (c as TerraGuardian).BodyFrame;
                    rect.Y += 4 * TextureGap;
                    dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i += 1;
                    //Pants
                    rect = (c as TerraGuardian).BodyFrame;
                    rect.Y += 3 * TextureGap;
                    dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i += 1;
                    //Shoes
                    rect = (c as TerraGuardian).BodyFrame;
                    rect.Y += 2 * TextureGap;
                    dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i += 1;
                    if (IsWithCloak)
                    {
                        //Cloak Right Arm
                        rect = (c as TerraGuardian).ArmFrame[1];
                        rect.Y += 6 * TextureGap;
                        dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i + 1, dd);
                        i += 1;
                        //Head
                        rect = (c as TerraGuardian).GetAnimationFrame(CloakFrame);
                        rect.Y += 5 * TextureGap;
                        dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i + 1, dd);
                        i += 1;
                    }
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[0]) //Left Arm
                {
                    Rectangle rect;
                    //Shirt Sleeve
                    rect = (c as TerraGuardian).ArmFrame[0];
                    rect.Y += 7 * TextureGap;
                    dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i += 1;
                    if (IsWithCloak)
                    {
                        //Cloak Front
                        rect = (c as TerraGuardian).ArmFrame[0];
                        rect.Y += 8 * TextureGap;
                        dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i + 1, dd);
                        i += 1;
                        //Head
                        rect = (c as TerraGuardian).GetAnimationFrame(CloakFrame);
                        rect.Y += 9 * TextureGap;
                        dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i - 1, dd);
                        i += 1;
                    }
                }
                else if (DrawDatas[i].texture == Holder.ArmTexture[1]) //Right Arm
                {
                    Rectangle rect;
                    rect = (c as TerraGuardian).ArmFrame[1];
                    if (IsWithCloak)
                    {
                        //Cloak Right Arm Back
                        dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                        DrawDatas.Insert(i, dd);
                        i += 1;
                    }
                    rect.Y += TextureGap;
                    dd = new DrawData(OutfitTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i++;
                }
                else if (DrawDatas[i].texture == Holder.BodyFrontTexture) //Body Front
                {
                    Rectangle rect = (c as TerraGuardian).BodyFrontFrame;
                    dd = new DrawData(OutfitFrontTexture, Holder.DrawPosition, rect, Holder.DrawColor, c.fullRotation, Holder.Origin, c.Scale, drawSet.playerEffect);
                    DrawDatas.Insert(i + 1, dd);
                    i += 1;
                }
            }
        }
    }
}