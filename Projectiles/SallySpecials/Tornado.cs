using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace terraguardians.Projectiles.SallySpecials
{
    public class Tornado : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = -1;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.timeLeft = 180;
            Projectile.light = 1.1f;
            Projectile.scale = 1.2f;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.width = 50;
            Projectile.height = 50;
            Projectile.maxPenetrate = Projectile.penetrate = -1;
            Main.projFrames[Projectile.type] = 8;
        }

        public override void ModifyHitPlayer(Player target, ref Player.HurtModifiers modifiers)
        {
            modifiers.FinalDamage *= 2;
        }
        
        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            modifiers.SetCrit();
        }

        public override void AI()
        {
            Projectile.frameCounter++;
            if (Projectile.frameCounter >= 3)
            {
                Projectile.frameCounter -= 3;
                Projectile.frame++;
                if (Projectile.frame >= 8)
                    Projectile.frame -= 8;
            }
            int targetnpc = (int)Projectile.ai[0] - 1;
            if (targetnpc != -1 && (!Main.npc[targetnpc].CanBeChasedBy(this) || Projectile.localNPCImmunity[targetnpc] > 0))
            {
                Projectile.ai[0] = 0f;
                targetnpc = -1;
            }
            if (targetnpc == -1)
            {
                bool checkCanHit = false;
                if (Projectile.type == 969)
                {
                    checkCanHit = true;
                }
                NPC nPC = Projectile.FindTargetWithinRange(400f, checkCanHit);
                if (nPC != null)
                {
                    targetnpc = nPC.whoAmI;
                    Projectile.ai[0] = targetnpc + 1;
                    Projectile.netUpdate = true;
                }
            }
            float hvelocity = 8f;
            float amount = 0.1f;
            float limit = 25f;
            if (targetnpc != -1)
            {
                NPC nPC2 = Main.npc[targetnpc];
                float dist = Projectile.Distance(nPC2.Center);
                if (hvelocity > dist)
                {
                    hvelocity = dist;
                }
                Vector2 vector = Projectile.DirectionTo(nPC2.Center);
                if (!vector.HasNaNs() && dist >= limit)
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, vector * hvelocity, amount);
                }
            }
            else
            {
                Vector2 vector2 = Projectile.DirectionTo(Projectile.Center + Projectile.velocity);
                if (!vector2.HasNaNs())
                {
                    Projectile.velocity = Vector2.Lerp(Projectile.velocity, vector2 * hvelocity, amount);
                }
            }
            Projectile.rotation = Projectile.velocity.X * 0.0125f;
			if (Projectile.timeLeft % 3 == 0)
			{
				int num6 = Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Cloud, Projectile.velocity.X, Projectile.velocity.Y, 120, default(Color), 0.5f);
				Main.dust[num6].noGravity = true;
				Main.dust[num6].fadeIn = 0.9f;
				Main.dust[num6].velocity = Main.rand.NextVector2Circular(2f, 2f) + new Vector2(0f, -2f) + Projectile.velocity * 0.75f;
				for (int j = 0; j < 2; j++)
				{
					Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.GreenTorch, Projectile.velocity.X, Projectile.velocity.Y, 60, default(Color), 0.5f);
					dust.noGravity = true;
					dust.fadeIn = 0.7f;
					dust.velocity = Main.rand.NextVector2Circular(2f, 2f) * 0.2f + new Vector2(0f, -0.4f) + Projectile.velocity * 1.5f;
					dust.position -= Projectile.velocity * 3f;
				}
			}
        }
    }
}