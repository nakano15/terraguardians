using Terraria;
using Terraria.ModLoader;
using System;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class CelesteSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.Celeste);

        public override float SpawnChance(NPCSpawnInfo spawnInfo)
        {
            if (!spawnInfo.Water && CanSpawnCompanionNpc(false) && Main.dayTime && Main.time >= 5f * 3600 && Main.time < 6f * 3600)
                return 1f / 7;
            return 0;
        }

        public override void AI()
        {
            Companion NearestCompanionNPC = null;
            foreach(Companion c in MainMod.ActiveCompanions.Values)
            {
                if (c.HasHouse && c.Owner == null)
                {
                    bool AnyPlayerNearby = false;
                    for (int p = 0; p < 255; p++)
                    {
                        if (Main.player[p].active && !(Main.player[p] is Companion) && Math.Abs(c.Center.X - Main.player[p].Center.X) < NPC.sWidth && Math.Abs(c.Center.Y - Main.player[p].Center.Y) < NPC.sHeight)
                        {
                            AnyPlayerNearby = true;
                            break;
                        }
                    }
                    if (!AnyPlayerNearby && (NearestCompanionNPC == null || Main.rand.NextBool(2)))
                    {
                        NearestCompanionNPC = c;
                    }
                }
            }
            if (NearestCompanionNPC != null)
            {
                NPC.Bottom = NearestCompanionNPC.Bottom;
            }
            else
            {
                NPC NearestAllyNpc = null;
                for(int i = 0; i < 200; i++)
                {
                    if (i != NPC.whoAmI && Main.npc[i].active && Main.npc[i].townNPC && Main.npc[i].type != Terraria.ID.NPCID.OldMan && (NearestAllyNpc == null || Main.rand.NextBool(2)))
                    {

                        NearestAllyNpc = Main.npc[i];
                    }
                }
                if (NearestAllyNpc != null)
                {
                    NPC.Bottom = NearestAllyNpc.Bottom;
                }
            }
            base.AI();
        }
    }
}