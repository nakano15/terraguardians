using System;
using Microsoft.Build.Evaluation;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace terraguardians.Projectiles.SallySpecials
{
    public class AmberThorns : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.aiStyle = Terraria.ID.ProjAIStyleID.BloodThorn;
            Projectile.friendly = true;
            Projectile.hostile = false;
            //Projectile.timeLeft = 75;
            Projectile.ignoreWater = true;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Melee;
            Projectile.width = 32;
            Projectile.alpha = 255;
            Projectile.height = 32;
            Projectile.localNPCHitCooldown = 25;
            Projectile.maxPenetrate = Projectile.penetrate = 4;
            Projectile.scale = 2f;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override bool ShouldUpdatePosition()
        {
            return false;
        }

        public override void AI()
        {
            bool spawn = Projectile.ai[0] < 20f;
            bool despawn = Projectile.ai[0] >= 30f;
            if (spawn || despawn)
            {
                for (int k = 0; k < 2; k++)
                {
                    Dust dust3 = Dust.NewDustPerfect(Projectile.Center + Main.rand.NextVector2Circular(16f, 16f), 138, Projectile.velocity * MathHelper.Lerp(0.2f, 0.5f, Main.rand.NextFloat()));
                    dust3.velocity += Main.rand.NextVector2Circular(0.5f, 0.5f);
                    dust3.velocity *= 0.5f;
                    dust3.scale = 0.8f + Main.rand.NextFloat() * 0.5f;
                }
            }
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D texture = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle frame = texture.Frame(1, 6, 0, Projectile.frame);
            Vector2 origin11 = new Vector2(16f, (float)(frame.Height / 2));
            Color alpha14 = Projectile.GetAlpha(lightColor);
            float scale16 = Projectile.scale;
            float lerpValue4 = Utils.GetLerpValue(35f, 30f, Projectile.ai[0], clamped: true);
            scale16 *= lerpValue4;
            Vector4 vector125 = lightColor.ToVector4();
            Vector4 vector126 = new Color(67, 17, 17).ToVector4();
            vector126 *= vector125;
            SpriteEffects dir = Projectile.spriteDirection == -1 ? (SpriteEffects)1 : (SpriteEffects)0;
            //Main.spriteBatch.Draw(TextureAssets.Extra[98].Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * Projectile.scale * 0.5f, null, Projectile.GetAlpha(new Color(vector126.X, vector126.Y, vector126.Z, vector126.W)) * 1f, Projectile.rotation + (float)Math.PI / 2f, TextureAssets.Extra[98].Value.Size() / 2f, Projectile.scale * 0.9f, dir, 0);
            Main.spriteBatch.Draw(texture, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), frame, alpha14, Projectile.rotation, origin11, scale16, dir, 0);
            return false;
        }
    }
}