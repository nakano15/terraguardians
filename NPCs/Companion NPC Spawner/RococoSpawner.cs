using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class RococoSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(0);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            /*if(!spawnInfo.Water && !MainMod.HasCompanionInWorld(ToSpawnID) && !WorldMod.HasMetCompanion(ToSpawnID) && 
                Main.dayTime && Main.time >= 27000 && Main.time < 48600)
            {
                return ((float)Main.time - 27000) * (1f / 432000 * 0.5f);
            }*/
            if (!MainMod.HasCompanionInWorld(ToSpawnID))
                return 1f / 50;
            return 0;
        }
    }
}