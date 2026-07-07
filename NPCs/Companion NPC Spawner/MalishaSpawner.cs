using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MalishaSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Malisha);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!Terraria.Main.dayTime && !Terraria.Main.eclipse && !spawner.spawnFriendly && CanSpawnCompanionNpc() && TargetIsPlayer(spawner.Player) && Terraria.Main.time > 19800)
                return (float)(Terraria.Main.time - 19800) * (1f / 54000);
            return 0;
        }
    }
}