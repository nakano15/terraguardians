using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class Hug : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Hug");
            Description.SetDefault("Increased health regeneration.\nGreat damage reduction.");
            Main.debuff[Type] = false;
            Main.pvpBuff[Type] = false;
            Main.buffNoSave[Type] = true;
            Main.buffNoTimeDisplay[Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.lifeRegen += 3;
            player.endurance += 0.15f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.lifeRegen += 3;
        }
    }
}
