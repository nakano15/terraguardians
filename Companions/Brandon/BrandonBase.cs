using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{/*i Saw him in terrarian companion file in 1.3 ver.i ported all his stats from that file.*/
    /// <summary>
    /// -Our antagonist, or is It?
    /// -Zacks old partner.
    /// </summary>
    public class BrandonBase : TerrarianBase
    {
        public override string Name => "Brandon";
        public override string Description => "";
        public override int Age => 21;
        public override Genders Gender => Genders.Male;
        public override float AccuracyPercent => 0.59f;
        //protected override CompanionDialogueContainer GetDialogueContainer => new BrandonDialogues();
        protected override TerrarianCompanionInfo SetTerrarianCompanionInfo
        {
            get
            {
                return new TerrarianCompanionInfo()
                {
                    HairStyle = 20,
                    SkinVariant = 3,
                    HairColor = new Color(145, 41, 229),
                    EyeColor = new Color(47, 157, 198),
                    SkinColor = new Color(255, 159, 133),
                    ShirtColor = new Color(27, 112, 198),
                    UndershirtColor = new Color(234, 67, 157),
                    PantsColor = new Color(66, 210, 0),
                    ShoesColor = new Color(160, 105, 60)
                };
            }
        }
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.WoodenSword),
                new InitialItemDefinition(ItemID.HealingPotion, 10)
            };
        }
    }
}