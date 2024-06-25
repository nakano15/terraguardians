using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Brutus
{
    public class BrutusCombatBehavior : CombatBehavior
    {
        public override void Update(Companion companion)
        {
            AllowMovement = !companion.IsFollower || !(companion.followBehavior as Brutus.BrutusFollowerBehavior).ProtectionModeActivated;
            UpdateWeaponProfiles(companion);
            if (UsingNewCombatBehavior)
                NewUpdateCombat(companion);
            else
                UpdateCombat(companion);
        }
    }
}