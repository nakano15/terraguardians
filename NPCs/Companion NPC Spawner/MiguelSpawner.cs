using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MiguelSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Miguel);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && Main.dayTime && Main.invasionSize == 0 && !Main.eclipse &&
                spawner.Player.ZoneOverworldHeight && !Main.slimeRain)
            {
                return 1f / 200;
            }
            return 0;
        }
    }
}