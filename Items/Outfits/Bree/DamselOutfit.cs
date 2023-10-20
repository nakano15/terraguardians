using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Items.Outfits.Bree
{
    public class DamselOutfit : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 46;
            Item.height = 40;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 5);
            Item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}