using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace terraguardians.Projectiles.SallySpecials
{
    public class BloodSickle : ModProjectile
    {
        protected override bool CloneNewInstances => false;

        bool InsideTile = true;
        public override void SetDefaults()
        {
            Projectile.aiStyle = Terraria.ID.ProjAIStyleID.Typhoon;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 75;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = true;
            Projectile.width = 40;
            Projectile.height = 40;
            Projectile.maxPenetrate = Projectile.penetrate = -1;
            Projectile.scale = 1.8f;
            Main.projFrames[Projectile.type] = 3;
        }

        public override void AI()
        {
            base.AI();
            Projectile.localAI[1] -= 2f;
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter -= 3;
                Projectile.frame++;
                if (Projectile.frame >= 3)
                    Projectile.frame -= 3;
            }
            if (Projectile.localAI[1] < -10f)
            {
				int num637 = 6;
				for (int num638 = 0; num638 < num637; num638++)
				{
					Vector2 spinningpoint13 = Vector2.Normalize(Projectile.velocity) * new Vector2((float)Projectile.width, (float)Projectile.height) / 2f;
					Vector2 spinningpoint29 = spinningpoint13;
					double radians14 = (double)(num638 - (num637 / 2 - 1)) * Math.PI / (double)num637;
					spinningpoint13 = spinningpoint29.RotatedBy(radians14, default(Vector2)) + Projectile.Center;
					Vector2 vector125 = ((float)(Main.rand.NextDouble() * 3.1415927410125732) - (float)Math.PI / 2f).ToRotationVector2() * (float)Main.rand.Next(3, 8);
					int num639 = Dust.NewDust(spinningpoint13 + vector125, 0, 0, DustID.Water_BloodMoon, vector125.X * 2f, vector125.Y * 2f, 100, default(Color), 1.4f);
					Main.dust[num639].noGravity = true;
					Main.dust[num639].noLight = true;
					Dust dust90 = Main.dust[num639];
					Dust dust212 = dust90;
					dust212.velocity /= 4f;
					dust90 = Main.dust[num639];
					dust212 = dust90;
					dust212.velocity -= Projectile.velocity;
				}
				Projectile.alpha -= 5;
				if (Projectile.alpha < 50)
				{
					Projectile.alpha = 50;
				}
				Lighting.AddLight((int)Projectile.Center.X / 16, (int)Projectile.Center.Y / 16, 0.6f, 0.2f, 0.1f);
            }
            if (InsideTile)
            {
                Terraria.DataStructures.Point16 position = Projectile.Center.ToTileCoordinates16();
                Tile tile = Main.tile[position.X, position.Y];
                InsideTile = tile != null && Main.tileSolid[tile.TileType];
            }
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (!InsideTile)
            {
                if (Projectile.velocity.X != oldVelocity.X)
                {
                    Projectile.velocity.X = oldVelocity.X * -1f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y)
                {
                    Projectile.velocity.Y = oldVelocity.Y * -1f;
                }
            }
            else
            {
                Projectile.velocity.X = oldVelocity.X;
                Projectile.velocity.Y = oldVelocity.Y;
            }
            return false;
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Player p = Main.player[Projectile.owner];
            if (p.dead) return;
            int HealedValue = (int)(hit.Damage * .35f);
            p.Heal(HealedValue);
            CreateDrainEffect(target.Center);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            Player p = Main.player[Projectile.owner];
            if (p.dead) return;
            int HealedValue = (int)(info.Damage * .35f);
            p.Heal(HealedValue);
            CreateDrainEffect(target.Center);
        }

        void CreateDrainEffect(Vector2 Center)
        {
            for (int x = 0; x < 5; x++)
            {
                Vector2 center = new Vector2(Center.X, Center.Y);
                center.X += (float)Main.rand.Next(-100, 100) * 0.05f;
                center.Y += (float)Main.rand.Next(-100, 100) * 0.05f;
                //center += velocity;
                int num2 = Dust.NewDust(center, 1, 1, DustID.LifeDrain);
                Dust obj = Main.dust[num2];
                obj.velocity *= 0f;
                Main.dust[num2].scale = (float)Main.rand.Next(70, 85) * 0.01f;
                Main.dust[num2].fadeIn = Projectile.owner + 1;
            }
        }
    }
}