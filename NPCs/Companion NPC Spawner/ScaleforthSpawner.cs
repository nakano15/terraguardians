using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class ScaleforthSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Scaleforth);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.Water && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && !spawnInfo.PlayerInTown && NPC.downedBoss3)
                return 1f / 500;
            return 0;
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            if (Main.gameMenu) return 200;
            int BedX = -1, BedY = -1;
            bool Found = false;
            for(int x = -16; x < 16; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    if(WorldGen.InWorld(tileX + x, tileY + y))
                    {
                        Tile t = Main.tile[tileX + x, tileY + y];
                        if (t.HasTile && t.TileType == Terraria.ID.TileID.Beds)
                        {
                            BedX = tileX + x;
                            BedY = tileY + y;
                            Found = true;
                            break;
                        }
                    }
                }
                if(Found) break;
            }
            if (Found)
            {
                Tile t = Main.tile[BedX, BedY];
                int FrameX = t.TileFrameX % 54, FrameY = t.TileFrameY % 36;
                if(FrameX < 18)
                    BedX++;
                if (FrameX > 18)
                    BedX--;
                if(FrameY < 18)
                    BedY++;
                sbyte Direction = (sbyte)(Main.rand.NextDouble() < 0.5f ? -1 : 1);
                if (WorldGen.TryToggleLight(BedX, BedY, true, true))
                {
                    
                }
                BedX -= Direction * 2;
                int NpcPos = NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), BedX * 16 + 8, BedY * 16, ModContent.NPCType<BlueSpawner>());
                if(NpcPos < 200)
                {
                    Main.npc[NpcPos].direction = Direction;
                }
                return NpcPos;
            }
            return base.SpawnNPC(tileX, tileY);
        }
    }
}