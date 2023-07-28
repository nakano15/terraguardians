using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Weapons
{
    public class JungleStar : GuardianItemPrefab
	{
        public override void SetStaticDefaults()
        {
            // Tooltip.SetDefault("Poisonous stings on a blunt weapon. Looks like a good idea.");
        }

        public override void SetDefaults()
        {
			Item.damage = 53;
			Item.DamageType = DamageClass.Melee;
			Item.width = 40;
			Item.height = 104;
			Item.useTime = 49;
			Item.useAnimation = 49;
			Item.useStyle = 1;
			Item.knockBack = 6;
            Item.crit = 16;
			Item.value = Item.buyPrice(0,0,85);
			Item.rare = 0;
			Item.UseSound = SoundID.Item1;
            Item.autoReuse = false;
            itemType = ItemType.Heavy;
            ItemOrigin = new Vector2(20, 84);
        }
    }
}