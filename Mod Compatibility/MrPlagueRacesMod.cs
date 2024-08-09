using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.ModCompatibility
{
    internal class MrPlagueRacesMod
    {
        const string ModName = "MrPlagueRaces";
        static Mod mod;
        public static bool HasMrPlagueMod => mod != null;

        internal static void Load()
        {
            ModLoader.TryGetMod(ModName, out mod);
        }

        internal static void Unload()
        {
            mod = null;
        }
    }
}