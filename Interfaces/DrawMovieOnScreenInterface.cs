using Terraria;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.UI.Chat;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace terraguardians
{
    public class DrawMovieOnScreenInterface : LegacyGameInterfaceLayer
    {
        public DrawMovieOnScreenInterface() :
            base("TerraGuardians: Movie Interface", Draw, InterfaceScaleType.UI)
        {
            
        }

        new public static bool Draw()
        {
            if (MainMod.MoviePlayer.IsPlayingMovie)
            {
                MainMod.MoviePlayer.Draw();
            }
            return true;
        }
    }
}