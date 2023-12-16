using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System;
using System.Collections.Generic;
using Terraria.ModLoader.IO;

namespace terraguardians.Companions.Generics.Gamer
{
    public class GamerPreRecruitBehavior : PreRecruitBehavior
    {
        int Time = 0;
        byte Step = 0;
        bool Close = false;

        public override bool AllowDespawning => true;

        public GamerPreRecruitBehavior()
        {
            CanBeHurtByNpcs = false;
            CanTargetNpcs = false;
        }

        public override bool AllowStartingDialogue(Companion companion)
        {
            return false;
        }

        public override void Update(Companion companion)
        {
            if (Target != null)
            {
                switch(Step)
                {
                    case 0:
                        if (Math.Abs(companion.Center.X - Target.Center.X) > 40)
                        {
                            if (companion.Center.X < Target.Center.X)
                            {
                                companion.MoveRight = true;
                                companion.MoveLeft = false;
                            }
                            else
                            {
                                companion.MoveLeft = true;
                                companion.MoveRight = false;
                            }
                        }
                        else
                        {
                            Step ++;
                            Time = 0;
                            companion.SaySomething("They're all dread, this place we're in.");
                        }
                        break;
                    case 1:
                        {
                            Time++;
                            if (Time >= 30)
                            {
                                Step ++;
                                Time = 0;
                                companion.SaySomething("He got that girl, he's gonna get you.");
                            }
                        }
                        break;
                    case 2:
                        {
                            Time++;
                            if (Time >= 30)
                            {
                                Step ++;
                                Time = 0;
                                companion.SaySomething("Gotta get through, you know the deal.");
                            }
                        }
                        break;
                    case 3:
                        {
                            Time++;
                            if (Time >= 30)
                            {
                                Step ++;
                                Time = 0;
                                companion.SaySomething("Or the robot you, he's gonna get through.");
                            }
                        }
                        break;
                    case 4:
                        {
                            Time++;
                            if (Time >= 30)
                            {
                                Step ++;
                                Time = 0;
                                companion.SaySomething("Get yourself ready for a fight.");
                            }
                        }
                        break;
                    case 5:
                        {
                            Time++;
                            if (Time >= 30)
                            {
                                Step ++;
                                Time = 0;
                                companion.SaySomething("This time he's super.");
                            }
                        }
                        break;
                    case 6:
                        {
                            Time++;
                            if (Time >= 30)
                            {
                                Step ++;
                                Time = 0;
                                companion.SaySomething("Yo. We're outta here!");
                            }
                        }
                        break;
                    case 7:
                        {
                            if (Time >= 30)
                            {
                                if (companion.Center.X > Target.Center.X)
                                {
                                    companion.MoveRight = true;
                                    companion.MoveLeft = false;
                                }
                                else
                                {
                                    companion.MoveLeft = true;
                                    companion.MoveRight = false;
                                }
                            }
                            else
                            {
                                Time++;
                            }
                        }
                        break;
                }
            }
            else
            {
                WanderAI(companion);
            }
        }
    }
}