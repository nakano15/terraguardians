using Terraria;
using Terraria.ID;
using Terraria.GameContent.UI;
using Microsoft.Xna.Framework;

namespace terraguardians
{
    public class PreRecruitNoMonsterAggroBehavior : PreRecruitBehavior
    {
        public PreRecruitNoMonsterAggroBehavior() : base()
        {
            CanBeHurtByNpcs = false;
            CanTargetNpcs = false;
        }
    }
}