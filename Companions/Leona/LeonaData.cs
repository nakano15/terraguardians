using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.ModLoader.IO;
using Terraria.DataStructures;

namespace terraguardians.Companions.Leona
{
    public class LeonaData : CompanionData
    {
        protected override uint CustomSaveVersion => 1;

        public bool HoldingSword = true;

        public override void CustomSave(TagCompound save, uint UniqueID)
        {
            save.Add("LeonaSword_" + UniqueID, HoldingSword);
        }

        public override void CustomLoad(TagCompound tag, uint UniqueID, uint LastVersion)
        {
            if (LastVersion >= 1)
                HoldingSword = tag.GetBool("LeonaSword_" + UniqueID);
        }
    }
}