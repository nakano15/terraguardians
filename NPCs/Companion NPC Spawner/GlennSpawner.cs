using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class GlennSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Glenn);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && spawner.spawnFriendly && PlayerMod.IsPlayerCharacter(spawner.Player) && CanSpawnCompanionNpc() && Main.dayTime && (PlayerMod.PlayerHasCompanion(spawner.Player, CompanionDB.Sardine) || PlayerMod.PlayerHasCompanion(spawner.Player, CompanionDB.Bree)))
            {
                return 1f / 200;
            }
            return 0;
        }
    }
}