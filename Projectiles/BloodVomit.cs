using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Projectiles
{
    public class BloodVomit : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Blood Vomit");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 12;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255;
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = true;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            Projectile.Kill();
            return true;
        }

        public override void AI()
        {
            Projectile.velocity.Y += 0.05f;
            if (Projectile.wet)
                Projectile.Kill();
        }

        public override bool PreDraw(ref Color lightColor)
        {
            for (int bd = 0; bd < 4; bd++)
            {
                Vector2 DustSpawnPos = Projectile.position;
                DustSpawnPos.X += Main.rand.Next(Projectile.width);
                DustSpawnPos.Y += Main.rand.Next(Projectile.height);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f);
            }
            return base.PreDraw(ref lightColor);
        }

        public override void Kill(int timeLeft)
        {
            for (int bd = 0; bd < 8; bd++)
            {
                Vector2 DustSpawnPos = Projectile.position;
                DustSpawnPos.X += Main.rand.Next(Projectile.width);
                DustSpawnPos.Y += Main.rand.Next(Projectile.height);
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, 5, Projectile.velocity.X * 1.2f, Projectile.velocity.Y * 1.2f);
            }
        }

        public override bool CanHitPvp(Player target)
        {
            return target.whoAmI != Projectile.owner && Companions.Zack.ZackPreRecruitZombieBossBehavior.BloodVomitCanHit(target);
        }

        public override bool CanHitPlayer(Player target)
        {
            return target.whoAmI != Projectile.owner && Companions.Zack.ZackPreRecruitZombieBossBehavior.BloodVomitCanHit(target);
        }
    }
}