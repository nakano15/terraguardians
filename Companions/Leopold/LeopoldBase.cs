using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    /// <summary>
    /// -Coward
    /// -Knows of many things
    /// -Brags about knowing many things
    /// -Female Terrarians keep touching his tail.
    /// -Spends most of his day reading books.
    /// -Rarelly leaves house, unless for a research.
    /// -Fears Malisha.
    /// </summary>
    public class LeopoldBase : TerraGuardianBase
    {
        public override string Name => "Leopold";
        public override string Description => "A sage from the Ether Realm.";
        public override Sizes Size => Sizes.Medium;
        public override int Width => 22;
        public override int Height => 58;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 64;
        public override int SpriteHeight => 64;
        public override float Scale => 52f / 58;
        public override int FramesInRow => 2048 / SpriteWidth;
        public override int Age => 23;
        public override Genders Gender => Genders.Male;
        public override int InitialMaxHealth => 160; //640
        public override int HealthPerLifeCrystal => 12;
        public override int HealthPerLifeFruit => 15;
        public override int InitialMaxMana => 40;
        public override int ManaPerManaCrystal => 30;
        public override float AccuracyPercent => 0.86f;
        public override float MaxRunSpeed => 5.2f;
        public override float RunAcceleration => 0.18f;
        public override float RunDeceleration => 0.47f;
        public override int JumpHeight => 17;
        public override float JumpSpeed => 7.66f;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 5, MountUnlock = 7 };
        public override void UpdateAttributes(Companion companion)
        {
            companion.GetDamage<MagicDamageClass>() += 0.15f;
            companion.GetCritChance<MagicDamageClass>() += 10;
            companion.DefenseRate -= 0.03f;
            companion.autoJump = true;
            if (companion.velocity.Y != 0)
            {
                companion.maxRunSpeed *= 1.6f;
                companion.runAcceleration *= 1.6f;
                companion.runSlowdown *= 1.6f;
            }
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new LeopoldDialogues();
        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24); //8
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i < 13; i++)
                {
                    anim.AddFrame(i, 1);
                }
                return anim;
            }
        }
        protected override Animation SetHeavySwingFrames
        {
            get
            {
                Animation anim = new Animation();
                anim.AddFrame(10, 1);
                anim.AddFrame(22, 1);
                anim.AddFrame(23, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(14);
        protected override Animation SetChairSittingFrames => new Animation(24);
        protected override Animation SetThroneSittingFrames => new Animation(15);
        protected override Animation SetBedSleepingFrames => new Animation(16);
        protected override Animation SetRevivingFrames => new Animation(20);
        protected override Animation SetDownedFrames => new Animation(21);
        protected override Animation SetBackwardStandingFrames => new Animation(25);
        protected override Animation SetBackwardReviveFrames => new Animation(26);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(14, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(new Vector2(14, 20), true), 
                    right = new AnimationPositionCollection(new Vector2(21, 20), true);
                //Left Hand
                left.AddFramePoint2X(10, 15, 2);
                left.AddFramePoint2X(11, 24, 7);
                left.AddFramePoint2X(12, 24, 14);
                left.AddFramePoint2X(13, 23, 21);
                
                left.AddFramePoint2X(20, 19, 23);
                
                left.AddFramePoint2X(22, 26, 13);
                left.AddFramePoint2X(23, 23, 25);

                //Right Hand
                right.AddFramePoint2X(10, 18, 2);
                right.AddFramePoint2X(11, 28, 7);
                right.AddFramePoint2X(12, 28, 14);
                right.AddFramePoint2X(13, 26, 21);

                right.AddFramePoint2X(20, 26, 23);
                
                right.AddFramePoint2X(22, 28, 13);
                right.AddFramePoint2X(23, 25, 25);

                return new AnimationPositionCollection[] { left, right };
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition => new AnimationPositionCollection(new Vector2(12, 9), true);
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(14, 9), true);
                anim.AddFramePoint2X(20, 22, 18);
                anim.AddFramePoint2X(22, 18, 12);
                anim.AddFramePoint2X(23, 21, 17);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetWingPosition => new AnimationPositionCollection(new Vector2(16, 17), true);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(new Vector2(14, 23), true);
                return anim;
            }
        }
        #endregion
        #region Extra Animation
        public override void ModifyAnimation(Companion companion)
        {
            if (companion.velocity.Y != 0 && !companion.sliding)
            {
                if (companion.velocity.Y < -1.5f)
                    companion.BodyFrameID = 17;
                else if (companion.velocity.Y < 1.5f)
                    companion.BodyFrameID = 18;
                else
                    companion.BodyFrameID = 19;
                TerraGuardian tg = (TerraGuardian)companion;
                for (int i = 0; i < tg.HeldItems.Length; i++)
                {
                    if (tg.HeldItems[i].ItemAnimation == 0)
                    {
                        companion.ArmFramesID[i] = companion.BodyFrameID;
                    }
                }
            }
        }
        public override void UpdateBehavior(Companion companion)
        {
            if (companion.dash <= 0 && (companion.MoveLeft || companion.MoveRight) && companion.velocity.X != 0 && companion.velocity.Y == 0 && companion.KnockoutStates == KnockoutStates.Awake)
            {
                companion.velocity.Y = -5.6f;
            }
        }
        #endregion
    }
}