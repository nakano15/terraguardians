using System.Linq;
using System.Collections.Generic;
using terraguardians.Skills;

namespace terraguardians
{
    public class CompanionSkillContainer
    {
        private static CompanionSkillBase DefaultSkill = CompanionSkillBase.GetInvalidSkill();
        private static Dictionary<string, CompanionModSkillsContainer> ModSkills = new Dictionary<string, CompanionModSkillsContainer>();
        public const uint StrengthID = 0,
            EnduranceID = 1,
            AthleticsID = 2,
            AcrobaticsID = 3,
            MarksmanshipID = 4,
            MysticismID = 5,
            LeadershipID = 6,
            LuckID = 7;

        internal static void Initialize()
        {
            AddSkill(new StrengthSkill(), StrengthID);
            AddSkill(new EnduranceSkill(), EnduranceID);
            AddSkill(new AthleticsSkill(), AthleticsID);
            AddSkill(new AcrobaticsSkill(), AcrobaticsID);
            AddSkill(new MarksmanshipSkill(), MarksmanshipID);
            AddSkill(new MysticismSkill(), MysticismID);
            AddSkill(new LeadershipSkill(), LeadershipID);
            AddSkill(new LuckSkill(), LuckID);
        }

        internal static bool AddSkill(CompanionSkillBase Skill, uint ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (!ModSkills.ContainsKey(ModID))
                ModSkills.Add(ModID, new CompanionModSkillsContainer());
            if (ModSkills[ModID].HasSkill(ID)) return false;
            ModSkills[ModID].AddSkill(ID, Skill);
            return true;
        }

        public static CompanionSkillBase GetSkillBase(uint ID, string ModID = "")
        {
            if (ModID == "") ModID = MainMod.GetModName;
            if (!ModSkills.ContainsKey(ModID) || !ModSkills[ModID].HasSkill(ID))
                return DefaultSkill;
            return ModSkills[ModID].GetSkill(ID);
        }

        internal static string[] GetSkillModKeys()
        {
            return ModSkills.Keys.ToArray();
        }

        internal static CompanionModSkillsContainer GetModSkillContainer(string ModID)
        {
            return ModSkills[ModID];
        }

        public static void Unload()
        {
            DefaultSkill = null;
            foreach(string skill in ModSkills.Keys)
            {
                ModSkills[skill].Unload();
            }
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

            internal void AddSkill(uint ID, CompanionSkillBase Skill)
            {
                skills.Add(ID, Skill);
            }

            internal uint[] GetSkillIDs()
            {
                return skills.Keys.ToArray();
            }

            public void Unload()
            {
                skills.Clear();
                skills = null;
            }
        }
    }
}