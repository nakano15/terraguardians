using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class ZackSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Zacks);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!WorldMod.HasMetCompanion(ToSpawnID) && !MainMod.HasCompanionInWorld(ToSpawnID) && TargetIsPlayer(spawnInfo.Player))
            {
                if (!spawnInfo.Player.ZoneUnderworldHeight && !spawnInfo.Player.ZoneDirtLayerHeight && !spawnInfo.Player.ZoneRockLayerHeight)
                {
                    if (Main.bloodMoon)
                    {
                        if (System.Math.Abs(spawnInfo.Player.Center.X / 16 - Main.spawnTileX) >= Main.maxTilesX / 3)
                            return 0.03f;
                    }
                    if (!Main.dayTime && !spawnInfo.PlayerInTown && PlayerMod.PlayerHasCompanion(spawnInfo.Player, ToSpawnID))
                    {
                        return 0.03f;
                    }
                }
            }
            return 0;
        }

        /*public override void AI() //Anti early access script
        {
            NPC.NewNPC(new Terraria.DataStructures.EntitySource_SpawnNPC(), (int)NPC.Center.X, (int)NPC.Center.Y, 68);
            NPC.active = false;
            Main.NewText("Zacks isn't ready yet, but you may face the Dungeon Guardian instead.", 255, 0, 0);
        }*/
    }
}