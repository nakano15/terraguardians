using Terraria;
using Terraria.ModLoader;
using System;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;

namespace terraguardians.Companions.Wrath
{
    internal class WrathBodySlamAttack : SubAttackBase
    {
        public override string Name => "Body Slam of Doom";
        public override string Description => "Wrath jumps in the air to flatten the target with their weight.";
        public override bool AllowItemUsage => false;
        public override float Cooldown => 38;
        public override SubAttackData GetSubAttackData => new WrathBodySlamAttackData();

        public override void OnBeginUse(Companion User, SubAttackData RawData)
        {
            WrathBodySlamAttackData Data = (WrathBodySlamAttackData)RawData;
            if (User.Target != null)
            {
                Data.SkillTarget = User.Target;
            }
            else
            {
                //Try getting a target
                Data.SkillTarget = GetTargetInAimRange(User, 50);
            }
            Data.Damage = GetDamage(User);
            Data.FallHurt = false;
            Data.BodySlamResist = 0;
        }

        public override void Update(Companion User, SubAttackData RawData)
        {
            WrathBodySlamAttackData Data = (WrathBodySlamAttackData)RawData;
            bool LastMoveLeft = User.MoveLeft, LastMoveRight = User.MoveRight;
            User.MoveLeft = User.MoveRight = User.ControlJump = User.MoveDown = false;
            if (Data.GetTime == 90)
            {
                User.velocity.Y = -15;
            }
            else if (Data.GetTime >= 91)
            {
                User.immuneTime = 3;
                User.immune = true;
                User.immuneNoBlink = true;
                if (Data.BodySlamResist == 0)
                {
                    float Speed = Main.expertMode ? .5f : .3f;
                    if (User.IsBeingControlledBy(MainMod.GetLocalPlayer))
                    {
                        if (LastMoveLeft)
                            User.velocity.X -= Speed;
                        if (LastMoveRight)
                            User.velocity.X += Speed;
                    }
                    else
                    {
                        if (Data.SkillTarget != null)
                        {
                            if (Data.SkillTarget.Center.X + User.velocity.X < User.Center.X)
                            {
                                User.MoveLeft = true;
                                User.velocity.X -= Speed;
                            }
                            else
                            {
                                User.MoveRight = true;
                                User.velocity.X += Speed;
                            }
                        }
                    }                
                }
                if (User.IgnoreCollision || User.velocity.Y != 0)
                {
                    if(Main.expertMode)
                    {
                        User.velocity.Y += 0.5f;
                        if(User.velocity.Y > 6f)
                            User.velocity.Y = 6f;
                    }
                    else
                    {
                        User.velocity.Y += 0.4f;
                        if(User.velocity.Y > 5f)
                            User.velocity.Y = 5f;
                    }
                    User.SetFallStart();
                }
                if (Data.BodySlamResist > 0)
                {
                    if (Data.SkillTarget != null && Data.SkillTarget is Player)
                    {
                        Player p = Data.SkillTarget as Player;
                        p.AddBuff(Terraria.ID.BuffID.Cursed, 3);
                        p.Center = User.Bottom;
                        p.fullRotation = -MathHelper.ToRadians(90) * p.direction;
                        p.fullRotationOrigin.X = p.width * 0.5f;
                        p.fullRotationOrigin.Y = p.height * 0.5f;
                        if (User.velocity.Y == 0)
                        {
                            if (!Data.FallHurt)
                            {
                                Data.FallHurt = true;
                                p.Hurt(Terraria.DataStructures.PlayerDeathReason.ByCustomReason(p.name + " was flattened by " + User.GetName + "."), Data.Damage, User.direction, false, false, -1, false);
                                if (p.whoAmI == MainMod.GetLocalPlayer.whoAmI && !p.dead && PlayerMod.GetPlayerKnockoutState(p) == KnockoutStates.Awake)
                                {
                                    Main.NewText("Press 'Jump' repeatedly to escape.");
                                }
                                switch (Main.rand.Next(3))
                                {
                                    default:
                                        User.SaySomething("*I'VE BEEN LOOKING FOR A GOOD PUNCHING BAG!*");
                                        break;
                                    case 1:
                                        User.SaySomething("*CRACK! CRACK! CRACK! SNAP! SNAP! SNAP!*");
                                        break;
                                    case 2:
                                        User.SaySomething("*PILE DRIVER!*");
                                        break;
                                }
                                const int DustSpawnWidth = 32, DustSpawnHeight = 4;
                                Vector2 DustStartPos = User.Bottom;
                                DustStartPos.X -= DustSpawnWidth / 2;
                                DustStartPos.Y -= DustSpawnHeight / 2;
                                for (int i = 0; i < 10; i++)
                                {
                                    Dust.NewDust(DustStartPos, DustSpawnWidth, DustSpawnHeight, Terraria.ID.DustID.Dirt, 0f, -2f);
                                }
                                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item11, User.Bottom);
                            }
                            p.AddBuff(Terraria.ID.BuffID.Suffocation, 3);
                            if ((p != MainMod.GetLocalPlayer || (p is Companion && !(p as Companion).IsBeingControlledBy(MainMod.GetLocalPlayer))) && Main.rand.Next(10) == 0)
                            {
                                Data.BodySlamResist--;
                            }
                            if (p.controlJump && !p.releaseJump)
                            {
                                Data.BodySlamResist--;
                            }
                            if (p.dead || Data.BodySlamResist <= 0)
                            {
                                p.velocity.Y = -6f;
                                Data.EndUse();
                            }
                        }
                    }
                    else if (Data.SkillTarget != null && Data.SkillTarget is NPC)
                    {
                        NPC n = Data.SkillTarget as NPC;
                        if (n.boss || Terraria.ID.NPCID.Sets.ShouldBeCountedAsBoss[n.type])
                        {
                            NPC.HitInfo info = new NPC.HitInfo()
                            {
                                DamageType = DamageClass.Melee,
                                Crit = true,
                                HitDirection = User.direction, 
                                Knockback = 3f,
                                Damage = Data.Damage
                            };
                            n.StrikeNPC(info);
                            User.immuneTime = 15;
                            User.immune = true;
                            Data.EndUse();
                            if (User.velocity.Y > 0)
                                User.velocity.Y *= -1;
                            return;
                        }
                        n.Center = User.Bottom;
                        n.rotation = -MathHelper.ToRadians(90) * n.direction;
                        if (User.velocity.Y == 0)
                        {
                            if (!Data.FallHurt)
                            {
                                Data.FallHurt = true;
                                NPC.HitInfo info = new NPC.HitInfo()
                                {
                                    DamageType = DamageClass.Melee,
                                    Crit = true,
                                    HitDirection = User.direction, 
                                    Knockback = 3f,
                                    Damage = Data.Damage
                                };
                                n.StrikeNPC(info);
                                switch (Main.rand.Next(3))
                                {
                                    default:
                                        User.SaySomething("*I'VE BEEN LOOKING FOR A GOOD PUNCHING BAG!*");
                                        break;
                                    case 1:
                                        User.SaySomething("*CRACK! CRACK! CRACK! SNAP! SNAP! SNAP!*");
                                        break;
                                    case 2:
                                        User.SaySomething("*PILE DRIVER!*");
                                        break;
                                }
                                const int DustSpawnWidth = 32, DustSpawnHeight = 4;
                                Vector2 DustStartPos = User.Bottom;
                                DustStartPos.X -= DustSpawnWidth / 2;
                                DustStartPos.Y -= DustSpawnHeight / 2;
                                for (int i = 0; i < 10; i++)
                                {
                                    Dust.NewDust(DustStartPos, DustSpawnWidth, DustSpawnHeight, Terraria.ID.DustID.Dirt, 0f, -2f);
                                }
                                Terraria.Audio.SoundEngine.PlaySound(Terraria.ID.SoundID.Item11, User.Bottom);
                            }
                            if (Main.rand.Next(10) == 0)
                            {
                                Data.BodySlamResist --;
                            }
                            if (!n.active || Data.BodySlamResist <= 0)
                            {
                                Data.EndUse();
                                return;
                            }
                            if (Data.GetTime % 15 == 0)
                            {
                                n.StrikeNPC(new NPC.HitInfo(){ Damage = 3, DamageType = DamageClass.Melee });
                            }
                        }
                    }
                    else
                    {
                        if (User.velocity.Y == 0)
                            Data.EndUse();
                        return;
                    }
                    DrawOrderInfo.AddDrawOrderInfo(Data.SkillTarget, User, DrawOrderInfo.DrawOrderMoment.InFrontOfParent);
                }
                else if (!User.IgnoreCollision && User.velocity.Y == 0)
                {
                    Data.EndUse();
                }
                else //Doesn't work well like this...
                {
                    bool AnyPicked = false;
                    for (int i = 0; i < 255; i++)
                    {
                        if (Main.player[i].active && !Main.player[i].dead && Main.player[i] != User && User.IsHostileTo(Main.player[i]) && User.Hitbox.Intersects(Main.player[i].Hitbox))
                        {
                            Data.SkillTarget = Main.player[i];
                            AnyPicked = true;
                            break;
                        }
                        if (i < 200 && Main.npc[i].active && !Main.npc[i].friendly && Main.npc[i].Hitbox.Intersects(User.Hitbox))
                        {
                            Data.SkillTarget = Main.npc[i];
                            AnyPicked = true;
                            break;
                        }
                    }
                    if (AnyPicked)
                        Data.BodySlamResist = 10;
                }
            }
        }

