using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class LiebreSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Liebre);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && !Main.bloodMoon)
            {
                if (Lighting.Brightness(spawnInfo.SpawnTileX, spawnInfo.SpawnTileY) < .15f)
                {
                    switch (Companions.LiebreBase.EncounterTimes)
                    {
                        default:
                            if (!spawnInfo.Player.ZoneDungeon && !spawnInfo.Player.ZoneCorrupt && !spawnInfo.Player.ZoneCrimson)
                            {
                                return 1f / 200;
                            }
                            break;
                        case 1:
                            if (!spawnInfo.Player.ZoneDungeon && (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson))
                            {
                                return 1f / 200;
                            }
                            break;
                        case 2:
                            if (spawnInfo.Player.ZoneDungeon)
                            {
                                return 1f / 200;
                            }
                            break;
                        case 3:
                            if (spawnInfo.PlayerInTown)
                            {
                                return 1f / 200;
                            }
                            break;
                    }
                }
            }
            return 0;
        }
    }
}