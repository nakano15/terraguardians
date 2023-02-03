using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians.Items
{
    public class GuardianItemPrefab : ModItem
    {
        public Vector2 ItemOrigin = Vector2.Zero;
        public Vector2 ShotSpawnPosition = Vector2.Zero;
        public float ProjectileFallRate = 0f;
        public byte BlockRate = 0;
        public ItemType itemType = ItemType.None;

        protected override bool CloneNewInstances => true;

        public static ItemType GetItemType(Item i)
        {
            if (i.ModItem is GuardianItemPrefab)
                return (i.ModItem as GuardianItemPrefab).itemType;
            return ItemType.None;
        }

        public bool PlayerCanUseItem(Player player)
        {
            return (itemType == ItemType.PlayerCanUse || (player is TerraGuardian && (itemType == ItemType.Heavy && ((TerraGuardian)player).Base.CanUseHeavyItem)));
        }

        public override bool CanUseItem(Player player)
        {
            if(PlayerCanUseItem(player))
                return true;
            if(!(player is TerraGuardian) && player.whoAmI == MainMod.MyPlayerBackup)
            {
                switch(itemType)
                {
                    case ItemType.Heavy:
                        Main.NewText("This item is too heavy for me.");
                        break;
                    case ItemType.Shield:
                    case ItemType.OffHand:
                        Main.NewText("I can't use this.");
                        break;
                }
            }
            return false;
        }

        public override bool CanEquipAccessory(Player player, int slot, bool modded)
        {
            if(PlayerCanUseItem(player))
                return true;
            Main.NewText("Can't use this.");
            return false;
        }

        public override void ModifyTooltips(List<TooltipLine> tooltips)
        {
            if(itemType != ItemType.PlayerCanUse)
            {
                tooltips.Insert(1, new TooltipLine(Mod, "PlayerCantUseTooltip", "TerraGuardian Item"));
            }
            if(itemType == ItemType.Heavy)
            {
                tooltips.Insert(1, new TooltipLine(Mod, "HeavyItem", "Heavy Item"));
            }
        }

        public enum ItemType : byte
        {
            None = 0,
            Heavy = 1,
            Shield = 2,
            OffHand = 3,
            PlayerCanUse = 4
        }
    }
}