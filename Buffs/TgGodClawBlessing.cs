using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class TgGodClawBlessing : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault(MainMod.TgGodName + "'s Paw");
            Description.SetDefault("You've been blessed by " + MainMod.TgGodName + "'s Paw.");
            Main.debuff[this.Type] = Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            Main.persistentBuff[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 1;
            player.statDefense += 1;
            player.moveSpeed += 0.05f;
        }
    }
}