using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
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
                        if (c.KnockoutStates < KnockoutStates.KnockedOutCold)
                        {
                            int Healing = (int)(20 * c.GetHealthScale);
                            c.statLife += Healing;
                            c.HealEffect(Healing, false);
                            if(c.statLife > c.statLifeMax2) c.statLife = c.statLifeMax2;
                        }
                    }
                    break;
                case ItemID.Star:
                case ItemID.SoulCake:
                case ItemID.SugarPlum:
                    foreach(Companion c in PlayerMod.PlayerGetSummonedCompanions(player))
                    {
                        if (c.KnockoutStates < KnockoutStates.KnockedOutCold)
                        {
                            int Healing = 100;
                            c.statMana += Healing;
                            c.ManaEffect(Healing);
                            if(c.statMana > c.statManaMax2) c.statMana = c.statManaMax2;
                        }
                    }
                    break;
            }
            return base.OnPickup(item, player);
        }

        public override void GrabRange(Item item, Player player, ref int grabRange)
        {
            if(!item.beingGrabbed)
            {
                Companion c = PlayerMod.PlayerGetMountedOnCompanion(player);
                if (c != null)
                {
                    if (System.MathF.Abs(item.Center.X - c.Center.X) < (item.width + c.width) * 0.5f + grabRange && 
                        System.MathF.Abs(item.Center.Y - c.Center.Y) < (item.height + c.height) * 0.5f + grabRange)
                    {
                        item.velocity = (player.Center - item.Center);
                        item.velocity.Normalize();
                        item.velocity *= 5;
                    }
                }
            }
        }

        public override bool CanPickup(Item item, Player player)
        {
            return PlayerMod.GetPlayerKnockoutState(player) == KnockoutStates.Awake;
        }
    }
}