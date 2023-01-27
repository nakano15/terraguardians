using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class StrengthSkill : CompanionSkillBase
    {
        public override string Name => "Strength";
        public override string Description => "Increases Damage, Attack Speed and Knockback of melee attacks.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.GetDamage<MeleeDamageClass>() += Power * 0.03f;
            companion.GetAttackSpeed<MeleeDamageClass>() += Power * 0.02f;
            companion.GetKnockback<MeleeDamageClass>() += Power * 0.02f;
        }
    }
}