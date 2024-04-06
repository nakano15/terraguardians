using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Audio;
using System;
using System.Linq;
using System.Collections.Generic;
using Terraria.DataStructures;

namespace terraguardians.Companions.Liebre
{
    public class SoulUnloadingAction : BehaviorBase
    {
        int SoulsValue = 0;
        Player MountedOnCharacter = null;
        byte Step = 0;
        bool StepStart = true;
        int Time = 0;

        public override void Update(Companion companion)
        {
            LiebreBase.LiebreCompanion c = (LiebreBase.LiebreCompanion)companion;
            LiebreBase.LiebreData data = (LiebreBase.LiebreData)c.Data;
            switch (Step)
            {
                case 0:
                    {
                        if (StepStart)
                        {
                            SoulsValue = data.SoulsLoaded;
                            if (SoulsValue >= LiebreBase.LiebreCompanion.MaxSoulsContainedValue)
                            {
                                companion.SaySomething("*I must go, now!*");
                            }
                            else if (SoulsValue >= LiebreBase.LiebreCompanion.MaxSoulsContainedValue * 0.9f)
                            {
                                companion.SaySomething("*I'll unload those souls, I feel like I'm reaching my capacity.*");
                            }
                            else
                            {
                                companion.SaySomething("*I'll be right back.*");
                            }
                        }
                        RunCombatBehavior = false;
                        if(Main.rand.NextFloat() < 0.333f)
                        {
                            Dust.NewDust(companion.position, companion.width, companion.height, 192, 0, -0.2f, Scale:Main.rand.NextFloat(0.8f, 1.2f));
                        }
                        if (Time >= 3 * 60)
                        {
                            ChangeStep();
                            data.SoulsLoaded = 0;
                            IsVisible = false;
                            MountedOnCharacter = companion.GetCharacterMountedOnMe;
                            if (companion.IsMountedOnSomething)
                                companion.ToggleMount(companion.GetCharacterMountedOnMe, true);
                            companion.ClearChat();
                            return;
                        }
                    }
                    break;
                
                case 1:
                    {
                        IsVisible = false;
                        RunCombatBehavior = false;
                        CanBeAttacked = false;
                        CanBeHurtByNpcs = false;
                        if (companion.Owner != null)
                            companion.Bottom = companion.Owner.Bottom;
                        if (Time >= 10 * 60)
                        {
                            ChangeStep();
                            return;
                        }
                    }
                    break;
                
                case 2:
                    {
                        IsVisible = true;
                        RunCombatBehavior = true;
                        CanBeAttacked = true;
                        CanBeHurtByNpcs = true;
                        if (StepStart)
                        {
                            companion.Spawn(PlayerSpawnContext.RecallFromItem);
                            if (companion.Owner != null)
                            {
                                companion.Bottom = companion.Owner.Bottom;
                                companion.SetFallStart();
                                if (MountedOnCharacter != null)
                                {
                                    companion.ToggleMount(MountedOnCharacter, true);
                                }
                            }
                        }
                        if (Time >= 30)
                        {
                            if (SoulsValue >= LiebreBase.LiebreCompanion.MaxSoulsContainedValue)
                            {
                                companion.SaySomething("*I felt like I was about to explode...*");
                            }
                            else if (SoulsValue >= LiebreBase.LiebreCompanion.MaxSoulsContainedValue * 0.9f)
                            {
                                companion.SaySomething("*I'm glad I managed to do the delivery in time.*");
                            }
                            else
                            {
                                companion.SaySomething("*I returned.*");
                            }
                        }
                        if (Time % 10 == 0)
                        {
                            float HeightVal = Time / 10 * 0.1f * companion.height;
                            for (int i = -1; i < 2; i ++)
                            {
                                Vector2 EffectPosition = new Vector2(companion.Center.X, companion.position.Y + companion.height);
                                EffectPosition.Y -= HeightVal;
                                EffectPosition.X += companion.width * 0.6f * i;
                                Dust.NewDust(EffectPosition, 1, 1, 192, 0, -0.2f, 192, Scale: Main.rand.NextFloat(0.8f, 1.2f));
                            }
                        }
                        if (Time >= 100)
                        {
                            float BuffPower = (float)SoulsValue / LiebreBase.LiebreCompanion.MaxSoulsContainedValue;
                            int BuffDuration = (int)(BuffPower * 900) * 60;
                            if (SoulsValue > LiebreBase.LiebreCompanion.MaxSoulsContainedValue)
                            {
                                BuffDuration = (int)(BuffDuration * 0.75f);
                                BuffPower *= 0.75f;
                            }
                            LiebreBase.BlessedSoulBuffPower = BuffPower;
                            LiebreBase.BlessedSoulBuffDuration = BuffDuration;
                            companion.SaySomethingCanSchedule("*Take this blessing as a reward for helping me.*");
                            Deactivate();
                        }
                    }
                    break;
            }
            Time++;
            StepStart = false;
        }

        void ChangeStep()
        {
            Step++;
            StepStart = true;
            Time = 0;
        }
    }
}