using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class ChlorophyteCrescentAxe : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Also used to take care of Terrarians who are AFK.");
		}

        public override void SetDefaults()
        {
            Item.damage = 117;
			Item.DamageType = DamageClass.Melee;
			Item.width = 56;
			Item.height = 128;
			Item.useTime = 53;
			Item.useAnimation = 53;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6.5f;
            Item.crit = 22;
			Item.value = Item.buyPrice(0,0,85);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(28, 116);
        }
    }
}