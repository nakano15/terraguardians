using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallySpecials
{
    public class Tornado : ModProjectile
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
            
        }
    }
}