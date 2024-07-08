using System;
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
            Projectile.width = 32;
            Projectile.alpha = 255;
            Projectile.height = 32;
            Projectile.localNPCHitCooldown = 25;
            Projectile.maxPenetrate = Projectile.penetrate = 4;
            //Projectile.scale = 2f;
            Projectile.usesLocalNPCImmunity = true;
            Main.projFrames[Projectile.type] = 6;
        }

        public override bool PreDraw(ref Color lightColor)
        {
            Texture2D value198 = TextureAssets.Projectile[Projectile.type].Value;
            Rectangle value125 = value198.Frame(1, 6, 0, Projectile.frame);
            Vector2 origin11 = new Vector2(16f, (float)(value125.Height / 2));
            Color alpha14 = Projectile.GetAlpha(lightColor);
            Vector2 scale16 = new Vector2(Projectile.scale);
            float lerpValue4 = Utils.GetLerpValue(35f, 30f, Projectile.ai[0], clamped: true);
            scale16.Y *= lerpValue4;
            Vector4 vector125 = lightColor.ToVector4();
            //Vector4 vector126 = new Color(67, 17, 17).ToVector4();
            //vector126 *= vector125;
            SpriteEffects dir = Projectile.spriteDirection == -1 ? (SpriteEffects)1 : (SpriteEffects)0;
            //Main.spriteBatch.Draw(TextureAssets.Extra[98].Value, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY) - Projectile.velocity * Projectile.scale * 0.5f, null, Projectile.GetAlpha(new Color(vector126.X, vector126.Y, vector126.Z, vector126.W)) * 1f, Projectile.rotation + (float)Math.PI / 2f, TextureAssets.Extra[98].Value.Size() / 2f, Projectile.scale * 0.9f, dir, 0);
            Main.spriteBatch.Draw(value198, Projectile.Center - Main.screenPosition + new Vector2(0f, Projectile.gfxOffY), value125, alpha14, Projectile.rotation, origin11, scale16, dir, 0);
            return false;
        }
    }
}