using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class BallisticSkill : CompanionSkillBase
    {
        public override string Name => "Ballistic";
        public override string Description => "Increases Damage and Accuracy of ranged attacks.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.GetDamage<RangedDamageClass>() += Power * 0.03f;
            //Need accuracy too
        }
    }
}