using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class BreeSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Bree);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }
    }
}