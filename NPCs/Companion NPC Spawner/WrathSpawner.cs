using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class WrathSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Wrath);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.PlayerSafe && !spawnInfo.PlayerInTown && ((!Terraria.Main.dayTime && spawnInfo.Player.ZoneOverworldHeight) || (Terraria.Main.remixWorld && spawnInfo.Player.ZoneUnderworldHeight)) && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && !Terraria.Main.snowMoon && !Terraria.Main.pumpkinMoon && !Terraria.Main.bloodMoon && !spawnInfo.Water && Terraria.Main.tile[spawnInfo.SpawnTileX, spawnInfo.SpawnTileY].WallType == 0)
                return 1f / 64;
            return 0;
        }
    }
}