using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class QuentinBase : TerrarianBase
    {
        public override string Name => "Quentin";
        public override string Description => "He is a young green bunny who dreams of becoming a powerful wizard one day, his hobbies are reading fiction books and telling stories.";
        public override int Age => 15;
        public override Genders Gender => Genders.Male;
        public override float AccuracyPercent => 0.32f;
        public override int InitialMaxMana => 50;
        public override int HealthPerLifeCrystal => 15;
        public override int HealthPerLifeFruit => 10;
        public override int ManaPerManaCrystal => 25;
        protected override CompanionDialogueContainer GetDialogueContainer => new QuentinDialogues();
        protected override TerrarianCompanionInfo SetTerrarianCompanionInfo
        {
            get
            {
                return new TerrarianCompanionInfo()
                {
                    HairStyle = 15,
                    SkinVariant = 0,
                    HairColor = new Color(153, 229, 80),
                    EyeColor = new Color(0, 0, 0),
                    SkinColor = new Color(153, 229, 80),
                    ShirtColor = new Color(153, 229, 80),
                    UndershirtColor = new Color(153, 229, 80),
                    PantsColor = new Color(153, 229, 80),
                    ShoesColor = new Color(153, 229, 80)
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