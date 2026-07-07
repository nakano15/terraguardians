using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class AlexSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Alex);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            return 0; //He shouldn't ever spawn naturally
        }
    }
}