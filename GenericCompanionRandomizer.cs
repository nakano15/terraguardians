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
            Data.ChangeName(NewName);
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
            TerrarianCompanionInfo info = Data.GetGenericCompanionInfo;
            List<int> ValidSets = new List<int>();
            bool[] MaleSets = Terraria.ID.PlayerVariantID.Sets.Male;
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
            info.HairStyle = Main.rand.Next(1, Terraria.ID.HairID.Count);
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
            if (!Data.IsGeneric) return;
            TerrarianCompanionInfo info = Data.GetGenericCompanionInfo;
            int HeadID = 0, BodyID = 0, LegID = 0;
            int[] AccessoryID = new int[]{0,0,0,0,0,0,0};
            Data.Base.GenericModifyVanityGear(Data, ref HeadID, ref BodyID, ref LegID, ref AccessoryID);
            Data.Equipments[10].SetDefaults(HeadID);
            Data.Equipments[11].SetDefaults(BodyID);
            Data.Equipments[12].SetDefaults(LegID);
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
            int[] Accessories = new int[] { 0, 0, 0, 0, 0, 0};
            
        }
    }
}