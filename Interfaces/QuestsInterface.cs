using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.GameContent;
using Terraria.GameContent.Events;
using Terraria.DataStructures;
using Terraria.Graphics.Renderers;
using Terraria.IO;
using Terraria.Audio;
using Terraria.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace terraguardians
{
    public class QuestInterface : LegacyGameInterfaceLayer
    {
        new static bool Active = false;
        public static bool IsActive => Active;
        static Vector2 Position = Vector2.Zero;
        const int Width = 600, Height = 500;

        public QuestInterface() : base("TerraGuardians: Quest Interface", DrawInterface, InterfaceScaleType.UI)
        {

        }

        public static bool DrawInterface()
        {

            return true;
        }

        public static void Open()
        {
            if (Active) return;
            Position.X = Main.screenWidth * 0.5f - Width * 0.5f;
            Position.Y = Main.screenHeight * 0.5f - Height * 0.5f;
        }
    }
}