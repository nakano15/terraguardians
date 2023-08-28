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
        public override float Cooldown => 15;

        public override void Update(Companion User, SubAttackData Data)
        {
            if (Data.GetTimeSecs >= 3)
            {
                Data.EndUse();
            }
            else
            {
                LeonaCompanion Leona = User as LeonaCompanion;
                if (Leona.velocity.Y == 0)
                    Leona.MoveLeft = Leona.MoveRight = Leona.ControlJump = Leona.MoveDown = false;
            }
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            const short Frame = 33;
            User.BodyFrameID = User.ArmFramesID[0] = User.ArmFramesID[1] = Frame;
            (User as LeonaCompanion).SwordRotation += -50f * MainMod.Deg2Rad * User.direction;
        }

        public override bool PreHitAvoidDamage(Companion User, SubAttackData Data, Player.HurtInfo info)
        {
            Data.EndUse();
            User.UseSubAttack<LeonaGreatswordAttack>(true);
            return true;
        }
    }
}