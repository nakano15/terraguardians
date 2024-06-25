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

namespace terraguardians.Companions.Alexander
{
    internal class AlexanderSleuthBehavior : BehaviorBase
    {
        public float SleuthPercentage = 0;
        public bool Sleuthing = false;
        public Companion Target;

        public AlexanderSleuthBehavior(Companion Target)
        {
            this.Target = Target;
        }

        public override void Update(Companion companion)
        {
            if (companion.KnockoutStates > KnockoutStates.Awake)
            {
                Deactivate();
                return;
            }
            float Range = Math.Abs((companion.position.X + companion.width * .5f) - (Target.position.X + Target.width * .5f));
            companion.WalkMode = Range < 20;
            if (companion.UsingFurniture) companion.LeaveFurniture();
            if (companion.IsBeingPulledByPlayer)
            {
                companion.SaySomething(companion.GetTranslation("sleuthinterrupt"));
                Deactivate();
                return;
            }
            Sleuthing = false;
            if (Target.KnockoutStates == KnockoutStates.Awake && !Target.IsSleeping)
            {
                if (SleuthPercentage > 70)
                    companion.SaySomething(companion.GetTranslation("sleuthfail2"));
                else
                    companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingFail, companion.GetTranslation("sleuthfail")));
                Deactivate();
                return;
            }
            if (Target.dead)
            {
                Deactivate();
                companion.SaySomething(companion.GetTranslation("sleuthdeadtarget"));
                return;
            }
            if (Range < 12)
            {
                companion.MoveLeft = companion.MoveRight = false;
                if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                {
                    Sleuthing = true;
                    companion.direction = (Target.position.X + Target.width * .5f < companion.position.X + companion.width * .5f ? -1 : 1);
                    float LastSleuthPercent = SleuthPercentage;
                    float FillSpeed = Target.IsSleeping ? .13f : .2f;
                    SleuthPercentage += FillSpeed * Main.rand.NextFloat();
                    if (SleuthPercentage >= 100)
                    {
                        (companion as AlexanderBase.AlexanderCompanion).AddIdentifiedCompanion(Target.GetCompanionID);
                        Deactivate();
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingFinished, companion.GetTranslation("sleuthdone")));
                        return;
                    }
                    else if (SleuthPercentage >= 70 && LastSleuthPercent < 70)
                    {
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingNearlyDone, companion.GetTranslation("sleuth3")));
                    }
                    else if (SleuthPercentage >= 35 && LastSleuthPercent < 35)
                    {
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingProgress, companion.GetTranslation("sleuth2")));
                    }
                    else if (SleuthPercentage > 0 && LastSleuthPercent <= 0)
                    {
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingStart, companion.GetTranslation("sleuth1")));
                    }
                }
            }
            else
            {
                if (Target.position.X + Target.width * .5f < companion.position.X + companion.width * .5f)
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

        public override void UpdateAnimationFrame(Companion companion)
        {
            if (Sleuthing)
            {
                short Frame = AlexanderBase.SleuthBackAnimationID;
                companion.BodyFrameID = companion.ArmFramesID[0] = companion.ArmFramesID[1] = Frame;
            }
        }
    }
}