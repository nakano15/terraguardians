using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class MarksmanshipSkill : CompanionSkillBase
    {
        public override string Name => "Marksmanship";
        public override string Description => "Increases Damage and Accuracy of ranged attacks.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.GetDamage<RangedDamageClass>() += Power * 0.03f;
            companion.Accuracy += Power * 0.025f;
            //Need accuracy too
        }
    }
}