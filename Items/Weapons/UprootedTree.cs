using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class UprootedTree : GuardianItemPrefab
	{
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("How can a TerraGuardian wield this?!");
        }

        public override void SetDefaults()
        {
			Item.damage = 120;
			Item.DamageType = DamageClass.Melee;
			Item.width = 92;
			Item.height = 304;
			Item.useTime = 60;
			Item.useAnimation = 60;
			Item.useStyle = 1;
			Item.knockBack = 8;
            Item.value = Item.buyPrice(0, 0, 15);
            Item.crit = 23;
			Item.rare = 1;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(56, 296);
        }
    }
}