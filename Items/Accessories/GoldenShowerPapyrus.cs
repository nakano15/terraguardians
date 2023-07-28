using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Accessories
{
    public class GoldenShowerPapyrus : ModItem
	{
		public override void SetStaticDefaults()
		{
			// Tooltip.SetDefault("Teaches a stance for increasing efficience of the Golden Shower book.\nIncreases Golden Shower damage by 20%.");
		}

		public override void SetDefaults()
		{
            Item.accessory = true;
			Item.width = 48;            //Weapon's texture's width
			Item.height = 76;           //Weapon's texture's height
			Item.value = Item.buyPrice(0,0,15);           //The value of the weapon
			Item.rare = 0;              //The rarity of the weapon, from -1 to 13
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player.HeldItem.type == ItemID.GoldenShower)
                player.GetModPlayer<PlayerMod>().GoldenShowerStance = true;
        }

        /*public override void ItemStatusScript(TerraGuardian g)
        {
            g.AddFlag(GuardianFlags.GoldenShowerStance);
        }*/
	}
}
