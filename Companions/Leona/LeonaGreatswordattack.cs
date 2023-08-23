using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.Companions.Leona
{
    internal class LeonaGreatswordAttack : SubAttackBase
    {
        public override string Name => "Greatsword Slice";
        public override string Description => "Leona will use the Greatsword she's carrying to attack a foe.";
        public override SubAttackData GetSubAttackData => new LeonaGreatswordAttackData();

        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            //LeonaCompanion Leona = User as LeonaCompanion;
            return true;
        }

        public override void OnBeginUse(Companion User, SubAttackData data)
        {
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            Data.SwingDuration = (int)(MathF.Max(3, 40 * User.GetAttackSpeed<MeleeDamageClass>()));
            Data.Damage = (int)(User.GetDamage<MeleeDamageClass>().Multiplicative * 20);
        }

        public override void Update(Companion User, SubAttackData data)
        {
            LeonaGreatswordAttackData Data = (LeonaGreatswordAttackData)data;
            if (Data.GetTime >= Data.SwingDuration)
            {
                Data.EndUse();
            }
            LeonaCompanion Leona = User as LeonaCompanion;
            Data.AnimationPercentage = Data.GetTime * (1f / Data.SwingDuration);
            Data.SwordPercentage = Data.AnimationPercentage * Data.AnimationPercentage;
            //Data.AnimationPercentage *= Data.AnimationPercentage;
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
            public int SwingDuration = 15;
            public int Damage = 20;
        }
    }
}