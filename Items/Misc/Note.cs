using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;

namespace terraguardians.Items.Misc
{
    public class Note : ModItem
    {
        public override void SetStaticDefaults()
        {
            
        }
        
        public override void SetDefaults()
        {
            Item.maxStack = 1;
            Item.consumable = false;
            Item.value = 0;
            Item.width = 24;
            Item.height = 16;
            Item.rare = 7;
        }
    }
}