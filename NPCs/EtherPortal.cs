using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;

namespace terraguardians.NPCs
{
    public class EtherPortal : ModNPC
    {
        public override string Texture => "terraguardians/NPCs/EtherPortal";
        protected override bool CloneNewInstances => false;
        public override bool IsCloneable => false;

        private byte WaveSpawnCount = 0;
        private int WaveDelay = -1;
        private Rectangle PortalBackgroundFrame = new Rectangle();
        private int Time = 0;
        private float Rotation = 0;

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
            NPC.behindTiles = true;
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
                    NPC.NewNPC(new Terraria.DataStructures.EntitySource_BossSpawn(NPC), (int)NPC.Center.X, (int)NPC.Bottom.Y, 1);
                }
            }
            Lighting.AddLight(NPC.Center, new Vector3(25f / 255, 255f / 255, 236f / 255) * (1.1f + MathF.Cos(Rotation * (MathF.PI * 2)) * 0.1f));
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
            NPC.frame.Height = 192;
            PortalBackgroundFrame.X = 196 * (4) + 2;
            PortalBackgroundFrame.Y = 2;
            PortalBackgroundFrame.Width = 192;
            PortalBackgroundFrame.Height = 192;
            Time ++;
            Rotation += 0.015f;
            if (Rotation > 360) Rotation -= 360;
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 Position = NPC.Bottom - Main.screenPosition;
            Vector2 Origin = new Vector2(196 / 2, 158);
            float PortalInsideFrame = (float)MathF.Sin(Time * 0.02f) * 0.03f + 1.04f;
            spriteBatch.Draw(texture, Position, PortalBackgroundFrame, Color.White, 0, Origin, NPC.scale * PortalInsideFrame, SpriteEffects.None, 0);
            spriteBatch.Draw(texture, Position, NPC.frame, Color.White, 0, Origin, NPC.scale, SpriteEffects.None, 0);
            Vector2 SwirlPosition = Position - Vector2.UnitY * 72 * NPC.scale;
            Origin.Y -= 60;
            //Origin.X -= 4;
            spriteBatch.Draw(texture, SwirlPosition, new Rectangle(196 * 3, 0, 192, 192), Color.White, Rotation * (MathF.PI * 2), Origin, NPC.scale, SpriteEffects.None, 0);
            return false;
        }
    }
}