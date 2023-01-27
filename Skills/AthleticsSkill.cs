using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class AthleticsSkill : CompanionSkillBase
    {
        public override string Name => "Athletics";
        public override string Description => "Increases Movement Speed and Max Health.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.moveSpeed += Power * 0.01f;
            companion.statLifeMax2 += (int)(companion.Base.InitialMaxHealth * (Power * 0.025f));
        }
    }
}