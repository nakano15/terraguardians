using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MiguelSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Miguel);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            /*if (!spawnInfo.Water && CanSpawnCompanionNpc() && Main.dayTime && Main.invasionSize == 0 && !Main.eclipse &&
                spawnInfo.Player.ZoneOverworldHeight && !Main.slimeRain)
            {
                return 1f / 200;
            }*/
            return 0;
        }
    }
}