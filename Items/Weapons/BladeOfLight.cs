using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class BladeOfLight : GuardianItemPrefab
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("\'By the light of the heavens, I'll cut you in half!\'");
		}

        public override void SetDefaults()
        {
            Item.damage = 78;
			Item.DamageType = DamageClass.Melee;
			Item.width = 26;
			Item.height = 112;
			Item.useTime = 49;
			Item.useAnimation = 49;
			Item.useStyle = ItemUseStyleID.Swing;
			Item.knockBack = 6.5f;
            Item.crit = 19;
			Item.value = Item.buyPrice(0,0,85);
			Item.rare = ItemRarityID.Pink;
			Item.UseSound = SoundID.Item1;
			Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(12, 96);
        }
    }
}