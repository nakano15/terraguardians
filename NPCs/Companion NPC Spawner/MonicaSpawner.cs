using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MonicaSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Monica);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && NPC.downedSlimeKing)
            {
                return 1f / 150;
            }
            return 0;
        }
    }
}