using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class CinnamonSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Cinnamon);

        /*public override int SpawnNPC(int tileX, int tileY)
        {
            if (NPC.AnyNPCs(NPCID.TravellingMerchant))
            {
                Vector2 NpcBottom = Main.npc[NPC.FindFirstNPC(NPCID.TravellingMerchant)].Bottom;
                Main.NewText("Someone has arrived by following the Travelling Merchant.", MainMod.MysteryCloseColor);
                return NPC.NewNPC(NPC.GetSource_NaturalSpawn(), (int)NpcBottom.X, (int)NpcBottom.Y, ModContent.NPCType<CinnamonSpawner>());
            }
            return base.SpawnNPC(tileX, tileY);
        }

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if(!spawnInfo.Water && Main.time < 8.5f * 3600 && NPC.AnyNPCs(NPCID.TravellingMerchant) && CanSpawnCompanionNpc() && TargetIsPlayer(spawnInfo.Player) && !WorldMod.IsInRange(spawnInfo.Player.Center, Main.npc[NPC.FindFirstNPC(NPCID.TravellingMerchant)].Center))
                return 1f / 125;
            return 0;
        }*/
    }
}