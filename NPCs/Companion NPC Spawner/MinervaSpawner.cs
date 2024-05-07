using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MinervaSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Minerva);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(Main.dayTime && Main.time < 3600 * 6.5 && !spawnInfo.Water && CanSpawnCompanionNpc(false) && TargetIsPlayer(spawnInfo.Player) && (!WorldMod.HasMetCompanion(CompanionDB.Minerva) || PlayerMod.PlayerGetCompanionFriendshipLevel(spawnInfo.Player, CompanionDB.Minerva) < 3))
                return 1f / 150;
            return 0;
        }
    }
}