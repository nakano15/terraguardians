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
                Animation anim = Base.GetAnimation(CompanionBase.AnimationTypes.SittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(CompanionBase.AnimationTypes.ChairSittingFrames);
                if(!anim.HasFrames) anim = Base.GetAnimation(CompanionBase.AnimationTypes.StandingFrame);
                BodyFrameID = anim.UpdateTimeAndGetFrame(1, ref BodyFrameTime);
            }
            else //If using Djin's Curse, but...
            {
                if (swimTime > 0)
                {
                    BodyFrameID = Base.GetAnimation(CompanionBase.AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(2, ref BodyFrameTime);
                }
                else if (velocity.Y != 0 || grappling[0] > -1)
                {
                    BodyFrameID = Base.GetAnimation(CompanionBase.AnimationTypes.JumpingFrames).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if(carpetFrame >= 0)
                {
                    BodyFrameID = Base.GetAnimation(CompanionBase.AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                }
                else if (velocity.X != 0)
                {
                    if((slippy || slippy2 || windPushed) && !controlLeft && !controlRight)
                    {
                        BodyFrameID = Base.GetAnimation(CompanionBase.AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
                    }
                    else
                    {
                        BodyFrameID = Base.GetAnimation(CompanionBase.AnimationTypes.WalkingFrames).UpdateTimeAndGetFrame(System.Math.Abs(velocity.X) * 1.3f, ref BodyFrameTime);
                    }
                }
                else
                {
                    BodyFrameID = Base.GetAnimation(CompanionBase.AnimationTypes.StandingFrame).UpdateTimeAndGetFrame(1, ref BodyFrameTime);
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
                ArmFramesID[1] = GetItemUseArmFrame();
            }
            BodyFrame = GetAnimationFrame(BodyFrameID);
            LeftArmFrame = GetAnimationFrame(ArmFramesID[1]);
            RightArmFrame = GetAnimationFrame(ArmFramesID[0]);
        }

        public int GetItemUseArmFrame()
        {
            int Frame = 0;
            if(HeldItem.useStyle == 1 || HeldItem.useStyle == 11 || HeldItem.type == 0)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 7)
            {
                float AnimationPercentage = 1f - (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.67f + 0.33f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 2)
            {
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame(animation.GetFrames.Count - 1);
            }
            else if(HeldItem.useStyle == 9 || HeldItem.useStyle == 8)
            {
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.StandingFrame);
                Frame = animation.GetFrame(0);
            }
            else if(HeldItem.useStyle == 6)
            {
                float AnimationPercentage = System.Math.Min(1, (1f - (float)itemAnimation / itemAnimationMax) * 6);
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime((AnimationPercentage * 0.5f + 0.5f) * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 3 || HeldItem.useStyle == 12)
            {
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame(animation.GetFrames.Count - 1);
            }
            else if(HeldItem.useStyle == 4)
            {
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrame((int)(animation.GetFrames.Count * 0.5f));
            }
            else if(HeldItem.useStyle == 13)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * animation.GetTotalAnimationDuration);
            }
            else if(HeldItem.useStyle == 5)
            {
                float AnimationPercentage = (float)itemAnimation / itemAnimationMax;
                float RotationValue = (1f + itemRotation * direction) * 0.5f;
                if(gravDir == -1)
                    AnimationPercentage = 1f - AnimationPercentage;
                Animation animation = Base.GetAnimation(CompanionBase.AnimationTypes.ItemUseFrames);
                Frame = animation.GetFrameFromTime(AnimationPercentage * RotationValue * animation.GetTotalAnimationDuration);
            }
            return Frame;
        }

        public override void UseItemHitbox(Item item, ref Rectangle hitbox, ref bool noHitbox)
        { //For item hitbox
            
        }

        public override void HoldStyle(Item item, Rectangle heldItemFrame)
        { //For item holding style
            
        }

        public override void UseStyle(Item item, Rectangle heldItemFrame)
        { //For item use style
            
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