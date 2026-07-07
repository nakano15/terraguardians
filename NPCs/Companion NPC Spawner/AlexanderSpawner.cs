using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class AlexanderSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Alexander);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if(!spawner.waterTile && CanSpawnCompanionNpc() && TargetIsPlayer(spawner.Player) && spawner.Player.ZoneDungeon)
                return 1f / 300;
            return 0;
        }
    }
}