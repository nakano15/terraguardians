using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class LunaSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Luna);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            
            if (Terraria.Main.dayTime && !Terraria.Main.eclipse && spawnInfo.PlayerInTown && !MainMod.HasCompanionInWorld(ToSpawnID) && !WorldMod.HasMetCompanion(ToSpawnID) && TargetIsPlayer(spawnInfo.Player) && PlayerMod.PlayerGetTerraGuardianCompanionsMet(spawnInfo.Player) > 0)
                return 1f / 10;
            return 0;
        }
    }
}