using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using Terraria.Cinematics;

namespace terraguardians.Cutscenes
{
    public class FlufflesCatchPlayerCutscene : FilmExpanded
    {
        Vector2 FlufflesPosition = Vector2.Zero;
        Vector2 FlufflesPawsPosition = Vector2.Zero;
        Vector2 DemonEyePosition = Vector2.UnitY * -9000;
        float DemonEyeScale = 0;
        const float FlufflesScale = 16f;
        float Scale = 1f;
        float Opacity = 1f;
        float BackgroundOpacity = 0;
        int FlufflesFrame = 0;
        float ViewRectanglesHeight = 0;
        Vector2 ViewSway = Vector2.Zero, SwayForce = Vector2.Zero;
        float ViewSkyY = 0;
        public static CutsceneType SceneType = CutsceneType.Brief;

        const int BlackoutFrames = 20;
        const float BlackoutPercent = 1f / BlackoutFrames;
        const float FlufflesLowestY = 20f;
        const float FlufflesHighestY = 2f;
        const float FlufflesRestY = 6f;
        const float JumpY = -4f;
        const int SurgingDuration = 30;
        Vector2[] Stars = new Vector2[50];

        float TotalScale => Scale * FlufflesScale;

        const int CryDuration = 16 * 12;//16 * 12;

        public FlufflesCatchPlayerCutscene()
        {
            switch(SceneType)
            {
                case CutsceneType.Brief:
                    BriefSetup();
                    break;
                case CutsceneType.LongerVersion:
                    LongVersionSetup();
                    break;
            }
        }

        void BriefSetup()
        {
            AppendSequences(80, DarkenScreen, FlufflesSurgingFrames);
            //AppendSequence(18 * 3, FlufflesGrabsPlayer);
            AppendSequence(8, ScarySpookyFrames);
            AppendSequences(1, PlayedKOdAndFlufflesNpcDespawns, KickPlayerOut);
        }

        void LongVersionSetup()
        {
            AppendSequences(80, DarkenScreen, FlufflesSurgingFrames);
            AppendSequence(18 * 3, FlufflesGrabsPlayer);
            AppendSequences(CryDuration, FlufflesCries, DemonEyeAttack);
            AppendSequences(16, PlayerFalls);
            AppendSequences(180, PlayerBlacksOut, PlayerUnconsciousAndHitSounds);
            AppendSequences(210, PlayerOpensEyes);
            AppendSequences(180, PlayerClosesEyes);
            AppendSequences(120, ScreenFadeOut, PlayedKOdAndFlufflesNpcDespawns);
        }

        void ScarySpookyFrames(FrameEventData data)
        {
            if (data.Frame == 0) PlayHurtSound();
            FlufflesFrame = 14;
        }

        void KickPlayerOut(FrameEventData data)
        {
            MainMod.MoviePlayer.StopMovie();
            WorldGen.SaveAndQuit();
        }

        void DarkenScreen(FrameEventData data)
        {
            BackgroundOpacity = MathF.Min(1, data.Frame * BlackoutPercent);
        }

        void PlayedKOdAndFlufflesNpcDespawns(FrameEventData data)
        {
            if (data.Frame != 0) return;
            MainMod.GetLocalPlayer.GetModPlayer<PlayerMod>().EnterKnockoutState(true);
            MainMod.GetLocalPlayer.statLife = 1;
            MainMod.GetLocalPlayer.Spawn(PlayerSpawnContext.RecallFromItem);
            Companion c = WorldMod.GetCompanionNpc(CompanionDB.Fluffles);
            if (c != null)
            {
                WorldMod.RemoveCompanionNPC(c);
            }
            Main.time += 4 * 3600;
            for (int n = 0; n < 200; n++)
            {
                if (!Main.npc[n].friendly)
                {
                    Main.npc[n].active = false;
                }
            }
        }

        void FlufflesSurgingFrames(FrameEventData data)
        {
            const int SurgeTime = 80 - SurgingDuration;
            const float SurgingPercentage = 1f / SurgingDuration;
            const int OpacityIncreaseDuration = 20;
            const float OpacityPercentage = 1f / OpacityIncreaseDuration;
            const float MinScale = 0.3f;
            const float ScaleChange = 1f - MinScale;
            if (data.Frame == SurgeTime)
                SoundEngine.PlaySound(SoundID.SoundByIndex[29], MainMod.GetLocalPlayer.position);
            if (data.Frame >= SurgeTime)
            {
                float SurgePercentage = MathF.Min(1, (data.Frame - SurgeTime) * SurgingPercentage);
                Scale = MinScale + (SurgePercentage * ScaleChange);
                float SurgeY = 0;
                {
                    float Percentage = SurgePercentage;
                    /*if (Percentage < .5f)
                    {
                        Percentage *= 2;
                        //Percentage *= Percentage;
                        SurgeY = FlufflesLowestY + (FlufflesHighestY - FlufflesLowestY) * Percentage;
                    }
                    else
                    {
                        Percentage -= .5f;
                        Percentage *= 2;
                        //Percentage *= Percentage;
                        SurgeY = FlufflesHighestY + (FlufflesRestY - FlufflesHighestY) * Percentage;
                    }*/
                    //Jump is not working as intended.
                    SurgeY = FlufflesRestY + ((Percentage * 2) * ((1f - Percentage) * 2));//((FlufflesHighestY - FlufflesLowestY) * (1f - Percentage)) * ((FlufflesRestY - FlufflesLowestY) * Percentage);//FlufflesRestY + Percentage * FlufflesHighestY * 2 + Percentage * FlufflesLowestY;
                    //SurgeY = -FlufflesRestY + (1f - Percentage) * -12;
                }
                FlufflesPosition.X = (int)(Main.screenWidth * .5f);
                FlufflesPosition.Y = (int)(Main.screenHeight + SurgeY * TotalScale);
                FlufflesPawsPosition = FlufflesPosition;
                Opacity = MathF.Min(1, (data.Frame - SurgeTime) * OpacityPercentage);
            }
        }

