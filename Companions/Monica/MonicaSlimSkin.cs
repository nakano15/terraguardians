using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Companions.Monica;

public class MonicaSlimSkin : CompanionSkinInfo
{
    const string SlimBodyID = "slim";

    public override string Name => "Slim Monica";
    public override string Description => "Monica has lost weight during her travels.";
    public override bool Availability(Companion companion)
    {
        return nterrautils.QuestContainer.HasQuestBeenCompleted(QuestDB.MonicaSlimSkinQuest, MainMod.GetModName);
    }

    protected override void OnLoad()
    {
        AddTexture(SlimBodyID, "terraguardians/Companions/Monica/body_slim");
    }

    public override void PreDrawCompanions(Companion c, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
    {
        Holder.BodyTexture = GetTexture(SlimBodyID);
    }
}