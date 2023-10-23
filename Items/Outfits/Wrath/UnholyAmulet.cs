using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Items.Outfits.Wrath
{
    public class UnholyAmulet : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 34;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 5);
            Item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}