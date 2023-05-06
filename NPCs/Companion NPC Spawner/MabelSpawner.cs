using Terraria.ModLoader;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using System;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class MabelSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Mabel);
        private static ushort Cooldown = 3;

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (Cooldown > 0)
            {
                Cooldown --;
                return 0;
            }
            Cooldown = (ushort)(Main.rand.Next(6, 21));//(ushort)(Main.rand.Next(180, 601) * 30);
            if (Terraria.Main.dayTime && !Terraria.Main.eclipse && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && Main.rand.Next(5) == 0)
                return 1;
            return 0;
        }

        public override void AI()
        {
            NPC.TargetClosest();
            Player NearestPlayer = Main.player[NPC.target];
            int tileX = (int)(NearestPlayer.Center.X / 16) + Main.rand.Next(-10, 11), 
                tileY = (int)(NearestPlayer.Center.Y / 16);
            if (Main.tile[tileX, tileY].HasTile && Main.tileSolid[Main.tile[tileX, tileY].TileType])
            {
                NPC.active = false;
                return;
            }
            while(!Main.tile[tileX, tileY + 1].HasTile|| !Main.tileSolid[Main.tile[tileX, tileY + 1].TileType])
            {
                tileY++;
                if (tileY >= Main.worldSurface + 20)
                {
                    NPC.active = false;
                    return;
                }
            }
            switch(Main.tile[tileX, tileY + 1].TileType)
            {
                default:
                {
                    NPC.active = false;
                    return;
                }
                case Terraria.ID.TileID.Dirt:
                case Terraria.ID.TileID.ClayBlock:
                case Terraria.ID.TileID.Sand:
                case Terraria.ID.TileID.Mud:
                case Terraria.ID.TileID.Grass:
                case Terraria.ID.TileID.CorruptGrass:
                case Terraria.ID.TileID.CrimsonGrass:
                case Terraria.ID.TileID.HallowedGrass:
                case Terraria.ID.TileID.JungleGrass:
                case Terraria.ID.TileID.MushroomGrass:
                    break;
            }
            for (int y = 1; y < 65; y++)
            {
                int ty = tileY - y;
                if (Main.tile[tileX, ty].HasTile && Main.tileSolid[Main.tile[tileX, ty].TileType])
                {
                    NPC.active = false;
                }
            }
            NPC.position.X = tileX * 16;
            NPC.position.Y = tileY * 16;
            base.AI();
        }
    }
}