using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians
{
    public class HandyMethods
    {
        public static bool IsXmas(Player p, CompanionData d)
        {
            return Main.xMas;
        }

        public static bool IsHalloween(Player p, CompanionData d)
        {
            return Main.halloween;
        }
        public static bool IsXmasHardmode(Player p, CompanionData d)
        {
            return Main.hardMode && Main.xMas;
        }

        public static bool IsHalloweenHardmode(Player p, CompanionData d)
        {
            return Main.hardMode && Main.halloween;
        }

        public static bool IsQueenSlimeDown(Player p, CompanionData d)
        {
            return NPC.downedAncientCultist;
        }

        public static bool IsLunaticCultistDown(Player p, CompanionData d)
        {
            return NPC.downedAncientCultist;
        }

        public static bool IsKingSlimeDown(Player p, CompanionData d)
        {
            return NPC.downedSlimeKing;
        }

        public static bool WorldIsCorruptAndBossKilled(Player p, CompanionData d)
        {
            return !WorldGen.crimson && NPC.downedBoss2;
        }

        public static bool WorldIsCrimsonAndBossKilled(Player p, CompanionData d)
        {
            return WorldGen.crimson && NPC.downedBoss2;
        }

        public static bool WorldIsCorrupt(Player p, CompanionData d)
        {
            return !WorldGen.crimson;
        }

        public static bool WorldIsCrimson(Player p, CompanionData d)
        {
            return WorldGen.crimson;
        }

        public static bool WorldIsCorruptAndHardmode(Player p, CompanionData d)
        {
            return !WorldGen.crimson && Main.hardMode;
        }

        public static bool WorldIsCrimsonAndHardmode(Player p, CompanionData d)
        {
            return WorldGen.crimson && Main.hardMode;
        }

        public static bool IsGolemDown(Player p, CompanionData d)
        {
            return NPC.downedGolemBoss;
        }

        public static bool IsPlanteraDown(Player p, CompanionData d)
        {
            return NPC.downedPlantBoss;
        }

        public static bool IsAnyMechBossDown(Player p, CompanionData d)
        {
            return NPC.downedMechBossAny;
        }

        public static bool IsQueenBeeDown(Player p, CompanionData d)
        {
            return NPC.downedQueenBee;
        }

        public static bool IsSkeletronDown(Player p, CompanionData d)
        {
            return NPC.downedBoss3;
        }

        public static bool IsEvilBossDown(Player p, CompanionData d)
        {
            return NPC.downedBoss2;
        }

        public static bool IsEoCDown(Player p, CompanionData d)
        {
            return NPC.downedBoss1;
        }

        public static bool IsAnyFirstThreeBossesDown(Player p, CompanionData d)
        {
            return NPC.downedBoss1 || NPC.downedBoss2 || NPC.downedBoss3;
        }

        public static bool IsEoCDownCorruption(Player p, CompanionData d)
        {
            return NPC.downedBoss1 && !WorldGen.crimson;
        }

        public static bool IsEoCDownCrimson(Player p, CompanionData d)
        {
            return NPC.downedBoss1 && WorldGen.crimson;
        }

        public static bool IsFullMoonHardmode(Player p, CompanionData d)
        {
            return Main.hardMode && Main.moonPhase == 0;
        }

        public static bool IsFullMoon(Player p, CompanionData d)
        {
            return Main.moonPhase == 0;
        }

        public static bool IsHardmode(Player p, CompanionData d)
        {
            return Main.hardMode;
        }

        public static bool IsAllMechBossesDead(Player p, CompanionData d)
        {
            return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        public static bool IsRemixWorld(Player p, CompanionData d)
        {
            return Main.remixWorld;
        }

        public static bool PlayerHasBugNets(Player p, CompanionData d)
        {
            for(int i = 0; i < 50; i++)
            {
                if (p.inventory[i].type > 0 && (p.inventory[i].type == ItemID.BugNet || p.inventory[i].type == ItemID.GoldenBugNet|| p.inventory[i].type == ItemID.FireproofBugNet))
                {
                    return true;
                }
            }
            return false;
        }

        public static bool PlayerHasFishingRod(Player p, CompanionData d)
        {
            for(int i = 0; i < 50; i++)
            {
                if (p.inventory[i].type > 0 && p.fishingSkill > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public static bool DefaultGetRewardTrue(Player p, CompanionData d)
        {
            return true;
        }
    }
}