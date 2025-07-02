using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class CottonSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Cotton);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.Water && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && !spawnInfo.Player.ZoneCorrupt && spawnInfo.Player.ZoneBeach)
                return 1f / 120;
            return 0;
        }
    }
}