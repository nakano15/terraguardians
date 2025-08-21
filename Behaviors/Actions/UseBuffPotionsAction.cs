using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Behaviors.Actions
{
    public class UseBuffPotionsAction : BehaviorBase
    {
        int LastBuffSlot = 0;

        public override void Update(Companion companion)
        {
            if (companion.KnockoutStates > 0) 
            {
                Deactivate();
                return;
            }
            if (companion.itemAnimation == 0)
            {
                if(LastBuffSlot >= 50)
                    Deactivate();
                else
                {
                    if(companion.inventory[LastBuffSlot].type > ItemID.None && companion.inventory[LastBuffSlot].consumable && companion.inventory[LastBuffSlot].buffType > -1 && companion.inventory[LastBuffSlot].buffType != BuffID.PotionSickness && companion.inventory[LastBuffSlot].buffType != BuffID.ManaSickness)
                    {
                        companion.selectedItem = LastBuffSlot;
                        companion.ControlAction = true;
                    }
                    LastBuffSlot++;
                }
            }
        }
    }
}