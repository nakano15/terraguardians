using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Terraria.ModLoader.IO;

namespace terraguardians.Items.Consumables
{
    public class PortraitOfAFriend : ModItem
    {
        public override void SetStaticDefaults()
        {
            Tooltip.SetDefault("This is the portrait of someone dear to you.\nUsing it allows you to pick a known companion as your Buddy.\nYou have 12 in-game minutes to use this item.");
        }
        
        public override void SetDefaults()
        {
            Item.UseSound = Terraria.ID.SoundID.Item4;
            Item.useStyle = 4;
            Item.useAnimation = 30;
            Item.useTime = 30;
            Item.maxStack = 1;
            Item.consumable = true;
            Item.value = 0;
            Item.width = 24;
            Item.height = 16;
            Item.rare = 7;
        }

        public override bool CanUseItem(Player player)
        {
            if (!BuddyModeSetupInterface.IsActive)
                BuddyModeSetupInterface.Open();
            return base.CanUseItem(player);
        }

        public override void UpdateInventory(Player player)
        {
            if (player == MainMod.GetLocalPlayer && Main.ActivePlayerFileData.GetPlayTime() >= System.TimeSpan.FromMinutes(12))
            {
                Item.SetDefaults(0);
                Main.NewText("The Portrait of a Friend has suddenly disappeared from your inventory.", Microsoft.Xna.Framework.Color.PaleVioletRed);
            }
        }
    }
}