using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class ItemMod : GlobalItem
    {
        public override void UseItemHitbox(Item item, Player player, ref Rectangle hitbox, ref bool noHitbox)
        {
            if(player is Companion)
                ((Companion)player).UseItemHitbox(item, ref hitbox, ref noHitbox);
        }

        public override void HoldStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if(player is Companion)
                ((Companion)player).HoldStyle(item, heldItemFrame);
        }

        public override void UseStyle(Item item, Player player, Rectangle heldItemFrame)
        {
            if(player is Companion)
                ((Companion)player).UseStyle(item, heldItemFrame);
        }
    }
}