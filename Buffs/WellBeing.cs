using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class WellBeing : ModBuff
    {
        const float DamageIncrease = 0.04f;
        const int DefenseIncrease = 6;
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Well Being");
            // Description.SetDefault("Bonus to many status.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() += DamageIncrease;
            player.statDefense += DefenseIncrease;
            player.lifeRegen++;
            player.manaRegen++;
            player.manaCost -= 0.08f;
        }
    }
}
