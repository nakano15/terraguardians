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
        byte Step = 0;
        byte Time = 0;

        public LiftPlayerBehavior(Player Target)
        {
            this.Target = Target;
            RunCombatBehavior = false;
        }

        public override void Update(Companion companion)
        {
            companion.WalkMode = false;
            switch(Step)
            {
                case 0:
                    if(companion.getRect().Intersects(Target.getRect()))
                    {
                        Step = 1;
                    }
                    else
                    {
                        MoveTowards(companion, Target.Center);
                    }
                    break;
                case 1:
                    FrameID = companion.Base.GetAnimation(AnimationTypes.ItemUseFrames).GetFrameFromPercentage(System.MathF.Max(.4f, 1f - .6f * ((float)Time / 12)));
                    Vector2 Position = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, FrameID);
                    if (Time < 16)
                    {
                        Target.position.X = Position.X - Target.width * 0.5f;
                        Target.position.Y = Position.Y - Target.height * 0.5f;
                        Time++;
                    }
                    else
                    {
                        Target.position.X = Position.X - Target.width * 0.5f;
                        Target.position.Y = Position.Y - Target.height;
                    }
                    Target.velocity.X = companion.velocity.X;
                    Target.velocity.Y = -Player.defaultGravity;
                    Target.fallStart = (int)(Target.position.Y * (1f / 16));
                    companion.MoveRight = Target.controlRight;
                    companion.MoveLeft = Target.controlLeft;
                    if (Target.mount.Active) Target.mount.Dismount(Target);

                    if (Target.controlJump)
                    {
                        Target.justJumped = true;
                        Target.velocity.Y = -Player.jumpSpeed * Target.gravDir;
                        Target.jump = Player.jumpHeight;
                        Deactivate();
                    }
                    break;
            }
            
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (Step > 0)
            {
                for(byte i = 0; i < companion.ArmFramesID.Length; i++)
                    companion.ArmFramesID[i] = FrameID;
            }
        }
    }
}