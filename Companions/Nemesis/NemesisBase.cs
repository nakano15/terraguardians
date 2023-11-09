using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class NemesisBase : TerrarianBase
    {
        public override string Name => "Nemesis";
        public override string Description => "It's cryptic to know who the Nemesis is, or was.\nNeither if is a \"he\" or a \"she\".";
        public override int Age => 256;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Autumn, 3);
        public override bool IsNocturnal => true;
        public override bool SleepsWhenOnBed => false;
        public override Genders Gender => Genders.Male;
        public override bool CanChangeGenders => true;
        public override bool RandomGenderOnSpawn => true;
        public override float AccuracyPercent => 0.32f;
        protected override TerrarianCompanionInfo SetTerrarianCompanionInfo
        {
            get
            {
                return new TerrarianCompanionInfo()
                {
                    HairStyle = 15,
                    EyeColor = Color.Red,
                    SkinColor = new Color(39, 39, 39)
                };
            }
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new NemesisDialogues();
        public override SoundStyle HurtSound => SoundID.NPCHit54;
        public override SoundStyle DeathSound => SoundID.NPCDeath52;
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.WoodenSword),
                new InitialItemDefinition(ItemID.HealingPotion, 10)
            };
        }
        public override BehaviorBase PreRecruitmentBehavior => new PreRecruitNoMonsterAggroBehavior();
        public override bool CanSpawnNpc()
        {
            return Main.hardMode;
        }

        public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            if (MainMod.NemesisFadeEffect < 0)
            {
                drawSet.colorBodySkin.R = 0;
                drawSet.colorBodySkin.G = 0;
                drawSet.colorBodySkin.B = 0;
                drawSet.colorHead.R = 0;
                drawSet.colorHead.G = 0;
                drawSet.colorHead.B = 0;
            }
            else
            {
                float Percentage = 1f;
                if (MainMod.NemesisFadeEffect < MainMod.NemesisFadingTime * 0.3f)
                {
                    Percentage = MainMod.NemesisFadeEffect / (MainMod.NemesisFadingTime * 0.3f);
                }
                else if (MainMod.NemesisFadeEffect >= MainMod.NemesisFadingTime * 0.7f)
                {
                    Percentage = 1f - (MainMod.NemesisFadeEffect - (MainMod.NemesisFadingTime * 0.7f)) / (MainMod.NemesisFadingTime * 0.3f);
                }
                drawSet.colorBodySkin.R = (byte)(drawSet.colorBodySkin.R * Percentage);
                drawSet.colorBodySkin.G = (byte)(drawSet.colorBodySkin.G * Percentage);
                drawSet.colorBodySkin.B = (byte)(drawSet.colorBodySkin.B * Percentage);
                drawSet.colorHead.R = (byte)(drawSet.colorHead.R * Percentage);
                drawSet.colorHead.G = (byte)(drawSet.colorHead.G * Percentage);
                drawSet.colorHead.B = (byte)(drawSet.colorHead.B * Percentage);
            }
            drawSet.colorEyeWhites = drawSet.colorHead;
        }
    }
}