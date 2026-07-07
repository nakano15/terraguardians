using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class LeopoldSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Leopold);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && Main.dayTime && !Main.eclipse && Main.invasionSize <= 0)
                return 0.03125f;
            return 0;
        }
    }
}