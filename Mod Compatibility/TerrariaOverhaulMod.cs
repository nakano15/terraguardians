using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.ModCompatibility
{
    internal class TerrariaOverhaulMod
    {
        const string ModName = "TerrariaOverhaul";
        internal static Mod mod;

        internal static void Load()
        {
            ModLoader.TryGetMod(ModName, out mod);
        }

        internal static void Unload()
        {
            mod = null;
        }

        internal static void OverhaulDirectionFixingTrick()
        {
            if (mod == null) return;
            
        }
    }
}