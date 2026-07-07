using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class LunaSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Luna);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            
            if (Terraria.Main.dayTime && !Terraria.Main.eclipse && spawner.spawnFriendly && CanSpawnCompanionNpc() && TargetIsPlayer(spawner.Player) && PlayerMod.PlayerGetTerraGuardianCompanionsMet(spawner.Player) > 0)
                return 1f / 10;
            return 0;
        }
    }
}