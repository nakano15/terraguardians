using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class VladimirSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Vladimir);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            //if (Terraria.Main.dayTime && !Terraria.Main.eclipse && !MainMod.HasCompanionInWorld(ToSpawnID) && !WorldMod.HasMetCompanion(ToSpawnID) && TargetIsPlayer(spawnInfo.Player))
            //    return 1f / 50;
            return 0;
        }
    }
}