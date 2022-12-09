using Terraria;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionNpcSpawner : ModNPC
    {
        public override string Texture => "terraguardians/NPCs/CompanionNpcSpawner";
        public virtual CompanionID ToSpawnID { get { return new CompanionID(); } }

        public override void SetDefaults()
        {
            NPC.width = 40;
            NPC.height = 56;
            NPC.damage = 0;
            NPC.defense = 0;
            NPC.lifeMax = 500;
            NPC.HitSound = Terraria.ID.SoundID.NPCHit1;
            NPC.DeathSound = Terraria.ID.SoundID.NPCDeath2;
            NPC.knockBackResist = 0.33f;
            NPC.aiStyle = -1;
            NPC.dontTakeDamage = NPC.dontTakeDamageFromHostiles = true;
            NPC.townNPC = false;
            NPC.friendly = true;
            NPC.direction = Main.rand.NextDouble() < 0.5 ? -1 : 1;
        }

        public override void AI()
        {
            if(!WorldMod.HasCompanionNPCSpawned(ToSpawnID))
            {
                Companion c = WorldMod.SpawnCompanionNPC(NPC.Bottom, ToSpawnID);
                if (c != null)
                {
                    c.direction = NPC.direction;
                    //Main.NewText(c.name + " spawned " + (c.Center.X < MainMod.GetLocalPlayer.Center.X ? "west" : "east") + " of you.");
                }
            }
            NPC.active = false;
        }
    }
}