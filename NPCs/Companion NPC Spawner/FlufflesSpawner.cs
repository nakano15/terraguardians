using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class FlufflesSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Fluffles);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc() && Main.invasionSize == 0 && spawnInfo.Player is not Companion && 
                !spawnInfo.Player.GetModPlayer<PlayerMod>().GhostFoxHaunt && ((spawnInfo.Player.position.Y < Main.worldSurface * 16 && !Main.dayTime && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon) || 
                (spawnInfo.Player.position.Y >= Main.worldSurface * 16)) && !spawnInfo.PlayerInTown)
            {
                if (!NPC.AnyDanger())
                    return (Main.halloween || NPC.downedHalloweenTree) ? .02f : .0025f;
            }
            return 0;
        }
    }
}