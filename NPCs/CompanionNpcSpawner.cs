using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionNpcSpawner : ModNPC
    {
        public override string Texture => "terraguardians/NPCs/CompanionNpcSpawner";
        public virtual CompanionID ToSpawnID { get { return new CompanionID(); } }

        public override void AI()
        {
            Companion c = WorldMod.SpawnCompanionNPC(ToSpawnID);
            if (c != null)
            {
                Main.NewText(c.name + " spawned " + (c.Center.X < MainMod.GetLocalPlayer.Center.X ? "west" : "east") + " of you.");
            }
            NPC.active = false;
        }
    }
}