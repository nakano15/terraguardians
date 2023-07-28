using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class DaylightBane : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Legends says that you can cut the sun in half with this weapon.");
		}

        public override void SetDefaults()
        {
            Item.damage = 38;
			Item.DamageType = DamageClass.Melee;
			Item.width = 30;
			Item.height = 96;
			Item.useTime = 47;
			Item.useAnimation = 47;
			Item.useStyle = 1;
			Item.knockBack = 6;
            Item.crit = 14;
			Item.value = Item.buyPrice(0,0,85);
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(4, 86);
        }
    }
}