using System;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.Companions.Daphne
{
    public class DaphnePreRecruitBehavior : PreRecruitBehavior
    {
        byte MeetStep = 0;
        int Time = 0;
        Player Target = null;

        public override void Update(Companion companion)
        {
            switch (MeetStep)
            {
                case 0:
                    {
                        Target = ViewRangeCheck(companion, companion.direction);
                        if (Target != null)
                        {
                            MeetStep = 1;
                        }
                    }
                    break;
                case 1:
                    {
                        if (!Target.active || Target.dead)
                        {
                            MeetStep = 0;
                            Target = null;
                        }
                        else
                        {
                            float PosXDif = Target.Center.X - companion.Center.X;
                            if (Math.Abs(PosXDif) > 80)
                            {
                                if (PosXDif > 0)
                                {
                                    companion.MoveRight = true;
                                }
                                else
                                {
                                    companion.MoveLeft = true;
                                }
                            }
                            else
                            {
                                MeetStep = 2;
                                Time = 0;
                            }
                        }
                    }
                    break;
                case 2:
                    {
                        if (!Target.active || Target.dead)
                        {
                            MeetStep = 0;
                            Target = null;
                        }
                        else
                        {
                            if (companion.velocity.Length() == 0)
                            {
                                if (Target.Center.X < companion.Center.X)
                                {
                                    companion.direction = -1;
                                }
                                else
                                {
                                    companion.direction = 1;
                                }
                                Time++;
                                if (Time >= 180)
                                {
                                    WorldMod.AddCompanionMet(companion);
                                    PlayerMod.PlayerAddCompanion(Target, companion);
                                    Quests.MysteriousNoteQuest.CompleteOnDaphneArrive();
                                    Time = 0;
                                    MeetStep = 0;
                                    Target = null;
                                }
                                else if (Time == 20)
                                {
                                    companion.SaySomething("Bark! Bark! *Wags tail.*");
                                }
                            }
                        }
                    }
                    break;
            }
        }

        public void ChangeHerTarget(Player Target)
        {
            this.Target = Target;
            MeetStep = 1;
            Time = 0;
        }
    }
}