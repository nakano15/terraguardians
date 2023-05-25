using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.Graphics.Renderers;
using Terraria.ModLoader;
using System.Linq;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.WorldBuilding;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class SardineBountyBoard
    {
        public static bool BountyBoardTalkedAbout = false;
        public static int SignID = -1;
        public static bool IsAnnouncementBox = false;
        public const int NewRequestMinTime = 10 * 3600, NewRequestMaxTime = 20 * 3600,
            RequestEndMinTime = 20 * 3600, RequestEndMaxTime = 35 * 3600;
        public static int CoinReward = 0;
        public static Item[] RewardList = new Item[0];
        public const string NoRequestText = "No bounty right now.";
        public static int TargetMonsterID = 0;
        public static int TargetMonsterPosition = -1;
        public static string TargetName = "";
        public static string TargetSuffix = "";
        public static int ActionCooldown = 0;
        public static SpawnBiome spawnBiome = 0;
        public static int SpawnStress = 0;
        private static byte BoardUpdateTime = 0;

        internal static void Unload()
        {
            for(int i = 0; i < RewardList.Length; i++)
                RewardList[i] = null;
            RewardList = null;
            TargetName = null;
            TargetSuffix = null;
        }

        public static void Reset()
        {
            TargetMonsterID = 0;
            TargetMonsterPosition = -1;
            SetDefaultCooldown();
        }

        public static void Update()
        {
            if (SignID == -1)
            {
                if (Main.rand.Next(200) == 0)
                    TryFindingASign();
            }
            else
            {
                if(BoardUpdateTime == 0)
                {
                    BoardUpdateTime = 60;
                    UpdateBountyBoardText();
                }
                BoardUpdateTime--;
            }
        }

        private static void UpdateBountyBoardText()
        {

        }

        public static void TryFindingASign()
        {
            bool LastHadSign = SignID >= 0;
            SignID = -1;
            Companion Sardine = WorldMod.GetCompanionNpc(CompanionDB.Sardine);
            if(Sardine == null)
            {
                return;
            }
            CompanionTownNpcState tns = Sardine.GetTownNpcState;
            if (tns == null || tns.Homeless) return;
            if(tns.HouseInfo == null) return;
            int HomeX = tns.HomeX, HomeY = tns.HomeY;
            foreach (FurnitureInfo furniture in tns.HouseInfo.Furnitures)
            {
                bool AnnouncementBox;
                if (AnnouncementBox = (furniture.FurnitureID == TileID.AnnouncementBox) || furniture.FurnitureID == TileID.Signs)
                {
                    SignID = Sign.ReadSign(furniture.FurnitureX, furniture.FurnitureY);
                    if (SignID > -1)
                    {
                        IsAnnouncementBox = AnnouncementBox;
                        break;
                    }
                }
            }
            if (!LastHadSign && SignID > -1)
            {
                Main.sign[SignID].text = "Request coming soon...";
            }
        }

        public static void ResetSpawnStress(bool OnGeneration)
        {
            if (OnGeneration)
                SpawnStress = Main.rand.Next(20, 41);
            else
                SpawnStress = Main.rand.Next(10, 21);
            if (Main.hardMode) SpawnStress += 20;
            if (spawnBiome == SpawnBiome.Night)
                SpawnStress /= 2;
        }

        public static void CreateRewards(float RewardMod)
        {
            List<Item> Rewards = new List<Item>();
            Item i;
            /*if (Main.rand.NextDouble() < 0.03 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(4))
                {
                    case 0:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.PackLeaderNecklace>(), true);
                        break;
                    case 1:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.GoldenShowerPapyrus>(), true);
                        break;
                    case 2:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.FirstSymbol>(), true);
                        break;
                    case 3:
                        i.SetDefaults(ModContent.ItemType<Items.Accessories.TwoHandedMastery>(), true);
                        break;
                }
                Rewards.Add(i);
            }*/
            if (Main.rand.NextDouble() < 0.0667f * RewardMod)
            {
                i = new Item();
                i.SetDefaults(ItemID.FuzzyCarrot);
                Rewards.Add(i);
            }
                if (Main.rand.NextDouble() < 0.1 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ItemID.FishermansGuide, true);
                        break;
                    case 1:
                        i.SetDefaults(ItemID.WeatherRadio, true);
                        break;
                    case 2:
                        i.SetDefaults(ItemID.Sextant, true);
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.2 * RewardMod)
            {
                i = new Item();
                switch (Main.rand.Next(3))
                {
                    case 0:
                        i.SetDefaults(ItemID.LifeformAnalyzer, true);
                        break;
                    case 1:
                        i.SetDefaults(ItemID.MetalDetector, true);
                        break;
                    case 2:
                        i.SetDefaults(ItemID.Radar, true);
                        break;
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.05f * RewardMod)
            {
                i = new Item();
                if (Main.rand.NextDouble() < 0.75)
                {
                    i.SetDefaults(ItemID.MagicMirror, true);
                }
                else
                {
                    i.SetDefaults(ItemID.PocketMirror, true);
                }
                Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.1f * RewardMod)
            {
                i = GetRandomAccessory();
                if (i != null)
                    Rewards.Add(i);
            }
            if (Main.rand.NextDouble() < 0.15 * RewardMod)
            {
                i = GetRandomWeaponID();
                if (i != null)
                {
                    Rewards.Add(i);
                }
            }
            if (Main.rand.NextDouble() < 0.25 * RewardMod)
            {
                i = new Item();
                List<int> BossSpawnItems = new List<int>();
                //Check each boss that has been killed, then add the respective item to the boss spawn item list.
                if (NPC.downedBoss1)
                    BossSpawnItems.Add(ItemID.SuspiciousLookingEye);
                if (NPC.downedBoss2)
                {
                    if (WorldGen.crimson)
                        BossSpawnItems.Add(ItemID.BloodySpine);
                    else
                        BossSpawnItems.Add(ItemID.WormFood);
                }
                if (NPC.downedBoss3)
                    BossSpawnItems.Add(ItemID.ClothierVoodooDoll);
                if (NPC.downedSlimeKing)
                    BossSpawnItems.Add(ItemID.SlimeCrown);
                if (NPC.downedQueenBee)
                    BossSpawnItems.Add(ItemID.Abeemination);
                if (Main.hardMode)
                    BossSpawnItems.Add(ItemID.GuideVoodooDoll);
                if (NPC.downedMechBossAny)
                {
                    BossSpawnItems.Add(ItemID.MechanicalEye);
                    BossSpawnItems.Add(ItemID.MechanicalSkull);
                    BossSpawnItems.Add(ItemID.MechanicalWorm);
                }
                if (NPC.downedGolemBoss)
                    BossSpawnItems.Add(ItemID.LihzahrdPowerCell);
                if (NPC.downedFishron)
                    BossSpawnItems.Add(ItemID.TruffleWorm);
                if (NPC.downedMoonlord)
                    BossSpawnItems.Add(ItemID.CelestialSigil);
                if (BossSpawnItems.Count > 0)
                {
                    i.SetDefaults(BossSpawnItems[Main.rand.Next(BossSpawnItems.Count)], true);
                    /*if (i.maxStack > 0)
                    {
                        i.stack += Main.rand.Next((int)(3 * RewardMod));
                    }*/
                    Rewards.Add(i);
                }
            }
            if (Main.hardMode && NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3 && Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.LifeFruit, true);
                if (Main.rand.NextDouble() < 0.6 * RewardMod)
                    i.stack += Main.rand.Next((int)(2 * RewardMod));
                Rewards.Add(i);
            }
            if (Main.rand.Next(3) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.LifeCrystal, true);
                //if (Main.rand.NextDouble() < 0.4 * RewardMod)
                //    i.stack += Main.rand.Next((int)(RewardMod));
                Rewards.Add(i);
            }
            if (spawnBiome != SpawnBiome.Underworld && Main.rand.Next(5) == 0)
            {
                i = new Item();
                switch (spawnBiome)
                {
                    case SpawnBiome.Corruption:
                        i.SetDefaults(ItemID.CorruptFishingCrate, true);
                        break;
                    case SpawnBiome.Crimson:
                        i.SetDefaults(ItemID.CrimsonFishingCrate, true);
                        break;
                    case SpawnBiome.Dungeon:
                        i.SetDefaults(ItemID.DungeonFishingCrate, true);
                        break;
                    case SpawnBiome.Hallow:
                        i.SetDefaults(ItemID.HallowedFishingCrate, true);
                        break;
                    case SpawnBiome.Jungle:
                        i.SetDefaults(ItemID.JungleFishingCrate, true);
                        break;
                    case SpawnBiome.Sky:
                        i.SetDefaults(ItemID.FloatingIslandFishingCrate, true);
                        break;
                }
                i.stack += Main.rand.Next((int)(3 * RewardMod));
                if(i.type != 0)
                    Rewards.Add(i);
            }
            if (Main.rand.Next(5) == 0)
            {
                i = new Item();
                i.SetDefaults(ItemID.HerbBag);
                i.stack += Main.rand.Next((int)(3 * RewardMod));
                Rewards.Add(i);
            }
            RewardList = Rewards.ToArray();
        }

        public static Item GetRandomWeaponID()
        {
            int WeaponID = 0;
            {
                List<int> RewardToGet = new List<int>();
                switch (spawnBiome)
                {
                    case SpawnBiome.Corruption:
                        RewardToGet.AddRange(new int[] { ItemID.BallOHurt, ItemID.DemonBow, ItemID.Musket, ItemID.Vilethorn, ItemID.LightsBane, ItemID.DemonBow });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.Toxikarp, ItemID.DartRifle, ItemID.CursedFlames, ItemID.ClingerStaff });
                        }
                        break;

                    case SpawnBiome.Crimson:
                        RewardToGet.AddRange(new int[] { ItemID.TheRottedFork, ItemID.TheUndertaker, ItemID.TheMeatball, ItemID.TendonBow, ItemID.CrimsonRod, ItemID.BloodButcherer });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.Bladetongue, ItemID.DartPistol, ItemID.GoldenShower });
                        }
                        break;

                    case SpawnBiome.Dungeon:
                        RewardToGet.AddRange(new int[] { ItemID.Muramasa, ItemID.Handgun, ItemID.AquaScepter, ItemID.MagicMissile, ItemID.BlueMoon });
                        if (NPC.downedPlantBoss)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.TacticalShotgun, ItemID.SniperRifle, ItemID.Keybrand, ItemID.RocketLauncher, ItemID.SpectreStaff, ItemID.InfernoFork, ItemID.ShadowbeamStaff, ItemID.MagnetSphere });
                        }
                        break;

                    case SpawnBiome.Jungle:
                        RewardToGet.AddRange(new int[] { ItemID.BladeofGrass, ItemID.ThornChakram, ItemID.BeeKeeper, ItemID.BeesKnees, ItemID.BeeGun, ItemID.HornetStaff });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.ChlorophyteClaymore, ItemID.ChlorophytePartisan, ItemID.ChlorophyteSaber, ItemID.ChlorophyteShotbow, ItemID.Uzi });
                        }
                        break;

                    case SpawnBiome.Underworld:
                        RewardToGet.AddRange(new int[] { ItemID.FieryGreatsword, ItemID.DarkLance, ItemID.Sunfury, ItemID.FlowerofFire, ItemID.Flamelash, ItemID.HellwingBow, ItemID.ImpStaff });
                        if (NPC.downedMechBossAny)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.UnholyTrident, ItemID.ObsidianSwordfish });
                        }
                        break;

                    case SpawnBiome.Hallow:
                        RewardToGet.AddRange(new int[] { ItemID.PearlwoodSword });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.CrystalStorm, ItemID.CrystalSerpent });
                        }
                        break;

                    case SpawnBiome.Ocean:
                        RewardToGet.AddRange(new int[] { ItemID.Swordfish });
                        break;

                    case SpawnBiome.Underground:
                        RewardToGet.AddRange(new int[] { ItemID.ChainKnife, ItemID.Spear, ItemID.WoodenBoomerang, ItemID.EnchantedBoomerang });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.BeamSword, ItemID.Marrow, ItemID.PoisonStaff, ItemID.SpiderStaff, ItemID.QueenSpiderStaff });
                        }
                        break;

                    case SpawnBiome.Sky:
                        RewardToGet.AddRange(new int[] { ItemID.Starfury, ItemID.DaedalusStormbow });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.NimbusRod });
                        }
                        break;

                    case SpawnBiome.OldOneArmy:

                        break;

                    case SpawnBiome.Snow:
                        RewardToGet.AddRange(new int[] { ItemID.IceBlade, ItemID.IceBoomerang, ItemID.SnowballCannon });
                        if (Main.hardMode)
                        {
                            RewardToGet.AddRange(new int[] { ItemID.Frostbrand, ItemID.IceBow, ItemID.FlowerofFrost, ItemID.FrostStaff, ItemID.IceRod });
                        }
                        break;

                    case SpawnBiome.Desert:
                        RewardToGet.AddRange(new int[] { ItemID.AntlionMandible, ItemID.AmberStaff });
                        break;
                }
                if (!Main.hardMode)
                {
                    float LootRate = Main.rand.NextFloat();
                    RewardToGet.AddRange(new int[] { ItemID.CopperShortsword, ItemID.CopperBroadsword, ItemID.CopperBow,
                        ItemID.TinShortsword, ItemID.TinBroadsword, ItemID.TinBow});
                    //if (LootRate < 0.667)
                    {
                        RewardToGet.AddRange(new int[] { ItemID.SilverShortsword, ItemID.SilverBroadsword, ItemID.SilverBow,
                            ItemID.TungstenShortsword, ItemID.TungstenBroadsword, ItemID.TungstenBow});
                    }
                    //if (LootRate < 0.333)
                    {
                        RewardToGet.AddRange(new int[] { ItemID.GoldShortsword, ItemID.GoldBroadsword, ItemID.GoldBow,
                            ItemID.PlatinumShortsword, ItemID.PlatinumBroadsword, ItemID.PlatinumBow});
                    }
                }
                else
                {
                    float LootRate = Main.rand.NextFloat();
                    RewardToGet.AddRange(new int[] { ItemID.CobaltSword, ItemID.CobaltNaginata, ItemID.CobaltRepeater,
                        ItemID.PalladiumSword, ItemID.PalladiumPike, ItemID.PalladiumRepeater});
                    //if (LootRate < 0.667)
                    {
                        RewardToGet.AddRange(new int[] {ItemID.MythrilSword, ItemID.MythrilHalberd, ItemID.MythrilRepeater,
                            ItemID.OrichalcumSword, ItemID.OrichalcumHalberd, ItemID.OrichalcumRepeater });
                    }
                    //if (LootRate < 0.333)
                    {
                        RewardToGet.AddRange(new int[] { ItemID.AdamantiteSword, ItemID.AdamantiteGlaive, ItemID.AdamantiteRepeater,
                            ItemID.TitaniumSword, ItemID.TitaniumTrident, ItemID.TitaniumRepeater});
                    }
                }
                if(NPC.downedMechBoss1 && NPC.downedMechBoss2 && NPC.downedMechBoss3)
                {
                    RewardToGet.AddRange(new int[] { ItemID.Excalibur, ItemID.Gungnir, ItemID.HallowedRepeater });
                }
                if (RewardToGet.Count > 0)
                    WeaponID = RewardToGet[Main.rand.Next(RewardToGet.Count)];
            }
            if (WeaponID > 0)
            {
                Item i = new Item();
                i.SetDefaults(WeaponID, true);
                byte prefix = 0;
                if (i.DamageType is MeleeDamageClass)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Dangerous;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Savage;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Savage;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Legendary;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.DamageType is RangedDamageClass)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Deadly2;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Rapid;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Powerful;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Unreal;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.DamageType is MagicDamageClass)
                {
                    switch (Main.rand.Next(10))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Masterful;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Celestial;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Mystic;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Mythical;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                if (i.DamageType is SummonDamageClass)
                {
                    switch (Main.rand.Next(9))
                    {
                        case 1:
                            prefix = Terraria.ID.PrefixID.Deadly; //Common
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Ruthless; //Universal
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Godly; //Universal
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Demonic; //Universal
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Murderous;
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Hurtful;
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Unpleasant;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Superior;
                            break;
                    }
                }
                i.Prefix(prefix);
                return i;
            }
            return null;
        }

        public static Item GetRandomAccessory()
        {
            List<int> ItemIDs = new List<int>();
            ItemIDs.Add(ItemID.Aglet);
            ItemIDs.Add(ItemID.HermesBoots);
            ItemIDs.Add(ItemID.ClimbingClaws);
            ItemIDs.Add(ItemID.ShoeSpikes);
            ItemIDs.Add(ItemID.Flipper);
            ItemIDs.Add(ItemID.LuckyHorseshoe);
            ItemIDs.Add(ItemID.ShinyRedBalloon);
            //
            ItemIDs.Add(ItemID.BandofRegeneration);

            switch (spawnBiome)
            {
                case SpawnBiome.Corruption:
                    ItemIDs.Add(ItemID.BandofRegeneration);
                    break;
                case SpawnBiome.Crimson:
                    ItemIDs.Add(ItemID.PanicNecklace);
                    break;
                case SpawnBiome.Dungeon:
                    ItemIDs.Add(ItemID.CobaltShield);
                    break;
                case SpawnBiome.Jungle:
                    ItemIDs.Add(ItemID.AnkletoftheWind);
                    ItemIDs.Add(ItemID.FeralClaws);
                    ItemIDs.Add(ItemID.FlowerBoots);
                    break;
                case SpawnBiome.Underworld:
                    ItemIDs.Add(ItemID.LavaCharm);
                    ItemIDs.Add(ItemID.ObsidianRose);
                    ItemIDs.Add(ItemID.ObsidianSkull);
                    ItemIDs.Add(ItemID.MagmaStone);
                    break;
            }
            if (ItemIDs.Count > 0)
            {
                Item i = new Item();
                i.SetDefaults(ItemIDs[Main.rand.Next(ItemIDs.Count)], true);
                byte prefix = 0;
                if (Main.rand.NextDouble() < 0.8f)
                {
                    switch (Main.rand.Next(12))
                    {
                        case 0:
                            prefix = Terraria.ID.PrefixID.Armored;
                            break;
                        case 1:
                            prefix = Terraria.ID.PrefixID.Warding;
                            break;
                        case 2:
                            prefix = Terraria.ID.PrefixID.Precise;
                            break;
                        case 3:
                            prefix = Terraria.ID.PrefixID.Lucky;
                            break;
                        case 4:
                            prefix = Terraria.ID.PrefixID.Angry;
                            break;
                        case 5:
                            prefix = Terraria.ID.PrefixID.Menacing;
                            break;
                        case 6:
                            prefix = Terraria.ID.PrefixID.Hasty;
                            break;
                        case 7:
                            prefix = Terraria.ID.PrefixID.Quick;
                            break;
                        case 8:
                            prefix = Terraria.ID.PrefixID.Intrepid;
                            break;
                        case 9:
                            prefix = Terraria.ID.PrefixID.Violent;
                            break;
                        case 10:
                            prefix = Terraria.ID.PrefixID.Arcane;
                            break;

                    }
                }
                i.Prefix(prefix);
                return i;
            }
            return null;
        }

        public static int ExtraCoinRewardFromProgress()
        {
            int Extra = 0;
            if (NPC.downedBoss1)
                Extra += 125;
            if (NPC.downedBoss2)
                Extra += 250;
            if (NPC.downedBoss3)
                Extra += 500;
            if (NPC.downedSlimeKing)
                Extra += 350;
            if (NPC.downedQueenBee)
                Extra += 400;
            if (Main.hardMode)
                Extra += 1000;
            if (NPC.downedMechBoss1)
                Extra += 1150;
            if (NPC.downedMechBoss2)
                Extra += 1400;
            if (NPC.downedMechBoss3)
                Extra += 1850;
            if (NPC.downedPlantBoss)
                Extra += 2220;
            if (NPC.downedGolemBoss)
                Extra += 2650;
            if (NPC.downedAncientCultist)
                Extra += 3500;
            if (NPC.downedFishron)
                Extra += 4000;
            if (NPC.downedMoonlord)
                Extra += 5000;
            return Extra;
        }
        
        public static string GetRandomString(string[] List)
        {
            return Utils.SelectRandom(Main.rand, List);
        }

        public static string NameGen(string[] Syllabes)
        {
            string NewName = "";
            double Chance = 2f;
            bool First = true;
            List<int> UsedSyllabes = new List<int>();
            while (Main.rand.NextDouble() < Chance)
            {
                int SelectedSyllabe = Main.rand.Next(Syllabes.Length);
                int SyllabesDisponible = 0;
                for (int s = 0; s < Syllabes.Length; s++)
                {
                    if (!UsedSyllabes.Contains(s))
                    {
                        SyllabesDisponible++;
                    }
                }
                if (SyllabesDisponible == 0)
                    break;
                if (UsedSyllabes.Contains(SelectedSyllabe))
                    continue;
                UsedSyllabes.Add(SelectedSyllabe);
                string Syllabe = Syllabes[SelectedSyllabe].ToLower();
                foreach (char Letter in Syllabe)
                {
                    NewName += Letter;
                    if (First)
                    {
                        NewName = NewName.ToUpper();
                        First = false;
                    }
                }
                if (Chance > 1f)
                    Chance--;
                else if (Chance > 0.5f)
                    Chance -= 0.2f;
                else
                {
                    Chance *= 0.5f;
                }
            }
            return NewName;
        }

        public static void SetDefaultCooldown()
        {
            ActionCooldown = 5 * 3600;
        }
        
        public enum SpawnBiome : byte
        {
            Corruption, 
            Crimson, 
            Dungeon, 
            Jungle, 
            Underworld, 
            Hallow,
            Ocean, 
            Underground, 
            Sky, 
            OldOneArmy, 
            GoblinArmy, 
            Night, 
            Snow, 
            Desert,
            PirateArmy, 
            MartianMadness, 
            SnowLegion, 
            LihzahrdTemple
        }

        public enum Modifiers : byte
        {
            ExtraHealth,
            ExtraDamage,
            ExtraDefense,
            Armored,
            HealthRegen,
            Boss,
            Noob,
            Petrifying,
            Leader,
            FireRain,
            Fatal,
            Imobilizer,
            MeleeDefense,
            RangedDefense,
            MagicDefense,
            Sapping,
            Osmose,
            Retaliate,
            ManaBurn,
            Count
        }

        public enum DangerousModifier : byte
        {
            None,
            Reaper,
            Cyclops,
            GoldenShower,
            Haunted,
            Alchemist,
            Sharknado,
            Sapper,
            Cursed,
            Invoker
        }
    }
}