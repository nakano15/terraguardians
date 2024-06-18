using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons.SallyStench
{
    public class LaserPistol : GuardianItemPrefab
    {
        public override void SetDefaults()
        {
			Item.autoReuse = true;
			Item.useStyle = 5;
            Item.damage = 8;
            Item.DamageType = DamageClass.Ranged;
            Item.mana = 2;
            Item.useTime = 12;
            Item.useAnimation = 12;
            Item.knockBack = 3f;
			Item.noMelee = true;
			Item.UseSound = SoundID.Item157;
            Item.crit = 4;
            Item.shoot = ModContent.ProjectileType<Projectiles.LaserPistolShot>();
        }
    }
}