using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class GenericTerrarianSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.GamerGenericTerrarian);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            return 0;
            //return TargetIsPlayer(spawnInfo.Player) && !spawnInfo.Water && (spawnInfo.Player.ZoneCorrupt || spawnInfo.Player.ZoneCrimson) && CanSpawnCompanionNpc(false) ? 0.15f : 0f;
        }

        public override void AI() //Better avoid companion copies from cluttering people companions list for now.
        {
            NPC.TargetClosest(false);
            Terraria.NPC.SpawnOnPlayer(NPC.target, 68);
            NPC.active = false;
        }
    }
}