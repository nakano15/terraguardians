namespace terraguardians
{
    public class CompanionSkillBase
    {
        public virtual string Name { get { return "???"; } }
        public virtual string Description { get { return "???"; } }

        public virtual void UpdateStatus(Companion companion, int Level, float Power)
        {

        }

        public virtual float GetMaxSkillProgressForLevel(int Level)
        {
            float Value = 0;
            const float MaxValue = 500000000;
            try
            {
                checked { Value = System.Math.Min(Level * 25 - (Level - 1) * 12 + 20, MaxValue); }
            }
            catch
            {
                Value = MaxValue;
            }
            return Value;
        }
    }
}