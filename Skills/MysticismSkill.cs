using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class MysticismSkill : CompanionSkillBase
    {
        public override string Name => "Mysticism";
        public override string Description => "Increases Damage of Magic attacks, and increases Accuracy.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.GetDamage<MagicDamageClass>() += Power* 0.03f;
            companion.Accuracy += Power * 0.025f;
        }
    }
}