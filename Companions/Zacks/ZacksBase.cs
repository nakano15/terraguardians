using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class ZacksBase : TerraGuardianBase
    {
        public override string Name => "Zacks";
        public override string FullName => "Zackary Howler";
        public override string Description => "He didn't used to be a zombie, but something happened and he ended up in that state.\nHe's also Blue's boyfriend.";
        public override int Age => 16;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 104;
        public override bool CanCrouch => false;
        public override int Width => 30;
        public override int Height => 94;
        public override float Scale => 107f / 86;
        public override Sizes Size => Sizes.Large;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override bool CanUseHeavyItem => true;
        public override int InitialMaxHealth => 185; //1275
        public override int HealthPerLifeCrystal => 50;
        public override int HealthPerLifeFruit => 17;
        public override float AccuracyPercent => 0.32f;
        public override Genders Gender => Genders.Male;
        public override float MaxRunSpeed => 3.9f;
        public override float RunAcceleration => 0.12f;
        public override float RunDeceleration => 0.52f;
        public override int JumpHeight => 17;
        public override float JumpSpeed => 6.15f;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override bool IsNocturnal => false;
        public override bool SleepsWhenOnBed => false;
        public override bool DrawBehindWhenSharingBed => true;
        public override SoundStyle HurtSound => SoundID.NPCHit1;
        public override SoundStyle DeathSound => SoundID.ZombieMoan;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 2 };
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[] {
                new InitialItemDefinition(ItemID.BloodButcherer),
                new InitialItemDefinition(ItemID.HealingPotion, 10)
            };
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new ZacksDialogues();
        public override BehaviorBase PreRecruitmentBehavior => new terraguardians.Companions.Zacks.ZacksPreRecruitZombieBossBehavior();
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames {
            get{
                Animation a = new Animation();
                for(short i = 1; i <= 8; i++)
                    a.AddFrame(i, 24);
                return a;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(10);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation a = new Animation();
                for (short i = 14; i  <= 17; i++)
                    a.AddFrame(i);
                return a;
            }
        }
        protected override Animation SetSittingFrames => new Animation(18);
        protected override Animation SetChairSittingFrames => new Animation(19);
        protected override Animation SetThroneSittingFrames => new Animation(20);
        protected override Animation SetBedSleepingFrames => new Animation(21);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation a = new Animation();
                for (short i = 11; i <= 13; i++) a.AddFrame(i);
                return a;
            }
        }
        protected override Animation SetRevivingFrames => new Animation(22);
        protected override Animation SetDownedFrames => new Animation(23);
        protected override Animation SetPetrifiedFrames => new Animation(24);
        protected override Animation SetBackwardStandingFrames => new Animation(25);
        protected override Animation SetBackwardReviveFrames => new Animation(26);
        #endregion
        #region Animation Positions
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer a = new AnimationFrameReplacer();
                a.AddFrameToReplace(18, 0);
                a.AddFrameToReplace(19, 0);
                return a;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[2];
                //Left Hand
                Hands[0] = new AnimationPositionCollection(17, 32, true);
                Hands[0].AddFramePoint2X(11, 27, 8);
                Hands[0].AddFramePoint2X(12, 38, 19);
                Hands[0].AddFramePoint2X(13, 40, 42);

                Hands[0].AddFramePoint2X(14, 21, 6);
                Hands[0].AddFramePoint2X(15, 33, 16);
                Hands[0].AddFramePoint2X(16, 37, 26);
                Hands[0].AddFramePoint2X(17, 32, 34);

                Hands[0].AddFramePoint2X(22, 38, 43);

                //Right Hand
                Hands[1] = new AnimationPositionCollection(30, 32, true);
                Hands[1].AddFramePoint2X(11, 30, 8);
                Hands[1].AddFramePoint2X(12, 42, 19);
                Hands[1].AddFramePoint2X(13, 43, 41);

                Hands[1].AddFramePoint2X(14, 24, 6);
                Hands[1].AddFramePoint2X(15, 37, 16);
                Hands[1].AddFramePoint2X(16, 40, 26);
                Hands[1].AddFramePoint2X(17, 36, 34);
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(16, 19, true);
                a.AddFramePoint2X(12, 24, 19);
                a.AddFramePoint2X(13, 27, 25);
                return a;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition => new AnimationPositionCollection(22, 36, true);
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(21, 10, true);
                a.AddFramePoint2X(12, 30 - 2, 14 + 2);
                a.AddFramePoint2X(13, 33 - 2, 21 + 2);
                
                a.AddFramePoint2X(20, 21 + 1, 8);
                
                a.AddFramePoint2X(22, 35, 39);
                return a;
            }
        }
        protected override AnimationPositionCollection SetWingPosition => new AnimationPositionCollection(22, 21, true);
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16, true);
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(5, -4, true);
                a.AddFramePoint2X(20, -10, -16);
                return a;
            }
        }
        #endregion
    }
}