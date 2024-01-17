using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class PigGuardianFragmentPiece : TerraGuardianBase
    {
        public const short CloudFormStanding = 19,
            CloudFormMoveForward = 20,
            CloudFormMoveBackward = 21,
            CloudFormChairSit = 22,
            CloudFormThroneSit = 23,
            CloudFormKO = 24,
            CloudFormRevive = 25,
            CloudFormBedSleep = 26,
            CloudFormBackwardStanding = 29,
            CloudFormBackwardRevive = 30;
        public virtual byte PigID => 0;
        public const byte AngerPigGuardianID = 0, SadnessPigGuardianID = 1, HappinessPigGuardianID = 2, FearPigGuardianID = 3, BlandPigGuardianID = 4;

        public override CompanionData CreateCompanionData => new PigGuardianFragmentData();
        public override Sizes Size => Sizes.Medium;
        public override int Age => 15;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Autumn, 14);
        public override string ContributorName => "Smokey";
        public override Genders Gender => Genders.Genderless;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        public override bool CanUseHeavyItem => false;

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 1; i <= 8; i++)
                    anim.AddFrame(i, 24);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 10; i <= 13; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(14);
        protected override Animation SetChairSittingFrames => new Animation(14);
        protected override Animation SetThroneSittingFrames => new Animation(16);
        protected override Animation SetBedSleepingFrames => new Animation(18);
        protected override Animation SetDownedFrames => new Animation(15);
        protected override Animation SetRevivingFrames => new Animation(17);
        protected override Animation SetBackwardStandingFrames => new Animation(27);
        protected override Animation SetBackwardReviveFrames => new Animation(28);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                anim.AddFrameToReplace(14, 0);
                anim.AddFrameToReplace(16, 1);
                anim.AddFrameToReplace(22, 2);
                anim.AddFrameToReplace(23, 3);
                return anim;
            }
        }
        #endregion
        #region Animation Position
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(), right = new AnimationPositionCollection();
                left.AddFramePoint2X(10, 10, 4);
                left.AddFramePoint2X(11, 22, 11);
                left.AddFramePoint2X(12, 24, 19);
                left.AddFramePoint2X(13, 20, 24);

                left.AddFramePoint2X(17, 24, 28);

                right.AddFramePoint2X(10, 16, 4);
                right.AddFramePoint2X(11, 26, 11);
                right.AddFramePoint2X(12, 28, 19);
                right.AddFramePoint2X(13, 24, 24);

                right.AddFramePoint2X(17, 26, 28);
                return new AnimationPositionCollection[] { left, right };
            }
        }
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(14, 15, 24);
                anim.AddFramePoint2X(CloudFormChairSit, 15, 24);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(16, 11, true);
                anim.AddFramePoint2X(14, 16, 9);
                anim.AddFramePoint2X(17, 23, 18);
                anim.AddFramePoint2X(22, 16, 9);
                anim.AddFramePoint2X(25, 23, 18);

                anim.AddFramePoint2X(23, -1000, -1000);
                anim.AddFramePoint2X(24, -1000, -1000);
                anim.AddFramePoint2X(26, -1000, -1000);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(18, 0, -16);
                anim.AddFramePoint2X(26, 0, -16);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetSleepingOffset
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                anim.AddFramePoint2X(18, 16, 0);
                anim.AddFramePoint2X(26, 16, 0);
                return anim;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition => new AnimationPositionCollection(10, 15, true);
        #endregion
        #region Animation Overrides
        public override void ModifyAnimation(Companion companion)
        {
            if (IsCloudForm(companion))
            {
                switch(companion.BodyFrameID)
                {
                    case 28:
                        companion.BodyFrameID = CloudFormBackwardRevive;
                        return;
                    case 27:
                        companion.BodyFrameID = CloudFormStanding;
                        return;
                    case 15:
                        companion.BodyFrameID = CloudFormKO;
                        return;
                    case 17:
                        companion.BodyFrameID = CloudFormRevive;
                        return;
                    case 14:
                        companion.BodyFrameID = CloudFormChairSit;
                        return;
                    case 16:
                        companion.BodyFrameID = CloudFormThroneSit;
                        return;
                    case 18:
                        companion.BodyFrameID = CloudFormBedSleep;
                        return;
                    default:
                        if (companion.velocity.Y != 0)
                        {
                            companion.BodyFrameID = 20;
                        }
                        else if (companion.velocity.X != 0)
                        {
                            if ((companion.direction == -1 && companion.velocity.X < 0) || (companion.direction == 1 && companion.velocity.X > 0))
                                companion.BodyFrameID = 20;
                            else
                                companion.BodyFrameID = 21;
                        }
                        else
                        {
                            companion.BodyFrameID = 19;
                        }
                        return;
                }
            }
        }

        public override void PreDrawCompanions(ref PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder)
        {
            if (IsCloudForm(Holder.GetCompanion))
            {
                Holder.DrawColor *= 0.6f;
            }
        }

        public override void UpdateCompanion(Companion companion)
        {
            if (IsCloudForm(companion))
            {
                if (companion.velocity.Y > 2.3f)
                {
                    companion.velocity.Y = 2.3f;
                    companion.SetFallStart();
                }
                if (companion.KnockoutStates == 0 && !companion.dead && !companion.IsMountedOnSomething && !companion.UsingFurniture && companion.velocity.Y == 0)
                {
                    companion.BodyOffset.Y -= System.MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 2) * 5f;
                }
            }
        }
        #endregion


        public bool IsCloudForm(Companion companion)
        {
            return companion.Data is PigGuardianFragmentData && (companion.Data as PigGuardianFragmentData).IsCloudForm;
        }

        public class PigGuardianFragmentData : CompanionData
        {
            public bool IsCloudForm = true;
        }
    }
}