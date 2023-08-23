using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Leona
{
    internal class LeonaGreatswordAttack : SubAttackBase
    {
        public override string Name => "Greatsword Slice";
        public override string Description => "Leona will use the Greatsword she's carrying to attack a foe.";
        public override SubAttackData GetSubAttackData => new LeonaGreatswordAttackData();
        public override float Cooldown => 5f;

        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            LeonaCompanion Leona = User as LeonaCompanion;
            if (User.TargettingSomething && Leona.HoldingSword)
            {
                Vector2 Distance = (User.Target.Center - Leona.Center);
                int TargetWidth = User.Target.width, TargetHeight = User.Target.height;
                if (MathF.Abs(Distance.X) < TargetWidth + 80 && MathF.Abs(Distance.Y) < TargetHeight + 80)
                {
                    return true;
                }
            }
            return false;
        }

        public override void OnBeginUse(Companion User, SubAttackData data)
        {
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            Data.SwingDuration = (int)(MathF.Max(3, 40 * User.GetAttackSpeed<MeleeDamageClass>()));
            Data.Damage = 20;
        }

        public override void Update(Companion User, SubAttackData data)
        {
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            if (Data.GetTime >= Data.SwingDuration)
            {
                Data.EndUse();
            }
            LeonaCompanion Leona = User as LeonaCompanion;
            Leona.MoveLeft = Leona.MoveRight = Leona.ControlJump = Leona.MoveDown = false;
            Leona.controlUseItem = false;
            Data.AnimationPercentage = Data.GetTime * (1f / Data.SwingDuration);
            Data.SwordPercentage = MathF.Min(Data.AnimationPercentage * 1.3f, 1);
            Data.SwordPercentage = Data.SwordPercentage * Data.SwordPercentage;
            //Data.AnimationPercentage *= Data.AnimationPercentage;
            Rectangle ItemHitbox = new Rectangle((int)User.Center.X, (int)User.Center.Y, (int)(80 * User.Scale), (int)(80 * User.Scale));
            if (Data.SwordPercentage < 0.333f)
            {
                ItemHitbox.Inflate((int)(ItemHitbox.Width * 0.2f), (int)(ItemHitbox.Height * 0.4f));
                ItemHitbox.X += (int)(ItemHitbox.Width * 0.5f * User.direction);
            }
            else if (Data.SwordPercentage < 0.666f)
            {
                ItemHitbox.Inflate((int)(ItemHitbox.Width * 0.3f), (int)(ItemHitbox.Height * 0.3f));
                ItemHitbox.X += (int)(ItemHitbox.Width * 0.4f * User.direction);
                ItemHitbox.Y -= (int)(ItemHitbox.Height * 0.4f * User.gravDir);
            }
            else
            {
                ItemHitbox.Inflate((int)(ItemHitbox.Width * 0.6f), (int)(ItemHitbox.Height * 0.2f));
                ItemHitbox.Y -= (int)(ItemHitbox.Width * 0.5f * User.gravDir);
            }
            HurtCharactersInRectangle(User, ItemHitbox, Data.Damage, ModContent.GetInstance<MeleeDamageClass>(), 8, Data);
        }

        public override void UpdateAnimation(Companion User, SubAttackData data)
        {
            LeonaCompanion Leona = User as LeonaCompanion;
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            short AnimationFrame = User.Base.GetAnimation(AnimationTypes.HeavySwingFrames).GetFrameFromPercentage(Data.AnimationPercentage);
            User.BodyFrameID = User.ArmFramesID[0] = User.ArmFramesID[1] = AnimationFrame;
            Leona.SwordRotation = MainMod.Deg2Rad * ((30 + 245f * Data.SwordPercentage) * User.direction);
        }

        public class LeonaGreatswordAttackData : SubAttackData
        {
            public float AnimationPercentage = 1f;
            public float SwordPercentage = 1f;
            public int SwingDuration = 12;
            public int Damage = 20;
        }
    }
}