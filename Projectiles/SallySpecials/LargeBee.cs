using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallySpecials
{
    public class LargeBee : ModProjectile
    {
        public override void SetDefaults()
        {
			Projectile.width = 8;
			Projectile.height = 8;
			Projectile.aiStyle = 36;
			Projectile.friendly = true;
			Projectile.penetrate = 3;
			Projectile.alpha = 255;
			Projectile.timeLeft = 600;
            Projectile.DamageType = DamageClass.Melee;
			Projectile.extraUpdates = 1;
            Main.projFrames[Projectile.type] = 4;
        }
    }
}