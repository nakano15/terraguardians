using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class BrutusSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Brutus);
        public const int TownNpcsForBrutusToBeginAppearing = 2;

        public static int ChanceCounter()
        {
            int Chance = 0;
            if (NPC.downedBoss1)
                Chance++;
            if (NPC.downedBoss2)
                Chance++;
            if (NPC.downedBoss3)
                Chance++;
            if (NPC.downedQueenBee)
                Chance++;
            if (NPC.downedSlimeKing)
                Chance++;
            if (Main.hardMode)
                Chance++;
            if (NPC.downedMechBoss1)
                Chance++;
            if (NPC.downedMechBoss2)
                Chance++;
            if (NPC.downedMechBoss3)
                Chance++;
            if (NPC.downedPlantBoss)
                Chance++;
            if (NPC.downedGolemBoss)
                Chance++;
            if (NPC.downedFishron)
                Chance++;
            if (NPC.downedMoonlord)
                Chance++;
            if (NPC.downedGoblins)
                Chance++;
            if (NPC.downedPirates)
                Chance++;
            if (NPC.downedMartians)
                Chance++;
            if (NPC.downedFrost)
                Chance++;
            return Chance;
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if ((spawnInfo.Player is Companion) || WorldMod.HasMetCompanion(ToSpawnID.ID, ToSpawnID.ModID) || MainMod.HasCompanionInWorld(ToSpawnID.ID, ToSpawnID.ModID) || Main.fastForwardTime || (Main.invasionType > 0 && Main.invasionSize > 0) || !spawnInfo.PlayerInTown || spawnInfo.Water  || Main.eclipse || !Main.dayTime || Main.time < 3 * 3600)
                return 0;
            float npccount = WorldMod.GetCompanionsCount * 0.5f;
            for (int n = 0; n < 200; n++) if (Main.npc[n].active && Main.npc[n].townNPC) npccount++;
            if (npccount < TownNpcsForBrutusToBeginAppearing)
                return 0;
            float SpawnChance = 20 - ChanceCounter() * 0.5f;
            if (SpawnChance > 0 && Main.rand.NextFloat() * SpawnChance > (npccount - TownNpcsForBrutusToBeginAppearing) * 0.5f)
                return 0;
            return 1;
        }
    }
}