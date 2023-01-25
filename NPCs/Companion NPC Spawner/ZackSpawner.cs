using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class ZackSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Zacks);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
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