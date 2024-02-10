using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework;
using Terraria.Audio;
using System.Collections.Generic;
using Terraria.IO;
using Terraria.WorldBuilding;
using Terraria.GameContent.Generation;

namespace terraguardians
{
    public class AlexRecruitmentScript
    {
        public static string AlexOldPartner 
        {
            get
            {
                return MainMod.GetCompanionBase(CompanionDB.Alex).GetTranslation("oldpartner");
            }
        }//"Irene";
        const int TombstoneWidth = 2, TombstoneHeight = 2;
        public const string TombstoneText = "Here lies [oldpartner].\n\'Brave adventurer and Alex's best friend.\'\n\n\"Take good care of Alex.\"";
        public static int TombstoneTileX = 0, TombstoneTileY = 0;
        public static bool SpawnedTombstone = false;
        public static int AlexNPCPosition = -1;
        public static byte TombstonePlacementAttemptTime = 0;

        internal static bool WorldGenTrySpawningIreneTombstone()
        {
            for (int i = 0; i < 1000; i++)
            {
                if (TrySpawningTombstone())
                    return true;
            }
            return false;
        }

        public static bool TrySpawningTombstone()
        {
            int PositionX = 0, PositionY = 0;
            PositionX = Main.rand.Next((int)(Main.leftWorld * (1f / 16)) + 2, (int)(Main.rightWorld * (1f / 16))- 4);
            PositionY = Main.rand.Next((int)(Main.worldSurface * 0.35f), (int)(Main.worldSurface * 0.95f));
            bool BlockedTilePosition = false;
            for (int x = -1; x < 2; x++)
            {
                Tile tile = Main.tile[PositionX + x, PositionY];
                if ((tile.HasTile && Main.tileSolid[tile.TileType]) || tile.WallType > 0 || tile.LiquidAmount > 0)
                {
                    BlockedTilePosition = true;
                    break;
                }
            }
            if (BlockedTilePosition) return false;
            bool GroundBellow = false;
            while (true)
            {
                PositionY++;
                if (PositionY >= Main.worldSurface) return false;
                bool InvalidTile = false;
                byte FloorCount = 0;
                for (int x = -1; x < 2; x++)
                {
                    Tile tile = Main.tile[PositionX + x, PositionY];
                    if (tile.HasTile && Main.tileSolid[tile.TileType] && tile.TileType != 192 && tile.LiquidAmount == 0)
                    {
                        switch(tile.TileType)
                        {
                            case TileID.CorruptGrass:
                            case TileID.Ebonstone:
                            case TileID.Ebonsand:
                            case TileID.CrimsonGrass:
                            case TileID.Crimtane:
                            case TileID.Crimsand:
                            case TileID.LeafBlock:
                            case TileID.LivingWood:
                            case TileID.BlueDungeonBrick:
                            case TileID.GreenDungeonBrick:
                            case TileID.PinkDungeonBrick:
                                InvalidTile = true;
                                break;
                        }
                        if (!InvalidTile)
                            FloorCount++;
                    }
                }
                if (InvalidTile) break;
                
                if (FloorCount == 1)
                    return false;
                else if (FloorCount == 2)
                {
                    if (Main.tile[PositionX, PositionY - 1].WallType == 0)
                    {
                        GroundBellow = true;
                    }
                    break;
                }
            }
            if (!GroundBellow) return false;
            PositionY -= 1;
            for(int x = -1; x < 2; x++) //Fail safe to avoid spawning tombstones on the water.
            {
                for (int y = 0; y < 2; y++)
                {
                    Tile tile = Main.tile[PositionX + x, PositionY + y];
                    if (tile.LiquidAmount > 0)
                        return false;
                }
            }
            TileObject to;
            if (TileObject.CanPlace(PositionX, PositionY, TileID.Tombstones, Main.rand.Next(255) == 0 ? 9 : 4, 1, out to))
            {
                TileObject.Place(to);
                TombstoneTileX = PositionX;
                TombstoneTileY = PositionY;
                int signpos = Sign.ReadSign(PositionX, PositionY);
                if (signpos > -1)
                {
                    if (Main.sign[signpos] == null)
                    {
                        Main.sign[signpos] = new Sign();
                    }
                    Sign.TextSign(signpos, MainMod.GetCompanionBase(CompanionDB.Alex).GetTranslation("tombstonetext")); //TombstoneText
                }
                WorldMod.SpawnCompanionNPC(new Vector2(TombstoneTileX, TombstoneTileY) * 16, CompanionDB.Alex);
                SpawnedTombstone = true;
                return true;
            }
            return false;
        }

        public static void UpdateTombstoneScript()
        {
            if (MainMod.DisableModCompanions) return;
            if (TombstoneTileX > 0 && !WorldMod.HasMetCompanion(CompanionDB.Alex) && !WorldMod.HasCompanionNPCSpawned(CompanionDB.Alex))
            {
                WorldMod.SpawnCompanionNPC(new Vector2(TombstoneTileX, TombstoneTileY) * 2, CompanionDB.Alex);
            }
            if (TombstoneTileX == 0 && TombstonePlacementAttemptTime++ >= 200)
            {
                TombstonePlacementAttemptTime = 0;
                if (!WorldMod.HasMetCompanion(CompanionDB.Alex))
                {
                    TrySpawningTombstone();
                }
            }
        }

        public static void Save(TagCompound tag)
        {
            tag.Add("SpawnedTombstone", SpawnedTombstone);
            if (SpawnedTombstone)
            {
                tag.Add("TombstoneX", TombstoneTileX);
                tag.Add("TombstoneY", TombstoneTileY);
            }
        }

        public static void Load(TagCompound tag, uint LastVersion)
        {
            SpawnedTombstone = tag.GetBool("SpawnedTombstone");
            if (SpawnedTombstone)
            {
                TombstoneTileX = tag.GetInt("TombstoneX");
                TombstoneTileY = tag.GetInt("TombstoneY");
            }
        }

        public class WorldGenAlexTombstonePlacement : PassLegacy
        {
            public WorldGenAlexTombstonePlacement() : base ("TerraGuardians : Place Irene's Tombstone", Generate)
            {

            }

            public static void Generate(GenerationProgress progress, GameConfiguration configuration)
            {
                if (MainMod.DisableModCompanions) return;
                progress.Message = "Trying to place tombstone...";
                if(WorldGenTrySpawningIreneTombstone())
                {
                    progress.Message = "Tombstone placed successfully.";
                }
                else
                {
                    progress.Message = "Failed to place tombstone.";
                }
                System.Threading.Thread.Sleep(1000);
            }
        }
    }
}