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
        const float FlufflesScale = 10f;
        float Scale = 1f;
        float Opacity = 1f;
        float BackgroundOpacity = 0;
        int FlufflesFrame = 0;

        const int BlackoutFrames = 20;
        const float BlackoutPercent = 1f / BlackoutFrames;
        const float FlufflesLowestY = -20f;
        const float FlufflesHighestY = -2f;
        const float FlufflesRestY = -12f;

        public FlufflesCatchPlayerCutscene()
        {
            AppendSequences(80, DarkenScreen, FlufflesSurgingFrames);
            AppendSequence(8 * 3, FlufflesGrabsPlayer);
            AppendSequences(16 * 4, FlufflesCries);
        }

        void DarkenScreen(FrameEventData data)
        {
            BackgroundOpacity = MathF.Min(1, data.Frame * BlackoutPercent);
        }

        void FlufflesSurgingFrames(FrameEventData data)
        {
            const int SurgeTime = 30;
            const int SurgingDuration = 50;
            const float SurgingPercentage = 1f / SurgingDuration;
            const int OpacityIncreaseDuration = 20;
            const float OpacityPercentage = 1f / OpacityIncreaseDuration;
            const float MinScale = 0.3f;
            const float ScaleChange = 1f - MinScale;

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
                    SurgeY = FlufflesLowestY + ((FlufflesHighestY - FlufflesLowestY) * (1f - Percentage)) * ((FlufflesRestY - FlufflesLowestY) * Percentage);//FlufflesRestY + Percentage * FlufflesHighestY * 2 + Percentage * FlufflesLowestY;
                    //SurgeY = -FlufflesRestY + (1f - Percentage) * -12;
                }
                FlufflesPosition.X = (int)(Main.screenWidth * .5f);
                FlufflesPosition.Y = (int)(Main.screenHeight + SurgeY * Scale * FlufflesScale);
                FlufflesPawsPosition = FlufflesPosition;
                Opacity = MathF.Min(1, (data.Frame - SurgeTime) * OpacityPercentage);
            }
        }

        void FlufflesGrabsPlayer(FrameEventData data)
        {
            const int FramesDuration = 8;
            const float FramesPercentage = 1f / FramesDuration;
            FlufflesFrame = 1 + (int)(data.Frame * FramesPercentage);
            const float BounceDuration = 1f / (FramesDuration * 3);
            FlufflesPosition.Y = (int)(Main.screenHeight + FlufflesRestY * MathF.Sin(MathF.Min(1, BounceDuration) * MathF.Tau));
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
            FlufflesFrame = (int)MathF.Min(5, 2 + (int)(data.Frame * FramesPercentage));
            FlufflesPosition.Y = (int)(Main.screenHeight + FlufflesRestY);
            FlufflesPawsPosition.Y = Main.screenHeight;
        }

        public override void OnBegin()
        {
            
        }

        public override void OnEnd()
        {
            
        }

        public override void DrawOnScreen()
        {
            MainMod.GetLocalPlayer.mouseInterface = true;
            Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), null, Color.Black * BackgroundOpacity);
            Rectangle Frame = new Rectangle(FlufflesFrame * 64, 0, 64, 48);
            float FinalScale = FlufflesScale * Scale;
            Vector2 Origin = new Vector2(32, 48)/* * FinalScale*/;
            Color c = Companions.FlufflesBase.FlufflesCompanion.GhostfyColor(Color.White, Opacity, MainMod.GetGhostColorMod);
            Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, FlufflesPosition, Frame, c, 0f, Origin, FinalScale, SpriteEffects.None, 0);
            Frame.Y += 48;
            Main.spriteBatch.Draw(MainMod.FlufflesCatchPlayerViewTexture.Value, FlufflesPosition, Frame, c, 0f, Origin, FinalScale, SpriteEffects.None, 0);
        }
    }
}