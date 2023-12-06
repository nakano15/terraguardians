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

        public override void AI() //Better avoid companion copies from cluttering people companions list for now.
        {
            NPC.TargetClosest(false);
            Terraria.NPC.SpawnOnPlayer(NPC.target, 68);
            NPC.active = false;
        }
    }
}