using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians
{
    public class GenericCompanionRandomizer
    {
        static Dictionary<float, GearLevelInfo> GearLevels = new Dictionary<float, GearLevelInfo>();

        internal static void Initialize()
        {
            AddGearLevel(0f, 0, 0, 0);
            AddGearLevel(.1f, ItemID.WoodHelmet, ItemID.WoodBreastplate, ItemID.WoodGreaves);
            AddGearLevel(.3f, ItemID.CopperHelmet, ItemID.CopperChainmail, ItemID.CopperGreaves);
            AddGearLevel(.4f, ItemID.TinHelmet, ItemID.TinChainmail, ItemID.TinGreaves);
            AddGearLevel(.5f, ItemID.IronHelmet, ItemID.IronChainmail, ItemID.IronGreaves);
            AddGearLevel(.6f, ItemID.LeadHelmet, ItemID.LeadChainmail, ItemID.LeadGreaves);
            AddGearLevel(.7f, ItemID.SilverHelmet, ItemID.SilverChainmail, ItemID.SilverGreaves);
            AddGearLevel(.8f, ItemID.TungstenHelmet, ItemID.TungstenChainmail, ItemID.TungstenGreaves);
            AddGearLevel(.9f, ItemID.GoldHelmet, ItemID.GoldChainmail, ItemID.GoldGreaves);
            AddGearLevel(1.0f, ItemID.PlatinumHelmet, ItemID.PlatinumChainmail, ItemID.PlatinumGreaves);
            AddGearLevel(2.4f, ItemID.ShadowHelmet, ItemID.ShadowScalemail, ItemID.ShadowGreaves);
            AddGearLevel(2.6f, ItemID.CrimsonHelmet, ItemID.CrimsonScalemail, ItemID.CrimsonGreaves);
        }

        internal static void Unload()
        {
            GearLevels.Clear();
            GearLevels = null;
        }

        static void AddGearLevel(float Power, int Headgear, int Armor, int Leggings)
        {
            GearLevelInfo NewInfo = new GearLevelInfo(){ Headgear = Headgear, Armor = Armor, Leggings = Leggings };
            if(GearLevels.ContainsKey(Power))
                GearLevels[Power] = NewInfo;
            else
                GearLevels.Add(Power, NewInfo);
        }
        
        public static void RandomizeCompanion(CompanionData Data)
        {
            if (!Data.IsGeneric) return;
            RandomizeCompanionGender(Data);
            RandomizeCompanionLook(Data);
            RandomizeName(Data);
            RandomizeEquipments(Data);
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
            Data.Gender = Main.rand.Next(2) == 0 ? Genders.Male : Genders.Female;
        }

        public static void RandomizeCompanionLook(CompanionData Data)
        {
            if (!Data.IsGeneric) return;
            GenericCompanionInfos info = Data.GetGenericCompanionInfo;
            List<int> ValidSets = new List<int>();
            bool[] MaleSets = PlayerVariantID.Sets.Male;
            for (int i = 0; i < MaleSets.Length; i++)
            {
                if ((Data.Gender == Genders.Genderless || MaleSets[i] == (Data.Gender == Genders.Male)) && i != Terraria.ID.PlayerVariantID.MaleDisplayDoll && i != Terraria.ID.PlayerVariantID.FemaleDisplayDoll)
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
                if (Data.Equipments[1].type == 1) BodyID = ItemID.FamiliarShirt;
                if (Data.Equipments[2].type == 1) LegsID = ItemID.FamiliarPants;
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

        public static void RandomizeEquipments(CompanionData data)
        {
            int Headgear = 0, Armor = 0, Leggings = 0;
            int[] Accessories = new int[] { 0, 0, 0, 0, 0, 0, 0};
            float GearLevel = 0.3f;
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
            GearLevel += Main.rand.NextFloat(-5f, 5f);
            float LatestDistance = float.MaxValue;
            foreach (float Power in GearLevels.Keys)
            {
                float Distance = System.MathF.Abs(Power - GearLevel);
                if (Distance < LatestDistance)
                {
                    Headgear = GearLevels[Power].Headgear;
                    Armor = GearLevels[Power].Armor;
                    Leggings = GearLevels[Power].Leggings;
                    LatestDistance = Distance;
                }
            }
            data.Equipments[0].SetDefaults(Headgear);
            data.Equipments[1].SetDefaults(Armor);
            data.Equipments[2].SetDefaults(Leggings);
        }

        struct GearLevelInfo
        {
            public int Headgear;
            public int Armor;
            public int Leggings;
        }
    }
}