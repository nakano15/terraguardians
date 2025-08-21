using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Terraria.GameContent;
using Terraria.Audio;

namespace terraguardians.Interfaces
{
    public class HallowsGreet: LegacyGameInterfaceLayer
    {
        const int MaxDuration = 30;
        const float MinScale = 1f, MaxScale = 5f, ScaleChange = MaxScale - MinScale;
        const float TimeToPercentage = 1f / MaxDuration;
        static int Duration = 0;
        static Companion Head = null;
        static Vector2 SpawnPosition = Vector2.Zero, MoveOrientation = Vector2.One;
        static Color BgColor = new Color(50, 0, 0);
        static int DelayUntilNext = 0;

        public HallowsGreet() : base("TG: Hallow's Greet", Draw, InterfaceScaleType.UI)
        {

        }

        public static void TryTriggerHallowsGreet(int Chance = 50)
        {
            if (Main.halloween && (Chance < 2 || Main.rand.NextBool(Chance)))
            {
                TriggerHallowsGreet();
            }
        }

        internal static void TriggerHallowsGreet()
        {
            if (Duration > 0 || NpcMod.AnyBossAlive || DelayUntilNext > 0 || Main.gameMenu || MainMod.DisableHalloweenJumpscares) return;
            Head = null;
            foreach (Companion c in MainMod.ActiveCompanions.Values)
            {
                if (Head == null || Main.rand.NextBool(2))
                {
                    Head = c;
                }
            }
            if (Head != null)
            {
                Duration = MaxDuration;
                SpawnPosition = new Vector2(Main.rand.Next(0, Main.screenWidth), Main.rand.Next(0, Main.screenHeight));
                MoveOrientation = Vector2.One - new Vector2(SpawnPosition.X / Main.screenWidth, SpawnPosition.Y / Main.screenHeight) * 2f;
            }
            DelayUntilNext = 3 * 60;
            SoundEngine.PlaySound(SoundID.ForceRoar);
        }

        internal static void Unload()
        {
            Head = null;
        }

        internal static bool Draw()
        {
            if (Duration > 0)
            {
                Duration--;
                Rectangle ScreenBounds = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, ScreenBounds, Color.Black);
                if (Head != null)
                {
                    float Percentage = 1f - Duration * TimeToPercentage;
                    Vector2 Movement = MoveOrientation * Percentage * new Vector2(Main.screenWidth, Main.screenHeight) * .25f;
                    float HeadScale = MinScale + (ScaleChange * Percentage);
                    Head.DrawCompanionHead(SpawnPosition + Movement, MoveOrientation.X < 0, HeadScale);
                }
                Main.spriteBatch.Draw(TextureAssets.BlackTile.Value, ScreenBounds, BgColor * .5f);
            }
            else
            {
                if (DelayUntilNext > 0)
                {
                    DelayUntilNext--;
                }
            }
            return true;
        }
    }
}