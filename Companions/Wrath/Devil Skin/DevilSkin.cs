using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions.Wrath
{
    public class DevilOutfit : CompanionSkinInfo
    {
        public override string Name => "Devil Skin";
        public override string Description => "";

        public override bool Availability(Companion companion)
        {
            return companion.HasItem(ModContent.ItemType<Items.Outfits.Wrath.UnholyAmulet>());
        }

        protected override void OnLoad()
        {
            AddTexture("body", "terraguardians/Companions/Wrath/Devil Skin/Wrath_Devil");
            AddTexture("bodyf", "terraguardians/Companions/Wrath/Devil Skin/Wrath_Devil_BodyF");
            AddTexture("leftarm", "terraguardians/Companions/Wrath/Devil Skin/Wrath_Devil_LeftArm");
            AddTexture("rightarm", "terraguardians/Companions/Wrath/Devil Skin/Wrath_Devil_RightArm");
            AddTexture("rightarmf", "terraguardians/Companions/Wrath/Devil Skin/Wrath_Devil_RightArmF");
        }

        public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            Holder.BodyTexture = GetTexture("body");
            Holder.BodyFrontTexture = GetTexture("bodyf");
            Holder.ArmTexture[0] = GetTexture("leftarm");
            Holder.ArmTexture[1] = GetTexture("rightarm");
            Holder.ArmFrontTexture[1] = GetTexture("rightarmf");
        }
    }
}