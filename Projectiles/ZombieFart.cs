using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Projectiles
{
    public class ZombieFart : ModProjectile
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Zombie Fart");
        }

        public override void SetDefaults()
        {
            Projectile.width = Projectile.height = 8;
            Projectile.aiStyle = -1;
            Projectile.friendly = false;
            Projectile.hostile = true;
            Projectile.timeLeft = 180;
            Projectile.alpha = 255;
            Projectile.light = 0f;
            Projectile.ignoreWater = false;
            Projectile.tileCollide = false;
        }

        public override void AI()
        {
            Projectile.velocity.Y -= 0.006f;
            if (Projectile.velocity.Y < -0.65f)
                Projectile.velocity.Y = -0.65f;
            Projectile.scale += 0.23f * (Main.expertMode ? 1.5f : 1f);
            Vector2 ProjectileCenter = Projectile.Center;
            float Width = Projectile.width * Projectile.scale * 0.5f;
            float Height = Projectile.height * Projectile.scale * 0.5f;
            for(int i = 0; i < 255; i++)
            {
                if (i == Projectile.owner || !Main.player[i].active || Main.player[i].dead || Main.player[i].ghost) continue;
                Vector2 NosePosition = Main.player[i].position;
                NosePosition.Y += Main.player[i].height * 0.25f;
                NosePosition.X += Main.player[i].width * 0.5f;
                if(NosePosition.X >= ProjectileCenter.X - Width && NosePosition.X < ProjectileCenter.X + Width && 
                    NosePosition.Y >= ProjectileCenter.Y - Height && NosePosition.Y < ProjectileCenter.Y + Height)
                {
                    Main.player[i].AddBuff(Terraria.ID.BuffID.Suffocation, 5);
                    Main.player[i].AddBuff(Terraria.ID.BuffID.Weak, (Main.expertMode ? 75 : 15) * 60);
                    Main.player[i].AddBuff(Terraria.ID.BuffID.Slow, 5);
                    Main.player[i].AddBuff(Terraria.ID.BuffID.Stinky, 60 * 10);
                    if (Main.expertMode)
                        Main.player[i].AddBuff(Terraria.ID.BuffID.Poisoned, 60 * 5);
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            int Width = (int)(Projectile.width * Projectile.scale * 0.5f), 
                Height = (int)(Projectile.height * Projectile.scale * 0.5f);
            for (int g = 0; g < 4; g++)
            {
                Vector2 GorePos = Projectile.Center;
                GorePos.X += Main.rand.Next(-Width, Width + 1);
                GorePos.Y += Main.rand.Next(-Height, Height + 1);
                float VelX = Main.rand.Next(-100, 101),
                    VelY = Main.rand.Next(-100, 101);
                Dust dust = Main.dust[Dust.NewDust(GorePos, Projectile.width, Projectile.height, 4, VelX * 0.02f, VelY * 0.02f, 100, new Color(), 1f)];
                dust.scale = Main.rand.Next(50, 166) * 0.01f;
                dust.noLight = true;
                dust.noGravity = true;
                dust.color = new Color(80, 223, 40, 66);
                //if (Main.rand.Next(5) == 0)Gore.NewGore(new Terraria.DataStructures.EntitySource_SpawnNPC(), GorePos, new Vector2(VelX, VelY) * 0.02f, Main.rand.Next(11, 14), Main.rand.Next(75, 121) * 0.01f);
            }
            return true;
        }
    }
}