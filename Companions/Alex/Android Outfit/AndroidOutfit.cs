using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using System.Collections.Generic;

namespace terraguardians.Companions.Alex
{
    public class AndroidOutfit : CompanionSkinInfo
    {
        public const string AndroidSkinBodyTextureID = "android_body", AndroidSkinLeftArmTextureID = "android_arm", AndroidSkinBodyFrontTextureID = "android_bodyf";
        public override string Name => "Android Outfit";
        public override string Description => "DO.NOT.FEAR.HUMAN.I.AM.PEACEFUL.AND.FRIENDLY.";
        protected override void OnLoad()
        {
            AddTexture(AndroidSkinBodyTextureID, "terraguardians/Companions/Alex/Android Outfit/alex_android_body");
            AddTexture(AndroidSkinBodyFrontTextureID, "terraguardians/Companions/Alex/Android Outfit/alex_android_bodyf");
            AddTexture(AndroidSkinLeftArmTextureID, "terraguardians/Companions/Alex/Android Outfit/alex_android_leftarm");
        }

        public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            Holder.BodyTexture = GetTexture(AndroidSkinBodyTextureID);
            Holder.BodyFrontTexture = GetTexture(AndroidSkinBodyFrontTextureID);
            Holder.ArmTexture[0] = GetTexture(AndroidSkinLeftArmTextureID);
        }
    }
}