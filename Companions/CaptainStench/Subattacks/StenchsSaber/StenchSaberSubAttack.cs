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
    public class StenchSaberSubAttack : SubAttackBase
    {
        public override string Name => "Stench's Saber";
        public override string Description => "Stench slashes her opponents with her saber.\nIf her weapon is sheathed, using the skill will make her pull her saber, so it can be used.";
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
            CaptainStenchBase.StenchCompanion stench = (CaptainStenchBase.StenchCompanion)User;
            return stench.HoldingWeapon || stench.SheathedWeapon;
        }

        public override void OnBeginUse(Companion User, SubAttackData Data)
        {
            if (!(User as CaptainStenchBase.StenchCompanion).HoldingWeapon)
            {
                (User as CaptainStenchBase.StenchCompanion).UnsheathBlade();
                Data.EndUse(1.5f);
                return;
            }
            Damage = 0;
            for (int i = 0; i < 10; i++)
            {
                if (User.inventory[i].type > ItemID.None && User.inventory[i].damage > 0 && User.inventory[i].DamageType.CountsAsClass<MeleeDamageClass>())
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
            foreach (SubAttackData d in User.SubAttackList)
                d.ChangeCurrentCooldown(-.5f);
            switch (Infusion)
            {
                case CaptainStenchBase.WeaponInfusions.Topaz:
                    {
                        Duration = (int)(Duration * 1.25f);
                        Damage = (int)(Damage * 1.2f);
                    }
                    break;
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
                case CaptainStenchBase.WeaponInfusions.Diamond:
                    {
                        Damage += (int)(User.MaxHealth * .02f);
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
                (User as CaptainStenchBase.StenchCompanion).UnsheathBlade();
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
                case CaptainStenchBase.WeaponInfusions.Sapphire:
                    {
                        if (Targets.Length == 0) return;
                        const int MpGain = 2;
                        User.statMana += (int)MathF.Min(User.statMana + MpGain, User.statManaMax2);
                        User.ManaEffect(MpGain);
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Amber:
                    {
                        if (Targets.Length > 0)
                        {
                            bool LargeBees = Main.rand.NextBool(2);
                            int MaxBees = (!LargeBees ? Main.rand.Next(2, 4) : Main.rand.Next(5, 7));
                            int ProjID = (!LargeBees ? 181 : 566);
                            for (int i = 0; i < MaxBees; i++)
                            {
                                Projectile.NewProjectile(User.GetSource_FromAI(), User.Center, Vector2.UnitX * User.direction * 6f + Vector2.UnitY * (1f - Main.rand.NextFloat() * 2f) * 6f, ProjID, (int)(20 + Damage * .95f), 3f, User.whoAmI);
                            }
                        }
                    }
                    break;
                case CaptainStenchBase.WeaponInfusions.Diamond:
                    {
                        foreach (Entity Target in Targets)
                        {
                            if (Main.rand.NextFloat() < 0.45f)
                            {
                                if (Target is NPC)
                                    (Target as NPC).AddBuff(BuffID.Confused, 7 * 60);
                                else if (Target is Player)
                                    (Target as Player).AddBuff(BuffID.Confused, 7 * 60);
                            }
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
                for (int i = 0; i < 2; i++)
                    User.ArmFramesID[i] = ArmFrame;
                User.BodyFrameID = ArmFrame;
            }
            else
            {
                ArmFrame = (short)(45 + MathF.Min(FrameDurationPercentage * Data.GetTime, 3));
                User.ArmFramesID[1] = ArmFrame;
            }
        }
    }
}