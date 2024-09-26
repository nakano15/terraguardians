using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;
using terraguardians.Companions.Miguel;

namespace terraguardians.Companions.Miguel
{
    public class MiguelBase : TerraGuardianBase
    {
        public override string Name => "Miguel";
        public override string Description => "Your very own personal trainer, like it or not.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 22;
        public override int Height => 94;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 120;
        public override float Scale => 112f / 96;
        public override CombatTactics DefaultCombatTactic => CombatTactics.CloseRange;
        public override int Age => 21;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Summer, 4);
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 225; //1200
        public override int HealthPerLifeCrystal => 45;
        public override int HealthPerLifeFruit => 15;
        public override float AccuracyPercent => .36f;
        public override float MaxFallSpeed => .45f;
        public override float MaxRunSpeed => 6.2f;
        public override float RunAcceleration => .26f;
        public override float RunDeceleration => .38f;
        public override int JumpHeight => 12;
        public override float JumpSpeed => 7.08f;
        public override bool CanCrouch => true;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0 };
        protected override CompanionDialogueContainer GetDialogueContainer => new MiguelDialogue();
        public override CompanionData CreateCompanionData => new MiguelData();
        public override PersonalityBase GetPersonality(Companion c)
        {
            return PersonalityDB.Tough;
        }
        public override void InitialInventory(out InitialItemDefinition[] InitialInventoryItems, ref InitialItemDefinition[] InitialEquipments)
        {
            InitialInventoryItems = new InitialItemDefinition[]
            {
                new InitialItemDefinition(ItemID.BladeofGrass),
                new InitialItemDefinition(ItemID.HealingPotion, 5)
            };
        }
        public override void UpdateAttributes(Companion companion)
        {
            companion.GetDamage<MeleeDamageClass>() += .1f;
            companion.GetCritChance<MeleeDamageClass>() += 8;
            companion.DefenseRate += .02f;
            if (companion.BlockRate > 0)
                companion.BlockRate += .03f;
        }
        #region Animations
        protected override Animation SetStandingFrames => new Animation(2);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 3; i <= 10; i++)
                    anim.AddFrame(i, 24f);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(11);
        protected override Animation SetPlayerMountedArmFrame => new Animation(11);
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 13; i <= 15; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 12; i <= 15; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetCrouchingFrames => new Animation(18);
        protected override Animation SetCrouchingSwingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 19; i <= 21; i++)
                    anim.AddFrame(i);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(17);
        protected override Animation SetChairSittingFrames => new Animation(16);
        protected override Animation SetThroneSittingFrames => new Animation(22);
        protected override Animation SetBedSleepingFrames => new Animation(23);
        protected override Animation SetRevivingFrames => new Animation(24);
        protected override Animation SetDownedFrames => new Animation(25);
        protected override Animation SetBackwardStandingFrames => new Animation(1);
        protected override Animation SetBackwardReviveFrames => new Animation(26);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(16, 0);
                anim.AddFrameToReplace(17, 0);
                return anim;
            }
        }
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer left = new AnimationFrameReplacer(), right = new AnimationFrameReplacer();
                right.AddFrameToReplace(24, 0);
                return new AnimationFrameReplacer[] { left, right };
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                for (short i = 16; i <= 17; i++)
                    anim.AddFramePoint2X(i, 22, 47);
                return anim;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(12, 12, 12);
                left.AddFramePoint2X(13, 31, 20);
                left.AddFramePoint2X(14, 35, 28);
                left.AddFramePoint2X(15, 30, 37);
                
                left.AddFramePoint2X(17, 28, 35);
                left.AddFramePoint2X(26, 28, 35);
                
                left.AddFramePoint2X(19, 14, 13);
                left.AddFramePoint2X(20, 33, 23);
                left.AddFramePoint2X(21, 32, 42);
                
                left.AddFramePoint2X(24, 32, 46);
                
                left.AddFramePoint2X(26, 32, 46);

                right.AddFramePoint2X(12, 24, 12);
                right.AddFramePoint2X(13, 34, 20);
                right.AddFramePoint2X(14, 38, 28);
                right.AddFramePoint2X(15, 34, 37);
                
                right.AddFramePoint2X(19, 17, 13);
                right.AddFramePoint2X(20, 36, 23);
                right.AddFramePoint2X(21, 35, 42);
                
                right.AddFramePoint2X(24, 32, 46);

                right.AddFramePoint2X(26, 32, 46);
                return new AnimationPositionCollection[] { left, right };
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(17, 23, true);
                anim.AddFramePoint2X(18, 17, 27);
                anim.AddFramePoint2X(23, 18, 29);
                anim.AddFramePoint2X(24, 17, 29);
                anim.AddFramePoint2X(26, 17, 29);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(23, 22, true);
                anim.AddFramePoint2X(18, 23, 26);
                anim.AddFramePoint2X(24, 23, 26);
                anim.AddFramePoint2X(26, 23, 26);
                return anim;
            }
        }
        #endregion
        public override void ModifyAnimation(Companion companion)
        {
            if (companion.BodyFrameID == 2 && companion.Target == null)
            {
                companion.BodyFrameID = 0;
                if (companion.itemAnimation <= 0)
                {
                    for (int i = 0; i < 2; i++)
                        companion.ArmFramesID[i] = 0;
                }
            }
        }
    }

    public class MiguelData : CompanionData
    {
        public ExerciseTypes ExerciseType = ExerciseTypes.None;
        public int ExerciseCounter = 0;
        public uint ExercisesDone = 0;

        public string GiveNewExercise()
        {
            ExerciseType = (ExerciseTypes)Main.rand.Next(1, (int)ExerciseTypes.Count);
            switch (ExerciseType)
            {
                case ExerciseTypes.AttackTimes:
                    ExerciseCounter = Main.rand.Next(2, 8) * 20;
                    return "*Yes, I do. Today's exercise may interest you, since will use something you Terrarians loves doing. I want you to attack anything "+ExerciseCounter+" times. Once you do that, come back to me.*";
                case ExerciseTypes.JumpTimes:
                    ExerciseCounter = Main.rand.Next(2, 7) * 5;
                    return "*This time I want to see you jumping like popcorn. Jump "+ExerciseCounter+" times and then come talk to me.*";
                case ExerciseTypes.TravelDistance:
                    ExerciseCounter = Main.rand.Next(2, 6) * 1500;
                    return "*Time to exercise your legs. You need to walk "+(int)(ExerciseCounter * 0.5f)+" feets and then talk to me.*";
            }
            ExerciseCounter = 0;
            return "*Believe me or not, but I got nothing for you today.*";
        }

        protected override void CustomUpdate(Player owner)
        {
            if (ExerciseType > 0 && ExerciseType < ExerciseTypes.Count && ExerciseCounter > 0)
            {
                Player player = MainMod.GetLocalPlayer;
                bool Notify = false;
                switch (ExerciseType)
                {
                    case ExerciseTypes.AttackTimes: //Need rework. Doesn't work with Copper Shortsword (projectile weapon)
                        if (player.itemAnimation > 0 && player.attackCD == 1)
                        {
                            ExerciseCounter--;
                        }
                        break;
                    case ExerciseTypes.JumpTimes:
                        if (player.velocity.Y < 0 && player.justJumped)
                        {
                            ExerciseCounter--;
                            if (ExerciseCounter <= 0)
                            {
                                Notify = true;
                            }
                        }
                        break;
                    case ExerciseTypes.TravelDistance:
                        if (player.velocity.X != 0)
                        {
                            ExerciseCounter -= (int)System.MathF.Abs(player.velocity.X);
                            if (ExerciseCounter <= 0)
                            {
                                Notify = true;
                            }
                        }
                        break;
                }
                if (Notify)
                {
                    Main.NewText("I've completed today's exercises.");
                }
            }
        }
    }

    public enum ExerciseTypes : byte
    {
        None = 0,
        JumpTimes = 1,
        TravelDistance = 2,
        AttackTimes = 3,
        Count = 4,
        WaitUntilNextDay = 255
    }
}