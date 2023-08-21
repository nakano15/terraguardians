using Terraria;

namespace terraguardians
{
    public class SubAttackBase
    {
        public virtual string Name {get { return "MissingNAME"; }}
        public virtual string Description { get { return "Description is missing!"; }}
        public virtual float Cooldown { get { return 0; } }
    }
}