using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.IO;
using Terraria.Audio;
using ReLogic.Utilities;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians
{
    public class ProjMod : GlobalProjectile
    {
        public static bool IsThisCompanionProjectile(int proj, Companion companion)
        {
            return IsThisCompanionProjectile(Main.projectile[proj], companion);
        }

        public static bool IsThisCompanionProjectile(Projectile proj, Companion companion)
        {
            return proj.whoAmI == companion.whoAmI; //Need to rework this when proper script to recognize companions is implemented.
        }
    }
}