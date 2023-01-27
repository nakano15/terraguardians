using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Skills
{
    public class AcrobaticsSkill : CompanionSkillBase
    {
        public override string Name => "Acrobatics";
        public override string Description => "Increases Jump Speed, Fall Distance Tolerance and Dodge Rate.";

        public override void UpdateStatus(Companion companion, int Level, float Power)
        {
            companion.jumpSpeedBoost += Power * 0.033f;
            companion.extraFall += (int)(companion.Base.FallHeightTolerance * Power * 0.3f);
            companion.DodgeRate += Power * 1.5f;
        }
    }
}