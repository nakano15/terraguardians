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

        CaptainStenchBase.WeaponInfusions Infusion = CaptainStenchBase.WeaponInfusions.None;
        int Damage = 0;
        int Duration = 21;
        int HitFrame = 0;
        float FrameDurationPercentage = 1f;
        int UseDirection = 0;

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
            Damage = 15 + (int)(Damage * .8f);
            Infusion = (User as CaptainStenchBase.StenchCompanion).CurrentInfusion;
            Duration = 21;
            if (Infusion != CaptainStenchBase.WeaponInfusions.None)
                Damage *= 2;
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Amethyst:
                    {

                    }
                    break;
            }
            FrameDurationPercentage = 1f / ((float)Duration / 4f);
            HitFrame = Duration / 2;
            UseDirection = User.direction;
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            if (Data.GetTime == HitFrame)
            {
                Rectangle Rect = new Rectangle(20, 12, (int)(40 * User.Scale), (int)(86 * User.Scale));
                if (User.direction < 0)
                {
                    Rect.X = -Rect.Width - Rect.X;
                }
                Rect.X += (int)User.Center.X;
                Rect.Y += (int)User.Bottom.Y - Rect.Height;
                HurtCharactersInRectangle(User, Rect, Damage, DamageClass.Melee, 5f, Data, User.direction);
            }
            if (Data.GetTime >= Duration)
            {
                Data.EndUse();
                return;
            }
            User.direction = UseDirection;
            User.LockCharacterDirection = true;
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            short ArmFrame = (short)(45 + MathF.Min(FrameDurationPercentage * Data.GetTime, 3));
            User.ArmFramesID[1] = ArmFrame;
        }
    }
}