        void FlufflesGrabsPlayer(FrameEventData data)
        {
            const int FramesDuration = 18;
            const float FramesPercentage = 1f / FramesDuration;
            FlufflesFrame = 1 + (int)(data.Frame * FramesPercentage);
            const float BounceDuration = 1f / (FramesDuration * 3);
            FlufflesPosition.Y = (int)(Main.screenHeight + FlufflesRestY * TotalScale + MathF.Sin(MathF.Min(1, BounceDuration * data.Frame) * MathF.Tau) * 3);
            if (FlufflesFrame >= 2)
            {
                FlufflesPawsPosition.Y = Main.screenHeight;
            }
            else
            {
                FlufflesPawsPosition.Y = FlufflesPosition.Y;
            }
        }

        void FlufflesCries(FrameEventData data)
        {
            const int FramesDuration = 16;
            const float FramesPercentage = 1f / FramesDuration;
            FlufflesFrame = (int)MathF.Min(13, 3 + (int)(data.Frame * FramesPercentage));
            FlufflesPosition.Y = (int)(Main.screenHeight + FlufflesRestY * TotalScale + MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 2) * 6);
            FlufflesPawsPosition.Y = Main.screenHeight;
        }

        void DemonEyeAttack(FrameEventData data)
        {
            const float StartY = -60f * FlufflesScale;
            float EndY = Main.screenHeight * .5f + 16;
            float Percentage = data.Frame * (1f / CryDuration);
            const float StartScale = .6f, EndScale = 1.2f;
            Percentage *= Percentage;
            DemonEyePosition.X = (int)(Main.screenWidth * .5f);
            DemonEyePosition.Y = (int)(StartY + (EndY - StartY) * Percentage);
            DemonEyeScale = StartScale + (EndScale - StartScale) * Percentage;
        }

        void PlayerViewSways(FrameEventData data)
        {
            float Rotation = MathF.PI * (1f / 90) * data.Frame;
            ViewSway.X = MathF.Sin(Rotation) * .3f;
            ViewSway.Y = -MathF.Abs(MathF.Cos(Rotation) * .5f);
            //if (data.Frame % 90 == 0)
            //    SoundEngine.PlaySound(SoundID.NPCHit1, MainMod.GetLocalPlayer.position + new Vector2(48 * (ViewSway.X > 0 ? 1f : -1f)));
            /*if(MathF.Abs(SwayForce.X) < 0.1f)
            {
                if (Main.rand.Next(2) == 0)
                    SwayForce.X = Main.rand.NextFloat();
                else
                    SwayForce.X = -Main.rand.NextFloat();
            }
            else
            {
                SwayForce.X *= .9f;
            }
            if(MathF.Abs(SwayForce.Y) < 0.1f)
            {
                if (Main.rand.Next(2) == 0)
                    SwayForce.Y = Main.rand.NextFloat();
                else
                    SwayForce.Y = -Main.rand.NextFloat();
            }
            else
            {
                SwayForce.Y *= .9f;
            }
            ViewSway.X += (SwayForce.X - ViewSway.X) * .1f;
            ViewSway.Y += (SwayForce.Y - ViewSway.Y) * .1f;*/
        }

        void PlayerFalls(FrameEventData data)
        {
            if (data.Frame == 0)
            {
                //SoundEngine.PlaySound(SoundID.NPCHit1, MainMod.GetLocalPlayer.position);
                SoundEngine.PlaySound(SoundID.SoundByIndex[83], MainMod.GetLocalPlayer.position);
            }
            //PlayerViewSways(data);
            const float TotalFallHeight = 16 * 4;
            float Percentage = (float)data.Frame * (1f / 16);
            Percentage *= Percentage;
            FlufflesFrame = 14;
            FlufflesPosition.Y = Main.screenHeight + (FlufflesRestY + TotalFallHeight * Percentage) * TotalScale;
            Scale = 1f - .3f * Percentage;
            DemonEyeScale = 1f - .3f * Percentage;
            DemonEyePosition.Y = Main.screenHeight * .5f - 16 + TotalFallHeight * TotalScale * Percentage;
            ViewSkyY = Main.screenHeight * Percentage * .6f;
            if (data.Frame == 15)
            {
                PlayHurtSound();
                DemonEyePosition.Y = -9999;
            }
        }

