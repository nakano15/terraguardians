using Terraria;
using terraguardians.Personalities;

namespace terraguardians
{
    public class PersonalityDB
    {
        public static PersonalityBase Neutral, Friendly;

        internal static void Load()
        {
            Neutral = new NeutralPersonality();
            Friendly = new FriendlyPersonality();
        }

        internal static void Unload()
        {
            Neutral = null;
            Friendly = null;
        }
    }
}