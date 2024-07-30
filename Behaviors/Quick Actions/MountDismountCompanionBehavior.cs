using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians.Behaviors.Actions
{
    public class MountDismountCompanionBehavior : BehaviorBase
    {
        Player Target;
        short FrameID = 0;
        byte Arm = 0;
        int Duration = 0;
        const int MaxDuration = 30;
        bool Mounting = false;

        public MountDismountCompanionBehavior(Companion companion, Player Target, bool MountUp) : base(companion)
        {
            if (Target == companion)
            {
                Deactivate();
                return;
            }
            this.Target = Target;
            Mounting = MountUp;
            Arm = (byte)MathF.Min(1, companion.ArmFramesID.Length - 1);
            companion.IsBeingPulledByPlayer = false;
        }

        public override void Update(Companion companion)
        {
            Animation anim = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames);
            float Percentage = (float)Duration / MaxDuration;
            if (Mounting) Percentage = 1 - Percentage;
            FrameID = anim.GetFrameFromPercentage(Percentage);
            Vector2 HandPos = companion.GetAnimationPosition(AnimationPositions.HandPosition, FrameID, Arm);
            Target.position.X = HandPos.X - Target.width * 0.5f;
            Target.position.Y = HandPos.Y - Target.height * 0.5f;
            Target.fallStart = (int)(Target.position.Y * (1f / 16));
            Target.velocity.X = 0;
            Target.velocity.Y = 0;
            if (Target.itemAnimation == 0)
            {
                if (Target.Center.X > companion.Center.X)
                    Target.direction = -1;
                else
                    Target.direction = 1;
            }
            Duration++;
            if (Duration >= MaxDuration)
            {
                Deactivate();
            }
            companion.MoveRight = Target.controlRight;
            companion.MoveLeft = Target.controlLeft;
            companion.ControlJump = Target.controlJump;
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            companion.ArmFramesID[Arm] = FrameID;
            if (companion.itemAnimation == 0 && Arm == 1)
            {
                companion.ArmFramesID[0] = companion.BodyFrameID;
            }
        }
    }
}