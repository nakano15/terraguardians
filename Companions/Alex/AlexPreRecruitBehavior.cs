using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Companions.Blue
{
    public class AlexPreRecruitBehavior : BehaviorBase
    {
        public AlexPreRecruitBehavior()
        {
            CanBeHurtByNpcs = false;
            CanTargetNpcs = false;
        }
    }
}