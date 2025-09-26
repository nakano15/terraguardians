using Terraria;
using Terraria.GameContent.Bestiary;
using Terraria.ModLoader;

namespace terraguardians
{
    public class CompanionNpcSpawner : ModNPC
    {
        public override string Texture => "terraguardians/NPCs/CompanionNpcSpawner";
        public virtual CompanionID ToSpawnID { get { return new CompanionID(); } }
        public virtual byte GenericFamiliarFaceChancePercent => 50;
        public virtual byte MaxGenericSpawns => 10;

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
            CompanionBase Base = MainMod.GetCompanionBase(ToSpawnID);
            return Base.CanSpawnNpc() && (!Base.IsGeneric || MainMod.EnableGenericCompanions) && 
                ((Base.IsGeneric && WorldMod.CompanionNPCs.Count < WorldMod.MaxCompanionNpcsInWorld) || 
                ((Base.IsGeneric && WorldMod.CountCompanionNPCSpawned(ToSpawnID.ID, ToSpawnID.ModID, false) < MaxGenericSpawns) || (!CheckIfMet || !WorldMod.HasMetCompanion(ToSpawnID)) && !WorldMod.HasCompanionNPCSpawned(ToSpawnID))) && 
                (!MainMod.DisableModCompanions || ToSpawnID.ModID != MainMod.GetModName);
        }

        public bool HasAnyUnrecruitedNpcSpawned()
        {
            foreach (Companion c in WorldMod.CompanionNPCs)
            {
                if (!c.HasBeenMet) return true;
            }
            return false;
        }

        public override void AI()
        {
            bool IsGeneric = MainMod.GetCompanionBase(ToSpawnID).IsGeneric;
            if (IsGeneric || !WorldMod.HasCompanionNPCSpawned(ToSpawnID))
            {
                Companion c;
                ushort GenericID = 0;
                if (IsGeneric)
                {
                    if (Main.rand.Next(0, 100) < GenericFamiliarFaceChancePercent)
                    {
                        GenericID = GenericCompanionInfos.GetRandomGenericOfType(ToSpawnID.ID, ToSpawnID.ModID);
                    }
                    if (GenericID == 0 && !GenericCompanionInfos.CanCreateNewGenericEntry())
                    {
                        NPC.active = false;
                        return;
                    }
                }
                c = WorldMod.SpawnCompanionNPC(NPC.Bottom, GenericID, ToSpawnID);
                if (c != null)
                {
                    c.direction = NPC.direction;
                }
                PostSpawnCompanion(c);
            }
            NPC.active = false;
        }

        protected virtual void PostSpawnCompanion(Companion companion)
        {

        }
        
        public bool IsDecentSpawnCondition(NPCSpawnInfo spawninfo)
        {
            return Main.tile[spawninfo.SpawnTileX, spawninfo.SpawnTileY].WallType == 0 || Lighting.Brightness(spawninfo.SpawnTileX, spawninfo.SpawnTileY) >= 0.3f;
        }
    }
}