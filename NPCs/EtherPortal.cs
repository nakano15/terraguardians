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
        private static float ParticlesIntensity = 0;
        private byte Spawns = 10;

        public override void SetStaticDefaults()
        {
            
        }

        public override void SetDefaults()
        {
            NPC.width = 192;
            NPC.height = 160;
            NPC.dontTakeDamage = NPC.dontTakeDamageFromHostiles = true;
            NPC.damage = 0;
            NPC.lifeMax = 30;
            NPC.aiStyle = -1;
            NPC.townNPC = false;
            NPC.direction = 0;
            NPC.behindTiles = true;
            NPC.ShowNameOnHover = false;
            NPC.scale = 0;
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
            switch((int)NPC.ai[0])
            {
                case 0:
                    PortalSpawnEffect();
                    break;
                case 1:
                    WavesBehavior();
                    break;
                case 2:
                    PortalDespawnEffect();
                    break;
            }
            PortalEffects();
        }

        private void PortalSpawnEffect()
        {
            const float SpawnAnimationDuration = 10 * 60;
            const float PortalEmergeStartTime = 9f * 60;
            const float DivisionBySpawnDuration = 1f / SpawnAnimationDuration;
            const float DivisionByEmergeTime = 1f / (SpawnAnimationDuration - PortalEmergeStartTime);
            if(NPC.ai[1] >= SpawnAnimationDuration)
            {
                NPC.ai[0] = 1;
                NPC.ai[1] = 0;
                NPC.scale = 1;
                ParticlesIntensity = 1;
            }
            else
            {
                float Percentage = NPC.ai[1] * DivisionBySpawnDuration;
                NPC.ai[1]++;
                ParticlesIntensity = Percentage * Percentage;
                if (NPC.ai[1] >= PortalEmergeStartTime)
                {
                    Percentage = (NPC.ai[1] - PortalEmergeStartTime) * DivisionByEmergeTime;
                    NPC.scale = Percentage * Percentage * Percentage;
                }
            }
        }

        private void PortalDespawnEffect()
        {
            const float MaxTime = 60;
            const float DivisionByMaxTime = 1f / MaxTime;
            if (NPC.ai[1] >= MaxTime)
            {
                NPC.active = false;
            }
            else
            {
                float Percentage = NPC.ai[1] * DivisionByMaxTime;
                ParticlesIntensity = 1f - Percentage * Percentage;
                NPC.scale = ParticlesIntensity;
                NPC.ai[1] += 1;
            }
        }

        private void PortalEffects()
        {
            const float DivisionBy16 = 1f / 16;
            if (NPC.scale > 0)
                Lighting.AddLight(NPC.Bottom - Vector2.UnitY * 80 * NPC.scale, new Vector3(25f / 255, 255f / 255, 236f / 255) * (1.1f + MathF.Cos(Rotation * (MathF.PI * 2)) * 0.1f) * NPC.scale);
            if (Main.rand.NextFloat() < ParticlesIntensity * 0.25f)
            {
                Vector2 SpawnPosition = NPC.Bottom - Vector2.UnitY * 80;
                float Radius = Main.rand.NextFloat() * (MathF.PI * 2);
                float Range = 0.1f + Main.rand.NextFloat() * 0.8f;
                float Distance = Range * 80;
                Vector2 SpawnRegion = new Vector2(MathF.Sin(Radius), MathF.Cos(Radius)) * Distance;
                SpawnPosition += SpawnRegion;
                Tile tile = Main.tile[(int)(SpawnPosition.X * DivisionBy16), (int)(SpawnPosition.Y * DivisionBy16)];
                if (tile == null || tile.HasTile && Main.tileSolid[tile.TileType])
                    return;
                float Speed = Range * 3;
                SpawnRegion.Normalize();
                int dust = Dust.NewDust(SpawnPosition, 1, 1, Terraria.ID.DustID.Cloud, MathF.Cos(SpawnRegion.Y) * Speed, MathF.Sin(SpawnRegion.X) * Speed, Scale: 0.8f);
                Main.dust[dust].noGravity = true;
                Main.dust[dust].fadeIn = 0.3f;
            }
        }

        private void WavesBehavior()
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
                    Spawns--;
                    int MobID = 68;
                    NPC.NewNPC(new Terraria.DataStructures.EntitySource_BossSpawn(NPC), (int)NPC.Center.X, (int)NPC.Bottom.Y - 16, MobID);
                }
            }
            if (Spawns <= 0)
            {
                NPC.ai[0] = 2;
                NPC.ai[1] = 0;
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