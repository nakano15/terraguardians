using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians
{
    public class TerraGuardianBase : CompanionBase
    {
        public override CompanionTypes CompanionType => CompanionTypes.TerraGuardian;
        public override CompanionGroup GetCompanionGroup => MainMod.GetTerraGuardiansGroup;
        public override Companion GetCompanionObject => new TerraGuardian();
    }
}