

using Terraria;

namespace terraguardians.WeaponProfiles
{
    public class CrimsonRodWeapon : WeaponProfile
    {
        public override bool CanUseSpecialWeapon(Companion companion, Item item)
        {
            return companion.ownedProjectileCounts[243] < 1 && companion.ownedProjectileCounts[244] < 1;
        }

        public override bool IsSpecialWeapon(Companion companion, Item item)
        {
            return true;
        }

        public override void WeaponUsageCustomBehavior(Companion companion, Item item, ref CombatBehavior.BehaviorFlags Flags)
        {
            if (companion.Target != null)
            {
                Entity Target = companion.Target;
                Flags.TargetPosition.X += System.MathF.Sign(Target.velocity.X) * (4f * 16f);
                Flags.TargetPosition.Y -= 6f * 16f;
            }
        }
    }
}