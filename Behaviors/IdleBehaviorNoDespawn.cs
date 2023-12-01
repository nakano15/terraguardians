using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class IdleBehaviorNoDespawn : IdleBehavior
    {
        public override bool AllowDespawning => false;
    }
}