using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class BlueSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(1);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.Water && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player))
                return 1;
            return 0;
        }

        public override int SpawnNPC(int tileX, int tileY)
        {
            if (Main.gameMenu) return 200;
            int BonfireX = -1, BonfireY = -1;
            bool Found = false;
            for(int x = -16; x < 16; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    if(WorldGen.InWorld(tileX + x, tileY + y))
                    {
                        Tile t = Main.tile[tileX + x, tileY + y];
                        if (t.HasTile && t.TileType == Terraria.ID.TileID.Campfire)
                        {
                            BonfireX = tileX + x;
                            BonfireY = tileY + y;
                            Found = true;
                            break;
                        }
                    }
                }
                if(Found) break;
            }
            if (Found)
            {
                Tile t = Main.tile[BonfireX, BonfireY];
                int FrameX = t.TileFrameX % 54, FrameY = t.TileFrameY % 36;
                if(FrameX < 18)
                    BonfireX++;
                if (FrameX > 18)
                    BonfireX--;
                if(FrameY < 18)
                    BonfireY++;
                sbyte Direction = (sbyte)(Main.rand.NextDouble() < 0.5f ? -1 : 1);
                if (WorldGen.TryToggleLight(BonfireX, BonfireY, true, true))
                {
                    
                }
                BonfireX -= Direction * 2;
                int NpcPos = NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), BonfireX * 16 + 8, BonfireY * 16, ModContent.NPCType<BlueSpawner>());
                if(NpcPos < 200)
                {
                    Main.npc[NpcPos].direction = Direction;
                }
                return NpcPos;
            }
            return 200;
        }
    }
}