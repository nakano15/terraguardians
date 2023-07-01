using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class VladimirBase : TerraGuardianBase
    {
        public override string Name => "Vladimir";
        public override string FullName => "Vladimir Svirepyy Varvar"; //Surnames means Ferocious Barbarian
        public override string Description => "A bear TerraGuardian that likes giving hugs to people.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 44;
        public override int Height => 116;
    }
}