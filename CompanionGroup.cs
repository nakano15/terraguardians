namespace terraguardians
{
    public class CompanionGroup
    {
        public virtual string ID { get { return ""; } }
        public virtual string Name { get { return ""; } }
        public virtual string Description { get { return ""; } }
        public virtual bool IsTerraGuardian { get { return false; } }
        public virtual float AgingSpeed { get { return 1f; } }
    }
}