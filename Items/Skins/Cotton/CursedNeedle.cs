using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Skins.Cotton;

public class CursedNeedle : ModItem
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 20;
        Item.maxStack = 1;
        Item.value = Item.buyPrice(0, 5);
        Item.rare = ItemRarityID.LightPurple;
    }

    public override void AddRecipes()
    {
        CreateRecipe().
        AddRecipeGroup(RecipeGroupID.IronBar, 1).
        AddTile(TileID.DemonAltar).
        Register();
    }
}