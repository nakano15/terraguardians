using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;

namespace terraguardians.Companions
{
    public class PriestessBase : TerraGuardianBase
    {
        public override string Name => "Priestess";
        public override int SpriteWidth => 112;
        public override int SpriteHeight => 108;
        public override int Width => 38;
        public override int Height => 100;
        public override float Scale => 113f / 100;
        public override Sizes Size => Sizes.Large;
        public override Genders Gender => Genders.Female;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ MountUnlock = 0 };

        #region Animations
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation a = new Animation();
                for(short i = 1; i <= 8; i++)
                    a.AddFrame(i, 24);
                return a;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(9);
        protected override Animation SetPlayerMountedArmFrame => new Animation(9);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation a = new Animation();
                for(short i = 12; i <= 15; i++)
                    a.AddFrame(i);
                return a;
            }
        }
        protected override Animation SetSittingFrames => new Animation(16);
        protected override Animation SetChairSittingFrames => new Animation(16);
        protected override Animation SetThroneSittingFrames => new Animation(17);
        protected override Animation SetBedSleepingFrames => new Animation(18);
        protected override Animation SetBackwardStandingFrames => new Animation(19);
        protected override AnimationFrameReplacer[] SetArmFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer right = new AnimationFrameReplacer(), left = new AnimationFrameReplacer();
                right.AddFrameToReplace(10, 0);
                right.AddFrameToReplace(11, 1);
                right.AddFrameToReplace(16, 2);
                right.AddFrameToReplace(17, 3);
                right.AddFrameToReplace(18, 4);
                return new AnimationFrameReplacer[]{left, right};
            }
        }
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer a = new AnimationFrameReplacer();
                a.AddFrameToReplace(16, 0);
                return a;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 24);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(28, 40, true);
                a.AddFramePoint2X(17, -1, 0);
                return a;
            }
        }
        protected override AnimationPositionCollection SetPlayerSittingOffset
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection();
                a.AddFramePoint2X(16, 2, -6);
                a.AddFramePoint2X(17, -7, -15);
                return a;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition
        {
            get
            {
                AnimationPositionCollection a = new AnimationPositionCollection(new Vector2(22, 19), true);
                a.AddFramePoint2X(11, 23, 28);
                return a;
            }
        }
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection left = new AnimationPositionCollection(),
                right = new AnimationPositionCollection();

                left.AddFramePoint2X(12, 16, 4);
                left.AddFramePoint2X(13, 35, 12);
                left.AddFramePoint2X(14, 38, 23);
                left.AddFramePoint2X(15, 35, 34);

                right.AddFramePoint2X(12, 30, 4);
                right.AddFramePoint2X(13, 39, 12);
                right.AddFramePoint2X(14, 41, 23);
                right.AddFramePoint2X(15, 38, 33);

                return new AnimationPositionCollection[]{left, right};
            }
        }
        protected override AnimationPositionCollection SetPlayerSleepingOffset => new AnimationPositionCollection(new Vector2(8, -4), true);
        #endregion
    }
}