using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class GenericTerrarianSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.GenericTerrarian);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
        }
    }
}