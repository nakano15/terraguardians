using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class IchSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Ich);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && Main.dayTime && Main.time > 27000 && Main.time < 48600 && NPC.downedBoss1)
            {
                return 1f / 400;
            }
            return 0;
        }
    }
}