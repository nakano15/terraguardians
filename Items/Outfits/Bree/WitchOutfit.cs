using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Items.Outfits.Bree
{
    public class WitchOutfit : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 26;
            Item.height = 28;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 5);
            Item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}