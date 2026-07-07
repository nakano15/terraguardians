using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class GreenSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Green);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            if (!spawner.waterTile && CanSpawnCompanionNpc() && Main.dayTime && !Main.eclipse && Main.time < 9 * 3600 && NPC.downedBoss3)
            {
                bool HasTree = false;
                for (int y = 0; y >= -4; y--)
                {
                    for (int x = -2; x < 3; x++)
                    {
                        int TileX = spawner.SpawnTileX + x, TileY = spawner.SpawnTileY + y;
                        int TreeSize = 0;
                        while(Main.tile[TileX, TileY].HasTile && Main.tile[TileX, TileY].TileType == Terraria.ID.TileID.Trees)
                        {
                            TreeSize++;
                            TileY--;
                        }
                        if (TreeSize >= 9)
                            HasTree = true;
                    }
                }
                if (HasTree)
                {
                    return 1f / 35;
                }
            }
            return 0;
        }
    }
}