using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs
{
    public class Love : ModBuff
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Infatuated");
            Description.SetDefault("Damage you cause is reduced.");
            Main.debuff[this.Type] = true;
            Main.pvpBuff[this.Type] = true;
            Main.buffNoSave[this.Type] = false;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.GetDamage<GenericDamageClass>() -= .25f;
        }

        public override void Update(NPC npc, ref int buffIndex)
        {
            npc.damage -= (int)(npc.damage * (Main.expertMode ? 0.05f : 0.25f));
        }
    }
}
