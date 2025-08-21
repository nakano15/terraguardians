using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class WoodenGreathammer : GuardianItemPrefab
	{
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("My arms aren't strong enough to use this, but...");
        }

        public override void SetDefaults()
        {
			Item.damage = 12;
			Item.DamageType = DamageClass.Melee;
			Item.width = 60;
			Item.height = 92;
			Item.useTime = 48;
			Item.useAnimation = 48;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 13;
            Item.crit = 20;
			Item.value = Item.buyPrice(0,0,15);
			Item.rare = ItemRarityID.White;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(30, 84);
        }
    }
}