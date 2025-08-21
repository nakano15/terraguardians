using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MichelleSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Michelle);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!CanSpawnCompanionNpc())
                return 0;
            if (Main.IsFastForwardingTime() || Main.eclipse || !Main.dayTime || Main.time >= 27000)
            {
                return 0;
            }
            if (Main.invasionType > 0 && Main.invasionDelay == 0 && Main.invasionSize > 0)
                return 0;
            if (Main.rand.NextBool(17))
            {
                bool HasPlayerWithDefense = false;
                for (int p = 0; p < 255; p++)
                {
                    if (!(Main.player[p] is Companion) && Main.player[p].active && Main.player[p].statDefense > 0)
                    {
                        HasPlayerWithDefense = true;
                        break;
                    }
                }
                if (HasPlayerWithDefense)
                {
                    return 1;
                }
            }
            return 0;
        }

        public override void AI()
        {
            WorldMod.SpawnCompanionNPC(CompanionDB.Michelle);
            Main.NewText("Michelle has logged in.", 255, 255, 0);
            NPC.active = false;
        }
    }
}