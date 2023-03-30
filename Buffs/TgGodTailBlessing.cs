using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class TgGodTailBlessing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(MainMod.TgGodName + "'s Tail");
            Description.SetDefault("You're being protected by " + MainMod.TgGodName + "'s Tail Blessing.");
            Main.debuff[this.Type] = Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            Main.persistentBuff[this.Type] = false;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 2;
            player.statDefense += 4;
            player.thorns += 0.05f;
            player.endurance += 0.02f;
            if (Main.rand.NextBool(5))
                player.shadowDodge = true;
        }
    }
}