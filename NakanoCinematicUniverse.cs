using Terraria;
using Terraria.Cinematics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians
{
    public class NakanoCinematicUniverse
    {
        private FilmExpanded PlayedMovie = null;
        public bool IsPlayingMovie { get { return PlayedMovie != null; } }
        public bool DrawInFrontOfInterface { get { return PlayedMovie != null && PlayedMovie.DrawInFrontOfInterfaces; } }
        public bool MovieHidesInterface { get { return PlayedMovie != null && PlayedMovie.HideInterfaces; } }

        public void Update(GameTime gameTime)
        {
            if (PlayedMovie == null) return;
            if (Main.hasFocus && !Main.gamePaused && !PlayedMovie.OnUpdate(gameTime))
            {
                PlayedMovie.OnEnd();
                PlayedMovie = null;
            }
        }

        public void Draw()
        {
            if (PlayedMovie != null)
            {
                SamplerState state = Main.DefaultSamplerState;
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, SamplerState.PointClamp, null, null);
                PlayedMovie.DrawOnScreen();
                Main.spriteBatch.End();
                Main.spriteBatch.Begin(SpriteSortMode.Immediate, null, Main.DefaultSamplerState, null, null);
            }
        }

        public void PlayMovie(FilmExpanded Movie)
        {
            PlayedMovie = Movie;
            Movie.OnBegin();
        }

        public void StopMovie()
        {
            if (PlayedMovie != null)
            {
                PlayedMovie.OnEnd();
                PlayedMovie = null;
            }
        }
    }
}