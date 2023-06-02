using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using terraguardians;

namespace terraguardians.Items.Accessories
{
    public class TwoHandedMastery : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Allows TerraGuardians to wield a copy of their weapon in their hands.");  //The (English) text shown below your weapon's name
		}

        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 24;            //Weapon's texture's width
            Item.height = 24;           //Weapon's texture's height
            Item.value = Item.buyPrice(0, 25);           //The value of the weapon
            Item.rare = Terraria.ID.ItemRarityID.LightPurple;              //The rarity of the weapon, from -1 to 13
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if (player is TerraGuardian)
            {
                TerraGuardian tg = player as TerraGuardian;
                if (tg.HeldItems.Length > 1 && tg.HeldItems[1].ItemAnimation == 0 && MainMod.IsDualWieldableWeapon(tg.HeldItem.type))
                {
                    tg.HeldItems[1].IsActionHand = true;
                    tg.HeldItems[1].SelectedItem = tg.selectedItem;
                }
            }
        }
    }
}
