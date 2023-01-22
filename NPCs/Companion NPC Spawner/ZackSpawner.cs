using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class ZackSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Zacks);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }
    }
}