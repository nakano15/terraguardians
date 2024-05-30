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
    public class ReflectorSubAttack : SubAttackBase
    {
        public override string Name => "Reflector";
        public override string Description => "Stench projects a shield that makes projectiles bounce back towards the direction it came from at 2x the power.\nThe shield can only take 7 hits of damage before being shut down.";
        public override bool AutoUseCondition(Companion User, SubAttackData Data)
        {
            return false;
        }
        public override float Cooldown => 30f;
        public override int ManaCost => 150;
        public byte ReflectorStacks = 0;
        const float ArmingAnimationTotalTime = .6f, ArmingFrameDuration = 1f / (ArmingAnimationTotalTime / 7f);
        Asset<Texture2D> Reflector;

        public override void Load()
        {
            Reflector = ModContent.Request<Texture2D>("terraguardians/Companions/CaptainStench/Subattacks/Reflector/Reflector");
        }

        public override void Unload()
        {
            Reflector = null;
        }

        public void ResetReflectorStacks()
        {
            ReflectorStacks = 7;
        }

        public override bool CanUse(Companion User, SubAttackData Data)
        {
            return (User as CaptainStenchBase.StenchCompanion).HasPhantomDevice;
        }

        public override void Update(Companion User, SubAttackData Data)
        {
            if (Data.GetTime == 0)
            {
                ResetReflectorStacks();
            }
            if (Data.GetTimeSecs >= 7f)
            {
                Data.EndUse();
            }
        }

        public override bool ImmuneTo(Companion User, SubAttackData Data, PlayerDeathReason damageSource, int cooldownCounter, bool dodgeable)
        {
            if (ReflectorStacks > 0 && damageSource.SourceProjectileType > 0)
            {
                Projectile proj = Main.projectile[damageSource.SourceProjectileLocalIndex];
                if (proj.velocity.Length() > 0)
                {
                    proj.velocity *= -1f;
                    proj.hostile = false;
                    proj.friendly = true;
                    proj.originalDamage *= 2;
                    ReflectorStacks--;
                    if (ReflectorStacks == 0)
                        Data.EndUse();
                    return true;
                }
            }
            return base.ImmuneTo(User, Data, damageSource, cooldownCounter, dodgeable);
        }

        public override void Draw(Companion User, SubAttackData Data, bool DrawingFront, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (DrawingFront)
            {
                Texture2D ReflectorTexture = Reflector.Value;
                Rectangle Frame = new Rectangle(0, 0, 41, 38);
                if (Data.GetTimeSecs < ArmingAnimationTotalTime)
                {
                    Frame.X = Frame.Width * (int)MathF.Min(7, Data.GetTimeSecs * ArmingFrameDuration);
                }
                else
                {
                    if (ReflectorStacks == 7)
                    {
                        Frame.X = Frame.Width * 7;
                    }
                    else
                    {
                        Frame.X = Frame.Width * (6 - ReflectorStacks);
                        Frame.Y += Frame.Height;
                    }
                }
                Vector2 Center = Holder.DrawPosition + new Vector2(User.SpriteWidth * .5f, User.SpriteHeight - 20f * User.Scale);
                DrawData dd = new DrawData(ReflectorTexture, Center, Frame, Color.White * .5f, 0f, new Vector2(20f, 19f), User.Scale, 0);
                DrawDatas.Add(dd);
            }
        }
    }
}