        void PlayHurtSound()
        {
            if (MainMod.GetLocalPlayer.Male)
            {
                SoundEngine.PlaySound(SoundID.PlayerHit, MainMod.GetLocalPlayer.position);
            }
            else
            {
                SoundEngine.PlaySound(SoundID.FemaleHit, MainMod.GetLocalPlayer.position);
            }
        }

        void PlayerUnconsciousAndHitSounds(FrameEventData data)
        {
            Scale = 1f;
            switch(data.Frame)
            {
                case 30:
                case 70:
                case 90:
                    {
                        SoundEngine.PlaySound(SoundID.NPCHit1, MainMod.GetLocalPlayer.position + new Vector2(Main.rand.NextFloat(-64f, 64f), 0));
                    }
                    break;
                case 130:
                    {
                        SoundEngine.PlaySound(SoundID.NPCDeath1, MainMod.GetLocalPlayer.position + new Vector2(Main.rand.NextFloat(-64f, 64f), 0));
                    }
                    break;
            }
        }

        void PlayerBlacksOut(FrameEventData data)
        {
            //PlayerViewSways(data);
            ViewRectanglesHeight = MathF.Min(1, (1f / 8) * data.Frame);
        }

        void PlayerOpensEyes(FrameEventData data)
        {
            ViewRectanglesHeight = MathF.Max(.8f, 1 - (1f / 120) * data.Frame * .2f);
            UpdateFlufflesHealingPose();
        }

        void UpdateFlufflesHealingPose()
        {
            ViewSway.X = 0;
            ViewSway.Y = 0;
            FlufflesFrame = 13;
            FlufflesPosition.Y = Main.screenHeight + 8f * TotalScale + MathF.Sin((float)Main.gameTimeCache.TotalGameTime.TotalSeconds * 2) * 6;
            FlufflesPawsPosition = FlufflesPosition;
        }

        void PlayerClosesEyes(FrameEventData data)
        {
            ViewRectanglesHeight = MathF.Min(1, .8f + .2f * (1f / 150) * data.Frame);
            UpdateFlufflesHealingPose();
        }

        void ScreenFadeOut(FrameEventData data)
        {
            MainMod.FlufflesHauntOpacity = -1f;
            BackgroundOpacity = MathF.Max(0, 1f - (1f / 120) * data.Frame);
            Scale = 0;
        }

        public override void OnBegin()
        {
            for (int i = 0; i < Stars.Length; i++)
            {
                Stars[i] = new Vector2((int)(Main.screenWidth * Main.rand.NextFloat()), Main.screenHeight * .4f - Main.rand.NextFloat(0f, Main.screenHeight * 1.6f + 16 * 4 * FlufflesScale));
            }
        }

        public override void OnEnd()
        {
            Stars = null;
        }

        public override void DrawOnScreen()
        {
            MainMod.GetLocalPlayer.mouseInterface = true;
            Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), new Rectangle(16 * 64, 0, 64, 48), Color.White * BackgroundOpacity);
            byte Count = 0;
            foreach (Vector2 Star in Stars)
            {
                Rectangle StarType = new Rectangle(16 * 64, 11 + 48, 3, 3);
                if (Count == 2)
                {
                    StarType.X += 16;
                    Count = 0;
                }
                else
                {
                    StarType.X += 21;
                    Count++;
                }
                Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, Star + Vector2.UnitY * ViewSkyY, StarType, Color.White * BackgroundOpacity, 0, Vector2.One, 2, SpriteEffects.None, 0);
            }
            Rectangle Frame = new Rectangle(FlufflesFrame * 64, 0, 64, 48);
            float FinalScale = FlufflesScale * Scale;
            Color deyecolor = Color.White;
            Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, DemonEyePosition + ViewSway * 2 * TotalScale, new Rectangle(15 * 64, 0, 64, 48), deyecolor, 0f, new Vector2(31, 15), DemonEyeScale * FlufflesScale, SpriteEffects.None, 0);
            Vector2 Origin = new Vector2(32, 48);
            Color c = Companions.FlufflesBase.FlufflesCompanion.GhostfyColor(Color.White, Opacity, MainMod.GetGhostColorMod);
            Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, FlufflesPosition + ViewSway * 2 * TotalScale, Frame, c, 0f, Origin, FinalScale, SpriteEffects.None, 0);
            Frame.Y += 48;
            Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, FlufflesPosition, Frame, c, 0f, Origin, FinalScale, SpriteEffects.None, 0);
            if (ViewRectanglesHeight > 0)
            {
                Color color = new Color(55, 0, 0);
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, (int)(Main.screenHeight * .5f * ViewRectanglesHeight)), null, color * BackgroundOpacity);
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, (int)(Main.screenHeight * (1f - ViewRectanglesHeight * .5f)), Main.screenWidth, (int)(Main.screenHeight * .5f * ViewRectanglesHeight)), null, color * BackgroundOpacity);
            }
        }

        public enum CutsceneType : byte
        {
            Brief = 0,
            LongerVersion = 1
        }
    }
}