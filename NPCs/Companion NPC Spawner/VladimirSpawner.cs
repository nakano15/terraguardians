using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class VladimirSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Vladimir);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && IsDecentSpawnCondition(spawnInfo) && spawnInfo.Player.ZoneJungle && Main.rand.Next(256 - (int)(spawnInfo.Player.position.Y * (1f / 1024))) == 0)
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