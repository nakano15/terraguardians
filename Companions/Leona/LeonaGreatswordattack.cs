

namespace terraguardians.Companions.Leona
{
    internal class LeonaGreatswordAttack : SubAttackBase
    {
        public override string Name => "Greatsword Slice";
        public override string Description => "Leona will use the Greatsword she's carrying to attack a foe.";
        public override SubAttackData GetSubAttackData => new LeonaGreatswordAttackData();

        public class LeonaGreatswordAttackData : SubAttackData
        {

        }
    }
}