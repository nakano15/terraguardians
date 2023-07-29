using Terraria;
using Terraria.GameContent.Bestiary;
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

        public bool TargetIsPlayer(Player player)
        {
            return PlayerMod.IsPlayerCharacter(player);
        }

        public bool CanSpawnCompanionNpc(bool CheckIfMet = true)
        {
            return MainMod.GetCompanionBase(ToSpawnID).CanSpawnNpc() && (!CheckIfMet || !WorldMod.HasMetCompanion(ToSpawnID)) && !WorldMod.HasCompanionNPCSpawned(ToSpawnID) && (!MainMod.DisableModCompanions || ToSpawnID.ModID != MainMod.GetModName);
        }

        public override void AI()
        {
            if(!WorldMod.HasCompanionNPCSpawned(ToSpawnID))
            {
                Companion c = WorldMod.SpawnCompanionNPC(NPC.Bottom, ToSpawnID);
                if (c != null)
                {
                    c.direction = NPC.direction;
                }
            }
            NPC.active = false;
        }
        
        public bool IsDecentSpawnCondition(NPCSpawnInfo spawninfo)
        {
            return Main.tile[spawninfo.SpawnTileX, spawninfo.SpawnTileY].WallType == 0 || Lighting.Brightness(spawninfo.SpawnTileX, spawninfo.SpawnTileY) >= 0.3f;
        }
    }
}