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
        private short[] HandFrames = new short[2];

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
            short BodyFrameID = 0;
            short[] ArmFramesID = new short[Base.GetHands];
            if (mount.Active)
            {
                Animation anim = Base.GetAnimation(AnimationTypes.SittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.ChairSittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(AnimationTypes.StandingFrame);
                BodyFrameID = anim.UpdateTimeAndGetFrame(1, ref BodyFrameTime);
            }
            else //If using Djin's Curse, but...
            {
                if (swimTime > 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(2, ref BodyFrameTime);
                }
                else if (velocity.Y != 0 || grappling[0] > -1)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.JumpingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if(carpetFrame >= 0)
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if (velocity.X != 0)
                {
                    if((slippy || slippy2 || windPushed) && !controlLeft && !controlRight)
                    {
                        BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                    }
                    else
                    {
                        BodyFrameID = Base.GetAnimation(AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(System.Math.Abs(velocity.X) * 1.3f, ref BodyFrameTime);
                    }
                }
                else
                {
                    BodyFrameID = Base.GetAnimation(AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
            }
            if(BodyFrameID == -1) BodyFrameID = 0;
            for(int a = 0; a < ArmFramesID.Length; a++)
            {
                ArmFramesID[a] = BodyFrameID;
            }
            bool CanVisuallyHoldItem = this.CanVisuallyHoldItem(HeldItem);
            bool HeldItemTypeIsnt4952 = HeldItem.type != 4952;
            //Item attack animations here
            if(sandStorm)
            {

            }
            else if (itemAnimation > 0 && HeldItem.useStyle != 10 && HeldItemTypeIsnt4952)
            {
                ArmFramesID[0] = GetItemUseArmFrame();
            }
            BodyFrame = GetAnimationFrame(BodyFrameID);
            LeftArmFrame = GetAnimationFrame(ArmFramesID[0]);
            RightArmFrame = GetAnimationFrame(ArmFramesID[1]);
            HandFrames = ArmFramesID;
        }

        public short GetItemUseArmFrame()
        {
            short Frame = 0;
            if(HeldItem.useStyle == 1 || HeldItem.useStyle == 11 || HeldItem.type == 0)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 7)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.67f + 0.33f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 2)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == 9 || HeldItem.useStyle == 8)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.StandingFrame);
                Frame = animation.GetFrame(0);
            }
            else if(HeldItem.useStyle == 6)
            {
                float AnimationPercentage = System.Math.Min(1, (1f - (float)itemAnimation / itemAnimationMax) * 6);
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.5f + 0.5f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 3 || HeldItem.useStyle == 12)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count - 1));
            }
            else if(HeldItem.useStyle == 4)
            {
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((short)(animation.GetFrames.Count * 0.5f));
            }
            else if(HeldItem.useStyle == 13)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 5)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                float RotationValue = (1f + itemRotation * direction) * 0.5f;
                if(gravDir == -1)
                    AnimationPercentage = 1f - AnimationPercentage;
                Animation animation = Base.GetAnimation(AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * RotationValue * animation.GetTotalAnimationDuration);
            }
            return Frame;
        }

        public Vector2 GetAnimationPosition(AnimationPositions Animation, short Frame, byte MultipleAnimationsIndex = 0, bool AlsoTakePosition = true)
        {
            Vector2 Position = Base.GetAnimationPosition(Animation, MultipleAnimationsIndex).GetPositionFromFrame(Frame);
            if(direction < 0)
            {
                Position.X *= -1;
                //Position.X = Base.SpriteWidth - Position.X;
            }
            Position.X += Base.SpriteWidth * 0.5f;
            if(gravDir < 0)
            {
                Position.Y = Base.SpriteHeight + Position.Y;
            }
            //Position.Y += Base.SpriteHeight;
            Position *= Scale;
            Position.X -= Base.SpriteWidth * 0.5f * Scale;
            Position.Y -= Base.SpriteHeight * Scale;
            if(AlsoTakePosition)
                Position += position;
            return Position;
        }

        public override void UseItemHitbox(Item item, ref Rectangle hitbox, ref bool noHitbox)
        { //For item hitbox
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            hitbox.X = (int)(hitbox.X - position.X + HandPosition.X);
            hitbox.Y = (int)(hitbox.Y - position.Y + HandPosition.Y);
            itemLocation.X = (int)(itemLocation.X - position.X + HandPosition.X);
            itemLocation.Y = (int)(itemLocation.Y - position.Y + HandPosition.Y);
            
        }

        public override void HoldStyle(Item item, Rectangle heldItemFrame)
        { //For item holding style
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            heldItemFrame.X = (int)(heldItemFrame.X - position.X - width * 0.5f + HandPosition.X);
            heldItemFrame.Y = (int)(heldItemFrame.Y - position.Y - height * 0.5f + HandPosition.Y);
            
        }

        public override void UseStyle(Item item, Rectangle heldItemFrame)
        { //For item use style
            Vector2 HandPosition = GetAnimationPosition(AnimationPositions.HandPosition, HandFrames[0], 0);
            heldItemFrame.X = (int)(heldItemFrame.X - position.X - width * 0.5f + HandPosition.X);
            heldItemFrame.Y = (int)(heldItemFrame.Y - position.Y - height * 0.5f + HandPosition.Y);
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