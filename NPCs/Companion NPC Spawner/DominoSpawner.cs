using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class DominoSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Domino);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && !spawnInfo.PlayerInTown)
                return 0.00390625f; //1 / 256
            //if (Terraria.Main.dayTime && !Terraria.Main.eclipse && spawnInfo.PlayerInTown && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && PlayerMod.PlayerGetTerraGuardianCompanionsMet(spawnInfo.Player) > 0)
            //    return 1f / 10;
            return 0;
        }
    }
}