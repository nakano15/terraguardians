using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameContent;
using Terraria.ID;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader.IO;
using Microsoft.Xna.Framework.Graphics;

namespace terraguardians
{
    public class NpcMod : GlobalNPC
    {
        private const int PlaceCatOnKingSlimeValue = -50;
        private static int TrappedCatKingSlime = -1;

        public override void SetDefaults(NPC npc)
        {
            switch(npc.type)
            {
                case NPCID.KingSlime:
                    if (!WorldMod.HasMetCompanion(CompanionDB.Sardine) && !MainMod.HasCompanionInWorld(CompanionDB.Sardine) && Main.rand.NextFloat() < 0.4f)
                    {
                        TrappedCatKingSlime = PlaceCatOnKingSlimeValue;
                    }
                    break;
            }
        }

        public override bool PreAI(NPC npc)
        {
            if (npc.type == NPCID.KingSlime && TrappedCatKingSlime == PlaceCatOnKingSlimeValue)
            {
                TrappedCatKingSlime = npc.whoAmI;
            }
            else if (TrappedCatKingSlime == npc.whoAmI && npc.type != NPCID.KingSlime)
            {
                TrappedCatKingSlime = -1;
            }
            return base.PreAI(npc);
        }

        public override void EditSpawnPool(IDictionary<int, float> pool, NPCSpawnInfo spawnInfo)
        {
            
        }

        public override bool PreDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(npc.whoAmI == TrappedCatKingSlime)
            {
                TextureAssets.Ninja = MainMod.TrappedCatTexture;
            }
            return base.PreDraw(npc, spriteBatch, screenPos, drawColor);
        }

        public override void PostDraw(NPC npc, SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            if(npc.whoAmI == TrappedCatKingSlime)
            {
                TextureAssets.Ninja = MainMod.NinjaTextureBackup;
            }
        }

        public override void OnKill(NPC npc)
        {
            if (npc.whoAmI == TrappedCatKingSlime)
            {
                TrappedCatKingSlime = -1;
                if (!MainMod.HasCompanionInWorld(CompanionDB.Sardine))
                {
                    Companion Sardine = WorldMod.SpawnCompanionNPC(npc.Center, CompanionDB.Sardine);
                    if (Sardine != null)
                        Sardine.AddBuff(BuffID.Slimed, 10 * 60);
                }
            }
        }
    }
}