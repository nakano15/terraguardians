using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallySpecials
{
    public class TopazShard : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 75;
            Projectile.light = 1.1f;
            Projectile.scale = 2.05f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 7;
            Projectile.height = 7;
            Projectile.maxPenetrate = Projectile.penetrate = -1;
        }

        public override void AI()
        {
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation = MathF.PI * .5f;
                Projectile.direction = 1;
            }
            else
            {
                Projectile.rotation = -MathF.PI * .5f;
                Projectile.direction = -1;
            }
            //Projectile.position += Projectile.velocity;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Rocked>(), 5 * 60);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(ModContent.BuffType<Buffs.Rocked>(), 5 * 60);
        }
    }
}