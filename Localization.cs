using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.Localization;

namespace terraguardians
{
    public class Localization
    {
        public static string GetTranslation(LocalizationKeys key)
        {
            return Language.GetTextValue("Mods." + MainMod.GetModName + ".Others." + key.ToString());
        }

        public enum LocalizationKeys: byte
        {
            male,
            female,
            he,
            she,
            him,
            her,
            they,
            them
        }
    }
}