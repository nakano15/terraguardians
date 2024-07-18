using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallySpecials
{
    public class SapphireBolt : ModProjectile
    {
        public override void AutoStaticDefaults()
        {
            base.AutoStaticDefaults();
            TextureAssets.Projectile[Projectile.type] = TextureAssets.Projectile[435];
            Main.projFrames[Projectile.type] = 4;
        }

        public override void SetDefaults()
        {
            Projectile.aiStyle = 1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 90;
            Projectile.light = 1.1f;
            Projectile.scale = 2.05f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.width = 10;
            Projectile.height = 10;
            Projectile.maxPenetrate = Projectile.penetrate = 1;
        }

        public override void AI()
        {
            Projectile.ai[0] = 0;
            if (Projectile.velocity.X > 0)
            {
                Projectile.rotation = 0;//MathF.PI * .5f;
                Projectile.direction = 1;
            }
            else
            {
                Projectile.rotation = -MathF.PI;// * .5f;
                Projectile.direction = -1;
            }
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 4)
            {
                Projectile.frameCounter -= 4;
                Projectile.frame++;
                if (Projectile.frame >= 4)
                {
                    Projectile.frame -= 4;
                }
            }
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_None(), Projectile.Center, Vector2.Zero, ProjectileID.Electrosphere, Projectile.originalDamage, Projectile.knockBack, Projectile.owner);
        }
    }
}