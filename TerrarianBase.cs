using Terraria;
using Terraria.ID;
using Terraria.Audio;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians
{
    public class TerrarianBase : CompanionBase
    {
        public override int Width => 20;
        public override int Height => 42;
        public override int SpriteWidth => 40;
        public override int SpriteHeight => 56;
        public override bool CanCrouch => false;
        public override MountStyles MountStyle => MountStyles.CantMount;
        public override bool CanUseHeavyItem => false;
        public override CompanionTypes CompanionType => CompanionTypes.Terrarian;
        public override CompanionGroup GetCompanionGroup => MainMod.GetTerrariansGroup;
        public override bool AllowSharingChairWithPlayer => false;
        public override bool CanBeAppointedAsBuddy => false;
        protected override Animation SetStandingFrames => new Animation(0);
        protected override Animation SetWalkingFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 6; i <= 18; i++)
                    anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetJumpingFrames => new Animation(5);
        protected override Animation SetPlayerMountedArmFrame => new Animation(10);
        protected override Animation SetItemUseFrames
        {
            get
            {
                Animation anim = new Animation();
                for(short i = 1; i <= 4; i++) anim.AddFrame(i, 1);
                return anim;
            }
        }
        protected override Animation SetSittingFrames => new Animation(6);
        protected override Animation SetBedSleepingFrames => new Animation(0);
        protected override AnimationPositionCollection[] SetHandPositions
        {
            get
            {
                AnimationPositionCollection LeftHandPositions = new AnimationPositionCollection(new Vector2(17, 31), true);
                LeftHandPositions.AddFramePoint2X(1, 5, 10);
                LeftHandPositions.AddFramePoint2X(2, 12, 10);
                LeftHandPositions.AddFramePoint2X(3, 13, 17);
                LeftHandPositions.AddFramePoint2X(4, 12, 20);

                AnimationPositionCollection RightHandPositions = new AnimationPositionCollection(new Vector2(14, 19), true);
                RightHandPositions.AddFramePoint2X(1, 15, 17);
                RightHandPositions.AddFramePoint2X(2, 12, 19);
                RightHandPositions.AddFramePoint2X(3, 12, 17);
                RightHandPositions.AddFramePoint2X(4, 12, 17);
                
                RightHandPositions.AddFramePoint2X(5, 15, 9);
                
                RightHandPositions.AddFramePoint2X(6, 15, 17);
                RightHandPositions.AddFramePoint2X(7, 16, 16);
                RightHandPositions.AddFramePoint2X(8, 16, 16);
                RightHandPositions.AddFramePoint2X(9, 16, 16);
                RightHandPositions.AddFramePoint2X(10, 15, 17);
                RightHandPositions.AddFramePoint2X(11, 15, 17);
                RightHandPositions.AddFramePoint2X(12, 15, 17);
                RightHandPositions.AddFramePoint2X(13, 15, 17);
                RightHandPositions.AddFramePoint2X(14, 14, 16);
                RightHandPositions.AddFramePoint2X(15, 13, 16);
                RightHandPositions.AddFramePoint2X(16, 13, 16);
                RightHandPositions.AddFramePoint2X(17, 14, 17);
                RightHandPositions.AddFramePoint2X(18, 15, 17);
                RightHandPositions.AddFramePoint2X(19, 15, 17);
                AnimationPositionCollection[] Hands = new AnimationPositionCollection[]
                {
                    LeftHandPositions,
                    RightHandPositions
                };
                return Hands;
            }
        }
        protected override AnimationPositionCollection SetSittingPosition => new AnimationPositionCollection(new Vector2(10, 21), true);
    }
}