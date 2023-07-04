using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians.Items.Accessories
{
    public class PackLeaderNecklace : ModItem
	{
		public override void SetStaticDefaults()
		{
			Tooltip.SetDefault("Said to give titan powers to whoever uses it.\nBut only has effect on TerraGuardians.\nIt's not possible to have multiple guardians when having one using this.\n\"The head shown in the necklace, is it of an existing guardian?\"");  //The (English) text shown below your weapon's name
		}

		public override void SetDefaults()
		{
            Item.accessory = true;
			Item.width = 52;
			Item.height = 52;
            Item.value = Item.sellPrice(0, 12);           //The value of the weapon
			Item.rare = Terraria.ID.ItemRarityID.Orange;              //The rarity of the weapon, from -1 to 13
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            if(!(player is TerraGuardian)) return;
            TerraGuardian tg = player as TerraGuardian;
            tg.TitanCompanion = true;
            if (tg.Owner == null || tg.Owner.GetModPlayer<PlayerMod>().GetTitanGuardianSlot != tg.Index)
            {
                return;
            }
            tg.FinalScale *= 3;
            tg.MaxHealth = (int)(tg.MaxHealth * 1.8f);
            tg.GetDamage<GenericDamageClass>() += tg.GetDamage<GenericDamageClass>().Multiplicative * 0.2f;
            tg.statDefense += (int)(tg.statDefense * 0.2f);
            tg.GetAttackSpeed<MeleeDamageClass>() += tg.GetAttackSpeed<MeleeDamageClass>() * 0.2f;
            tg.moveSpeed -= tg.moveSpeed * 0.1f;
            tg.aggro += 300;
        }
    }
}
