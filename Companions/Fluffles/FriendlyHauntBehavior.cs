using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using Terraria.DataStructures;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Fluffles
{
    public class FriendlyHauntBehavior : BehaviorBase
    {
        Player Target;
        bool ByPlayerOrder = false;
        bool LastPlayerFollower = false;
        int DurationTime = 0;
        bool Reviving = false;
        byte Step = 255;
        int Time = 0;

        public Player GetTarget => Target;
        public bool IsByPlayerOrder => ByPlayerOrder;

        public FriendlyHauntBehavior(Player Target, bool ByPlayerOder = false, int Duration = 3 * 3600)
        {
            this.Target = Target;
            this.ByPlayerOrder = ByPlayerOder;
            CanAggroNpcs = false;
            DurationTime = Duration;
        }

        public override void Update(Companion companion)
        {
            if (Target.dead || !Target.active)
            {
                Deactivate();
                return;
            }
            switch (Step)
            {
                case 255:
                    LastPlayerFollower = GetOwner.IsFollower;
                    Step = 0;
                    break;
                case 0:
                    {
                        Rectangle TargetHitbox = Target.Hitbox;
                        Vector2 TargetCenter = Target.Center;
                        if (TargetHitbox.Intersects(companion.Hitbox) || Time++ >= 10 * 60)
                        {
                            Step = 1;
                            Time = 0;
                        }
                        else
                        {
                            if (companion.Center.X > TargetCenter.X)
                            {
                                companion.MoveLeft = true;
                                companion.MoveRight = false;
                            }
                            else
                            {
                                companion.MoveLeft = false;
                                companion.MoveRight = true;
                            }
                        }
                    }
                    break;
                case 1:
                    {
                        if (companion.IsFollower != LastPlayerFollower)
                        {
                            Deactivate();
                            return;
                        }
                        companion.IsBeingPulledByPlayer = false;
                        if (!ByPlayerOrder && Time++ >= DurationTime)
                        {
                            Deactivate();
                            return;
                        }
                        companion.MoveLeft = companion.MoveRight = companion.ControlJump = false;
                        Animation anim = companion.Base.GetAnimation(Reviving ? AnimationTypes.RevivingFrames : AnimationTypes.PlayerMountedArmFrame);
                        Vector2 MountedPosition = companion.GetBetweenAnimationPosition(AnimationPositions.HandPosition, anim.GetFrame(0), false, BottomCentered: true);
                        Vector2 HauntPosition = Vector2.Zero;
                        companion.gfxOffY = 0;
                        if (Reviving)
                        {
                            if (companion.itemAnimation == 0)
                            {
                                companion.direction = -Target.direction;
                            }
                            HauntPosition = Target.Center;
                        }
                        else
                        {
                            if (companion.itemAnimation == 0)
                            {
                                companion.direction = Target.direction;
                            }
                            HauntPosition = Target.position;
                        }
                        if (companion.direction != Target.direction)
                        {
                            MountedPosition.X *= -1;
                        }
                        HauntPosition.X += Target.width * .5f + (MountedPosition.X + 6 * companion.direction * companion.Scale);
                        HauntPosition.Y += Target.height + (MountedPosition.Y + 30 * companion.Scale);
                        DrawOrderInfo.AddDrawOrderInfo(Target, companion, DrawOrderInfo.DrawOrderMoment.InBetweenParent);
                        Target.AddBuff(ModContent.BuffType<Buffs.GhostFoxHaunts.FriendlyHaunt>(), 5);
                        Reviving = PlayerMod.GetPlayerKnockoutState(Target) > KnockoutStates.Awake;
                        companion.velocity = Vector2.Zero;
                        companion.position = HauntPosition;
                    }
                    break;
            }
        }

        public override void OnEnd()
        {
            if (Target != null && !Target.dead)
            {
                GetOwner.Bottom = Target.Bottom;
            }
        }

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (Step == 1)
            {
                Animation anim = companion.Base.GetAnimation(Reviving ? AnimationTypes.RevivingFrames : AnimationTypes.PlayerMountedArmFrame);
                short Frame = anim.GetFrame(0);
                TerraGuardian tg = (companion as TerraGuardian);
                tg.BodyFrameID = Frame;
                for (int i = 0; i < 2; i++)
                {
                    if (tg.HeldItems[i].ItemAnimation <= 0)
                        tg.ArmFramesID[i] = Frame;
                }
            }
        }
    }
}