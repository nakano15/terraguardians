using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class Fit : ModBuff
    {
        public override void Update(Player player, ref int buffIndex)
        {
            const float DamageBonus = .08f;
            player.GetDamage<GenericDamageClass>() += DamageBonus;
            player.statLifeMax2 += 40;
            player.moveSpeed += .1f;
            player.jumpSpeedBoost += .06f;
        }
    }
}
