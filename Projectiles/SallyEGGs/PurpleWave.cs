using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallyEGGs
{
    public class PurpleWave : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 30;
            Projectile.alpha = 255;
            Projectile.light = 1.1f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
            Projectile.width = 40;
            Projectile.height = 74;
            Projectile.maxPenetrate = /*Projectile.penetrate =*/ 10;
        }

        public override void AI()
        {
            if (Projectile.velocity.X > 0)
                Projectile.direction = 1;
            else
                Projectile.direction = -1;
            Projectile.position += Projectile.velocity;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            target.AddBuff(Terraria.ID.BuffID.ShadowFlame, 15 * 60);
            Projectile.damage -= 30;
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(Terraria.ID.BuffID.ShadowFlame, 15 * 60);
            Projectile.damage -= 30;
        }
    }
}