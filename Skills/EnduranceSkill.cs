using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class EnduranceSkill : CompanionSkillBase
    {
        public override string Name => "Endurance";
        public override string Description => "Increases Max Health, Defense, Block Rate and Defense Rate.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.statLifeMax2 += (int)(companion.Base.InitialMaxHealth * (Power * 0.05f));
            companion.DefenseRate += Power * 0.05f;
            companion.statDefense += (int)(Power * 2);
            if (companion.BlockRate > 0) companion.BlockRate += Power * 1.5f;
        }
    }
}