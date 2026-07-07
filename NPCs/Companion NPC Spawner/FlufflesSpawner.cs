using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class FlufflesSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Fluffles);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && Main.invasionSize == 0 && spawner.Player is not Companion && 
                !spawner.Player.GetModPlayer<PlayerMod>().GhostFoxHaunt && ((spawner.Player.position.Y < Main.worldSurface * 16 && !Main.dayTime && !Main.bloodMoon && !Main.pumpkinMoon && !Main.snowMoon) || 
                (spawner.Player.position.Y >= Main.worldSurface * 16)) && !spawner.spawnFriendly)
            {
                if (!NPC.AnyDanger())
                    return (Main.halloween || NPC.downedHalloweenTree) ? .02f : .0025f;
            }
            return 0;
        }
    }
}