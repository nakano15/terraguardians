using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.ModCompatibility
{
    internal class CalamityModCompatibility
    {
        const string ModName = "CalamityMod";
        public static Mod CalamityMod;
        public static DamageClass TrueMeleeDamage = null, RogueDamage = null;

        internal static void Load()
        {
            if (ModLoader.HasMod(ModName))
            {
                CalamityMod = ModLoader.GetMod(ModName);
                CalamityMod.TryFind<DamageClass>("RogueDamageClass", out RogueDamage);
                CalamityMod.TryFind<DamageClass>("TrueMeleeDamageClass", out TrueMeleeDamage);
            }
        }

        internal static void Unload()
        {
            CalamityMod = null;
            RogueDamage = null;
            TrueMeleeDamage = null;
        }
    }
}