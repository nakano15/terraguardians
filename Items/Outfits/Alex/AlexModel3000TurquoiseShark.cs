using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Items.Outfits.Alex
{
    public class AlexModel3000TurquoiseShark : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 36;
            Item.height = 38;
            Item.maxStack = 1;
            Item.value = Item.buyPrice(0, 5);
            Item.rare = Terraria.ID.ItemRarityID.Lime;
        }
    }
}