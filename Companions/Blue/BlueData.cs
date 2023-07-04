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
    public class BlueData : CompanionData
    {
        public bool HasBunny = false;
        private uint DelayBeforePickingUpLeopoldAgain = 0;

        public BlueData(uint ID = 0, string ModID = "", uint Index = 0) :
            base(ID, ModID, Index)
        {
            
        }

        public bool HasBunnyInInventory()
        {
            for(int i = 0; i < 50; i++)
            {
                if(Inventory[i].type == 0) continue;
                switch(Inventory[i].type)
                {
                    case ItemID.Bunny:
                    case ItemID.GemBunnyAmber:
                    case ItemID.GemBunnyAmethyst:
                    case ItemID.GemBunnyDiamond:
                    case ItemID.GemBunnyEmerald:
                    case ItemID.GemBunnyRuby:
                    case ItemID.GemBunnySapphire:
                    case ItemID.GemBunnyTopaz:
                    case ItemID.GoldBunny:
                        return true;
                }
            }
            return false;
        }

        public bool CanPickupLeopold
        {
            get
            {
                return DelayBeforePickingUpLeopoldAgain == 0;
            }
        }

        internal void UpdateBlueData(Companion companion)
        {
            HasBunny = HasBunnyInInventory();
            if (DelayBeforePickingUpLeopoldAgain > 0)
                DelayBeforePickingUpLeopoldAgain--;
        }

        internal void SetLeopoldPickupCooldown()
        {
            DelayBeforePickingUpLeopoldAgain = (uint)(Main.rand.Next(14, 19) * 60 * 60);
        }
    }
}