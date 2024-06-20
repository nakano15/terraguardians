using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Projectiles
{
    public class LaserPistolShot : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
			Projectile.width = 4;
			Projectile.height = 4;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.light = 0.75f;
			//Projectile.alpha = 255;
			Projectile.extraUpdates = 2;
			Projectile.scale = 1.4f;
			Projectile.timeLeft = 600;
			Projectile.DamageType = DamageClass.Ranged;
        }

        public override void AI()
        {
            Projectile.ai[0] = 0;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= 1.5f;
            modifiers.ArmorPenetration += 80;
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.FinalDamage *= 1.5f;
            modifiers.ArmorPenetration += 80;
        }
    }
}