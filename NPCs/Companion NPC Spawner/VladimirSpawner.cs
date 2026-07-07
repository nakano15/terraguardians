using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class VladimirSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Vladimir);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && IsDecentSpawnCondition(spawner) && spawner.Player.ZoneJungle && Main.rand.NextBool(256 - (int)(spawner.Player.position.Y * (1f / 1024))))
                return 1;
            //if (Terraria.Main.dayTime && !Terraria.Main.eclipse && !MainMod.HasCompanionInWorld(ToSpawnID) && !WorldMod.HasMetCompanion(ToSpawnID) && TargetIsPlayer(spawnInfo.Player))
            //    return 1f / 50;
            return 0;
        }

        public override void AI()
        {
            base.AI();
        }
    }
}