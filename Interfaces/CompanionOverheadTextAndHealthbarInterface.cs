using Terraria;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System;

namespace terraguardians
{
    public class CompanionOverheadTextAndHealthbarInterface : LegacyGameInterfaceLayer
    {
        public CompanionOverheadTextAndHealthbarInterface() : base("TerraGuardians: Companion Chat and Health Bar", DrawInterface, InterfaceScaleType.Game)
        {

        }

        public static bool DrawInterface()
        {
            Vector2 ScreenCenter = new Vector2(Main.screenPosition.X + Main.screenWidth * 0.5f, Main.screenPosition.Y + Main.screenHeight * 0.5f);
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if(Math.Abs(c.Center.X - ScreenCenter.X) >= (Main.screenWidth + c.width + 100) * 0.5f || 
                    Math.Abs(c.Center.Y - ScreenCenter.Y) >= (Main.screenHeight + c.height + 100) * 0.5f)
                    continue;
                if (!(c.whoAmI >= 0 && c.whoAmI < 255 && Main.player[c.whoAmI] == c))
                {
                    c.DrawOverheadMessage();
                    if (c.ghost || c.dead || c.invis || c.statLife >= c.statLifeMax2) continue;
                    if(Main.HealthBarDrawSettings == 1)
                    {
                        const float Offset = 10;
                        float lighting = Lighting.Brightness((int)((c.position.X + c.width * 0.5f) * (1f / 16)), (int)((c.position.Y + c.height * 0.5f + c.gfxOffY) * (1f / 16)));
                        Main.instance.DrawHealthBar(c.position.X + c.width * 0.5f, c.position.Y + c.height + Offset + c.gfxOffY, c.statLife, c.statLifeMax2, c.stealth * lighting);
                    }
                    else if (Main.HealthBarDrawSettings == 2)
                    {
                        const float Offset = -20;
                        float lighting = Lighting.Brightness((int)((c.position.X + c.width * 0.5f) * (1f / 16)), (int)((c.position.Y + c.height * 0.5f + c.gfxOffY) * (1f / 16)));
                        Main.instance.DrawHealthBar(c.position.X + c.width * 0.5f, c.position.Y + Offset + c.gfxOffY, c.statLife, c.statLifeMax2, c.stealth * lighting);
                    }
                }
                DrawFriendshipHeartDisplay(c);
            }
            return true;
        }

        static void DrawFriendshipHeartDisplay(Companion companion)
        {
            companion.HeartDisplay.GetHeartDisplayProgress(companion, out HeartDisplayHelper.DisplayStates State, out float Percentage);
            if (State == HeartDisplayHelper.DisplayStates.Hidden) return;
            Vector2 DrawPosition = companion.Bottom - Main.screenPosition;
            DrawPosition.Y -= System.MathF.Min(152, companion.SpriteHeight) + 24;
            float Opacity = 1;
            int LevelDisplayed = 0;
            float LevelProgress = 0;
            if (State <= HeartDisplayHelper.DisplayStates.FillingDisplay || (State == HeartDisplayHelper.DisplayStates.Delay && Percentage == 0))
            {
                LevelDisplayed = (int)companion.HeartDisplay.LastLevel;
                LevelProgress = (float)companion.HeartDisplay.LastProgress / MathF.Max(1, (int)companion.HeartDisplay.LastMaxProgress);
            }
            else if ((State > HeartDisplayHelper.DisplayStates.FillingDisplay && State < HeartDisplayHelper.DisplayStates.Delay) || (State == HeartDisplayHelper.DisplayStates.Delay && Percentage == 1))
            {
                LevelDisplayed = (int)companion.FriendshipLevel;
                LevelProgress = (float)companion.FriendshipExp / MathF.Max(1, (int)companion.FriendshipMaxExp);
            }
            switch(State)
            {
                case HeartDisplayHelper.DisplayStates.FadingIn:
                case HeartDisplayHelper.DisplayStates.FadingOut:
                    Opacity = Percentage;
                    break;
                case HeartDisplayHelper.DisplayStates.FillingDisplay:
                    {
                        float LevelChanges = (int)companion.FriendshipLevel - LevelDisplayed;
                        LevelChanges += (float)companion.FriendshipExp / MathF.Max(1, (int)companion.FriendshipMaxExp) - LevelProgress; //Causes NaN.
                        LevelChanges *= Percentage;
                        int LevelSum = (int)LevelChanges;
                        //Main.NewText("Level display: " + LevelDisplayed + "  Level Changes: " + LevelChanges + "  Level Sum: " + LevelSum + "  Result: " + (LevelDisplayed + LevelSum));
                        LevelDisplayed = LevelDisplayed + LevelSum;
                        LevelProgress = (LevelProgress + LevelChanges) % 1;
                    }
                    break;
            }
            MainMod.DrawFriendshipHeart(DrawPosition, LevelDisplayed, LevelProgress, Opacity);
            //companion.SaySomething("State: " + State.ToString() + " Percentage: " + Percentage);
        }
    }
}