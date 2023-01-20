using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace terraguardians
{
    public class RequestReward
    {
        private static List<RequestReward> Rewards = new List<RequestReward>();

        public int ID = 0, Stack = 1;
        public float AcquisitionChance = 1;
        public Func<Player, CompanionData, bool> CanGetReward = DefaultGetRewardTrue;

        public static void Initialize()
        {
            AddPossibleReward(ItemID.LifeCrystal, Chance: 0.2f);
            AddPossibleReward(ItemID.LifeFruit, Chance: 0.2f).CanGetReward = IsAllMechBossesDead;
            //Crates
            AddPossibleReward(ItemID.WoodenCrate, Chance: 0.625f);
            AddPossibleReward(ItemID.IronCrate, Chance: 0.390625f);
            AddPossibleReward(ItemID.GoldenCrate, Chance: 0.09765625f);
            AddPossibleReward(ItemID.CorruptFishingCrate, Chance: 0.05f);
            AddPossibleReward(ItemID.CrimsonFishingCrate, Chance: 0.05f);
            AddPossibleReward(ItemID.HallowedFishingCrate, Chance: 0.05f);
            AddPossibleReward(ItemID.OasisCrate, Chance: 0.05f);
            AddPossibleReward(ItemID.FrozenCrate, Chance: 0.05f);
            AddPossibleReward(ItemID.LavaCrate, Chance: 0.05f).CanGetReward = IsEvilBossDown;
            AddPossibleReward(ItemID.JungleFishingCrate, Chance: 0.05f).CanGetReward = IsQueenBeeDown;
            AddPossibleReward(ItemID.DungeonFishingCrate, Chance: 0.05f).CanGetReward = IsSkeletronDown;
            AddPossibleReward(ItemID.FloatingIslandFishingCrate, Chance: 0.05f).CanGetReward = IsEoCDown;
            //HM Crates
            AddPossibleReward(ItemID.WoodenCrateHard, Chance: 0.625f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.IronCrateHard, Chance: 0.390625f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.GoldenCrateHard, Chance: 0.09765625f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.CorruptFishingCrateHard, Chance: 0.05f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.CrimsonFishingCrateHard, Chance: 0.05f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.HallowedFishingCrateHard, Chance: 0.05f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.JungleFishingCrateHard, Chance: 0.05f).CanGetReward = IsPlanteraDown;
            AddPossibleReward(ItemID.DungeonFishingCrateHard, Chance: 0.05f).CanGetReward = IsGolemDown;
            AddPossibleReward(ItemID.OasisCrateHard, Chance: 0.05f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.FrozenCrateHard, Chance: 0.05f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.LavaCrateHard, Chance: 0.05f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.FloatingIslandFishingCrateHard, Chance: 0.05f).CanGetReward = IsAnyMechBossDown;
            // Food
            AddPossibleReward(ItemID.CookedFish, 3, 0.125f);
            AddPossibleReward(ItemID.BowlofSoup, 3, 0.125f);
            AddPossibleReward(ItemID.BunnyStew, 3, 0.125f);
            AddPossibleReward(ItemID.GrilledSquirrel, 3, 0.125f);
            // Boss Spawn Items
            AddPossibleReward(ItemID.SuspiciousLookingEye, 1, 0.1f);
            AddPossibleReward(ItemID.WormFood, 1, 0.1f).CanGetReward = WorldIsCorruptAndBossKilled;
            AddPossibleReward(ItemID.BloodySpine, 1, 0.1f).CanGetReward = WorldIsCrimsonAndBossKilled;
            AddPossibleReward(ItemID.SlimeCrown, 1, 0.1f).CanGetReward = IsKingSlimeDown;
            AddPossibleReward(ItemID.Abeemination, 1, 0.1f).CanGetReward = IsQueenBeeDown;
            // HM Boss Spawn Items
            AddPossibleReward(ItemID.MechanicalSkull, 1, 0.1f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.MechanicalEye, 1, 0.1f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.MechanicalWorm, 1, 0.1f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.LihzahrdPowerCell, 1, 0.1f).CanGetReward = IsPlanteraDown;
            AddPossibleReward(4988, 1, 0.1f).CanGetReward = IsQueenSlimeDown;
            AddPossibleReward(ItemID.CelestialSigil, 1, 0.1f).CanGetReward = IsLunaticCultistDown;
            // Seasonal
            AddPossibleReward(ItemID.Present, 1, 0.5f).CanGetReward = IsXmas;
            AddPossibleReward(ItemID.GoodieBag, 1, 0.5f).CanGetReward = IsHalloween;
            // Loot
            AddPossibleReward(ItemID.SlimeStaff, 1, 0.01f);
            AddPossibleReward(ItemID.TerraBlade, 1, 0.01f);
            AddPossibleReward(ItemID.EnchantedSword, 1, 0.01f);
            AddPossibleReward(ItemID.Muramasa, 1, 0.01f).CanGetReward = IsSkeletronDown;
            AddPossibleReward(ItemID.FieryGreatsword, 1, 0.01f).CanGetReward = IsEvilBossDown;
            AddPossibleReward(ItemID.BladeofGrass, 1, 0.01f).CanGetReward = IsQueenBeeDown;
            AddPossibleReward(ItemID.BeamSword, 1, 0.01f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.Toxikarp, 1, 0.01f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.Bladetongue, 1, 0.01f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.CrystalSerpent, 1, 0.01f).CanGetReward = IsHardmode;
            // Potions
            AddPossibleReward(ItemID.LesserHealingPotion, 5, 0.5f);
            AddPossibleReward(ItemID.HealingPotion, 5, 0.5f);
            AddPossibleReward(ItemID.GreaterHealingPotion, 5, 0.5f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.SuperHealingPotion, 5, 0.5f).CanGetReward = IsLunaticCultistDown;
            AddPossibleReward(ItemID.LesserManaPotion, 5, 0.5f);
            AddPossibleReward(ItemID.ManaPotion, 5, 0.5f);
            AddPossibleReward(ItemID.GreaterManaPotion, 5, 0.5f).CanGetReward = IsHardmode;
            AddPossibleReward(ItemID.SuperManaPotion, 5, 0.5f).CanGetReward = IsLunaticCultistDown;
            //Buff Potions
            AddPossibleReward(ItemID.ArcheryPotion, 3, 0.1f);
            AddPossibleReward(ItemID.BattlePotion, 3, 0.1f);
            AddPossibleReward(ItemID.CalmingPotion, 3, 0.1f);
            AddPossibleReward(ItemID.CratePotion, 3, 0.1f);
            AddPossibleReward(ItemID.TrapsightPotion, 3, 0.1f);
            AddPossibleReward(ItemID.EndurancePotion, 3, 0.1f);
            AddPossibleReward(ItemID.GillsPotion, 3, 0.1f);
            AddPossibleReward(ItemID.GravitationPotion, 3, 0.05f);
            AddPossibleReward(ItemID.HunterPotion, 3, 0.1f);
            AddPossibleReward(ItemID.InfernoPotion, 3, 0.05f);
            AddPossibleReward(ItemID.IronskinPotion, 3, 0.1f);
            AddPossibleReward(ItemID.LifeforcePotion, 3, 0.05f);
            AddPossibleReward(ItemID.NightOwlPotion, 3, 0.1f);
            AddPossibleReward(ItemID.ObsidianSkinPotion, 3, 0.1f);
            AddPossibleReward(ItemID.RagePotion, 3, 0.1f);
            AddPossibleReward(ItemID.RegenerationPotion, 3, 0.1f);
            AddPossibleReward(ItemID.ShinePotion, 3, 0.1f);
            AddPossibleReward(ItemID.SpelunkerPotion, 3, 0.1f);
            AddPossibleReward(ItemID.SwiftnessPotion, 3, 0.1f);
            AddPossibleReward(ItemID.TitanPotion, 3, 0.1f);
            AddPossibleReward(ItemID.WaterWalkingPotion, 3, 0.1f);
            AddPossibleReward(ItemID.WrathPotion, 3, 0.1f);
        }

        public static void Unload()
        {
            Rewards.Clear();
        }

        public static RequestReward[] GetPossibleRewards(Player player, CompanionData companion, int Count = 3, float GettingNothingPercentage = 0)
        {
            List<RequestReward> PossibleRewards = new List<RequestReward>();
            float MaxChance = 0;
            foreach(RequestReward r in Rewards)
            {
                if (r.CanGetReward(player, companion))
                {
                    PossibleRewards.Add(r);
                    MaxChance += r.AcquisitionChance;
                }
            }
            MaxChance += GettingNothingPercentage * MaxChance;
            List<RequestReward> FinalRewards = new List<RequestReward>();
            while(FinalRewards.Count < Count)
            {
                if(PossibleRewards.Count == 0)
                {
                    FinalRewards.Add(null);
                }
                else
                {
                    float PickedChance = Main.rand.NextFloat() * MaxChance;
                    float Stack = 0;
                    RequestReward PickedReward = null;
                    foreach(RequestReward r in PossibleRewards)
                    {
                        if(PickedChance >= Stack && PickedChance < Stack + r.AcquisitionChance)
                        {
                            PickedReward = r;
                            break;
                        }
                        Stack += r.AcquisitionChance;
                    }
                    FinalRewards.Add(PickedReward);
                    if (PickedReward != null)
                    {
                        PossibleRewards.Remove(PickedReward);
                        MaxChance -= PickedReward.AcquisitionChance;
                    }
                }
            }
            return FinalRewards.ToArray();
        }

        public static RequestReward AddPossibleReward(int ID, int Stack = 1, float Chance = 1f)
        {
            RequestReward reward = new RequestReward(){ ID = ID, Stack = Stack, AcquisitionChance =Chance };
            Rewards.Add(reward);
            return reward;
        }

        public static bool IsXmas(Player p, CompanionData d)
        {
            return Main.xMas;
        }

        public static bool IsHalloween(Player p, CompanionData d)
        {
            return Main.halloween;
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

        public static bool IsHardmode(Player p, CompanionData d)
        {
            return Main.hardMode;
        }

        public static bool IsAllMechBossesDead(Player p, CompanionData d)
        {
            return Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3;
        }

        public static bool DefaultGetRewardTrue(Player p, CompanionData d)
        {
            return true;
        }
    }
}