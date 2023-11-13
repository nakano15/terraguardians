using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using terraguardians.Companions.Wrath;

namespace terraguardians.Companions
{
    public class WrathBase : PigGuardianFragmentPiece
    {
        readonly string[] Names = new string[]{  "Wrath", "Rage", "Fury", "Irk" };
        public override string Name => "Wrath";
        public override string[] PossibleNames => Names;
        public override string Description => "The angry emotional pig piece\nof a TerraGuardian. Very volatile.";
        public override int Width => 20;
        public override int Height => 54;
        public override int SpriteWidth => 70;
        public override int SpriteHeight => 68;
        public override int FramesInRow => 28;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int InitialMaxHealth => 110; //320
        public override int HealthPerLifeCrystal => 15;
        public override int HealthPerLifeFruit => 5;
        public override float AccuracyPercent => .67f;
        public override float MaxFallSpeed => .4f;
        public override float MaxRunSpeed => 3.62f;
        public override float RunAcceleration => 0.12f;
        public override float RunDeceleration => 0.35f;
        public override int JumpHeight => 12;
        public override float JumpSpeed => 9.76f;
        protected override CompanionDialogueContainer GetDialogueContainer => new WrathDialogue();

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(Terraria.ID.ItemID.RedPhaseblade, 1),
                new InitialItemDefinition(Terraria.ID.ItemID.LesserHealingPotion, 10)
            };
        }

        protected override SubAttackBase[] GetDefaultSubAttacks()
        {
            return new SubAttackBase[]{ new WrathBodySlamAttack(), new WrathDestructiveRushAttack() };
        }

        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(0, 0);
                right.AddFrameToReplace(1, 0);
                right.AddFrameToReplace(2, 1);
                right.AddFrameToReplace(3, 2);
                right.AddFrameToReplace(4, 2);
                right.AddFrameToReplace(5, 1);
                right.AddFrameToReplace(6, 0);
                right.AddFrameToReplace(7, 0);
                right.AddFrameToReplace(8, 0);
                return new AnimationFrameReplacer[]{ left, right };
            }
        }

        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 11, 4);
                left.AddFramePoint2X(11, 23, 11);
                left.AddFramePoint2X(12, 24, 19);
                left.AddFramePoint2X(13, 22, 24);
                
                left.AddFramePoint2X(17, 25, 28);

                right.AddFramePoint2X(10, 15, 4);
                right.AddFramePoint2X(11, 25, 11);
                right.AddFramePoint2X(12, 27, 19);
                right.AddFramePoint2X(13, 23, 24);
                
                right.AddFramePoint2X(17, 27, 28);

                return new AnimationPositionCollection[]{ left, right };
            }
        }

        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(18, 11, true);
                anim.AddFramePoint2X(14, 18, 9);
                anim.AddFramePoint2X(17, 25, 18);
                anim.AddFramePoint2X(22, 18, 9);
                anim.AddFramePoint2X(25, 25, 18);
                return anim;
            }
        }

        protected override void SetupSkinsOutfitsContainer(ref Dictionary<byte, CompanionSkinInfo> Skins, ref Dictionary<byte, CompanionSkinInfo> Outfits)
        {
            Skins.Add(1, new Wrath.DevilOutfit());
        }
    }
}