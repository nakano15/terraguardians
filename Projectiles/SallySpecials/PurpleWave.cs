using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallySpecials
{
    public class PurpleWave : ModProjectile
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
            Projectile.width = 40;
            Projectile.height = 74;
            Projectile.maxPenetrate = Projectile.penetrate = 10;
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
            target.AddBuff(Terraria.ID.BuffID.ShadowFlame, 15 * 60);
            Projectile.damage = Math.Max(1, Projectile.damage - 30);
        }

        public override void OnHitPlayer(Player target, Player.HurtInfo info)
        {
            target.AddBuff(Terraria.ID.BuffID.ShadowFlame, 15 * 60);
            Projectile.damage = Math.Max(1, Projectile.damage - 30);
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Vector2 Position = Projectile.position - Main.screenPosition;
            Vector2 Origin = new Vector2(40, 16);
            Position.X += Projectile.width * .5f;
            Position.Y += Projectile.height * .5f;
            const float DistanceX = 36f;
            Position.X -= Projectile.direction * (DistanceX * 2 - Projectile.width * .5f);
            Texture2D ProjTexture = TextureAssets.Projectile[Projectile.type].Value;
            Lighting.AddLight(Position, 0.165f, 0, 0.165f);
            for (int i = 0; i < 4; i++)
            {
                Color color = lightColor * (i * .2f);
                Main.spriteBatch.Draw(ProjTexture, Position, null, color, Projectile.rotation, Origin, Projectile.scale, SpriteEffects.None, 0f);
                Position.X += Projectile.direction * DistanceX;
            }
            return base.PreDraw(ref lightColor);
        }
    }
}