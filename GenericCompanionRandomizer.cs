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
            RandomizeName(Data);
            RandomizeCompanionGender(Data);
            RandomizeCompanionLook(Data);
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
            Data.ChangeName(Data.Base.NameGeneratorParameters(Data));
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
                if (MaleSets[i] == (Data.Gender == Genders.Male) && i != Terraria.ID.PlayerVariantID.MaleDisplayDoll && i != Terraria.ID.PlayerVariantID.FemaleDisplayDoll)
                {
                    ValidSets.Add(i);
                }
            }
            if (ValidSets.Count > 0)
                info.SkinVariant = ValidSets[Main.rand.Next(ValidSets.Count)];
            ValidSets.Clear();
            info.HairStyle = Main.rand.Next(1, Terraria.ID.HairID.Count);
            RandomizeColor(ref info.EyeColor);
            RandomizeColor(ref info.HairColor);
            RandomizeColor(ref info.SkinColor);
            RandomizeColor(ref info.PantsColor);
            RandomizeColor(ref info.ShirtColor);
            RandomizeColor(ref info.ShoesColor);
            RandomizeColor(ref info.UndershirtColor);
            info.HairColor.A = 255;
        }

        static void RandomizeColor(ref Color color)
        {
            color.R = (byte)Main.rand.Next(256);
            color.G = (byte)Main.rand.Next(256);
            color.B = (byte)Main.rand.Next(256);
        }
    }
}