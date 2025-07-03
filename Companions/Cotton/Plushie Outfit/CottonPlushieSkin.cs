using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace terraguardians.Companions.Cotton;

public class CottonPlushieSkin : CompanionSkinInfo
{
    const string BodyID = "body", LeftArmID = "leftarm", RightArmID = "rightarm",
        LeftArmFrontID = "leftarmfront", RightArmFrontID = "rightarmfront",
        BodyFrontID = "bodyfront";

    public override string Name => "Plushie";
    public override string Description => "Now he's really a plushie dog.\n - Made by Backrevol";

    public override bool Availability(Companion companion)
    {
        return companion.HasItem(ModContent.ItemType<Items.Skins.Cotton.CursedNeedle>());
    }

    protected override void OnLoad()
    {
        AddTexture(BodyID, "terraguardians/Companions/Cotton/Plushie Outfit/body");
        AddTexture(BodyFrontID, "terraguardians/Companions/Cotton/Plushie Outfit/body_front");
        AddTexture(LeftArmID, "terraguardians/Companions/Cotton/Plushie Outfit/left_arm");
        AddTexture(LeftArmFrontID, "terraguardians/Companions/Cotton/Plushie Outfit/left_arm_front");
        AddTexture(RightArmID, "terraguardians/Companions/Cotton/Plushie Outfit/right_arm");
        AddTexture(RightArmFrontID, "terraguardians/Companions/Cotton/Plushie Outfit/right_arm_front");
    }

    public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
    {
        Holder.BodyTexture = GetTexture(BodyID);
        Holder.BodyFrontTexture = GetTexture(BodyFrontID);
        Holder.ArmTexture[0] = GetTexture(LeftArmID);
        Holder.ArmFrontTexture[0] = GetTexture(LeftArmFrontID);
        Holder.ArmTexture[1] = GetTexture(RightArmID);
        Holder.ArmFrontTexture[1] = GetTexture(RightArmFrontID);
    }
}