        public override void OnEndUse(Companion User, SubAttackData RawData)
        {
            WrathBodySlamAttackData Data = RawData as WrathBodySlamAttackData;
            if (Data.SkillTarget != null)
            {
                if (Data.SkillTarget is Player)
                {
                    Player p = Data.SkillTarget as Player;
                    p.fullRotation = 0;
                    p.fullRotationOrigin.X = 0;
                    p.fullRotationOrigin.Y = 0;
                }
                else
                {
                    NPC n = Data.SkillTarget as NPC;
                    n.Bottom = User.Bottom;
                    n.rotation = 0;
                }
            }
            User.immuneTime = 15;
            User.immune = true;
        }

        public int GetDamage(Companion companion)
        {
            if (NPC.downedGolemBoss)
            {
                return 150;
            }
            else if (NPC.downedMechBossAny)
            {
                return 90;
            }
            else if (Main.hardMode)
            {
                return 60;
            }
            else if (NPC.downedBoss3)
            {
                return 40;
            }
            return 20;
        }

        public override void UpdateStatus(Companion User, SubAttackData RawData)
        {
            WrathBodySlamAttackData Data = (WrathBodySlamAttackData)RawData;
            if (Data.GetTime > 91)
            {
                if (Data.BodySlamResist == 0)
                {
                    if (User.velocity.Y > 0 && Data.SkillTarget.position.Y > User.Bottom.Y)
                    {
                        User.IgnoreCollision = true;
                    }
                    User.GravityPower = 0;
                }
            }
            User.noKnockback = true;
        }

