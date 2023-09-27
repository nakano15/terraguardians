using Terraria;
using terraguardians.Personalities;

namespace terraguardians
{
    public class PersonalityDB
    {
        public static PersonalityBase Neutral;

        internal static void Load()
        {
            Neutral = new NeutralPersonality();
        }

        internal static void Unload()
        {
            Neutral = null;
        }
    }
}