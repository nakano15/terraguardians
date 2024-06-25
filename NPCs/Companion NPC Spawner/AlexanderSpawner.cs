using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class AlexanderSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Alexander);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.Water && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && spawnInfo.Player.ZoneDungeon)
                return 1f / 400;
            return 0;
        }
    }
}