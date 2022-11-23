using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
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

        public override bool OnPickup(Item item, Player player)
        {
            switch(item.type)
            {
                case ItemID.Heart:
                case ItemID.CandyApple:
                case ItemID.CandyCane:
                    foreach(Companion c in PlayerMod.PlayerGetSummonedCompanions(player))
                    {
                        int Healing = (int)(20 * c.GetHealthScale);
                        c.statLife += Healing;
                        c.HealEffect(Healing, false);
                        if(c.statLife > c.statLifeMax2) c.statLife = c.statLifeMax2;
                    }
                    break;
                case ItemID.Star:
                case ItemID.SoulCake:
                case ItemID.SugarPlum:
                    foreach(Companion c in PlayerMod.PlayerGetSummonedCompanions(player))
                    {
                        int Healing = 100;
                        c.statMana += Healing;
                        c.ManaEffect(Healing);
                        if(c.statMana > c.statManaMax2) c.statMana = c.statManaMax2;
                    }
                    break;
            }
            return base.OnPickup(item, player);
        }
    }
}