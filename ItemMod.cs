using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class ItemMod : GlobalItem
    {
        public override void ModifyItemScale(Item item, Player player, ref float scale)
        {
            if (player is Companion)
            {
                scale *= ((Companion)player).Scale;
            }
        }
    }
}