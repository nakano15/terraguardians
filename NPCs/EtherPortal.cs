using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;

namespace terraguardians.NPCs
{
    public class EtherPortal : ModNPC
    {
        public override string Texture => "terraguardians/NPCs/EtherPortal";
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        private byte WaveSpawnCount = 0;
        private int WaveDelay = -1;

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            NPC.width = 192;
            NPC.height = 160;
            NPC.dontTakeDamage = NPC.dontTakeDamageFromHostiles = true;
            NPC.damage = 0;
            NPC.lifeMax = 150;
            NPC.aiStyle = -1;
            NPC.townNPC = false;
            NPC.direction = 0;
        }

        public override void AI()
        {
            TryPlacingOnGround();
            if (!NPC.active)
                return;
            UpdatePortalBehavior();
        }

        private void UpdatePortalBehavior()
        {
            if (WaveDelay > 0)
            {
                WaveDelay--;
            }
            else
            {
                if (WaveDelay == -1)
                {
                    WaveDelay = 300;
                }
                else if (WaveSpawnCount <= 0)
                {
                    WaveDelay = Main.rand.Next(7, 16) * 60;
                    WaveSpawnCount = (byte)Main.rand.Next(3, 7);
                }
                else
                {
                    WaveSpawnCount--;
                    WaveDelay = Main.rand.Next(3, 6) * 30;
                    NPC.life--;
                    NPC.NewNPC(new Terraria.DataStructures.EntitySource_BossSpawn(NPC), (int)NPC.Center.X, (int)NPC.Bottom.Y, 68);
                }
            }
        }

        private void TryPlacingOnGround()
        {
            const float DivisionBy16 = 1f / 16;
            int TileX = (int)(NPC.Center.X * DivisionBy16), 
                TileY = (int)(NPC.Bottom.Y * DivisionBy16);
            if (WorldGen.InWorld(TileX, TileY))
            {
                Tile tile = Main.tile[TileX, TileY];
                if (!tile.HasTile || tile.IsActuated || !Main.tileSolid[tile.TileType])
                {
                    NPC.position.Y += 16;
                }
            }
            else
            {
                NPC.active = false;
            }
        }

        public override void FindFrame(int frameHeight)
        {
            NPC.frameCounter++;
            const int MaxFrameTime = 8;
            if (NPC.frameCounter >= MaxFrameTime * 3)
            {
                NPC.frameCounter -= MaxFrameTime * 3;
            }
            int FrameIndex = (int)(NPC.frameCounter * (1f / MaxFrameTime));
            NPC.frame.X = 196 * FrameIndex + 2;
            NPC.frame.Y = 2;
            NPC.frame.Width = 192;
            NPC.frame.Height = 160;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            spriteBatch.Draw(TextureAssets.Npc[NPC.type].Value, NPC.Bottom - Main.screenPosition, NPC.frame, Color.White, 0, new Vector2(196 / 2, 158), 1f, SpriteEffects.None, 0);
            return false;
        }
    }
}