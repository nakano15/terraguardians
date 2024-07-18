using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.DataStructures;
using System.Collections.Generic;
using System;
using Terraria.ID;

namespace terraguardians.Companions.CaptainStench.Subattacks
{
    public class StenchTripleSlashSubAttack : SubAttackBase
    {
        public override string Name => "Funky Combination";
        public override string Description => "";
        
        CaptainStenchBase.WeaponInfusions Infusion = CaptainStenchBase.WeaponInfusions.None;
        int Damage = 0;
        int CritRate = -1;
        int Duration = 14*6; //Each frame is 100ms, and there's 14 frames, so 1 frame for each 6 frames.
        int HitFrame = 0;
        float FrameDurationPercentage = 1f;
        int UseDirection = 0;
        int LastFrame = 0;
        int ASixthOfDuration = 1;

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
            Duration = 13 * 6;
            FrameDurationPercentage = .2f;
            if (Infusion != CaptainStenchBase.WeaponInfusions.None)
                Damage *= 2;
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Sapphire:
                    {
                        FrameDurationPercentage *= 2.5f;
                        Duration = (int)(Duration * .4f);
                        Damage = (int)(Damage * 0.75f);
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Topaz:
                    {
                        FrameDurationPercentage *= .8f;
                        Duration = (int)(Duration * 1.25f);
                        Damage = (int)(Damage * 1.2f);
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Emerald:
                    {
                        CritRate = 50;
                        Damage = (int)(Damage * .9f);
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Diamond:
                    {
                        Damage += (int)(User.MaxHealth * .02f);
                    }
                    break;
            }
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
            ASixthOfDuration = Duration / 6;
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
                (User as CaptainStenchBase.StenchCompanion).UnsheathBlade();
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
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * 28 * User.Scale, Vector2.UnitX * UseDirection * 17f, ModContent.ProjectileType<Projectiles.SallySpecials.PurpleWave>(), Damage * 2, 3f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Topaz:
                    {
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            for (int y = -1; y <= 2; y += 2)
                                Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * (28 + 12 * y) * User.Scale, Vector2.UnitX * UseDirection * 22f, ModContent.ProjectileType<Projectiles.SallySpecials.TopazShard>(), (int)(Damage * 1.5f), 5f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Ruby:
                    {
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            for (int y = -1; y < 2; y++)
                                Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * ((28 + 66 * y) * User.Scale), Vector2.UnitX * UseDirection * 16f, ModContent.ProjectileType<Projectiles.SallySpecials.BloodSickle>(), (int)(Damage * .5f), 6f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Emerald:
                    {
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * 28 * User.Scale, Vector2.UnitX * UseDirection * 10f, ModContent.ProjectileType<Projectiles.SallySpecials.Tornado>(), (int)(Damage * .8f), 3f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Diamond:
                    {
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * 28 * User.Scale, Vector2.UnitX * UseDirection * 16f, ProjectileID.RainbowRodBullet, (int)(Damage * .8f), 3f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Sapphire:
                    {
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            Projectile.NewProjectile(User.GetSource_FromAI(), User.Bottom - Vector2.UnitY * 28 * User.Scale, Vector2.UnitX * UseDirection * 16f, ModContent.ProjectileType<Projectiles.SallySpecials.SapphireBolt>(), (int)(Damage * .8f), 3f, User.whoAmI);
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Amber:
                    {
                        if (Time == ASixthOfDuration || Time == ASixthOfDuration * 3 || Time == ASixthOfDuration * 5)
                        {
                            Vector2 CasterCenter = User.GetCompanionCenter;
                            Vector2 Direction = Vector2.UnitX * User.direction;
                            float NearestTargetDistance = float.MaxValue;
                            Vector2 NearestTargetCenter = Vector2.Zero;
                            int NearestTargetWidth = 30, NearestTargetHeight = 30;
                            for (int i = 0; i < 200; i++)
                            {
                                if (Main.npc[i].active && User.IsHostileTo(Main.npc[i]))
                                {
                                    Vector2 TargetCenter = Main.npc[i].Center;
                                    float Distance = (CasterCenter - TargetCenter).Length();
                                    if (Distance < NearestTargetDistance)
                                    {
                                        NearestTargetCenter = TargetCenter;
                                        NearestTargetDistance = Distance;
                                        NearestTargetWidth = Main.npc[i].width;
                                        NearestTargetHeight = Main.npc[i].height;
                                    }
                                }
                            }
                            Vector2 SpawnCenter = CasterCenter;
                            if (NearestTargetDistance < float.MaxValue)
                            {
                                SpawnCenter = NearestTargetCenter;
                                if (Main.rand.Next(2) == 0)
                                {
                                    SpawnCenter.X += Main.rand.Next(-NearestTargetWidth, NearestTargetWidth);
                                    SpawnCenter.Y += NearestTargetHeight * (Main.rand.Next(2) == 0 ? 1f : -1f);
                                }
                                else
                                {
                                    SpawnCenter.X += NearestTargetWidth * (Main.rand.Next(2) == 0 ? 1f : -1f);
                                    SpawnCenter.Y += Main.rand.Next(-NearestTargetHeight, NearestTargetHeight);
                                }
                                Direction = (NearestTargetCenter - SpawnCenter).SafeNormalize(Vector2.UnitX * User.direction);
                            }
                            Projectile.NewProjectile(User.GetSource_FromAI(), SpawnCenter, Direction * 16f, ModContent.ProjectileType<Projectiles.SallySpecials.AmberThorns>(), (int)(Damage * .8f), 3f, User.whoAmI, 0, Main.rand.NextFloat() * 0.5f + 0.6f);
                            for (int i = 0; i < 2; i++)
                            {
                                Projectile.NewProjectile(User.GetSource_FromAI(), SpawnCenter, Vector2.UnitX * (1f - Main.rand.NextFloat() * 2f) * 6f + Vector2.UnitY * (1f - Main.rand.NextFloat() * 2f) * 6f, ModContent.ProjectileType<Projectiles.SallySpecials.LargeBee>(), (int)(20 + Damage * .95f), 3f, User.whoAmI);
                            }
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
                case CaptainStenchBase.WeaponInfusions.Diamond:
                    {
                        foreach (Entity Target in Targets)
                        {
                            if (Target is NPC)
                                (Target as NPC).AddBuff(BuffID.Confused, 7 * 60);
                            else if (Target is Player)
                                (Target as Player).AddBuff(BuffID.Confused, 7 * 60);
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