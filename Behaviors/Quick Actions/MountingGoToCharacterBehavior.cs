using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Behaviors.Actions
{
    public class MountingGoToCharacterBehavior : BehaviorBase
    {
        Player Target;
        int Duration = 0;

        public MountingGoToCharacterBehavior(Player Target)
        {
            this.Target = Target;
        }

        public override void OnBegin()
        {
            if (GetOwner.GetMountedOnCharacter == Target || GetOwner.GetCharacterMountedOnMe == Target)
            {
                GetOwner.ToggleMount(Target);
                Deactivate();
            }
        }

        public override void Update(Companion companion)
        {
            if (companion.dead || !Target.active || Target.dead)
            {
                Deactivate();
                return;
            }
            Player ReferenceCharacter = PlayerMod.GetPlayerImportantControlledCharacter(Target);
            MoveTowards(companion, ReferenceCharacter.Bottom);
            if (Duration >= 600 || ReferenceCharacter.Hitbox.Intersects(companion.Hitbox))
            {
                companion.ToggleMount(Target);
                Deactivate();
            }
        }
    }
}