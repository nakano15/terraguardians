using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class CottonSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Cotton);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && TargetIsPlayer(spawner.Player) && !spawner.Player.ZoneCorrupt && spawner.Player.ZoneBeach)
                return 1f / 120;
            return 0;
        }
    }
}