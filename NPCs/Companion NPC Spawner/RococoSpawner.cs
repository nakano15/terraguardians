using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;

namespace terraguardians
{
    public class RococoSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(0);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.Water && !MainMod.HasCompanionInWorld(ToSpawnID) && !WorldMod.HasMetCompanion(ToSpawnID) && 
                Main.time >= 27000 && Main.time < 48600)
            {
                return ((float)Main.time - 27000) * (1f / 432000 * 0.5f);
            }
            return 0;
        }
    }
}