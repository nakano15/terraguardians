using Terraria;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;

namespace terraguardians
{
    public struct FurnitureInfo
    {
        public ushort FurnitureID;
        public int FurnitureX, FurnitureY;
        public bool FacingLeft;

        public FurnitureInfo(ushort FID, int FX, int FY, bool FacingLeft = false)
        {
            FurnitureID = FID;
            FurnitureX = FX;
            FurnitureY = FY;
            this.FacingLeft = FacingLeft;
        }
    }
}