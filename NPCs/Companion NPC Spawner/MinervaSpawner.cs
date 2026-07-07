using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MinervaSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Minerva);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if(Main.dayTime && Main.time < 3600 * 6.5 && !spawner.waterTile && CanSpawnCompanionNpc(false) && TargetIsPlayer(spawner.Player) && (!WorldMod.HasMetCompanion(CompanionDB.Minerva) || PlayerMod.PlayerGetCompanionFriendshipLevel(spawner.Player, CompanionDB.Minerva) < 3))
                return 1f / 150;
            return 0;
        }
    }
}