using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Leona
{
    internal class LeonaGreatswordCounter : SubAttackBase
    {
        public override string Name => "Counter Attack";
        public override string Description => "Block an incoming attack and slice your foe with a powerful attack.";
        public override bool AllowItemUsage => false;

        public override void Update(Companion User, SubAttackData Data)
        {
            if (Data.GetTimeSecs >= 3)
            {
                Data.EndUse();
            }
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            const short Frame = 33;
            User.BodyFrameID = User.ArmFramesID[0] = User.ArmFramesID[1] = Frame;
        }
    }
}