using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Companions.Liebre;

public class LiebreIgnoreMourningBehaviour : MournPlayerBehavior
{
    public override void UpdateMourning(Companion companion)
    {
        Deactivate();
    }
}