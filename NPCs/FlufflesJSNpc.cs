using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.GameContent;
using Terraria.ModLoader;
using System;
using System.Collections.Generic;
using Terraria.Localization;

namespace terraguardians.NPCs
{
    public class FlufflesJSNpc : ModNPC
    {
        public override string Texture => "terraguardians/NPCs/CompanionNpcSpawner";
        public CompanionBase FlufflesBase;

        public override void AI()
        {
            if (NPC.ai[0] == 0)
            {
                FlufflesBase = MainMod.GetCompanionBase(CompanionDB.Fluffles);
                NPC.ai[0] = 1;
                NPC.ai[2] = Main.rand.Next(40, 81);
                NPC.TargetClosest(false);
                if (NPC.target > -1)
                {
                    Vector2 CenteredPosition = Main.player[NPC.target].Bottom;
                    float DefaultOrientation = Main.rand.NextFloat() < 0.5f ? 1f : -1f;
                    for (int i = 1; i >= -1; i-= 2)
                    {
                        int Distance = Main.rand.Next(3, 7);
                        Vector2 SpawnPosition = CenteredPosition + new Vector2(Distance * 16 * i, 0);
                        SpawnPosition.X -= NPC.width * .5f;
                        SpawnPosition.Y -= NPC.height;
                        int TileStartX = (int)(SpawnPosition.X * TerraGuardian.DivisionBy16),
                            TileStartY = (int)(SpawnPosition.Y * TerraGuardian.DivisionBy16);
                        bool AnyTileIntersecting = false, AnyTileUnder = false;
                        for (int tries = 0; tries < 10; tries++)
                        {
                            AnyTileIntersecting = false;
                            AnyTileUnder = false;
                            for (int x = 0; x < 2; x++)
                            {
                                for (int y = 0; y < 3; y++)
                                {
                                    Tile tile = Main.tile[TileStartX + x, TileStartY + y];
                                    if(tile.HasTile && !tile.IsActuated && Main.tileSolid[tile.TileType])
                                    {
                                        AnyTileIntersecting = true;
                                        break;
                                    }
                                }
                                Tile undertile = Main.tile[TileStartX + x, TileStartY + 3];
                                if (undertile.HasTile && !undertile.IsActuated && Main.tileSolid[undertile.TileType])
                                {
                                    AnyTileUnder = true;
                                }
                            }
                            if (AnyTileIntersecting)
                            {
                                SpawnPosition.Y -= 16;
                                TileStartY--;
                            }
                            else if (!AnyTileUnder)
                            {
                                SpawnPosition.Y += 16;
                                TileStartY++;
                            }
                            else if (!AnyTileIntersecting && AnyTileUnder)
                            {
                                break;
                            }
                        }
                        if (!AnyTileIntersecting && AnyTileUnder)
                        {
                            NPC.position = SpawnPosition;
                            if(NPC.Center.X < Main.player[NPC.target].Center.X)
                            {
                                NPC.direction = 1;
                            }
                            else
                            {
                                NPC.direction = -1;
                            }
                            SoundEngine.PlaySound(SoundID.NPCHit54, NPC.position);
                            return;
                        }
                    }
                }
            }
            else
            {
                if (NPC.ai[1] < NPC.ai[2])
                {
                    NPC.ai[1]++;
                }
                else
                {
                    NPC.active = false;
                }
            }
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor)
        {
            
            return false;
        }
    }
}