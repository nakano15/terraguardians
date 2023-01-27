using System.Collections.Generic;

namespace terraguardians
{
    public class CompanionSkillContainer
    {
        private static CompanionSkillBase DefaultSkill = new CompanionSkillBase();
        private static Dictionary<string, CompanionModSkillsContainer> ModSkills = new Dictionary<string, CompanionModSkillsContainer>();
        
        public static CompanionSkillBase GetSkillBase(uint ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (!ModSkills.ContainsKey(ModID) || !ModSkills[ModID].HasSkill(ID))
                return DefaultSkill;
            return ModSkills[ModID].GetSkill(ID);
        }

        public static void Unload()
        {
            DefaultSkill = null;
            ModSkills.Clear();
            ModSkills = null;
        }
        
        public class CompanionModSkillsContainer
        {
            private Dictionary<uint, CompanionSkillBase> skills = new Dictionary<uint, CompanionSkillBase>();

            public CompanionSkillBase GetSkill(uint Skill)
            {
                return skills[Skill];
            }

            public bool HasSkill(uint Skill)
            {
                return skills.ContainsKey(Skill);
            }

            public void Unload()
            {
                skills.Clear();
                skills = null;
            }
        }
    }
}