using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class TwoHandedSword : GuardianItemPrefab
	{
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Even with both my hands, I can't wield this.");
        }

        public override void SetDefaults()
        {
			Item.damage = 17;
			Item.DamageType = DamageClass.Melee;
			Item.width = 22;
			Item.height = 96;
			Item.useTime = 42;
			Item.useAnimation = 42;
			Item.useStyle = 1;
			Item.knockBack = 6;
            Item.crit = 7;
			Item.value = Item.buyPrice(0,0,15);
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(10, 88);
        }
    }
}