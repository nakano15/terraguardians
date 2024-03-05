using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Buffs
{
    public class WofFoodDebuff : ModBuff
    {
        public override void SetStaticDefaults()
        {
            // DisplayName.SetDefault("Wof Food");
            // Description.SetDefault("");
            Main.debuff[this.Type] = Main.buffNoSave[this.Type] = true;
            Main.pvpBuff[this.Type] = false;
            Main.persistentBuff[this.Type] = false;
            Main.buffNoTimeDisplay[this.Type] = true;
        }

        public override void Update(Player player, ref int buffIndex)
        {
            if (Main.wofNPCIndex == -1)
            {
                player.DelBuff(buffIndex);
                player.immuneNoBlink = false;
            }
            else
            {
                player.immuneAlpha = 255;
                player.immuneTime = 5;
                player.immuneNoBlink = true;
                player.aggro = -10000000;
                Vector2 Position = Main.npc[Main.wofNPCIndex].Center;
                if (Main.npc[Main.wofNPCIndex].position.X < 608 || Main.npc[Main.wofNPCIndex].position.X > (Main.maxTilesX - 38) * 16)
                {
                    PlayerMod.ForceKillPlayer(player, " couldn't be saved from Wall of Flesh in time...");
                }
            }
        }
    }
}