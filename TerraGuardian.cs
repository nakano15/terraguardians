using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians
{
    public class TerraGuardian : Companion
    {
        public Rectangle BodyFrame = new Rectangle();
        public Rectangle LeftArmFrame = new Rectangle();
        public Rectangle RightArmFrame = new Rectangle();
        private float BodyFrameTime = 0;
        private AnimationStates PreviousAnimationState = AnimationStates.Standing;

        protected override void UpdateAnimations()
        {
            PlayerFrame();
            AnimationStates NewState = AnimationStates.Standing;
            if(swimTime > 0) NewState = AnimationStates.Swiming;
            else if (velocity.Y != 0) NewState = AnimationStates.InAir;
            else if (mount.Active) NewState = AnimationStates.RidingMount;
            else if (sliding) NewState = AnimationStates.WallSliding;
            else if (velocity.X != 0 && (slippy || slippy2 || windPushed) && !controlLeft && !controlRight) NewState = AnimationStates.IceSliding;
            else if (velocity.X != 0) NewState = AnimationStates.Moving;
            if(NewState != PreviousAnimationState)
                BodyFrameTime = 0;
            PreviousAnimationState = NewState;
            int BodyFrameID = 0;
            int[] ArmFramesID = new int[]{0,0};
            if (mount.Active)
            {
                Animation anim = Base.SittingFrames;
                if(!anim.HasFrames) anim = Base.ChairSittingFrames;
                if(!anim.HasFrames) anim = Base.StandingFrames;
                BodyFrameID = anim.UpdateFrameAndGetTime(1, ref BodyFrameTime);
            }
            else //If using Djin's Curse, but...
            {
                if (swimTime > 0)
                {
                    BodyFrameID = Base.WalkingFrames.UpdateFrameAndGetTime(2, ref BodyFrameTime);
                }
                else if (velocity.Y != 0 || grappling[0] > -1)
                {
                    BodyFrameID = Base.JumpingFrames.UpdateFrameAndGetTime(1, ref BodyFrameTime);
                }
                else if(carpetFrame >= 0)
                {
                    BodyFrameID = Base.StandingFrames.UpdateFrameAndGetTime(1, ref BodyFrameTime);
                }
                else if (velocity.X != 0)
                {
                    if((slippy || slippy2 || windPushed) && !controlLeft && !controlRight)
                    {
                        BodyFrameID = Base.StandingFrames.UpdateFrameAndGetTime(1, ref BodyFrameTime);
                    }
                    else
                    {
                        BodyFrameID = Base.WalkingFrames.UpdateFrameAndGetTime(System.Math.Abs(velocity.X) * 1.3f, ref BodyFrameTime);
                    }
                }
                else
                {
                    BodyFrameID = Base.StandingFrames.UpdateFrameAndGetTime(1, ref BodyFrameTime);
                }
            }
            if(BodyFrameID == -1) BodyFrameID = 0;
            for(int a = 0; a < ArmFramesID.Length; a++)
            {
                ArmFramesID[a] = BodyFrameID;
            }
            BodyFrame = GetAnimationFrame(BodyFrameID);
            LeftArmFrame = GetAnimationFrame(ArmFramesID[1]);
            RightArmFrame = GetAnimationFrame(ArmFramesID[0]);
        }

        public Rectangle GetAnimationFrame(int FrameID)
        {
            Rectangle rect = new Rectangle(FrameID, 0, Base.SpriteWidth, Base.SpriteHeight);
            if(rect.X >= Base.FramesInRow)
            {
                rect.Y = rect.Y + rect.X / Base.FramesInRow;
                rect.X = rect.X - rect.Y * Base.FramesInRow;
            }
            rect.X = rect.X * rect.Width;
            rect.Y = rect.Y * rect.Height;
            return rect;
        }

        public enum AnimationStates : byte
        {
            Standing,
            Moving,
            Swiming,
            InAir,
            Defeated,
            RidingMount,
            UsingFurniture,
            WallSliding,
            IceSliding
        }
    }
}