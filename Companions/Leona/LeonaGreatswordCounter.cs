using Terraria;
using Terraria.Audio;
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

        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            if(User.TargettingSomething && (User as LeonaCompanion).HoldingSword)
            {
                Vector2 Diference = User.Target.Center - User.Center;
                if (MathF.Abs(Diference.X) < (User.Target.width + User.width) * 0.5f + 20 && 
                    MathF.Abs(Diference.Y) < (User.Target.height + User.height) * 0.5f + 20)
                {
                    return true;
                }
            }
            return base.AutoUseCondition(User, Data);
        }

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
            User.UseSubAttack<LeonaGreatswordAttack>(true, false);
            User.immune = true;
            User.immuneTime = 80;
            User.direction = info.HitDirection;
            SoundEngine.PlaySound(Terraria.ID.SoundID.NPCHit5, User.Center);
            if (Main.rand.NextFloat() < 0.8f)
                User.SaySomethingAtRandom(new string[]{ "*Predictable!*", "*Foolishness!*" });
            User.AddBuff(ModContent.BuffType<Buffs.LeonaCounter>(), 60);
            return true;
        }
    }
}