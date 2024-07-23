using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class Rocked : ModBuff
    { 
       public override void SetStaticDefaults()
        {
            Main.debuff[Type] = true;
            Main.pvpBuff[Type] = true;
            Main.buffNoSave[Type] = false;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.statDefense -= (int)(player.statDefense * .65f);
        }

        internal static void ApplyRockedDebuff(ref NPC.HitModifiers mod)
        {
            mod.Defense *= .65f;
        }
    }
}
