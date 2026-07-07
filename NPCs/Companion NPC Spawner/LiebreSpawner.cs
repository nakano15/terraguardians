using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class LiebreSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Liebre);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && !Main.bloodMoon)
            {
                if (Lighting.Brightness(spawner.SpawnTileX, spawner.SpawnTileY) < .15f)
                {
                    switch (Companions.LiebreBase.EncounterTimes)
                    {
                        default:
                            if (!spawner.Player.ZoneDungeon && !spawner.Player.ZoneCorrupt && !spawner.Player.ZoneCrimson)
                            {
                                return 1f / 200;
                            }
                            break;
                        case 1:
                            if (!spawner.Player.ZoneDungeon && (spawner.Player.ZoneCorrupt || spawner.Player.ZoneCrimson))
                            {
                                return 1f / 200;
                            }
                            break;
                        case 2:
                            if (spawner.Player.ZoneDungeon)
                            {
                                return 1f / 200;
                            }
                            break;
                        case 3:
                            if (spawner.spawnFriendly)
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