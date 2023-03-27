using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class MountCompanionBehavior : BehaviorBase
    {
        Player Target;

        public MountCompanionBehavior(Player Target)
        {
            this.Target = Target;
        }

        public override void Update(Companion companion)
        {

        }
    }
}