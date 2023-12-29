using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs.GhostFoxHaunts
{
    public class BeeHaunt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Haunted");
            //Description.SetDefault("A Ghost Guardian haunts you.\nHaunt is lifted if you defeat what killed It.\nYou sees flashes of a giant bee shotting stingers on her until death.");
            Main.debuff[this.Type] = true;
            Main.persistentBuff[this.Type] = true; //true
            Main.buffNoTimeDisplay[this.Type] = true;
            Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            player.buffTime[buffIndex] = 60;
            PlayerMod pm = player.GetModPlayer<PlayerMod>();
            if (PlayerMod.GetPlayerKnockoutState(player) > KnockoutStates.Awake)
            {
                pm.ChangeReviveStack(1);
            }
            pm.GhostFoxHaunt = true;
        }
    }
}
