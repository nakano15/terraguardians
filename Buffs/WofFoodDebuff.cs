using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class WofFoodDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wof Food");
            Description.SetDefault("");
            Main.debuff[this.Type] = Main.buffNoSave[this.Type] = true;
            Main.pvpBuff[this.Type] = false;
            Main.persistentBuff[this.Type] = false;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.immuneAlpha = 255;
            player.immuneTime = 5;
            player.immuneNoBlink = true;
        }
    }
}