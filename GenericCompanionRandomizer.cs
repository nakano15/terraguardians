using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;
using System;

namespace terraguardians
{
    public class GenericCompanionRandomizer
    {
        static Dictionary<float, GearLevelInfo> GearLevels = new Dictionary<float, GearLevelInfo>();

        internal static void Initialize()
        {
            AddGearLevel(0f, 0, 0, 0);
            AddGearLevel(.1f, ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves);
            AddGearLevel(.3f, ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.CopperBroadsword), new ItemDefinition(ItemID.CopperBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.Mushroom, 5)]);
            AddGearLevel(.4f, ItemID.TinHelmet, ItemID.TinChainmail, ItemID.TinGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.TinBroadsword), new ItemDefinition(ItemID.TinBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.Mushroom, 5)]);
            AddGearLevel(.5f, ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.IronBroadsword), new ItemDefinition(ItemID.IronBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.LesserHealingPotion, 5)]);
            AddGearLevel(.6f, ItemID.LeadHelmet, ItemID.LeadChainmail, ItemID.LeadGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.LeadBroadsword), new ItemDefinition(ItemID.LeadBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.LesserHealingPotion, 5)]);
            AddGearLevel(.7f, ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.SilverBroadsword), new ItemDefinition(ItemID.SilverBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.LesserHealingPotion, 5)]);
            AddGearLevel(.8f, ItemID.TungstenHelmet, ItemID.TungstenChainmail, ItemID.TungstenGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.TungstenBroadsword), new ItemDefinition(ItemID.TungstenBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.LesserHealingPotion, 5)]);
            AddGearLevel(.9f, ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.GoldBroadsword), new ItemDefinition(ItemID.GoldBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.LesserHealingPotion, 5)]);
            AddGearLevel(1.0f, ItemID.PlatinumHelmet, ItemID.PlatinumChainmail, ItemID.PlatinumGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.PlatinumBroadsword), new ItemDefinition(ItemID.PlatinumBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.LesserHealingPotion, 5)]);
            //Tier 2
            GearLevelInfo li = AddGearLevel(2.4f, ItemID.ShadowHelmet, ItemID.ShadowScalemail, ItemID.ShadowGreaves);
            li.AddLoadoutWeapons([new ItemDefinition(ItemID.LightsBane), new ItemDefinition(ItemID.DemonBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li.AddLoadout([new ItemDefinition(ItemID.BallOHurt), new ItemDefinition(ItemID.DemonBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li.AddLoadout([new ItemDefinition(ItemID.LightsBane), new ItemDefinition(ItemID.Musket), new ItemDefinition(ItemID.MusketBall, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li.AddLoadout([new ItemDefinition(ItemID.LightsBane), new ItemDefinition(ItemID.DemonBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.Vilethorn), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li = AddGearLevel(2.6f, ItemID.CrimsonHelmet, ItemID.CrimsonScalemail, ItemID.CrimsonGreaves);
            li.AddLoadoutWeapons([new ItemDefinition(ItemID.BloodButcherer), new ItemDefinition(ItemID.TendonBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li.AddLoadout([new ItemDefinition(ItemID.TheRottedFork), new ItemDefinition(ItemID.TendonBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li.AddLoadout([new ItemDefinition(ItemID.BloodButcherer), new ItemDefinition(ItemID.TheUndertaker), new ItemDefinition(ItemID.MusketBall, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            li.AddLoadout([new ItemDefinition(ItemID.BloodButcherer), new ItemDefinition(ItemID.TendonBow), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.CrimsonRod), new ItemDefinition(ItemID.HealingPotion, 5)]);

            //Tier 3
            AddGearLevel(2.1f, ItemID.NinjaHood, ItemID.NinjaShirt, ItemID.NinjaPants)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.Katana), new ItemDefinition(ItemID.Shuriken, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(2.2f, ItemID.FossilHelm, ItemID.FossilShirt, ItemID.FossilPants)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.ThunderSpear), new ItemDefinition(ItemID.BoneJavelin, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(2.3f, ItemID.BeeHeadgear, ItemID.BeeBreastplate, ItemID.BeeGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.BeeKeeper), new ItemDefinition(ItemID.BeesKnees), new ItemDefinition(ItemID.WoodenArrow, 250), new ItemDefinition(ItemID.HornetStaff), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(2.7f, ItemID.ObsidianHelm, ItemID.ObsidianShirt, ItemID.ObsidianPants)
                .AddLoadoutWeapons([new ItemDefinition(5074), new ItemDefinition(ItemID.Minishark), new ItemDefinition(ItemID.MusketBall, 250), new ItemDefinition(ItemID.VampireFrogStaff), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(2.8f, ItemID.MeteorHelmet, ItemID.MeteorSuit, ItemID.MeteorLeggings)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.SpaceGun), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(2.9f, ItemID.JungleHat, ItemID.JungleShirt, ItemID.JunglePants)
                .AddLoadoutWeapons([new ItemDefinition(4913), new ItemDefinition(ItemID.FlintlockPistol), new ItemDefinition(ItemID.MusketBall, 250), new ItemDefinition(ItemID.MagicMissile), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(3.0f, ItemID.NecroHelmet, ItemID.NecroBreastplate, ItemID.NecroGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.Muramasa), new ItemDefinition(ItemID.Handgun), new ItemDefinition(ItemID.MusketBall, 250), new ItemDefinition(ItemID.HealingPotion, 5)]);
            AddGearLevel(3.2f, ItemID.MoltenHelmet, ItemID.MoltenBreastplate, ItemID.MoltenGreaves)
                .AddLoadoutWeapons([new ItemDefinition(ItemID.FieryGreatsword), new ItemDefinition(ItemID.PhoenixBlaster), new ItemDefinition(ItemID.MusketBall, 250), new ItemDefinition(ItemID.FlowerofFire), new ItemDefinition(ItemID.HealingPotion, 5)]);

            //Tier 4

        }

        internal static void Unload()
        {
            GearLevels.Clear();
            GearLevels = null;
        }

        static GearLevelInfo AddGearLevel(float Power, int Headgear, int Armor, int Leggings)
        {
            GearLevelInfo NewInfo = new GearLevelInfo();
            NewInfo.AddLoadout(Headgear, Armor, Leggings);
            if(GearLevels.ContainsKey(Power))
                GearLevels[Power] = NewInfo;
            else
                GearLevels.Add(Power, NewInfo);
            return NewInfo;
        }
        
        public static void RandomizeCompanion(CompanionData Data)
        {
            if (!Data.IsGeneric) return;
            RandomizeCompanionGender(Data);
            float GearLevel = CalculateGearLevel(Data);
            RandomizeBasicStats(Data, GearLevel);
            RandomizeEquipments(Data, GearLevel);
            RandomizeCompanionLook(Data);
            RandomizeName(Data);
        }

        public static float CalculateGearLevel(CompanionData Data)
        {
            float GearLevel = 0.5f;
            if (NPC.downedBoss1 || NPC.downedSlimeKing)
            {
                GearLevel = 1.5f;
            }
            if (NPC.downedBoss2 || NPC.downedQueenBee || NPC.downedBoss3)
            {
                GearLevel = 2.5f;
            }
            if (Main.hardMode)
            {
                GearLevel = 3.3f;
            }
            if (NPC.downedMechBossAny)
            {
                GearLevel = 4.5f;
            }
            if (NPC.downedPlantBoss)
            {
                GearLevel = 5.5f;
            }
            if (NPC.downedGolemBoss)
            {
                GearLevel = 6.4f;
            }
            if (NPC.downedMoonlord)
            {
                GearLevel = 7.8f;
            }
            GearLevel += Main.rand.NextFloat(-.5f, .5f);
            return GearLevel;
        }

        public static void RandomizeName(Companion companion)
        {
            if (RandomizeName(companion.Data))
            {
                companion.name = companion.GetName;
            }
        }

        public static bool RandomizeName(CompanionData Data)
        {
            if (!Data.IsGeneric) return false;
            string NewName = Data.Base.NameGeneratorParameters(Data);
            if (NewName == "")
                NewName = Data.Base.DisplayName;
            Data.ChangeGenericName(NewName);
            return true;
        }

        public static void RandomizeCompanionGender(CompanionData Data)
        {
            if (!Data.IsGeneric) return;
            Data.Gender = Main.rand.NextBool(2)? Genders.Male : Genders.Female;
        }

        public static void RandomizeCompanionLook(CompanionData Data)
        {
            if (!Data.IsGeneric) return;
            GenericCompanionInfos info = Data.GetGenericCompanionInfo;
            List<int> ValidSets = new List<int>();
            bool[] MaleSets = PlayerVariantID.Sets.Male;
            for (int i = 0; i < MaleSets.Length; i++)
            {
                if ((Data.Gender == Genders.Genderless || MaleSets[i] == (Data.Gender == Genders.Male)) && i != PlayerVariantID.MaleDisplayDoll && i != PlayerVariantID.FemaleDisplayDoll)
                {
                    ValidSets.Add(i);
                }
            }
            if (ValidSets.Count > 0)
                info.SkinVariant = ValidSets[Main.rand.Next(ValidSets.Count)];
            ValidSets.Clear();
            info.HairStyle = Main.rand.Next(1, HairID.Count);
            RandomizeColor(ref info.HairColor);
            RandomizeColor(ref info.EyeColor);
            RandomizeSkin(ref info.SkinColor);
            RandomizeColor(ref info.PantsColor);
            RandomizeColor(ref info.ShirtColor);
            RandomizeColor(ref info.ShoesColor);
            RandomizeColor(ref info.UndershirtColor);
            info.HairColor.A = 255;
            RandomizeCompanionOutfit(Data);
        }

        static void RandomizeCompanionOutfit(CompanionData Data)
        {
            GenericCompanionInfos info = Data.GetGenericCompanionInfo;
            int HeadID = 0, BodyID = 0, LegsID = 0;
            if (MainMod.MrPlagueRacesInstalled)
            {
                if (Data.Equipments[1].type == ItemID.None) BodyID = ItemID.FamiliarShirt;
                if (Data.Equipments[2].type == ItemID.None) LegsID = ItemID.FamiliarPants;
            }
            int[] AccessoryID = new int[]{ 0, 0, 0, 0, 0, 0, 0};
            Data.Base.GenericModifyVanityGear(Data, ref HeadID, ref BodyID, ref LegsID, ref AccessoryID);
            Data.Equipments[10].SetDefaults(HeadID);
            Data.Equipments[11].SetDefaults(BodyID);
            Data.Equipments[12].SetDefaults(LegsID);
            for (int i = 0; i < 7; i++)
            {
                Data.Equipments[13 + i].SetDefaults(AccessoryID[i]);
            }
        }

        static void RandomizeSkin(ref Color color)
        {
            float Pow = System.Math.Min(.6f + Main.rand.NextFloat() * .6f, 1);
            color.R = (byte)(Main.rand.Next(240,255) * Pow);
            color.G = (byte)(Main.rand.Next(110, 140) * Pow);
            color.B = (byte)(Main.rand.Next(75, 110) * Pow);
            color.A = 255;
        }

        static void RandomizeColor(ref Color color)
        {
            color.R = (byte)Main.rand.Next(256);
            color.G = (byte)Main.rand.Next(256);
            color.B = (byte)Main.rand.Next(256);
            color.A = 255;
        }

        public static void RandomizeEquipments(CompanionData data, float GearLevel)
        {
            float LatestDistance = float.MaxValue;
            GearLevelInfo.LoadoutInfo PickedLoadout = null;
            foreach (float Power in GearLevels.Keys)
            {
                float Distance = System.MathF.Abs(Power - GearLevel);
                if (Distance < LatestDistance)
                {
                    if (GearLevels[Power].Loadouts.Count > 0)
                    {
                        PickedLoadout = GearLevels[Power].Loadouts[Main.rand.Next(GearLevels[Power].Loadouts.Count)];
                    }
                    LatestDistance = Distance;
                }
            }
            if (PickedLoadout != null)
            {
                GearLevelInfo.LoadoutInfo Loadout = PickedLoadout;
                data.Equipments[0].SetDefaults(Loadout.Headgear);
                data.Equipments[1].SetDefaults(Loadout.Armor);
                data.Equipments[2].SetDefaults(Loadout.Leggings);
                int slot = 0;
                foreach (ItemDefinition Item in Loadout.items)
                {
                    data.Inventory[slot].SetDefaults(Item.ItemType);
                    data.Inventory[slot].stack = Item.Count;
                    slot++;
                }
            }
        }

        public static void RandomizeBasicStats(CompanionData Data, float GearLevel)
        {
            int HealthIncrease = (int)MathF.Min(15, (int)((GearLevel - 1f) * 5 + Main.rand.Next(-2, 3)));
            int ManaIncrease = (int)MathF.Min(9, (int)((GearLevel - .5f) * 3 + Main.rand.Next(-2, 3)));
            int LifeFruitIncrease = 0;
            if (GearLevel >= 4.5f)
            {
                LifeFruitIncrease = (int)MathF.Min(20, (int)((GearLevel - 4.5f) * 5 + Main.rand.Next(-2, 3)));
            }
            Data.LifeCrystalsUsed = HealthIncrease;
            Data.LifeFruitsUsed = LifeFruitIncrease;
            Data.ManaCrystalsUsed = ManaIncrease;
        }

        public struct GearLevelInfo
        {
            public List<LoadoutInfo> Loadouts;

            public GearLevelInfo()
            {
                Loadouts = new List<LoadoutInfo>();
            }

            public LoadoutInfo AddLoadout(int Headgear, int Armor, int Leggings)
            {
                return AddLoadout(Headgear, Armor, Leggings, null);
            }

            public LoadoutInfo AddLoadout(ItemDefinition[] StarterItems)
            {
                LoadoutInfo Last = GetLatestLoadout();
                LoadoutInfo l = new LoadoutInfo() { Headgear = Last.Headgear, Armor = Last.Armor, Leggings = Last.Leggings, items = StarterItems };
                Loadouts.Add(l);
                return l;
            }

            public LoadoutInfo AddLoadout(int Headgear, int Armor, int Leggings, ItemDefinition[] StarterItems)
            {
                if (StarterItems == null) StarterItems = new ItemDefinition[0];
                LoadoutInfo l = new LoadoutInfo() { Headgear = Headgear, Armor = Armor, Leggings = Leggings, items = StarterItems };
                Loadouts.Add(l);
                return l;
            }

            public LoadoutInfo GetLatestLoadout()
            {
                if (Loadouts.Count == 0)
                {
                    AddLoadout(0, 0, 0);
                }
                return Loadouts[Loadouts.Count - 1];
            }

            public void AddLoadoutWeapons(ItemDefinition[] newitems)
            {
                GetLatestLoadout().ChangeItems(newitems);
            }

            public class LoadoutInfo
            {
                public ItemDefinition[] items;
                public int Headgear;
                public int Armor;
                public int Leggings;

                public LoadoutInfo()
                {
                    Headgear = 0;
                    Armor = 0;
                    Leggings = 0;
                    items = [];
                }

                public void ChangeItems(ItemDefinition[] newitems)
                {
                    items = newitems;
                }
            }
        }

        public struct ItemDefinition
        {
            public int ItemType;
            public int Count;

            public ItemDefinition(int Type)
            {
                ItemType = Type;
                Count = 1;
            }

            public ItemDefinition(int Type, int Count)
            {
                ItemType = Type;
                this.Count = Count;
            }
        }
    }
}