        public override void UpdateAnimation(Companion User, SubAttackData Data)
        {
            if (Data.GetTime < 90)
            {
                int ActionTime = Data.GetTime;
                byte Frame = 0;
                if (ActionTime >= 80)
                {
                    Frame = 1;
                }
                else if (ActionTime >= 70)
                {
                    Frame = 2;
                }
                else if (ActionTime >= 30)
                {
                    Frame = 3;
                }
                else if (ActionTime >= 20)
                {
                    Frame = 2;
                }
                else if (ActionTime >= 10)
                {
                    Frame = 1;
                }
                User.ArmFramesID[0] = Frame;
                User.ArmFramesID[1] = Frame;
                if ((User.Data as WrathBase.PigGuardianFragmentData).IsCloudForm)
                {
                    User.BodyFrameID = 19;
                }
            }
            else
            {
                if ((Data as WrathBodySlamAttackData).BodySlamResist > 0)
                {
                    User.BodyFrameID = User.ArmFramesID[0] = User.ArmFramesID[1] = (short)((User.Data as WrathBase.PigGuardianFragmentData).IsCloudForm ? 23 : 16);
                }
                else
                {
                    User.BodyFrameID = (short)((User.Data as WrathBase.PigGuardianFragmentData).IsCloudForm ? 19 : 9);
                    User.ArmFramesID[0] = User.ArmFramesID[1] = 9;
                }
            }
        }

        public override void PreDraw(Companion User, SubAttackData RawData, ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            WrathBodySlamAttackData Data = (WrathBodySlamAttackData)RawData;
            if (Data.BodySlamResist > 0)
            {
                Holder.DrawPosition.Y += 6;
            }
        }

        public class WrathBodySlamAttackData : SubAttackData
        {
            public bool FallHurt = false;
            public Entity SkillTarget = null;
            public float BodySlamResist = 0;
            public int Damage = 0;
        }
    }
}
