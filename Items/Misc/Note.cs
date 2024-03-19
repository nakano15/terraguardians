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
            // Tooltip.SetDefault("This is the portrait of someone dear to you.\nUsing it allows you to pick a known companion as your Buddy.\nYou have 12 in-game minutes to use this item.");
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