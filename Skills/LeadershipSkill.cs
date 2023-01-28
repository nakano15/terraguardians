using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class LeadershipSkill : CompanionSkillBase
    {
        public override string Name => "Leadership";
        public override string Description => "Increases the Damage of Summon attacks and Max Health.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.statLifeMax += (int)(companion.Base.InitialMaxHealth * (Level * 0.05f));
            companion.GetDamage<SummonDamageClass>() += Level * 0.03f;
        }
    }
}