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
            float Range = Math.Abs(companion.position.X + companion.width * .5f - Target.position.X + Target.width * .5f);
            companion.WalkMode = Range < 20;
            if (companion.UsingFurniture) companion.LeaveFurniture();
            if (companion.IsBeingPulledByPlayer)
            {
                companion.SaySomething("*Alright. I'm coming. I'm coming.*");
                Deactivate();
                return;
            }
            Sleuthing = false;
            if (Target.KnockoutStates == KnockoutStates.Awake && !Target.IsSleeping)
            {
                if (SleuthPercentage > 70)
                    companion.SaySomething("*...So close...*");
                else
                    companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingFail, "*I... Was just checking if you were fine.*"));
                Deactivate();
                return;
            }
            if (Target.dead)
            {
                Deactivate();
                companion.SaySomething("*...I should have helped instead...*");
                return;
            }
            if (Range < 8)
            {
                companion.MoveLeft = companion.MoveRight = false;
                if (companion.velocity.X == 0 && companion.velocity.Y == 0)
                {
                    Sleuthing = true;
                    companion.direction = (Target.position.X + Target.width * .5f < companion.position.X + companion.width * .5f ? -1 : 1);
                    float LastSleuthPercent = SleuthPercentage;
                    float FillSpeed = Target.IsSleeping ? .07f : .2f;
                    SleuthPercentage += Main.rand.NextFloat() * FillSpeed;
                    if (SleuthPercentage >= 100)
                    {
                        (companion as AlexanderBase.AlexanderCompanion).AddIdentifiedCompanion(Target.GetCompanionID);
                        Deactivate();
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingFinished, "*Okay, so that's how you work.*"));
                        return;
                    }
                    else if (SleuthPercentage >= 70 && LastSleuthPercent < 70)
                    {
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingNearlyDone, "*Hm... Interesting...*"));
                    }
                    else if (SleuthPercentage >= 35 && LastSleuthPercent < 35)
                    {
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingProgress, "*Uh huh...*"));
                    }
                    else if (SleuthPercentage > 0 && LastSleuthPercent <= 0)
                    {
                        companion.SaySomething(Target.GetOtherMessage(MessageIDs.AlexanderSleuthingStart, "*Let's see how you work...*"));
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