using Terraria;
using terraguardians.Personalities;

namespace terraguardians
{
    public class PersonalityDB
    {
        public static PersonalityBase Neutral, Friendly, Tough;

        internal static void Load()
        {
            Neutral = new NeutralPersonality();
            Friendly = new FriendlyPersonality();
            Tough = new ToughPersonality();
        }

        internal static void Unload()
        {
            Neutral = null;
            Friendly = null;
            Tough = null;
        }
    }
}