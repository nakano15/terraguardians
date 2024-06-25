using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.ModCompatibility
{
    internal class ThoriumModCompatibility
    {
        const string ModName = "ThoriumMod";
        public static Mod ThoriumMod;
        public static DamageClass BardDamage = null, HealerDamage = null;

        public static void Load()
        {
            if (ModLoader.HasMod(ModName))
            {
                ThoriumMod = ModLoader.GetMod(ModName);
                if (ThoriumMod.TryFind<DamageClass>("BardDamage", out BardDamage))
                {
                    
                }
                if (ThoriumMod.TryFind<DamageClass>("HealerTool", out HealerDamage))
                {
                    
                }
            }
        }

        public static void Unload()
        {
            ThoriumMod = null;
            BardDamage = null;
            HealerDamage = null;
        }
    }
}