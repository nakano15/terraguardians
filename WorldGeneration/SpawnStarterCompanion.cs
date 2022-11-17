using Terraria;
using Terraria.ModLoader;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using Terraria.Graphics.Renderers;
using Terraria.UI;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;
using terraguardians;

namespace terraguardians.WorldGeneration
{
    public class SpawnStarterCompanion : PassLegacy
    {
        public SpawnStarterCompanion() : base("TerraGuardians: Spawn Starter Companion", Generate)
        {
            
        }

        public static void Generate(GenerationProgress progress, GameConfiguration configuration)
        {
            progress.Message = "Spawning Starter Companion";
            List<CompanionID> PossibleCompanions = new List<CompanionID>(MainMod.GetStarterCompanions.ToArray());
            List<CompanionID> SpawnedCompanions = new List<CompanionID>();
            if(PossibleCompanions.Count > 0)
            {
                int Picked = Main.rand.Next(PossibleCompanions.Count);
                Companion tg = WorldMod.SpawnCompanionNPC(PossibleCompanions[Picked]);
                if(tg != null)
                {
                    WorldMod.AddCompanionMet(PossibleCompanions[Picked]);
                    SpawnedCompanions.Add(PossibleCompanions[Picked]);
                    WorldMod.AllowCompanionNPCToSpawn(PossibleCompanions[Picked]);
                }
            }
            WorldMod.StarterCompanions = SpawnedCompanions.ToArray();
        }
    }
}