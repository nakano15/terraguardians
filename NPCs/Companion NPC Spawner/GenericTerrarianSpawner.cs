using System;
using Terraria;
using Terraria.ModLoader;

namespace terraguardians.NPCs.CompanionNPCSpawner
{
    public class GenericTerrarianSpawner : CompanionNpcSpawner
    {
        public override CompanionID ToSpawnID => new CompanionID(CompanionDB.GenericTerrarian);

        public override float SpawnChance(NPC.Spawner spawner)
        {
            return TargetIsPlayer(spawner.Player) && MathF.Abs(spawner.Player.velocity.X) >= spawner.Player.moveSpeed && !spawner.waterTile && !(spawner.Player.ZoneCorrupt || spawner.Player.ZoneCrimson) && CanSpawnCompanionNpc(false) ? 1f / 125 : 0f;
        }

        /*public override void AI() //Better avoid companion copies from cluttering people companions list for now.
        {
            NPC.TargetClosest(false);
            Terraria.NPC.SpawnOnPlayer(NPC.target, 68);
            NPC.active = false;
        }*/
    }
}