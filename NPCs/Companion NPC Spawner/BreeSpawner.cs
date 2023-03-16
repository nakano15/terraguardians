using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class BreeSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Bree);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && Main.dayTime && Main.time > 27000 && Main.time < 48600)
            {
                return (float)(Main.time - 27000) * (1f / 432000);
            }
            return 0;
        }
    }
}