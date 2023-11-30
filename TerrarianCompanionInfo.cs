using Microsoft.Xna.Framework;
using System;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class TerrarianCompanionInfo
    {
        public int HairStyle = 1;
        public int SkinVariant = 1;
        public Color HairColor = new Color(),
            EyeColor = new Color(),
            SkinColor = new Color(),
            ShirtColor = new Color(),
            UndershirtColor = new Color(),
            PantsColor = new Color(),
            ShoesColor = new Color();
        
        public void Save(TagCompound save, uint UniqueID)
        {
            save.Add("CompanionGenericHair_"+UniqueID, HairStyle);
            save.Add("CompanionGenericSkin_"+UniqueID, SkinVariant);
            save.Add("CompanionGenericHairColor_"+UniqueID, HairColor);
            save.Add("CompanionGenericEyeColor_"+UniqueID, EyeColor);
            save.Add("CompanionGenericSkinColor_"+UniqueID, SkinColor);
            save.Add("CompanionGenericShirtColor_"+UniqueID, ShirtColor);
            save.Add("CompanionGenericUShirtColor_"+UniqueID, UndershirtColor);
            save.Add("CompanionGenericPantsColor_"+UniqueID, PantsColor);
            save.Add("CompanionGenericShoesColor_"+UniqueID, ShoesColor);
        }
        
        public void Load(TagCompound tag, uint UniqueID, uint LastVersion)
        {
            HairStyle = tag.GetInt("CompanionGenericHair_"+UniqueID);
            SkinVariant = tag.GetInt("CompanionGenericSkin_"+UniqueID);
            HairColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericHairColor_"+UniqueID);
            EyeColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericEyeColor_"+UniqueID);
            SkinColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericSkinColor_"+UniqueID);
            ShirtColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericShirtColor_"+UniqueID);
            UndershirtColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericUShirtColor_"+UniqueID);
            PantsColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericPantsColor_"+UniqueID);
            ShoesColor = tag.Get<Microsoft.Xna.Framework.Color>("CompanionGenericShoesColor_"+UniqueID);
        }
    }
}