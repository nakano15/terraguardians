using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class LeonaCounter : ModBuff
    {

        public override void SetStaticDefaults()
        {
            Main.debuff[this.Type] = Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<MeleeDamageClass>() *= 2;
        }
    }
}
