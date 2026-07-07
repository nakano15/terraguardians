using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class WrathSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Wrath);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.noWorms && !spawner.spawnFriendly && ((!Terraria.Main.dayTime && spawner.Player.ZoneOverworldHeight) || (Terraria.Main.remixWorld && spawner.Player.ZoneUnderworldHeight)) && CanSpawnCompanionNpc() && TargetIsPlayer(spawner.Player) && !Terraria.Main.snowMoon && !Terraria.Main.pumpkinMoon && !Terraria.Main.bloodMoon && !spawner.waterTile && Terraria.Main.tile[spawner.SpawnTileX, spawner.SpawnTileY].WallType == 0)
                return 1f / 64;
            return 0;
        }
    }
}