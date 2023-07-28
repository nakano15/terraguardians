using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class Flamberge : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Blade forged with inspirations from the waves of a flame.");
		}

        public override void SetDefaults()
        {
            Item.damage = 61;
			Item.DamageType = DamageClass.Melee;
			Item.width = 38;
			Item.height = 112;
			Item.useTime = 40;
			Item.useAnimation = 40;
			Item.useStyle = 1;
			Item.knockBack = 6;
            Item.crit = 12;
			Item.value = Item.buyPrice(0,1,20);
            Item.rare = 0;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(18, 100);
        }
    }
}