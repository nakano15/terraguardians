using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.Buffs.GhostFoxHaunts
{
    public class FriendlyHaunt : ModBuff
    {
        public override void SetStaticDefaults()
        {
            //DisplayName.SetDefault("Haunted");
            //Description.SetDefault("This time she just wants to rest on your shoulder.");
            Main.debuff[this.Type] = false;
            Main.persistentBuff[this.Type] = false; //true
            Main.buffNoTimeDisplay[this.Type] = true;
            Main.pvpBuff[this.Type] = Main.buffNoSave[this.Type] = false;
            Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[this.Type] = true;
            Terraria.ID.BuffID.Sets.NurseCannotRemoveDebuff[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (PlayerMod.GetPlayerKnockoutState(player) > KnockoutStates.Awake)
            {
                player.GetModPlayer<PlayerMod>().ChangeReviveStack(1);
            }
        }
    }
}
