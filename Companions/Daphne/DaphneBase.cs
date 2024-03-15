using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions
{
    public class DaphneBase : TerraGuardianBase
    {
        const string HaloTextureID = "halo";

        public DaphneBase() : base()
        {
            VladimirBase.CarryBlacklist.Add(new CompanionID(CompanionDB.Daphne));
        }

        public override string Name => "Daphne";
        public override string Description => "The Guardian Angel of a Terrarian.\nShe can help you meanwhile.";
        public override Sizes Size => Sizes.Large;
        public override int Width => 70;
        public override int Height => 66;
        public override int CrouchingHeight => 52;
        public override int SpriteWidth => 96;
        public override int SpriteHeight => 96;
        public override int Age => 53;
        public override BirthdayCalculator SetBirthday => new BirthdayCalculator(Seasons.Summer, 17);
        public override Genders Gender => Genders.Female;
        public override int InitialMaxHealth => 175; //1125
        public override int HealthPerLifeCrystal => 30;
        public override int HealthPerLifeFruit => 25;
        public override float AccuracyPercent => .36f;
        public override float MaxFallSpeed => .7f;
        public override float MaxRunSpeed => 5.65f;
        public override float RunAcceleration => .33f;
        public override float RunDeceleration => .83f;
        public override int JumpHeight => 15;
        public override float JumpSpeed => 6.71f;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.PlayerMountsOnCompanion;
        protected override FriendshipLevelUnlocks SetFriendshipUnlocks => new FriendshipLevelUnlocks(){ FollowerUnlock = 0, MoveInUnlock = 0 };
        public override CompanionSpritesContainer SetSpritesContainer
        {
            get
            {
                CompanionSpritesContainer container = new CompanionSpritesContainer();
                container.AddExtraTexture(HaloTextureID, "halo");
                return container;
            }
        }
        protected override CompanionDialogueContainer GetDialogueContainer => new DaphneDialogues();
        public override bool AllowSharingChairWithPlayer => false;

        #region
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 1; i <= 8; i++)
                {
                    anim.AddFrame(i, 24);
                }
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
                {
                    anim.AddFrame(i, 24);
                }
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(15);
        protected override Animation SetChairSittingFrames => new Animation(14);
        protected override Animation SetSittingItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for (short i = 16; i <= 18; i++)
                {
                    anim.AddFrame(i, 24);
                }
                return anim;
            }
        }
        protected override Animation SetThroneSittingFrames => new Animation(14);
        protected override Animation SetBedSleepingFrames => new Animation(19);
        protected override Animation SetRevivingFrames => new Animation(21);
        protected override Animation SetDownedFrames => new Animation(20);
        protected override Animation SetBackwardStandingFrames => new Animation(22);
        protected override Animation SetBackwardReviveFrames => new Animation(23);
        protected override AnimationFrameReplacer SetBodyFrontFrameReplacers
        {
            get
            {
                AnimationFrameReplacer anim = new AnimationFrameReplacer();
                for (short i = 15; i<= 18; i++)
                    anim.AddFrameToReplace(i, 0);
                return anim;
            }
        }
        #endregion
        #region Animation Positions
        protected override AnimationPositionCollection SetSleepingOffset => new AnimationPositionCollection(Vector2.UnitX * 16, false);
        protected override AnimationPositionCollection SetSittingPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection();
                for (short i = 14; i <= 15; i++)
                {
                    anim.AddFramePoint2X(i, 20, 40);
                }
                return anim;
            }
        }
        protected override AnimationPositionCollection SetMountShoulderPosition => new AnimationPositionCollection(28, 24, true);
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection arm = new AnimationPositionCollection();
                arm.AddFramePoint2X(10, 39, 31);
                arm.AddFramePoint2X(11, 42, 32);
                arm.AddFramePoint2X(12, 43, 38);
                arm.AddFramePoint2X(13, 41, 42);
                
                arm.AddFramePoint2X(15, 25, 33);
                arm.AddFramePoint2X(16, 30, 18);
                arm.AddFramePoint2X(17, 32, 25);
                arm.AddFramePoint2X(18, 29, 30);
                
                arm.AddFramePoint2X(21, 40, 40);

                return new AnimationPositionCollection[] {arm, arm};
            }
        }
        protected override AnimationPositionCollection SetHeadVanityPosition
        {
            get
            {
                AnimationPositionCollection anim = new AnimationPositionCollection(38, 21, true);
                for (short i = 15; i <= 18; i++)
                    anim.AddFramePoint2X(i, 25, 16);
                for (short i = 19; i <= 20; i++)
                    anim.AddFramePoint2X(i, 40, 39);
                return anim;
            }
        }
        #endregion
        public override void CompanionDrawLayerSetup(bool IsDrawingFrontLayer, PlayerDrawSet drawSet, ref TgDrawInfoHolder Holder, ref List<DrawData> DrawDatas)
        {
            if (!IsDrawingFrontLayer)
            {
                Texture2D HaloTexture = Holder.GetCompanion.Base.GetSpriteContainer.GetExtraTexture(HaloTextureID);
                Vector2 HaloPosition = Holder.GetCompanion.GetAnimationPosition(AnimationPositions.HeadVanityPosition, Holder.GetCompanion.BodyFrameID, 0, false, false, false, false, false);
                HaloPosition.Y -= 30;
                if (Holder.GetCompanion.direction < 0)
                {
                    HaloPosition.X = Holder.GetCompanion.SpriteWidth - HaloPosition.X;
                }
                HaloPosition += Holder.DrawPosition;
                DrawData dd = new DrawData(HaloTexture, HaloPosition, new Rectangle(0, 0, 26, 12), Color.White, 0, new Vector2(13, 6), Holder.GetCompanion.Scale, SpriteEffects.None, 0);
                DrawDatas.Insert(0, dd);
                float TransparencyValue = (float)MainMod.NemesisFadeEffect / (MainMod.NemesisFadingTime * 0.5f);
                if(TransparencyValue > 1f)
                {
                    TransparencyValue = 2f - TransparencyValue;
                }
                dd = new DrawData(HaloTexture, HaloPosition, new Rectangle(26, 0, 26, 12), Color.White * TransparencyValue, 0, new Vector2(13, 6), Holder.GetCompanion.Scale, SpriteEffects.None, 0);
                DrawDatas.Insert(1, dd);
            }
        }
    }
}