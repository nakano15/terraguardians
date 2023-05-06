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

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (CanSpawnCompanionNpc()) return 1;
            if (Terraria.Main.dayTime && !Terraria.Main.eclipse && spawnInfo.PlayerInTown && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && PlayerMod.PlayerGetTerraGuardianCompanionsMet(spawnInfo.Player) > 0)
                return 1f / 10;
            return 0;
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            Player NearestPlayer = null;
            float NearestDistance = float.MaxValue;
            Vector2 TileCenter = new Vector2(tileX * 16 + 8, tileY * 16 + 8);
            for(int i = 0; i < 255; i++)
            {
                if(Main.player[i].active && !(Main.player[i] is Companion))
                {
                    float Distance = Math.Abs((Main.player[i].Center - TileCenter).Length());
                    if (Distance < NearestDistance)
                    {
                        NearestPlayer = Main.player[i];
                        NearestDistance = Distance;
                    }
                }
            }
            if (NearestPlayer != null)
            {
                tileX = (int)(NearestPlayer.Center.X / 16) + Main.rand.Next(-10, 11);
                tileY = (int)(NearestPlayer.Center.Y / 16);
                if (Main.tile[tileX, tileY].HasTile && Main.tileSolid[Main.tile[tileX, tileY].TileType]) return -1;
                while(!Main.tile[tileX, tileY + 1].HasTile|| !Main.tileSolid[Main.tile[tileX, tileY + 1].TileType])
                {
                    tileY++;
                    if (tileY >= Main.worldSurface + 20)
                        return -1;
                }
                switch(Main.tile[tileX, tileY + 1].TileType)
                {
                    default:
                        return -1;
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
                        return -1;
                }
                return NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), tileX * 16, tileY * 16, ModContent.NPCType<MabelSpawner>());
            }
            return -1;
        }
    }
}