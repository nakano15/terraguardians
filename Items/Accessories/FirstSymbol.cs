using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Items.Accessories
{
    public class FirstSymbol : ModItem
    {
        public override void SetDefaults()
        {
            Item.accessory = true;
            Item.width = 22;            //Weapon's texture's width
            Item.height = 24;           //Weapon's texture's height
            Item.value = Item.sellPrice(0, 15);           //The value of the weapon
            Item.rare = 1;              //The rarity of the weapon, from -1 to 13
        }

        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("Increases following Terra Guardians status based on Summon Damage.");
        }

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<PlayerMod>().HasFirstSimbol = true;
        }
    }
}
