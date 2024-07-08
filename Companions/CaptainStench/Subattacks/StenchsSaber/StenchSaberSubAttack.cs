using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;

namespace terraguardians.Companions.CaptainStench.Subattacks
{
    public class StenchSaberSubAttack : SubAttackBase
    {
        public override string Name => "Stench's Saber";
        public override string Description => "Stench slashes her opponents with her saber.";
        public override float Cooldown => base.Cooldown;

        CaptainStenchBase.WeaponInfusions Infusion = CaptainStenchBase.WeaponInfusions.None;
        int Damage = 0;
        int CritRate = -1;
        int Duration = 21;
        int HitFrame = 0;
        float FrameDurationPercentage = 1f;
        int UseDirection = 0;
        bool Crouching = false;

        public override bool CanUse(Companion User, SubAttackData Data)
        {
            return (User as CaptainStenchBase.StenchCompanion).HoldingWeapon;
        }

        public override void OnBeginUse(Companion User, SubAttackData Data)
        {
            Damage = 0;
            for (int i = 0; i < 10; i++)
            {
                if (User.inventory[i].type > 0 && User.inventory[i].damage > 0 && User.inventory[i].DamageType.CountsAsClass<MeleeDamageClass>())
                {
                    if (User.inventory[i].damage > Damage)
                        Damage = User.inventory[i].damage;
                }
            }
            Crouching = User.MoveDown;
            CritRate = -1;
            Damage = 15 + (int)(Damage * .8f);
            Infusion = (User as CaptainStenchBase.StenchCompanion).CurrentInfusion;
            Duration = 21;
            if (Infusion != CaptainStenchBase.WeaponInfusions.None)
                Damage *= 2;
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Sapphire:
                    {
                        Duration = (int)(Duration * .4f);
                        Damage = (int)(Damage * 0.75f);
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Emerald:
                    {
                        CritRate = 50;
                        Damage = (int)(Damage * .9f);
                    }
                    break;
            }
            FrameDurationPercentage = 1f / ((float)Duration / (Crouching ? 3f : 4f));
            HitFrame = Duration / 2;
            UseDirection = User.direction;
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            if (Data.GetTime == HitFrame)
            {
                Rectangle Rect = new Rectangle((int)(20 * User.Scale), (int)(12 * User.Scale), (int)(40 * User.Scale), (int)(86 * User.Scale));
                if (User.direction < 0)
                {
                    Rect.X = -Rect.Width - Rect.X;
                }
                Rect.X += (int)User.Center.X;
                Rect.Y += (int)User.Bottom.Y - Rect.Height;
                Entity[] Targets = HurtCharactersInRectangleAndGetTargets(User, Rect, Damage, DamageClass.Melee, 5f, Data, out int[] DamageDealt, UseDirection, CritRate: CritRate);
                DoYEGGHitEffect(User, Targets, DamageDealt);
            }
            if (Data.GetTime >= Duration)
            {
                Data.EndUse();
                return;
            }
            User.direction = UseDirection;
            User.LockCharacterDirection = true;
        }

        void DoYEGGHitEffect(Companion User, Entity[] Targets, int[] DamageDealt)
        {
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Ruby:
                    {
                        if (Targets.Length > 0)
                        {
                            int DDSum = 0;
                            foreach (int i in DamageDealt)
                            {
                                if (i > 0)
                                    DDSum += i;
                        }
                        User.Heal((int)MathF.Max(1, DDSum * .15f));
                        }
                    }
                    break;
            }
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            short ArmFrame;
            if (Crouching)
            {
                ArmFrame = (short)(50 + MathF.Min(FrameDurationPercentage * Data.GetTime, 2));
            }
            else
            {
                ArmFrame = (short)(45 + MathF.Min(FrameDurationPercentage * Data.GetTime, 3));
            }
            User.ArmFramesID[1] = ArmFrame;
        }
    }
}