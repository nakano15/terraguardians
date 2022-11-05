using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class MeatMasher : GuardianItemPrefab
	{
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Good at flattening jobs.");
        }

        public override void SetDefaults()
        {
			Item.damage = 46;
			Item.DamageType = DamageClass.Melee;
			Item.width = 48;
			Item.height = 94;
			Item.useTime = 52;
			Item.useAnimation = 52;
			Item.useStyle = 1;
			Item.knockBack = 6;
            Item.crit = 22;
			Item.value = Item.buyPrice(0,0,85);
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(24, 84);
        }
    }
}