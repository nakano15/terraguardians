using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace terraguardians.Companions.Wrath.SubAttacks
{
    internal class WrathBellyTackleAttack : SubAttackBase
    {
        public override string Name => "Tackle";
        public override string Description => "Wrath launches itself forward, slaming foes in the direction with its belly.";
        public override bool AllowItemUsage => false;
        public override float Cooldown => 15;
        public override SubAttackData GetSubAttackData => new WrathBellyTackleAttackData();

        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            if (User.TargettingSomething && User.HasBeenMet)
            {
                Vector2 Distances = User.Center - User.Target.Center;
                if (Main.rand.NextFloat() < 0.6f && Math.Abs(Distances.X) < (User.width + User.Target.width) * .5f + 60 && Math.Abs(Distances.Y) < User.Target.height * .5f + 10&& User.IsFacingTarget(User.Target))
                {
                    return true;
                }
            }
            return base.AutoUseCondition(User, Data);
        }

        public override void OnBeginUse(Companion User, SubAttackData RawData)
        {
            WrathBellyTackleAttackData Data = RawData as WrathBellyTackleAttackData;
            Data.BouncedBack = false;
            Data.Damage = GetDamage(User);
        }

        public override void Update(Companion User, SubAttackData RawData)
        {
            WrathBellyTackleAttackData Data = (WrathBellyTackleAttackData)RawData;
            User.immune = true;
            User.immuneTime = 30;
            User.immuneNoBlink = true;
            User.MoveLeft = User.MoveRight = User.ControlJump = User.MoveDown = false;
            if(Data.GetTime == 0)
            {
                User.velocity.Y = -7f;
            }
            if (!Data.BouncedBack)
            {
                User.velocity.X = 8f * User.direction;
                Rectangle rect = User.Hitbox;
                bool Bounce = false;
                for (int i = 0; i < 255; i++)
                {
                    if(Main.player[i].active && Main.player[i] != User && !Main.player[i].immune && !Main.player[i].dead && User.IsHostileTo(Main.player[i]) && PlayerMod.GetPlayerKnockoutState(Main.player[i]) == KnockoutStates.Awake && rect.Intersects(Main.player[i].Hitbox))
                    {
                        Bounce = true;
                        Main.player[i].Hurt(PlayerDeathReason.ByCustomReason(Main.player[i].name + " bounced back to " + User.GetName + "'s tackle."), Data.Damage, User.direction);
                    }
                    if (i < 200 && Main.npc[i].active && User.IsHostileTo(Main.npc[i]) && rect.Intersects(Main.npc[i].Hitbox))
                    {
                        Bounce = true;
                        NPC.HitInfo hit = new NPC.HitInfo()
                        {
                            Damage = Data.Damage,
                            DamageType = DamageClass.Melee
                        };
                        Main.npc[i].StrikeNPC(hit);
                    }
                }
                if (Bounce)
                {
                    User.velocity.X *= -1;
                    Data.BouncedBack = true;
                }
            }
            if (Data.GetTime >= 40)
            {
                Data.EndUse();
            }
        }

        public override void UpdateAnimation(Companion User, SubAttackData RawData)
        {
            WrathBellyTackleAttackData Data = (WrathBellyTackleAttackData)RawData;
            bool CloudForm = (User.Data as WrathBase.PigGuardianFragmentData).IsCloudForm;
            if (!Data.BouncedBack)
            {
                User.ArmFramesID[0] = User.ArmFramesID[1] = 21;
                User.BodyFrameID = (short)(CloudForm ? 19 : 9);
            }
            else
            {
                User.ArmFramesID[0] = User.ArmFramesID[1] = 9;
                User.BodyFrameID = (short)(CloudForm ? 21 : 9);
            }
        }

        public int GetDamage(Companion companion)
        {
            if (NPC.downedGolemBoss)
            {
                return 85;
            }
            else if (NPC.downedMechBossAny)
            {
                return 50;
            }
            else if (Main.hardMode)
            {
                return 40;
            }
            else if (NPC.downedBoss3)
            {
                return 20;
            }
            return 15;
        }

        public class WrathBellyTackleAttackData : SubAttackData
        {
            public bool BouncedBack = false;
            public int Damage = 0;
        }
    }
}