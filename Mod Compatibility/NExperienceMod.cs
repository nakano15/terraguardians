using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.ModCompatibility
{
    internal class NExperienceModCompatibility
    {
        const string ModName = "nexperience1dot4";
        internal static Mod NExperienceMod;
        private static Func<Player, int> GetPlayerLevelFunc;
        private static Action<Player, int> SetPlayerLevelFunc;
        private static int LoggedPlayerLevel = 1;

        internal static void Load()
        {
            if (ModLoader.HasMod(ModName))
            {
                NExperienceMod = ModLoader.GetMod(ModName);
                GetPlayerLevelFunc = (Func<Player, int>)NExperienceMod.Call("GetPlayerLevelFunc");
                SetPlayerLevelFunc = (Action<Player, int>)NExperienceMod.Call("SetPlayerLevelFunc");
            }
        }

        internal static void Unload()
        {
            NExperienceMod = null;
            GetPlayerLevelFunc = null;
            SetPlayerLevelFunc = null;
        }

        internal static void CheckCompanionLevel(Player companion)
        {
            if (NExperienceMod == null) return;
            int Level = GetPlayerLevelFunc(companion);
            if (Level != LoggedPlayerLevel)
            {
                SetPlayerLevelFunc(companion, LoggedPlayerLevel);
                NExperienceMod.Call("NormalizeStatusPointsInvested", companion);
            }
        }

        internal static void UpdatePlayerLevel(Player player)
        {
            if (NExperienceMod == null) return;
            LoggedPlayerLevel = GetPlayerLevelFunc(player);
        }
    }
}