namespace terraguardians
{
    public class CaitSithGroup : CompanionGroup
    {
        public override string ID => "cs";
        public override string Name => "Cait Sith";
        public override string Description => "Fairy Cat creatures that live in the Terra Realm. They are technically TerraGuardians, but it is unknown when they moved to Terra Realm.";
        public override float AgingSpeed => 1f / 0.272f;
        public override bool IsTerraGuardian => true;
    }
}