using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class LuckSkill : CompanionSkillBase
    {
        public override string Name => "Luck";
        public override string Description => "Not only increases general critical rate, but also may cause good things to happen.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            int Increase = (int)(Power * 0.25f);
            companion.GetCritChance<MeleeDamageClass>() += Increase;
            companion.GetCritChance<RangedDamageClass>() += Increase;
            companion.GetCritChance<MagicDamageClass>() += Increase;
            companion.luck += Increase;
        }
    }
}