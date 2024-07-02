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
    public class StenchTripleSlashSubAttack : SubAttackBase
    {
        public override string Name => "Triple Slash Combo";
        public override string Description => "";
        
        CaptainStenchBase.WeaponInfusions Infusion = CaptainStenchBase.WeaponInfusions.None;
        int Damage = 0;
        int CritRate = -1;
        int Duration = 14*6; //Each frame is 100ms, and there's 14 frames, so 1 frame for each 6 frames.
        int HitFrame = 0;
        float FrameDurationPercentage = 1f;
        int UseDirection = 0;
        int LastFrame = 0;
        int AThirdOfDuration = 1;

        public override bool AllowItemUsage => false;

        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            return base.AutoUseCondition(User, Data);
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
            CritRate = -1;
            Damage = 15 + (int)(Damage * .8f);
            Infusion = (User as CaptainStenchBase.StenchCompanion).CurrentInfusion;
            Duration = 14 * 6;
            if (Infusion != CaptainStenchBase.WeaponInfusions.None)
                Damage *= 2;
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Amethyst:
                    {
                        
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Emerald:
                    {
                        CritRate = 50;
                        Damage = (int)(Damage * .9f);
                    }
                    break;
            }
            FrameDurationPercentage = .2f;
            UseDirection = User.direction;
            LastFrame = 0;
            switch (Main.rand.Next(3))
            {
                default:
                    User.SaySomething("Bugger off!!!");
                    break;
                case 1:
                    User.SaySomething("Ha! Fanny!!!");
                    break;
                case 2:
                    User.SaySomething("Take This! Ya Bludger!!!");
                    break;
            }
            AThirdOfDuration = Duration / 6;
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            int NewFrame = (int)(Data.GetTime * FrameDurationPercentage);
            User.MoveRight = User.MoveLeft = User.controlJump = false;
            User.immuneTime = 5;
            User.direction = UseDirection;
            if (NewFrame != LastFrame)
            {
                if (NewFrame == 2 || NewFrame == 6 || NewFrame == 10)
                {
                    Rectangle Rect = new Rectangle((int)(10 * User.Scale), (int)(34 * User.Scale), (int)(44 * User.Scale), (int)(100 * User.Scale));
                    if (User.direction < 0)
                    {
                        Rect.X = -Rect.Width - Rect.X;
                    }
                    Rect.X += (int)User.Center.X;
                    Rect.Y += (int)User.Bottom.Y - Rect.Height;
                    Entity[] Targets = HurtCharactersInRectangleAndGetTargets(User, Rect, Damage, DamageClass.Melee, 5f, Data, out int[] DamageDealt, UseDirection, CritRate: CritRate);
                    DoOnHitEffect(User, Targets, DamageDealt);
                }
            }
            UpdateYegg(User, Data.GetTime);
            if (Data.GetTime == Duration)
            {
                (User as CaptainStenchBase.StenchCompanion).HoldingWeapon = true;
                Data.EndUse();
            }
            LastFrame = NewFrame;
        }

        void UpdateYegg(Companion User, int Time)
        {
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Amethyst:
                    {
                        if (Time == AThirdOfDuration || Time == AThirdOfDuration * 3 || Time == AThirdOfDuration * 5)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * 28 * User.Scale, Vector2.UnitX * UseDirection * 17f, ModContent.ProjectileType<Projectiles.SallySpecials.PurpleWave>(), Damage * 2, 3f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Topaz:
                    {
                        if (Time == AThirdOfDuration || Time == AThirdOfDuration * 3 || Time == AThirdOfDuration * 5)
                        {
                            for (int y = -1; y <= 2; y += 2)
                                Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * (28 + 12 * y) * User.Scale, Vector2.UnitX * UseDirection * 22f, ModContent.ProjectileType<Projectiles.SallySpecials.TopazShard>(), (int)(Damage * 1.5f), 5f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Ruby:
                    {
                        if (Time == AThirdOfDuration || Time == AThirdOfDuration * 3 || Time == AThirdOfDuration * 5)
                        {
                            for (int y = -1; y < 2; y++)
                                Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * ((28 + 66 * y) * User.Scale), Vector2.UnitX * UseDirection * 16f, ModContent.ProjectileType<Projectiles.SallySpecials.BloodSickle>(), (int)(Damage * .5f), 6f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Emerald:
                    {
                        if (Time == AThirdOfDuration || Time == AThirdOfDuration * 3 || Time == AThirdOfDuration * 5)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * 28 * User.Scale, Vector2.UnitX * UseDirection * 10f, ModContent.ProjectileType<Projectiles.SallySpecials.Tornado>(), (int)(Damage * .8f), 3f, User.whoAmI);
                        }
                    }
                    break;
            }
        }

        void DoOnHitEffect(Companion User, Entity[] Targets, int[] DamageDealt)
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
            short Frame = (short)(53 + MathF.Min(14, LastFrame));
            User.BodyFrameID = Frame;
            for (int i = 0; i < 2; i++)
                User.ArmFramesID[i] = Frame;
        }
    }
}