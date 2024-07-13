using Terraria;
using Terraria.ModLoader;


namespace terraguardians.Companions
{
    public class CinnamonBase : TerraGuardianBase
    {
        public override string Name => "Cinnamon";
        public override string[] PossibleNames => new string[] { "Cinnamon", "Canela" };
        public override string Description => "A food enthusiast who is travelling worlds,\nseeking the best seasonings for food.";
        public override Sizes Size => Sizes.Medium;
        public override int Width => 24;
        public override int Height => 68;
        public override int CrouchingHeight => 60;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override float Scale => 59f / 68;
        public override int Age => 13;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Spring, 28);
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 160; //860
        public override int HealthPerLifeCrystal => 20;
        public override int HealthPerLifeFruit => 20;
        public override float AccuracyPercent => 0.43f;
        public override float MaxFallSpeed => .36f;
        public override float RunAcceleration => .22f;
        public override float RunDeceleration => .26f;
        public override int JumpHeight => 18;
        public override float JumpSpeed => 7.19f;
        public override bool CanCrouch => true;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 4, MoveInUnlock = 3, MountUnlock = 6 };

        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(Terraria.ID.ItemID.RedRyder),
                new InitialItemDefinition(Terraria.ID.ItemID.LesserHealingPotion, 5),
                new InitialItemDefinition(Terraria.ID.ItemID.CookedFish, 3),
                new InitialItemDefinition(Terraria.ID.ItemID.MusketBall, 250)
            };
        }

        #region Animation
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 2; i <= 9; i++)
                {
                    anim.AddFrame(i, 24);
                }
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(10);
        protected override Animation SetPlayerMountedArmFrame => new Animation(23);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 11; i < 15; i++)
                {
                    anim.AddFrame(i);
                }
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(18);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 20; i < 23; i++)
                {
                    anim.AddFrame(i);
                }
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(16);
        protected override Animation SetChairSittingFrames => new Animation(15);
        protected override Animation SetThroneSittingFrames => new Animation(24);
        protected override Animation SetBedSleepingFrames => new Animation(25);
        protected override Animation SetRevivingFrames => new Animation(19);
        protected override Animation SetDownedFrames => new Animation(17);
        protected override Animation SetBackwardStandingFrames => new Animation(26);
        protected override Animation SetBackwardReviveFrames => new Animation(27);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(15, 0);
                anim.AddFrameToReplace(16, 0);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(),
                    right = new AnimationFrameReplacer();
                right.AddFrameToReplace(15, 0);
                right.AddFrameToReplace(18, 1);
                right.AddFrameToReplace(19, 1);
                return new AnimationFrameReplacer[]{ left, right };
            }
        }
        #endregion
        #region Animation Position
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),
                    right = new AnimationPositionCollection();
                
                left.AddFramePoint2X(11, 16, 4);
                left.AddFramePoint2X(12, 30, 22);
                left.AddFramePoint2X(13, 32, 28);
                left.AddFramePoint2X(14, 29, 34);

                left.AddFramePoint2X(19, 26, 38);

                left.AddFramePoint2X(20, 15, 17);
                left.AddFramePoint2X(21, 29, 25);
                left.AddFramePoint2X(22, 29, 36);

                right.AddFramePoint2X(11, 29, 14);
                right.AddFramePoint2X(12, 32, 22);
                right.AddFramePoint2X(13, 35, 28);
                right.AddFramePoint2X(14, 31, 35);

                right.AddFramePoint2X(20, 26, 17);
                right.AddFramePoint2X(21, 31, 25);
                right.AddFramePoint2X(22, 31, 36);

                return new AnimationPositionCollection[]{ left, right };
            }
        }

        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(18, 23, true);

                anim.AddFramePoint2X(18, 18, 27);
                anim.AddFramePoint2X(19, 18, 27);

                return anim;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();

                anim.AddFramePoint2X(15, 21, 39);
                anim.AddFramePoint2X(16, 21, 39);

                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(16, 0);
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(23, 20, true);

                anim.AddFramePoint2X(18, 23, 24);
                anim.AddFramePoint2X(19, 23, 24);

                return anim;
            }
        }
        #endregion
    }
}