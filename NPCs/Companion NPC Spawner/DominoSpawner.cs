using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class DominoSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Domino);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && !spawner.spawnFriendly)
                return 0.00390625f; //1 / 256
            //if (Terraria.Main.dayTime && !Terraria.Main.eclipse && spawnInfo.PlayerInTown && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && PlayerMod.PlayerGetTerraGuardianCompanionsMet(spawnInfo.Player) > 0)
            //    return 1f / 10;
            return 0;
        }
    }
}