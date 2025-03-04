using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class MichelleBase : TerrarianBase
    {
        public override string Name => "Michelle";
        public override string Description => "Your personal TerraGuardians fan girl.";
        public override int Age => 16;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Summer, 16);
        public override Genders Gender => Genders.Female;
        public override float AccuracyPercent => 0.27f;
        public override int FavoriteFood => ItemID.IceCream;
        protected override CompanionDialogueContainer GetDialogueContainer => new MichelleDialogues();
        protected override TerrarianCompanionInfo SetTerrarianCompanionInfo
        {
            get
            {
                return new TerrarianCompanionInfo()
                {
                    HairStyle = 21,
                    SkinVariant = 1,
                    HairColor = new Color(55, 215, 255),
                    EyeColor = new Color(196, 10, 227),
                    SkinColor = new Color(237, 118, 85),
                    ShirtColor = new Color(248, 28, 28),
                    UndershirtColor = new Color(30, 249, 20),
                    PantsColor = new Color(179, 36, 245),
                    ShoesColor = new Color(206, 29, 29)
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
        public override bool CanSpawnNpc()
        {
            return MainMod.GetLocalPlayer.statDefense > 0;
        }
    }
}