using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class LiftPlayerBehavior : BehaviorBase
    {
        short FrameID = 0;
        Player Target;
        public LiftPlayerBehavior(Player Target)
        {
            this.Target = Target;
            RunCombatBehavior = false;
        }

        public override void Update(Companion companion)
        {
            FrameID = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrameFromPercentage(.4f);
            Vector2 Position = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, FrameID);
            Target.position.X = Position.X - Target.width * 0.5f;
            Target.position.Y = Position.Y - Target.height;
            Target.velocity.X = companion.velocity.X;
            Target.velocity.Y = -.5f;
            Target.fallStart = (int)(Target.position.Y * (1f / 16));
            companion.MoveRight = Target.controlRight;
            companion.MoveLeft = Target.controlLeft;
            if (Target.controlJump)
            {
                Deactivate();
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            for(byte i = 0; i < companion.ArmFramesID.Length; i++)
                companion.ArmFramesID[i] = FrameID;
        }
    }
}