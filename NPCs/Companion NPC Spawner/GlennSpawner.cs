using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class GlennSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Glenn);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && spawnInfo.PlayerInTown && PlayerMod.IsPlayerCharacter(spawnInfo.Player) && CanSpawnCompanionNpc() && Main.dayTime && (PlayerMod.PlayerHasCompanion(spawnInfo.Player, CompanionDB.Sardine) || PlayerMod.PlayerHasCompanion(spawnInfo.Player, CompanionDB.Bree)))
            {
                return 1f / 200;
            }
            return 0;
        }
    }
}