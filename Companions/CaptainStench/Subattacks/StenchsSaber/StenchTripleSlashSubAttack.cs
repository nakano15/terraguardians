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
        int Duration = 14*6; //Each frame is 100ms, and there's 14 frames, so 1 frame for each 6 frames.
        int HitFrame = 0;
        float FrameDurationPercentage = 1f;
        int UseDirection = 0;
        int LastFrame = 0;

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
            }
            FrameDurationPercentage = .2f;
            UseDirection = User.direction;
            LastFrame = 0;
            switch (Main.rand.Next(7))
            {
                default:
                    User.SaySomething("AAAAAAAAAAAAAIEEEEEEEEEEEE!!!");
                    break;
                case 1:
                    User.SaySomething("YEEEEEEEEARRRRGHHHHH!!!");
                    break;
                case 2:
                    User.SaySomething("YAAAAAAAAAAAYYIIEEEE!!!");
                    break;
                case 3:
                    User.SaySomething("NYYYYYAAAAAARRGUEEEEE!!!");
                    break;
                case 4:
                    User.SaySomething("Bugger off!!!");
                    break;
                case 5:
                    User.SaySomething("Ha! Fanny!!!");
                    break;
                case 6:
                    User.SaySomething("Take This! Ya Bludger!!!");
                    break;
            }
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            int NewFrame = (int)(Data.GetTime * FrameDurationPercentage);
            //User.velocity.X = UseDirection * 0.4f;
            User.MoveRight = User.MoveLeft = User.controlJump = false;
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
                    Entity[] Targets = HurtCharactersInRectangleAndGetTargets(User, Rect, Damage, DamageClass.Melee, 5f, Data, UseDirection);
                }
            }
            UpdateYegg(User, Data.GetTime);
            if (NewFrame == 15)
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
                case CaptainStenchBase.WeaponInfusions.None:
                case CaptainStenchBase.WeaponInfusions.Amethyst:
                    {
                        int AThirdOfDuration = Duration / 3;
                        if (Time == AThirdOfDuration || Time == AThirdOfDuration * 2 || Time == Duration)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Center, Vector2.UnitX * UseDirection * 17f, ModContent.ProjectileType<Projectiles.SallyEGGs.PurpleWave>(), Damage * 2, 3f, User.whoAmI);
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